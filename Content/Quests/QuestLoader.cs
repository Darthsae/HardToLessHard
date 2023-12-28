using System.Collections.Generic;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Quests
{
    public static class QuestLoader
    {
        public static int Count => quests.Count;

        internal static readonly IList<ModQuest> quests = new List<ModQuest>();
        internal static readonly IList<string> questNames = new List<string>();

        internal static int Register(ModQuest quest)
        {
            quests.Add(quest);
            questNames.Add(quest.Name);
            return Count - 1;
        }

        public static bool HasPlayerCompletedQuest(string questName, string player)
        {
            return GetQuest(questName).completed.Contains(player);
        }

        public static ModQuest GetQuest(int type)
        {
            return type >= 0 && type < Count ? quests[type] : null;
        }

        public static ModQuest GetQuest(string name)
        {
            return GetQuest(questNames.IndexOf(name));
        }

        internal static void Unload()
        {
            quests.Clear();
        }
    }
}
