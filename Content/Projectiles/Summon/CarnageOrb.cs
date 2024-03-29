﻿using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SupernovaMod.Common.Players;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class CarnageOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carnage Orb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            //Projectile.minion = true;
            //Projectile.minionSlots = 0;
            Projectile.DamageType = DamageClass.Default;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
        }

        /*public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.ModProjectile != null)
                return projectile.ModProjectile.OnTileCollide(oldVelocity);
            return true;
        }*/

        public override void AI()
        {
            CheckActive();

            int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width - 10, Projectile.height - 10, ModContent.DustType<Dusts.BloodDust>(), Projectile.velocity.X * 20, Projectile.velocity.Y * 20, 70, default);
            Main.dust[DustID2].noGravity = true;

            #region orbit
            //Making player variable "p" set as the projectile's owner
            Player p = Main.player[Projectile.owner];
            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<CarnageOrb>()] > 1)
            {
                Projectile.timeLeft = 0;
            }

            //Factors for calculations
            double deg = Projectile.ai[1]; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
            double rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 90; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
            Projectile.position.X = p.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = p.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
            Projectile.ai[1] += 3;
            #endregion

            Projectile.rotation = (float)rad;
        }
        public void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();
            if (player.dead)
            {
                modPlayer.hasMinionCarnageOrb = false;
            }
            if (modPlayer.hasMinionCarnageOrb)
            {
                Projectile.timeLeft = 2;
            }
            if (!player.HasBuff(ModContent.BuffType<Buffs.Minion.CarnageOrbBuff>()))
            {
                modPlayer.hasMinionCarnageOrb = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X, Projectile.velocity.Y, Scale: 1.5f);
            }
            base.Kill(timeLeft);
        }
    }
}
