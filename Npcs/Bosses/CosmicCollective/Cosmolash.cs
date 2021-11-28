using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Npcs.Bosses.CosmicCollective
{
    public class Cosmolash : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmolash");

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.width = 40;
            item.crit = 4;
            item.height = 20;
            item.useTime = 48;
            item.useAnimation = 48;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1.2f;
            item.value = Item.buyPrice(0, 12, 50, 0);
            item.autoReuse = true;
            item.rare = Rarity.LigtRed;
            item.UseSound = SoundID.Item11;
            item.shoot = 10; //idk why but all the guns in the vanilla source have this
            item.shootSpeed = 13f;
            item.useAmmo = AmmoID.Bullet;
            item.ranged = true; // For Ranged Weapon
            item.channel = true;
        }
        public override bool ConsumeAmmo(Player player) => Main.rand.NextFloat() >= .12f;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            // If the player channels the weapon, do something. This check only works if item.channel is true for the weapon.
            if (player.channel)
            {
                if (item.useAnimation >= 23 && item.useTime >= 23)
                {
                    item.useTime -= 10;
                    item.useAnimation -= 10;
                }else if (item.useAnimation <= 24 && item.useTime <= 24 && item.useTime >= 5)
                {
                    item.useTime -= 1;
                    item.useAnimation -= 1;
                }
            }
            else
            {
                player.channel = false;
                item.useTime = 48;
                item.useAnimation = 48;
            }

            if (Main.rand.Next(340) == 0 && item.useAnimation <= 23 && item.useTime <= 23)
            {
                item.autoReuse = false;
                player.channel = false;
                item.useTime = 48;
                item.useAnimation = 48;
            }
            else
                item.autoReuse = true;

            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}