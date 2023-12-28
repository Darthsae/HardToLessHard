using Terraria.ModLoader;
using Terraria;
using HardToLessHard.Utilities.IK;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;

namespace HardToLessHard.Content.Projectiles
{
    public class IKProjectile : ModProjectile
    {
        private const int NumSegments = 5;
        private const float SegmentLength = 20f;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            // Get the player's cursor position
            Vector2 target = Main.MouseWorld;

            // Set up joints and limbs
            IKSystem.Joint[] joints = new IKSystem.Joint[NumSegments];
            IKSystem.Limb[] limbs = new IKSystem.Limb[NumSegments - 1];

            // Initialize joints
            for (int i = 0; i < NumSegments; i++)
            {
                joints[i] = new IKSystem.Joint
                {
                    Position = Projectile.Center,
                    Angle = 0f
                };

                if (i < NumSegments - 1)
                {
                    limbs[i] = new IKSystem.Limb
                    {
                        Length = SegmentLength,
                        Angle = 0f
                    };
                }
            }

            // Solve IK to update joint positions
            IKSystem.SolveIK(ref joints, ref limbs, target);

            // Set projectile position to the end of the IK chain
            Projectile.position = joints[NumSegments - 1].Position - new Vector2(Projectile.width / 2, Projectile.height / 2);

            // Update projectile rotation
            Projectile.rotation = joints[NumSegments - 2].Angle; // Adjust if needed

            // Optional: Add projectile code for other behaviors

            // Destroy the projectile after a certain time
            if (Projectile.timeLeft <= 0)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.spriteBatch.IsDisposed) Main.spriteBatch.Begin();

            DrawIKChain(Main.spriteBatch, lightColor);
            return base.PreDraw(ref lightColor);
        }

        private void DrawIKChain(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 start = Projectile.Center;

            for (int i = 0; i < NumSegments - 1; i++)
            {
                Vector2 end = Projectile.Center + new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * SegmentLength;

                // Draw a line segment
                DrawLine(spriteBatch, start, end, lightColor);

                start = end;
            }
        }

        private static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            
            Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, new Vector2((int)start.X, (int)start.Y), null, color, angle, Vector2.Zero, 1f, SpriteEffects.None);
        }
    }
}
