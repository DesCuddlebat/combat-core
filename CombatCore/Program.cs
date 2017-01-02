using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace GameBot
{
    class Program
    {
        public Random Rand = new Random();

        public DiscordSocketClient Client;
        public IO IO;
        public Dictionary<ulong, Player> Players = new Dictionary<ulong, Player>();
        public static Program Prog;

        static void Main(string[] args)
        {
           new Program().Start().GetAwaiter().GetResult();
        }
        //173007751822311424
        async Task Start()
        {
            Client = new DiscordSocketClient();
            IO = new IO();
            Prog = this;

            var token = "MTczMDA3NzUxODIyMzExNDI0.CsfOwA.B1tTj13iySnqgLzxxMa6zEgZRYo";

            // Login and connect to Discord.
            while (true)
            {
                try
                {
                    await Client.LoginAsync(TokenType.User, token);
                    await Client.ConnectAsync();
                    break;
                }
                catch (TimeoutException)
                {
                    await Task.Delay(3000);
                }
            }

            Client.MessageReceived += IO.DiscordInput;

            Console.Out.WriteLine("Eeeep!");

            while (true) IO.ProcessInput(IOMode.Console, new ACIData(await Console.In.ReadLineAsync()));
        }

        public void AddPlayer(ulong Id, Player Player)
        {
            if(!Players.ContainsKey(Id))
            Players.Add(Id, Player);
        }
    }
}
