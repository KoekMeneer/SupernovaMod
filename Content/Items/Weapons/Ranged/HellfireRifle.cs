using Microsoft.Xna.Framework;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class HellfireRifle : SupernovaGunItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-2, -2);

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.width = 70;
            Item.height = 30;

            Item.damage = 23;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.noMelee = true; //so the item's animation doesn't do damage
            Item.knockBack = 2;
            Item.value = Item.buyPrice(0, 15, 50, 0);
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            Item.shootSpeed = 11f;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;

            Item.scale = .8f;

            Gun.spread = 1.25f;
            Gun.recoil = .3f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<WoodenRifle>());
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FirearmManual>(), 2);
            recipe.AddIngredient(ItemID.HellstoneBar, 17);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void OnConsumeAmmo(Item ammo, Player player)
        {
            // An 18% chance not to consume ammo
            if (Main.rand.NextFloat() >= .18f)
            {
                base.OnConsumeAmmo(ammo, player);
            }
        }
    }
}