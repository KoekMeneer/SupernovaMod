using SupernovaMod.Common.Systems;
using SupernovaMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class VerglasThrowingAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.crit = 4;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = SellPrice.VerglasItem;
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.VerglasThrowingAxe>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

			Item.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.VerglasBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}