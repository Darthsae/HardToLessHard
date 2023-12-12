using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Spells
{
    public class ModSpell
    {
        public static int[]? ParseContext(string context)
        {
            string[] splitContext = context.Split(':');

            string[] componentNames = splitContext[0].Split('|');

            if (componentNames.Length > 0 )
            {
                int[] spellComponents = new int[componentNames.Length];
                for (int i = 0; i < componentNames.Length; i++)
                {
                    spellComponents[i] = SpellComponentLoader.GetSpellComponent(componentNames[i]).Type;
                }

                return spellComponents;
            }
            else
            {
                return null;
            }
        }

        public int[] SpellComponents { get; set; }

        public Color Color = Color.White;

        public Dictionary<string, ModifierData> modifiers = new()
        {
            { "damage", new ModifierData() },
            { "knockBack", new ModifierData() },
            { "stopsDealingDamageAfterPenetrateHits", new ModifierData() },
            { "tileCollide", new ModifierData() },
            { "timeLeft", new ModifierData() },
            { "ignoreWater", new ModifierData() },
            { "light", new ModifierData() },
            { "maxPenetrate", new ModifierData() },
            { "scale", new ModifierData() },
            { "speed", new ModifierData() }
        };

        public List<int> buffs = new List<int>();

        public void SetModifiers()
        {
            modifiers["damage"].Clear();
            modifiers["knockBack"].Clear();
            modifiers["stopsDealingDamageAfterPenetrateHits"].Clear();
            modifiers["tileCollide"].Clear();
            modifiers["timeLeft"].Clear();
            modifiers["ignoreWater"].Clear();
            modifiers["light"].Clear();
            modifiers["maxPenetrate"].Clear();
            modifiers["scale"].Clear();
            modifiers["speed"].Clear();

            float r = 0;
            float g = 0;
            float b = 0;

            foreach (var spellComponentID in  SpellComponents)
            {
                ModSpellComponent spellComponent = SpellComponentLoader.GetSpellComponent(spellComponentID);
                modifiers["damage"] += spellComponent.modifiers["damage"];
                modifiers["knockBack"] += spellComponent.modifiers["knockBack"];
                modifiers["stopsDealingDamageAfterPenetrateHits"] += spellComponent.modifiers["stopsDealingDamageAfterPenetrateHits"];
                modifiers["tileCollide"] += spellComponent.modifiers["tileCollide"];
                modifiers["timeLeft"] += spellComponent.modifiers["timeLeft"];
                modifiers["ignoreWater"] += spellComponent.modifiers["ignoreWater"];
                modifiers["light"] += spellComponent.modifiers["light"];
                modifiers["maxPenetrate"] += spellComponent.modifiers["maxPenetrate"];
                modifiers["scale"] += spellComponent.modifiers["scale"];
                modifiers["speed"] += spellComponent.modifiers["speed"];

                r += spellComponent.Color.R;
                g += spellComponent.Color.G;
                b += spellComponent.Color.B;

                if (spellComponent.buffs != null)
                {
                    foreach (var buff in spellComponent.buffs)
                    {
                        if (!buffs.Contains(buff)) buffs.Add(buff);
                    }
                }
            }

            r /= SpellComponents.Length;
            g /= SpellComponents.Length;
            b /= SpellComponents.Length;

            Color = new(r, g, b);
        }
    }
}
