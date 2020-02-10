using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrainStation
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        RenderTarget2D mainTarget;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            graphics.PreferredBackBufferWidth = Globals.screenWidth;
            graphics.PreferredBackBufferHeight = Globals.screenHeight;
        }

        protected override void Initialize()
        {
            var pp = GraphicsDevice.PresentationParameters;
            Globals.Initialize(GraphicsDevice, pp);
            mainTarget = new RenderTarget2D(graphics.GraphicsDevice, Globals.gameWidth, Globals.gameHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.content = Content;
            Globals.LoadContent(GraphicsDevice);
            Textures.LoadTextures();
            Cell.CreateGrid(new Point(-2048, -2048), 400, 400);
            Rail.LoadContent();
            Level.LoadContent();
            Input.LoadInput();
            Globals.LoadLevel();
            Globals.level.SpawnMapRails();
            Globals.player = new Player();
            GUI.LoadGUI();
        }

        protected override void UnloadContent()
        {
            //Globals.ResetLevel();
            //Globals.level.Save("lv1"); // Save the level
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.BeginInput();

            if (Input.keyState.IsKeyDown(Keys.Escape))
                Exit();

            GUI.Update(gameTime);
            Globals.Update(gameTime);
            Power.UpdatePower(gameTime);

            //Cell.UpdateCellsOnScreen(Globals.screenCenter, Globals.gameWidth, Globals.gameHeight);

            Input.EndInput();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            #region Lighting
            GraphicsDevice.SetRenderTarget(Globals.lightsTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive); // drawing lights

            spriteBatch.Draw(Globals.lightMask, Input.mousePosition, null, Color.White, 0f, Globals.lightMaskOrigin, 2f, SpriteEffects.None, 0f);

            spriteBatch.End();
            #endregion

            GraphicsDevice.SetRenderTarget(mainTarget);

            if (Globals.player.gameState != GameState.LevelFinished)
            {
                #region Draw game to rendertarget

                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, samplerState: SamplerState.PointWrap, transformMatrix: Globals.player.camera.View); // drawing the map

                //Cell.DrawGrid(spriteBatch);

                Globals.level.Draw(spriteBatch);

                Item.DrawItems(spriteBatch);

                //Rail.DrawRails(spriteBatch);

                Globals.player.Draw(spriteBatch);

                if (Globals.displayPower)
                {
                    Power.DrawPower(spriteBatch);
                }

                spriteBatch.End();

                #endregion
            }

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region Drawing to backbuffer

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            //Globals.DeactivateLight();

            spriteBatch.Draw(mainTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Globals.gameScale, SpriteEffects.None, 0f);
            spriteBatch.End();
            
            base.Draw(gameTime);

            #endregion

            #region drawingUI

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (Globals.player.gameState != GameState.LevelFinished)
            {
                Globals.DeactivateButtonEffect();
                GUI.DrawGUI(spriteBatch);
            }
            else if (Globals.player.gameState == GameState.LevelFinished)
            {
                GraphicsDevice.Clear(Color.Black);
                GUI.DrawFinishLevelUI(spriteBatch);
            }

            spriteBatch.End();

            #endregion
        }
    }
}