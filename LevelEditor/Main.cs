using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TrainStation;

namespace LevelEditor
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Editor editor;
        
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Input.LoadInput();
            Globals.content = new ContentManager(Services, "Content");
            Globals.LoadContent(GraphicsDevice);
            Level.LoadContent();
            Cell.CreateGrid(new Point(-2048, -2048), 400, 400);
            Rail.LoadContent();
            Globals.level = new Level(Cell.GetCell(new Vector2(100, Globals.gameHeight - Cell.cellHeight)), Cell.GetCell(new Vector2(Globals.gameWidth - Cell.cellWidth, 200)), 1, 0, -1, 0, 50, 10, 10, new List<Rail>(), new List<Obstacle>(), new List<Bump>(), new List<Portal>(), new List<Cell>());
            Globals.level.SpawnMapRails();
            GUI.LoadGUI(Content);

            editor = new Editor();
        }

        protected override void UnloadContent()
        {
            Globals.level.Save("level1");
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.BeginInput();

            if (Input.keyState.IsKeyDown(Keys.Escape))
                Exit();

            editor.Update(gameTime);
            TrainStation.GUI.Update(gameTime);

            base.Update(gameTime);
            Input.EndInput();
        }

        public static void OnButtonClick(Button btn)
        {
            switch (btn.identifier)
            {
                case "straight":
                    editor.selectedRail = Rail.RailType.Straight;
                    editor.placeObject = Editor.PlaceType.Rail;
                    break;
                case "turn":
                    editor.selectedRail = Rail.RailType.Turn;
                    editor.placeObject = Editor.PlaceType.Rail;
                    break;
                case "boost":
                    editor.selectedRail = Rail.RailType.Boost;
                    editor.placeObject = Editor.PlaceType.Rail;
                    break;
                case "obstacle":
                    editor.placeObject = Editor.PlaceType.Obstacle;
                    break;
                case "bump":
                    editor.placeObject = Editor.PlaceType.Bump;
                    break;
                case "portal":
                    editor.placeObject = Editor.PlaceType.Portal;
                    break;
                case "orb":
                    editor.placeObject = Editor.PlaceType.Orb;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Pressed button");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.LinearWrap);

            editor.Draw(spriteBatch);
            TrainStation.GUI.DrawGUI(spriteBatch);

            Globals.level.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
