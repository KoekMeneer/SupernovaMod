using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Typeless
{
    public class ThornedRingProj : ModProjectile
    {
        public static int BuffType
        {
            get
            {
                if (_buffType == null)
                {
                    _buffType = ModContent.BuffType<Buffs.Rings.ThornedRingBuff>();
                }
                return _buffType.Value;
            }
        }
        private static int? _buffType;

        public override void SetDefaults()
        {
            Projectile.width = 236;
            Projectile.height = 236;
            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default; 
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 28;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.ForestGreen;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            CheckActive(owner);

            Projectile.Center = owner.Center;
            Projectile.rotation += .05f;

            if (Main.rand.NextBool(2))
            {
                int DustID2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JunglePlants, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 20, default, Utils.NextFloat(Main.rand, 0.75f, 1.5f));
                Main.dust[DustID2].noGravity = true;
            }
        }

        protected virtual bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                if (BuffType != -1) owner.ClearBuff(BuffType);
            }

            if (BuffType == -1 || owner.HasBuff(BuffType))
            {
                Projectile.timeLeft = 2;
                return true;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
            {
                target.AddBuff(BuffID.Poisoned, 210);
            }
        }
    }
}
