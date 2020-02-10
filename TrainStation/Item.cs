using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    public class Item
    {
        public static List<Item> placedItems = new List<Item>();

        public PowerConnection power;

        public Cell cell;

        [ProtoMember(1)]
        public bool removable;

        public Texture2D texture;

        public Item(Cell cell, bool removable = true)
        {
            this.cell = cell;

            this.removable = removable;

            if (cell.isEmpty)
            {
                cell.item = this;
            }
        }
        public Item() { }

        public static bool PlaceItem(Item item, bool modifyLevel = false)
        {

            if (!item.cell.isEmpty)
                return false;

            if (modifyLevel)
            {
                Globals.level.rails.Add((Rail)item);
            }
            else
            {
                placedItems.Add(item);
            }

            //SetGridCell(item);
            item.cell.isEmpty = false;

            return true;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Destroy(bool modifyLevel = false)
        {
            if (modifyLevel)
            {
                Globals.level.rails.Remove(this as Rail);
            }
            else
            {
                placedItems.Remove(this);
                //Globals.level.rails.Remove(cell.rail);
            }
            cell.item = null;
            cell.isEmpty = true;

            if (power != null)
            {
                power.Destroy();
            }

            //SetGridCell(new Cell(x, y));
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, cell.ToVector2(), null, Color.White, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0f);
        }

        public static void DrawItems(SpriteBatch batch)
        {
            foreach (var item in placedItems)
            {
                item.Draw(batch);
            }
        }
    }
}