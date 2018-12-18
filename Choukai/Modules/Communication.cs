using Discord;
using Discord.Commands;
using DiscordUtils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Choukai.Modules
{
    public class Communication : ModuleBase
    {
        [Command("Help")]
        public async Task Help(params string[] args)
        {
            await ReplyAsync("Member: Join Confiture as a member." + Environment.NewLine +
                "BotInfo: Gives information about the bot");
        }

        [Command("BotInfo"), Alias("InfoBot")]
        private async Task BotInfo(params string[] args)
        {
            await ReplyAsync("", false, Utils.GetBotInfo(Program.P.StartTime, "Choukai", Program.P.client.CurrentUser));
        }

        [Command("Member")]
        private async Task Member(params string[] args)
        {
            if (Context.Channel.Id != 514909469541531669) // Public General
                await ReplyAsync("Please request this in <#514909469541531669>.");
            else if (Context.Channel as ITextChannel == null)
                await ReplyAsync("You can't request this by private message.");
            else
            {
                IGuildUser user = (IGuildUser)Context.User;
                if (user.RoleIds.ToList().Contains(511569693807607830)) // Member
                    await ReplyAsync("You are already a member.");
                else
                {
                    await user.AddRoleAsync(Context.Guild.GetRole(511569693807607830));
                    await ReplyAsync("<@" + Context.User.Id +"> You are now a member, welcome to Confiture.");
                }
            }
        }
    }
}
