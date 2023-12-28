using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Buffs
{
    public abstract class AnimatedBuff : ModBuff
    {
        // Some constants we define to make our life easier.
        public virtual int FrameCount => 4; // Amount of frames we have on our animation spritesheet.
        public virtual int AnimationSpeed => 60; // In ticks.

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            Texture2D ourTexture = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle ourSourceRectangle = ourTexture.Frame(verticalFrames: FrameCount, frameY: (int)Main.GameUpdateCount / AnimationSpeed % FrameCount);

			spriteBatch.Draw(ourTexture, drawParams.Position, ourSourceRectangle, drawParams.DrawColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			return false;
        }
    }
}