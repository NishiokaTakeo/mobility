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

namespace mobility.Helpers
{
    public class WeatherAPIClient: IWeatherAPI
    {
        
        public RestClient _client;
        public IRestResponse _lastResponse;

        ILogger _logger;

        Interfaces.IConfiguration _IConfig;

        public WeatherAPIClient(Interfaces.IConfiguration iConfig)
        {
            _IConfig = iConfig;
            _client = new RestClient(_IConfig.GetServerURL()); 


            _logger = _IConfig.GetLogger();
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

            var request = new RestRequest(requestPath("weather"), Method.GET);
            
            AddQuery(request, ObjectToQueryString<CurrentWeatherQuery>(query));

            _lastResponse = Execute(request);

            CurrentWeather d = JsonConvert.DeserializeObject<CurrentWeather>(_lastResponse.Content);

            return d;
        }
        static Dictionary<string,string> ObjectToQueryString<T>(T obj) where T : class
        {
            var ans = new Dictionary<string,string>();

            StringBuilder sb = new StringBuilder();

            IEnumerable data = obj as IEnumerable ?? new[] { obj };

            foreach (var datum in data)
            {
                Type t = datum.GetType();
                var properties = t.GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    if (p.CanRead)
                    {
                        var indexes = p.GetIndexParameters();
                        if (indexes.Count() > 0)
                        {
                            var pp = p.GetValue(datum, new object[] { 1 });
                            sb.Append(ObjectToQueryString(pp));
                        }
                        else if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType  != typeof(string))
                        {
                            sb.Append(ObjectToQueryString(p.GetValue(datum)));
                        }
                        else
                        {

                            //I dont think this is a good way to do it
                            if (p.PropertyType.FullName != p.GetValue(datum, null).ToString())
                            {
                                if ( !string.IsNullOrWhiteSpace(p.GetValue(datum, null).ToString()))
                                {
                                    ans.Add(p.Name,p.GetValue(datum, null).ToString());
                                    // sb.Append(String.Format("{0}={1}&", p.Name, p.GetValue(datum, null).ToString()));
                                }
                            }
                            else
                            {
                                sb.Append(ObjectToQueryString(p.GetValue(datum, null)));
                            }
                        }
                    }
                }
            }
            // return sb.ToString().TrimEnd('&');
            return ans;
        }        

        string requestPath(string endpoint) 
        {
            _logger.Trace("Begining of Path");
            
            return string.Format("/{0}/?appid={1}", endpoint, _IConfig.APIKey());
        }

        /// <summary>
        /// Add HTTP query string parameter
        /// </summary>
        /// <param name="request">Rest Request object</param>
        /// <param name="dic">KeyValue, key=querystring key, value=querystring value</param>
        void AddQuery(IRestRequest request, Dictionary<string,string> dic)
        {
            if ( dic != null )
            {
                foreach( var item in dic)
                {
                    request.AddQueryParameter(item.Key,item.Value,true);
                }
            }            
        }

        /// <summary>
        /// Make HTTP request
        /// </summary>
        /// <param name="r">request object</param>
        /// <returns>response from api server</returns>
        IRestResponse Execute(RestRequest r)
        {
            _logger.Trace("Execute RestSharp");

            IRestResponse response = _client.Execute(r);

            if (response.StatusCode == HttpStatusCode.OK)
            {
            }
            else
            {
                response.Headers.ToList().ForEach(x =>
                {                        
                    _logger.Error("HTTP HEADER {0} : {1}", x.Name, x.Value);
                });

            }

            CookieContainer _cookieJar = _client.CookieContainer ?? new CookieContainer();
            foreach (var c in response.Cookies)
            {
                _cookieJar.Add(new System.Net.Cookie(c.Name, c.Value, c.Path, c.Domain));
                _logger.Debug(" Cookie {0} {1} {2} {3}", c.Name, c.Value, c.Path, c.Domain);
            }

            if (_cookieJar.Count > 0)
                _client.CookieContainer = _cookieJar;


            return response;
        }

    }

}
