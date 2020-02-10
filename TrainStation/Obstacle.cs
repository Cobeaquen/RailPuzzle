using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    public class Obstacle : CellItem
    {
        public Obstacle(Cell cell) : base(cell)
        {
            texture = Textures.Obstacle;
            origin = SpriteOrigin;

            cell.isEmpty = false;
        }

        public Obstacle()
        {

        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}