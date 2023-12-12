using HardToLessHard.Content.Factions;
using System.Collections.Generic;

namespace HardToLessHard.Content.Spells
{
    public static class SpellComponentLoader
    {
        public static int Count => spellComponents.Count;

        internal static readonly IList<ModSpellComponent> spellComponents = new List<ModSpellComponent>();
        internal static readonly IList<string> spellComponentNames = new List<string>();

        internal static int Register(ModSpellComponent spellComponent)
        {
            spellComponents.Add(spellComponent);
            spellComponentNames.Add(spellComponent.Name);
            return Count - 1;
        }

        public static ModSpellComponent GetSpellComponent(int type)
        {
            return type >= 0 && type < Count ? spellComponents[type] : null;
        }

        public static ModSpellComponent GetSpellComponent(string name)
        {
            return GetSpellComponent(spellComponentNames.IndexOf(name));
        }

        internal static void Unload()
        {
            spellComponents.Clear();
        }
    }
}
