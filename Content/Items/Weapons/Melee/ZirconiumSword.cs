using Microsoft.Xna.Framework;
using SupernovaMod.Core;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class ZirconiumSword : ModItem
    {
        private readonly int _projIdSpark = ModContent.ProjectileType<Projectiles.Melee.ZicroniumSpark>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Sword");
            // Tooltip.SetDefault("Release a blast of Zirconium Sparks every 4 hits.\nZirconium Sparks linger for a short while.");
        }
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.crit = 1;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.value = SellPrice.ZirconiumItem;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.scale += .05f;
            Item.DamageType = DamageClass.Melee;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                // Create a subtle dust trail behind the sword during the swing
                int dustIndex = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.ZirconDust>(), 0f, 0f, 100, default, 1.2f);
                Dust dust = Main.dust[dustIndex];
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.position.X += player.direction * hitbox.Width * 0.5f;  // Position dust behind the sword
            }
        }

        private int _hits;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Create a burst of zircon dust when the sword hits an NPC
            int dustAmount = 5;
            for (int i = 0; i < dustAmount; i++)
            {
                int dustIndex = Dust.NewDust(target.position, Item.width, Item.height, ModContent.DustType<Dusts.ZirconDust>(), Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 100, default, 1.5f);
                Dust dust = Main.dust[dustIndex];
                dust.noGravity = true;
                dust.velocity *= 0.75f;
            }

            _hits++;
			if (_hits < 4)
            {
                return;
            }
            SoundEngine.PlaySound(SoundID.Item93, new Vector2?(player.position));

            // Spark Explosion effect
            for (int j = 0; j <= Main.rand.Next(2, 4); j++)
            {
                Vector2 velocity = (Vector2.One * Main.rand.Next(2, 4)).RotatedByRandom(180);
                Projectile.NewProjectile(Item.GetSource_FromAI(), target.position.X, target.position.Y, velocity.X, velocity.Y, _projIdSpark, Item.damage / 2, 3, player.whoAmI);
            }
            _hits = 0;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ZirconiumBar>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
