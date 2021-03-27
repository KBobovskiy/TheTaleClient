using DataBaseContext;
using DataBaseContext.DTO;
using EmailSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using Resources.Enums;
using Resources.StringResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheTaleApiClient;
using TheTaleApiClient.Models;
using TheTaleApiClient.Models.Responses;
using TheTaleWorker.Mappers;

namespace TheTaleWorker
{
    public class Worker : BackgroundService
    {
        private readonly Logger _nlog = LogManager.GetCurrentClassLogger();
        private readonly ILogger<Worker> _logger;
        private readonly IEmailSender _emailSender;
        private string _emailReceiver;
        private string _login;
        private string _password;
        private string _connectionString;
        private const int DelayAfterCardUsingInMilliSeconds = 10000;
        private const int LowHealthValue = 500;

        private SqliteContext _dbContext;

        public Worker(ILogger<Worker> logger, WorkerConfiguration workerConfiguration, SqliteContext dbContext, IEmailSender emailSender)
        {
            if (workerConfiguration == null)
            {
                throw new ArgumentNullException($"{nameof(workerConfiguration)} is null!");
            }

            _login = workerConfiguration.Login;
            _password = workerConfiguration.Password;
            _connectionString = workerConfiguration.ConnectionString;
            _emailReceiver = workerConfiguration.EmailTo;
            _logger = logger;
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_dbContext.Database.EnsureCreated();
            //_dbContext.Database.Migrate();

            var logEvent = new LogEventDto()
            {
                Type = "Game bot started",
                TurnNumber = 0,
                Description = $"New world begin!"
            };
            SendEmailWithLogEvent(logEvent);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Working...");
                _nlog.Info("Working...");

                try
                {
                    await RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error: on call RunWorkerAsync " + ex.Message);
                    _nlog.Error(ex, "Error: on call RunWorkerAsync");
                }

                _logger.LogInformation("Sleeping...");
                _nlog.Info("Sleeping...");
                await Task.Delay(30000, stoppingToken);
            }
        }

        private async Task<bool> RunWorkerAsync()
        {
            var apiClient = new ApiClient();

            int turnNumber = 0;

            var EfCoreDao = (IEfCoreDao)new SQLiteEfDao(_dbContext);
            var lastHeroInfos = EfCoreDao.SelectLatestHeroInfosAsync(50);
            var lastHeroInfo = GetLastestHeroInfoForLoggingActionTypeChanges(lastHeroInfos);
            var lastHeroActionDescription = lastHeroInfo.ActionDescription?.Substring(0, lastHeroInfo.ActionDescription.Length / 2);

            _logger.LogInformation($"Login into the game. Last hero turn: {lastHeroInfo.TurnNumber} action: {lastHeroInfo.ActionDescription}");
            _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Login into the game. Last hero turn: {lastHeroInfo.TurnNumber} action: {lastHeroInfo.ActionDescription}");
            EfCoreDao.SaveLogEventAsync(
                new LogEventDto()
                {
                    Type = "Game",
                    Description = "Trying to login into game"
                });
            var cookieWithCredentials = await apiClient.LoginAsync(_login, _password);
            if (cookieWithCredentials != null)
            {
                EfCoreDao.SaveLogEventAsync(
                    new LogEventDto()
                    {
                        Type = "Game",
                        Description = "Login success."
                    });
                _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Login success.");

                var cards = await apiClient.GetCardsAsync(cookieWithCredentials);

                var gameInfo = await apiClient.GetGameInfoAsync(cookieWithCredentials);
                if (gameInfo != null)
                {
                    var turnDto = MapApiResponseIntoDto.MapGameInfoIntoTurnDto(gameInfo);
                    turnNumber = turnDto.Number;
                    var heroInfoDto = MapApiResponseIntoDto.MapGameInfoIntoHeroInfoDto(gameInfo);
                    var actionPercentsLogInfo = heroInfoDto.ActionPercents > 0 ? $"{heroInfoDto.ActionPercents:N0}%" : string.Empty;
                    _logger.LogInformation($"Current hero turn: {heroInfoDto.TurnNumber} action: {actionPercentsLogInfo}");

                    EfCoreDao.SaveLogEventAsync(
                        new LogEventDto()
                        {
                            Type = "Game",
                            TurnNumber = turnNumber,
                            Description = $"Got game info: {heroInfoDto.ActionDescription} {actionPercentsLogInfo}."
                        });
                    _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Got game info: {heroInfoDto.ActionDescription} {actionPercentsLogInfo}.");

                    EfCoreDao.SaveTurnAsync(turnDto);

                    EfCoreDao.SaveHeroInfoAsync(heroInfoDto);

                    var heroState = GetHeroState(heroInfoDto);

                    EfCoreDao.SaveLogEventAsync(
                        new LogEventDto()
                        {
                            Type = "Game",
                            TurnNumber = turnNumber,
                            Description = $"Hero state: {heroState}."
                        });
                    _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Hero state: {heroState}.");

                    var excludingActionTypes = HeroInfoDto.GetExcludedFromLogActionTypes();
                    if (!excludingActionTypes.Contains((int)heroInfoDto.ActionType)
                        && ((int)lastHeroInfo.ActionPercents / 10) != ((int)heroInfoDto.ActionPercents / 10)
                        //&& !heroInfoDto.ActionDescription.StartsWith(lastHeroActionDescription)
                        )
                    {
                        var logEvent = new LogEventDto()
                        {
                            Type = "Game",
                            TurnNumber = turnNumber,
                            Description = $"Hero action changed from {lastHeroInfo.ActionDescription} to {heroInfoDto.ActionDescription} {actionPercentsLogInfo}."
                        };
                        _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + logEvent.Description);
                        SendEmailWithLogEvent(logEvent);
                    }

                    if (heroState == HeroStates.Idle)
                    {
                        var newWay = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.NewWay);
                        if (newWay != null)
                        {
                            var logEvent = new LogEventDto()
                            {
                                Type = "Game",
                                TurnNumber = turnNumber,
                                Description = $"Try to use card: {newWay.name} with uid={newWay.uid}."
                            };
                            SendEmailWithLogEvent(logEvent);
                            EfCoreDao.SaveLogEventAsync(logEvent);
                            _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Try to use card: {newWay.name} with uid={newWay.uid}.");

                            var result = await apiClient.UseCardsAsync(cookieWithCredentials, newWay);
                            if (result)
                            {
                                await Task.Delay(DelayAfterCardUsingInMilliSeconds);
                                return true;
                            }
                        }
                    }

                    if (heroState == HeroStates.LowHealthInFightWithMob)
                    {
                        var handOfDeathCard = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.HandOfDeath);
                        if (handOfDeathCard != null)
                        {
                            var logEvent = new LogEventDto()
                            {
                                Type = "Game",
                                TurnNumber = turnNumber,
                                Description = $"Try to use card: {handOfDeathCard.name} with uid={handOfDeathCard.uid}."
                            };
                            SendEmailWithLogEvent(logEvent);
                            EfCoreDao.SaveLogEventAsync(logEvent);

                            _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Try to use card: {handOfDeathCard.name} with uid={handOfDeathCard.uid}.");

                            var result = await apiClient.UseCardsAsync(cookieWithCredentials, handOfDeathCard);
                            if (result)
                            {
                                await Task.Delay(DelayAfterCardUsingInMilliSeconds);
                                return true;
                            }
                        }
                    }

