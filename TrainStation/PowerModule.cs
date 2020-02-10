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
    public class PowerModule : Item
    {
        public PowerConnection input, output;

        public Vector2 inputModulePosition;
        public Vector2 outputModulePosition;

        public PowerModule(Cell cell, bool removable = true) : base(cell, removable)
        {
            inputModulePosition = cell.ToVector2() + new Vector2(0, 3 * 8);
            outputModulePosition = cell.ToVector2() - new Vector2(0, 3 * 8);

            input = new PowerConnection(inputModulePosition, Connection.Input, 0, OnInputConnected, OnInputUpdate);
            output = new PowerConnection(outputModulePosition, Connection.Output, 0, OnOutputConnected);
            
            //Power.powerModules.Add(this);
        }

        public virtual void OnInputUpdate(bool state, bool prevState)
        {
            Console.WriteLine("Power module recieved a change in the input signal");
        }

        private void OnInputConnected()
        {
            if (output.connected)
                OnConnected();
        }
        private void OnOutputConnected()
        {
            if (input.connected)
                OnConnected();
        }
        protected virtual void OnConnected()
        {
            OnInputUpdate(input.state, !input.state);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            output.Destroy();
            output = null;
            input.Destroy();
            input = null;
            base.Destroy(modifyLevel);
        }

        /// <summary>
        /// Returns the part that the mouse is over.
        /// -1 - none,
        /// 0 - input,
        /// 1 - output
        /// </summary>
        /// <returns></returns>
        public int IsOver()
        {
            if (Input.MouseOver(inputModulePosition, 16, 16))
            {
                return 0;
            }
            if (Input.MouseOver(outputModulePosition, 16, 16))
            {
                return 1;
            }
            return -1;
        }

        public override void Draw(SpriteBatch batch)
        {

            base.Draw(batch);
        }
    }
}