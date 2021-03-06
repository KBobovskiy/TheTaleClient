using Resources.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheTaleApiClient.Models.Responses
{
    public class Turn
    {
        public int number { get; set; }
        public string verbose_date { get; set; }
        public string verbose_time { get; set; }
    }

    public class Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double dx { get; set; }
        public double dy { get; set; }
    }

    public class _187709
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<object> integrity { get; set; }
        public object rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public object preference_rating { get; set; }
        public object power { get; set; }
    }

    public class _187710
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _187711
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<object> integrity { get; set; }
        public object rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public object preference_rating { get; set; }
        public object power { get; set; }
    }

    public class Bag
    {
        public _187709 _187709 { get; set; }
        public _187710 _187710 { get; set; }
        public _187711 _187711 { get; set; }
    }

    public class _0
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _1
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _2
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _3
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public double preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _4
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public double preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _5
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _6
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _7
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public double preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _8
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public double preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _9
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public int preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class _10
    {
        public int type { get; set; }
        public int id { get; set; }
        public bool equipped { get; set; }
        public string name { get; set; }
        public List<int> integrity { get; set; }
        public int rarity { get; set; }
        public int effect { get; set; }
        public int special_effect { get; set; }
        public double preference_rating { get; set; }
        public List<int> power { get; set; }
    }

    public class Equipment
    {
        public _0 _0 { get; set; }
        public _1 _1 { get; set; }
        public _2 _2 { get; set; }
        public _3 _3 { get; set; }
        public _4 _4 { get; set; }
        public _5 _5 { get; set; }
        public _6 _6 { get; set; }
        public _7 _7 { get; set; }
        public _8 _8 { get; set; }
        public _9 _9 { get; set; }
        public _10 _10 { get; set; }
    }

    public class Might
    {
        public double value { get; set; }
        public double pvp_effectiveness_bonus { get; set; }
        public double politics_power { get; set; }
    }

    public class Permissions
    {
        public bool can_participate_in_pvp { get; set; }
        public bool can_repair_building { get; set; }
    }

    public class Action
    {
        public double percents { get; set; }
        public ActionTypes type { get; set; }
        public string description { get; set; }
        public string info_link { get; set; }
        public bool? is_boss { get; set; }
        public object data { get; set; }
    }

    public class Path
    {
        public List<List<int>> cells { get; set; }
    }

    public class Companion
    {
        public int type { get; set; }
        public string name { get; set; }
        public int health { get; set; }
        public int max_health { get; set; }
        public int experience { get; set; }
        public int experience_to_level { get; set; }
        public int coherence { get; set; }
        public int real_coherence { get; set; }
    }

    public class HeroBase
    {
        public string name { get; set; }
        public int level { get; set; }
        public int destiny_points { get; set; }
        public int health { get; set; }
        public int max_health { get; set; }
        public int experience { get; set; }
        public int experience_to_level { get; set; }
        public int gender { get; set; }
        public int race { get; set; }
        public int money { get; set; }
        public bool alive { get; set; }
    }

    public class Secondary
    {
        public List<int> power { get; set; }
        public double move_speed { get; set; }
        public double initiative { get; set; }
        public int max_bag_size { get; set; }
        public int loot_items_count { get; set; }
    }

    public class Honor
    {
        public string verbose { get; set; }
        public double raw { get; set; }
    }

    public class Peacefulness
    {
        public string verbose { get; set; }
        public double raw { get; set; }
    }

    public class Habits
    {
        public Honor honor { get; set; }
        public Peacefulness peacefulness { get; set; }
    }

    public class Line
    {
        public string type { get; set; }
        public string uid { get; set; }
        public string name { get; set; }
        public string action { get; set; }
        public object choice { get; set; }
        public List<object> choice_alternatives { get; set; }
        public int experience { get; set; }
        public int power { get; set; }
        public List<List<object>> actors { get; set; }
    }

    public class Quests
    {
        public List<Line> line { get; set; }
    }

    public class Hero
    {
        public int id { get; set; }
        public object patch_turn { get; set; }
        public int actual_on_turn { get; set; }
        public long ui_caching_started_at { get; set; }
        public int diary { get; set; }
        public List<List<object>> messages { get; set; }
        public Position position { get; set; }
        public Bag bag { get; set; }
        public Equipment equipment { get; set; }
        public Might might { get; set; }
        public Permissions permissions { get; set; }
        public Action action { get; set; }
        public Path path { get; set; }
        public Companion companion { get; set; }
        public HeroBase @base { get; set; }
        public Secondary secondary { get; set; }
        public Habits habits { get; set; }
        public Quests quests { get; set; }
        public int sprite { get; set; }
    }

    public class Account
    {
        public int id { get; set; }
        public long last_visit { get; set; }
        public bool is_own { get; set; }
        public bool is_old { get; set; }
        public Hero hero { get; set; }
        public int energy { get; set; }
    }

    public class GameInfoData
    {
        public string mode { get; set; }
        public Turn turn { get; set; }
        public int game_state { get; set; }
        public string map_version { get; set; }
        public Account account { get; set; }
        public string enemy { get; set; }
    }

    public class GameInfoResponse
    {
        public string status { get; set; }
        public GameInfoData data { get; set; }
    }
}