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
    public class Wire
    {
        public PowerConnection input, output;

        public float Delay = 0.1f;

        private Color color;

        public Wire(PowerConnection input, PowerConnection output)
        {
            this.input = input;
            this.output = output;
            Power.wires.Add(this);

            color = Color.DarkRed;
        }

        public void UpdateColor(bool state)
        {
            color = state ? Color.Red : Color.DarkRed;
        }

        public void SendSignal(bool state, bool prevState)
        {
            input.state = state;
            input.onSignalUpdate?.Invoke(state, prevState);
        }

        public void Disconnect(Connection connection)
        {
            if (input.wires.Count == 1)
                input.connected = false;
            if (output.wires.Count == 1)
                output.connected = false;

            switch (connection)
            {
                case Connection.Input:
                    output.wires.Remove(this);
                    break;
                case Connection.Output:
                    input.wires.Remove(this);
                    break;
                default:
                    break;
            }

            Power.wires.Remove(this);
        }

        public void Draw(SpriteBatch batch)
        {
            DebugTextures.DrawDebugLine(batch, input.position, output.position, color, 3);
        }
    }
}