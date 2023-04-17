﻿using SupernovaMod.Api;
using Terraria;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class HarbingersCrest : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Harbingers Crest");
            Tooltip.SetDefault("Summons a Harbinger Arm to fight for you.");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = -12;
            Item.expert = true;
            Item.value = BuyPrice.RarityBlue;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
			// Summon the Harbingers Arm
			//
            if (player.whoAmI == Main.myPlayer)
            {
				player.AddBuff(ModContent.BuffType<Buffs.Summon.HarbingersArmBuff>(), 1);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>()] < 1)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>(), 17, 4, player.whoAmI, Item.whoAmI);
				}
			}
		}
		/*public override void UpdateVanity(Player player)
		{
			// Summon the Harbingers Arm
			//
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(ModContent.BuffType<Buffs.Summon.HarbingersArmBuff>(), 1);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>()] < 1)
				{
					Projectile.NewProjectile(player.GetSource_Accessory(Item), player.position, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<Projectiles.Summon.HarbingersArmSummon>(), 17, 4, player.whoAmI, Item.whoAmI, 1);
				}
			}
		}*/
	}
}