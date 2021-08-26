using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using iot.Models;

namespace iot
{
    class GetRequestService
    {
        const string path = "http://localhost:5000/api/";
        readonly HttpClient client = new HttpClient();

        async public Task<T> GetRequest<T>(string body) where T : class 
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path + body);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                T container = JsonConvert.DeserializeObject<T>(responseBody);
                return container;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }

        async public Task<string> GetRequestAsString(string body)  
        {
            try
            { 
                HttpResponseMessage response = await client.GetAsync(path + body);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        } 
    }
}
