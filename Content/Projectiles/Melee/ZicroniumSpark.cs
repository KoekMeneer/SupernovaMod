﻿using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Melee
{
    public class ZicroniumSpark : ModProjectile
    {
        public override string Texture => "SupernovaMod/Assets/Textures/InvisibleProjectile";
        //protected virtual int DustId => DustID.UnusedWhiteBluePurple;
        protected int dustId = ModContent.DustType<Dusts.ZirconDust>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zicronium Spark");
        }
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.1f;
            }
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y && Projectile.velocity.Y > 1f)
            {
                Projectile.velocity.Y = Projectile.velocity.Y * -0.5f;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            int num199 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustId, 0f, 0f, 100, default, 1f);
            Dust dust = Main.dust[num199];
            dust.position.X = dust.position.X - 2f;
            Dust dust2 = Main.dust[num199];
            dust2.position.Y = dust2.position.Y + 2f;
            Main.dust[num199].scale += Main.rand.Next(50) * 0.01f;
            Main.dust[num199].noGravity = true;
            Dust dust3 = Main.dust[num199];
            dust3.velocity.Y = dust3.velocity.Y - 2f;
            if (Main.rand.NextBool(2))
            {
                int num200 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustId, 0f, 0f, 100, default, 1f);
                Dust dust4 = Main.dust[num200];
                dust4.position.X = dust4.position.X - 2f;
                Dust dust5 = Main.dust[num200];
                dust5.position.Y = dust5.position.Y + 2f;
                Main.dust[num200].scale += 0.3f + Main.rand.Next(50) * 0.01f;
                Main.dust[num200].noGravity = true;
                Main.dust[num200].velocity *= 0.1f;
            }
            if (Projectile.velocity.Y < 0.25 && Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X = Projectile.velocity.X * 0.8f;
            }
            Projectile.rotation = -Projectile.velocity.X * 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			ArmorPlayer player = Main.player[Projectile.owner].GetModPlayer<ArmorPlayer>();
            player.ZirconiumArmor_ModifyHitNPC(target, ref modifiers);
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
    }
}

