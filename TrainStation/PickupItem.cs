using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation
{
    public class PickupItem : Cell
    {
        public Texture2D texture;
        public Vector2 origin;

        public PickupItem(Cell cell)
        {
            SetPosition(cell);

            cell.pickupItem = this;
        }
        public PickupItem() { }

        public virtual void Pickup()
        {
            Globals.level.pickupItems.Remove(this);
            GetCell(this).pickupItem = null;
        }

        public static bool PlaceItem(PickupItem item)
        {
            if (!GetCell(item).isEmpty)
                return false;

            Globals.level.pickupItems.Add(item);

            return true;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, ToVector2(), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0.1f);
        }
    }
}
