using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation.ui
{
    public class ItemElement
    {
        public Vector2 position;

        public PlacableItem item;

        public Texture2D texture;

        public ResourceElement element;

        public ItemElement(Vector2 position, PlacableItem item, string description)
        {
            this.item = item;
            this.position = position;

            switch (item)
            {
                case PlacableItem.StraightRail:
                    texture = Textures.RailStraight;
                    break;
                case PlacableItem.TurnRail:
                    texture = Textures.RailTurn;
                    break;
                case PlacableItem.BoosterRail:
                    texture = Textures.RailBooster;
                    break;
                case PlacableItem.DetectorRail:
                    texture = Textures.RailDetector;
                    break;
                case PlacableItem.PushRail:
                    texture = Textures.RailPush;
                    break;
                case PlacableItem.Lever:
                    texture = Textures.LeverDeactive;
                    break;
                case PlacableItem.InverterModule:
                    texture = Textures.ModuleInverter;
                    break;
                default:
                    break;
            }

            GUI.ItemElements.Add(this);
        }

        public void Update()
        {
            if (Input.mouseClicked && Input.MouseOverUI(position, texture.Width, texture.Height))
            {
                Globals.player.selectedItem = item;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 1f);
        }
    }
}