                    if (heroState == HeroStates.LowHealth)
                    {
                        var regenerationCard = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.Regeneration);
                        if (regenerationCard != null)
                        {
                            var result = await apiClient.UseCardsAsync(cookieWithCredentials, regenerationCard);
                            if (result)
                            {
                                var logEvent = new LogEventDto()
                                {
                                    Type = "Game",
                                    TurnNumber = turnNumber,
                                    Description = $"Try to use card: {regenerationCard.name} with uid={regenerationCard.uid}."
                                };
                                SendEmailWithLogEvent(logEvent);
                                EfCoreDao.SaveLogEventAsync(logEvent);
                                _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Try to use card: {regenerationCard.name} with uid={regenerationCard.uid}.");

                                await Task.Delay(DelayAfterCardUsingInMilliSeconds);
                                return true;
                            }
                        }
                    }
                }
            }

            EfCoreDao.SaveLogEventAsync(
                new LogEventDto()
                {
                    Type = "Game",
                    TurnNumber = turnNumber,
                    Description = "Task complete."
                });
            _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + $" Task complete.");

            return true;
        }

        /// <summary>
        /// Get hero state to choice next action
        /// </summary>
        /// <param name="heroInfoDto"></param>
        /// <returns></returns>
        private HeroStates GetHeroState(HeroInfoDto heroInfoDto)
        {
            if (heroInfoDto.ActionType == ActionTypes.Idle && heroInfoDto.ActionPercents > 3)
            {
                if (heroInfoDto.Quests?.FirstOrDefault(q => q.type == QuestTypes.NoQuest) != null)
                {
                    return HeroStates.Idle;
                }
            }

            if (heroInfoDto.Health < LowHealthValue)
            {
                if (heroInfoDto.ActionType == ActionTypes.FightingWithMob)
                {
                    return HeroStates.LowHealthInFightWithMob;
                }

                return HeroStates.LowHealth;
            }

            return HeroStates.Undefined;
        }

        private void SendEmailWithLogEvent(LogEventDto logEvent)
        {
            var message = new EmailMessage
            {
                AddressTo = new System.Net.Mail.MailAddress(_emailReceiver),
                Subject = $"{logEvent.Type} {logEvent.TurnNumber}",
                Body = logEvent.Description,
                IsBodyInHtml = true
            };

            _emailSender.TrySendEmail(message);
        }

        private HeroInfoDto GetLastestHeroInfoForLoggingActionTypeChanges(HeroInfoDto[] lastHeroInfos)
        {
            var excludingActionTypes = HeroInfoDto.GetExcludedFromLogActionTypes();

            var heroInfoDto = lastHeroInfos
                .Where(x => !excludingActionTypes.Contains((int)x.ActionType))
                .OrderByDescending(x => x.TurnNumber)
                .FirstOrDefault();

            return heroInfoDto;
        }
    }
}