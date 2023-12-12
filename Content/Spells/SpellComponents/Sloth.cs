using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Spells.SpellComponents
{
    public class Sloth : ModSpellComponent
    {
        public override Color Color => Color.Brown;

        public override void SetStaticDefaults()
        {
            modifiers["speed"] = new ModifierData(divisive: 1.1f);
            modifiers["timeLeft"] = new ModifierData(multiplicative: 1.2f);
            modifiers["scale"] = new ModifierData(additive: 0.15f, multiplicative: 1.55f);
        }
    }
}
