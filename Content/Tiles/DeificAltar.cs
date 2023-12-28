using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using HardToLessHard.Content.TileEntities;
using HardToLessHard.Content.Items.Placeable;
using HardToLessHard.Common.Players;

namespace HardToLessHard.Content.Tiles
{
    public class DeificAltar : ModTile
    {
        public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.newTile.CoordinatePadding = 1;
			DeificAltarTileEntity deificAltarTileEntity = ModContent.GetInstance<DeificAltarTileEntity>();
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(deificAltarTileEntity.Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);

			TileID.Sets.PreventsSandfall[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;
		}

		public override void PlaceInWorld(int i, int j, Item item)
		{
			DeificAltarTileEntity deificAltarTileEntity = ModContent.GetInstance<DeificAltarTileEntity>();
			deificAltarTileEntity.Place(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			DeificAltarTileEntity deificAltarTileEntity = ModContent.GetInstance<DeificAltarTileEntity>();

			HTLHPlayer htlhPlayer = Main.LocalPlayer.GetModPlayer<HTLHPlayer>();

			if (TileEntity.ByID[deificAltarTileEntity.Find(i, j)] != null)
			{
				htlhPlayer.deificAltarUI = true;
				htlhPlayer.deificAltarTileEntity = (DeificAltarTileEntity)TileEntity.ByID[deificAltarTileEntity.Find(i, j)];
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.cursorItemIconEnabled = true;
			Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<DeificAltarItem>();
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			ModContent.GetInstance<DeificAltarTileEntity>().Kill(i, j);
		}
	}
}
