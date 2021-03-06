using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseContext.Models
{
    internal class Account
    {
        public int Id { get; set; }

        public string AccountId { get; set; }

        public string SessionId { get; set; }

        public string CsrfToken { get; set; }

        public DateTime? LastVisit { get; set; }
    }
}