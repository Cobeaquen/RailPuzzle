using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrainStation.ui;

namespace TrainStation
{
    public static class GUI
    {
        public static SpriteFont defaultFont;

        public static List<Button> buttonsGUI = new List<Button>();
        public static List<Button> buttonsFinish = new List<Button>();
        public static List<Text> textsGUI = new List<Text>();
        public static List<Text> textsFinish = new List<Text>();
        public static List<ResourceElement> resources = new List<ResourceElement>();
        public static List<ItemElement> ItemElements;

        public static Text instructions;

        public static Text timeText;

        public static Text finishGameText;

        public static ResourceElement straightRailElement;
        public static ResourceElement turnRailElement;
        public static ResourceElement boostRailElement;

        public static Button sendCartButton;
        public static Button destroyCartButton;
        public static Button togglePowerdisplayButton;

        public static ItemsMenu itemsMenu;

        public static void LoadGUI()
        {
            buttonsGUI = new List<Button>();
            buttonsFinish = new List<Button>();
            textsGUI = new List<Text>();
            textsFinish = new List<Text>();
            resources = new List<ResourceElement>();
            ItemElements = new List<ItemElement>();

            timeText = new Text(new Vector2(Globals.screenCenter.X, Cell.cellHeight), string.Empty, Cell.cellWidth, Cell.cellHeight, 1f, defaultFont, Color.Black);

            instructions = new Text(new Vector2(Cell.cellWidth * 3.5f, Cell.cellHeight), "Rotate: Q\nHold and drag to place multiple rails at once\nUse scroll-wheel to change rail-type", Cell.cellWidth * 8, Cell.cellHeight * 2, 1f, defaultFont, Color.Black);

            finishGameText = new Text(Globals.screenCenter, "THANKS FOR PLAYING!\n<MADE BY COBEAQUEN>", Cell.cellWidth * 8, Cell.cellHeight * 2, 1f, defaultFont, Color.White, true, GameState.LevelFinished);

            straightRailElement = new ResourceElement(Cell.GetCell(Globals.screenWidth, Globals.screenHeight).ToVector2() - new Vector2(Cell.cellWidth * 2, Cell.cellHeight * 2), string.Empty, Textures.RailStraight, Globals.level.straightRailCount.ToString(), 1f);
            turnRailElement = new ResourceElement(Cell.GetCell(Globals.screenWidth, Globals.screenHeight).ToVector2() - new Vector2(Cell.cellWidth * 4, Cell.cellHeight * 2), string.Empty, Textures.RailTurn, Globals.level.turnRailCount.ToString(), 1f);
            boostRailElement = new ResourceElement(Cell.GetCell(Globals.screenWidth, Globals.screenHeight).ToVector2() - new Vector2(Cell.cellWidth * 6, Cell.cellHeight * 2), string.Empty, Textures.RailBooster, Globals.level.boostRailCount.ToString(), 1f);

            sendCartButton = new Button(new Vector2(Cell.cellWidth * 1, Cell.cellHeight * 2), "run", "RUN", false, DebugTextures.GenerateRectangle(64, 64, Color.DarkBlue), defaultFont, Globals.player.RunSimulation);
            destroyCartButton = new Button(new Vector2(Cell.cellWidth * 3, Cell.cellHeight * 2), "reset", "RESET", false, DebugTextures.GenerateRectangle(64, 64, Color.DarkRed), defaultFont, Globals.player.Reset);
            togglePowerdisplayButton = new Button(new Vector2(Cell.cellWidth * 19, Cell.cellHeight * 1), "power_toggle", "Power: hidden", false, DebugTextures.GenerateRectangle(64, 64, Color.IndianRed), defaultFont, Globals.player.TogglePowerDisplay);

            itemsMenu = new ItemsMenu();
        }
        public static void Update(GameTime gameTime)
        {
            foreach (var b in buttonsGUI)
            {
                b.Update(gameTime);
            }
            if (timeText != null)
            {
                timeText.text = Globals.time.ToString("0.0");
            }

            itemsMenu.Update(gameTime);
        }

        public static void DrawGUI(SpriteBatch batch)
        {
            foreach (var b in buttonsGUI)
            {
                if (b.mouseOver)
                    Globals.ActivateButtonEffect();
                else
                    Globals.DeactivateButtonEffect();
                b.Draw(batch);
            }
            Globals.DeactivateButtonEffect();
            foreach (var t in textsGUI)
            {
                t.Draw(batch);
            }
            foreach (var r in resources)
            {
                r.Draw(batch);
            }
            foreach (var i in ItemElements)
            {
                i.Draw(batch);
            }

            itemsMenu.Draw(batch);
        }

        public static void DrawFinishLevelUI(SpriteBatch batch)
        {
            foreach (var t in textsFinish)
            {
                t.Draw(batch);
            }
        }
    }
}