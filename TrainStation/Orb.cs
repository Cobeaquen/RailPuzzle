using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    public class Orb : PickupItem
    {

        public Orb(Cell cell) : base(cell)
        {
            //Rail.PlaceStaticRail(new StraightRail(cell.ToVector2(), orientation));

            cell.pickupItem = this;

            texture = Textures.Orb; // make static texture for optimization.
            origin = SpriteOrigin;

        }
        public Orb()
        {

        }

        public override void Pickup()
        {
            base.Pickup();
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
