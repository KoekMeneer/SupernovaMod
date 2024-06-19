﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
	public class EldrichBolt : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaArcanumExplosionShot}";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.NebulaArcanumExplosionShot);
			Projectile.penetrate = 1;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			int num3;
			int num240 = (int)Projectile.ai[0];
			for (int num241 = 0; num241 < 3; num241 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CorruptionThorns, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 1.2f);
				Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center) / 2f;
				Main.dust[num242].noGravity = true;
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.5f;
				num3 = num241;
			}
			for (int num243 = 0; num243 < 2; num243 = num3 + 1)
			{
				int num242 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CosmicEmber, Projectile.velocity.X, Projectile.velocity.Y, num240, default(Color), 0.4f);
				if (num243 == 0)
				{
					Main.dust[num242].position = (Main.dust[num242].position + Projectile.Center * 5f) / 6f;
				}
				else if (num243 == 1)
				{
					Main.dust[num242].position = (Main.dust[num242].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
				}
				Dust dust2 = Main.dust[num242];
				dust2.velocity *= 0.1f;
				Main.dust[num242].noGravity = true;
				Main.dust[num242].fadeIn = 1f;
				num3 = num243;
			}
		}

		public override void OnKill(int timeLeft)
		{
			int num220 = Main.rand.Next(3, 6);
			for (int num221 = 0; num221 < num220; num221++)
			{
				// Create velocity for angle
				Vector2 value17 = -Vector2
					// Normalize so the velocity ammount of the projectile doesn't matter
					.Normalize(Projectile.velocity)
					// Rotate by angle
					.RotatedBy(MathHelper.ToRadians(360 / num220 * (num221 - 2)))
					// Make the velocity 3 - 6
					* Main.rand.Next(3, 6);

				// Create a projectile for velocity
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.position, value17, ModContent.ProjectileType<EldrichTentacle>(), (int)(Projectile.damage * .75f), 1f, Projectile.owner);
				proj.localNPCHitCooldown *= 2;
			}
		}
	}
}
