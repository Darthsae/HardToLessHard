using HardToLessHard.Utilities.Loot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Terraria;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Quests
{
    public class ItemReward : QuestReward
    {
        public LootSim loot;

        public ItemReward(LootSim loot)
        {
            this.loot = loot;
        }

        public override void Reward(Player player)
        {
            List<Item> lute = loot.GetItems();

            foreach (Item drop in lute)
            {
                player.QuickSpawnItem(player.GetSource_Loot(), drop);
            }
        }
    }
}
