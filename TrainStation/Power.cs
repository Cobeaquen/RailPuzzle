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
    public static class Power
    {
        public static List<Wire> wires = new List<Wire>();
        public static List<PowerModule> powerModules = new List<PowerModule>();

        public static void UpdatePower(GameTime gameTime)
        {
            foreach (var pc in PowerConnection.powerConnections)
            {
                pc.Update(gameTime);
            }
        }

        public static void DrawPower(SpriteBatch batch)
        {
            foreach (var w in wires)
            {
                w.Draw(batch);
            }
            foreach (var pm in powerModules)
            {
                pm.Draw(batch);
            }
            foreach (var pc in PowerConnection.powerConnections)
            {
                pc.Draw(batch);
            }
        }
    }
}
