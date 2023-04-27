using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Discord.Net;


namespace bot_Discord_Test1.Modules
{
    public class Commands: ModuleBase<SocketCommandContext>
    {
        private bool Permission = true;
        private List<ulong> requiredRoleIds = new List<ulong>
            {
                1091010694004084766,
                1092472560958046320,
                1092472221697593475,
                1092472331563180134,
                1092471106142744777,
                1092475853969047603
            };
        public bool StopSpam = false;
        public List<SocketUser> ListUser = new List<SocketUser>();

        private async Task VerifRole(SocketCommandContext context)
        {
            Permission = true;

            // Récupérer l'utilisateur qui a envoyé la commande
            var user2 = Context.User as SocketGuildUser;


            // Récupérer les rôles requis par leur ID
            var requiredRoles = requiredRoleIds.Select(x => Context.Guild.GetRole(x)).ToList();

            // Vérifier si l'utilisateur a au moins l'un des rôles requis
            if (requiredRoles.Any(x => x == null) || !requiredRoles.Any(x => user2.Roles.Contains(x)))
            {
                await Context.Channel.SendMessageAsync("Vous devez avoir l'un de ces rôles suivants pour utiliser cette commande : [**" + string.Join(", ", requiredRoles) + "**]");
                Permission = false;
                return;
            }
        }


        [Command("ping")]
        [Summary("donne le ping du bot")]
        public async Task PingAsync()
        {
            int leping = Context.Client.Latency;
            await ReplyAsync($"le ping du bot est de : {leping}");
        }


        [Command("pp")]
        [Summary("Affiche ton avatar")]
        public async Task PPAsync(ushort size = 512)
        {
            await ReplyAsync(CDN.GetUserAvatarUrl(Context.User.Id, Context.User.AvatarId, size, ImageFormat.Auto));
        }

        [Command("react")]
        [Summary("Réagis avec la reaction donné")]
        public async Task ReactAsync(string msg, string pEmoji)
        {
            var message = await Context.Channel.SendMessageAsync(msg);
            var emoji = new Emoji(pEmoji);

            await message.AddReactionAsync(emoji);
        }

        

        [Command("say")]
        [Summary("Envoie un message à un canal spécifié. [**prefix**][\"nom du channel sans le #\"] [\"Le message\"")]
        public async Task SayAsync(ITextChannel channel, [Remainder] string message = null)
        {
                
            if (message == null)
            {
                await Context.Channel.SendMessageAsync("il faut mettre un message a envoier");
                return;
            }

            await channel.SendMessageAsync(message);
        }

        [Command("embed")]
        [Summary("Envoie un embed, [**prefix**][**commande**][**channel**][**titre**][**description**]")]
        public async Task Embed(ITextChannel channel, string Titre,[Remainder] string description)
        {

            if (description == null || Titre == null)
            {
                await Context.Channel.SendMessageAsync("remplissé le Titre et la descrption de votre embed");
                return;
            }

            //string[] Regle = description.Split('☺');

            var test = Context.User.AvatarId;
            
            var user = Context.User as SocketGuildUser;
            var bannerUrl = user.GetAvatarUrl(ImageFormat.WebP, 1024);



            var embed = new EmbedBuilder()
                    .WithTitle(Titre)
                    .WithColor(Color.Red)
                    .WithDescription(description)
                    .WithFooter(DateTime.Now.ToString());
                    //.WithImageUrl(bannerUrl);
                    //.Build();

            await channel.SendMessageAsync(embed: embed.Build());

        }

        
        [Command("regle")]
        [Summary("Envoie un embed, [**prefix**][**commande**][**channel**][**titre**][**description**]")]
        public async Task regle(ITextChannel channel, string Titre, [Remainder] string description)
        {

            if (description == null || Titre == null)
            {
                await Context.Channel.SendMessageAsync("remplissé le Titre et la descrption de votre embed");
                return;
            }

            string[] Regle = description.Split('²');





            var embed = new EmbedBuilder()
                    .WithTitle(Titre)
                    .WithColor(Color.Red)
                    .WithFooter(DateTime.Now.ToString());
            //.Build();


            for (int i = 0; i < Regle.Length; i+=2)
            {
                embed.AddField(Regle[i], Regle[i+1]);
            }

            await channel.SendMessageAsync(embed: embed.Build());

        }

        

