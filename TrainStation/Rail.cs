using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    [ProtoInclude(506, typeof(StraightRail))]
    [ProtoInclude(507, typeof(TurnRail))]
    public class Rail : Item
    {
        public static List<Rail> rails = new List<Rail>();

        public bool isCartApproaching = false;

        public bool isFinishRail = false;

        protected Vector2 origin;

        protected Color color = Color.White;

        private bool debugCartOver = false;

        public Rail(Cell cell, bool removable = true) : base(cell, removable)
        {
            if (!removable)
                isFinishRail = cell.ComparePosition(Globals.level.destroyCartCell);
            origin = Cell.SpriteOrigin;
        }
        public Rail()
        {

        }

        public static void LoadContent()
        {

        }

        public virtual Rail Clone()
        {
            return (Rail)MemberwiseClone();
        }

        public virtual void CartStartApproaching()
        {
            isCartApproaching = true;

            color = debugCartOver ? Color.OrangeRed : Color.White;
        }
        public virtual void CartStopApproaching()
        {
            isCartApproaching = false;

            color = Color.White;
        }

        public virtual void CartPassOver(Cart cart)
        {
            if (isFinishRail && Globals.player.carts.Count == 0 && Globals.level.pickupItems.Count == 0) // this should vary based on where the endposition is
            {
                Globals.player.LevelFinished(); // might need an orb class to seperate them - or make another list.
            }

            if (cell.cellItem != null)
                cell.cellItem.OnCartPass(cart);
            if (cell.pickupItem != null)
                cell.pickupItem.Pickup();

            //CartStartApproaching();

            /*if (bump != null)
            {
                cart.velocity = MathHelper.Clamp(cart.velocity - cart.startVel * bump.slowness, 0, cart.startVel);
            }
            if (portal != null)
            {
                if (Globals.debugMessages)
                    Console.WriteLine("ENTERING PORTAL");
                cart.Teleport(portal.connectedPortal);
            }
            if (orb != null)
            {
                orb.Pickup();
            }*/
        }

        public override void Destroy(bool modifyLevel = false)
        {
            //CartStopApproaching();
            base.Destroy(modifyLevel);
        }

        public override void Draw(SpriteBatch batch)
        {

        }
        public static void DrawRails(SpriteBatch batch)
        {
            foreach (var rail in rails)
            {
                rail.Draw(batch);
            }
        }

        public enum RailType : byte
        {
            Straight, Turn, Boost
        };
    }
}