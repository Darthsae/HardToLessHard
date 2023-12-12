using HardToLessHard.Content.Spells;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Projectiles
{
    public class SpellProjectile : ExtendedModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public void SetModified(ModSpell modSpell, string faction)
        {
            Faction = faction;
            Color = modSpell.Color;

            Projectile.damage = modSpell.modifiers["damage"].ApplyToInt(25);
            Projectile.knockBack = modSpell.modifiers["knockBack"].ApplyToFloat(0f);
            Projectile.stopsDealingDamageAfterPenetrateHits = modSpell.modifiers["stopsDealingDamageAfterPenetrateHits"].ApplyToBool(false);
            Projectile.tileCollide = modSpell.modifiers["tileCollide"].ApplyToBool(false);
            Projectile.timeLeft = modSpell.modifiers["timeLeft"].ApplyToInt(100);
            Projectile.ignoreWater = modSpell.modifiers["ignoreWater"].ApplyToBool(true);
            Projectile.light = modSpell.modifiers["light"].ApplyToFloat(0f);
            Projectile.maxPenetrate = modSpell.modifiers["maxPenetrate"].ApplyToInt(1);
            Projectile.scale = modSpell.modifiers["scale"].ApplyToFloat(1f);

            Projectile.Hitbox = new Rectangle(Projectile.Hitbox.X, Projectile.Hitbox.Y, (int)(Projectile.Hitbox.Width * Projectile.scale), (int)(Projectile.Hitbox.Height * Projectile.scale));

            if (modSpell.buffs.Count > 0) buffs = modSpell.buffs.ToArray();

            /*
            foreach (var spellComponent in modSpell.SpellComponents)
            {
                Mod.Logger.Info($"{SpellComponentLoader.GetSpellComponent(spellComponent)}");
            }
            */

            /*
            // Log set values
            Mod.Logger.Info(faction);
            Mod.Logger.Info(Color);
            Mod.Logger.Info($"damage: {Projectile.damage}");
            Mod.Logger.Info($"knockBack: {Projectile.knockBack}");
            Mod.Logger.Info($"stopsDealingDamageAfterPenetrateHits: {Projectile.stopsDealingDamageAfterPenetrateHits}");
            Mod.Logger.Info($"tileCollide: {Projectile.tileCollide}");
            Mod.Logger.Info($"timeLeft: {Projectile.timeLeft}");
            Mod.Logger.Info($"ignoreWater: {Projectile.ignoreWater}");
            Mod.Logger.Info($"light: {Projectile.light}");
            Mod.Logger.Info($"maxPenetrate: {Projectile.maxPenetrate}");
            Mod.Logger.Info($"scale: {Projectile.scale}");
            */
        }

        public static int NewSpellProjectile(IEntitySource spawnSource, Vector2 position, Vector2 velocity, ModSpell modSpell, string faction = "NoFaction", int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
        {
            return NewSpellProjectile(spawnSource, position.X, position.Y, velocity.X, velocity.Y, modSpell, faction, Owner, ai0, ai1, ai2);
        }

        public static int NewSpellProjectile(IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, ModSpell modSpell, string faction = "NoFaction", int Owner = -1, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f)
        {
            if (Owner == -1) Owner = Main.myPlayer;
            int num = 1000;
            for (int i = 0; i < 1000; i++)
            {
                if (!Main.projectile[i].active)
                {
                    num = i;
                    break;
                }
            }
            if (num == 1000) num = Projectile.FindOldestProjectile();
            Projectile projectile = Main.projectile[num];
            projectile.SetDefaults(ModContent.ProjectileType<SpellProjectile>());
            modSpell.SetModifiers();

            ((SpellProjectile)projectile.ModProjectile).SetModified(modSpell, faction);

            projectile.friendly = true;
            projectile.hostile = true;

            Vector2 speed = new Vector2(SpeedX, SpeedY) * modSpell.modifiers["speed"].ApplyToFloat(1);

            projectile.position.X = X - projectile.width * 0.5f;
            projectile.position.Y = Y - projectile.height * 0.5f;
            projectile.owner = Owner;
            projectile.velocity.X = speed.X;
            projectile.velocity.Y = speed.Y;
            projectile.identity = num;
            projectile.gfxOffY = 0f;
            projectile.stepSpeed = 1f;
            projectile.wet = !projectile.ignoreWater && Collision.WetCollision(projectile.position, projectile.width, projectile.height);
            projectile.honeyWet = Collision.honey;
            projectile.shimmerWet = Collision.shimmer;
            Main.projectileIdentity[Owner, num] = num;
            if (Owner == Main.myPlayer)
            {
                projectile.ai[0] = ai0;
                projectile.ai[1] = ai1;
                projectile.ai[2] = ai2;
            }
            //if (ProjectileID.Sets.NeedsUUID[Projectile.]) projectile.projUUID = projectile.identity;
            if (Main.netMode != NetmodeID.SinglePlayer && Owner == Main.myPlayer) NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num);
            if (Owner == Main.myPlayer) Main.player[Owner].TryUpdateChannel(projectile);
            return num;
        }

    }
}
