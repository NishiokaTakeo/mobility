using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mobility.Interfaces;
namespace mobility.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherAPI _weatherAPI;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherAPI weatherAPI)
        {
            _logger = logger;
            _weatherAPI = weatherAPI;
        }

        [HttpGet("{city}")]
        public Models.CurrentWeather Get(string city)
        {
            Models.CurrentWeather res = null;

            try 
            {
                res = _weatherAPI.GetCurrentWeather(new Models.CurrentWeatherQuery(){q = city});            
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, "query parameter was not passed");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "API Error.");
            }
            
            return res;           
        }
    }
}
