using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using SupernovaMod.Core;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class Kusanagi : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
        }
		public override void SetDefaults()
		{
            // Common Properties
            Item.width = 72;
            Item.height = 80;
            Item.rare = ItemRarityID.LightRed;
            Item.value = BuyPrice.RarityLightRed;

            // Use Properties
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            // Weapon Properties
            Item.crit = 3;
            Item.damage = 89;
            Item.knockBack = 7;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.channel = true;

            // Projectile Properties
            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<Kusanagi_Proj>();
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Katana);
			recipe.AddIngredient(ModContent.ItemType<Materials.BrokenSwordShards>());
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
