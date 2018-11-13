using Discord.Commands;
using DiscordUtils;
using System.Threading.Tasks;

namespace Choukai.Modules
{
    public class Communication : ModuleBase
    {
        [Command("Help")]
        public async Task Help(params string[] args)
        {

        }

        [Command("BotInfo"), Alias("InfoBot")]
        private async Task BotInfo(params string[] args)
        {
            await ReplyAsync("", false, Utils.GetBotInfo(Program.P.StartTime, "Choukai", Program.P.client.CurrentUser));
        }
    }
}
