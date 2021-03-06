namespace TheTaleApiClient.Models.Responses
{
    public class Data
    {
        public int number { get; set; }
    }

    public class NewMessagesNumberResponse
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
}