using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Supernova.Items.Weapons.Hardmode
{
    public class Magnitrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnitrix");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override void SetDefaults()
        {
            item.damage = 42;
            item.autoReuse = false;
            item.crit = 4;
            item.width = 16;
            item.height = 24;
            item.useAnimation = 10;
            item.useTime = 2;
            item.reuseDelay = 57;
            item.useStyle = 5; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.buyPrice(1, 0, 0, 0); // Another way to handle value of item.
            item.rare = Rarity.LigtRed;
            item.UseSound = SoundID.Item5; // Sound for Bows
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = 1;
            item.shootSpeed = 12f;
            item.ranged = true; // For Ranged Weapon
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 speed = Calc.RandomSpread(speedX, speedY, .05f);
            Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
}
