using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation
{
    public class CellItem : Cell
    {
        protected Texture2D texture;
        protected Vector2 origin;

        public CellItem(Cell cell)
        {
            SetPosition(cell);

            cell.cellItem = this;
        }
        public CellItem() { }

        public static bool PlaceItem(CellItem item)
        {
            Cell cell = GetCell(item);
            
            Globals.level.cellItems.Add(item);

            item.isEmpty = false;

            return true;
        }

        public virtual void OnCartPass(Cart cart)
        {

        }

        public virtual void OnCartCollision(Cart cart)
        {
            
        }

        public virtual void Destroy()
        {
            Globals.level.cellItems.Remove(this);

            isEmpty = true;

            SetGridCell(new Cell(x, y));
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, ToVector2(), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
