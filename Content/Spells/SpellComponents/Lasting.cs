using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.Spells.SpellComponents
{
    public class Lasting : ModSpellComponent
    {
        public override Color Color => Color.LightGray;

        public override void SetStaticDefaults()
        {
            modifiers["timeLeft"] = new ModifierData(additive: 200, multiplicative: 1.7f);
            modifiers["speed"] = new ModifierData(additive: 1.1f);
        }
    }
}
