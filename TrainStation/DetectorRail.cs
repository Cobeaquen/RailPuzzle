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
    public class DetectorRail : StraightRail
    {
        public DetectorRail(Cell cell, Orientation orientation, bool removable = true) : base(cell, orientation, removable)
        {
            texture = Textures.RailDetector;

            power = new PowerConnection(cell.ToVector2(), Connection.Output, 0);
        }

        public override void CartStartApproaching()
        {
            power.UpdateSignal(true);
            base.CartStartApproaching();
        }
        public override void CartStopApproaching()
        {
            power.UpdateSignal(false);
            base.CartStopApproaching();
        }

        public override void CartPassOver(Cart cart)
        {
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
