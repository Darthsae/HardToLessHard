using HardToLessHard.Content.Religions;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace HardToLessHard.Common.Systems
{
    public class UpdateSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            //Mod.Logger.Info("A");
            foreach (ModDeity deity in DeityLoader.deities)
            {
                deity.InternalUpdate();
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            foreach (ModDeity deity in DeityLoader.deities)
            {
                if (deity.enraged) {
                    deity.InterfaceDraw(spriteBatch);
                }
            }
        }
    }
}
