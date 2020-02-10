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
    public class BoosterRail : StraightRail
    {
        public BoosterRail(Cell cell, Orientation faceDirection, bool removable = true) : base(cell, faceDirection, removable)
        {
            texture = Textures.RailBooster;
        }
        public BoosterRail()
        {

        }

        public override void CartPassOver(Cart cart)
        {
            cart.velocity = MathHelper.Clamp(cart.velocity + cart.startVel/3f, 0, cart.startVel);

            base.CartPassOver(cart);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            base.Destroy(modifyLevel);
        }
    }
}
