using HardToLessHard.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using HardToLessHard.Content.TileEntities;
using HardToLessHard.Content.Religions;
using Terraria.ModLoader.UI;
using HardToLessHard.Content.Factions;

namespace HardToLessHard.Common.UI.DeificAltarUI
{
    // This custom UI will show whenever the player is holding the ExampleCustomResourceWeapon item and will display the player's custom resource amounts that are tracked in ExampleResourcePlayer
    internal class DeificAltarUIState : UIState
    {
        // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
        // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
        private UIElement area;
        private UIPanel background;
        private UIScrollbar scrollbar;
        private UIPanel deityArea;
        private DeityUIElement[] deities;
        private UIPanel religionArea;
        private ReligionUIElement[] religions;
        private UIPanel factionArea;
        private FactionUIElement[] factions;
        private UIText name;
        private UITextPanel<string> description;
        private UIPanel deityInfoArea;
        private UIButton<string> religionButton;
        private UIButton<string> deityButton;
        private UIButton<string> factionButton;

        public override void OnInitialize()
        {
            area = new UIElement();
            area.Left.Set(-area.Width.Pixels - 850, 1f); // Place the resource bar to the left of the hearts.
            area.Top.Set(50, 0f); // Placing it just a bit below the top of the screen.
            area.Width.Set(550, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
            area.Height.Set(600, 0f);

            background = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            background.Left.Set(0, 0f);
            background.Top.Set(0, 0f);
            background.Width.Set(500, 0f);
            background.Height.Set(500, 0f);

            deityInfoArea = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            deityInfoArea.Left.Set(25, 0f);
            deityInfoArea.Top.Set(25, 0f);
            deityInfoArea.Width.Set(200, 0f);
            deityInfoArea.Height.Set(450, 0f);

            name = new UIText("Name");
            name.Left.Set(25, 0f);
            name.Top.Set(25, 0f);
            name.Width.Set(150, 0f);
            name.Height.Set(50, 0f);

            description = new UITextPanel<string>("Description");
            description.Left.Set(25, 0f);
            description.Top.Set(100, 0f);
            description.Width.Set(150, 0f);
            description.Height.Set(325, 0f);

            deityInfoArea.Append(name);
            deityInfoArea.Append(description);

            scrollbar = new UIScrollbar();
            scrollbar.Left.Set(250, 0f);
            scrollbar.Top.Set(25, 0f);
            scrollbar.Width.Set(25, 0f);
            scrollbar.Height.Set(450, 0f);
            scrollbar.OnUpdate += (evt) => {
                for (int i = 0; i < deities.Length; i++)
                {
                    deities[i].Top.Set(4 + (56 * i) - (scrollbar.ViewPosition * 16), 0f);
                }

                for (int i = 0; i < religions.Length; i++)
                {
                    religions[i].Top.Set(4 + (56 * i) - (scrollbar.ViewPosition * 16), 0f);
                }

                for (int i = 0; i < factions.Length; i++)
                {
                    factions[i].Top.Set(4 + (56 * i) - (scrollbar.ViewPosition * 16), 0f);
                }
            };

            deityArea = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            deityArea.Left.Set(275, 0f);
            deityArea.Top.Set(25, 0f);
            deityArea.Width.Set(200, 0f);
            deityArea.Height.Set(450, 0f);
            deityArea.OverflowHidden = true;

            deities = new DeityUIElement[DeityLoader.Count];
            for (int i = 0; i < deities.Length; i++)
            {
                deities[i] = new DeityUIElement(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"), DeityLoader.deityNames[i]);
                deities[i].Left.Set(25, 0f);
                deities[i].Width.Set(150, 0f);
                deities[i].Height.Set(52, 0f);
                deities[i].SetHoverImage(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"));
                deities[i].OnLeftClick += (evt, e) =>
                {
                    name.SetText(DeityLoader.GetDeity(((DeityUIElement)e).Deity).DisplayName.Value);
                    description.SetText(DeityLoader.GetDeity(((DeityUIElement)e).Deity).Description.Value);
                };
                deityArea.Append(deities[i]);
            }

            religionArea = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            religionArea.Left.Set(275, 0f);
            religionArea.Top.Set(25, 0f);
            religionArea.Width.Set(200, 0f);
            religionArea.Height.Set(450, 0f);
            religionArea.OverflowHidden = true;

            religions = new ReligionUIElement[ReligionLoader.Count];
            for (int i = 0; i < religions.Length; i++)
            {
                religions[i] = new ReligionUIElement(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"), ReligionLoader.religionNames[i]);
                religions[i].Left.Set(25, 0f);
                religions[i].Width.Set(150, 0f);
                religions[i].Height.Set(52, 0f);
                religions[i].SetHoverImage(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"));
                religions[i].OnLeftClick += (evt, e) =>
                {
                    name.SetText(ReligionLoader.GetReligion(((ReligionUIElement)e).Religion).DisplayName.Value);
                    description.SetText(ReligionLoader.GetReligion(((ReligionUIElement)e).Religion).Description.Value);
                };
                religionArea.Append(religions[i]);
            }

            religionArea.Top.Set(12000, 0f);

            factionArea = new()
            {
                BackgroundColor = Color.Blue,
                BorderColor = Color.DarkBlue
            };
            factionArea.Left.Set(275, 0f);
            factionArea.Top.Set(25, 0f);
            factionArea.Width.Set(200, 0f);
            factionArea.Height.Set(450, 0f);
            factionArea.OverflowHidden = true;

            factions = new FactionUIElement[FactionLoader.Count];
            for (int i = 0; i < factions.Length; i++)
            {
                factions[i] = new FactionUIElement(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"), FactionLoader.factionNames[i]);
                factions[i].Left.Set(25, 0f);
                factions[i].Width.Set(150, 0f);
                factions[i].Height.Set(52, 0f);
                factions[i].SetHoverImage(ModContent.Request<Texture2D>("HardToLessHard/Common/UI/DeificAltarUI/DeityButton"));
                factions[i].OnLeftClick += (evt, e) =>
                {
                    name.SetText(FactionLoader.GetFaction(((FactionUIElement)e).Faction).DisplayName.Value);
                    description.SetText(FactionLoader.GetFaction(((FactionUIElement)e).Faction).Description.Value);
                };
                factionArea.Append(factions[i]);
            }

            factionArea.Top.Set(12000, 0f);

            deityButton = new UIButton<string>("Deities");
            deityButton.Left.Set(25, 0f);
            deityButton.Top.Set(525, 0f);
            deityButton.Width.Set(100, 0f);
            deityButton.Height.Set(50, 0f);
            deityButton.OnLeftClick += (evt, e) =>
            {
                religionArea.Top.Set(12000, 0f);
                factionArea.Top.Set(12000, 0f);
                deityArea.Top.Set(25, 0f);
            };

            religionButton = new UIButton<string>("Religions");
            religionButton.Left.Set(150, 0f);
            religionButton.Top.Set(525, 0f);
            religionButton.Width.Set(100, 0f);
            religionButton.Height.Set(50, 0f);
            religionButton.OnLeftClick += (evt, e) =>
            {
                deityArea.Top.Set(12000, 0f);
                factionArea.Top.Set(12000, 0f);
                religionArea.Top.Set(25, 0f);
            };

            factionButton = new UIButton<string>("Factions");
            factionButton.Left.Set(275, 0f);
            factionButton.Top.Set(525, 0f);
            factionButton.Width.Set(100, 0f);
            factionButton.Height.Set(50, 0f);
            factionButton.OnLeftClick += (evt, e) =>
            {
                deityArea.Top.Set(12000, 0f);
                religionArea.Top.Set(12000, 0f);
                factionArea.Top.Set(25, 0f);
            };

            area.Append(background);
            area.Append(deityArea);
            area.Append(religionArea);
            area.Append(factionArea);
            area.Append(deityInfoArea);
            area.Append(scrollbar);
            area.Append(deityButton);
            area.Append(religionButton);
            area.Append(factionButton);
            Append(area);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.GetModPlayer<HTLHPlayer>().deificAltarUI || Main.LocalPlayer.GetModPlayer<HTLHPlayer>().deificAltarTileEntity == null) return;

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.deificAltarUI && modPlayer.deificAltarTileEntity == null))
                return;

            ref DeificAltarTileEntity deificAltarTileEntity = ref modPlayer.deificAltarTileEntity;
            base.Update(gameTime);
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if (Main.LocalPlayer.TryGetModPlayer(out HTLHPlayer modPlayer) && (!modPlayer.deificAltarUI && modPlayer.deificAltarTileEntity == null))
                return;

            if (name.IsMouseHovering)
            {
                modPlayer.deificAltarUI = false;
                modPlayer.deificAltarTileEntity = null;
            }
        }
    }

    // This class will only be autoloaded/registered if we're not loading on a server
    [Autoload(Side = ModSide.Client)]
    internal class DeificAltarUISystem : ModSystem
    {
        private UserInterface DeificAltarUserInterface;

        internal DeificAltarUIState DeificAltarUIState;

        public override void Load()
        {
            DeificAltarUIState = new();
            DeificAltarUserInterface = new();
            DeificAltarUserInterface.SetState(DeificAltarUIState);
        }

        public override void SetStaticDefaults()
        {
            DeificAltarUIState = new();
            DeificAltarUserInterface = new();
            DeificAltarUserInterface.SetState(DeificAltarUIState);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DeificAltarUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "HardToLessHard: Deific Altar",
                    delegate {
                        DeificAltarUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
