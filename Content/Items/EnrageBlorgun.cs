using HardToLessHard.Common.Players;
using HardToLessHard.Content.Religions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Items
{
    public class EnrageBlorgun : ModItem
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
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            Mod.Logger.Info("USE");
            HTLHPlayer modPlayer = player.GetModPlayer<HTLHPlayer>();
            ModDeity deity = DeityLoader.GetDeity("Blorgun");
            deity.target = modPlayer.Player.whoAmI;
            deity.enraged = true;
        }
    }
}