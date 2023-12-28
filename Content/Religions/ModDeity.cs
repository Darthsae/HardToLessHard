using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace HardToLessHard.Content.Religions
{
    ///<summary>
    /// Base class for deity-related content in the Terraria modding framework.
    ///</summary>
    public abstract class ModDeity : ModType, ILocalizedModType
    {
        public virtual DeityNPC[] Npcs => new DeityNPC[64];

        public virtual DeityProjectile[] Projectiles => new DeityProjectile[64];

        ///<summary>
        /// Gets or sets the file path for the deity's texture.
        ///</summary>
        public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

        ///<summary>
        /// Gets or sets the file path for the deity's icon.
        ///</summary>
        public virtual string Icon => (GetType().Namespace + "." + Name + "_Icon").Replace('.', '/');

        ///<summary>
        /// Gets or sets the file path for the deity's screen.
        ///</summary>
        public virtual string Screen => (GetType().Namespace + "." + Name + "_Screen").Replace('.', '/');

        ///<summary>
        /// Gets the localization category for the deity.
        ///</summary>
        public string LocalizationCategory => "Deities";

        ///<summary>
        /// Gets or sets the deity type.
        ///</summary>
        public int Type { get; internal set; }

        ///<summary>
        /// Gets or sets the maximum life of the deity.
        ///</summary>
        public long maxLife = 1;

        ///<summary>
        /// Gets or sets the current life of the deity.
        ///</summary>
        public long life = 1;

        ///<summary>
        /// Gets or sets the target of the deity.
        ///</summary>
        public int target = -1;

        ///<summary>
        /// Gets or sets a value indicating whether the deity is enraged.
        ///</summary>
        public bool enraged = false;

        ///<summary>
        /// Gets or sets the religion associated with the deity.
        ///</summary>
        public string Religion = "NoReligion";

        public string Faction = "NoFaction";

        ///<summary>
        /// Gets the color associated with the deity.
        ///</summary>
        public virtual Color Color => Color.White;

        ///<summary>
        /// Gets the localized display name of the deity.
        ///</summary>
        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);

        ///<summary>
        /// Gets the localized description of the deity.
        ///</summary>
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        ///<summary>
        /// Registers the deity type.
        ///</summary>
        protected sealed override void Register()
        {
            ModTypeLookup<ModDeity>.Register(this);
            Type = DeityLoader.Register(this);
        }

        ///<summary>
        /// Sets up static defaults during content setup.
        ///</summary>
        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        ///<summary>
        /// Combines pre-update, update, post-update, pre-AI, AI, and post-AI logic.
        ///</summary>
        public void InternalUpdate()
        {
            if (PreUpdate())
            {
                Update();
                PostUpdate();
            }

            if (PreAI())
            {
                AI();
                PostAI();
            }
        }

        ///<summary>
        /// Allows customization of pre-update logic.
        ///</summary>
        ///<returns>True if the pre-update logic should proceed; otherwise, false.</returns>
        public virtual bool PreUpdate()
        {
            return true;
        }

        ///<summary>
        /// Allows customization of pre-AI logic.
        ///</summary>
        ///<returns>True if the pre-AI logic should proceed; otherwise, false.</returns>
        public virtual bool PreAI()
        {
            return (enraged && target != -1);
        }

        ///<summary>
        /// Updates the deity.
        ///</summary>
        public virtual void Update() { }

        ///<summary>
        /// Post-update logic for the deity.
        ///</summary>
        public virtual void PostUpdate() { }

        ///<summary>
        /// AI logic for the deity.
        ///</summary>
        public virtual void AI() { }

        ///<summary>
        /// Post-AI logic for the deity.
        ///</summary>
        public virtual void PostAI() { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void InterfaceDraw(SpriteBatch spriteBatch) 
        { 
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Screen), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        }
    }

    public abstract class DeityNPC : ModNPC
    {
    }

    public abstract class DeityProjectile : ModProjectile
    {
    }

    internal class DeityUIElement : UIImageButton
    {
        internal string Deity;
        internal UIImage icon;
        internal UIText name;
        private readonly float _scale;


        public DeityUIElement(Asset<Texture2D> background, string deity, float scale = 1f) : base(background)
        {
            Deity = deity;
            _scale = scale;
            icon = new UIImage(ModContent.Request<Texture2D>(DeityLoader.GetDeity(Deity).Icon));
            icon.Left.Set(10, 0f);
            icon.Top.Set(10, 0f);
            name = new UIText(DeityLoader.GetDeity(Deity).DisplayName);
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
