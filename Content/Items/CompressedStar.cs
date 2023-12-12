using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Items
{
    public class CompressedStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Registers a vertical animation with 4 frames and each one will last 5 ticks (1/12 second)
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 14));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation

            ItemID.Sets.ItemIconPulse[Item.type] = true; // The item pulses while in the player's inventory
            ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity

            Item.ResearchUnlockCount = 25; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 1000; // Makes the item worth 1 gold.
            Item.rare = ItemRarityID.Orange;
            /*
            Item.damage = 9999999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            */
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CopperBar, 100);
            recipe.AddIngredient(ItemID.TinBar, 100);
            recipe.AddIngredient(ItemID.IronBar, 100);
            recipe.AddIngredient(ItemID.LeadBar, 100);
            recipe.AddIngredient(ItemID.SilverBar, 100);
            recipe.AddIngredient(ItemID.TungstenBar, 100);
            recipe.AddIngredient(ItemID.GoldBar, 100);
            recipe.AddIngredient(ItemID.PlatinumBar, 100);
            recipe.AddIngredient(ItemID.DemoniteBar, 100);
            recipe.AddIngredient(ItemID.CrimtaneBar, 100);
            recipe.AddIngredient(ItemID.MeteoriteBar, 100);
            recipe.AddIngredient(ItemID.Obsidian, 100);
            recipe.AddIngredient(ItemID.HellstoneBar, 100);
            recipe.AddIngredient(ItemID.CobaltBar, 100);
            recipe.AddIngredient(ItemID.PalladiumBar, 100);
            recipe.AddIngredient(ItemID.MythrilBar, 100);
            recipe.AddIngredient(ItemID.OrichalcumBar, 100);
            recipe.AddIngredient(ItemID.AdamantiteBar, 100);
            recipe.AddIngredient(ItemID.TitaniumBar, 100);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 100);
            recipe.AddIngredient(ItemID.LunarBar, 100);
            recipe.AddIngredient(ItemID.Amethyst, 100);
            recipe.AddIngredient(ItemID.Topaz, 100);
            recipe.AddIngredient(ItemID.Sapphire, 100);
            recipe.AddIngredient(ItemID.Emerald, 100);
            recipe.AddIngredient(ItemID.Ruby, 100);
            recipe.AddIngredient(ItemID.Diamond, 100);
            recipe.AddIngredient(ItemID.Amber, 100);
            recipe.AddIngredient(ItemID.KingSlimeTrophy);
            recipe.AddIngredient(ItemID.EyeofCthulhuTrophy);
            recipe.AddIngredient(ItemID.EaterofWorldsTrophy);
            recipe.AddIngredient(ItemID.BrainofCthulhuTrophy);
            recipe.AddIngredient(ItemID.QueenBeeTrophy);
            recipe.AddIngredient(ItemID.SkeletronTrophy);
            recipe.AddIngredient(ItemID.WallofFleshTrophy);
            recipe.AddIngredient(ItemID.DestroyerTrophy);
            recipe.AddIngredient(ItemID.SpazmatismTrophy);
            recipe.AddIngredient(ItemID.RetinazerTrophy);
            recipe.AddIngredient(ItemID.SkeletronPrimeTrophy);
            recipe.AddIngredient(ItemID.PlanteraTrophy);
            recipe.AddIngredient(ItemID.GolemTrophy);
            recipe.AddIngredient(ItemID.DukeFishronTrophy);
            recipe.AddIngredient(ItemID.AncientCultistTrophy);
            recipe.AddIngredient(ItemID.MoonLordTrophy);
            recipe.AddIngredient(ItemID.BossTrophyBetsy);
            recipe.AddIngredient(ItemID.BossTrophyDarkmage);
            recipe.AddIngredient(ItemID.BossTrophyOgre);
            recipe.AddIngredient(ItemID.FairyQueenTrophy);
            recipe.AddIngredient(ItemID.EverscreamTrophy);
            recipe.AddIngredient(ItemID.FlyingDutchmanTrophy);
            recipe.AddIngredient(ItemID.IceQueenTrophy);
            recipe.AddIngredient(ItemID.MartianSaucerTrophy);
            recipe.AddIngredient(ItemID.MourningWoodTrophy);
            recipe.AddIngredient(ItemID.PumpkingTrophy);
            recipe.AddIngredient(ItemID.QueenSlimeTrophy);
            recipe.AddIngredient(ItemID.SantaNK1Trophy);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.AddTile(TileID.CookingPots);
            recipe.Register();
        }
    }
}