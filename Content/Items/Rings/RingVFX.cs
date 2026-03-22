using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace SupernovaMod.Content.Items.Rings
{
    public static class RingVFX
    {
        public static void DustCircle(Vector2 center, int dustType, float radius, int amount, float inwardSpeed = 0f, float scale = 1f)
        {
            for (int i = 0; i < amount; i++)
            {
                float rot = MathHelper.TwoPi * i / amount;
                Vector2 pos = center + new Vector2(radius, 0).RotatedBy(rot);

                Vector2 vel = inwardSpeed != 0f
                    ? (center - pos).SafeNormalize(Vector2.Zero) * inwardSpeed
                    : Vector2.Zero;

                Dust d = Dust.NewDustPerfect(pos, dustType, vel, Scale: scale);
                d.noGravity = true;
            }
        }

        public static void DustBurst(Vector2 center, int dustType, int amount, float speed = 4f, float scale = 1.2f)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(speed, speed);
                Dust d = Dust.NewDustPerfect(center, dustType, vel, Scale: scale);
                d.noGravity = true;
            }
        }

        public static void DustOrbit(ref float rot, Vector2 center, int dustType, float radius, float speed = 0.1f)
        {
            rot += speed;
            Vector2 pos = center + new Vector2(radius, 0).RotatedBy(rot);

            Dust d = Dust.NewDustPerfect(pos, dustType, Vector2.Zero);
            d.noGravity = true;
        }

        /// <summary>
        /// Plays the standard charge sound for rings.
        /// </summary>
        public static void PlayChargeSound(Vector2 pos)
        {
            SoundEngine.PlaySound(SoundID.Item15 with { Volume = 0.3f }, pos);
        }

        public static void DirectionalBurst(Vector2 center, Vector2 direction, int dustType, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 vel = direction.RotatedByRandom(0.5f) * Main.rand.NextFloat(2f, 6f);
                Dust.NewDustPerfect(center, dustType, vel).noGravity = true;
            }
        }
    }
}
