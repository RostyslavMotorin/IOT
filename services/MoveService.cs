using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iot.Models;
using System.Threading;
using Microsoft.AspNetCore.SignalR.Client;

namespace iot.Services
{
    class MoveService
    {
        SignalRClient client;
        public MoveService(SignalRClient client = null)
        {
            this.client = client;
        }
        private List<Square[]> roomsCoordinates;
        private bool IsOpenConnection = true;
        private Random r = new Random();
        private Square lastPos;
        private string user;

        async public Task Init()
        {
            try
            {
                GetRequestService getRequestService = new GetRequestService();
                roomsCoordinates = await getRequestService.GetRequest<List<Square[]>>("SensorNonCRUD/GetCoordinates");
                user = await getRequestService.GetRequestAsString("SensorNonCRUD/TakeUserForSensor");
            }
            catch (NullReferenceException e)
            {
                System.Console.WriteLine("Error null reference exeption!");
            }
        }
        async public Task Move()
        {
            int room = r.Next(0, roomsCoordinates.Count);
            int startIndex = r.Next(0, roomsCoordinates[room].Length);
            lastPos = new Square();
            Square position = new Square();
            position = roomsCoordinates[room][startIndex];

            while (IsOpenConnection)
            {
                position = generateMove(position);
                if (lastPos.X == position.X && lastPos.Y == position.Y)
                {
                    await Move();
                    return;
                }
                await client.SendMessage(position.X +":"+ position.Y, user);
                System.Console.WriteLine(position.X + " " + position.Y + " ^^" + user);
                
                await Task.Delay(1000);
            }
        }
        private Square generateMove(Square position)
        {
            List<Square> arrayPos = new List<Square>();
            if (CheckPosition(new Square() { X = position.X - 1, Y = position.Y }) && (lastPos.X != position.X - 1))
                arrayPos.Add(new Square() { X = position.X - 1, Y = position.Y });
            if (CheckPosition(new Square() { X = position.X + 1, Y = position.Y }) && (lastPos.X != position.X + 1))
                arrayPos.Add(new Square() { X = position.X + 1, Y = position.Y });
            if (CheckPosition(new Square() { X = position.X, Y = position.Y - 1 }) && (lastPos.Y != position.Y - 1))
                arrayPos.Add(new Square() { X = position.X, Y = position.Y - 1 });
            if (CheckPosition(new Square() { X = position.X, Y = position.Y + 1 }) && (lastPos.Y != position.Y + 1))
                arrayPos.Add(new Square() { X = position.X, Y = position.Y + 1 });
            foreach (var d in arrayPos)
            {
                System.Console.WriteLine("test:" + d.X + " " + d.Y);
            }
            int index = r.Next(0, arrayPos.Count);

            if (arrayPos.Count == 0)
            {
                return position;
            }
            lastPos = new Square() { X = position.X, Y = position.Y };
            return arrayPos[index];
        }
        private bool CheckPosition(Square position)
        {
            for (int i = 0; i < roomsCoordinates.Count; i++)
            {
                for (int j = 0; j < roomsCoordinates[i].Length; j++)
                {
                    if (position.X == roomsCoordinates[i][j].X && position.Y == roomsCoordinates[i][j].Y)
                        return true;
                }
            }
            return false;
        }

    }
}