namespace ValstroStarWars;

using ValstroStarWars.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using SocketIOClient;


public class program 
{
     public static void Main(String[] args)
    {
        MainAsync().GetAwaiter().GetResult();

    }


    public static async Task MainAsync() 
    {
        bool procesing = false;

        Queue<SocketIOResponse> list= new Queue<SocketIOResponse>();

        try {
            Console.WriteLine("Welcome to Valstro Star Wars SocketIO Machine");
            var _client = new SocketIO("http://localhost:3000/");

            _client.On("search", response =>
            {
                // Add to Queue to be procesed
                list.Enqueue(response);
            });


            _client.OnConnected += async (sender, e) =>
            {
                // Connected to client.
                Console.WriteLine("Connected to client");
            };

            await _client.ConnectAsync();


            
            if (_client.Connected) 
            {
                bool ask = true;
                while(ask)         
                {
                    Console.Write("What do you want to search:");
                    string? search = Console.ReadLine();
                    

                    await _client.EmitAsync("search", new Search {Query = search}); 
                    procesing = true;

                    while (procesing) {
                        while(list.Count != 0) 
                        {
                            var res = list.Dequeue();
                            //Console.WriteLine(res);
                            var sres = res.GetValue<SearchResult>(0);
                            Console.WriteLine("({0}/{1}) - {2} - [{3}]", sres.Page, sres.ResultCount, sres.Name, sres.Films);

                            if (sres.Page == sres.ResultCount) procesing = false;
                        }

                    }

                    Console.WriteLine("Streaming finished.");
                    Console.WriteLine("Another search?. (Y/n)");
                    string? anotherTry = Console.ReadLine();
                    if (anotherTry.ToLower()!= "y") ask = false;
                }
            }


        } catch (Exception ex){
            Console.WriteLine("EX => " + ex.Message);
        }
    }

    
}