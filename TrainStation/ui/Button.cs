using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TrainStation
{
    public class Button
    {
        static Vector2 latestButtonPosition;

        public Vector2 position;
        public Vector2 origin;

        public Texture2D image;
        public Text text;
        public Color color;
        public string identifier;

        public int timesPressed;
        public float timePressed;

        public bool mouseOver;

        public bool autoRelease;

        public delegate void onPressed(Button btn);
        onPressed pressed;

        MouseState previousState;

        public Button(Vector2 position, string identifier, string text, bool autoRelease, Texture2D image, SpriteFont font, onPressed onPressed, GameState drawState = GameState.None)
        {
            this.position = position;
            this.image = image;
            this.autoRelease = autoRelease;
            this.text = new Text(position, text, image.Width, image.Height, 1f, font, Color.White, true, GameState.None, 0.1f);
            this.identifier = identifier;
            origin = new Vector2(image.Width / 2f, image.Height / 2f);
            pressed = onPressed;

            previousState = Mouse.GetState();
            color = Color.White;

            latestButtonPosition = position;

            if (drawState == GameState.None || drawState == GameState.Editing || drawState == GameState.Simulating)
            {
                GUI.buttonsGUI.Add(this);
            }
            else if (drawState == GameState.LevelFinished)
            {
                GUI.buttonsFinish.Add(this);
            }

            Cell.GetCell(position).isEmpty = false;
        }

        public static Button Debug(string text, int width, int height, Color color, onPressed onPressed)
        {
            return new Button(latestButtonPosition + new Vector2(width * 1.5f, 0), "debug", text, false, DebugTextures.GenerateRectangle(width, height, color), GUI.defaultFont, onPressed);
        }

        public void Update(GameTime gameTime)
        {
            mouseOver = MouseOver();
            if (mouseOver)
            {
                color.R = 250;
                color.G = 250;
                color.B = 250;
                text.color = color;
                if (Input.mouseState.LeftButton == ButtonState.Released && Input.prevMouseState.LeftButton == ButtonState.Pressed && autoRelease)
                {
                    pressed(this);
                    timesPressed++;
                }
                else if (Input.mouseState.LeftButton == ButtonState.Pressed && Input.prevMouseState.LeftButton == ButtonState.Released && !autoRelease)
                {
                    pressed(this);
                    timePressed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                color = Color.White;
                text.color = color;
            }
        }

        public bool MouseOver()
        {
            return Input.MouseOverUI(position, image.Width, image.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
            text.Draw(batch);
        }
    }
}
