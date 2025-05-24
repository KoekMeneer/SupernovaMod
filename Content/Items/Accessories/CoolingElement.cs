using SupernovaMod.Common.Players;
using SupernovaMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class CoolingElement : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = BuyPrice.RarityGreen;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            ResourcePlayer resourcePlayer = player.GetModPlayer<ResourcePlayer>();
			resourcePlayer.ringCoolRegen -= 0.08f;

            if (player.HasBuff(ModContent.BuffType<Buffs.Cooldowns.RingCooldown>()))
            {
                player.moveSpeed *= 1.1f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.Rime>(), 4);
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 2);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
