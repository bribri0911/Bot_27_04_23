using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord.Net;
using DiscordRPC.Message;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRpcDemo;
using bot_Discord_Test1.Modules;
using System.ComponentModel;
using System.IO;

namespace bot_Discord_Test1
{
    class Program
    {
        private string test;
        private DiscordRpc.EventHandlers handlers;
        private DiscordRpc.RichPresence presence;
        public string MessageRecu = "";
        private bool connected = false;


        /*
          static void UpdatePresence()
          {
              DiscordRichPresence discordPresence;
              memset(&discordPresence, 0, sizeof(discordPresence));
              discordPresence.state = "playing Valorant";
              discordPresence.details = "Prefix !";
              discordPresence.startTimestamp = 1507665886;
              discordPresence.endTimestamp = 1507665889;
              discordPresence.largeImageText = "Numbani";
              discordPresence.smallImageText = "Rogue - Level 100";
              discordPresence.partyId = "ae488379-351d-4a4f-ad32-2b9b01c91657";
              discordPresence.partySize = 2;
              discordPresence.partyMax = 10;
              discordPresence.joinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= ";
              Discord_UpdatePresence(&discordPresence);
          }*/

        public void test2()
        {
            if (connected)
            {
                this.handlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("1041501324761640990", ref this.handlers, true, null);
                this.presence = new DiscordRpc.RichPresence();
                this.presence.details = "Programmer";
                this.presence.state = "Mon bot : \r\n https://urlz.fr/kE6g";
                this.presence.matchSecret = "https://discord.gg/GF7ANtKh5J";
                this.presence.joinSecret = "https://discord.gg/GF7ANtKh5J";
                this.presence.largeImageKey = "plage";
                this.presence.smallImageKey = "plage";
                this.presence.largeImageText = "Image 1 Text";
                this.presence.smallImageText = "Image 2 Text";
                DiscordRpc.UpdatePresence(ref this.presence);
            }
        }

         
        


        private string MaClef = "MTA0MTUwMTMyNDc2MTY0MDk5MA.GRN4iD.ua2RCGgT3Ev8zQhtV0wnQne63YCel2FBPubhUY";

        private Bot client;
        private CommandService commands;


        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private bool Debut = false;

        public async Task RunBotAsync()
        {
            if(!Debut)
            {
                test = DateTime.Now.ToString("MM/dd/yyyy=HH/mm/ss/FFF");
            }


            client = new Bot(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.All
            });


            commands = client.CommandService;

            Buttons buttons = new Buttons();

            client.Log += Log;
            

            client.ButtonExecuted += async (component) =>
            {
                buttons.Handle(component, client);
            };


            client.Ready += () =>
            {
                Console.WriteLine("Je suis prêt");
                connected = true;
                test2();
                return Task.CompletedTask;
            };




            await InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, MaClef);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }




        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var message = (SocketUserMessage)msg;

            if (message == null) return;

            int argPos = 0;
            //BDD maBDD = new BDD();

            //maBDD.ConnectionBDD();
            

            if (!message.HasStringPrefix("!!", ref argPos))
            {
                /*var context2 = new SocketCommandContext(client, message);
                int i = 1;

                while (i != 0)
                {
                    await context2.User.SendMessageAsync("Je vais te hanter jusqu'au bout de ta vie");
                }*/
                
                MessageRecu += message;

                VerifMsgRecu test = new VerifMsgRecu();
                test.asyncVerifMsgRecu(MessageRecu);

                return;

            }


            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, null);


            //Error

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);

        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            /*
            StreamWriter logBot = new StreamWriter($"LogBot{test}.txt", true);

            logBot.WriteLine(arg.ToString() + "\r\n=");

            logBot.Close();
            */
            return Task.CompletedTask;
        }




    }
}
