using DataBaseContext.DTO;
using System;
using TheTaleApiClient.Models.Responses;

namespace TheTaleWorker.Mappers
{
    internal static class MapApiResponseIntoDto
    {
        internal static TurnDto MapGameInfoIntoTurnDto(GameInfoResponse gameInfo)
        {
            if (gameInfo?.data?.turn != null)
            {
                var result = new TurnDto()
                {
                    Number = gameInfo.data.turn.number,
                    VerboseDate = gameInfo.data.turn.verbose_date,
                    VerboseTime = gameInfo.data.turn.verbose_time
                };

                return result;
            }

            return (TurnDto)null;
        }

        internal static HeroInfoDto MapGameInfoIntoHeroInfoDto(GameInfoResponse gameInfo)
        {
            if (gameInfo != null)
            {
                var result = new HeroInfoDto();
                result.AccountId = gameInfo.data.account.id;
                result.Date = DateTime.UtcNow;
                result.TurnNumber = gameInfo.data.turn.number;
                result.Name = gameInfo.data.account.hero.@base.name;
                result.GenderId = gameInfo.data.account.hero.@base.gender;
                result.RaceId = gameInfo.data.account.hero.@base.race;
                result.Health = gameInfo.data.account.hero.@base.health;
                result.MaxHealth = gameInfo.data.account.hero.@base.max_health;
                result.Level = gameInfo.data.account.hero.@base.level;
                result.Money = gameInfo.data.account.hero.@base.money;
                result.Alive = gameInfo.data.account.hero.@base.alive;
                result.Experience = gameInfo.data.account.hero.@base.experience;
                result.ExperienceToLevel = gameInfo.data.account.hero.@base.experience_to_level;
                result.MaxBagSize = gameInfo.data.account.hero.secondary.max_bag_size;
                result.PowerPhysical = gameInfo.data.account.hero.secondary.power[0];
                result.PowerMagic = gameInfo.data.account.hero.secondary.power[1];
                result.MoveSpeed = gameInfo.data.account.hero.secondary.move_speed;
                result.LootItemsCount = gameInfo.data.account.hero.secondary.loot_items_count;
                result.Initiative = gameInfo.data.account.hero.secondary.initiative;
                result.Energy = gameInfo.data.account.energy;
                result.Mode = gameInfo.data.mode;
                result.Enemy = gameInfo.data.enemy;
                result.Honor = gameInfo.data.account.hero.habits.honor.raw;
                result.Peacefulness = gameInfo.data.account.hero.habits.peacefulness.raw;

                result.ActionType = gameInfo.data.account.hero.action.type;
                result.ActionDescription = gameInfo.data.account.hero.action.description;
                result.ActionPercents = gameInfo.data.account.hero.action.percents * 100;
                result.ActionIsBoss = gameInfo.data.account.hero.action.is_boss ?? false;

                return result;
            }

            return (HeroInfoDto)null;
        }
    }
}