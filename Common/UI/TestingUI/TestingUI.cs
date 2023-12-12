using HardToLessHard.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace HardToLessHard.Common.UI.TestingUI
{
    // This custom UI will show whenever the player is holding the ExampleCustomResourceWeapon item and will display the player's custom resource amounts that are tracked in ExampleResourcePlayer
    internal class TestingUIState : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        private UIPanel background;
        private UIScrollbar scrollbar;
        private UIText text1;
        private UIText text2;

        public override void OnInitialize()
        {
            // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
            // UIElement is invisible and has no padding.
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 850, 1f); // Place the resource bar to the left of the hearts.
            area.Top.Set(50, 0f); // Placing it just a bit below the top of the screen.
            area.Width.Set(550, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(500, 0f);
            area.OverflowHidden = true;

            background = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            background.Left.Set(20, 0f);
            background.Top.Set(0, 0f);
            background.Width.Set(530, 0f);
            background.Height.Set(500, 0f);
            
            scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0f);
            scrollbar.Top.Set(20, 0f);
            scrollbar.Height.Set(460, 0f);
            scrollbar.SetView(0, 10);
            scrollbar.OnLeftMouseUp += (evt, e) => {
                text1.Top.Set(0 + (scrollbar.ViewPosition * 16), 0f);
                text2.Top.Set(48 + (scrollbar.ViewPosition * 16), 0f);
            };

            text1 = new UIText("TEXT1aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            text1.Left.Set(0, 0f);
            text1.Top.Set(650, 0f);
            text1.Width.Set(100, 0f);
            text1.Height.Set(32, 0f);

            text2 = new UIText("TEXT2bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
            text2.Left.Set(0, 0f);
            text2.Top.Set(0, 0f);
            text2.Width.Set(600, 0f);
            text2.Height.Set(32, 0f);

            area.Append(scrollbar);
            background.Append(text1);
            background.Append(text2);
            area.Append(background);

            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (!Main.LocalPlayer.GetModPlayer<HTLHPlayer>().testingUI) return;

            base.Draw(spriteBatch);
        }

        // Here we draw our UI
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.testingUI))
                return;

            base.Update(gameTime);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.testingUI))
                return;
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class TestingUISystem : ModSystem
    {
        private UserInterface TestingUserInterface;

        internal TestingUIState TestingUIState;

        public override void Load()
        {
            TestingUIState = new();
            TestingUserInterface = new();
            TestingUserInterface.SetState(TestingUIState);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            TestingUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "HardToLessHard: Testing UI",
                    delegate {
                        TestingUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
