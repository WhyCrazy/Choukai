using Discord;
using Discord.Commands;
using DiscordUtils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Choukai.Modules
{
    public class GameJam : ModuleBase
    {
        [Command("Submit")]
        public async Task Submit(params string[] args)
        {
            await Context.Message.DeleteAsync();
            string[] content = string.Join(" ", args).Split('|');
            if (content.Length != 5)
            {
                await ReplyAsync(Context.User.Mention + " " + Help());
                return;
            }

            // Users
            string[] userString = content[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            IUser[] users = new IUser[userString.Length + 1];
            users[0] = Context.User;
            for (int i = 0; i < userString.Length; i++)
            {
                users[i + 1] = await Utils.GetUser(userString[i], Context.Guild);
                if (users[i + 1] == null)
                {
                    await ReplyAsync(Context.User.Mention + " The user \"" + users[i] + "\"does not exist.");
                    return;
                }
                if (users[i + 1].Id == 509132242824724531)
                {
                    await ReplyAsync(Context.User.Mention + " I don't remember joining your group.");
                    return;
                }
            }
            users = users.Distinct().ToArray();

            // Game Name
            string gameName = content[1].Trim();
            if (gameName == "")
            {
                await ReplyAsync(Context.User.Mention + " Your game name can't be empty.");
                return;
            }
            if (gameName.Length > 256)
            {
                await ReplyAsync(Context.User.Mention + " Game name must be less than 256 characters.");
                return;
            }

            // Game Description
            string description = content[2].Trim();
            if (description == "")
            {
                await ReplyAsync(Context.User.Mention + " Your game description can't be empty");
                return;
            }
            if (description.Length > 2000)
            {
                await ReplyAsync(Context.User.Mention + " Game description must be less than 2000 characters.");
                return;
            }

            // Image link
            string imageLink = content[3].Trim();
            if (imageLink == "" || !Utils.IsLinkValid(imageLink))
            {
                await ReplyAsync(Context.User.Mention + " Your image link isn't valid.");
                return;
            }
            if (imageLink == "" || !Utils.IsImage(imageLink.Split('.').Last()))
            {
                await ReplyAsync(Context.User.Mention + " Your image link isn't a valid format");
                return;
            }

            // Game links
            string[] gameLinks = content[4].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (gameLinks.Length % 2 == 1)
            {
                await ReplyAsync(Context.User.Mention + " Each of your game links must be give with a format.");
                return;
            }
            if (gameLinks.Length > 40)
            {
                await ReplyAsync(Context.User.Mention + " You can't have more than 20 links to your game.");
                return;
            }
            for (int i = 0; i < gameLinks.Length; i += 2)
            {
                if (!Utils.IsLinkValid(gameLinks[i]))
                {
                    await ReplyAsync(Context.User.Mention + " " + gameLinks[i] + " isn't a valid link.");
                    return;
                }
                string format = gameLinks[i + 1];
                if (format != "WebGL" && format != "Windows" && format != "Linux" && format != "Mac" && format != "Android"
                    && format != "IOS" && format != "WindowsPhone" && format != "Other")
                {
                    await ReplyAsync(Context.User.Mention + " " + format + " isn't a valid format.");
                    return;
                }
            }
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = gameName,
                ImageUrl = imageLink,
                Description = description,
                Color = Color.Blue,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Made by " + string.Join(", ", users.Select(x => x.ToString()))
                }
            };
            for (int i = 0; i < gameLinks.Length; i += 2)
                embed.AddField(gameLinks[i + 1], gameLinks[i]);
            await ReplyAsync("", false, embed.Build());
        }

        private string Help()
        {
            return ("Format: user1, user2... | GameName | Description | ImageLink | GameLink1 GameFormat1 GameLink2 GameFormat2" + Environment.NewLine + Environment.NewLine +
                "Users can either be Discord username or user ID" + Environment.NewLine +
                "GameName must be a string containing less than 256 characters" + Environment.NewLine +
                "Description must be a string containing less than 2000 characters" + Environment.NewLine +
                "ImageLink must be an URL to a valid image (must be png, jpg, jpeg or gif format)" + Environment.NewLine +
                "GameLink must be a valid URL to your game (max 20)" + Environment.NewLine +
                "GameFormat must be one of the following: WebGL, Windows, Linux, Mac, Android, IOS, WindowsPhone, Other");
        }
    }
}
