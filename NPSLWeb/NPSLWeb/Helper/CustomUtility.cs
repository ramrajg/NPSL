using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NPSLWeb.Helper
{
    public static class CustomUtility
    {
        static HttpClient client = new HttpClient();
        static string UrlHostingPath = "http://localhost:50704/";

        public static T GetAsync<T>(string uri) where T : new()
        {
            //try
            //{
            T tempObject = new T();
            uri = UrlHostingPath + uri;
            HttpResponseMessage response = client.GetAsync(uri).Result;
            string readAsStringAsync = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
            {
                tempObject = JsonConvert.DeserializeObject<T>(readAsStringAsync);
                //tempObject = resp.Deserialize<T>();
            }
            //}
            //catch (Exception ex)
            //{
            //    statusDescription = ex.Message;
            //}
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
    }
}
