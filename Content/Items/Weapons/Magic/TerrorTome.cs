using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using SupernovaMod.Content.Npcs.FlyingTerror;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
	public class TerrorTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Terror Tome");
        }
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.crit = 2;
            Item.width = 24;
            Item.height = 28;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = BuyPrice.RarityGreen;
            Item.rare = ItemRarityID.Green;
            Item.mana = 4;             //mana use
            Item.UseSound = SoundID.Item21;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Magic.TerrorProjFirendly>();
            Item.shootSpeed = 9;
            Item.DamageType = DamageClass.Magic;
        }

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
            velocity = velocity.RotatedByRandom(.2f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TerrorTuft>());
			recipe.AddTile(TileID.DemonAltar);
			recipe.Register();
		}
	}
}