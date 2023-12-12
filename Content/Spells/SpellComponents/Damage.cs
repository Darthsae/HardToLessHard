using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Spells.SpellComponents
{
    public class Damage : ModSpellComponent
    {
        public override Color Color => Color.Red;

        public override void SetStaticDefaults()
        {
            modifiers["damage"] = new ModifierData(additive: 10, multiplicative: 2.5f);
            modifiers["timeLeft"] = new ModifierData(additive: 120, multiplicative: 1.1f);
        }
    }
}
