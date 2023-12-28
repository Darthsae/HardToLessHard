using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace HardToLessHard.Content.Items
{
    public class StrangeSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A Strange Metallic Skull.");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = 1000;
            Item.rare = ItemRarityID.Pink;
        }
    }
}
