using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class Cosmorang : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmorang");
        }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 3;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.magic = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 900;
            projectile.extraUpdates = 1;
        }
		public override void AI()
		{
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.PinkFlame, projectile.velocity.X * 1.5f, projectile.velocity.Y * 1.5f, 40, default(Color), .7f);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
            Main.dust[dust].noGravity = false; //this make so the dust has no gravity
            base.AI();
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int radius = 10;     //this is the explosion radius, the highter is the value the bigger is the explosion
            for (int x = -radius; x <= radius; x++)
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width * 2, projectile.height * 2, DustID.PinkFlame, projectile.velocity.X * -0.6f, projectile.velocity.Y * 1.2f, 80, default(Color), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.7f;
            }
        }
    }
}
