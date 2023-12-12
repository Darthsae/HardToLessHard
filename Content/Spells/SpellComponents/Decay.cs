using Microsoft.Xna.Framework;
using System.Linq;
using Terraria.ID;

namespace HardToLessHard.Content.Spells.SpellComponents
{
    public class Decay : ModSpellComponent
    {
        public override Color Color => new(15, 15, 15);

        public override void SetStaticDefaults()
        {
            modifiers["damage"] = new ModifierData(multiplicative: 0.9f);
            buffs = new int[] { BuffID.Bleeding };
        }
    }
}
