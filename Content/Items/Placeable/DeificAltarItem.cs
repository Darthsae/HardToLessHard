using Terraria.Enums;
using Terraria.ModLoader;
using Terraria;
using HardToLessHard.Content.Tiles;

namespace HardToLessHard.Content.Items.Placeable
{
    public class DeificAltarItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<DeificAltar>());
            Item.SetShopValues(ItemRarityColor.Lime7, Terraria.Item.buyPrice(platinum: 10));
        }
    }
}
