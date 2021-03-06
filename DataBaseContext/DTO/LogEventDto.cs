using System;

namespace DataBaseContext.DTO
{
    public class LogEventDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TurnNumber { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}