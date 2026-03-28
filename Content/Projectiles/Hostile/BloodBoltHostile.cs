using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;

namespace SupernovaMod.Content.Projectiles.Hostile
{
    public class BloodBoltHostile : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.light = 0.5f;
			Projectile.timeLeft = 240;
		}

		public override void AI()
		{
			// Make the projectile face the right way
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(90);

            // Subtle wobble
            Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(Projectile.timeLeft * 0.15f) * 0.01f);

            // Blood dust (main)
            if (Main.rand.NextBool(2))
            {
                int dustId = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<Dusts.BloodDust>(),
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    30,
                    default,
                    Main.rand.NextFloat(0.8f, 1.3f)
                );

                Main.dust[dustId].noGravity = true;
            }

            // Occasional brighter crimson spark
            if (Main.rand.NextBool(5))
            {
                int dustId = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.CrimsonTorch,
                    0f,
                    0f,
                    100,
                    default,
                    1.2f
                );

                Main.dust[dustId].noGravity = true;
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = new Color(255, 180, 180, 245);
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.oldPos[6] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - 6) / (float)Projectile.oldPos.Length);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return true;
		}
	}
}
