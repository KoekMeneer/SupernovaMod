using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class Cosmalice : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmalice");
        }

        public override void SetDefaults()
        {
            item.rare = Rarity.LigtRed;
            item.UseSound = SoundID.Item21;
            item.useStyle = 5;
            item.noMelee = true; 
            item.noUseGraphic = false;
            item.magic = true;    
            item.crit = 4;
            item.damage = 47;
            item.useTime = 10;
            item.useAnimation = 10;
            item.mana = 2;
            item.shootSpeed = 8;
            item.shoot = 496;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 6, 80, 64);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            Vector2 perturbedSpeed1 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
            speedX = perturbedSpeed1.X;
            speedY = perturbedSpeed1.Y;

            float rotation = MathHelper.ToRadians(28);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 85f;
            return true; // return true to allow tmodloader to call Projectile.NewProjectile as normal
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BookofSkulls);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ItemID.DirtBlock, 15);
            recipe.AddIngredient(ItemID.StoneBlock, 15);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddIngredient(ItemID.IceBlock, 15);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(ItemID.MudBlock, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 7);
            recipe.AddIngredient(ItemID.SoulofLight, 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}
