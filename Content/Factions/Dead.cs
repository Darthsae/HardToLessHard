using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Factions
{
    public class Dead : ModFaction
    {
        public override Color Color => Color.Black;

        public override void RelationSetup()
        {
            
            relations[FactionLoader.GetFaction("NoFaction").Type] = -25;
            relations[FactionLoader.GetFaction("Swarm").Type] = 0;
        }
    }
}
