using System;
using System.Threading.Tasks;
using iot.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace iot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SignalRClient Client = new SignalRClient();
            await Client.Connect();
            MoveService moveService = new MoveService(Client);
            await moveService.Init();
            try
            {
                await moveService.Move();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