        [Command("mission")]
        [Summary("Envoie un embed, [**prefix**][**commande**][**channel**][**titre**][**description**]")]
        public async Task Mission(ITextChannel channel, string description, string pEmoji)
        {
            try
            {


                if (description == null || channel.Name == null)
                {
                    await Context.Channel.SendMessageAsync("Remplissez le Titre et la description de votre embed.");
                    return;
                }

                if (pEmoji == null)
                {
                    pEmoji = "✅";
                }

                var embed = new EmbedBuilder()
                    .WithTitle("Mission " + channel.Name)
                    .WithDescription(description)
                    .WithColor(Color.Red)
                    .WithFooter(DateTime.Now.ToString())
                    .Build();

                var message = await channel.SendMessageAsync(embed: embed);


                IEmote emoji;
                if (Emoji.TryParse(pEmoji, out var unicodeEmoji))
                {
                    // L'emoji est un emoji Unicode
                    emoji = unicodeEmoji;
                }
                else if (Emote.TryParse(pEmoji, out var customEmote))
                {
                    // L'emoji est un emoji personnalisé
                    emoji = customEmote;
                }
                else
                {
                    // L'emoji est un GIF animé
                    emoji = new Emoji(pEmoji);
                }

                // Ajouter l'emoji en tant que réaction
                await message.AddReactionAsync(emoji);

                var delay = TimeSpan.FromDays(7);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(delay);
                    await message.DeleteAsync();
                });

            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync($"Une erreur s'est produite : {ex.Message}");
            }
        }

        /*
        [Command("sup")]
        [Summary("Supprime les N derniers messages sur le canal.")]
        public async Task SupprimerMessages(int count = 1, SocketTextChannel channel = null)
        {
            if (channel == null)
            {
                // Récupérer le canal où la commande a été utilisée
                channel = Context.Channel as SocketTextChannel;
            }

            // Récupérer les messages du canal
            var messages = await channel.GetMessagesAsync(count + 1).FlattenAsync();

            // Envoyer un message de confirmation
            var confirmationMessage = await ReplyAsync($"Êtes-vous sûr de vouloir supprimer {messages.Count()} messages ? Répondez par oui ou non.");

            try
            {
                // Attendre la réponse de l'utilisateur pendant 15 secondes
                var response = await Context.Channel.NextMessageAsync(Context);

                if (response == null)
                {
                    await ReplyAsync("Vous n'avez pas répondu à temps, la commande a été annulée.");
                    return;
                }

                if (response.Content.ToLower() == "oui")
                {
                    // Supprimer les messages récupérés
                    await channel.DeleteMessagesAsync(messages);

                    await ReplyAsync($"{messages.Count()} messages ont été supprimés.");
                }
                else
                {
                    await ReplyAsync("La commande a été annulée.");
                }
            }
            catch (Exception)
            {
                await ReplyAsync("Une erreur est survenue lors de la récupération de la réponse de l'utilisateur.");
                return;
            }

            // Supprimer le message de confirmation
            await confirmationMessage.DeleteAsync();
        }*/

        [Command("sup")]
        [Summary("Supprime les N derniers messages sur le canal.")]
        public async Task SupprimerMessages(int count = 1, SocketTextChannel channel = null)
        {
            if (channel == null)
            {
                // Récupérer le canal où la commande a été utilisée
                channel = Context.Channel as SocketTextChannel;
            }

            // Récupérer les messages du canal
            var messages = await channel.GetMessagesAsync(count + 1).FlattenAsync();


            //await channel.SendMessageAsync($"Etes vous sûr de vouloir del les {count} messages du channel {channel}");


            // Supprimer les messages récupérés
            await channel.DeleteMessagesAsync(messages);
        }


        [Command("mspam")]
        public async Task SendSpamAsync(IGuildUser user, [Remainder] string message)
        {

            //await VerifRole(Context);

            if (Permission)
            {
                //1096004280818872352     // salon pour discd's service
                // 1096359478938570802     // salon pour trankility


                if ((!ListUser.Contains(Context.User)))
                {

                    ListUser.Add(Context.User);

                    while (!StopSpam)
                    {


                        var channel = Context.Guild.GetTextChannel(1096359478938570802);
                        await user.SendMessageAsync(message);



                        await Context.Channel.SendMessageAsync($"Message envoie à :  {user.Mention}");


                        var embed = new EmbedBuilder()
                            .WithTitle("Envoye d'un message en mp :")
                            .WithDescription(message)
                            .WithColor(Color.Red)
                            .WithFooter(DateTime.Now.ToString())
                            .AddField("De", Context.User.Mention)
                            .AddField("À", user.Mention)
                            .Build();

                        await channel.SendMessageAsync(embed: embed);

                        StreamWriter MaBDDtext = new StreamWriter("MaBDDtext.txt", true);

                        MaBDDtext.WriteLine($"{embed.Title} \r\n " +
                            $"{embed.Description}\r\n " +
                            $"de : {Context.User.Username} ID : {Context.User.Id} \r\n " +
                            $"A : {user.Username} ID : {user.Id} \r\n" +
                            $"#################################################################\r\n" +
                            $"#################################################################");
                        MaBDDtext.Close();
                    }
                    StopSpam= false;
                }
                
            }
        }

        [Command("message")]    
        public async Task SendMessageAsync(IGuildUser user, [Remainder] string message)
        {

            //await VerifRole(Context);

            if (Permission)
            {
                //1096004280818872352
                //1096359478938570802

                var channel = Context.Guild.GetTextChannel(1096359478938570802);
                await user.SendMessageAsync(message);



                await Context.Channel.SendMessageAsync($"Message envoie à :  {user.Mention}");


                var embed = new EmbedBuilder()
                    .WithTitle("Envoye d'un message en mp :")
                    .WithDescription(message)
                    .WithColor(Color.Red)
                    .WithFooter(DateTime.Now.ToString())
                    .AddField("De", Context.User.Mention)
                    .AddField("À", user.Mention)
                    .Build();

                await channel.SendMessageAsync(embed: embed);

                StreamWriter MaBDDtext = new StreamWriter("MaBDDtext.txt", true);

                MaBDDtext.WriteLine($"{embed.Title} \r\n " +
                    $"{embed.Description}\r\n " +
                    $"de : {Context.User.Username} ID : {Context.User.Id} \r\n " +
                    $"A : {user.Username} ID : {user.Id} \r\n" +
                    $"#################################################################\r\n" +
                    $"#################################################################");
                MaBDDtext.Close();

            }
        }



        [Command("help")]
        [Summary("Affiche la liste de toutes les commandes disponibles.")]
        public async Task HelpAsync()
        {
            var commandList = ((Bot) Context.Client).CommandService.Commands;
            int commandsCount = commandList.Count();

            int perPage = 2;

            var embedBuilder = new EmbedBuilder
            {
                Title = "Liste des commandes",
                Color = Color.Green
            };

            int i = 0;
            foreach (var command in commandList)
            {
                if (i >= perPage) break;

                embedBuilder.AddField(command.Name, command.Summary ?? "Pas de description disponible.");

                i++;
            }

            var builder = new ComponentBuilder()
                .WithButton("⏮️", "button -previous")
                .WithButton("⏭️", "button-next");


            IUserMessage replyMessage = await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());

            ((Bot) Context.Client).RegisterHelpMessage(replyMessage);
        }

        [Command("test")]
        public async Task Weather(IUserMessage umsg, string city, string country)
        {
            var channel = (ITextChannel)umsg.Channel;
            city = city.Replace(" ", "");
            country = city.Replace(" ", "");
            string response;
            using (var http = new HttpClient())
                response = await http.GetStringAsync($"http://api.ninetales.us/nadekobot/weather/?city={city}&country={country}").ConfigureAwait(false);

            var obj = JObject.Parse(response)["weather"];

            var embed = new EmbedBuilder()
                .AddField(fb => fb.WithName("🌍 Location").WithValue($"{obj["target"]}").WithIsInline(true))
                .AddField(fb => fb.WithName("📏 Lat,Long").WithValue($"{obj["latitude"]}, {obj["longitude"]}").WithIsInline(true))
                .AddField(fb => fb.WithName("☁ Condition").WithValue($"{obj["condition"]}").WithIsInline(true))
                .AddField(fb => fb.WithName("😓 Humidity").WithValue($"{obj["humidity"]}%").WithIsInline(true))
                .AddField(fb => fb.WithName("💨 Wind Speed").WithValue($"{obj["windspeedk"]}km/h / {obj["windspeedm"]}mph").WithIsInline(true))
                .AddField(fb => fb.WithName("🌡 Temperature").WithValue($"{obj["centigrade"]}°C / {obj["fahrenheit"]}°F").WithIsInline(true))
                .AddField(fb => fb.WithName("🔆 Feels like").WithValue($"{obj["feelscentigrade"]}°C / {obj["feelsfahrenheit"]}°F").WithIsInline(true))
                .AddField(fb => fb.WithName("🌄 Sunrise").WithValue($"{obj["sunrise"]}").WithIsInline(true))
                .AddField(fb => fb.WithName("🌇 Sunset").WithValue($"{obj["sunset"]}").WithIsInline(true));


            await channel.SendMessageAsync(embed :embed.Build()).ConfigureAwait(false);
        }


    }
}
