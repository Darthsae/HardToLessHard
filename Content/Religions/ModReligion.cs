using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HardToLessHard.Content.Religions
{
    public abstract class ModReligion : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Religions";
        public int Type { get; internal set; }

        public virtual Color Color => Color.White;

        public virtual string Icon => (GetType().Namespace + "." + Name + "_Icon").Replace('.', '/');

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        protected sealed override void Register()
        {
            ModTypeLookup<ModReligion>.Register(this);
            Type = ReligionLoader.Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

    }

    internal class ReligionUIElement : UIImageButton
    {
        internal string Religion;
        internal UIImage icon;
        internal UIText name;
        private readonly float _scale;


        public ReligionUIElement(Asset<Texture2D> background, string religion, float scale = 1f) : base(background)
        {
            Religion = religion;
            _scale = scale;
            icon = new UIImage(ModContent.Request<Texture2D>(ReligionLoader.GetReligion(Religion).Icon));
            icon.Left.Set(10, 0f);
            icon.Top.Set(10, 0f);
            name = new UIText(ReligionLoader.GetReligion(Religion).DisplayName);
            name.Width.Set(88, 0f);
            name.Height.Set(32, 0f);
            name.Left.Set(52, 0f);
            name.Top.Set(10, 0f);
            Append(icon);
            Append(name);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
