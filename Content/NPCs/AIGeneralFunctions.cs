using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace HardToLessHard.Content.NPCs
{
    internal abstract class AIGeneralFunctions : ExtendedModNPC
    {
        public static bool CanHitLine(Vector2 StartCenter, Vector2 EndCenter)
        {
            int startX = Math.Clamp((int)(StartCenter.X / 16f), 1, Main.maxTilesX - 1);
            int startY = Math.Clamp((int)(StartCenter.Y / 16f), 1, Main.maxTilesY - 1);
            int endX = Math.Clamp((int)(EndCenter.X / 16f), 1, Main.maxTilesX - 1);
            int endY = Math.Clamp((int)(EndCenter.Y / 16f), 1, Main.maxTilesY - 1);
            float distanceX = Math.Abs(startX - endX);
            float distanceY = Math.Abs(startY - endY);
            if (distanceX == 0f && distanceY == 0f)
            {
                return true;
            }
            float ratioX = 1f;
            float ratioY = 1f;
            if (distanceX == 0f || distanceY == 0f)
            {
                if (distanceX == 0f) ratioX = 0f;
                if (distanceY == 0f) ratioY = 0f;
            }
            else if (distanceX > distanceY)
            {
                ratioX = distanceX / distanceY;
            }
            else
            {
                ratioY = distanceY / distanceX;
            }
            float accumulatedRatioX = 0f;
            float accumulatedRatioY = 0f;
            int direction = (startY < endY) ? 2 : 1;
            int remainingTilesX = (int)distanceX;
            int remainingTilesY = (int)distanceY;
            int signX = Math.Sign(endX - startX);
            int signY = Math.Sign(endY - startY);
            bool reachedEnd = false;
            bool reachedEndInSingleDirection = false;
            try
            {
                do
                {
                    switch (direction)
                    {
                        case 2:
                            {
                                accumulatedRatioX += ratioX;
                                int num17 = (int)accumulatedRatioX;
                                accumulatedRatioX %= 1f;
                                for (int j = 0; j < num17; j++)
                                {
                                    if (Main.tile[startX, startY - 1] == null || Main.tile[startX, startY] == null || Main.tile[startX, startY + 1] == null) return false;
                                    
                                    Tile tile4 = Main.tile[startX, startY - 1];
                                    Tile tile5 = Main.tile[startX, startY + 1];
                                    Tile tile6 = Main.tile[startX, startY];
                                    
                                    if (IsTileValid(tile4) || IsTileValid(tile5) || IsTileValid(tile6)) return false;
                                    
                                    if (remainingTilesX == 0 && remainingTilesY == 0)
                                    {
                                        reachedEnd = true;
                                        break;
                                    }

                                    startX += signX;
                                    remainingTilesX--;

                                    if (remainingTilesX == 0 && remainingTilesY == 0 && num17 == 1) reachedEndInSingleDirection = true;
                                }
                                if (remainingTilesY != 0) direction = 1;
                                break;
                            }
                        case 1:
                            {
                                accumulatedRatioY += ratioY;
                                int num16 = (int)accumulatedRatioY;
                                accumulatedRatioY %= 1f;
                                for (int i = 0; i < num16; i++)
                                {
                                    if (Main.tile[startX - 1, startY] == null || Main.tile[startX, startY] == null || Main.tile[startX + 1, startY] == null) return false;
                                    
                                    Tile tile = Main.tile[startX - 1, startY];
                                    Tile tile2 = Main.tile[startX + 1, startY];
                                    Tile tile3 = Main.tile[startX, startY];

                                    if (IsTileValid(tile) || IsTileValid(tile2) || IsTileValid(tile3)) return false;

                                    if (remainingTilesX == 0 && remainingTilesY == 0)
                                    {
                                        reachedEnd = true;
                                        break;
                                    }

                                    startY += signY;
                                    remainingTilesY--;

                                    if (remainingTilesX == 0 && remainingTilesY == 0 && num16 == 1) reachedEndInSingleDirection = true;
                                }
                                if (remainingTilesX != 0) direction = 2;
                                break;
                            }
                    }
                    if (Main.tile[startX, startY] == null) return false;

                    Tile tile7 = Main.tile[startX, startY];

                    if (!tile7.IsActuated && tile7.HasTile && Main.tileSolid[tile7.TileType] && !Main.tileSolidTop[tile7.TileType]) return false;
                }
                while (!(reachedEnd || reachedEndInSingleDirection));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsTileValid(Tile tile)
        {
            return !tile.IsActuated && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        }
    }
}
