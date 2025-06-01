﻿using Microsoft.Xna.Framework;
using SupernovaMod.Common.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.BaseProjectiles
{
    // Based on: https://github.com/stormytuna/Bangarang/blob/main/Content/Projectiles/Weapons/Boomerang.cs
    /// <summary>
    /// Provides a base projectile AI for boomerang projectiles.
    /// </summary>
    public abstract class BoomerangProjectile : ModProjectile
    {
        /// <summary>
        /// How quickly the boomerang will return to its owner. (Default = 9)
        /// </summary>
        public float ReturnSpeed { get; set; } = 9f;

        /// <summary>
        /// How strong the boomerang will home in on its owner when returning. (Default = .4f)
        /// </summary>
        public float HomingOnOwnerStrength { get; set; } = 0.4f;

        /// <summary>
        /// How many frames the boomerang will travel away from the player for. (Default = 30)
        /// </summary>
        public int TravelOutFrames { get; set; } = 30;

        /// <summary>
        /// How many radians the boomerang will rotate per frame. (Default = .4f)
        /// </summary>
        public float RotationSpeed { get; set; } = 0.4f;

        /// <summary>
        /// Whether or not the boomerang will turn around when it reaches its max TravelOutFrames. (Default = true)
        /// </summary>
        public bool DoTurn { get; set; } = true;

        public Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1; // Custom AI

            Projectile.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;

            Projectile.tileCollide = true;
        }

        /// <summary>
        /// An "event" method called when the projectile has reached its apex.
        /// </summary>
        protected virtual void OnReachedApex() { }

        public override void AI()
        {
            // Play the "boomerang" sound every so often
            //
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Spin our boomerang
            Projectile.rotation += RotationSpeed * Projectile.direction;

            // AI state 1 - travelling away from player 
            if (Projectile.ai[0] == 0f)
            {
                // Increase our frame counter
                Projectile.ai[1] += 1f;
                // Check if our frame counter is high enough to turn around
                if (Projectile.ai[1] >= TravelOutFrames && DoTurn)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                    OnReachedApex();
                }
            }
            else if (Projectile.ai[0] == 1f)
            {
                // AI state 2 - travelling back to player
                // Should travel through tiles
                Projectile.tileCollide = false;

                // See if our projectile is too far away
                //
                float xDif = Owner.Center.X - Projectile.Center.X;
                float yDif = Owner.Center.Y - Projectile.Center.Y;
                float dist = MathF.Sqrt((xDif * xDif) + (yDif * yDif));
                if (dist > 3000f)
                {
                    Projectile.Kill();
                }

                // Get our x and y velocity values
                float mult = ReturnSpeed / dist;
                float xVel = xDif * mult;
                float yVel = yDif * mult;

                // Increase or decrease our X velocity accordingly 
                if (Projectile.velocity.X < xVel)
                {
                    Projectile.velocity.X += HomingOnOwnerStrength;
                    if (Projectile.velocity.X < 0f && xVel > 0f)
                    {
                        Projectile.velocity.X += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.X > xVel)
                {
                    Projectile.velocity.X -= HomingOnOwnerStrength;
                    if (Projectile.velocity.X > 0f && xVel < 0f)
                    {
                        Projectile.velocity.X -= HomingOnOwnerStrength;
                    }
                }

                // Same for our Y velocity
                if (Projectile.velocity.Y < yVel)
                {
                    Projectile.velocity.Y += HomingOnOwnerStrength;
                    if (Projectile.velocity.Y < 0f && yVel > 0f)
                    {
                        Projectile.velocity.Y += HomingOnOwnerStrength;
                    }
                }
                else if (Projectile.velocity.Y > yVel)
                {
                    Projectile.velocity.Y -= HomingOnOwnerStrength;
                    if (Projectile.velocity.Y > 0f && yVel < 0f)
                    {
                        Projectile.velocity.Y -= HomingOnOwnerStrength;
                    }
                }

                // Catch our projectile
                //
                if (Main.myPlayer == Projectile.owner)
                {
                    if (Projectile.getRect().Intersects(Main.player[Projectile.owner].getRect()))
                    {
                        Projectile.Kill();
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => TurnAround();

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width > 20 ? 20 : Projectile.width;
            height = Projectile.height > 20 ? 20 : Projectile.height;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            TurnAround();

            return false;
        }

        protected void TurnAround()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.ai[1] = 0f;
                Projectile.velocity = -Projectile.velocity;
                Projectile.netUpdate = true;
            }
        }
    }
}
