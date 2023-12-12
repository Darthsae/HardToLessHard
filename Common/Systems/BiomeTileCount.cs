using System;
using Terraria.ModLoader;

namespace HardToLessHard.Common.Systems
{
    public class BiomeTileCount : ModSystem
    {
        public int[] blockCount = new int[(int)Biomes.length];

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            for (int biomeIndex = 0; biomeIndex < (int)Biomes.length; biomeIndex++)
            {
                blockCount[biomeIndex] = 0;

                if (HardToLessHard.biomes.Length > biomeIndex && HardToLessHard.biomes[biomeIndex].tiles.Length > 0)
                {
                    for (int tileIndex = 0; tileIndex < HardToLessHard.biomes[biomeIndex].tiles.Length; tileIndex++)
                    {
                        if (tileCounts.Contains(HardToLessHard.biomes[biomeIndex].tiles[tileIndex]))
                            blockCount[biomeIndex] += tileCounts[HardToLessHard.biomes[biomeIndex].tiles[tileIndex]];
                    }
                }
            }
        }
    }
}