using System.Collections.Generic;

namespace HardToLessHard.Content.Structures
{
    public static class StructureLoader
    {
        public static int Count => structures.Count;

        internal static readonly IList<ModStructure> structures = new List<ModStructure>();

        internal static int Register(ModStructure structure)
        {
            structures.Add(structure);
            return Count - 1;
        }

        public static ModStructure GetStructure(int type)
        {
            return type >= 0 && type < Count ? structures[type] : null;
        }

        internal static void Unload()
        {
            structures.Clear();
        }
    }
}
