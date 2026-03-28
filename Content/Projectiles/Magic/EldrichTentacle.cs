using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Magic
{
	public class EldrichTentacle : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.NebulaArcanumExplosionShot}";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ShadowFlame);
			Projectile.penetrate = 2;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
        }

		public override void AI()
        {
            // Life progression
            Projectile.localAI[0] += (Projectile.localAI[0] < 0.1f) ? 0.01f : 0.025f;
            if (Projectile.localAI[0] >= 0.95f)
            {
                Projectile.Kill();
                return;
            }

            // Scale & center
            Projectile.scale = 1f - Projectile.localAI[0];
            Vector2 center = Projectile.Center;
            Projectile.width = (int)(20f * Projectile.scale);
            Projectile.height = Projectile.width;
            Projectile.position = center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);

            // Initialize rotation offset once
            if (Projectile.ai[2] == 0)
            {
                Projectile.ai[2] = Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);
            }

            // Add random wriggling to velocity
            float wiggleAmount = Main.rand.NextFloat(.25f, .3f); // adjust for more/less tentacle wriggle
            Projectile.velocity += new Vector2(Main.rand.NextFloat(-wiggleAmount, wiggleAmount),
                                               Main.rand.NextFloat(-wiggleAmount, wiggleAmount));

            // Slightly curve motion over time
            Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[2] / 50f);
            Projectile.ai[2] *= 1.02f;

            // Clamp max speed
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;
            }

            // Direction vectors
            Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Vector2 right = forward.RotatedBy(MathHelper.PiOver2);

            // Tentacle length & thickness scale with projectile
            float length = Projectile.scale * 30f;
            float thickness = Projectile.scale * 6f;

            // How many segments along the tentacle
            int segments = (int)(Projectile.scale * 12f);

            for (int i = 0; i < segments; i++)
            {
                float progress = i / (float)segments;

                // Position along the tentacle
                Vector2 basePos = Projectile.Center - forward * progress * length;

                // Organic wiggle
                float wiggle = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 10f + i) * thickness * 0.3f;

                // === CENTER CORE (bright, fleshy) ===
                Vector2 centerPos = basePos + right * wiggle;
                int coreDust = Dust.NewDust(centerPos, 0, 0, DustID.Blood, 0f, 0f, 0, default, Projectile.scale * 1.2f);
                Main.dust[coreDust].noGravity = true;
                Main.dust[coreDust].velocity = -forward * 0.5f;

                // === OUTLINE LEFT ===
                Vector2 leftPos = basePos + right * (thickness + wiggle);
                int leftDust = Dust.NewDust(leftPos, 0, 0, DustID.CrimsonTorch, 0f, 0f, 0, default, Projectile.scale);
                Main.dust[leftDust].noGravity = true;
                Main.dust[leftDust].velocity = -forward * 0.3f;

                // === OUTLINE RIGHT ===
                Vector2 rightPos = basePos - right * (thickness - wiggle);
                int rightDust = Dust.NewDust(rightPos, 0, 0, DustID.CrimsonTorch, 0f, 0f, 0, default, Projectile.scale);
                Main.dust[rightDust].noGravity = true;
                Main.dust[rightDust].velocity = -forward * 0.3f;
            }

            // Rotation for visual tentacle curl
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
