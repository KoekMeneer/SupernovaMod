using log4net;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SupernovaMod.Common.Systems;
using SupernovaMod.Api.Effects;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SupernovaMod
{
    public sealed partial class Supernova : Mod
	{
		public static Supernova Instance { get ; private set; }

		public static ILog Log { get; private set; }

		public static bool DebugMode => false;//ModContent.GetInstance<Common.Configs.SupernovaModConfig>().debugMode;

		private SupernovaModCalls _calls = new SupernovaModCalls();

		#region Other Mods

		public Mod? supernovaMusic;
		public Mod? calamity;
		public Mod? thorium;
		public Mod? bossChecklist;
        /// <summary>
        /// https://steamcommunity.com/sharedfiles/filedetails/?id=2832487441
        /// </summary>
        public Mod? wikithis;

		[MemberNotNullWhen(true, nameof(supernovaMusic))]
		public bool HasModSupernovaMusic => supernovaMusic != null;
        [MemberNotNullWhen(true, nameof(calamity))]
        public bool HasModCalamity => calamity != null;
        [MemberNotNullWhen(true, nameof(thorium))]
        public bool HasModThorium => thorium != null;
        [MemberNotNullWhen(true, nameof(bossChecklist))]
        public bool HasModBossChecklist => bossChecklist != null;
        [MemberNotNullWhen(true, nameof(wikithis))]
        public bool HasWikiThis => wikithis != null;

		#endregion

		public override object Call(params object[] args) => _calls.Call(args);

		public override void Load()
		{
			Instance = this;
			Log = Logger;

			// Set our global defaults
			//
			GlobalModifiers.SetStaticDefaults();

			// Try load other mods
			//
			supernovaMusic = null;
			ModLoader.TryGetMod("SupernovaMusic", out supernovaMusic);
			calamity = null;
			ModLoader.TryGetMod("CalamityMod", out calamity);
			ModIntegrationsSystem.HandleIntegrationRogueCalamity();
			thorium = null;
			ModLoader.TryGetMod("ThoriumMod", out calamity);
			bossChecklist = null;
			ModLoader.TryGetMod("BossChecklist", out bossChecklist);
			wikithis = null;
            ModLoader.TryGetMod("Wikithis", out wikithis);

            //
            if (!Main.dedServ)
            {
				// Load systems
				SupernovaModShaders.LoadEffects();
				SupernovaModTextures.LoadTextures();
                ParticleSystem.Load();
			}
		}

		public override void Unload()
		{
			// Unload other mod variables
			//
			supernovaMusic = null;
			calamity = null;
			thorium = null;
			bossChecklist = null;
			wikithis = null;

            // Unload systems
            SupernovaModShaders.UnloadEffects();
			ParticleSystem.Unload();
		}

		/// <summary>
		/// Gets a song from the Supernova Music mod
		/// </summary>
		/// <param name="songFile"></param>
		/// <returns></returns>
		public int? GetMusicFromMusicMod(string songFile)
		{
			if (!HasModSupernovaMusic)
			{
				return null;
			}
			try
			{
                return new int?(MusicLoader.GetMusicSlot(supernovaMusic, "Assets/Music/" + songFile));
            }
			catch (Exception ex)
			{
				Logger.Error("Failed to load music '" + songFile + "' | Exception: " + ex.ToString());
				return null;
			}
		}

		#region Statics
		public static string GetEffectPath(string effectName) => $"SupernovaMod/Assets/Effects/{effectName}";
		public static string GetTexturePath(string textureName) => $"SupernovaMod/Assets/Textures/{textureName}";
		#endregion
	}
}