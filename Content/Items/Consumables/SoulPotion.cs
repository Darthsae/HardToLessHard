using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Items.Consumables
{
    public class SoulPotion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
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
            return player.GetModPlayer<Common.Players.SoulPlayer>().soulCurrent < player.GetModPlayer<Common.Players.SoulPlayer>().soulMax2;
        }

        public override void OnConsumeItem(Player player)
        {
            player.GetModPlayer<Common.Players.SoulPlayer>().soulCurrent += 75;
        }
    }
}
