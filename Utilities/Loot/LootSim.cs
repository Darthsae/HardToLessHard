using System;
using System.Collections.Generic;
using Terraria;

namespace HardToLessHard.Utilities.Loot
{
    public struct LootSim
    {
        public List<IChoice> choices;

        public LootSim(List<IChoice> choices) 
        { 
            this.choices = choices;
        }

        public void AddChoice(IChoice choice)
        {
            choices.Add(choice);
        }

        public List<Item> GetItems()
        {
            List<Item> toReturn = new();

            foreach (IChoice choice in choices)
            {
                toReturn.AddRange(choice.GetItems());
            }

            return toReturn;
        }
    }

    public interface IChoice
    {
        public List<Item> GetItems();
    }

    public struct WeightedChoice : IChoice
    {
        public List<Item> items;

        public WeightedChoice(List<Tuple<Item, int>> Items) 
        {
            items = new List<Item>();

            foreach (Tuple<Item, int> item in Items)
            {
                AddItem(item.Item1, item.Item2);
            }
        }

        public List<Item> GetItems()
        {
            return new List<Item>
            {
                Main.rand.NextFromCollection(items)
            };
        }

        public void AddItem(Item item, int amount)
        {
            for (int i = 0; i < amount; i++) items.Add(item);
        }
    }

    public struct NumberedChoice : IChoice
    {
        public int min;
        public int max;

        public Item item;

        public NumberedChoice(int min, int max, Item item) 
        {
            this.min = min;
            this.max = max;
            this.item = item;
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();

            for (int i = 0; i < Main.rand.Next(min, max); i++) items.Add(item);

            return items;
        }
    }
}
