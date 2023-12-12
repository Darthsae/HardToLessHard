using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace HardToLessHard.Common.Systems
{
    public class WorldGeneration : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // Add a GenPass immediately after the "Piles" pass. ExampleOreSystem explains this approach in more detail.
            int SunflowersIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Sunflowers"));

            //if (SunflowersIndex != -1) tasks.Insert(SunflowersIndex + 1, new ExamplePilesPass("Hard To Less Hard", 100f));
        }
    }
}
