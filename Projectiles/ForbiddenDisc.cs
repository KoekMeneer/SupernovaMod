using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Supernova.Projectiles
{
    public class ForbiddenDisc : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forbidden Disc");

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
            projectile.timeLeft = 780;
            projectile.extraUpdates = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(Main.rand.Next(9) == 0)
            {
                Vector2 position = projectile.Center;

                int Type = mod.ProjectileType("CactusBoomerangProj");
                int Type2 = mod.ProjectileType("DiscOfTheDesertProj");
                const int ShootDirection = 4;
                int ShootDamage = 38;
                float ShootKnockback = 1.2f;

                Projectile.NewProjectile(position.X, position.Y, -ShootDirection * 1.6f, 0, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection * 1.6f, 0, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, ShootDirection * 1.6f, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, 0, -ShootDirection * 1.6f, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection, -ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection, -ShootDirection, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, -ShootDirection, ShootDirection, Type2, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(position.X, position.Y, ShootDirection, ShootDirection, Type, ShootDamage, ShootKnockback, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
