using Microsoft.Xna.Framework;
using SupernovaMod.Core.Helpers;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class EldrichBolt : ModProjectile
    {
        private const float KILL_WHEN_BELOW_SPEED = 3;

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
            // Setup
            //
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = Main.rand.NextFloat(0.8f, 1.2f); // wiggle speed
                Projectile.ai[1] = Main.rand.NextFloat(0.15f, 0.25f); // wiggle strength
            }

            Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Vector2 right = forward.RotatedBy(MathHelper.PiOver2);

            float time = Main.GlobalTimeWrappedHourly * 10f * Projectile.ai[0];
            float wiggle = (float)Math.Sin(time + Projectile.whoAmI) * Projectile.ai[1];

            Projectile.velocity += right * wiggle;

            // Slowdown the projectile over time
            Projectile.velocity *= 0.99f;

            float speed = Projectile.velocity.Length();

            // Kill when almost stopped
            //
            if (speed < KILL_WHEN_BELOW_SPEED)
            {
                Projectile.Kill();
                return;
            }

            if (speed > 0.01f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }

            AI_SoftHoming();
            AI_DrawTrail(time, forward, right);
        }

        private void AI_SoftHoming()
        {
            float homingRange = 100;
            float homingStrength = 0.12f;

            NPC? target = null;
            float closestDist = homingRange;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.CanBeChasedBy(this))
                {
                    continue;
                }

                float dist = Vector2.Distance(Projectile.Center, npc.Center);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    target = npc;
                }
            }

            if (target != null)
            {
                Vector2 desiredDirection = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

                // Blend toward target instead of snapping
                Projectile.velocity = Vector2.Lerp(
                    Projectile.velocity,
                    desiredDirection * Projectile.velocity.Length(),
                    homingStrength
                );
            }
        }

        private void AI_DrawTrail(float time, Vector2 forward, Vector2 right)
        {
            // Create dust trail

            float length = 18f;
            float thickness = 3.5f;
            int segments = 4;          // keep light (performance)

            for (int i = 0; i < segments; i++)
            {
                float progress = i / (float)segments;

                // Position along trail (behind projectile)
                Vector2 basePos = Projectile.Center - forward * progress * length;

                // Wiggle along body
                float segmentWiggle = (float)Math.Sin(time + i) * thickness * 0.5f * (1f - progress);

                // Center dust
                Vector2 centerPos = basePos + right * segmentWiggle;

                int core = Dust.NewDust(centerPos, 0, 0, DustID.Blood);
                Dust d1 = Main.dust[core];

                d1.noGravity = true;
                d1.scale = 1.1f * (1f - progress);
                d1.velocity = -forward * 1.2f;

                // Outline dust ( less dark red outline )
                float edgeOffset = thickness * (1f - progress);

                Vector2 leftPos = centerPos + right * edgeOffset;
                Vector2 rightPos = centerPos - right * edgeOffset;

                int dL = Dust.NewDust(leftPos, 0, 0, DustID.CrimsonTorch);
                int dR = Dust.NewDust(rightPos, 0, 0, DustID.CrimsonTorch);

                Dust dl = Main.dust[dL];
                Dust dr = Main.dust[dR];

                dl.noGravity = true;
                dr.noGravity = true;

                dl.scale = 0.9f * (1f - progress);
                dr.scale = 0.9f * (1f - progress);

                dl.velocity = -forward * 0.8f;
                dr.velocity = -forward * 0.8f;
            }

            // Projectile dust
            //
            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.LifeDrain);
                Dust dust = Main.dust[d];

                dust.noGravity = true;
                dust.scale = 1.3f;
                dust.velocity = -forward * 2f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // When killed because the projectile is slow,
            // don't spawn tentacles.
            //
            float speed = Projectile.velocity.Length();
            if (speed < KILL_WHEN_BELOW_SPEED)
            {
                return;
            }

            SoundEngine.PlaySound(SoundID.NPCDeath19, Projectile.Center);

            int type = ModContent.ProjectileType<EldrichTentacle>();
            int damage = (int)(Projectile.damage * 0.6f);

            int[] spawnedProjectiles;

            if (Main.rand.NextBool())
            {
                // Cross pattern
                spawnedProjectiles = ProjectileHelper.ShootCrossPattern(
                    Projectile.GetSource_FromAI(),
                    Projectile.position,
                    Main.rand.Next(3, 5),
                    Main.rand.Next(3, 6),
                    type,
                    damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
            else
            {
                // Plus pattern
                spawnedProjectiles = ProjectileHelper.ShootPlusPattern(
                    Projectile.GetSource_FromAI(),
                    Projectile.position,
                    Main.rand.Next(3, 5),
                    Main.rand.Next(3, 6),
                    type,
                    damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            // Make spawned tentacles hit less frequently (balance)
            foreach (int projId in spawnedProjectiles)
            {
                Main.projectile[projId].localNPCHitCooldown *= 2;
            }
        }
    }
}
