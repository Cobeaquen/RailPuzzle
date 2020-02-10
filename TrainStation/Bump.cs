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
    public class Bump : CellItem
    {
        /// <summary>
        /// Between 0 and 1
        /// </summary>
        [ProtoMember(1)]
        public float slowness;

        public Bump(Cell cell, float slowness) : base(cell)
        {
            this.slowness = slowness;

            isEmpty = true;

            texture = Textures.Bump;
            origin = SpriteOrigin;
        }

        public Bump() { }

        public override void OnCartPass(Cart cart)
        {
            cart.velocity = MathHelper.Clamp(cart.velocity - cart.startVel * slowness, 0, cart.startVel);
            base.OnCartPass(cart);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}