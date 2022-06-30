using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UGC_API.Database_Models
{
    public class DB_Explorer
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int User { get; set; }
        public int BaseValue { get; set; }
        public int Bonus { get; set; }
        public int TotalEarnings { get; set; }
        public string DataType { get; set; }
        [NotMapped]
        public List<Bio> BioData { get; set; }
        [NotMapped]
        public class Bio
        {
            public int Value { get; set; }
        }
    }
}
