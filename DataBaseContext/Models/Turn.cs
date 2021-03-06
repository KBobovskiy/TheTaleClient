using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataBaseContext.Models
{
    public class Turn
    {
        [Key]
        public int Number { get; set; }

        public string VerboseDate { get; set; }

        public string VerboseTime { get; set; }
    }
}