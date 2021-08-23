using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace iot.Services
{
    class SignalRClient
    {
        HubConnection connection;
        const string path = "http://localhost:5000";

        public SignalRClient()
        {
            connection = new HubConnectionBuilder()
                            .WithUrl(path + "/chat")
                            .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

        }
        public async Task Connect()
        {
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("connection signalr exeption!");
            }
        }
        public async Task SendMessage(string message)
        {
            await connection.InvokeCoreAsync("SendMessage", args: new[] { message });
        }
    }
}