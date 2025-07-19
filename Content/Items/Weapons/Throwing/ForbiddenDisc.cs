using SupernovaMod.Common.Systems;
using SupernovaMod.Core;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class ForbiddenDisc : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = BuyPrice.RarityLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 14.5f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.ForbiddenDiscProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

			Item.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
		}

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial);
            recipe.AddIngredient(ModContent.ItemType<DiscOfTheDesert>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}