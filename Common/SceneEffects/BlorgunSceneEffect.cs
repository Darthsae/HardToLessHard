using HardToLessHard.Content.Religions;
using Terraria;
using Terraria.ModLoader;

namespace HardToLessHard.Common.SceneEffects
{
    public class BlorgunSceneEffect : ModSceneEffect
    {
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => base.SurfaceBackgroundStyle;

        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => base.UndergroundBackgroundStyle;

        public sealed override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public sealed override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/Rhapsody of Crimson Divinity");

        public override void SpecialVisuals(Player player, bool isActive)
        {
            base.SpecialVisuals(player, isActive);
        }

        public sealed override bool IsSceneEffectActive(Player player)
        {
            return DeityLoader.GetDeity("Blorgun").enraged;
        }
    }
}
