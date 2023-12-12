using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Spells
{
    public abstract class ModSpellComponent : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "SpellComponent";
        public int Type { get; internal set; }

        public virtual Color Color => Color.White;

        public Dictionary<string, ModifierData> modifiers = new()
        {
            { "damage", new ModifierData() },
            { "knockBack", new ModifierData() },
            { "stopsDealingDamageAfterPenetrateHits", new ModifierData() },
            { "tileCollide", new ModifierData() },
            { "timeLeft", new ModifierData() },
            { "ignoreWater", new ModifierData() },
            { "light", new ModifierData() },
            { "maxPenetrate", new ModifierData() },
            { "scale", new ModifierData() },
            { "speed", new ModifierData() }
        };

        public int[] buffs = System.Array.Empty<int>();

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        protected sealed override void Register()
        {
            ModTypeLookup<ModSpellComponent>.Register(this);
            Type = SpellComponentLoader.Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public override string ToString()
        {
            string test = $"Name: {DisplayName}\n";

            for (int i = 0; i < modifiers.Count; i++)
            {
                test += $"{modifiers.Keys.ToArray()[i]}: {modifiers.Values.ToArray()[i]}\n";
            }

            return test;
        }
    }

    public struct ModifierData
    {
        public float additive;
        public float subtractive;
        public float multiplicative;
        public float divisive;

        public ModifierData()
        {
            additive = 0;
            subtractive = 0;
            multiplicative = 0;
            divisive = 0;
        }

        public ModifierData(float additive = 0, float subtractive = 0, float multiplicative = 0, float divisive = 0)
        {
            this.additive = additive;
            this.subtractive = subtractive;
            this.multiplicative = multiplicative;
            this.divisive = divisive;
        }

        public readonly bool ApplyToBool(bool value)
        {
            return (((value ? 1 : 0) + additive - subtractive * (multiplicative + 1)) / (divisive + 1)) > 0;
        }

        public readonly int ApplyToInt(int value)
        {
            return (int)((value + additive - subtractive * (multiplicative + 1)) / (divisive + 1));
        }

        public readonly float ApplyToFloat(float value)
        {
            return (value + additive - subtractive * (multiplicative + 1)) / (divisive + 1);
        }

        public void Clear()
        {
            additive = 0;
            subtractive = 0;
            multiplicative = 0;
            divisive = 0;
        }

        public override string ToString()
        {
            return $"Add: {additive}, Sub: {subtractive}, Mul: {multiplicative}, Div: {divisive}";
        }

        public static ModifierData operator +(ModifierData modifierData) => modifierData;
        public static ModifierData operator -(ModifierData modifierData) => new (-modifierData.additive, -modifierData.subtractive, -modifierData.multiplicative, -modifierData.divisive);
        public static ModifierData operator +(ModifierData modifierData, ModifierData other) => new (modifierData.additive + other.additive, modifierData.subtractive + other.subtractive, modifierData.multiplicative + other.multiplicative, modifierData.divisive + other.divisive);
        public static ModifierData operator -(ModifierData modifierData, ModifierData other) => modifierData + (-other);
    }
}
