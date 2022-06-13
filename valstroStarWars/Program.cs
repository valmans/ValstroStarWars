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
        string server = "http://localhost:3000/";
        Queue<SocketIOResponse> list= new Queue<SocketIOResponse>();

        try {
            Console.WriteLine("Welcome to Valstro Star Wars SocketIO Service");
            var client = new SocketIO(server);

            client.On("search", response =>
            {
                // Add to Queue to be procesed
                list.Enqueue(response);
            });

            client.On("error", response =>
            {
                // Add to Queue to be procesed
                Console.WriteLine("ERROR => " + response);    
            });


            client.OnConnected += async (sender, e) =>
            {
                // Connected to client.
                Console.WriteLine("Connected to server: " + server);
               
            };

            client.OnError += async (sender, e) =>
            {
                // Connected to client.
                Console.WriteLine("ERROR => " + e.ToString());    
            };

            await client.ConnectAsync();


            
            if (client.Connected) 
            {
                bool ask = true;
                while(ask)         
                {
                    Console.Write("\n - What character would you like to search for? => ");
                    string? search = Console.ReadLine();
                    

                    await client.EmitAsync("search", new Search {Query = search}); 
                    procesing = true;

                    while (procesing) {
                        while(list.Count != 0) 
                        {
                            var res = list.Dequeue();
                            //Console.WriteLine(res);
                            var sres = res.GetValue<SearchResult>(0);
                            if (sres.ResultCount == -1) {
                                Console.WriteLine("\n - ERROR: '{0}' ", sres.Error);
                            } else {
                                Console.WriteLine(" - ({0}/{1}) - {2} - [{3}]", sres.Page, sres.ResultCount, sres.Name, sres.Films);   
                            }
                            if (sres.Page == sres.ResultCount) procesing = false;
                        }

                    }

                    Console.WriteLine(" - Streaming finished.\n");
                    
                    
                } 

            } else {
                Console.WriteLine("ERROR => Cannot connect to server: " + server);    
            }


        } catch (Exception ex){
            Console.WriteLine("EX => " + ex.Message);
        }
    }

    
}