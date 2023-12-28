using System.Collections.Generic;

namespace HardToLessHard.Content.Religions
{
    public static class DeityLoader
    {
        public static int Count => deities.Count;

        internal static readonly IList<ModDeity> deities = new List<ModDeity>();
        internal static readonly IList<string> deityNames = new List<string>();

        internal static int Register(ModDeity deity)
        {
            deities.Add(deity);
            deityNames.Add(deity.Name);
            return Count - 1;
        }

        public static ModDeity GetDeity(int type)
        {
            return type >= 0 && type < Count ? deities[type] : null;
        }

        public static ModDeity GetDeity(string name)
        {
            return GetDeity(deityNames.IndexOf(name));
        }

        internal static void Unload()
        {
            deities.Clear();
        }
    }
}
