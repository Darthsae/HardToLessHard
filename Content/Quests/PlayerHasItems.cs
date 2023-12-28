using System.Collections.Generic;
using Terraria;

namespace HardToLessHard.Content.Quests
{
    public class PlayerHasItems : QuestCondition
    {
        public List<Amount> Items = new ();

        public PlayerHasItems(List<Amount> items)
        {
            Items = items;
        }

        public override bool CheckCompletion(Player player)
        {
            bool toReturn = true;

            for (int i = 0; i < Items.Count; i++)
            {
                Amount item = Items[i];

                if (item.had) continue;

                if (player.CountItem(item.type.type) >= item.amount) Items[i].had = true;
                else toReturn = false;
            }

            return toReturn;
        }
    }

    public class Amount
    {
        public Item type;
        public int amount;
        public bool had;

        public Amount()
        {
            Item item = new();
            item.SetDefaults(0);
            type = item;
            amount = 0;
            had = false;
        }

        public Amount(Item type, int amount)
        {
            this.type = type;
            this.amount = amount;
            had = false;
        }
    }
}
