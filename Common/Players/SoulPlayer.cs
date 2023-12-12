using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace HardToLessHard.Common.Players
{
    public class SoulPlayer : ModPlayer
    {
        // Here we create a custom resource, similar to mana or health.
        // Creating some variables to define the current value of our example resource as well as the current maximum value. We also include a temporary max value, as well as some variables to handle the natural regeneration of this resource.
        public int soulCurrent; // Current value of our example resource
        public const int DefaultSoulMax = 100; // Default maximum value of example resource
        public int soulMax; // Buffer variable that is used to reset maximum resource to default value in ResetDefaults().
        public int soulMax2; // Maximum amount of our example resource. We will change that variable to increase maximum amount of our resource
        public float soulRegenRate; // By changing that variable we can increase/decrease regeneration rate of our resource
        internal int soulRegenTimer = 0; // A variable that is required for our timer
        public static readonly Color HealSoulResource = new(225, 225, 225); // We can use this for CombatText, if you create an item that replenishes soulCurrent.
        public int soulGems;


        // In order to make the Example Resource example straightforward, several things have been left out that would be needed for a fully functional resource similar to mana and health. 
        // Here are additional things you might need to implement if you intend to make a custom resource:
        // - Multiplayer Syncing: The current example doesn't require MP code, but pretty much any additional functionality will require this. ModPlayer.SendClientChanges and CopyClientState will be necessary, as well as SyncPlayer if you allow the user to increase soulMax.
        // - Save/Load permanent changes to max resource: You'll need to implement Save/Load to remember increases to your soulMax cap.
        // - Resource replenishment item: Use GlobalNPC.OnKill to drop the item. ModItem.OnPickup and ModItem.ItemSpace will allow it to behave like Mana Star or Heart. Use code similar to Player.HealEffect to spawn (and sync) a colored number suitable to your resource.

        public override void Initialize()
        {
            soulMax = DefaultSoulMax;
        }

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        // We need this to ensure that regeneration rate and maximum amount are reset to default values after increasing when conditions are no longer satisfied (e.g. we unequip an accessory that increases our resource)
        private void ResetVariables()
        {
            soulRegenRate = 1f;
            soulMax2 = soulMax + (soulGems * Content.Items.Consumables.SoulGem.SoulPerGem);
        }

        public override void PostUpdateMiscEffects()
        {
            UpdateResource();
        }

        public override void PostUpdate()
        {
            CapResourceGodMode();
        }

        // Lets do all our logic for the custom resource here, such as limiting it, increasing it and so on.
        private void UpdateResource()
        {
            // For our resource lets make it regen slowly over time to keep it simple, let's use soulRegenTimer to count up to whatever value we want, then increase currentResource.
            soulRegenTimer++; // Increase it by 60 per second, or 1 per tick.

            // A simple timer that goes up to 1 second, increases the soulCurrent by 1 and then resets back to 0.
            if (soulRegenTimer > 60 / soulRegenRate)
            {
                soulCurrent += 1;
                soulRegenTimer = 0;
            }

            // Limit soulCurrent from going over the limit imposed by soulMax.
            soulCurrent = Utils.Clamp(soulCurrent, 0, soulMax2);
        }

        private void CapResourceGodMode()
        {
            if (Main.myPlayer == Player.whoAmI && Player.creativeGodMode)
            {
                soulCurrent = soulMax2;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["soulCurrent"] = soulCurrent;
            tag["soulGems"] = soulGems;
        }

        public override void LoadData(TagCompound tag)
        {
            soulCurrent = tag.Get<int>("soulCurrent");
            soulGems = tag.Get<int>("soulGems");
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)HardToLessHard.MessageType.SoulPlayerSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)soulGems);
            packet.Send(toWho, fromWho);
        }

        // Called in ExampleMod.Networking.cs
        public void ReceivePlayerSync(BinaryReader reader)
        {
            soulGems = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            SoulPlayer clone = (SoulPlayer)targetCopy;
            clone.soulGems = soulGems;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            SoulPlayer clone = (SoulPlayer)clientPlayer;

            if (soulGems != clone.soulGems)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
    }
}
