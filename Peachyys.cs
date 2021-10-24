using System;
using DSharpPlus;

using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

using DSharpPlus.Interactivity.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Peachyys {
	public class Bot {
		// Client e extensões
		public DiscordClient Client { get; private set; }
		public InteractivityExtension Interactivity { get; private set; }
		public CommandsNextExtension Commands { get; private set; }

		public async Task RunAsync() {

			// Configurações do client
			this.Client = new DiscordClient(new DiscordConfiguration {
				Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
				TokenType = TokenType.Bot,
				AutoReconnect = true,
				MinimumLogLevel = LogLevel.Information,

			});

			// Depedency Injection
			ServiceProvider services = new ServiceCollection()
				.AddSingleton(this)
				.BuildServiceProvider();
			
			// Configurações do command handler
			this.Commands = this.Client.UseCommandsNext(new CommandsNextConfiguration {
				StringPrefixes = new[] { Environment.GetEnvironmentVariable("PREFIX") },
				EnableMentionPrefix = true,
				EnableDms = true,
				CaseSensitive = false,
				IgnoreExtraArguments = true,
				Services = services,
			});

			this.Interactivity = this.Client.UseInteractivity(new InteractivityConfiguration {
				Timeout = TimeSpan.FromMinutes(1),
			});

			// Registra os eventos
			this.Client.Ready += this.OnReady;

			// Registra as classes de comandos
			this.Commands.RegisterCommands<Commands.InfosModule>();

			await this.Client.ConnectAsync();
			await Task.Delay(-1);
		}

		private Task OnReady(DiscordClient client, ReadyEventArgs args) {
			DiscordUser user = this.Client.CurrentUser;

			this.Client.Logger
				.LogInformation(LoggerEvents.Startup,
					$"Logado como {user.Username}#{user.Discriminator}" +
					$"({user.Id}) com sucesso!");

			return Task.CompletedTask;
		}
	}
}
