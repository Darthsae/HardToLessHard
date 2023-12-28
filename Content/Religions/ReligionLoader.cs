using System.Collections.Generic;

namespace HardToLessHard.Content.Religions
{
    public static class ReligionLoader
    {
        public static int Count => religions.Count;

        internal static readonly IList<ModReligion> religions = new List<ModReligion>();
        internal static readonly IList<string> religionNames = new List<string>();

        internal static int Register(ModReligion religion)
        {
            religions.Add(religion);
            religionNames.Add(religion.Name);
            return Count - 1;
        }

        public static ModReligion GetReligion(int type)
        {
            return type >= 0 && type < Count ? religions[type] : null;
        }

        public static ModReligion GetReligion(string name)
        {
            return GetReligion(religionNames.IndexOf(name));
        }

        internal static void Unload()
        {
            religions.Clear();
        }
    }
}
