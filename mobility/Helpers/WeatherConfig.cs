using NLog;

namespace mobility.Helpers
{
    public class WeatherConfig : mobility.Interfaces.IConfiguration
    {
        static ILogger _logger = LogManager.GetCurrentClassLogger();

        public string APIKey()
        {
            //TODO: should get a value from appsettings.json
            return "fb5570ed16f31ca1e3570778ddceb75c";
        }

        public ILogger GetLogger()
        {
            return _logger;
        }

        public string GetServerURL()
        {
            //TODO: should get a value from appsettings.json
            return "http://api.openweathermap.org/data/2.5/";
        }
    }
}