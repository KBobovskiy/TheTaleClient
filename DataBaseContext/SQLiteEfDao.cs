using DataBaseContext.DTO;
using DataBaseContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBaseContext
{
    public class SQLiteEfDao : IEfCoreDao
    {
        private SqliteContext _dbContext;

        public SQLiteEfDao(SqliteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CookieDto LoadCookie(int AccountId)
        {
            throw new NotImplementedException();
        }

        public void SaveCookie(int AccountId, CookieDto Cookie)
        {
            throw new NotImplementedException();
        }

        public async void SaveTurnAsync(TurnDto turn)
        {
            if (turn == null)
            {
                return;
            }

            var turnEntry = new Turn()
            {
                Number = turn.Number,
                VerboseDate = turn.VerboseDate,
                VerboseTime = turn.VerboseTime,
            };

            if (!_dbContext.Turns.Any(x => x.Number == turnEntry.Number))
            {
                _dbContext.Turns.Add(turnEntry);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async void SaveHeroInfoAsync(HeroInfoDto heroInfo)
        {
            if (heroInfo == null)
            {
                return;
            }

            var heroHistory = new HeroInfoHistory()
            {
                AccountId = heroInfo.AccountId,
                Date = heroInfo.Date,
                TurnNumber = heroInfo.TurnNumber,
                Name = heroInfo.Name,
                GenderId = heroInfo.GenderId,
                RaceId = heroInfo.RaceId,
                Health = heroInfo.Health,
                MaxHealth = heroInfo.MaxHealth,
                Level = heroInfo.Level,
                Money = heroInfo.Money,
                Alive = heroInfo.Alive,
                Experience = heroInfo.Experience,
                ExperienceToLevel = heroInfo.ExperienceToLevel,
                MaxBagSize = heroInfo.MaxBagSize,
                PowerPhysical = heroInfo.PowerPhysical,
                PowerMagic = heroInfo.PowerMagic,
                MoveSpeed = heroInfo.MoveSpeed,
                LootItemsCount = heroInfo.LootItemsCount,
                Initiative = heroInfo.Initiative,
                Energy = heroInfo.Energy,
                InPvpQueue = heroInfo.InPvpQueue,
                Mode = heroInfo.Mode,
                Enemy = heroInfo.Enemy,
                Honor = heroInfo.Honor,
                Peacefulness = heroInfo.Peacefulness,
                ActionType = (int)heroInfo.ActionType,
                ActionDescription = heroInfo.ActionDescription,
                ActionPercents = heroInfo.ActionPercents,
                ActionIsBoss = heroInfo.ActionIsBoss,
            };

            if (!_dbContext.HeroInfoHistory.Any(x => x.TurnNumber == heroHistory.TurnNumber))
            {
                _dbContext.HeroInfoHistory.Add(heroHistory);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async void SaveLogEventAsync(LogEventDto logEvent)
        {
            if (logEvent == null || (string.IsNullOrEmpty(logEvent.Type) && string.IsNullOrEmpty(logEvent.Description)))
            {
                return;
            }

            var logEventEntry = new LogEvent()
            {
                Date = DateTime.Now,
                Type = logEvent.Type,
                TurnNumber = logEvent.TurnNumber,
                Description = logEvent.Description
            };

            _dbContext.LogEvents.Add(logEventEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}