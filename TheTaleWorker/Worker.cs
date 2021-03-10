using DataBaseContext;
using DataBaseContext.DTO;
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
        private string _login;
        private string _password;
        private string _connectionString;
        private const int DelayAfterCardUsingInMilliSeconds = 10000;
        private const int LowHealthValue = 500;

        private SqliteContext _dbContext;

        public Worker(ILogger<Worker> logger, WorkerConfiguration workerConfiguration, SqliteContext dbContext)
        {
            if (workerConfiguration == null)
            {
                throw new ArgumentNullException($"{nameof(workerConfiguration)} is null!");
            }

            _login = workerConfiguration.Login;
            _password = workerConfiguration.Password;
            _connectionString = workerConfiguration.ConnectionString;
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //_dbContext.Database.EnsureCreated();
            //_dbContext.Database.Migrate();

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
                    _nlog.Error(ex, "Error: on call RunWorkerAsync");
                    throw;
                }

                _logger.LogInformation("Sleeping...");
                _nlog.Info("Sleeping...");
                await Task.Delay(20000, stoppingToken);
            }
        }

        private async Task<bool> RunWorkerAsync()
        {
            IEfCoreDao EfCoreDao = new SQLiteEfDao(_dbContext);

            var apiClient = new ApiClient();

            int turnNumber = 0;
            _logger.LogInformation("Login into the game");

            _nlog.Info(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " Trying to login into game");
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

                    if (heroState == HeroStates.Idle)
                    {
                        var newWay = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.NewWay);
                        if (newWay != null)
                        {
                            EfCoreDao.SaveLogEventAsync(
                                new LogEventDto()
                                {
                                    Type = "Game",
                                    TurnNumber = turnNumber,
                                    Description = $"Try to use card: {newWay.name} with uid={newWay.uid}."
                                });
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
                            EfCoreDao.SaveLogEventAsync(
                                new LogEventDto()
                                {
                                    Type = "Game",
                                    TurnNumber = turnNumber,
                                    Description = $"Try to use card: {handOfDeathCard.name} with uid={handOfDeathCard.uid}."
                                });
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
                                EfCoreDao.SaveLogEventAsync(
                                    new LogEventDto()
                                    {
                                        Type = "Game",
                                        TurnNumber = turnNumber,
                                        Description = $"Try to use card: {regenerationCard.name} with uid={regenerationCard.uid}."
                                    });
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
            if (heroInfoDto.ActionType == ActionTypes.Idle)
            {
                if (heroInfoDto.Quests.FirstOrDefault(q=>q.type == QuestTypes.NoQuest) != null)
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
    }
}