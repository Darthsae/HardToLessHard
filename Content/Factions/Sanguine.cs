using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Factions
{
    public class Sanguine : ModFaction
    {
        public override Color Color => Color.PaleVioletRed;

        public override short defaultRelation => -100;

        public override void RelationSetup()
        {
            relations[FactionLoader.GetFaction("Swarm").Type] = defaultRelation;
            relations[FactionLoader.GetFaction("Dead").Type] = defaultRelation;
            relations[FactionLoader.GetFaction("NoFaction").Type] = defaultRelation;
        }
    }
}
