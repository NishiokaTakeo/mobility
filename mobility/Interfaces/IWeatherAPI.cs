using mobility.Models;

namespace mobility.Interfaces
{
    public interface IWeatherAPI
    {        
        CurrentWeather GetCurrentWeather(CurrentWeatherQuery query);

    }
}