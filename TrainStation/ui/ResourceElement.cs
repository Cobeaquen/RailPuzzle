using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation
{
    public class ResourceElement
    {
        public Vector2 position;
        public Text label;
        public Text text;
        public Texture2D texture;
        public Vector2 origin;
        public float scale;

        public ResourceElement(Vector2 position, string label, Texture2D texture, string startingValue, float scale, GameState drawState = GameState.None)
        {
            this.position = position;
            this.texture = texture;
            this.scale = scale;
            this.label = label != string.Empty ? new Text(position + new Vector2(0, -Cell.cellHeight), label, Cell.cellWidth, Cell.cellHeight, 1, GUI.defaultFont, Color.Black) : null;
            origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            text = new Text(position + new Vector2(0, Cell.cellHeight), startingValue, Cell.cellWidth, Cell.cellHeight, 1, GUI.defaultFont, Color.Black);

            if (drawState == GameState.None || drawState == GameState.Editing || drawState == GameState.Simulating)
            {
                GUI.resources.Add(this);
                Cell.GetCell(position).isEmpty = false;
            }
        }

        public void ChangeText(string text)
        {
            this.text.text = text;
        }

        public void Draw(SpriteBatch batch)
        {
            if (label != null)
                label.Draw(batch);
            text.Draw(batch);
            batch.Draw(texture, position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
