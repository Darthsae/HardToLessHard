using HardToLessHard.Common.Systems;
using HardToLessHard.Content.Factions;
using HardToLessHard.Content.Items;
using HardToLessHard.Content.NPCs;
using HardToLessHard.Content.Projectiles;
using HardToLessHard.Content.Quests;
using HardToLessHard.Content.Religions;
using HardToLessHard.Content.TileEntities;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace HardToLessHard.Common.Players
{
    public class HTLHPlayer : ModPlayer
    {
        public bool inConversation = false;
        public ExtendedModNPC conversationNPC = null;
        public bool testingUI = false;
        public bool deificAltarUI = false;
        public DeificAltarTileEntity deificAltarTileEntity = null;

        int timer = 0;

        public string faction = "NoFaction";

        public override void PostUpdate()
        {
            timer++;

            if (timer % 900 == 0)
            {
                foreach (ModQuest quest in QuestLoader.quests)
                {
                    quest.CheckQuest(Player.name);
                }
            }
        }

        public override bool ShiftClickSlot(Item[] inventory, int context, int slot)
        {
            if (context == ItemSlot.Context.InventoryItem && inventory[slot].type == ModContent.ItemType<Singularity>() && Player.HeldItem.type != ItemID.None && Player.HeldItem.type != ModContent.ItemType<Singularity>() && !(((Singularity)inventory[slot].ModItem).SingularityItemID < ItemLoader.ItemCount && ((Singularity)inventory[slot].ModItem).SingularityItemID > 0))
            {
                Singularity singularity = (Singularity)inventory[slot].ModItem;
                singularity.SingularityItemID = Player.HeldItem.type;
                singularity.SingularityItemAmount = Player.HeldItem.stack;
                inventory[slot].SetNameOverride($"Singularity: {Player.HeldItem.Name}");

                //Main.NewText("Shift T");
                return true;
            }

            return base.ShiftClickSlot(inventory, context, slot);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.SpiritGodKeybind.JustPressed && Player.GetModPlayer<SoulPlayer>().soulCurrent >= 60 && Player.GetModPlayer<HTLHPlayer>().faction == "Dead")
            {
                Player.GetModPlayer<SoulPlayer>().soulCurrent -= 60;

                // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
                Player.AddBuff(ModContent.BuffType<Content.Projectiles.Minions.SpiritGodMinionBuff>(), 2);

                // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
                var projectile = Projectile.NewProjectileDirect(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<Content.Projectiles.Minions.SpiritGodMinionBuff>())), Player.Center, Player.velocity, ModContent.ProjectileType<Content.Projectiles.Minions.SpiritGodMinion>(), 1, 1, Main.myPlayer);
                projectile.originalDamage = (int)Player.GetTotalDamage<SummonDamageClass>().ApplyTo(200f);
            }
            else if (KeybindSystem.TestingUI.JustPressed)
            {
                testingUI = !testingUI;
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return (npc.ModNPC is ExtendedModNPC extendedModNPC) ? ModFaction.IsFactionHostile(extendedModNPC.Faction, FactionLoader.GetFaction(faction)) : base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return (proj.ModProjectile is ExtendedModProjectile extendedModProjectile) ? ModFaction.IsFactionHostile(extendedModProjectile.Faction, FactionLoader.GetFaction(faction)) : base.CanBeHitByProjectile(proj);
        }

        public override bool CanHitNPC(NPC target)
        {
            return (target.ModNPC is ExtendedModNPC extendedModNPC) ? ModFaction.IsFactionHostile(extendedModNPC.Faction, FactionLoader.GetFaction(faction)) : base.CanHitNPC(target);
        }

        public override bool? CanHitNPCWithItem(Item item, NPC target)
        {
            return (target.ModNPC is ExtendedModNPC extendedModNPC) ? ModFaction.IsFactionHostile(extendedModNPC.Faction, FactionLoader.GetFaction(faction)) : base.CanHitNPCWithItem(item, target);
        }

        public override bool? CanHitNPCWithProj(Projectile proj, NPC target)
        {
            return (target.ModNPC is ExtendedModNPC extendedModNPC) ? ModFaction.IsFactionHostile(extendedModNPC.Faction, FactionLoader.GetFaction(faction)) : base.CanHitNPCWithProj(proj, target);
        }

        public override void LoadData(TagCompound tag)
        {
            faction = tag.GetString("faction");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["faction"] = faction;
        }

        public override void PostUpdateMiscEffects()
        {
            Filter bloodScreen = Filters.Scene["HardToLessHard:BloodScreen"];
            
            if (DeityLoader.GetDeity("Blorgun").enraged)
            {
                DeityLoader.GetDeity("Blorgun").Draw();

                if (!bloodScreen.IsActive()) Filters.Scene.Activate("HardToLessHard:BloodScreen");
                else bloodScreen.GetShader().UseProgress(Main.GlobalTimeWrappedHourly * 10);
            }
            else Filters.Scene.Deactivate("HardToLessHard:BloodScreen");
        }
    }
}
