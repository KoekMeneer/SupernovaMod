﻿using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SupernovaMod.Content.Projectiles.Magic
{
    public class VerglasScepterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Verglas Scepter");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 375;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(36));
            Projectile.velocity = perturbedSpeed;

            // Spawn dust on hit
            for (int i = 0; i <= Main.rand.Next(4, 8); i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(.5f, 1.5f));
                Main.dust[dust].noGravity = true;
            }
            // Ice break sound
            SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
        }

        private int _bounces;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            _bounces++;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            if (_bounces >= 6) return true;
            return false; // return false because we are handling collision
        }

        public override void AI()
        {
            //this is projectile dust
            if (Main.rand.NextBool(2))
            {
                int DustID2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Frost, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default, Main.rand.NextFloat(.85f, 1.35f));
                Main.dust[DustID2].noGravity = true;
            }

            // Add sinewave effect
            const double amp = 5;
            const double freq = .1;
            float sineWave = (float)(Math.Cos(freq * Projectile.timeLeft) * amp * freq) * Projectile.ai[0];
            //projectile.position.Y += sineWave;
            Projectile.velocity.Y += sineWave;
            //projectile.velocity.Y = (float)(1 * projectile.timeLeft + (Math.Cos(freq * projectile.timeLeft) * amp * freq)) * -1;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.60f;

            /*projectile.localAI[0] += 1f;

            // After x times swap the value
            projectile.ai[0]++;
            if (projectile.ai[0] >= 30)
			{
                projectile.ai[0] = 0;
                sineValue = -sineValue;
                if (sineValue == 0) sineValue = .025f;
            }
            // Rotate a small bit
            projectile.velocity = projectile.velocity.RotatedByRandom(sineValue);*/
        }
        public override void Kill(int timeLeft)
        {
            // Spawn dust on hit
            for (int i = 0; i <= Main.rand.Next(10, 20); i++)
                Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Ice, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, default, Main.rand.NextFloat(.5f, 1.5f));
            // Ice break sound
            SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
            base.Kill(timeLeft);
        }
    }
}
