using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainStation;

namespace LevelEditor
{
    public static class GUI
    {
        public static Button straightRailButton;
        public static Button turnRailButton;
        public static Button boostRailButton;
        public static Button obstacleButton;
        public static Button bumpButton;
        public static Button portalButton;

        public static SpriteFont defaultFont;

        public static void LoadGUI(ContentManager content)
        {
            defaultFont = content.Load<SpriteFont>("ui\\fonts\\default_font");

            straightRailButton = new Button(new Vector2(Cell.cellWidth * 19, 10 * Cell.cellHeight), "straight", string.Empty, true, Rail.straightTexture, defaultFont, Main.OnButtonClick);
            turnRailButton = new Button(new Vector2(Cell.cellWidth * 17, 10 * Cell.cellHeight), "turn", string.Empty, true, Rail.turnTexture, defaultFont, Main.OnButtonClick);
            boostRailButton = new Button(new Vector2(Cell.cellWidth * 15, 10 * Cell.cellHeight), "boost", string.Empty, true, Rail.boosterTexture, defaultFont, Main.OnButtonClick);
            obstacleButton = new Button(new Vector2(Cell.cellWidth * 13, 10 * Cell.cellHeight), "obstacle", string.Empty, true, Obstacle.obstacleTexture, defaultFont, Main.OnButtonClick);
            bumpButton = new Button(new Vector2(Cell.cellWidth * 11, 10 * Cell.cellHeight), "bump", string.Empty, true, Bump.bumpTexture, defaultFont, Main.OnButtonClick);
            portalButton = new Button(new Vector2(Cell.cellWidth * 9, 10 * Cell.cellHeight), "portal", string.Empty, true, Portal.portalTexture, defaultFont, Main.OnButtonClick);
        }
    }
}