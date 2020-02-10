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
    public class InverterModule : PowerModule
    {
        public InverterModule(Cell cell, bool removable = true) : base(cell, removable)
        {
            texture = Textures.ModuleInverter;
        }

        protected override void OnConnected()
        {
            //OnInputUpdate(input.state, false);

            base.OnConnected();
        }

        public override void OnInputUpdate(bool state, bool prevState)
        {
            output.UpdateSignal(!input.state);
            base.OnInputUpdate(state, prevState);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            base.Destroy(modifyLevel);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}
