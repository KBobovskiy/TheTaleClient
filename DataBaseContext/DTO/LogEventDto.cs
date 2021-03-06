using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseContext.DTO
{
    public class LogEventDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}