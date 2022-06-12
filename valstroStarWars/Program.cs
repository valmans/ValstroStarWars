namespace ValstroStarWars;

using ValstroStarWars.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketIOClient;


public class program 
{
     public static void Main(String[] args)
    {
        MainAsync().GetAwaiter().GetResult();

    }


    public static async Task MainAsync() 
    {
        bool receiving = false;

        try {
            Console.WriteLine("Welcome to Valstro Star Wars SocketIO Machine");
            var _client = new SocketIO("http://localhost:3000/");

            _client.On("search", response =>
            {
                // You can print the returned data first to decide what to do next.
                // output: ["hi client"]
                Console.WriteLine("Respuesta");
                Console.WriteLine(response);

                string text = response.GetValue<string>();
                receiving = false;
            });


            _client.OnConnected += async (sender, e) =>
            {
                // Emit a string
                Console.WriteLine("Connected to client");
            };

            await _client.ConnectAsync();



            if (_client.Connected) 
            {
                Console.Write("What do you want to search:");
                string search = Console.ReadLine();

                await _client.EmitAsync("search", new Search {Query = search});
                receiving = true;
                while (receiving);
            }

            
        } catch (Exception ex){
            Console.WriteLine("EX => " + ex.Message);
        }
    }

    
}