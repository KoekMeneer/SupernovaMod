﻿using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SupernovaMod.Api;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using SupernovaMod.Common;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class GazerMinion : SupernovaMinionProjectile
    {
        protected override int BuffType => ModContent.BuffType<Buffs.Summon.GazerMinionBuff>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gazer");

			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 7;

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
            Projectile.localNPCHitCooldown = 16;
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles() => false;

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage() => false;

		protected override void UpdateMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
		{
			UpdateIdleMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

			if (foundTarget)
            {
                ref float timer = ref Projectile.localAI[0];
                timer++;

                if (timer % 80 == 0)
                {
                    Shoot(targetCenter);
                }

				// Set direction
				//
				if (Projectile.Center.X < targetCenter.X - 2f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.Center.X > targetCenter.X + 2f)
				{
					Projectile.direction = -1;
				}
			}
            else
            {
				// Set direction
				//
				if (Projectile.Center.X < vectorToIdlePosition.X - 2f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.Center.X > vectorToIdlePosition.X + 2f)
				{
					Projectile.direction = 1;
				}
			}
		}

		void Shoot(Vector2 targetCenter)
		{
			int type = ProjectileID.RayGunnerLaser;// 438 || 100
			Vector2 Velocity = Mathf.VelocityFPTP(Projectile.Center, targetCenter, 5);
			int Spread = 1;
			float SpreadMult = 0.15f;
			Velocity.X = Velocity.X + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			Velocity.Y = Velocity.Y + Main.rand.Next(-Spread, Spread + 1) * SpreadMult;
			int i = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Velocity.X, Velocity.Y, type, Projectile.damage, 1.75f, Projectile.owner);
			Main.projectile[i].hostile = false;
			Main.projectile[i].friendly = true;
			Main.projectile[i].tileCollide = true;
		}
	}
}