using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client.Extensions
{
    public static class HttpClientExtension
    {
        private readonly static string _baseUrl = "https://localhost:5001/api/";

        public async static Task<T> GetDataAsync<T>(this HttpClient client, string url)
        {
            try
            {
                var content = await client.GetAsync(_baseUrl+url);     
                if(!content.IsSuccessStatusCode)
                    throw new HttpRequestException(await content.Content.ReadAsStringAsync(), null, content.StatusCode);

                return JsonConvert.DeserializeObject<T>(await content.Content.ReadAsStringAsync());
            }
            catch(HttpRequestException)
            {
                throw;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return default(T);
            }
        }

        public static async Task<HttpResponseMessage> GetDataAsync(this HttpClient client, string url)
        {
            var content = await client.GetAsync(_baseUrl+url);     
            if(!content.IsSuccessStatusCode)
                throw new HttpRequestException(await content.Content.ReadAsStringAsync(), null, content.StatusCode);

            return content;
        }

        public enum Methods
        {
            POST,
            PUT,
            DELETE
        }

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient client, Methods method, string url, object obj = null)
        {
            HttpResponseMessage response = null;;
            var path = _baseUrl+url;

            switch (method)
            {
                case Methods.POST:
                    response = await client.PostAsync(path, 
                        new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
                    break;
                case Methods.PUT:
                    response = await client.PutAsync(path, 
                        new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
                    break;
                case Methods.DELETE:
                    response = await client.DeleteAsync(path);
                    break;
                default:
                    break;
            } 

            if(response != null && !response.IsSuccessStatusCode)
                throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

            return response;
        }
    }
}