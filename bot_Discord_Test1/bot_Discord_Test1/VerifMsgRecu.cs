using bot_Discord_Test1.Modules;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace bot_Discord_Test1
{
    internal class VerifMsgRecu
    {
        public void asyncVerifMsgRecu(string msg) 
        {
            Commands monoutils = new Commands();  
            
            
            switch (msg.ToLower())
            {
                case "stop":
                    monoutils.StopSpam = true;
                    break;
            }
        }

    }
}
