using HardToLessHard.Common.Players;
using HardToLessHard.Content.NPCs;
using HardToLessHard.Content.Factions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ModLoader.UI;

namespace HardToLessHard.Common.UI.DiscussionUI
{
    // This custom UI will show whenever the player is holding the ExampleCustomResourceWeapon item and will display the player's custom resource amounts that are tracked in ExampleResourcePlayer
    internal class DiscussionUIState : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIText name;
        private UIText faction;
        private UIElement area;
        private UIPanel background;
        private UIButton<string> button1;
        private UIButton<string> button2;
        private UIButton<string> button3;

        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 850, 1f); // Place the resource bar to the left of the hearts.
            area.Top.Set(50, 0f); // Placing it just a bit below the top of the screen.
            area.Width.Set(550, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(500, 0f);

            background = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            background.Left.Set(0, 0f);
            background.Top.Set(0, 0f);
            background.Width.Set(500, 0f);
            background.Height.Set(500, 0f);
            
            name = new UIText("", 1.5f); // name to show stat
            name.Width.Set(400, 0f);
            name.Height.Set(50, 0f);
            name.Top.Set(50, 0f);
            name.Left.Set(50, 0f);

            faction = new UIText("", 1.2f); // name to show stat
            faction.Width.Set(400, 0f);
            faction.Height.Set(50, 0f);
            faction.Top.Set(150, 0f);
            faction.Left.Set(50, 0f);

            button1 = new UIButton<string>("Button 1");
            button1.Width.Set(100, 0f);
            button1.Height.Set(50, 0f);
            button1.Top.Set(400, 0f);
            button1.Left.Set(50, 0f);

            button2 = new UIButton<string>("Button 2");
            button2.Width.Set(100, 0f);
            button2.Height.Set(50, 0f);
            button2.Top.Set(400, 0f);
            button2.Left.Set(200, 0f);

            button3 = new UIButton<string>("Close");
            button3.Width.Set(100, 0f);
            button3.Height.Set(50, 0f);
            button3.Top.Set(400, 0f);
            button3.Left.Set(350, 0f);

            area.Append(background);
            area.Append(name);
            area.Append(faction);
            area.Append(button1);
            area.Append(button2);
            area.Append(button3);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (!Main.LocalPlayer.GetModPlayer<HTLHPlayer>().inConversation || Main.LocalPlayer.GetModPlayer<HTLHPlayer>().conversationNPC == null) return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            /*
            var modPlayer = Main.LocalPlayer.GetModPlayer<SoulPlayer>();
            // Calculate quotient
            float quotient = (float)modPlayer.soulCurrent / modPlayer.soulMax2; // Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
            quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

            // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
            Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
            hitbox.X += 40;
            hitbox.Width -= 50;
            hitbox.Y += 12;
            hitbox.Height -= 24;

            // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                // float percent = (float)i / steps; // Alternate Gradient Approach
                float percent = (float)i / (right - left);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
            */
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.inConversation && modPlayer.conversationNPC == null))
                return;

            ref ExtendedModNPC talkingTo = ref modPlayer.conversationNPC;
            // Setting the name per tick to update and show our resource values.
            name.SetText(talkingTo.DisplayName);
            faction.SetText(FactionLoader.GetFaction(talkingTo.Faction).DisplayName);
            faction.TextColor = FactionLoader.GetFaction(talkingTo.Faction).Color;
            button1.SetText(modPlayer.conversationNPC.buttonNames[0]);
            button2.SetText(modPlayer.conversationNPC.buttonNames[1]);
            base.Update(gameTime);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.inConversation && modPlayer.conversationNPC == null))
                return;

            if (button1.IsMouseHovering)
            {
                modPlayer.conversationNPC.ButtonClicked(0);
            }
            else if (button2.IsMouseHovering)
            {
                modPlayer.conversationNPC.ButtonClicked(1);
            }
            else if (button3.IsMouseHovering)
            {
                modPlayer.conversationNPC = null;
                modPlayer.inConversation = false;
            }
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class DiscussionUISystem : ModSystem
    {
        private UserInterface DiscussionUserInterface;

        internal DiscussionUIState DiscussionUIState;

        public override void Load()
        {
            DiscussionUIState = new();
            DiscussionUserInterface = new();
            DiscussionUserInterface.SetState(DiscussionUIState);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DiscussionUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "HardToLessHard: Discussion",
                    delegate {
                        DiscussionUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
