using Microsoft.Xna.Framework;
using HardToLessHard.Common.Players;
using HardToLessHard.Content.Factions;
using Terraria;
using Terraria.ModLoader;
using System;
using HardToLessHard.Content.Projectiles;

namespace HardToLessHard.Content.NPCs
{
    public abstract class ExtendedModNPC : ModNPC
    {
        public string Faction = "NoFaction";

        public string Religion = "NoReligion";

        public string Deity = "NoDeity";

        public bool CanChatWith = false;

        public string[] buttonNames = new string[] { "Button 1", "Button 2" };

        public Vector2 TilePosition { get {return new Vector2((int)(NPC.BottomLeft.X / 16f), (int)(NPC.BottomLeft.Y / 16f)); } }

        public bool NPCTarget;

        public bool LineOfSightRequired = true;

        public float agroRange = 2000;

        public float[] expandedAI = new float[4];

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return ModFaction.IsFactionHostile(Faction, FactionLoader.GetFaction(target.GetModPlayer<HTLHPlayer>().faction));
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return ModFaction.IsFactionHostile(Faction, FactionLoader.GetFaction(player.GetModPlayer<HTLHPlayer>().faction));
        }

        public override bool CanHitNPC(NPC target)
        {
            return ModFaction.IsNPCHostile(Faction, target);
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            return ModFaction.IsNPCHostile(Faction, attacker);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (projectile.ModProjectile is ExtendedModProjectile && ((ExtendedModProjectile)projectile.ModProjectile).Faction.Equals(Faction)) return false;
            Mod.Logger.Info("Something Went Wrong On NPC");
            bool y = ModFaction.IsProjectileHostile(Faction, projectile);
            //if (y) Mod.Logger.Info(projectile.Name);
            return y;
        }

        public virtual void ButtonClicked(int buttonIndex)
        {

        }

        public bool CanMoveHere(Vector2 position)
        {
            return NPC.noTileCollide || !Collision.SolidCollision(position, NPC.width, NPC.height);
        }

        public bool CanStandHere(Vector2 position)
        {
            return NPC.noTileCollide || Collision.SolidCollision(position + new Vector2(0, NPC.height), NPC.width, 16);
        }

        public void TargetFaction(bool faceTarget = false, bool stayFocused = false)
        {
            if (stayFocused && NPC.target != -1)
            {
                if (NPCTarget && ModFaction.IsNPCHostile(Faction, Main.npc[NPC.target])) return;
                else if (!NPCTarget && ModFaction.IsPlayerHostile(Faction, Main.player[NPC.target])) return;
            }
            int closest = -1;
            float distance = agroRange*agroRange;
            bool npcTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!ModFaction.IsNPCHostile(Faction, npc)) continue;
                if (LineOfSightRequired && !AIGeneralFunctions.CanHitLine(NPC.Center, npc.Center)) continue;
                float tempDistance = NPC.DistanceSQ(npc.position);
                if (tempDistance < distance)
                {
                    //Mod.Logger.Info($"Success: {i} {tempDistance}");
                    distance = tempDistance;
                    closest = i;
                    npcTarget = true;
                }
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!ModFaction.IsPlayerHostile(Faction, player)) continue;
                if (LineOfSightRequired && !AIGeneralFunctions.CanHitLine(NPC.Center, player.Center)) continue;
                float tempDistance = NPC.DistanceSQ(player.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    closest = i;
                    npcTarget = false;
                }
            }

            NPC.target = closest;
            NPCTarget = npcTarget;

            if (closest != -1 && faceTarget) FaceTarget();
        }

        /*
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 position = NPC.TopRight - screenPos + new Vector2(0, -64);
            Rectangle rectangle = new((int)position.X, (int)position.Y, 128, 64);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, Color.DarkGray);
            spriteBatch.DrawString(FontAssets.MouseText.Value, Faction, position, Color.Black);
        }
        */

        public void FaceTarget(float distanceToSwap = 16)
        {
            TargetData target = new (!NPCTarget ? Main.player[NPC.target] : Main.npc[NPC.target]);

            bool right = target.Center.X > NPC.Center.X;

            if (MathF.Abs(target.Center.X - NPC.Center.X) > distanceToSwap)
            {
                if (right)
                {
                    NPC.direction = 1;
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.direction = -1;
                    NPC.spriteDirection = -1;
                }
            }
        }
    }

    public struct TargetData
    {
        public Vector2 position;
        public Vector2 Center;
        public Vector2 Size;
        public Vector2 Bottom;
        public Entity Entity;
        public int width;
        public int height;
        public Rectangle Hitbox;

        public TargetData(Entity entity)
        {
            position = entity.position;
            Center = entity.Center;
            Size = entity.Size;
            Bottom = entity.Bottom;
            Entity = entity;
            width = entity.width; 
            height = entity.height;
            Hitbox = entity.Hitbox;
        }

        public TargetData(NPC npc)
        {
            position = npc.position;
            Center = npc.Center;
            Size = npc.Size;
            Bottom = npc.Bottom;
            Entity = npc;
            width = npc.width;
            height = npc.height;
            Hitbox = npc.Hitbox;
        }

        public TargetData(Player player) 
        {
            position = player.position;
            Center = player.Center;
            Size = player.Size;
            Bottom = player.Bottom;
            Entity = player;
            width = player.width;
            height = player.height;
            Hitbox = player.Hitbox;
        }

        public override readonly string ToString()
        {
            return $"position: {position}, Center: {Center}, Size: {Size}, Bottom: {Bottom}, Entity: {Entity}, width: {width}, height: {height}, Hitbox: {Hitbox}";
        }
    }
}
