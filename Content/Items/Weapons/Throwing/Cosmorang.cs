using SupernovaMod.Common.Systems;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Throwing
{
    public class Cosmorang : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 56;
            Item.knockBack = 1;
            Item.noMelee = true;
            Item.maxStack = 1;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 1, 46, 82);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 19;
            Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.CosmorangProj>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.DamageType = GlobalModifiers.DamageClass_ThrowingMelee;
        }
        public override bool CanUseItem(Player player) //this make that you can shoot only 2 boomerangs at once
        {
            int instances = 0;
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                {
                    instances++;
                    if (instances >= 2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<Materials.EldritchEssence>(20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}