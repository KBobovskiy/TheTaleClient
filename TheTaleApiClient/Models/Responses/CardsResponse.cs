using System.Collections.Generic;

namespace TheTaleApiClient.Models.Responses
{
    public class Card
    {
        public string name { get; set; }
        public int type { get; set; }
        public string full_type { get; set; }
        public int rarity { get; set; }
        public string uid { get; set; }
        public bool in_storage { get; set; }
        public bool auction { get; set; }
    }

    public class NewCardTimer
    {
        public int id { get; set; }
        public int owner_id { get; set; }
        public int type { get; set; }
        public double speed { get; set; }
        public double border { get; set; }
        public double resources { get; set; }
        public double resources_at { get; set; }
        public double finish_at { get; set; }
    }

    public class CardsResponseData
    {
        public List<Card> cards { get; set; }
        public int new_cards { get; set; }
        public NewCardTimer new_card_timer { get; set; }
    }

    public class CardsResponse
    {
        public string status { get; set; }
        public CardsResponseData data { get; set; }
    }
}