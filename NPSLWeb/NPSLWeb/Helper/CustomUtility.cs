using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NPSLWeb.Helper
{
    public class CustomUtility
    {
        static HttpClient client = new HttpClient();

        private async Task<T> GetAsync<T>(string uri)
        {
         
           HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                 t = JsonConvert.DeserializeObject<T>(response.Content.ToString());
            }
            return t;
        }
        static async Task<List<T>> GetProductAsync<T>(string path)
        {
            var client = new RestClient(url);
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                T = await response.Content.ReadAsAsync<List<T>>();
            }
            return T;
        }
    }
}
