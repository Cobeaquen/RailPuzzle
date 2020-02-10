using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TrainStation
{
    public static class Globals
    {
        public static int gameWidth = 1920, gameHeight = 1080; //960, 540
        public static int screenWidth = 1920, screenHeight = 1080;
        public static float gameScale = screenWidth / (float)gameWidth;
        public static Vector2 screenCenter;

        public static Player player;

        public static float time;

        public static Level level;
        public static Level originalLevel;

        public static ContentManager content;
        
        //public static Effect baseEffect;
        //public static Effect buttonEffect;
        public static Texture2D lightMask;
        public static Vector2 lightMaskOrigin;
        public static RenderTarget2D lightsTarget;
        public static GraphicsDevice graphics;

        public static Random rand;

        public static List<Cell> placedItems;

        public static bool displayPower;

        public static bool showFps = true;
        public static bool debugMessages = false;

        #region fps
        static double totalSeconds;
        static int frames;
        static float fps;
        #endregion

        public static void LoadContent(GraphicsDevice graphics)
        {
            DebugTextures.LoadTextures(graphics);

            GUI.defaultFont = content.Load<SpriteFont>("ui\\fonts\\default_font");

            screenCenter = new Vector2(gameWidth / 2f, gameHeight / 2f);
            rand = new Random();

            placedItems = new List<Cell>();

            //baseEffect = content.Load<Effect>("effects\\base_effect");
            //buttonEffect = content.Load<Effect>("effects\\button_effect");
            lightMask = content.Load<Texture2D>("effects\\lightmask");
            lightMaskOrigin = GetTextureOrigin(lightMask);
        }

        public static void Initialize(GraphicsDevice graphics, PresentationParameters pp)
        {
            Globals.graphics = graphics;
            lightsTarget = new RenderTarget2D(graphics, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public static void ActivateButtonEffect()
        {
            //buttonEffect.Parameters["t"].SetValue(time);
            //buttonEffect.Parameters["use"].SetValue(1);
            //buttonEffect.CurrentTechnique.Passes[0].Apply();
        }
        public static void DeactivateButtonEffect()
        {
            //buttonEffect.Parameters["t"].SetValue(0f);
            //buttonEffect.Parameters["use"].SetValue(0);
            //buttonEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void ActivateLight()
        {
            //baseEffect.Parameters["lightMask"].SetValue(lightsTarget);
            //baseEffect.Parameters["useLight"].SetValue(1);
            //baseEffect.CurrentTechnique.Passes[0].Apply();
        }
        public static void DeactivateLight()
        {
            //baseEffect.Parameters["useLight"].SetValue(0);
            //baseEffect.CurrentTechnique.Passes[0].Apply();
        }

        public static void Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Update(gameTime);

            if (showFps)
                CalculateFps(gameTime);
        }

        public static void CalculateFps(GameTime gameTime)
        {
            totalSeconds += gameTime.ElapsedGameTime.TotalSeconds;
            frames++;
            fps = (1f / (float)totalSeconds) * frames;

            if ((int)totalSeconds > 1)
            {
                frames = 0;
                totalSeconds = 0;
                Console.WriteLine(fps);
            }
        }

        public static void LoadLevel()
        {
            //level = Level.Load("level1"); // optimize please - the level is being read from the same file everytime it is reset, this causes it to perform unnessesary calculations
            level = Level.LoadExampleLevel();
        }
        public static void CreateLevel()
        {

        }
        public static void ResetLevel()
        {
            //level = Level.Load("level1");
            level = Level.LoadExampleLevel();
        }

        public static int IncrementEnum(Type enumType, int enumObject, int step)
        {
            int count = Enum.GetNames(enumType).Length;

            if (step > 0)
            {
                if (enumObject + step < count)
                    enumObject++;
                else
                    enumObject = 0;
            }
            else
            {
                if (enumObject + step >= 0)
                    enumObject--;
                else
                    enumObject = count - 1;
            }

            return enumObject;
        }

        public static Vector2 GetTextureOrigin(Texture2D texture)
        {
            return new Vector2(texture.Width / 2f, texture.Height / 2f);
        }
    }

    public enum GameState
    {
        None, Editing, Simulating, LevelFinished
    };
}