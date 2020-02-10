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
    public class Player
    {
        public List<Cart> carts;

        public Camera camera;

        public GameState gameState;

        public PlacableItem selectedItem;

        public bool canPlaceRail = true;

        private Vector2 mousePosition;

        #region resources
        private int straightRailCount;
        private int turnRailCount;
        private int boostRailCount;
        #endregion

        #region RailPlacement

        private bool placingRail;

        private bool scrollBetweenRails = false; // bad feature

        private Vector2 railStartPosition;
        private Vector2 railEndPosition;
        private int length;

        private Rectangle drawRect;
        private Vector2 origin;

        private StraightRail.Orientation orientation;
        private float straightRailRotation;
        private float straightRotation;

        private readonly Keys rotateKey = Keys.Q;
        private TurnRail.TurnDirection turnOrientation;
        private SpriteEffects turnOrientationEffect;

        #endregion

        public Player()
        {
            gameState = GameState.Editing;
            selectedItem = PlacableItem.StraightRail;
            FillResources();
            camera = new Camera(Globals.screenCenter, -5000, 5000, -4000, 4000);
            carts = new List<Cart>();

            turnOrientationEffect = TurnRail.GetEffects(turnOrientation);
        }

        public void RunSimulation(Button btn = null)
        {
            gameState = GameState.Simulating;
            if (btn != null)
            {
                carts.Clear();
                Globals.ResetLevel();
            }
            carts.Add(new Cart(Globals.level.startCell.ToVector2() + new Vector2(Cell.cellWidth, 0)));
        }
        public void Reset(Button btn)
        {
            gameState = GameState.Editing;
            Globals.ResetLevel();
            DestroyAllCarts();
        }
        public void DestroyAllCarts()
        {
            if (carts.Count > 0)
            {
                for (int i = 0; i < carts.Count; i++)
                {
                    carts[i].Destroy();
                }
                carts.Clear();
            }
        }
        public void TogglePowerDisplay(Button btn)
        {
            Globals.displayPower = !Globals.displayPower;
            btn.text.text = Globals.displayPower ? "Power: shown" : "Power: hidden";
        }

        public void LevelFinished()
        {
            if (Globals.debugMessages)
                Console.WriteLine("LEVEL FINISHED!");
            gameState = GameState.LevelFinished;
            DestroyAllCarts();
        }

        public void Update(GameTime gameTime)
        {
            camera.MoveWASD(gameTime);

            // check for interaction before placing things

            if (Input.mouseCell.item is Lever lever && Input.mouseClicked)
            {
                lever.ToggleLever();
            }

            switch (gameState)
            {
                case GameState.Editing:
                    break;
                case GameState.Simulating:
                    if (carts.Count > 0)
                    {
                        for (int i = 0; i < carts.Count; i++)
                        {
                            carts[i].Update(gameTime);
                        }
                        if (carts.Count == 0)
                            gameState = GameState.Editing;
                    }
                    return;
                default:
                    break;
            }

            mousePosition = Input.mouseCell.ToVector2();

            #region Item Placement

            #region StraightRail
            if ((selectedItem == PlacableItem.StraightRail || selectedItem == PlacableItem.BoosterRail) && canPlaceRail)
            {
                if (Input.mouseClicked && !placingRail && Input.mouseCell.isEmpty) // start placing rail
                {
                    placingRail = true;

                    railStartPosition = mousePosition;

                    drawRect = Rectangle.Empty;
                }
                else if (Input.mouseState.LeftButton == ButtonState.Released && placingRail) // place the rail if this is true
                {
                    placingRail = false;

                    length = (int)Math.Max(Math.Abs(railEndPosition.Y - railStartPosition.Y), Math.Abs(railEndPosition.X - railStartPosition.X)) / Cell.cellWidth + 1;

                    float xDif = Math.Abs(railEndPosition.X - railStartPosition.X);
                    float yDif = Math.Abs(railEndPosition.Y - railStartPosition.Y);

                    StraightRail.Orientation prevOrientation = orientation;

                    if (xDif > yDif)
                    {
                        orientation = StraightRail.Orientation.Horizontal;
                        straightRailRotation = MathHelper.PiOver2;

                        if (railEndPosition.X < railStartPosition.X) // switch the start and end points so that the start of the rail is always on top or to the left
                        {
                            float temp = railStartPosition.X;
                            railStartPosition.X = railEndPosition.X;
                            railEndPosition.X = temp;
                        }
                    }
                    else
                    {
                        if (xDif != yDif)
                        {
                            orientation = StraightRail.Orientation.Vertical;
                        }
                        straightRailRotation = 0f;

                        if (railEndPosition.Y < railStartPosition.Y)
                        {
                            float temp = railStartPosition.Y;
                            railStartPosition.Y = railEndPosition.Y;
                            railEndPosition.Y = temp;
                        }
                    }

                    switch (selectedItem)
                    {
                        case PlacableItem.StraightRail:
                            length = straightRailCount < length ? straightRailCount : length;

                            straightRailCount = MathHelper.Clamp(straightRailCount - StraightRail.PlaceMultipleRails(railStartPosition, length, orientation), 0, Globals.level.straightRailCount); // placing the rails

                            GUI.straightRailElement.ChangeText(straightRailCount.ToString());
                            break;
                        case PlacableItem.BoosterRail:
                            length = boostRailCount < length ? boostRailCount : length;

                            boostRailCount = MathHelper.Clamp(boostRailCount - StraightRail.PlaceMultipleRails(new BoosterRail(Cell.GetCell(railStartPosition), orientation), length), 0, Globals.level.boostRailCount); // placing the rails

                            GUI.boostRailElement.ChangeText(boostRailCount.ToString());
                            break;
                    }

                    orientation = prevOrientation;
                }

                else if (placingRail) // Display the rails being placed
                {
                    if (Input.mouseState.RightButton == ButtonState.Pressed)
                    {
                        placingRail = false;
                    }

                    railEndPosition = mousePosition;
                    length = (int)Math.Max(Math.Abs(railEndPosition.Y - railStartPosition.Y), Math.Abs(railEndPosition.X - railStartPosition.X)) / Cell.cellWidth + 1; // limit the length of the rail when drawn by amount of resources

                    float xDif = Math.Abs(railEndPosition.X - railStartPosition.X);
                    float yDif = Math.Abs(railEndPosition.Y - railStartPosition.Y);

                    if (xDif > yDif) // we want to place it horizontally
                    {
                        straightRailRotation = MathHelper.PiOver2;

                        if (railEndPosition.X > railStartPosition.X) // placing rails from left to right
                        {
                            drawRect = new Rectangle(0, Textures.RailStraight.Height * -length, Textures.RailStraight.Width, length * Cell.cellHeight);
                            origin = new Vector2(Textures.RailStraight.Width / 2f, Textures.RailStraight.Height * (length - 0.5f));
                        }
                        else
                        {
                            drawRect = new Rectangle(0, 0, Textures.RailStraight.Width, length * Cell.cellHeight);
                            origin = new Vector2(Textures.RailStraight.Width / 2f, Textures.RailStraight.Height / 2f);
                        }
                    }
                    else                                                                                                       // we want to place it vertically
                    {
                        if (xDif != yDif)
                        {
                            straightRailRotation = 0f;
                        }
                        else
                        {
                            straightRailRotation = straightRotation;
                        }

                        if (railEndPosition.Y < railStartPosition.Y) // placing rails from bottom to top
                        {
                            drawRect = new Rectangle(0, Textures.RailStraight.Height * -length, Textures.RailStraight.Width, length * Cell.cellHeight);
                            origin = new Vector2(Textures.RailStraight.Width / 2f, Textures.RailStraight.Height * (length - 0.5f));
                        }
                        else
                        {
                            drawRect = new Rectangle(0, 0, Textures.RailStraight.Width, length * Cell.cellHeight);
                            origin = new Vector2(Textures.RailStraight.Width / 2f, Textures.RailStraight.Height / 2f);
                        }
                    }
                }
            }
            #endregion

            else if (Input.mouseClicked && Input.mouseCell.isEmpty && canPlaceRail)
            {
                if (selectedItem == PlacableItem.Lever) // Lever placement
                {
                    Item.PlaceItem(new Lever(Input.mouseCell));
                }

                else if (selectedItem == PlacableItem.DetectorRail)
                {
                    Item.PlaceItem(new DetectorRail(Input.mouseCell, orientation));
                }

                else if (selectedItem == PlacableItem.PushRail)
                {
                    Item.PlaceItem(new PushRail(Input.mouseCell, orientation));
                }
                else if (selectedItem == PlacableItem.InverterModule)
                {
                    Item.PlaceItem(new InverterModule(Input.mouseCell));
                }

                #region TurnRails

                else if (selectedItem == PlacableItem.TurnRail) // place a turn-railtrack
                {
                    if (turnRailCount > 0)
                    {
                        turnRailCount = MathHelper.Clamp(Item.PlaceItem(new TurnRail(Cell.GetCell(mousePosition), turnOrientation)) ? turnRailCount - 1 : turnRailCount, 0, Globals.level.turnRailCount);
                        GUI.turnRailElement.ChangeText(turnRailCount.ToString());
                    }
                }

                #endregion
            }

            #endregion

            #region Rotation
            if (Input.keyState.IsKeyDown(rotateKey) && Input.prevKeyState.IsKeyUp(rotateKey)) // rotate the track
            {
                if (selectedItem == PlacableItem.TurnRail)
                {
                    turnOrientation = (TurnRail.TurnDirection)Globals.IncrementEnum(typeof(TurnRail.TurnDirection), (int)turnOrientation, 1);
                    turnOrientationEffect = TurnRail.GetEffects(turnOrientation);
                }
                else if (selectedItem == PlacableItem.StraightRail || selectedItem == PlacableItem.BoosterRail || selectedItem == PlacableItem.DetectorRail || selectedItem == PlacableItem.PushRail)
                {
                    orientation = (StraightRail.Orientation)Globals.IncrementEnum(typeof(StraightRail.Orientation), (int)orientation, 1);
                    straightRotation = StraightRail.OrientationToRotation(orientation);
                }
            }
            #endregion

            #region ItemSwitching

            if (scrollBetweenRails && Input.mouseState.ScrollWheelValue != Input.prevMouseState.ScrollWheelValue) // detecting mouse-scrolling
            {
                selectedItem = Input.mouseState.ScrollWheelValue > Input.prevMouseState.ScrollWheelValue ? (PlacableItem)Globals.IncrementEnum(typeof(PlacableItem), (int)selectedItem, 1) : selectedItem = (PlacableItem)Globals.IncrementEnum(typeof(PlacableItem), (int)selectedItem, -1);
            }

            #endregion

            #region Removing items
            if (Input.mouseState.RightButton == ButtonState.Pressed)
            {
                Cell cell = Cell.GetCell(mousePosition);
                if (cell.item is Item item && item.removable)
                {
                    if (item is TurnRail)
                    {
                        turnRailCount++;
                        GUI.turnRailElement.ChangeText(turnRailCount.ToString());
                    }
                    else if (item is BoosterRail)
                    {
                        boostRailCount++;
                        GUI.boostRailElement.ChangeText(boostRailCount.ToString());
                    }
                    else if (item is StraightRail)
                    {
                        straightRailCount++;
                        GUI.straightRailElement.ChangeText(straightRailCount.ToString());
                    }

                    item.Destroy();
                }
            }
            #endregion
        }

        public void FillResources()
        {
            straightRailCount = Globals.level.straightRailCount;
            turnRailCount = Globals.level.turnRailCount;
            boostRailCount = Globals.level.boostRailCount;
        }

        public void Draw(SpriteBatch batch)
        {
            if (placingRail) // draw the placing of the rail
            {
                switch (selectedItem) // please optimize
                {
                    case PlacableItem.StraightRail:
                        batch.Draw(Textures.RailStraight, railStartPosition, drawRect, Color.White, straightRailRotation, origin, 1f, SpriteEffects.None, 0.0f);
                        break;
                    case PlacableItem.BoosterRail:
                        batch.Draw(Textures.RailBooster, railStartPosition, drawRect, Color.White, straightRailRotation, origin, 1f, SpriteEffects.None, 0.0f);
                        break;
                    default:
                        break;
                }
            }
            else if (Cell.GetCell(mousePosition).isEmpty && !PowerConnection.isConnectingWire)
            {
                switch (selectedItem)
                {
                    case PlacableItem.StraightRail:
                        batch.Draw(Textures.RailStraight, mousePosition, null, new Color(255, 255, 255, 100), straightRotation, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    case PlacableItem.TurnRail:
                        batch.Draw(Textures.RailTurn, mousePosition, null, new Color(255, 255, 255, 100), 0f, Cell.SpriteOrigin, 1f, turnOrientationEffect, 0.1f);
                        break;
                    case PlacableItem.BoosterRail:
                        batch.Draw(Textures.RailBooster, mousePosition, null, new Color(255, 255, 255, 100), straightRotation, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    case PlacableItem.Lever:
                        batch.Draw(Textures.LeverDeactive, mousePosition, null, new Color(255, 255, 255, 100), 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    case PlacableItem.DetectorRail:
                        batch.Draw(Textures.RailDetector, mousePosition, null, new Color(255, 255, 255, 100), straightRotation, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    case PlacableItem.PushRail:
                        batch.Draw(Textures.RailPush, mousePosition, null, new Color(255, 255, 255, 100), straightRotation, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    case PlacableItem.InverterModule:
                        batch.Draw(Textures.ModuleInverter, mousePosition, null, new Color(255, 255, 255, 100), 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0.1f);
                        break;
                    default:
                        break;
                }
            }
            if (carts.Count > 0)
            {
                foreach (var cart in carts)
                {
                    cart.Draw(batch);
                }
            }
        }
    }
    public enum PlacableItem
    {
        StraightRail, TurnRail, BoosterRail, DetectorRail, PushRail, Lever, InverterModule
    };
}