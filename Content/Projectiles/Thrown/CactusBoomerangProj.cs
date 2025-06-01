using SupernovaMod.Common.Systems;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class CactusBoomerangProj : BoomerangProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 24;
            Projectile.height = 38;
        }
        public override void AI()
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.t_Cactus, Projectile.velocity.X, Projectile.velocity.Y, Scale: Main.rand.NextFloat(.7f, 1));
            Main.dust[dustID].noGravity = true;

            // Handle boomerang AI
            base.AI();
        }
    }
}
