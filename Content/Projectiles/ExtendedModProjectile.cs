using HardToLessHard.Common.Players;
using HardToLessHard.Content.Factions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace HardToLessHard.Content.Projectiles
{
    public abstract class ExtendedModProjectile : ModProjectile
    {
        public string Faction = "NoFaction";

        public int[] buffs = Array.Empty<int>();

        public Color Color = Color.White;

        public override bool PreKill(int timeLeft)
        {
            //Mod.Logger.Info($"Faction: {Faction}");
            return base.PreKill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            bool y = ModFaction.IsNPCHostile(Faction, target);
            if (y) Mod.Logger.Info(target.GivenName);
            return y;
        }

        public override bool CanHitPlayer(Player target)
        {
            return ModFaction.IsFactionHostile(Faction, FactionLoader.GetFaction(target.GetModPlayer<HTLHPlayer>().faction));
        }

        public virtual void PreBuffsOnHitPlayer(Player target, Player.HurtInfo info)
        {

        }

        public virtual void PostBuffsOnHitPlayer(Player target, Player.HurtInfo info)
        {

        }

        public virtual void PreBuffsOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public virtual void PostBuffsOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            PreBuffsOnHitPlayer(target, info);

            if (buffs.Length > 0) { 
                foreach (var buff in buffs)
                {
                    target.AddBuff(buff, 600);
                }
            }

            PostBuffsOnHitPlayer(target, info);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            PreBuffsOnHitNPC(target, hit, damageDone);

            

            if (buffs.Length > 0) {
                foreach (var buff in buffs)
                {
                    target.AddBuff(buff, 600);
                }
            }

            PostBuffsOnHitNPC(target, hit, damageDone);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Get this frame on texture
            Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.TopLeft();

            // Applying lighting and draw current frame
            //Color drawColor = Projectile.GetAlpha(lightColor);
            Color drawColor = Color;
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }

        /*
        public override void PostDraw(Color lightColor)
        {
            Vector2 position = Projectile.TopRight - Main.screenPosition + new Vector2(0, -64);
            Rectangle rectangle = new((int)position.X, (int)position.Y, 128, 64);
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, Color.DarkGray);
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Faction, position, Color.Black);
        }
        */
    }
}
