using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class CosmolingMinion : SupernovaMinionProjectile
    {
        protected override int BuffType => ModContent.BuffType<Buffs.Summon.CosmolingMinionBuff>();

        public override void SetStaticDefaults()
        {
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 2;

            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 28;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;

            speedMax = 10;
            inertia = 4;
            setProjectileDirection = false;
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }

        protected override void UpdateVisuals()
        {
            // Check if our projectile has multiple frames
            //
            if (Main.projFrames[Projectile.type] > 0)
            {
                // This is a simple "loop through all frames from top to bottom" animation
                Projectile.frameCounter++;

                if (Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;

                    if (Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 0;
                    }
                }
            }
        }

        protected override void UpdateMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, NPC target, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            Vector2 lookAtPosition;
            if (foundTarget)
            {
                lookAtPosition = targetCenter;
                UpdateAttackMovement(foundTarget, distanceFromTarget, targetCenter, target, distanceToIdlePosition, vectorToIdlePosition);
            }
            else
            {
                lookAtPosition = Main.player[Projectile.owner].Center;
                ModifyIdlePosition(ref lookAtPosition);

                UpdateIdleMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            }

            // Look at the position the summon will go to
            LookInDirection(lookAtPosition - Projectile.Center);
        }
        private void LookInDirection(Vector2 look)
        {
            float angle = 0.5f * (float)Math.PI;

            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }

            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }

            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }

            Projectile.rotation = angle;
        }

        protected override void UpdateAttackMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, NPC target, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            // Minion has a target: attack (here, fly towards the enemy)
            //
            if (distanceFromTarget < target.width + (Projectile.width * 2))
            {
                Projectile.velocity *= 1.02f;
                return;
            }
            base.UpdateAttackMovement(foundTarget, distanceFromTarget, targetCenter, target, distanceToIdlePosition, vectorToIdlePosition);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int i = 0;
            while ((double)i < hit.Damage / (double)target.lifeMax * 100.0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1.5f);
                i++;
            }
        }
    }
}
