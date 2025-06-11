using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class WoodenRifle : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 52;
            Item.height = 28;

            Item.damage = 13;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.knockBack = 1.2f;
            Item.value = Item.buyPrice(0, 10, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 7;

            Item.scale = .8f;

            Gun.spread = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 7);
            //recipe.anyIronBar = true;
            recipe.acceptedGroups = new() { RecipeGroupID.IronBar, RecipeGroupID.Wood };
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>());
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            // An 18% chance not to consume ammo
            if (Main.rand.NextFloat() >= .12f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }
    }
}