using HardToLessHard.Content.Tiles;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.TileEntities
{
    public class DeificAltarTileEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
			return tile.HasTile && tile.TileType == ModContent.TileType<DeificAltar>();
        }

		/*
        public override void OnNetPlace()
        {
			NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
		}

        public override void NetSend(BinaryWriter writer)
        {
            base.NetSend(writer);
        }

        public override void NetReceive(BinaryReader reader)
        {
            base.NetReceive(reader);
        }
        */
	}
}
