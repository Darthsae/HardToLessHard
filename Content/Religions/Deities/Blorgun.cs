using HardToLessHard.Content.Factions;
using HardToLessHard.Content.NPCs;
using HardToLessHard.Content.Religions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Religions.Deities
{
    public class Blorgun : ModDeity
    {
        public string Beam => (GetType().Namespace + "." + Name + "_Beam").Replace('.', '/');

        public enum AI_State
        {
            IDLE,
            ATTACK
        }

        public enum Attack_State
        {
            GRID,
            POINT,
            REAPER,
            LENGTH
        }

        public NPC reaper = null;

        public int timer = 0;

        public int stages = 0;
        public int maxStages = 10;

        public int state = 0;
        public int attack = 0;

        public override Color Color => Color.DarkRed;

        public Vector2 attackPos = new Vector2();

        public Vector2 orbitPos = new Vector2();

        public Vector3[] beams = new Vector3[16];

        public Vector3[] bars = new Vector3[8];

        public override void InterfaceDraw(SpriteBatch spriteBatch)
        {
            Color drawColor = Color.White * MathF.Abs(MathF.Sin(timer / 120f));

            foreach (Vector3 beam in beams)
            {
                if (beam.Z > 0) spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Beam), new Rectangle((int)(beam.X - Main.screenPosition.X), 0, (int)(beam.Y * Main.GameZoomTarget), Main.screenHeight), drawColor);
            }

            foreach (Vector3 bar in bars)
            {
                if (bar.Z > 0) spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, (int)(bar.X - Main.screenPosition.Y), Main.screenWidth, (int)(bar.Y * Main.GameZoomTarget)), drawColor);
            }

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, 40), Color.Black);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, (Main.screenWidth / maxStages) * stages, 40), Color.MediumVioletRed);

            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Screen), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        }

        public override void SetStaticDefaults()
        {
            Faction = "Sanguine";
        }

        public override void AI()
        {
            for (int i = 0; i < beams.Length; i++)
            {
                if (beams[i].Z > 0) beams[i].Z -= 1; //Mod.Logger.Info($"{beams[i].X}, {beams[i].Y}, {beams[i].Z}");
            }

            for (int i = 0; i < bars.Length; i++)
            {
                if (bars[i].Z > 0) bars[i].Z -= 1; //Mod.Logger.Info($"{beams[i].X}, {beams[i].Y}, {beams[i].Z}");
            }

            foreach (Player player in Main.player)
            {
                for(int i = 0; i < beams.Length; i++)
                {
                    if (!player.dead && player.active && player != null && beams[i].Z >= 0 && BeamCheck(new Vector2(player.position.X, player.Hitbox.Height), new Vector2(beams[i].X, beams[i].Y))) player.Hurt(PlayerDeathReason.ByCustomReason("Blorgun struck them down"), player.statDefense + (player.statLife / 2), 0, armorPenetration: 1f); //beams[i].Z = 0;
                }

                for (int i = 0; i < bars.Length; i++)
                {
                    if (!player.dead && player.active && player != null && bars[i].Z >= 0 && BeamCheck(new Vector2(player.position.Y, player.Hitbox.Height), new Vector2(bars[i].X, bars[i].Y))) player.Hurt(PlayerDeathReason.ByCustomReason("Blorgun struck them down"), player.statDefense + (player.statLife / 2), 0, armorPenetration: 1f); //beams[i].Z = 0;
                }
            }

            switch (state)
            {
                case (int)AI_State.IDLE:
                    Idle();
                    break;
                case (int)AI_State.ATTACK:
                    Attack();
                    break;
            }

            if (stages >= maxStages)
            {
                enraged = false;
                target = -1;
                stages = 0;
            }
        }

        private static bool BeamCheck(Vector2 x, Vector2 y)
        {
            return !((x.X + x.Y < y.X) || (x.X > y.X + y.Y));
        }

        private void Idle()
        {
            timer--;
            if (timer <= 0)
            {
                attackPos = Main.screenPosition;
                attack = Main.rand.Next((int)Attack_State.LENGTH);
                state = (int)AI_State.ATTACK;
            }
        }

        private void Attack()
        {
            timer++;

            switch (attack)
            {
                case (int)Attack_State.GRID:
                    if (timer % 20 == 0)
                    {
                        int i = timer / 20;
                        if (i < beams.Length)
                        {
                            beams[i] = new Vector3(attackPos.X + (Main.screenWidth / beams.Length) * i, 20, 500);
                            if (timer % 40 == 0 && (int)(i / 2) < bars.Length)
                            {
                                bars[(int)(i / 2)] = new Vector3(attackPos.Y + (Main.screenHeight / bars.Length) * (int)(i / 2), 20, 500);
                            }

                            SoundEngine.PlaySound(SoundID.DrumTomLow);
                        }
                        else
                        {
                            stages++;
                            state = (int)AI_State.IDLE;
                        }

                    }
                    break;
                case (int)Attack_State.POINT:
                    if (timer % 50 == 0)
                    {
                        Mod.Logger.Info(target);
                        int i = timer / 50;
                        Player targetPlayer = Main.player[target];

                        if (i < 4)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (Main.rand.NextBool())
                                {
                                    for (int j = 0; j < Main.rand.Next(16, 65); j++)
                                    {
                                        Vector2 position = targetPlayer.Center + Main.rand.NextVector2CircularEdge(320f, 320f);

                                        Projectile.NewProjectileDirect(Entity.GetSource_NaturalSpawn(), position, (targetPlayer.Center - position).SafeNormalize(Vector2.UnitX) * 15f, Main.rand.NextBool() ? ProjectileID.DD2DarkMageBolt : ProjectileID.FrostBlastHostile, targetPlayer.statDefense + (targetPlayer.statLife / 8), 1).tileCollide = false;
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < Main.rand.Next(16, 65); j++)
                                    {
                                        Vector2 position = targetPlayer.Center + new Vector2(Main.rand.Next(-1600, 1600), -2400);

                                        Projectile.NewProjectileDirect(Entity.GetSource_NaturalSpawn(), position, (targetPlayer.Center + new Vector2(0, -150) - position).SafeNormalize(Vector2.UnitX) * 75f, Main.rand.NextBool() ? ProjectileID.DD2DarkMageBolt : ProjectileID.FrostBlastHostile, targetPlayer.statDefense + (targetPlayer.statLife / 8), 1).tileCollide = false;
                                    }
                                }
                                
                            }

                            SoundEngine.PlaySound(SoundID.DrumTamaSnare);
                        }
                        else
                        {
                            stages++;
                            state = (int)AI_State.IDLE;
                        }
                    }
                    break;
                case (int)Attack_State.REAPER:
                    if (reaper == null)
                    {
                        Player targetPlayer = Main.player[target];
                        reaper = NPC.NewNPCDirect(Entity.GetSource_NaturalSpawn(), targetPlayer.Center + Main.rand.NextVector2CircularEdge(400f, 400f), ModContent.NPCType<FactionReaper>(), target: target);
                        ExtendedModNPC extendedModNPC = (ExtendedModNPC)reaper.ModNPC;
                        extendedModNPC.Faction = Faction;
                        extendedModNPC.Religion = Religion;
                        extendedModNPC.Deity = Name;
                        extendedModNPC.NPC.color = new Color((FactionLoader.GetFaction(Faction).Color * (1/3)).ToVector3() + (ReligionLoader.GetReligion(Religion).Color * (1 /3)).ToVector3() + (Color * (1 /3)).ToVector3());
                        extendedModNPC.NPCTarget = false;
                        extendedModNPC.NPC.target = target;
                    }
                    else if (reaper.active)
                    {
                        if (timer % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Player targetPlayer = Main.player[target];

                            for (int j = 0; j < Main.rand.Next(16, 33); j++)
                            {
                                Vector2 position = targetPlayer.Center + new Vector2(Main.rand.Next(-3200, 3200), Main.rand.Next(-2400, 2400));

                                Projectile.NewProjectileDirect(Entity.GetSource_NaturalSpawn(), position, (targetPlayer.Center + Main.rand.NextVector2Circular(120, 120) - position).SafeNormalize(Vector2.UnitX) * 75f, Main.rand.NextBool() ? ProjectileID.DD2BetsyFlameBreath : ProjectileID.BloodShot, targetPlayer.statDefense + (targetPlayer.statLife / 8), 1).tileCollide = false;
                            }
                        }
                    }
                    else
                    {
                        timer = 200;
                        stages++;
                        state = (int)AI_State.IDLE;
                    }
                    break;
            }
            
        }
    }
}
