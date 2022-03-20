namespace UGC_API.Database_Models
{
    public class DB_Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public string token { get; set; }        
        public bool active { get; set; }
        public string blocked { get; set; }
    }
}
