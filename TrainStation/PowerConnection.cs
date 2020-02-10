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
    public class PowerConnection
    {
        public static List<PowerConnection> powerConnections = new List<PowerConnection>();

        public static bool isConnectingWire;

        public bool toggler;
        public int signalDuration;

        public Connection connection;

        public List<Wire> wires;

        public bool connected;

        public bool state;

        public Vector2 position;
        private Cell cell;

        public delegate void SignalUpdate(bool state, bool prevState);
        public SignalUpdate onSignalUpdate;

        public delegate void Connected();
        public Connected onConnected;

        private Texture2D powerTexture;
        private Vector2 powerTextureOrigin;

        private bool connectingWire;

        private float time;

        public PowerConnection(Vector2 position, Connection connection, int signalDuration = 0, Connected onConnected = null, SignalUpdate onSignalUpdate = null) // a signal-duration of 0 means it will be toggled no and off
        {
            this.connection = connection;
            this.position = position;

            toggler = signalDuration == 0;

            if (!toggler)
            {
                this.signalDuration = signalDuration;
            }

            wires = new List<Wire>();

            powerTexture = DebugTextures.GenerateRectangle(4, 4, Color.DarkRed);
            powerTextureOrigin = Globals.GetTextureOrigin(powerTexture);

            this.onConnected = onConnected;

            switch (this.connection)
            {
                case Connection.Input:
                    this.onSignalUpdate = onSignalUpdate;
                    break;
                case Connection.Output:
                    break;
                default:
                    break;
            }

            powerConnections.Add(this);
        }

        public void Connect(PowerConnection connection)
        {
            Wire newWire = null;
            switch (this.connection)
            {
                case Connection.Input:
                    if (wires.Count < 1)
                        newWire = new Wire(this, connection);
                    else
                        return;
                    break;
                case Connection.Output:
                    //if (!wires.Contains(new Wire(connection, this)))
                        newWire = new Wire(connection, this);
                    break;
                default:
                    break;
            }

            if (wires.Contains(newWire))
            {
                return;
            }

            if (connection.connection == Connection.Input)
            {
                if (connection.wires.Count < 1)
                {
                    connection.wires.Add(newWire);
                }
                else
                {
                    return;
                }
            }
            else if (connection.connection == Connection.Output)
            {
                connection.wires.Add(newWire);
            }

            wires.Add(newWire);

            connection.connected = true;

            connected = true;

            onConnected?.Invoke();

            switch (this.connection)
            {
                case Connection.Input:
                    newWire.output.onConnected?.Invoke();
                    break;
                case Connection.Output:
                    newWire.input.onConnected?.Invoke();
                    break;
                default:
                    break;
            }
        }

        public void Disconnect()
        {
            if (wires.Count > 0)
            {
                foreach (var wire in wires)
                {
                    wire.Disconnect(connection);
                }
                wires.Clear();
            }
            powerConnections.Remove(this);
        }

        public void Destroy()
        {
            if (connection == Connection.Output)
            {
                foreach (var w in wires)
                {
                    w.SendSignal(false, state);
                }
            }

            Disconnect();
            isConnectingWire = false;
        }

        public void ChangeSignal()
        {
            if (connection == Connection.Output)
            {
                if (!toggler && state)
                {
                    time = 0f;
                    return;
                }

                bool prevState = state;

                state = toggler ? !state : true;

                foreach (var wire in wires)
                {
                    wire.UpdateColor(state);
                    wire.SendSignal(state, prevState);
                }
            }

            switch (connection)
            {
                case Connection.Input:
                    //onSignalChanged?.Invoke(state);
                    break;
                case Connection.Output:
                    break;
                default:
                    break;
            }
        }

        public void UpdateSignal(bool state)
        {
            if (!toggler && state && connection == Connection.Output)
            {
                time = 0f;
            }

            //if (this.state == state)
                //return;

            bool prevState = this.state;

            this.state = state;

            switch (connection)
            {
                case Connection.Input:
                    //onSignalChanged?.Invoke(state);
                    break;
                case Connection.Output:
                    foreach (var wire in wires)
                    {
                        wire.UpdateColor(state);
                        wire.SendSignal(state, prevState);
                        //wire.input.onSignalChanged?.Invoke(state);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            cell = Cell.GetCell(position);
            if (!toggler && state && connection == Connection.Output)
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (time >= signalDuration)
                {
                    UpdateSignal(false);
                    time = 0f;
                }
            }

            if (Input.mouseCell == cell && Input.mouseClicked) // the player just clicked on this object
            {
                if (Input.mouseCell.item is PowerModule mod)
                {
                    int modSelected = mod.IsOver();

                    if (modSelected == (int)connection)
                    {
                        Console.WriteLine("The player just clicked on the power thing!");
                        connectingWire = true;
                        isConnectingWire = true;
                    }
                }
                else
                {
                    Console.WriteLine("The player just clicked on the power thing!");
                    connectingWire = true;
                    isConnectingWire = true;
                }
            }

            else if (connectingWire && Input.mouseReleased) // Finish connecting the wire
            {
                connectingWire = false;
                isConnectingWire = false;

                if (Input.mouseCell.item is PowerModule powerMod)
                {
                    int modSelected = powerMod.IsOver();

                    if (powerMod.input != this && powerMod.output != this)
                    {
                        if (modSelected == 0 && !powerMod.input.connected && powerMod.input.connection != connection) // mouse is over input part
                        {
                            Connect(powerMod.input);
                            Console.WriteLine("Trying to connect the power module to the input part");
                        }
                        else if (modSelected == 1 && powerMod.output.connection != connection) // mouse is over output part
                        {
                            Connect(powerMod.output);
                            Console.WriteLine("Trying to connect the power module to the output part");
                        }
                    }
                }

                else if (Input.mouseCell.item is Item item && item != cell.item && item.power != null && !(item.power.connection == Connection.Input && item.power.connected) && item.power.connection != connection)
                {
                    Connect(item.power);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (!Globals.displayPower)
                return;

            batch.Draw(powerTexture, position, null, Color.White, 0f, powerTextureOrigin, 1f, SpriteEffects.None, 1f);

            if (connectingWire && Input.mouseCell != cell) // Connecting of the wire
            {
                DebugTextures.DrawDebugLine(batch, position, Input.mousePositionGame, Color.Red, 4);
            }
        }
    }
    public enum Connection
    {
        Input, Output
    };
}
