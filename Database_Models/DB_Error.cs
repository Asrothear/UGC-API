using System;

namespace UGC_API.Database_Models
{
    public class DB_Error
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Json { get; set; }
        public string Event { get; set; }
        public DateTime TimeStamp { get; set; }
        public int User { get; set; }

    }
}
