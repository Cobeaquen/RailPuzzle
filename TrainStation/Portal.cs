using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    public class Portal : CellItem
    {
        public static List<Portal> portals = new List<Portal>();
        [ProtoMember(1)]
        public Portal connectedPortal;

        public int index;

        public Portal(Cell cell, int index) : base(cell)
        {
            this.index = index;
            texture = Textures.Portal;
            origin = SpriteOrigin;

            foreach (var portal in portals)
            {
                if (portal.index == index) // then connect the portals
                {
                    ConnectPortals(this, portal);
                }
            }

            portals.Add(this);
        }

        public Portal()
        {

        }

        public override void OnCartPass(Cart cart)
        {
            cart.Teleport(connectedPortal);
            base.OnCartPass(cart);
        }

        public void ConnectPortal(Portal portal)
        {
            connectedPortal = portal;
            portal.connectedPortal = this;
        }

        public static void ConnectPortals(Portal portal1, Portal portal2)
        {
            Portal temp1 = (Portal)portal1.MemberwiseClone();
            Portal temp2 = (Portal)portal2.MemberwiseClone();
            portal1.connectedPortal = temp2;
            portal2.connectedPortal = temp1;
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}