using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
using mobility.Interfaces;
using NUnit.Framework;
namespace mobility.Controllers
{
    [TestFixture]
    public class Get 
    {
        private  WeatherForecastController _weatherAPI;
        private  IWeatherAPI _api;

        [SetUp]
        public void Setup()
        {
            
            IWeatherAPI api =  new mobilityTest.Mock.WeatherAPIClientMock();

            _weatherAPI = new WeatherForecastController(new NullLogger<WeatherForecastController>(),api);
        }

        
        [Test(Description = "Empty Argument")]
        public void GivenEmptyThenReturnsNull()
        {
            // Act 
            var actual = _weatherAPI.Get(string.Empty);

            // Assert
            var expect = new mobility.Models.CurrentWeather();
            Assert.IsNull(actual);
        }

        [Test(Description = "Null Argument")]
        public void GivenNullThenReturnsNull()
        {
            // Act 
            var actual = _weatherAPI.Get(null);

            // Assert
            var expect = new mobility.Models.CurrentWeather();
            Assert.IsNull(actual);
       }

        [Test(Description = "Exception case")]
        public void GivenExceptionThenReturnNull()
        {
            // Arrange
            var api =  new mobilityTest.Mock.WeatherAPIClientMock();
            api.ThrowException = true;

            _weatherAPI = new WeatherForecastController(new NullLogger<WeatherForecastController>(),api);

            // Act
            var actual = _weatherAPI.Get("Should cause Exception");
            Assert.IsNull(actual);
            
       }

        [Test(Description = "Succeed")]
        public void GivenRightValueThenReturns()
        {
            // Act
            var actual = _weatherAPI.Get("Perth");
            
            Assert.AreEqual("Cloud",actual.weather[0].main);
            Assert.AreEqual(50,actual.clouds.all);
                        
       }

    }
}
