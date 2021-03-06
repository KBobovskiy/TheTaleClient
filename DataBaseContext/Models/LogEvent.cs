using System;
using System.ComponentModel.DataAnnotations;

namespace DataBaseContext.Models
{
    public class LogEvent
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }

        public string Description { get; set; }
    }
}