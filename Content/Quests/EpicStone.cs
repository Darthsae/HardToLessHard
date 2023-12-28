using System;
using System.Collections.Generic;
using Terraria;
using HardToLessHard.Utilities.Loot;
using Terraria.ModLoader;
using Terraria.ID;

namespace HardToLessHard.Content.Quests
{
    public class EpicStone : ModQuest
    {
        public override QuestCondition[] Conditions => new QuestCondition[] { new PlayerHasItems(new List<Amount>() { new Amount(new Item(ItemID.StoneBlock), 3) }) };

        public override QuestReward[] Rewards => new QuestReward[] { new ItemReward(new LootSim(new List<IChoice>() { new NumberedChoice(1, 2, new Item(ItemID.DirtBomb)) })) };
    }
}
