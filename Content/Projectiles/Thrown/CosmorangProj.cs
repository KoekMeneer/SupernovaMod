using Microsoft.Xna.Framework;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using System.IO;
using Terraria;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Thrown
{
    public class CosmorangProj : BoomerangProjectile
    {
        private int cacheTravelOutFrames;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 50;

            ReturnSpeed = 32;
            RotationSpeed = .7f;

            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, DustID.t_Flesh, Projectile.velocity.X, Projectile.velocity.Y, Scale: Main.rand.NextFloat(.7f, 1));
            Main.dust[dustID].noGravity = true;

            // Handle boomerang AI
            base.AI();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            writer.Write(cacheTravelOutFrames);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            cacheTravelOutFrames = reader.Read();
        }

        protected override void OnReachedApex()
        {
            // Prevent the projectile from returing
            Projectile.ai[0] = 0;
            cacheTravelOutFrames = TravelOutFrames;
            TravelOutFrames = 0;

            // Stay for x time at the apex before returning
            Projectile.velocity = Vector2.Zero;
            Projectile.ai[2]++;
            if (Projectile.ai[2] > 20)
            {
                TravelOutFrames = cacheTravelOutFrames;
                Projectile.ai[0] = 1; // Set state: return to player
                Projectile.ai[1] = 0;
                Projectile.ai[2] = 0;
                cacheTravelOutFrames = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (cacheTravelOutFrames != 0)
            {
                return; // Already at apex
            }
            Projectile.ai[1] = TravelOutFrames;

            // To reduce worm boss melting damage, reduce the damage each hit, with a cap of 37
            if (Projectile.damage > 37)
            {
                Projectile.damage -= 2;
            }
        }
    }
}
