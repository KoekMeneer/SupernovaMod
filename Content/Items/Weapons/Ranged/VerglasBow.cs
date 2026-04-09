using Microsoft.Xna.Framework;
using SupernovaMod.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class VerglasBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.autoReuse = true;
            Item.crit = 2;
            Item.width = 16;
            Item.height = 24;
            Item.useTime = 46;
            Item.useAnimation = 46;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.value = SellPrice.VerglasItem;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item5;
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 8;
            Item.DamageType = DamageClass.Ranged;
        }

        private readonly int _proj2Type = ModContent.ProjectileType<Projectiles.Ranged.VerglasIcicle>();
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 icicleVelocity = velocity * .7f;
            int icicleDamage = (int)(damage * .6f);

			float spread = MathHelper.ToRadians(5);
            Projectile.NewProjectile(source, position, icicleVelocity.RotatedByRandom(spread), _proj2Type, icicleDamage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, icicleVelocity.RotatedByRandom(spread) * .85f, _proj2Type, icicleDamage, knockback, player.whoAmI);

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.VerglasBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
