using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace HardToLessHard.Content.Items
{
    public class SpawnerCage : ModItem
    {
        public int[] npcIDs = Array.Empty<int>();
        public override void SetDefaults()
        {
            Item.SetNameOverride("Empty Singularity");
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }


        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.altFunctionUse == 2 && SingularityItemAmount > 0)
            {
                for

                return true;
            }

            //Main.NewText("Use");
            return base.UseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (SingularityItemID >= ItemLoader.ItemCount || SingularityItemID <= 0) return;

            Item test = new Item();

            test.SetDefaults(SingularityItemID);

            TooltipLine tooltip = new(Mod, "Amount", $"{SingularityItemAmount} {test.Name}") { OverrideColor = Color.Red };
            tooltips.Add(tooltip);
        }

        public override void SaveData(TagCompound tag)
        {
            tag["SingularityItemID"] = SingularityItemID;
            tag["SingularityItemAmount"] = SingularityItemAmount;
            Item item = new Item();
            item.SetDefaults(SingularityItemID);
            tag["name"] = $"Singularity: {item.Name}";
        }

        public override void LoadData(TagCompound tag)
        {
            SingularityItemID = tag.Get<int>("SingularityItemID");
            SingularityItemAmount = tag.Get<int>("SingularityItemAmount");
            Item.SetNameOverride(tag.Get<string>("name"));
        }
    }
}
