using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TrainStation
{
    public class Cart
    {
        public Vector2 position;
        public float rotation;
        public Cell cell;

        public Vector2 move;

        public Rail previousRail;
        public Rail futureRail;

        public MoveDirection moveDirection;

        #region Physics
        public float stretch = 12.2f;
        public float startVel = 0f;
        public float endVel;
        public float velocity;
        public float acceleration;
        public float mass = 1f;
        public float force;
        public float startingWork = 100f;
        public float friction = 2f;
        #endregion

        private float t;

        private Texture2D texture;
        private Vector2 origin;

        public Cart(Vector2 position)
        {
            this.position = position;
            texture = Textures.Cart;
            origin = Cell.SpriteOrigin;

            cell = Cell.GetCell(position);
            CalculateTravelDirection();
            SetFutureCell();

            force = startingWork / stretch;

            acceleration = (-force / mass);
            // figure out time for the cart to reach it's destination - lerp between velocities along the time, till it reaches 0.
            startVel = (float)Math.Sqrt(-2 * acceleration * stretch);

            velocity = startVel;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public void Move(GameTime gameTime)
        {
            move = Vector2.Zero;

            if (moveDirection == MoveDirection.None)
            {
                Destroy();
            }
            
            if (futureRail != null) // move
            {
                position = Vector2.Lerp(cell.ToVector2(), futureRail.cell.ToVector2(), t);

                //velocity = MathHelper.Lerp(startVel, endVel, acceleration * -t);

                velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f;

                //velocity *= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (velocity <= endVel) // the cart is no longer travelling
                {
                    velocity = endVel;
                }

                t += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f;

                if (t >= 1)
                {
                    t = 0;

                    switch (moveDirection)
                    {
                        case MoveDirection.None:
                            break;
                        case MoveDirection.Up:
                            rotation = MathHelper.Pi;
                            break;
                        case MoveDirection.Down:
                            rotation = 0;
                            break;
                        case MoveDirection.Right:
                            rotation = -MathHelper.PiOver2;
                            break;
                        case MoveDirection.Left:
                            rotation = MathHelper.PiOver2;
                            break;
                        default:
                            break;
                    }

                    previousRail = (Rail)cell.item;
                    cell = Cell.GetCell(position);

                    position = futureRail.cell.ToVector2();

                    if (cell.item is Rail rail)
                        rail.CartPassOver(this);

                    SetFutureCell();

                    if (previousRail != null)
                        previousRail.CartStopApproaching();

                    if (futureRail != null)
                        futureRail.CartStartApproaching();

                    if (cell.item is Rail rl)
                    {
                        rl.CartStartApproaching();
                    }

                    if (futureRail == null)
                    {
                        CalculateTravelDirection();
                        SetFutureCell();
                        if (futureRail != null)
                            futureRail.CartStartApproaching();
                    }
                }
            }
            else
            {
                CalculateTravelDirection();
                SetFutureCell();

                if (futureRail == null)
                {
                    Destroy();
                }
            }
        }

        public void Destroy()
        {
            if (cell.item is Rail rl)
                rl.CartStopApproaching();
            if (futureRail != null)
                futureRail.CartStopApproaching();
            Globals.player.carts.Remove(this);
        }

        public void Teleport(Portal toPortal)
        {
            position = toPortal.ToVector2();
            previousRail.CartStopApproaching();
            previousRail = (Rail)cell.item;
            cell = Cell.GetCell(position);
            moveDirection = MoveDirection.None;
            CalculateTravelDirection();
            SetFutureCell();
            if (cell.item is Rail rl)
            {
                rl.CartStartApproaching();
            }
            futureRail.CartStartApproaching();
            //previousRail.CartStopApproaching();
        }

        public void SetFutureCell()
        {
            Cell newCell = null;

            switch (moveDirection)
            {
                case MoveDirection.None:
                    break;
                case MoveDirection.Up:
                    newCell = Cell.GetCell(cell, 0, -1);
                    if (newCell.item is Rail) // useless?
                    {
                        if (newCell.item is TurnRail rl) // the track is a turn
                        {
                            switch (rl.turnDir)
                            {
                                case TurnRail.TurnDirection.DownRight:
                                    moveDirection = MoveDirection.Right;
                                    break;
                                case TurnRail.TurnDirection.DownLeft:
                                    moveDirection = MoveDirection.Left;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (((StraightRail)newCell.item).orientation != StraightRail.Orientation.Vertical)
                            newCell = null;
                    }
                    break;

                case MoveDirection.Down:
                    newCell = Cell.GetCell(cell, 0, 1);
                    if (newCell.item is Rail)
                    {
                        if (newCell.item is TurnRail rl)
                        {
                            switch (rl.turnDir)
                            {
                                case TurnRail.TurnDirection.RightUp:
                                    moveDirection = MoveDirection.Right;
                                    break;
                                case TurnRail.TurnDirection.LeftUp:
                                    moveDirection = MoveDirection.Left;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (((StraightRail)newCell.item).orientation != StraightRail.Orientation.Vertical)
                            newCell = null;
                    }
                    break;

                case MoveDirection.Right:
                    newCell = Cell.GetCell(cell, 1, 0);
                    if (newCell.item is Rail)
                    {
                        if (newCell.item is TurnRail rl)
                        {
                            switch (rl.turnDir)
                            {
                                case TurnRail.TurnDirection.LeftUp:
                                    moveDirection = MoveDirection.Up;
                                    break;
                                case TurnRail.TurnDirection.DownLeft:
                                    moveDirection = MoveDirection.Down;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (((StraightRail)newCell.item).orientation != StraightRail.Orientation.Horizontal)
                            newCell = null;
                    }
                    break;

                case MoveDirection.Left:
                    newCell = Cell.GetCell(cell, -1, 0);

                    if (newCell.item is Rail)
                    {
                        if (newCell.item is TurnRail rl)
                        {
                            switch (rl.turnDir)
                            {
                                case TurnRail.TurnDirection.RightUp:
                                    moveDirection = MoveDirection.Up;
                                    break;
                                case TurnRail.TurnDirection.DownRight:
                                    moveDirection = MoveDirection.Down;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (((StraightRail)newCell.item).orientation != StraightRail.Orientation.Horizontal)
                            newCell = null;
                    }
                    break;
                default:
                    break;
            }
            //if (newCell == null)
            //{
            //    futureRail = null;
            //    return;
            //}
            futureRail = newCell != null && newCell.item != null && newCell.item is Rail ? (Rail)newCell.item : null;
        }

        public StraightRail.Orientation DirectionToOrientation(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.None:
                    break;
                case MoveDirection.Up:
                    return StraightRail.Orientation.Vertical;
                case MoveDirection.Down:
                    return StraightRail.Orientation.Vertical;
                case MoveDirection.Right:
                    return StraightRail.Orientation.Horizontal;
                case MoveDirection.Left:
                    return StraightRail.Orientation.Horizontal;
                default:
                    break;
            }
            return StraightRail.Orientation.Vertical;
        }

        public void CalculateTravelDirection()
        {
            if (cell.item is StraightRail rl)
            {
                Cell searchCell = Cell.GetCell(cell, 1, 0);

                if (searchCell.item is Rail) // track found to the right
                {
                    Rail rail = (Rail)searchCell.item;

                    if (moveDirection == MoveDirection.None && rail is StraightRail && ((StraightRail)rail).orientation == StraightRail.Orientation.Horizontal && rl.orientation == StraightRail.Orientation.Horizontal)
                    {

                        moveDirection = MoveDirection.Right;
                        if (Globals.debugMessages)
                            Console.WriteLine("Rail found at: Right");

                        return;
                    }
                    else if (rail is TurnRail)
                    {
                        if (((TurnRail)rail).turnDir == TurnRail.TurnDirection.LeftUp || ((TurnRail)rail).turnDir == TurnRail.TurnDirection.DownLeft)
                        {
                            moveDirection = MoveDirection.Right;
                            return;
                        }
                    }
                }

                searchCell = Cell.GetCell(cell, -1, 0);

                if (searchCell.item is Rail) // track found to the left
                {
                    Rail rail = (Rail)searchCell.item;
                    if (moveDirection == MoveDirection.None && rail is StraightRail && ((StraightRail)rail).orientation == StraightRail.Orientation.Horizontal && rl.orientation == StraightRail.Orientation.Horizontal)
                    {

                        moveDirection = MoveDirection.Left;
                        if (Globals.debugMessages)
                            Console.WriteLine("Rail found at: Left");

                        return;
                    }
                    else if (rail is TurnRail)
                    {
                        if (((TurnRail)rail).turnDir == TurnRail.TurnDirection.RightUp || ((TurnRail)rail).turnDir == TurnRail.TurnDirection.DownRight)
                        {
                            moveDirection = MoveDirection.Left;
                            return;
                        }
                    }
                }

                searchCell = Cell.GetCell(cell, 0, -1);

                if (searchCell.item is Rail) // track found up top
                {
                    Rail rail = (Rail)searchCell.item;
                    if (moveDirection == MoveDirection.None && rail is StraightRail && ((StraightRail)rail).orientation == StraightRail.Orientation.Vertical && rl.orientation == StraightRail.Orientation.Vertical)
                    {

                        moveDirection = MoveDirection.Up;
                        if (Globals.debugMessages)
                            Console.WriteLine("Rail found at: Top");

                        return;
                    }
                    else if (rail is TurnRail)
                    {
                        if (((TurnRail)rail).turnDir == TurnRail.TurnDirection.DownRight || ((TurnRail)rail).turnDir == TurnRail.TurnDirection.DownLeft)
                        {
                            moveDirection = MoveDirection.Up;
                            return;
                        }
                    }
                }

                searchCell = Cell.GetCell(cell, 0, 1);

                if (searchCell.item is Rail) // track found at bottom
                {
                    Rail rail = (Rail)searchCell.item;
                    if (moveDirection == MoveDirection.None && rail is StraightRail && ((StraightRail)rail).orientation == StraightRail.Orientation.Vertical && rl.orientation == StraightRail.Orientation.Vertical)
                    {
                        moveDirection = MoveDirection.Down;
                        if (Globals.debugMessages)
                            Console.WriteLine("Rail found at: Bottom");

                        return;
                    }
                    else if (rail is TurnRail)
                    {
                        if (((TurnRail)rail).turnDir == TurnRail.TurnDirection.LeftUp || ((TurnRail)rail).turnDir == TurnRail.TurnDirection.RightUp)
                        {
                            moveDirection = MoveDirection.Down;
                            return;
                        }
                    }
                }

                moveDirection = MoveDirection.None;
                if (Globals.debugMessages)
                    Console.WriteLine("Rail not found");
            }
        }

        public void Draw(SpriteBatch batch)
        {
            //Globals.baseEffect.Parameters["useLight"].SetValue(0);

            //Globals.baseEffect.CurrentTechnique.Passes[0].Apply();

            batch.Draw(texture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0.2f);
        }

        public enum MoveDirection
        {
            None, Up, Down, Right, Left
        };
    }
}