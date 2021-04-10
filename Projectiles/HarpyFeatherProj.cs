using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Projectiles
{
    public class HarpyFeatherProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.width = 24;
            projectile.height = 24;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            aiType = 1;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.80f;
            projectile.localAI[0] += 1f;
        }
    }
}
