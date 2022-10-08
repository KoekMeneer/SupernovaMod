﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace Supernova.Content.PreHardmode.Items.Armor.Zirconium
{
    [AutoloadEquip(EquipType.Head)]
    public class ZirconiumCasque : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Zirconium Casque");
            Tooltip.SetDefault("A Zirconium helmet for the ranged and throwing classes");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 2; // The Defence value for this piece of armour.
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ZirconiumBreastplate>() && legs.type == ModContent.ItemType<ZirconiumBoots>();

        public override void UpdateArmorSet(Player player)
        {
            player.GetDamage(DamageClass.Ranged)    += .03f;
            player.GetDamage(DamageClass.Throwing)  += .03f;
            player.setBonus = "+3% ranged/thrown damage";
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 22);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
