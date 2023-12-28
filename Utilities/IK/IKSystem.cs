using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace HardToLessHard.Utilities.IK
{
    public class IKSystem
    {
        // Define the joints
        public class Joint
        {
            public Vector2 Position { get; set; }
            public float Angle { get; set; }
        }

        // Define the limb
        public class Limb
        {
            public float Length { get; set; }
            public float Angle { get; set; }
        }

        // Solve the IK for a given target position
        public static void SolveIK(ref Joint[] joints, ref Limb[] limbs, Vector2 target)
        {
            const float epsilon = 0.001f;
            const int maxIterations = 100;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // Calculate the distance between the end effector and the target
                Vector2 endEffector = CalculateEndEffector(joints, limbs);
                float distance = Vector2.Distance(endEffector, target);

                // If the distance is within an acceptable range, break out of the loop
                if (distance < epsilon)
                {
                    break;
                }

                //Logging.PublicLogger.Debug(iteration);

                // Perform the Backward Reaching process
                for (int i = joints.Length - 1; i >= 0; i--)
                {
                    Vector2 toTarget = target - joints[i].Position;
                    float currentDistance = toTarget.Length();

                    Vector2 toEndEffector = endEffector - joints[i].Position;
                    float desiredDistance = toEndEffector.Length();

                    float scale = desiredDistance / currentDistance;

                    joints[i].Position = target - scale * toTarget;

                    // Rotate the limb to match the new position
                    if (i > 0)
                    {
                        Vector2 toParent = joints[i - 1].Position - joints[i].Position;
                        joints[i].Angle = (float)Math.Atan2(toParent.Y, toParent.X);
                    }
                }
            }
        }

        // Calculate the end effector position based on joint positions and limb lengths
        private static Vector2 CalculateEndEffector(Joint[] joints, Limb[] limbs)
        {
            Vector2 endEffector = joints[0].Position;

            for (int i = 0; i < joints.Length - 1; i++)
            {
                float angle = joints[i].Angle + limbs[i].Angle;
                endEffector += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * limbs[i].Length;
            }

            return endEffector;
        }
    }
}
