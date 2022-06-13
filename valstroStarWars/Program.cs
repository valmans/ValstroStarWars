namespace ValstroStarWars;

using ValstroStarWars.Models;
using System;
using SocketIOClient;


public class program 
{
     public static void Main(String[] args)
    {
        MainAsync().GetAwaiter().GetResult();
        return;
    }


    public static async Task MainAsync() 
    {
        bool procesing = false;
        string server = "http://localhost:3000/";
        Queue<SocketIOResponse> list= new Queue<SocketIOResponse>();
        var client = new SocketIO(server);
        try {
            Console.WriteLine("Welcome to Valstro Star Wars SocketIO Service");
            
            client.On("search", response =>
            {
                // Add to Queue to be procesed
                list.Enqueue(response);
            });


            client.OnConnected += async (sender, e) =>
            {
                // Connected to client.
                Console.WriteLine("Connected to server: " + server);
                await Task.Delay(10);
            };

            client.OnDisconnected += async (sender, e) =>
            {
                // Connected to client.                
                Console.WriteLine("server: " + server + " disconnected");                
                await Task.Delay(10);
                return;
            };

            client.OnError += async (sender, e) =>
            {
                Console.WriteLine("ERROR => " + e.ToString());    
                await Task.Delay(10);
            };

            await client.ConnectAsync();

            
            if (client.Connected) 
            {
                bool ask = true;
                while(ask)         
                {
                    Console.Write("\n - What character would you like to search for? => ");
                    string search = Console.ReadLine();
                    

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

                    Console.WriteLine("\n - Streaming finished.");
                    
                    string anotherTry = null;
                    while (anotherTry == null) {
                        Console.Write(" - Search again?. (Y/n) => ");
                        anotherTry = Console.ReadLine();
                        if (!anotherTry.ToLower().Equals("y") && !anotherTry.ToLower().Equals("n")) anotherTry = null;
                        if (anotherTry is not null && anotherTry.ToLower().Equals("n")) ask = false;                        
                    }                    
                } 
               
            } else {
                Console.WriteLine("ERROR => Cannot connect to server: " + server);    
            }
             client.Dispose();
             return;

        } catch (Exception ex){
            Console.WriteLine("EX => " + ex.Message);
            client.Dispose();
            return;
        }
    }

}