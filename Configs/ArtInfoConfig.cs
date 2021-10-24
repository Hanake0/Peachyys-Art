using DSharpPlus.Entities;

namespace Peachyys.Configs {
	public static class ArtInfoConfig {
		public static readonly ArtStyle[] ArtStyles = {
			new ArtStyle {
				Name = "Anime",
				Emoji = new DiscordComponentEmoji("🖌️"),
				Description = "Anime-like drawings",
				Urls = new[] {
					"https://peachyyyys.carrd.co/assets/images/image05.jpg?v=dcf17a8d",
					"https://peachyyyys.carrd.co/assets/images/image06.jpg?v=dcf17a8d",
				},
			},
			new ArtStyle {
				Name = "Semi-realistic",
				Emoji = new DiscordComponentEmoji("🖋️"),
				Description = "Semi-realist more datailed drawings",
				Urls = new [] {
					"https://peachyyyys.carrd.co/assets/images/image02.jpg?v=dcf17a8d",
					"https://peachyyyys.carrd.co/assets/images/image03.jpg?v=dcf17a8d",
					"https://peachyyyys.carrd.co/assets/images/image04.jpg?v=dcf17a8d",
				}
			}
		};

		public struct ArtStyle {
			public string Name;
			public DiscordComponentEmoji Emoji;
			public string Description;
			public string[] Urls;
		}
	}
}