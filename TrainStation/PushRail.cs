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
    public class PushRail : StraightRail
    {
        public bool state;

        public PushRail(Cell cell, Orientation orientation, bool removable = true) : base(cell, orientation, removable)
        {
            texture = Textures.RailPush;

            power = new PowerConnection(cell.ToVector2(), Connection.Input, 0, null, OnSignalUpdate);
        }

        public void OnSignalUpdate(bool state, bool prevState)
        {
            this.state = state;
        }

        public override void CartPassOver(Cart cart)
        {
            if (state) // push the cart backwards
            {
                switch (cart.moveDirection)
                {
                    case Cart.MoveDirection.None:
                        break;
                    case Cart.MoveDirection.Up:
                        cart.moveDirection = Cart.MoveDirection.Down;
                        break;
                    case Cart.MoveDirection.Down:
                        cart.moveDirection = Cart.MoveDirection.Up;
                        break;
                    case Cart.MoveDirection.Right:
                        cart.moveDirection = Cart.MoveDirection.Left;
                        break;
                    case Cart.MoveDirection.Left:
                        cart.moveDirection = Cart.MoveDirection.Right;
                        break;
                    default:
                        break;
                }
                //cart.SetFutureCell();
            }
            base.CartPassOver(cart);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            base.Destroy(modifyLevel);
        }

        public override Rail Clone()
        {
            return base.Clone();
        }
    }
}
