using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainStation.ui
{
    public class ItemCategory
    {
        public Vector2 position;

        Text text;
        int distanceBetweenTiles = 48;

        public string categoryName;
        public ItemElement[] itemElements;

        public ItemCategory(Vector2 position, string categoryName, PlacableItem[] items)
        {
            this.position = position;
            this.categoryName = categoryName;
            itemElements = new ItemElement[items.Length];
            for (int i = 0; i < itemElements.Length; i++)
            {
                itemElements[i] = new ItemElement(new Vector2(position.X + i * Textures.ItemsMenuTile.Width, position.Y), items[i], "some item");
            }

            text = new Text(position + new Vector2(0, -64), categoryName, Textures.ItemsMenu.Width * 2, Textures.ItemsMenu.Height * 2, 4f, GUI.defaultFont, Color.Black, true, GameState.None, 1f);
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position = position;
            text.position = position + new Vector2(0, -64);
        }

        public void Update(float xPos)
        {
            position.X = xPos;

            text.position.X = xPos;
            
            for (int i = 0; i < itemElements.Length; i++)
            {
                itemElements[i].position = new Vector2(position.X + i * distanceBetweenTiles * 2f - 96 * 2f, position.Y + 16);

                itemElements[i].Update();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var item in itemElements)
            {
                item.Draw(batch);
            }
            batch.Draw(Textures.ItemsMenu, position, null, Color.White, 0f, Textures.ItemsMenuOrigin, 2f, SpriteEffects.None, 0.1f);
        }
    }
}