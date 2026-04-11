using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace SupernovaMod.Content.Npcs.FlyingTerror.Projectiles
{
    public class TerrorBlast : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.light = 0.5f;
			Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void AI()
		{
            // Handle collision behavior dynamically
            if (Projectile.localAI[0] < 10)
            {
                Projectile.tileCollide = false; // Disable collision for initial movement
                Projectile.localAI[0]++;
            }
            else
            {
                Projectile.tileCollide = true;  // Allow tile collision after a short time
            }

            // Add flickering light effect for a more dynamic flame appearance
            Lighting.AddLight(Projectile.Center,
                (255 - Projectile.alpha) * 0.2f / 255f,
                (255 - Projectile.alpha) * 0.55f / 255f,
                (255 - Projectile.alpha) * 0.1f / 255f);

            // Add terror dust
            int dust1 = Dust.NewDust(Projectile.position, Projectile.width + 2, Projectile.height + 2,
                ModContent.DustType<Dusts.TerrorDust>(),
                Projectile.velocity.X * 0.45f, Projectile.velocity.Y * 0.45f, 80, default, 2);
            Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width + 2, Projectile.height + 2, DustID.Shadowflame,
                Projectile.velocity.X * 0.45f, Projectile.velocity.Y * 0.45f, 80, default);
            dust2.noGravity = true;

            // Add variation to dust scale for more dynamic appearance
            Main.dust[dust1].scale = 1.5f + Main.rand.NextFloat(0.5f);  // More variety
            Main.dust[dust2.dustIndex].scale = 1.5f + Main.rand.NextFloat(0.5f);

            // Add slight movement behavior to simulate fire movement
            Projectile.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);  // Slight vertical sway
        }

		public override void OnKill(int timeLeft)
		{
			if (Projectile.ai[0] == 1)
			{
				return; // Prevent recursive explosion
            }

			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			//if (Projectile.owner == Main.myPlayer)
			{
				int numProjectiles = 14;
				for (int i = 0; i < numProjectiles; i++)
				{
					// Create velocity for angle
					Vector2 value17 = -Vector2
						// Normalize so the velocity amount of the projectile doesn't matter
						.Normalize(Projectile.velocity)
						// Rotate by angle
						.RotatedBy(MathHelper.ToRadians(360 / numProjectiles * (i - 2)))
						// Make the velocity 6
						* 6;

					// Create a projectile for velocity
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, value17.X, value17.Y, ModContent.ProjectileType<TerrorBlast>(), Projectile.damage, 1f, Projectile.owner, ai0: 1, ai1: Main.rand.Next(-45, 1));
				}
			}
		}
	}
}
