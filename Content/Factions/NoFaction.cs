using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Factions
{
    public class NoFaction : ModFaction
    {
        public override Color Color => Color.White;

        public override void RelationSetup()
        {
            relations[FactionLoader.GetFaction("Swarm").Type] = -100;
            relations[FactionLoader.GetFaction("Dead").Type] = -25;
        }
    }
}
