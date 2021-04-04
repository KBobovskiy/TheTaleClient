using Resources.Enums;
using System;

namespace DataBaseContext.DTO
{
    public class HeroInfoDto
    {
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public int TurnNumber { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public int RaceId { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Level { get; set; }
        public int Money { get; set; }
        public bool Alive { get; set; }
        public int Experience { get; set; }
        public int ExperienceToLevel { get; set; }
        public int MaxBagSize { get; set; }
        public int PowerPhysical { get; set; }
        public int PowerMagic { get; set; }
        public double MoveSpeed { get; set; }
        public int LootItemsCount { get; set; }
        public double Initiative { get; set; }

        public ActionTypes ActionType { get; set; }
        public string ActionDescription { get; set; }
        public double ActionPercents { get; set; }
        public bool ActionIsBoss { get; set; }

        public int Energy { get; set; }
        public string InPvpQueue { get; set; }
        public string Mode { get; set; }
        public string Enemy { get; set; }
        public double Honor { get; set; }
        public double Peacefulness { get; set; }

        public Quest[] Quests { get; set; }

        public static int[] GetExcludedFromLogActionTypes()
        {
            return new int[]
            {
                (int)ActionTypes.FightingWithMob,
                (int)ActionTypes.CureHimself,
                (int)ActionTypes.CuringHisCompanion,
                (int)ActionTypes.HeroInTown,
                (int)ActionTypes.ChangeEquipment,
                (int)ActionTypes.Trading,
                (int)ActionTypes.TravelToNextPoint,
            };
        }
    }

    public class Quest
    {
        public string type { get; set; }
        public string uid { get; set; }
        public string name { get; set; }
        public string action { get; set; }
        public string choice { get; set; }
        public int experience { get; set; }
        public int power { get; set; }
    }
}