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
    public class Lever : Item
    {
        public bool state;

        public Lever(Cell cell) : base(cell)
        {
            if (!Cell.GetCell(cell).isEmpty)
                return;
            state = false;
            texture = Textures.LeverDeactive;

            power = new PowerConnection(cell.ToVector2(), Connection.Output, 0, OnConnected);
        }

        public void ToggleLever()
        {
            if (!power.connected)
                return;

            state = !state;
            texture = state ? Textures.LeverActive : Textures.LeverDeactive;
            Console.WriteLine("This lever is now {0}", state);

            power.UpdateSignal(state);
        }

        private void OnConnected()
        {
            power.UpdateSignal(state);
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.mouseCell == cell) // The cursor is held over the lever.
            {
                if (Input.mouseState.LeftButton == ButtonState.Pressed && Input.prevMouseState.LeftButton == ButtonState.Released)
                {
                    Console.WriteLine("SWITCHING LEVER");
                }
            }
        }

        public override void Destroy(bool modifyLevel = false)
        {
            power.Destroy();
            base.Destroy(modifyLevel);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, cell.ToVector2(), null, Color.White, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0f);
        }
    }
}
