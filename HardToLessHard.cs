using HardToLessHard.Content.Factions;
using HardToLessHard.Content.Religions;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard
{
    public enum Biomes
    {
        DIRT,
        length
    }

    public class HardToLessHard : Mod
	{
		public static HardToLessHard Instance;
		
		public static BiomeData[] biomes = new BiomeData[] { new BiomeData("Dirt", new int[] {TileID.Dirt}) };

        internal enum MessageType : byte
        {
            SoulPlayerSync
        }

        public override void Load()
        {
			Instance = this;

            if (Main.netMode != NetmodeID.Server)
            {
                Filters.Scene["HardToLessHard:BloodScreen"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("HardToLessHard/Assets/Effects/BloodScreenF", AssetRequestMode.ImmediateLoad).Value), "BloodScreen"), EffectPriority.VeryHigh);
                Filters.Scene["HardToLessHard:BloodScreen"].Load();
            }
        }

        public override void Unload()
        {
			Instance = null;
        }

        public override void PostSetupContent()
        {
            foreach (var f in FactionLoader.factions) 
            {
                f.relations = new short[FactionLoader.Count];
            }

            foreach (var f in FactionLoader.factions)
            {
                for (int i = 0; i < f.relations.Length; i++)
                {
                    f.relations[i] = FactionLoader.GetFaction(i).defaultRelation;
                }

                f.relations[f.Type] = 100;

                f.RelationSetup();

                Logger.Info("-----------------");
                Logger.Info(f.DisplayName);
                Logger.Info(f.Name);

                
            }

            foreach (var d in DeityLoader.deities)
            {
                Logger.Info("-----------------");
                Logger.Info(d.DisplayName);
                Logger.Info(d.Name);
            }
        }

        // Override this method to handle network packets sent for this mod.
        //TODO: Introduce OOP packets into tML, to avoid this god-class level hardcode.
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                // This message syncs ExampleStatIncreasePlayer.exampleLifeFruits and ExampleStatIncreasePlayer.exampleManaCrystals
                case MessageType.SoulPlayerSync:
                    byte playerNumber = reader.ReadByte();
                    Common.Players.SoulPlayer soulPlayer = Main.player[playerNumber].GetModPlayer<Common.Players.SoulPlayer>();
                    soulPlayer.ReceivePlayerSync(reader);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        // Forward the changes to the other clients
                        soulPlayer.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
                default:
                    Logger.WarnFormat("HardToLessHard: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }

	public struct BiomeData
	{
		public string name;
		public int[] tiles;

		public BiomeData(string name, int[] tiles)
		{
			this.name = name;
			this.tiles = tiles;
		}
	}
}