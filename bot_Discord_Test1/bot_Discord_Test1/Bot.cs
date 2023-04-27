using Discord.WebSocket;
using Discord.Commands;
using System.Windows.Forms;
using System.Collections.Generic;
using Discord;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bot_Discord_Test1
{
    public class Bot : DiscordSocketClient
    {


        private CommandService _commands;

        private List<Tuple<int, IUserMessage>> _helpMessages = new List<Tuple<int, IUserMessage>>();

        public Bot(DiscordSocketConfig config) : base(config)
        {
            _commands = new CommandService();        
        }

        public CommandService CommandService {
            get 
            {
                return _commands; 
            } 
        }

        public void RegisterHelpMessage(IUserMessage message, int page = 0)
        {
            _helpMessages.Add(new Tuple<int, IUserMessage>(page, message));
        }

        public void UnregisterHelpMessage(IUserMessage message)
        {
            _helpMessages.Remove(new Tuple<int, IUserMessage>(0, message));
        }

        public void ClearHelpMessages() 
        {
            _helpMessages.Clear();
        }
        public bool HelpMessageExists(IUserMessage message) 
        {
            Tuple<int, IUserMessage> page = _helpMessages.Find((tuple) => tuple.Item2 == message);
            if (page == null)
            {
                return false;
            }

            return true;

        }

        public int GetCurrentPage(IUserMessage message) 
        {

            Tuple<int, IUserMessage> page = _helpMessages.Find((tuple) => tuple.Item2 == message);

            if(page == null)
            {
                return 0;
            }

            return page.Item1;


        }
    }
}
