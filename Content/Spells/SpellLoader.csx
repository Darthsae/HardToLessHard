using HardToLessHard.Content.Factions;
using System.Collections.Generic;

namespace HardToLessHard.Content.Spells
{
    public static class SpellLoader
    {
        public static int Count => modSpells.Count;

        internal static readonly IList<ModSpell> modSpells = new List<ModSpell>();
        internal static readonly IList<string> spellNames = new List<string>();

        internal static int Register(ModSpell spell)
        {
            modSpells.Add(spell);
            spellNames.Add(spell.Name);
            return Count - 1;
        }

        public static ModSpell GetSpell(int type)
        {
            return type >= 0 && type < Count ? modSpells[type] : null;
        }

        public static ModSpell GetSpell(string name)
        {
            return GetSpell(spellNames.IndexOf(name));
        }

        internal static void Unload()
        {
            modSpells.Clear();
        }
    }
}
