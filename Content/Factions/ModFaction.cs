using HardToLessHard.Common.Players;
using HardToLessHard.Content.NPCs;
using HardToLessHard.Content.Projectiles;
using HardToLessHard.Content.Religions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Factions
{
    public abstract class ModFaction : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Factions";
        public int Type { get; internal set; }
        public short[] relations { get; set; }

        public virtual short defaultRelation => 100;

        public virtual string Icon => (GetType().Namespace + "." + Name + "_Icon").Replace('.', '/');

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

    internal class FactionUIElement : UIImageButton
    {
        internal string Faction;
        internal UIImage icon;
        internal UIText name;
        private readonly float _scale;


        public FactionUIElement(Asset<Texture2D> background, string faction, float scale = 1f) : base(background)
        {
            Faction = faction;
            _scale = scale;
            icon = new UIImage(ModContent.Request<Texture2D>(FactionLoader.GetFaction(Faction).Icon));
            icon.Left.Set(10, 0f);
            icon.Top.Set(10, 0f);
            name = new UIText(FactionLoader.GetFaction(Faction).DisplayName);
            name.Width.Set(88, 0f);
            name.Height.Set(32, 0f);
            name.Left.Set(52, 0f);
            name.Top.Set(10, 0f);
            Append(icon);
            Append(name);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
