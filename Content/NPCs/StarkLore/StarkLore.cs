using HardToLessHard.Content.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using HardToLessHard.Utilities.IK;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.DataStructures;

namespace HardToLessHard.Content.NPCs.StarkLore
{
	[AutoloadBossHead]
	public class StarkLore : ExtendedModNPC
	{
		public int detimer = 0;

		/*
        private const int TentacleSegments = 8;
        private const float TentacleLength = 60f;

        private IKSystem.Joint[] tentacleJoints = new IKSystem.Joint[TentacleSegments];
        private IKSystem.Limb[] tentacleLimbs = new IKSystem.Limb[TentacleSegments - 1];
		*/

        public override void SetDefaults() {
			NPC.width = 150;
			NPC.height = 230;
			NPC.damage = 300;
			NPC.defense = 50;
			NPC.lifeMax = 50000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 6000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			NPC.noGravity = true; // Not affected by gravity
			Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Stark Lore");
			SceneEffectPriority = SceneEffectPriority.BossLow;
			NPC.noTileCollide = true;
			NPC.boss = true;
			agroRange = 12000;
			LineOfSightRequired = false;
			Faction = "Dead";
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (NPC.downedMechBossAny == true){
				return SpawnCondition.OverworldNightMonster.Chance * 0.1f;
			} else {
				return 0f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			CommonDrop commonDrop = new CommonDrop(ModContent.ItemType<StrangeSkull>(), 2);
			npcLoot.Add(commonDrop);
		}

		/*
        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < TentacleSegments; i++)
            {
                tentacleJoints[i] = new IKSystem.Joint
                {
                    Position = NPC.Center,
                    Angle = 0f
                };

                if (i < TentacleSegments - 1)
                {
                    tentacleLimbs[i] = new IKSystem.Limb
                    {
                        Length = TentacleLength,
                        Angle = 0f
                    };
                }
            }
        }
		*/

        public override void AI() //this is where you program your AI
		{
			if (detimer >= 1200)
			{
				NPC.velocity.Y = float.MaxValue;
			}
			else 
			{
				TargetFaction(true, true);

				if (NPC.target != -1)
				{
					detimer = 0;

					TargetData target = new(!NPCTarget ? Main.player[NPC.target] : Main.npc[NPC.target]);

					float maxVelocity = 4 * (target.Center.Distance(NPC.Center) / 1000);

					if (target.Center.X < NPC.Center.X && NPC.velocity.X > -maxVelocity)
					{
						NPC.velocity.X -= 0.44f;
					}
					else if (target.Center.X > NPC.Center.X && NPC.velocity.X < maxVelocity)
					{
						NPC.velocity.X += 0.44f;
					}

					if (target.Center.Y < NPC.Center.Y && NPC.velocity.Y > -maxVelocity)
					{
						NPC.velocity.Y -= 0.44f;
					}
					else if (target.Center.Y > NPC.Center.Y && NPC.velocity.Y < maxVelocity)
					{
						NPC.velocity.Y += 0.44f;
					}

					/*
                    IKSystem.SolveIK(ref tentacleJoints, ref tentacleLimbs, target.Center);
					*/
                }
				else detimer++;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			/*
			if (NPC.target != -1) {
				DrawTentacles(spriteBatch, screenPos);
			}
			*/
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

		/*
        private void DrawTentacles(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            for (int i = 0; i < TentacleSegments - 1; i++)
            {
                Vector2 start = tentacleJoints[i].Position - screenPos;
                Vector2 end = tentacleJoints[i + 1].Position - screenPos;

                Vector2 segmentVector = end - start;

                // Replace "TextureAssets.MagicPixel.Value" with the actual texture you want to use for the tentacle segment
                Texture2D texture = TextureAssets.MagicPixel.Value;

                float scale = 1f; // Adjust the scale as needed

                float rotation = (float)Math.Atan2(segmentVector.Y, segmentVector.X); // Calculate rotation based on the segment's direction

                Vector2 origin = new Vector2(0f, 0.5f); // Set the origin to the left-center of the texture

                Rectangle destinationRect = new Rectangle((int)start.X, (int)start.Y, (int)segmentVector.Length(), (int)segmentVector.Length()); // Use a rectangle for the destination

                spriteBatch.Draw(texture, destinationRect, null, Color.Beige, rotation, origin, SpriteEffects.None, 0f);
            }
        }
		*/
    }
}
