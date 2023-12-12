using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Factions
{
    public class Swarm : ModFaction
    {
        public override Color Color => Color.DarkKhaki;

        public override void RelationSetup()
        {
            relations[FactionLoader.GetFaction("NoFaction").Type] = -100;
            relations[FactionLoader.GetFaction("Dead").Type] = 0;
        }
    }
}
