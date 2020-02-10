using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainStation
{
    static class Textures
    {
        public static Texture2D Cart { get; set; }
        public static Texture2D LeverActive { get; set; }
        public static Texture2D LeverDeactive { get; set; }
        public static Texture2D RailTurn { get; set; }
        public static Texture2D RailStraight { get; set; }
        public static Texture2D RailBooster { get; set; }
        public static Texture2D RailDetector { get; set; }
        public static Texture2D RailPush { get; set; }
        public static Texture2D RailCounter { get; set; }
        public static Texture2D ModuleInverter { get; set; }
        public static Texture2D ModuleSignalModifier { get; set; }
        public static Texture2D Start { get; set; }
        public static Texture2D Destination { get; set; }
        public static Texture2D Bump { get; set; }
        public static Texture2D Portal { get; set; }
        public static Texture2D Obstacle { get; set; }
        public static Texture2D Orb { get; set; }
        public static Texture2D CartSpawner { get; set; }

        #region UI

        public static Texture2D ItemsMenu { get; set; }
        public static Vector2 ItemsMenuOrigin { get; set; }
        public static Texture2D ItemsMenuOpen { get; set; }
        public static Vector2 ItemsMenuOpenOrigin { get; set; }
        public static Texture2D ItemsMenuTile { get; set; }
        public static Vector2 ItemsMenuTileOrigin { get; set; }
        public static Texture2D ItemsMenuExpand { get; set; }
        public static Vector2 ItemsMenuExpandOrigin { get; set; }

        #endregion

        public static void LoadTextures()
        {
            Cart = Load("cart");
            LeverActive = Load("lever_active");
            LeverDeactive = Load("lever_deactive");
            RailStraight = Load("track_straight");
            RailTurn = Load("track_turn_02");
            RailBooster = Load("track_boost");
            RailDetector = Load("track_detector");
            RailPush = Load("track_push");
            RailCounter = Load("track_counter");
            ModuleInverter = Load("power_module_inverter");
            Start = Load("start");
            Destination = Load("flag_finish");
            Bump = DebugTextures.GenerateRectangle(64, 64, Color.Gray);
            Portal = DebugTextures.GenerateRectangle(64, 64, Color.DarkMagenta);
            Obstacle = Load("obstacle_01");
            ModuleSignalModifier = Load("power_module_signalmod");
            Orb = Load("orb_01");
            CartSpawner = Load("spawner_cart");
            ItemsMenu = Load("ui\\ui_itemsmenu");
            ItemsMenuOrigin = Globals.GetTextureOrigin(ItemsMenu);
            ItemsMenuOpen = Load("ui\\ui_itemsmenu_open");
            ItemsMenuOpenOrigin = Globals.GetTextureOrigin(ItemsMenuOpen);
            ItemsMenuTile = Load("ui\\ui_itemsmenu_tile");
            ItemsMenuTileOrigin = Globals.GetTextureOrigin(ItemsMenuTile);
            ItemsMenuExpand = Load("ui\\ui_itemsmenu_expand");
            ItemsMenuExpandOrigin = Globals.GetTextureOrigin(ItemsMenuExpand);
        }

        private static Texture2D Load(string path)
        {
            return Globals.content.Load<Texture2D>("textures\\" + path);
        }
    }
}