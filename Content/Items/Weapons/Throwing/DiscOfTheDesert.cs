using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class DiscOfTheDesert : ModItem
    {
        private const int MAX_PROJECTILES = 2;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 23;
            Item.height = 23;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 5, 60, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shootSpeed = 8;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.DiscOfTheDesertProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

			Item.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
		}

        public override bool CanUseItem(Player player) //this make that you can shoot only 1 boomerang at once
        {
            int projCount = 0;
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                {
                    projCount++;
                    if (projCount >= MAX_PROJECTILES)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void AddRecipes() //SturdyFossil
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Amber, 6);
            recipe.AddIngredient(ItemID.FossilOre, 10);
            recipe.AddIngredient(ModContent.ItemType<CactusBoomerang>());
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}