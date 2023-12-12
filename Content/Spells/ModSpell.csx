/*
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Spells
{
    public abstract class ModSpelal : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Spell";
        public int Type { get; internal set; }

        public ModSpellComponent[] SpellComponents { get; set; }

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
            { "scale", new ModifierData() }
        };

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        public void SetModifiers()
        {
            modifiers["damage"].Clear();
            modifiers["knockBack"].Clear();
            modifiers["stopsDealingDamageAfterPenetrateHits"].Clear();
            modifiers["tileCollide"].Clear();
            modifiers["timeLeft"].Clear();
            modifiers["ignoreWater"].Clear();
            modifiers["light"].Clear();
            modifiers["maxPenetrate"].Clear();
            modifiers["scale"].Clear();

            foreach (var spellComponent in  SpellComponents)
            {
                modifiers["damage"] += spellComponent.modifiers["damage"];
                modifiers["knockBack"] += spellComponent.modifiers["knockBack"];
                modifiers["stopsDealingDamageAfterPenetrateHits"] += spellComponent.modifiers["stopsDealingDamageAfterPenetrateHits"];
                modifiers["tileCollide"] += spellComponent.modifiers["tileCollide"];
                modifiers["timeLeft"] += spellComponent.modifiers["timeLeft"];
                modifiers["ignoreWater"] += spellComponent.modifiers["ignoreWater"];
                modifiers["light"] += spellComponent.modifiers["light"];
                modifiers["maxPenetrate"] += spellComponent.modifiers["maxPenetrate"];
                modifiers["scale"] += spellComponent.modifiers["scale"];
            }
        }

        protected sealed override void Register()
        {
            ModTypeLookup<ModSpell>.Register(this);
            Type = SpellLoader.Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
            SetModifiers();
        }
    }
}
*/