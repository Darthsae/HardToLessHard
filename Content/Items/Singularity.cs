using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.UI;

namespace HardToLessHard.Content.Items
{
    public class Singularity : ModItem
    {
        public int SingularityItemID = 0;
        public int SingularityItemAmount = 0;
        public int SingularityTimer = 0;

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
                //Main.NewText("Good Use");

                Item toSpawn = new();
                toSpawn.SetDefaults(SingularityItemID);

                SingularityItemAmount--;

                //Main.NewText(toSpawn.Name);
                player.QuickSpawnItemDirect(player.GetSource_FromThis(), toSpawn);

                return true;
            }

            //Main.NewText("Use");
            return base.UseItem(player);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (SingularityItemID >= 0 && SingularityItemID <= ItemLoader.ItemCount)
            {
                Item item = new();

                item.SetDefaults(SingularityItemID);

                Main.DrawItemIcon(spriteBatch, item, position, drawColor, 20 * scale);
            }
        }

        public override void UpdateInventory(Player player)
        {
            SingularityTimer++;
            if (SingularityTimer > SingularityItemAmount)
            {
                SingularityItemAmount++;
                SingularityTimer = 0;
            }
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

        public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<CompressedStar>(10);
            recipe.Register(); 
        }

    }
}
