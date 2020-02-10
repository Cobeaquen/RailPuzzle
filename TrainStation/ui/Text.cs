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
    public class Text
    {
        public static SpriteFont defaultFont;
        public static SpriteFont guiFont;

        public int fieldWidth, fieldHeight;
        public string text
        {
            get
            {
                return this.m_text;
            }
            set
            {
                m_text = value;
                m_text = ClampText();
            }
        }
        public float size;
        public float layer;

        public SpriteFont font;
        public Color color;

        public Vector2 position;

        public GameState drawState;

        private Vector2 origin;

        private string m_text;

        private bool isui;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="text"></param>
        /// <param name="fieldWidth"></param>
        /// <param name="fieldHeight"></param>
        /// <param name="font"></param>
        public Text(Vector2 position, string text, int fieldWidth, int fieldHeight, float size, SpriteFont font, Color color, bool isui = true, GameState drawState = GameState.None, float layer = 0f)
        {
            this.position = position;
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            this.color = color;
            this.m_text = text;
            this.font = font;
            this.size = size;
            this.m_text = ClampText();
            this.drawState = drawState;
            origin = font.MeasureString(this.m_text) / 2f;
            this.layer = layer;

            if (isui)
            {
                if (drawState == GameState.None || drawState == GameState.Editing || drawState == GameState.Simulating)
                {
                    GUI.textsGUI.Add(this);
                }
                else if (drawState == GameState.LevelFinished)
                {
                    GUI.textsFinish.Add(this);
                }
            }
        }
        public string ClampText()
        {
            string[] words = m_text.Split(' ');

            string result = "";

            foreach (var word in words)
            {
                if (font.MeasureString(result + " " + word).X > fieldWidth) // the text is too wide
                {
                    result += "\n";
                }
                if (font.MeasureString(result + word).Y > fieldHeight)
                {
                    break;
                }
                result += word + " ";
            }
            return result;
        }
        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(font, m_text, position, color, 0f, origin, size, SpriteEffects.None, layer);
        }

        public static Text[] StringsToText(string[] values, Vector2 position, int fieldWidth, int fieldHeight, float size, SpriteFont font)
        {
            Text[] texts = new Text[values.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = new Text(position, values[i], fieldWidth, fieldHeight, size, font, Color.White);
            }
            return texts;
        }
    }
}