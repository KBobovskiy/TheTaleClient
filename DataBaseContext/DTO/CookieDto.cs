using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseContext.DTO
{
    public class CookieDto
    {
        public string CsrfToken { get; set; }
        public string SessionId { get; set; }

        public int AccountId { get; set; }
    }
}