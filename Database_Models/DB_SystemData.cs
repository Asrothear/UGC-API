namespace UGC_API.Database_Models
{
    public class DB_SystemData
    {
        public int id { get; set; }
        public string starSystem { get; set; }
        public ulong systemAddress { get; set; }
        public string starPos { get; set; }
        public long population { get; set; }
    }
}
