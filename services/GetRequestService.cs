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

        async public Task<List<Square[]>> GetRequest(string body)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path + body);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Square[]> container = JsonConvert.DeserializeObject<List<Square[]>>(responseBody); 
                return container;
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
