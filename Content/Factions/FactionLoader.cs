using System.Collections.Generic;

namespace HardToLessHard.Content.Factions
{
    public static class FactionLoader
    {
        public static int Count => factions.Count;

        internal static readonly IList<ModFaction> factions = new List<ModFaction>();
        internal static readonly IList<string> factionNames = new List<string>();

        internal static int Register(ModFaction faction)
        {
            factions.Add(faction);
            factionNames.Add(faction.Name);
            return Count - 1;
        }

        public static ModFaction GetFaction(int type)
        {
            return type >= 0 && type < Count ? factions[type] : null;
        }

        public static ModFaction GetFaction(string name)
        {
            return GetFaction(factionNames.IndexOf(name));
        }

        internal static void Unload()
        {
            factions.Clear();
        }
    }
}
