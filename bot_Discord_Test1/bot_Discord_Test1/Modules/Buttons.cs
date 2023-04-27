using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_Discord_Test1.Modules
{
    internal class Buttons
    {
        public async Task Handle(SocketMessageComponent component, Bot client) {
            switch(component.Data.CustomId)
            {
                case "button-previous":
                    this.HandleHelpPrevious(component, client);
                    break;
                case "button-next":
                    this.HandleHelpNext(component,client);
                    break;
            }
        }

        public async void HandleHelpNext(SocketMessageComponent component, Bot client) 
        {
            IUserMessage helpMessage = component.Message;
            int newPage = client.GetCurrentPage(helpMessage) + 1;
            int perPage = 2;
            var commandList = client.CommandService.Commands;

            var embedBuilder = new EmbedBuilder
            {
                Title = "Liste des commandes",
                Color = Color.Green
            };


            int startCommand = newPage * perPage;
            int endCommand = Minimum((newPage + 1) * perPage, commandList.Count());

            for (int i = startCommand; i < endCommand; i++)
            {
                var command = commandList.ElementAt(i);
                embedBuilder.AddField(command.Name, command.Summary);
            }

            // Add a footer to the embed with page information
            embedBuilder.Footer = new EmbedFooterBuilder
            {
                Text = $"Page {newPage + 1}/{(commandList.Count() + perPage - 1) / perPage}"
            };
            
            var builder = new ComponentBuilder()
                .WithButton("⏮️", "button-previous")
                .WithButton("⏭️", "button-next");


            var reply = await helpMessage.Channel.SendMessageAsync(embed: embedBuilder.Build(), components: builder.Build());
            await helpMessage.DeleteAsync();

            await component.DeferAsync();

            client.UnregisterHelpMessage(helpMessage);
            client.RegisterHelpMessage(reply, newPage);
        }

        private int Minimum(int Valeur1, int Valeur2)
        {
            if(Valeur1 < Valeur2)
            {
                return Valeur1;
            }
            else
            {
                return Valeur2;
            }
        }


        public async void HandleHelpPrevious(SocketMessageComponent component, Bot client) 
        {
            IUserMessage helpMessage = component.Message;
            int newPage = client.GetCurrentPage(helpMessage) - 1;
            int perPage = 2;
            var commandList = client.CommandService.Commands;

            var embedBuilder = new EmbedBuilder
            {
                Title = "Liste des commandes",
                Color = Color.Green
            };

            int startCommand = newPage * perPage;
            int endCommand = Minimum((newPage + 1) * perPage, commandList.Count());

            for (int i = startCommand; i < endCommand; i++)
            {
                var command = commandList.ElementAt(i);
                embedBuilder.AddField(command.Name, command.Summary);
            }

            // Ajouter un pied de page à l'embed avec les informations de la page
            embedBuilder.Footer = new EmbedFooterBuilder
            {
                Text = $"Page {newPage + 1}/{(commandList.Count() + perPage - 1) / perPage}"
            };

            // Créer un constructeur de composants avec les boutons suivant et précédent
            var builder = new ComponentBuilder()
                .WithButton("⏮️", "button-previous")
                .WithButton("⏭️", "button-next");

            // Envoyer le message d'aide mis à jour avec les boutons
            var reply = await helpMessage.Channel.SendMessageAsync(embed: embedBuilder.Build(), components: builder.Build());
            await helpMessage.DeleteAsync();

            await component.DeferAsync();

            // Enregistrer le nouveau message d'aide
            client.UnregisterHelpMessage(helpMessage);
            client.RegisterHelpMessage(reply, newPage);
        }
    }
}
