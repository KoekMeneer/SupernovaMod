using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Items.Accessories.Hardmode
{
    public class SealOfSolomon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Seal of Solomon");
            Tooltip.SetDefault("Summons a devil to fight for you");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 8, 0, 0);
            item.accessory = true;
            item.summon = true;
            item.damage = 48;
            item.rare = ItemRarityID.LightRed;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SwiftnessPotion, 4);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(mod.GetItem("HelixStone"));
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/

        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            player.AddBuff(mod.BuffType("RedDevilBuff"), 10);
            if (player.ownedProjectileCounts[mod.ProjectileType("RedDevilMinion")] < 1)
                Projectile.NewProjectile(player.position + new Vector2(0, -100), Vector2.Zero, mod.ProjectileType("RedDevilMinion"), item.damage, 2, item.owner);
        }
    }
}