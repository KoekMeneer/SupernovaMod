﻿using Terraria.Audio;
using Terraria.ID;
using Terraria;

namespace SupernovaMod.Core.Helpers
{
	public static class TerrariaRandom
	{
		public static short NextDustIDCrystalShard()
		{
			switch (Main.rand.Next(3))
			{
				default:
					return DustID.BlueCrystalShard;
				case 1:
					return DustID.PinkCrystalShard;
				case 2:
					return DustID.PurpleCrystalShard;
			}
		}
		/// <summary>
		/// Gets a random <see cref="SoundStyle"/>, between <see cref="SoundID.Item48"/> and <see cref="SoundID.Item50"/> (Ice struck sounds).
		/// </summary>
		/// <returns></returns>
		public static SoundStyle NextSoundIceStruck()
		{
			switch (Main.rand.Next(3))
			{
				default:
					return SoundID.Item48;
				case 1:
					return SoundID.Item49;
				case 2:
					return SoundID.Item50;
			}
		}
		/// <summary>
		/// Gets a random <see cref="ProjectileID"/>, between <see cref="ProjectileID.MolotovFire"/> and <see cref="ProjectileID.MolotovFire3"/>.
		/// </summary>
		/// <returns></returns>
		public static short NextProjectileIDMolotovFire()
		{
			switch (Main.rand.Next(3))
			{
				default:
					return ProjectileID.MolotovFire;
				case 1:
					return ProjectileID.MolotovFire2;
				case 2:
					return ProjectileID.MolotovFire3;
			}
		}
		/// <summary>
		/// Gets a random <see cref="ProjectileID"/>, between <see cref="ProjectileID.MolotovFire"/> and <see cref="ProjectileID.MolotovFire3"/>.
		/// </summary>
		/// <returns></returns>
		public static short NextProjectileIDAnyStar()
		{
			switch (Main.rand.Next(4))
			{
				default:
					return ProjectileID.HallowStar;
				case 1:
					return ProjectileID.StarCannonStar;
				case 2:
					return ProjectileID.StarCloakStar;
				case 3:
					return ProjectileID.StarVeilStar;
			}
		}
		public static short NextProjectileIDMeteor()
		{
			switch (Main.rand.Next(3))
			{
				default:
					return ProjectileID.Meteor1;
				case 1:
					return ProjectileID.Meteor2;
				case 2:
					return ProjectileID.Meteor3;
			}
		}
	}
}
