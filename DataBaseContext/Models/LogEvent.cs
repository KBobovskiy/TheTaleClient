using System;
using System.ComponentModel.DataAnnotations;

namespace DataBaseContext.Models
{
    public class LogEvent
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// Game turn number
        /// </summary>
        public int TurnNumber { get; set; }

        /// <summary>
        /// Log event type
        /// </summary>
        [MaxLength(20)]
        public string Type { get; set; }

        /// <summary>
        /// Log message
        /// </summary>
        public string Description { get; set; }
    }
}