using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria;

namespace HardToLessHard.Common.UI
{
    internal class UIHoverImageButton : UIImageButton
    {
        // Tooltip name that will be shown on hover
        internal string hoverText;

        public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture)
        {
            this.hoverText = hoverText;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.DrawSelf(spriteBatch);

            // IsMouseHovering becomes true when the mouse hovers over the current UIElement
            if (IsMouseHovering)
                Main.hoverItemName = hoverText;
        }
    }
}