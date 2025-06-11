using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SupernovaMod.Common.GlobalNPCs
{
	public class DebuffNPC : GlobalNPC
	{
		public int electrified;
		public int cursed;

		private int? _preCursedDamage;

		public override bool InstancePerEntity => true;

		public override GlobalNPC Clone(NPC from, NPC to)
		{
			DebuffNPC myClone = (DebuffNPC)base.Clone(from, to);
			myClone.electrified = electrified;
			return myClone;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (electrified > 0)
			{
				int baseElectrifiedDoTValue = (int)((double)(5 * ((npc.velocity.X == 0f) ? 1 : 4)) * GetElectrifiedMod(npc));
				ApplyDamageOverTime(baseElectrifiedDoTValue, baseElectrifiedDoTValue / 5, ref npc.lifeRegen, ref damage);
			}
			if (cursed > 0)
			{
				if (!_preCursedDamage.HasValue)
				{
					_preCursedDamage = npc.damage;
                }
                npc.damage = _preCursedDamage.Value / 2;
            }
        }
		private float GetElectrifiedMod(NPC npc)
		{
			bool increasedElectricityDamage = npc.wet || npc.honeyWet || npc.lavaWet || npc.dripping;
			return increasedElectricityDamage ? 2 : 1;
		}

		public override void PostAI(NPC npc)
		{
			if (electrified > 0)
			{
				electrified--;
			}
			if (cursed > 0)
			{
				cursed--;
				if (cursed == 0)
				{
#pragma warning disable CS8629 // Can not be null here
                    npc.damage = _preCursedDamage.Value;
#pragma warning restore CS8629 
                    _preCursedDamage = null;
				}
            }
		}

		public void ApplyDamageOverTime(int lifeRegenVal, int damageVal, ref int lifeRegen, ref int damage)
		{
			if (lifeRegen > 0)
			{
				lifeRegen = 0;
			}
			lifeRegen -= lifeRegenVal;

			if (damage < damageVal)
			{
				damage = damageVal;
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (electrified > 0 && Main.rand.Next(5) < 4)
			{
				int dust13 = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, DustID.Electric, npc.velocity.X * 0.4f,npc.velocity.Y * 0.4f, 0, default(Color), 1f);
				Main.dust[dust13].noGravity = true;
				Main.dust[dust13].velocity *= 1.1f;
				Dust dust27 = Main.dust[dust13];
				dust27.velocity.Y = dust27.velocity.Y + 0.25f;
				if (Utils.NextBool(Main.rand, 2))
				{
					Main.dust[dust13].noGravity = false;
					Main.dust[dust13].scale *= 0.5f;
				}
			}
		}
	}
}
