using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainStation;

namespace LevelEditor
{
    public class Editor
    {
        public PlaceType placeObject;
        public Rail.RailType selectedRail;
        
        #region Placing rails
        private bool placingRail;

        private Vector2 railStartPosition;
        private Vector2 railEndPosition;
        private int length;

        private Rectangle drawRect;
        private Vector2 origin;

        private StraightRail.Orientation orientation;
        private float rotation;

        private Keys rotateKey = Keys.Q;
        private TurnRail.TurnDirection turnOrientation;

        private Vector2 mousePosition;
        #endregion

        public Editor()
        {
            selectedRail = Rail.RailType.Straight;
        }

        public void Update(GameTime gameTime)
        {
            mousePosition = Cell.SnapToGrid(Input.mousePosition);

            if (placeObject == PlaceType.Rail)
            {
                #region StraightRail
                if (selectedRail == Rail.RailType.Straight || selectedRail == Rail.RailType.Boost)
                {
                    if (Input.mouseState.LeftButton == ButtonState.Pressed && Input.prevMouseState.LeftButton == ButtonState.Released && !placingRail && Cell.GetCell(mousePosition).isEmpty) // start placing rail
                    {
                        placingRail = true;

                        railStartPosition = mousePosition;

                        drawRect = Rectangle.Empty;
                    }
                    else if (Input.mouseState.LeftButton == ButtonState.Released && placingRail) // place the rail
                    {
                        placingRail = false;

                        length = (int)Math.Max(Math.Abs(railEndPosition.Y - railStartPosition.Y), Math.Abs(railEndPosition.X - railStartPosition.X)) / Cell.cellWidth + 1;

                        if (Math.Abs(railEndPosition.X - railStartPosition.X) > Math.Abs(railEndPosition.Y - railStartPosition.Y))
                        {
                            orientation = StraightRail.Orientation.Horizontal;
                            rotation = MathHelper.PiOver2;

                            if (railEndPosition.X < railStartPosition.X) // switch the start and end points so that the start of the rail is always on top or to the left
                            {
                                float temp = railStartPosition.X;
                                railStartPosition.X = railEndPosition.X;
                                railEndPosition.X = temp;
                            }
                        }
                        else
                        {
                            orientation = StraightRail.Orientation.Vertical;
                            rotation = 0f;

                            if (railEndPosition.Y < railStartPosition.Y)
                            {
                                float temp = railStartPosition.Y;
                                railStartPosition.Y = railEndPosition.Y;
                                railEndPosition.Y = temp;
                            }
                        }

                        switch (selectedRail)
                        {
                            case Rail.RailType.Straight:
                                StraightRail.PlaceMultipleRails(railStartPosition, length, orientation, false, true); // placing the rails
                                break;
                            case Rail.RailType.Boost:
                                StraightRail.PlaceMultipleRails(new BoosterRail(Cell.GetCell(railStartPosition), orientation, false), length, true); // placing the rails
                                break;
                        }
                    }

                    else if (placingRail)
                    {
                        if (Input.mouseState.RightButton == ButtonState.Pressed)
                        {
                            placingRail = false;
                        }

                        railEndPosition = Cell.SnapToGrid(mousePosition);
                        length = (int)Math.Max(Math.Abs(railEndPosition.Y - railStartPosition.Y), Math.Abs(railEndPosition.X - railStartPosition.X)) / Cell.cellWidth + 1; // limit the length of the rail when drawn by amount of resources

                        if (Math.Abs(railEndPosition.X - railStartPosition.X) > Math.Abs(railEndPosition.Y - railStartPosition.Y)) // we want to place it horizontally
                        {
                            rotation = MathHelper.PiOver2;

                            if (railEndPosition.X > railStartPosition.X) // placing rails from left to right
                            {
                                drawRect = new Rectangle(0, Rail.straightTexture.Height * -length, Rail.straightTexture.Width, length * Cell.cellHeight);
                                origin = new Vector2(Rail.straightTexture.Width / 2f, Rail.straightTexture.Height * (length - 0.5f));
                            }
                            else
                            {
                                drawRect = new Rectangle(0, 0, Rail.straightTexture.Width, length * Cell.cellHeight);
                                origin = new Vector2(Rail.straightTexture.Width / 2f, Rail.straightTexture.Height / 2f);
                            }
                        }
                        else                                                                                                       // we want to place it vertically
                        {
                            rotation = 0f;
                            if (railEndPosition.Y < railStartPosition.Y) // placing rails from bottom to top
                            {
                                drawRect = new Rectangle(0, Rail.straightTexture.Height * -length, Rail.straightTexture.Width, length * Cell.cellHeight);
                                origin = new Vector2(Rail.straightTexture.Width / 2f, Rail.straightTexture.Height * (length - 0.5f));
                            }
                            else
                            {
                                drawRect = new Rectangle(0, 0, Rail.straightTexture.Width, length * Cell.cellHeight);
                                origin = new Vector2(Rail.straightTexture.Width / 2f, Rail.straightTexture.Height / 2f);
                            }
                        }
                    }
                }
                #endregion

                #region TurnRails

                if (selectedRail == Rail.RailType.Turn && Input.mouseState.LeftButton == ButtonState.Released && Input.prevMouseState.LeftButton == ButtonState.Pressed) // place a turn-railtrack
                {
                    Rail.PlaceRail(new TurnRail(Cell.GetCell(Input.mouseState.Position.ToVector2()), turnOrientation, false), true);
                }

                #endregion

                #region Rotation
                if (Input.keyState.IsKeyDown(rotateKey) && Input.prevKeyState.IsKeyUp(rotateKey)) // rotate the track
                {
                    switch (selectedRail)
                    {
                        case Rail.RailType.Straight:
                            orientation = (StraightRail.Orientation)Globals.IncrementEnum(typeof(StraightRail.Orientation), (int)orientation, 1);
                            break;
                        case Rail.RailType.Turn:
                            turnOrientation = (TurnRail.TurnDirection)Globals.IncrementEnum(typeof(TurnRail.TurnDirection), (int)turnOrientation, 1);
                            break;
                        case Rail.RailType.Boost:
                            orientation = (StraightRail.Orientation)Globals.IncrementEnum(typeof(StraightRail.Orientation), (int)orientation, 1);
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region RailSwitching

                if (Input.mouseState.ScrollWheelValue != Input.prevMouseState.ScrollWheelValue) // detecting mouse-scrolling
                {
                    selectedRail = Input.mouseState.ScrollWheelValue > Input.prevMouseState.ScrollWheelValue ? (Rail.RailType)Globals.IncrementEnum(typeof(Rail.RailType), (int)selectedRail, 1) : selectedRail = (Rail.RailType)Globals.IncrementEnum(typeof(Rail.RailType), (int)selectedRail, -1);
                }

                #endregion
            }
            else if (Input.mouseState.LeftButton == ButtonState.Pressed && Input.prevMouseState.LeftButton == ButtonState.Released)
            {
                Cell cell = Cell.GetCell(mousePosition);
                switch (placeObject)
                {
                    case PlaceType.Obstacle:
                        if (!cell.isEmpty)
                            break;
                        cell.isEmpty = false;
                        Globals.level.obstacles.Add(new Obstacle(cell));
                        break;
                    case PlaceType.Bump:
                        if (!cell.isEmpty)
                            break;
                        cell.isEmpty = false;
                        Globals.level.bumps.Add(new Bump(0.2f, cell));
                        break;
                    case PlaceType.Portal:
                        if (!cell.isEmpty)
                            break;
                        cell.isEmpty = false;
                        Globals.level.portals.Add(new Portal(cell));
                        break;
                    default:
                        break;
                }
            }
            #region Removing objects
            if (Input.mouseState.RightButton == ButtonState.Pressed)
            {
                Cell cell = Cell.GetCell(mousePosition);
                if (cell.rail != null)
                    cell.rail.Destroy(true);
                else if (cell.obstacle != null)
                {
                    Globals.level.obstacles.Remove(cell.obstacle);
                    cell.obstacle = null;
                    cell.isEmpty = true;
                }
                else if (cell.bump != null)
                {
                    Globals.level.bumps.Remove(cell.bump);
                    cell.bump = null;
                    cell.isEmpty = true;
                }
                else if (cell.portal != null)
                {
                    Globals.level.portals.Remove(cell.portal);
                    cell.portal = null;
                    cell.isEmpty = true;
                }
                else if (cell.orb != null)
                {
                    Globals.level.orbs.Remove(cell.orb);
                    cell.orb = null;
                    cell.isEmpty = true;
                }
            }
            #endregion
        }

        public void Draw(SpriteBatch batch)
        {
            Texture2D drawTexture;
            Cell cell = Cell.GetCell(mousePosition);

            if (placeObject == PlaceType.Rail)
            {
                if (placingRail) // draw the placing of the rail
                {
                    switch (selectedRail) // please optimize
                    {
                        case Rail.RailType.Straight:
                            drawTexture = Rail.straightTexture;
                            break;
                        case Rail.RailType.Boost:
                            drawTexture = Rail.boosterTexture;
                            break;
                        default:
                            drawTexture = Rail.straightTexture;
                            break;
                    }
                    batch.Draw(drawTexture, railStartPosition, drawRect, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
                else if (cell.isEmpty)
                {
                    switch (selectedRail)
                    {
                        case Rail.RailType.Straight:
                            batch.Draw(Rail.straightTexture, mousePosition, null, new Color(255, 255, 255, 100), 0f, Rail.straightTextureOrigin, 1f, SpriteEffects.None, 0f);
                            break;
                        case Rail.RailType.Turn:
                            batch.Draw(Rail.turnTexture, mousePosition, null, new Color(255, 255, 255, 100), 0f, Rail.turnTextureOrigin, 1f, TurnRail.GetEffects(turnOrientation), 0f);
                            break;
                        case Rail.RailType.Boost:
                            batch.Draw(Rail.boosterTexture, mousePosition, null, new Color(255, 255, 255, 100), 0f, Rail.turnTextureOrigin, 1f, SpriteEffects.None, 0f);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (cell.isEmpty)
            {
                switch (placeObject)
                {
                    case PlaceType.Obstacle:
                        drawTexture = Obstacle.obstacleTexture;
                        break;
                    case PlaceType.Bump:
                        drawTexture = Bump.bumpTexture;
                        break;
                    case PlaceType.Portal:
                        drawTexture = Portal.portalTexture;
                        break;
                    default:
                        drawTexture = null;
                        break;
                }

                batch.Draw(drawTexture, mousePosition, null, new Color(255, 255, 255, 100), 0f, Rail.turnTextureOrigin, 1f, SpriteEffects.None, 0f);
            }
        }

        public enum PlaceType
        {
            Rail, Obstacle, Bump, Portal, Orb
        };
    }
}
