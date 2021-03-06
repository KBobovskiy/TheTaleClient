using DataBaseContext;
using DataBaseContext.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            //_dbContext.Database.EnsureDeleted();
            //_dbContext.Database.EnsureCreated();
            _dbContext.Database.Migrate();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Working...");

                await RunWorkerAsync();

                _logger.LogInformation("Sleeping...");
                await Task.Delay(6000, stoppingToken);
            }
        }

        private async Task<bool> RunWorkerAsync()
        {
            IEfCoreDao EfCoreDao = new SQLiteEfDao(_dbContext);

            var apiClient = new ApiClient();

            _logger.LogInformation("Login into the game");

            var cookieWithCredentials = await apiClient.LoginAsync(_login, _password);
            if (cookieWithCredentials != null)
            {
                var cards = await apiClient.GetCardsAsync(cookieWithCredentials);

                var gameInfo = await apiClient.GetGameInfoAsync(cookieWithCredentials);
                if (gameInfo != null)
                {
                    var turnDto = MapApiResponseIntoDto.MapGameInfoIntoTurnDto(gameInfo);
                    EfCoreDao.SaveTurnAsync(turnDto);

                    var heroInfoDto = MapApiResponseIntoDto.MapGameInfoIntoHeroInfoDto(gameInfo);
                    EfCoreDao.SaveHeroInfoAsync(heroInfoDto);

                    var heroState = GetHeroState(heroInfoDto);

                    if (heroState == HeroStates.Idle)
                    {
                        var newWay = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.NewWay);
                        if (newWay != null)
                        {
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
                        var regenerationCard = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.Regeneration);
                        var result = await apiClient.UseCardsAsync(cookieWithCredentials, regenerationCard);
                        if (result)
                        {
                            await Task.Delay(DelayAfterCardUsingInMilliSeconds);
                            return true;
                        }
                    }

                    if (heroState == HeroStates.LowHealth)
                    {
                        var regenerationCard = cards?.data?.cards?.FirstOrDefault(x => x.name == CardNames.Regeneration);
                        var result = await apiClient.UseCardsAsync(cookieWithCredentials, regenerationCard);
                        if (result)
                        {
                            await Task.Delay(DelayAfterCardUsingInMilliSeconds);
                            return true;
                        }
                    }
                }
            }

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
                return HeroStates.Idle;
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