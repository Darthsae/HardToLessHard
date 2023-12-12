using HardToLessHard.Common.Players;
using HardToLessHard.Content.NPCs;
using HardToLessHard.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Factions
{
    public abstract class ModFaction : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Factions";
        public int Type { get; internal set; }
        public short[] relations { get; set; }

        public virtual Color Color => Color.White;

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        protected sealed override void Register()
        {
            ModTypeLookup<ModFaction>.Register(this);
            Type = FactionLoader.Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public virtual void RelationSetup() { }

        public static bool IsFactionHostile(string thisFaction, ModFaction otherFaction)
        {
            return FactionLoader.GetFaction(thisFaction).relations[otherFaction.Type] <= -50;
        }

        public static bool IsFactionHostile(string thisFaction, string otherFaction)
        {
            return FactionLoader.GetFaction(thisFaction).relations[FactionLoader.GetFaction(otherFaction).Type] <= -50;
        }

        public static bool IsFactionHostile(ModFaction thisFaction, ModFaction otherFaction)
        {
            return thisFaction.relations[otherFaction.Type] <= -50;
        }

        public static bool IsFactionAllied(string thisFaction, ModFaction otherFaction)
        {
            return FactionLoader.GetFaction(thisFaction).relations[otherFaction.Type] >= 50;
        }

        public static bool IsFactionAllied(string thisFaction, string otherFaction)
        {
            return FactionLoader.GetFaction(thisFaction).relations[FactionLoader.GetFaction(otherFaction).Type] >= 50;
        }

        public static bool IsFactionAllied(ModFaction thisFaction, ModFaction otherFaction)
        {
            return thisFaction.relations[otherFaction.Type] >= 50;
        }

        public static bool IsNPCHostile(string thisFaction, NPC npc)
        {
            if (npc == null || !npc.active || npc.immortal) {
                return false; 
            }

            if (npc.ModNPC is ExtendedModNPC extendedModNPC) {
                //Logging.PublicLogger.Info("B");
                return IsFactionHostile(thisFaction, extendedModNPC.Faction); 
            }

            //Logging.PublicLogger.Info($"Not: {npc.GivenName}");
            return !(npc.friendly || npc.CountsAsACritter);
        }

        public static bool IsPlayerHostile(string thisFaction, Player player)
        {
            if (player == null || !player.active || player.dead) return false;
            if (player.TryGetModPlayer(out HTLHPlayer modPlayer)) return IsFactionHostile(thisFaction, FactionLoader.GetFaction(modPlayer.faction));

            return true;
        }

        public static bool IsProjectileHostile(string thisFaction, Projectile projectile)
        {
            if (projectile == null || !projectile.active) {
                return false;
            }

            if (projectile.ModProjectile is ExtendedModProjectile extendedModProjectile) {
                //Logging.PublicLogger.Info("Extended Mod Projectile");
                return IsFactionHostile(thisFaction, extendedModProjectile.Faction); 
            }

            //Logging.PublicLogger.Info($"Not: {projectile.Name}");
            return projectile.hostile;
        }
    }
}
