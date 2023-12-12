using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Items.Consumables
{
    public class SoulGem : ModItem
    {
        public static readonly int MaxSoulGems = 10;
        public static readonly int SoulPerGem = 10;

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item2;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
        }

        public override bool ConsumeItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            return player.GetModPlayer<Common.Players.SoulPlayer>().soulGems < MaxSoulGems;
        }

        public override void OnConsumeItem(Player player)
        {
            player.GetModPlayer<Common.Players.SoulPlayer>().soulGems++;
        }
    }
}
