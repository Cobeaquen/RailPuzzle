using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;

namespace TrainStation
{
    [ProtoInclude(508, typeof(BoosterRail))]
    [ProtoContract]
    public class StraightRail : Rail
    {
        [ProtoMember(1)]
        public Orientation orientation;

        private float rotation;

        public StraightRail(Cell cell, Orientation orientation, bool removable = true) : base(cell, removable)
        {
            // tell all of the tiles that contains the rail.

            this.orientation = orientation;

            texture = Textures.RailStraight;
            origin = Cell.SpriteOrigin;

            rotation = OrientationToRotation(this.orientation);
        }

        public static float OrientationToRotation(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Vertical:
                    return 0f;
                case Orientation.Horizontal:
                    return MathHelper.PiOver2;
            }
            return 0f;
        }

        public StraightRail()
        {

        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            batch.Draw(texture, cell.ToVector2(), null, color, rotation, origin, 1f, SpriteEffects.None, 0.05f);
        }

        public override void Destroy(bool modifyLevel = false)
        {
            base.Destroy(modifyLevel);
        }

        /// <summary>
        /// Returns the amount of rails that were placed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <param name="faceDirection"></param>
        /// <returns></returns>
        public static int PlaceMultipleRails(StraightRail rail, int length, bool modifyLevel = false)
        {
            Cell cell = new Cell(rail.cell.x, rail.cell.y);

            int j = 0;

            for (int i = 0; i < length; i++)
            {
                switch (rail.orientation)
                {
                    case Orientation.Vertical:
                        j = PlaceItem(new BoosterRail(Cell.GetCell(cell), rail.orientation, rail.removable), modifyLevel) ? j + 1 : j;
                        cell.y += Cell.cellWidth;
                        break;
                    case Orientation.Horizontal:
                        j = PlaceItem(new BoosterRail(Cell.GetCell(cell), rail.orientation, rail.removable), modifyLevel) ? j + 1 : j;
                        cell.x += Cell.cellHeight;
                        break;
                }
            }
            return j;
        }

        /// <summary>
        /// Returns the amount of rails that were placed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <param name="faceDirection"></param>
        /// <returns></returns>
        public static int PlaceMultipleRails(Vector2 position, int length, Orientation faceDirection, bool removable = true, bool modifyLevel = false)
        {
            int j = 0;
            for (int i = 0; i < length; i++)
            {
                if (faceDirection == Orientation.Horizontal)
                    j = PlaceItem(new StraightRail(Cell.GetCell(position + new Vector2(i * Cell.cellWidth, 0)), faceDirection, removable), modifyLevel) ? j + 1 : j;

                else if (faceDirection == Orientation.Vertical)
                    j = PlaceItem(new StraightRail(Cell.GetCell(position + new Vector2(0, i * Cell.cellHeight)), faceDirection, removable), modifyLevel) ? j + 1 : j;
            }
            return j;
        }

        public enum Orientation
        {
            Vertical, Horizontal
        };
    }
}
