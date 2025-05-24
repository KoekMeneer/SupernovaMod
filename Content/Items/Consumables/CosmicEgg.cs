using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Terraria.GameContent.Creative;
using SupernovaMod.Api;

namespace SupernovaMod.Content.Items.Consumables
{
    public class CosmicEgg : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.MechanicalEye);
            Item.width = 28;
            Item.height = 30;
        }

        public override bool CanUseItem(Player player)
        {
            // Does NPC already Exist?
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<Npcs.CosmicCollective.CosmicCollective>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            // Summon the Cosmic Collective
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Npcs.CosmicCollective.CosmicCollective>()); // Spawn the boss within a range of the player. 
			SoundEngine.PlaySound(SoundID.NPCDeath11, player.position); // Play item break sound
			SoundEngine.PlaySound(SoundID.Roar, player.position); // Play spawn sound
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Materials.ZirconiumBar>(), 3);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
