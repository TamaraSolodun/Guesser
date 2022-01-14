namespace Shared_Guesser.Logger
{
    public class SerilogOptions
    {
        public const string FILE_PATH_FORMAT = "/app/log/log-.log";

        public string ConnectionString { get; set; }
    }
}
