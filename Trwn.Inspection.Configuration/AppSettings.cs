namespace Trwn.Inspection.Configuration
{
    public class AppSettings
    {
        public DbSettings MongoDb { get; set; } = null!;
        public string PhotoStoragePath { get; set; } = "Photos";
    }
}
