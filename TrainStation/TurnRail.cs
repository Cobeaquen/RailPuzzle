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
    public class TurnRail : Rail
    {
        [ProtoMember(1)]
        public TurnDirection turnDir;

        [ProtoMember(2)]
        public bool turnOnCartPass = false;

        private SpriteEffects orientation;

        public TurnRail(Cell cell, TurnDirection turnDirection, bool removable = true) : base(cell, removable)
        {
            turnDir = turnDirection;
            texture = Textures.RailTurn;

            orientation = GetEffects(turnDir);

            if (cell.isEmpty)
            {
                power = new PowerConnection(cell.ToVector2(), Connection.Input, 1, null, OnSignalUpdate);
            }
        }
        public TurnRail()
        {

        }

        public void OnSignalUpdate(bool state, bool prevState)
        {
            if (state == prevState)
                return;
            if (Globals.debugMessages)
                Console.WriteLine("The rail received a signal to change direction...");

            switch (turnDir)
            {
                case TurnDirection.DownRight:
                    if (Cell.GetCell(cell, 0, -1).item is Rail)
                        turnDir = TurnDirection.RightUp;
                    else if (Cell.GetCell(cell, -1, 0).item is Rail)
                        turnDir = TurnDirection.DownLeft;
                    break;
                case TurnDirection.DownLeft:
                    if (Cell.GetCell(cell, 0, -1).item is Rail)
                        turnDir = TurnDirection.LeftUp;
                    else if (Cell.GetCell(cell, 1, 0).item is Rail)
                        turnDir = TurnDirection.DownRight;
                    break;
                case TurnDirection.RightUp:
                    if (Cell.GetCell(cell, 0, 1).item is Rail)
                        turnDir = TurnDirection.DownRight;
                    else if (Cell.GetCell(cell, -1, 0).item is Rail)
                        turnDir = TurnDirection.LeftUp;
                    break;
                case TurnDirection.LeftUp:
                    if (Cell.GetCell(cell, 0, 1).item is Rail)
                        turnDir = TurnDirection.DownLeft;
                    else if (Cell.GetCell(cell, 1, 0).item is Rail)
                        turnDir = TurnDirection.RightUp;
                    break;
                default:
                    break;
            }
            orientation = GetEffects(turnDir);
        }

        public override void CartPassOver(Cart cart)
        {
            if (turnOnCartPass) // switch direction of the turn
            {
                switch (turnDir)
                {
                    case TurnDirection.DownRight:
                        if (cart.moveDirection == Cart.MoveDirection.Down)
                            turnDir = TurnDirection.RightUp;
                        else if (cart.moveDirection == Cart.MoveDirection.Right)
                            turnDir = TurnDirection.DownLeft;
                        break;
                    case TurnDirection.DownLeft:
                        if (cart.moveDirection == Cart.MoveDirection.Down)
                            turnDir = TurnDirection.LeftUp;
                        else if (cart.moveDirection == Cart.MoveDirection.Left)
                            turnDir = TurnDirection.DownRight;
                        break;
                    case TurnDirection.RightUp:
                        if (cart.moveDirection == Cart.MoveDirection.Up)
                            turnDir = TurnDirection.DownRight;
                        else if (cart.moveDirection == Cart.MoveDirection.Right)
                            turnDir = TurnDirection.LeftUp;
                        break;
                    case TurnDirection.LeftUp:
                        if (cart.moveDirection == Cart.MoveDirection.Up)
                            turnDir = TurnDirection.DownLeft;
                        else if (cart.moveDirection == Cart.MoveDirection.Left)
                            turnDir = TurnDirection.RightUp;
                        break;
                    default:
                        break;
                }

                orientation = GetEffects(turnDir);
            }

            base.CartPassOver(cart);
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            power.Destroy();
            base.Destroy(modifyLevel);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            batch.Draw(texture, cell.ToVector2(), null, color, 0f, origin, 1f, orientation, 0.05f);
        }

        public static SpriteEffects GetEffects(TurnDirection turnOrientation)
        {
            SpriteEffects orientation = SpriteEffects.None;

            switch (turnOrientation)
            {
                case TurnDirection.DownRight:
                    orientation = SpriteEffects.FlipHorizontally;
                    break;
                case TurnDirection.DownLeft:
                    orientation = SpriteEffects.None;
                    break;
                case TurnDirection.RightUp:
                    orientation = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    break;
                case TurnDirection.LeftUp:
                    orientation = SpriteEffects.FlipVertically;
                    break;
                default:
                    break;
            }

            return orientation;
        }

        public enum TurnDirection
        {
            DownRight, DownLeft, RightUp, LeftUp
        };
    }
}