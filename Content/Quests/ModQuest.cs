using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace HardToLessHard.Content.Quests
{
    public abstract class ModQuest : ModType, ILocalizedModType
    {
        public string LocalizationCategory => "Quests";
        public int Type { get; internal set; }
        public List<string> completed;

        public virtual QuestCondition[] Conditions => new QuestCondition[0];

        public virtual QuestReward[] Rewards { get; set; }

        public virtual string Icon => (GetType().Namespace + "." + Name + "_Icon").Replace('.', '/');

        public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
        public virtual LocalizedText Description => this.GetLocalization(nameof(Description), () => "");

        protected sealed override void Register()
        {
            ModTypeLookup<ModQuest>.Register(this);
            Type = QuestLoader.Register(this);
        }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public void CheckQuest(string player)
        {
            if (completed.Contains(player)) return;

            Player toPlayer = null;

            foreach (Player playerIterated in Main.player) if (playerIterated != null && playerIterated.name == player) { toPlayer = playerIterated; break; }

            if (toPlayer == null) return;

            for (int i = 0; i < Conditions.Length; i++) if (!Conditions[i].CheckCompletion(toPlayer)) return;

            completed.Add(player);

            foreach (QuestReward reward in Rewards) 
            {
                reward.Reward(toPlayer);
            }

            Main.NewText($"Completed quest: {DisplayName.Value}");
            AdvancedPopupRequest request = new AdvancedPopupRequest();
            request.Text = $"Completed quest: {DisplayName.Value}";
            PopupText.NewText(request, toPlayer.position + new Microsoft.Xna.Framework.Vector2(0, -100));
        }
    }

    internal class QuestUIElement : UIImageButton
    {
        internal string Quest;
        internal UIImage icon;
        internal UIText name;
        private readonly float _scale;


        public QuestUIElement(Asset<Texture2D> background, string quest, float scale = 1f) : base(background)
        {
            Quest = quest;
            _scale = scale;
            icon = new UIImage(ModContent.Request<Texture2D>(QuestLoader.GetQuest(Quest).Icon));
            icon.Left.Set(10, 0f);
            icon.Top.Set(10, 0f);
            name = new UIText(QuestLoader.GetQuest(Quest).DisplayName);
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
