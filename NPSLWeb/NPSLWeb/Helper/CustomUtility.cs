using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace NPSLWeb.Helper
{
    public static class CustomUtility
    {
        static HttpClient client = new HttpClient();
      
        static readonly IConfiguration _Connectionstring;
        static string UrlHostingPath = "";

        static CustomUtility()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            _Connectionstring = builder.Build();
            UrlHostingPath = _Connectionstring["ApiConnection:ApiHost"];
        }

        public static T GetAsync<T>(string uri) where T : new()
        {
            T tempObject = new T();
            uri = UrlHostingPath + uri;
            HttpResponseMessage response = client.GetAsync(uri).Result;
            string readAsStringAsync = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                tempObject = JsonConvert.DeserializeObject<T>(readAsStringAsync);
            }
            return tempObject;
        }

        public static List<T> GetSingleRecord<T>(string uri) where T : new()
        {
            List<T> tempObject = new List<T>();
            try
            {
                uri = UrlHostingPath + uri;
                HttpResponseMessage response = client.GetAsync(uri).Result;
                string readAsStringAsync = response.Content.ReadAsStringAsync().Result;
                if (response.IsSuccessStatusCode)
                {
                    tempObject = readAsStringAsync.Deserialize<T>();
                }
                else { throw new Exception(readAsStringAsync); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return tempObject;
        }
        public static List<T> Deserialize<T>(this string SerializedJSONString)
        {
            var stuff = JsonConvert.DeserializeObject<List<T>>(SerializedJSONString);
            return stuff;
        }
        public static void Post( string apiCall, string message)
        {
            var response = Post(UrlHostingPath + apiCall, message != null ? new StringContent(message, Encoding.UTF8, "application/json") : null, apiCall);
        }
        public static string PostDataOfType(string apiCall, object objectToPost, out bool isSuccessStatusCode)
        {
            return PostData( apiCall, objectToPost, out isSuccessStatusCode);
        }
        public static string PostData(string apiCall, object objectToPost, out bool isSuccessStatusCode)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(objectToPost), Encoding.UTF8, "application/json");
            var response = Post(UrlHostingPath + apiCall, stringContent, apiCall);
            isSuccessStatusCode = response.IsSuccessStatusCode;
            var readAsStringAsync = response.Content.ReadAsStringAsync();
            var resp = readAsStringAsync.Result;
            string loggingMessage = string.Format("Log = API Request:{0} API Response:{1}", stringContent.ReadAsStringAsync().Result, resp);
            return resp;
        }
        private static HttpResponseMessage Post(string requestUri, HttpContent content, string uri)
        {
            HttpResponseMessage obj = client.PostAsync(requestUri, content).Result;
            return obj;
        }
    }
}
