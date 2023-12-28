using Terraria;

namespace HardToLessHard.Content.Quests
{
    public abstract class QuestCondition
    {
        public bool completed = false;

        public virtual bool CheckCompletion(Player player) { return completed; }

        public bool IsCompleted(string player)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name == player)
                {
                    return CheckCompletion(Main.player[i]);
                }
            }

            return false;
        }
    }
}
