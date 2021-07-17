using Newtonsoft.Json;
using NLog;
using RestSharp;
// using SpiderDocsModule;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;
using mobility.Models;
using System.Collections;
using System.Reflection;
using mobility.Interfaces;

namespace mobilityTest.Mock
{
    public class WeatherAPIClientMock: IWeatherAPI
    {
        CurrentWeather _weather;
        public WeatherAPIClientMock()
        {
            _weather =  new CurrentWeather();
            _weather.weather = new List<Weather>();
            _weather.clouds = new Clouds();
            _weather.weather.Add(new Weather(){ main = "Cloud"});
            _weather.clouds.all = 50;

        }
        public bool ThrowException {get;set;} = false;
        
        /// <summary>
        /// Create return value for currentweather
        /// </summary>
        /// <param name="weather"></param>
        /// <returns></returns>
        public CurrentWeather _Create(CurrentWeather weather)
        {
            _weather = _Clone(weather);

            return _weather;
        }
        
        /// <summary>
        /// Get Current Weather by query parameters
        /// All queryable parameter is https://openweathermap.org/current
        /// </summary>
        /// <param name="query">Currently it suppors only q param. </param>
        /// <returns>Current Weather object</returns>
        public CurrentWeather GetCurrentWeather(CurrentWeatherQuery query)
        {
            if ( query == null || string.IsNullOrWhiteSpace(query.q) )
            {
                throw new ArgumentException("Parameter is not passed");
            }

            if ( ThrowException) throw new Exception("API Error");

            return _weather;
        }

        /// <summary>
        /// Just clone object
        /// </summary>
        /// <param name="weather"></param>
        /// <returns></returns>
        static CurrentWeather _Clone(CurrentWeather weather)
        {
            var list = new System.Collections.Generic.List<mobility.Models.CurrentWeather>();
            list.Add(weather);

            return list.ToList().First();

        }        

    }

}
