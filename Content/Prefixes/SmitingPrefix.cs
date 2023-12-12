using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Prefixes
{
    public class SmitingPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult *= 1.2f;
            knockbackMult *= 0.5f;
            manaMult *= 0.2f;
            critBonus *= 3;
        }

        public override bool CanRoll(Item item)
        {
            return true;
        }
    }
}
