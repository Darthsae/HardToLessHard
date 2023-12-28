using Terraria.ModLoader;
using Terraria;
using HardToLessHard.Utilities.IK;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;

namespace HardToLessHard.Content.Projectiles
{
    public class ReaperSpear : ExtendedModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Faction = "Sanguine";
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver4 + MathHelper.Pi;
        }
    }
}
