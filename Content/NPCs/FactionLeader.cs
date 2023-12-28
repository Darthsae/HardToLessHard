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
    public class FactionLeader : ExtendedModNPC
    {
        public const string ShopName = "Shop";

        public ref float AttackCounter => ref expandedAI[0];
        public ref float WorldCounter => ref expandedAI[1];
        public int State = 0;

        public enum AI_State
        {
            IDLE,
            WALKING
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 16; // The total amount of frames the NPC has
            NPCID.Sets.NoTownNPCHappiness[Type] = true; // Prevents the happiness button

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
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = false; // NPC Will not attack player
            NPC.width = 46;
            NPC.height = 64;
            NPC.aiStyle = -1;
            NPC.damage = 600;
            NPC.defense = 70;
            NPC.lifeMax = 2000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.lifeRegen = 15;
            TownNPCStayingHomeless = true;
            LineOfSightRequired = false;
            CanChatWith = true;
            buttonNames[1] = "Dread";
        }

        public override void OnSpawn(IEntitySource source)
        {
            Faction = Main.rand.NextFromCollection(FactionLoader.factionNames.ToList());
            NPC.GivenName = $"{FactionLoader.GetFaction(Faction).DisplayName} Leader";
            NPC.color = FactionLoader.GetFaction(Faction).Color;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor name in the bestiary.
				new FlavorTextBestiaryInfoElement("The Faction Leader."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement("Mods.HardToLessHard.Bestiary.FactionLeader")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            /*
            NPC.frame.Width = 46;
			if (((int)Main.time / 10) % 2 == 0)
			{
                NPC.frame.X = 40;
			}
			else
			{
                NPC.frame.X = 0;
			}*/
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            // These are things that the NPC has a chance of telling you when you talk to it.
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.CommonDialogue"), 5.0);
            chat.Add(Language.GetTextValue("Mods.HardToLessHard.Dialogue.FactionLeader.RareDialogue"), 0.1);

            string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

            return chosenChat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            //button = Main.LocalPlayer.GetModPlayer<HTLHPlayer>().faction != Faction ? $"Join {FactionLoader.GetFaction(Faction).DisplayName}" : Language.GetTextValue("LegacyInterface.28");
            button = "Talk";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                // We want 3 different functionalities for chat buttons, so we use HasItem to change button 1 between a shop and upgrade action.
                /*
                if (Main.LocalPlayer.GetModPlayer<HTLHPlayer>().faction != Faction)
                {
                    SoundEngine.PlaySound(SoundID.Item37); // Reforge/Anvil sound

                    Main.npcChatText = $"Welcome to {FactionLoader.GetFaction(Faction).DisplayName}.";

                    Main.LocalPlayer.GetModPlayer<HTLHPlayer>().faction = Faction;

                    return;
                }

                shop = ShopName; // Name of the shop tab we want to open.
                */

                if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer))
                {
                    if (modPlayer.faction != Faction)
                    {
                        if (FactionLoader.GetFaction(Faction).relations[FactionLoader.factionNames.IndexOf(modPlayer.faction)] > -50)
                        {
                            buttonNames[0] = "Join";
                        }
                        else
                        {
                            buttonNames[0] = "Challenge";
                        }
                        
                    }
                    else
                    {
                        buttonNames[0] = "Leave";
                    }

                    modPlayer.inConversation = true;
                    modPlayer.conversationNPC = this;

                    Main.CloseNPCChatOrSign();
                }
            }
        }

        public override void ButtonClicked(int buttonIndex)
        {
            HTLHPlayer modPlayer = Main.LocalPlayer.GetModPlayer<HTLHPlayer>();
            switch (buttonIndex)
            {
                case 0:
                    switch (buttonNames[0])
                    {
                        case "Join":
                            modPlayer.faction = Faction;
                            break;
                        case "Leave":
                            modPlayer.faction = "NoFaction";
                            break;
                        case "Challenge":
                            Mod.Logger.Info("Challenge");
                            break;
                    }
                    break;
                case 1:
                    Mod.Logger.Info("Button 2");
                    break;
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add(ItemID.Bone)
                .Add(ItemID.Ectoplasm)
                .Add(ItemID.ZombieArm)
                .Add(new Item(ModContent.ItemType<Content.Items.Consumables.LesserSoulPotion>()) { shopCustomPrice = Item.buyPrice(0, 0, 2, 15) });
            npcShop.Register(); // Name of this shop tab
        }

        public override void AI()
        {
            WorldCounter++;

            TargetFaction(true, false);

            /*
            if (WorldCounter % 120 == 0)
            {
                Mod.Logger.Info(NPC.target);
            } 
            */

            if (NPC.target != -1)
            {
                TargetData target = new (!NPCTarget ? Main.player[NPC.target] : Main.npc[NPC.target]);

                float distanceToTarget = Vector2.Distance(NPC.Center, target.Center);

                if (WorldCounter % 60 == 0)
                {
                    Dust.QuickDustLine(NPC.Center, target.Center, distanceToTarget / 4f, Color.BlueViolet);
                    //Mod.Logger.Debug(target.ToString());
                }

                if (distanceToTarget > 2000)
                {
                    Vector2 position;

                    for (int i = 0; i < 60;  i++)
                    {
                        position = target.position + Main.rand.NextVector2Circular(150, 150);
                        if (CanMoveHere(position))
                        {
                            NPC.position = position;
                            break;
                        }
                    }
                }
                else if (distanceToTarget > 500)
                {
                    Faction_Leader_Fighter_AI(target);
                }
                else
                {
                    NPC.velocity.X = ((NPC.position.Y > target.position.Y)  ? -4 : 2) * ((NPC.position.X - target.position.X) / Math.Abs(NPC.position.X - target.position.X));

                    if (MathF.Abs(NPC.position.Y - target.position.Y) > 100) NPC.velocity.Y = -3f;
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (AttackCounter > 0) AttackCounter--; // tick down the attack counter.

                    if (AttackCounter <= 0 && distanceToTarget < 3500)
                    {
                        if (AIGeneralFunctions.CanHitLine(NPC.Center, target.Center))
                        {
                            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);

                            int complexity = Main.rand.Next(12) + 3;

                            ModSpell modSpell = new()
                            {
                                SpellComponents = new int[complexity]
                            };

                            //Mod.Logger.Info(SpellComponentLoader.Count);

                            for (int i = 0; i < complexity; i++)
                            {
                                modSpell.SpellComponents[i] = Main.rand.Next(SpellComponentLoader.Count - 1);
                            }

                            //Mod.Logger.Info(Faction);

                            SpellProjectile.NewSpellProjectile(NPC.GetSource_FromThis(), NPC.Center + direction * 32, direction * 4, modSpell, Faction, Main.myPlayer);
                            //Mod.Logger.Info(((SpellProjectile)Main.projectile[spell].ModProjectile).Faction);
                        }
                        else
                        {
                            for (int j = -9; j < 10; j++)
                            {
                                Vector2 position = target.Center + new Vector2(j , -200);
                                Vector2 direction = (target.Center - position).SafeNormalize(Vector2.UnitX);

                                int complexity = Main.rand.Next(12) + 3;

                                ModSpell modSpell = new()
                                {
                                    SpellComponents = new int[complexity]
                                };

                                //Mod.Logger.Info(SpellComponentLoader.Count);

                                for (int i = 0; i < complexity; i++)
                                {
                                    modSpell.SpellComponents[i] = Main.rand.Next(SpellComponentLoader.Count - 1);
                                }

                                SpellProjectile.NewSpellProjectile(NPC.GetSource_FromThis(), position, direction * 4, modSpell, Faction, Main.myPlayer);
                            }
                        }
                        AttackCounter = 120;
                        NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                NPC.velocity.X = 0f;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["Faction"] = Faction;
        }

        public override void LoadData(TagCompound tag)
        {
            Faction = tag.GetString("Faction");
            NPC.GivenName = $"{FactionLoader.GetFaction(Faction).DisplayName} Leader";
            NPC.color = FactionLoader.GetFaction(Faction).Color;
        }

        private void Faction_Leader_Fighter_AI(TargetData target)
        {
            if (target.position.Y + target.height == NPC.position.Y + NPC.height) NPC.directionY = -1;
            bool HFlag1 = false;
            int num53 = 60;

            if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= num53 || (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0))))
            {
                NPC.ai[3] += 1f;
            }
            else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
            {
                NPC.ai[3] -= 1f;
            }

            if (NPC.ai[3] > (num53 * 10) || NPC.justHit) NPC.ai[3] = 0f;
            if (NPC.ai[3] == num53) NPC.netUpdate = true;
            if (target.Hitbox.Intersects(NPC.Hitbox)) NPC.ai[3] = 0f;
            if (NPC.direction == 0) NPC.direction = 1;

            float num77 = 1.5f * (1f + (1f - NPC.scale));
            if (NPC.velocity.X < 0f - num77 || NPC.velocity.X > num77)
            {
                if (NPC.velocity.Y == 0f) NPC.velocity *= 0.8f;
            }
            else if (NPC.velocity.X < num77 && NPC.direction == 1)
            {
                NPC.velocity.X += 0.07f;
                if (NPC.velocity.X > num77) NPC.velocity.X = num77;
            }
            else if (NPC.velocity.X > 0f - num77 && NPC.direction == -1)
            {
                NPC.velocity.X -= 0.07f;
                if (NPC.velocity.X < 0f - num77) NPC.velocity.X = 0f - num77;
            }

            if (NPC.velocity.Y == 0f)
            {
                int num178 = (int)(NPC.position.Y + NPC.height + 7f) / 16;
                int num179 = (int)(NPC.position.Y - 9f) / 16;
                int num180 = (int)NPC.position.X / 16;
                int num181 = (int)(NPC.position.X + NPC.width) / 16;
                int num182 = (int)(NPC.position.X + 8f) / 16;
                int num183 = (int)(NPC.position.X + NPC.width - 8f) / 16;
                bool flag22 = false;
                for (int num184 = num182; num184 <= num183; num184++)
                {
                    if (num184 >= num180 && num184 <= num181 && Main.tile[num184, num178] == null)
                    {
                        flag22 = true;
                        continue;
                    }
                    if (Main.tile[num184, num179] != null && Main.tile[num184, num179].HasUnactuatedTile && Main.tileSolid[Main.tile[num184, num179].TileType])
                    {
                        HFlag1 = false;
                        break;
                    }
                    HFlag1 = (!flag22 && num184 >= num180 && num184 <= num181 && Main.tile[num184, num178].HasUnactuatedTile && Main.tileSolid[Main.tile[num184, num178].TileType]);
                }
                if (!HFlag1 && NPC.velocity.Y < 0f) NPC.velocity.Y = 0f;
                if (flag22) return;
            }

            if (NPC.velocity.Y >= 0f && NPC.directionY != 1)
            {
                int num185 = 0;
                if (NPC.velocity.X < 0f)
                {
                    num185 = -1;
                }
                if (NPC.velocity.X > 0f)
                {
                    num185 = 1;
                }
                Vector2 position3 = NPC.position;
                position3.X += NPC.velocity.X;
                int num186 = (int)((position3.X + (NPC.width / 2) + ((NPC.width / 2 + 1) * num185)) / 16f);
                int num187 = (int)((position3.Y + NPC.height - 1f) / 16f);
                if (WorldGen.InWorld(num186, num187, 4))
                {

                    if (Main.tile[num186, num187] == null) Framing.GetTileSafely(num186, num187).ClearEverything();
                    if (Main.tile[num186, num187 - 1] == null) Framing.GetTileSafely(num186, num187 - 1).ClearEverything();
                    if (Main.tile[num186, num187 - 2] == null) Framing.GetTileSafely(num186, num187 - 2).ClearEverything();
                    if (Main.tile[num186, num187 - 3] == null) Framing.GetTileSafely(num186, num187 - 3).ClearEverything();
                    if (Main.tile[num186, num187 + 1] == null) Framing.GetTileSafely(num186, num187 + 1).ClearEverything();
                    if (Main.tile[num186 - num185, num187 - 3] == null) Framing.GetTileSafely(num186 - num185, num187 - 3).ClearEverything();

                    if ((num186 * 16) < position3.X + NPC.width && (num186 * 16 + 16) > position3.X && ((Main.tile[num186, num187].HasUnactuatedTile && !Main.tile[num186, num187].TopSlope && !Main.tile[num186, num187 - 1].TopSlope && Main.tileSolid[Main.tile[num186, num187].TileType] && !Main.tileSolidTop[Main.tile[num186, num187].TileType]) || (Main.tile[num186, num187 - 1].IsHalfBlock && Main.tile[num186, num187 - 1].HasUnactuatedTile)) && (!Main.tile[num186, num187 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num186, num187 - 1].TileType] || Main.tileSolidTop[Main.tile[num186, num187 - 1].TileType] || (Main.tile[num186, num187 - 1].IsHalfBlock && (!Main.tile[num186, num187 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num186, num187 - 4].TileType] || Main.tileSolidTop[Main.tile[num186, num187 - 4].TileType]))) && (!Main.tile[num186, num187 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num186, num187 - 2].TileType] || Main.tileSolidTop[Main.tile[num186, num187 - 2].TileType]) && (!Main.tile[num186, num187 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num186, num187 - 3].TileType] || Main.tileSolidTop[Main.tile[num186, num187 - 3].TileType]) && (!Main.tile[num186 - num185, num187 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num186 - num185, num187 - 3].TileType]))
                    {
                        float num188 = num187 * 16;
                        if (Main.tile[num186, num187].IsHalfBlock)
                        {
                            num188 += 8f;
                        }
                        if (Main.tile[num186, num187 - 1].IsHalfBlock)
                        {
                            num188 -= 8f;
                        }
                        if (num188 < position3.Y + NPC.height)
                        {
                            float num189 = position3.Y + NPC.height - num188;
                            float num190 = 16.1f;
                            if (num189 <= num190)
                            {
                                NPC.gfxOffY += NPC.position.Y + NPC.height - num188;
                                NPC.position.Y = num188 - NPC.height;
                                NPC.stepSpeed = num189 < 9f ? 1f : 2f;
                            }
                        }
                    }
                }
            }
            
            if (HFlag1)
            {
                int num191 = (int)((NPC.position.X + (NPC.width / 2) + (15 * NPC.direction)) / 16f);
                int num192 = (int)((NPC.position.Y + NPC.height - 15f) / 16f);
                if (Main.tile[num191, num192] == null) Framing.GetTileSafely(num191, num192);
                if (Main.tile[num191, num192 - 1] == null) Framing.GetTileSafely(num191, num192 - 1);
                if (Main.tile[num191, num192 - 2] == null) Framing.GetTileSafely(num191, num192 - 2);
                if (Main.tile[num191, num192 - 3] == null) Framing.GetTileSafely(num191, num192 - 3);
                if (Main.tile[num191, num192 + 1] == null) Framing.GetTileSafely(num191, num192 + 1).ClearEverything();
                if (Main.tile[num191 + NPC.direction, num192 - 1] == null) Framing.GetTileSafely(num191 + NPC.direction, num192 - 1).ClearEverything();
                if (Main.tile[num191 + NPC.direction, num192 + 1] == null) Framing.GetTileSafely(num191 + NPC.direction, num192 + 1).ClearEverything();
                if (Main.tile[num191 - NPC.direction, num192 + 1] == null) Framing.GetTileSafely(num191 - NPC.direction, num192 + 1).ClearEverything();
                //Framing.GetTileSafely(num191, num192 + 1).IsHalfBlock = true;

                if (Main.tile[num191, num192 - 1].HasUnactuatedTile && (Main.tile[num191, num192 - 1].TileType == 10 || Main.tile[num191, num192 - 1].TileType == 388))
                {
                    NPC.ai[2] += 1f;
                    NPC.ai[3] = 0f;
                    if (NPC.ai[2] >= 60f)
                    {
                        if (!Main.bloodMoon || Main.getGoodWorld) NPC.ai[1] = 0f;
                        //NPC.velocity.X = 0.5f * (-NPC.direction);
                        int num193 = Main.tile[num191, num192 - 1].TileType == 388  ? 2 : 5;
                        NPC.ai[1] += num193;
                        NPC.ai[2] = 0f;
                        bool flag25 = false;
                        if (NPC.ai[1] >= 10f)
                        {
                            flag25 = true;
                            NPC.ai[1] = 10f;
                        }
                        WorldGen.KillTile(num191, num192 - 1, fail: true);
                        if ((Main.netMode != NetmodeID.MultiplayerClient || !flag25) && flag25 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (Main.tile[num191, num192 - 1].TileType == 10)
                            {
                                bool flag26 = WorldGen.OpenDoor(num191, num192 - 1, NPC.direction);
                                if (!flag26)
                                {
                                    NPC.ai[3] = num53;
                                    NPC.netUpdate = true;
                                }
                                if (Main.netMode == NetmodeID.Server && flag26) NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num191, num192 - 1, NPC.direction);
                            }

                            if (Main.tile[num191, num192 - 1].TileType == 388)
                            {
                                bool flag27 = WorldGen.ShiftTallGate(num191, num192 - 1, closing: false);
                                if (!flag27)
                                {
                                    NPC.ai[3] = num53;
                                    NPC.netUpdate = true;
                                }
                                if (Main.netMode == NetmodeID.Server && flag27) NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num191, num192 - 1);
                            }
                        }
                    }
                }
                else
                {
                    int num194 = NPC.spriteDirection;
                    if ((NPC.velocity.X < 0f && num194 == -1) || (NPC.velocity.X > 0f && num194 == 1))
                    {
                        if (NPC.height >= 32 && Main.tile[num191, num192 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num191, num192 - 2].TileType])
                        {
                            if (Main.tile[num191, num192 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num191, num192 - 3].TileType])
                            {
                                NPC.velocity.Y = -8f;
                                NPC.netUpdate = true;
                            }
                            else
                            {
                                NPC.velocity.Y = -7f;
                                NPC.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num191, num192 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num191, num192 - 1].TileType])
                        {
                            NPC.velocity.Y = -6f;
                            NPC.netUpdate = true;
                        }
                        else if (NPC.position.Y + NPC.height - (num192 * 16) > 20f && Main.tile[num191, num192].HasUnactuatedTile && !Main.tile[num191, num192].TopSlope && Main.tileSolid[Main.tile[num191, num192].TileType])
                        {
                            NPC.velocity.Y = -5f;
                            NPC.netUpdate = true;
                        }
                        else if (NPC.directionY < 0 && (!Main.tile[num191, num192 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num191, num192 + 1].TileType]) && (!Main.tile[num191 + NPC.direction, num192 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num191 + NPC.direction, num192 + 1].TileType]))
                        {
                            NPC.velocity.Y = -8f;
                            NPC.velocity.X *= 1.5f;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            NPC.ai[1] = 0f;
                            NPC.ai[2] = 0f;
                        }

                        if (NPC.velocity.Y == 0f && (NPC.velocity.X == 0f && !NPC.justHit) && NPC.ai[3] == 1f) NPC.velocity.Y = -5f;

                        if (NPC.velocity.Y == 0f && Main.expertMode && target.Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - target.Center.X) < (target.width * 3) && Collision.CanHit(NPC, target.Entity))
                        {
                            if (NPC.velocity.Y == 0f)
                            {
                                int num197 = 6;
                                if (target.Bottom.Y > NPC.Top.Y - (num197 * 16))
                                {
                                    NPC.velocity.Y = -7.9f;
                                }
                                else
                                {
                                    int num198 = (int)(NPC.Center.X / 16f);
                                    int num199 = (int)(NPC.Bottom.Y / 16f) - 1;
                                    for (int num200 = num199; num200 > num199 - num197; num200--)
                                    {
                                        if (Main.tile[num198, num200].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num198, num200].TileType])
                                        {
                                            NPC.velocity.Y = -7.9f;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
            }
        }
    }
}
