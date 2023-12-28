using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using Terraria.Utilities;
using HardToLessHard.Content.Factions;
using HardToLessHard.Common.Players;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using System.Linq;
using System;
using HardToLessHard.Content.Projectiles;
using HardToLessHard.Content.Spells;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using HardToLessHard.Utilities.IK;

namespace HardToLessHard.Content.NPCs
{
    public class FactionReaper : ExtendedModNPC
    {
        public ref float AttackCounter => ref expandedAI[0];
        public ref float WorldCounter => ref expandedAI[1];
        public int State = 0;

        public ExtendedModProjectile[] scythes = new ExtendedModProjectile[4];

        public enum AI_State
        {
            IDLE,
            WALKING
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 3; // The total amount of frames the NPC has

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new ()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.friendly = false; // NPC Will not attack player
            NPC.width = 34;
            NPC.height = 78;
            NPC.aiStyle = -1;
            NPC.damage = 600;
            NPC.defense = 270;
            NPC.lifeMax = 265000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.lifeRegen = 15;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.CanBeReplacedByOtherNPCs = false;
            NPC.rarity = 10;
            NPC.reflectsProjectiles = true;
            NPC.takenDamageMultiplier = 0.5f;
            NPC.noTileCollide = true;
            LineOfSightRequired = false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            int buff = Main.rand.Next(BuffLoader.BuffCount);
            while (!Main.debuff[buff]) buff = Main.rand.Next(BuffLoader.BuffCount);
            target.AddBuff(buff, 300);
        }

        public override void OnSpawn(IEntitySource source)
        {
            Faction = Main.rand.NextFromCollection(FactionLoader.factionNames.ToList());

            NPC.GivenName = $"{FactionLoader.GetFaction(Faction).DisplayName} Reaper";
            NPC.color = FactionLoader.GetFaction(Faction).Color;

            for (int i = 0; i < 4; i++)
            {
                scythes[i] = (ExtendedModProjectile)Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, ModContent.ProjectileType<ReaperSpear>(), NPC.damage, 0f).ModProjectile;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor name in the bestiary.
				new FlavorTextBestiaryInfoElement("The Faction Reaper."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement("Mods.HardToLessHard.Bestiary.FactionReaper")
            });
        }

        public override void FindFrame(int frameHeight)
        {
			if (((int)Main.time / 10) % 2 == 0)
			{
                NPC.frame.Y += NPC.frame.Height;
                if (NPC.frame.Y > NPC.height * 2) NPC.frame.Y = 0;
            }
        }

        public override void AI()
        {
            WorldCounter++;

            TargetFaction(true, true);

            if (NPC.target != -1)
            {
                TargetData target = new (!NPCTarget ? Main.player[NPC.target] : Main.npc[NPC.target]);

                float distanceToTarget = Vector2.Distance(NPC.Center, target.Center);

                if (WorldCounter % 60 == 0)
                {
                    Dust.QuickDustLine(NPC.Center, target.Center, distanceToTarget / 4f, Color.BlueViolet);
                    //Mod.Logger.Debug(target.ToString());
                }

                Vector2 targetPosition = target.Center + Main.rand.NextVector2Circular(8, 8);

                float maxVelocity = 4;

                if (targetPosition.X < NPC.Center.X && NPC.velocity.X > -maxVelocity)
                {
                    NPC.velocity.X -= 0.44f;
                }
                else if (targetPosition.X > NPC.Center.X && NPC.velocity.X < maxVelocity)
                {
                    NPC.velocity.X += 0.44f;
                }

                if (targetPosition.Y < NPC.Center.Y && NPC.velocity.Y > -maxVelocity)
                {
                    NPC.velocity.Y -= 0.44f;
                }
                else if (targetPosition.Y > NPC.Center.Y && NPC.velocity.Y < maxVelocity)
                {
                    NPC.velocity.Y += 0.44f;
                }

                if (WorldCounter % 200 == 0)
                {
                    //Mod.Logger.Info(Faction);

                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            ((ExtendedModProjectile)Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(target.Center) * 4f, ModContent.ProjectileType<ReaperSpear>(), NPC.damage, 0f).ModProjectile).Faction = Faction;
                            break;
                        case 1:
                            for (int i = -2; i < 3; i++)
                            {
                                ((ExtendedModProjectile)Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(target.Center).RotatedBy(MathHelper.PiOver4 * i) * 4f, ModContent.ProjectileType<ReaperSpear>(), NPC.damage, 0f).ModProjectile).Faction = Faction;
                            }
                            break;
                        case 2:
                            for (int i = -1; i < 2; i++)
                            {
                                ((ExtendedModProjectile)Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(target.Center).RotatedBy(MathHelper.PiOver4 * i) * 4f, ModContent.ProjectileType<ReaperSpear>(), NPC.damage, 0f).ModProjectile).Faction = Faction;
                            }
                            break;
                        case 3:
                            ((ExtendedModProjectile)Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(target.Center) * 4f, ModContent.ProjectileType<ReaperSpear>(), NPC.damage, 0f).ModProjectile).Faction = Faction;
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                    }
                }
            }
            else
            {
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 0f;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Faction"] = Faction;
        }

        public override void LoadData(TagCompound tag)
        {
            Faction = tag.GetString("Faction");
            NPC.GivenName = $"{FactionLoader.GetFaction(Faction).DisplayName} Reaper";
            NPC.color = FactionLoader.GetFaction(Faction).Color;
        }
    }
}
