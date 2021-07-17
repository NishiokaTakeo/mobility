using NLog;

namespace mobility.Interfaces 
{
    public interface IConfiguration
    {
        string APIKey();

        string GetServerURL();

        ILogger GetLogger();
        
    }
}