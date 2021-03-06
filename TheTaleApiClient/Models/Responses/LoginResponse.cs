using System;
using System.Collections.Generic;
using System.Text;

namespace TheTaleApiClient.Models.Responses
{
    public class LoginData
    {
        public string next_url { get; set; }
        public int account_id { get; set; }
        public string account_name { get; set; }
        public long session_expire_at { get; set; }
    }

    public class LoginResponse
    {
        public string status { get; set; }
        public LoginData data { get; set; }
    }
}