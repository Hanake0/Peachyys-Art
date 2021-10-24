using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using Microsoft.Extensions.Logging;

using Peachyys.Configs;

namespace Peachyys.Commands {
	public class InfosModule : BaseCommandModule {
		public Bot Peachyys { private get; set; }

		public InfosModule(Bot peachyys) {
			peachyys.Client.ComponentInteractionCreated += this.OnButtonInteraction;
		}

		private async Task OnButtonInteraction(DiscordClient client, ComponentInteractionCreateEventArgs args)
			=> await Task.Run(async () => {
				string[] actions = args.Id.Split("_");
				
				Task interactionTask = actions[0] switch {
					"artinfo" => this.OnArtInfoInteraction(args, actions),
					_ => Task.CompletedTask
				};

				await interactionTask;
			});

		#region Art Info Command
		[Command("artinfo"), Aliases("art")]
		public async Task ArtInfoAsync(CommandContext ctx) {
			await ctx.TriggerTypingAsync();

			await ctx.Channel.SendMessageAsync(this.GetArtInfoMain());
		}

		private async Task OnArtInfoInteraction(ComponentInteractionCreateEventArgs args, string[] actions) {
			//await args.Channel.TriggerTypingAsync();
			
			// Caso seja o dropdown de seleção
			if (actions[1] == "main") {
				await args.Interaction.CreateResponseAsync(
					InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(
						this.GetArtInfoStyle(args.Values[0], "0")));
				
			// Caso seja o botão de voltar
			} else if (actions[1] == "goto") {
				await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
					new DiscordInteractionResponseBuilder(
						this.GetArtInfoMain()));
				
			// Caso seja uma opção de navegação
			} else {
				await args.Interaction.CreateResponseAsync(
					InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(
						this.GetArtInfoStyle(actions[1], actions[2])));
			}
		}
		private DiscordMessageBuilder GetArtInfoMain() {
			DiscordEmbedBuilder builder = new DiscordEmbedBuilder()
				.WithTitle("This is my portifolio!")
				.WithDescription("Please don't repost my art without credit ♡\n\n" +
				                 "you can acess my Carrd or you can\n" +
				                 "select bellow the art style to see some examples:");
			
			// Create the options for the user to pick
			List<DiscordSelectComponentOption> options = ArtInfoConfig.ArtStyles
				.Select(style => new DiscordSelectComponentOption(
					label: style.Name,
					value: style.Name,
					description: style.Description,
					isDefault: false,
					emoji: style.Emoji))
				.ToList();

			// Make the dropdown
			DiscordSelectComponent dropdown = new("artinfo_main", null, options);
			
			return new DiscordMessageBuilder()
				.WithEmbed(builder)
				.AddComponents(dropdown)
				.AddComponents(new DiscordLinkButtonComponent("https://peachyyyys.carrd.co/", "My Carrd"));
		}

		private DiscordMessageBuilder GetArtInfoStyle(string style, string position) {
			int pos = int.Parse(position);
			ArtInfoConfig.ArtStyle sty = ArtInfoConfig.ArtStyles.Single(s => s.Name == style);
			string[] urls = sty.Urls;
			
			DiscordEmbedBuilder builder = new DiscordEmbedBuilder()
				.WithAuthor(sty.Name)
				.WithDescription(sty.Description)
				.WithImageUrl(urls[pos]);

			return new DiscordMessageBuilder()
				.WithEmbed(builder)
				.AddComponents(
					new DiscordButtonComponent(ButtonStyle.Primary, $"artinfo_{style}_{pos - 1}",
						null, pos == 0, new DiscordComponentEmoji("⬅")),
					new DiscordButtonComponent(ButtonStyle.Secondary, $"artinfo_goto_main",
						null, false, new DiscordComponentEmoji("↪")),
					new DiscordLinkButtonComponent("https://peachyyyys.carrd.co/", "My Carrd"),
					new DiscordButtonComponent(ButtonStyle.Primary, $"artinfo_{style}_{pos + 1}",
						null, urls.Length == (pos+1), new DiscordComponentEmoji("➡")));
		}
		#endregion
	}
}