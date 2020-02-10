using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using ProtoBuf;

namespace TrainStation
{
    [ProtoContract]
    public class Level
    {
        // exchange these lists for one array of type cell.
        /*[ProtoMember(1)]
        public List<Obstacle> obstacles;
        [ProtoMember(2)]
        public List<Bump> bumps;
        [ProtoMember(3)]
        public List<Portal> portals;
        [ProtoMember(4)]
        public List<Cell> orbs;
        */
        public List<CellItem> cellItems;
        public List<PickupItem> pickupItems;

        [ProtoMember(5)]
        public List<Rail> rails;

        [ProtoMember(6)]
        public int straightRailCount;
        [ProtoMember(7)]
        public int turnRailCount;
        [ProtoMember(8)]
        public int boostRailCount;

        [ProtoMember(9)]
        public Cell startCell;
        [ProtoMember(10)]
        public Cell finishCell;
        [ProtoMember(11)]
        public Cell spawnCartCell;
        [ProtoMember(12)]
        public Cell destroyCartCell;

        public static void LoadContent()
        {

        }

        public Level()
        {

        }

        public Level(Cell startCell, Cell finishCell, int spawnCartOffsetX, int spawnCartOffsetY, int destroyCartOffsetX, int destroyCartOffsetY, int straightRailCount, int turnRailCount, int boostRailCount, List<CellItem> cellItems, List<PickupItem> pickupItems)//List<Rail> rails, List<Obstacle> obstacles, List<Bump> bumps, List<Portal> portals, List<Cell> orbs)
        {
            this.startCell = startCell;
            this.finishCell = finishCell;
            this.spawnCartCell = Cell.GetCell(startCell, spawnCartOffsetX, spawnCartOffsetY);
            this.destroyCartCell = Cell.GetCell(finishCell, destroyCartOffsetX, destroyCartOffsetY);
            this.straightRailCount = straightRailCount;
            this.turnRailCount = turnRailCount;
            this.boostRailCount = boostRailCount;
            this.cellItems = cellItems;
            this.pickupItems = pickupItems;
            /*this.obstacles = obstacles;
            this.bumps = bumps;
            this.portals = portals;
            this.orbs = orbs;
            */
            startCell.isEmpty = false;
            finishCell.isEmpty = false;
        }
        public void SpawnMapRails()
        {
            Item.PlaceItem(new StraightRail(spawnCartCell, StraightRail.Orientation.Horizontal, false));
            Item.PlaceItem(new StraightRail(destroyCartCell, StraightRail.Orientation.Horizontal, false));
        }

        void LoadLevel()
        {
            startCell = Cell.GetCell(startCell);
            finishCell = Cell.GetCell(finishCell);
            startCell.isEmpty = false;
            finishCell.isEmpty = false;

            if (cellItems != null)
            {
                for (int i = 0; i < cellItems.Count; i++)
                {
                    cellItems[i] = new CellItem(cellItems[i]);
                }
            }

            /*if (bumps != null)
            {
                for (int i = 0; i < bumps.Count; i++)
                {
                    bumps[i] = new Bump(bumps[i].slowness, Cell.GetCell(bumps[i]));
                }
            }
            if (obstacles != null)
            {
                for (int i = 0; i < obstacles.Count; i++)
                {
                    obstacles[i] = new Obstacle(Cell.GetCell(obstacles[i]));
                }
            }
            if (portals != null)
            {
                for (int i = 1; i < portals.Count; i++)
                {
                    portals[i - 1] = new Portal(Cell.GetCell(portals[i - 1]));
                    portals[i] = new Portal(Cell.GetCell(portals[i]));

                    Portal.ConnectPortals(portals[i - 1], portals[i]);
                }
            }
            
            if (orbs != null)
            {
                for (int i = 0; i < orbs.Count; i++)
                {
                    orbs[i] = new Orb(Cell.GetCell(orbs[i]), ((Orb)orbs[i]).orientation);
                }
            }
            if (rails != null)
            {
                for (int i = 0; i < rails.Count; i++)
                {
                    if (rails[i] is BoosterRail)
                    {
                        BoosterRail rail = (BoosterRail)rails[i];
                        rails[i] = new BoosterRail(Cell.GetCell(rail), rail.orientation, rail.removable);
                    }
                    else if (rails[i] is StraightRail)
                    {
                        StraightRail rail = (StraightRail)rails[i];
                        rails[i] = new StraightRail(Cell.GetCell(rail), rail.orientation, rail.removable);
                    }
                    else if (rails[i] is TurnRail)
                    {
                        TurnRail rail = (TurnRail)rails[i];
                        rails[i] = new TurnRail(Cell.GetCell(rail), rail.turnDir, rail.removable);
                    }
                }
            }*/
        }

        public void Reset()
        {

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Textures.Start, startCell.ToVector2(), null, Color.White, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0f);
            batch.Draw(Textures.Destination, finishCell.ToVector2(), null, Color.White, 0f, Cell.SpriteOrigin, 1f, SpriteEffects.None, 0f);

            if (cellItems != null)
            {
                foreach (var item in cellItems)
                {
                    item.Draw(batch);
                }
            }
            if (pickupItems != null)
            {
                foreach (var item in pickupItems)
                {
                    item.Draw(batch);
                }
            }
            /*if (bumps != null)
            {
                foreach (var bump in bumps)
                {
                    bump.Draw(batch);
                }
            }
            if (obstacles != null)
            {
                foreach (var obstacle in obstacles)
                {
                    obstacle.Draw(batch);
                }
            }
            if (portals != null)
            {
                foreach (var portal in portals)
                {
                    portal.Draw(batch);
                }
            }
            if (orbs != null)
            {
                foreach (var orb in orbs)
                {
                    orb.Draw(batch);
                }
            }
            if (rails != null)
            {
                foreach (var rail in rails)
                {
                    rail.Draw(batch);
                }
            }*/
        }

        #region Saving and loading
        public void Save(string levelName)
        {
            using (var file = File.Create(levelName + ".lvl"))
            {
                Serializer.Serialize(file, this);
            }
        }
        public static Level Load(string levelName)
        {
            Level lvl;

            using (var file = File.OpenRead(levelName + ".lvl"))
            {
                lvl = Serializer.Deserialize<Level>(file);
            }

            lvl.LoadLevel();

            return lvl;
        }

        public static Level LoadExampleLevel()
        {
            Portal.portals.Clear();

            List<CellItem> cellItems = new List<CellItem>()
            {
                new Bump(Cell.GetCell(Cell.cellWidth * 5, Cell.cellHeight * 2), 0.2f),
                new Bump(Cell.GetCell(Cell.cellWidth * 15, Cell.cellHeight * 7), 0.2f),
                new Bump(Cell.GetCell(Cell.cellWidth * 2, Cell.cellHeight * 5), 0.2f),

                new Obstacle(Cell.GetCell(Cell.cellWidth * 9, Cell.cellHeight * 9)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 12, Cell.cellHeight * 8)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 16, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 1)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 2)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 4)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 5)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 6)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 1)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 2)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 4)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 5)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 6)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 10, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 9, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 8, Cell.cellHeight * 7)),

                new Portal(Cell.GetCell(Cell.cellWidth * 5, Cell.cellHeight * 5), 1),
                new Portal(Cell.GetCell(Cell.cellWidth * 9, Cell.cellHeight * 1), 1),

                new CartSpawner(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 9), 10)
            };

            List<PickupItem> pickupItems = new List<PickupItem>()
            {
                new Orb(Cell.GetCell(Cell.cellWidth * 4, Cell.cellHeight * 8)),
                new Orb(Cell.GetCell(Cell.cellWidth * 8, Cell.cellHeight * 4))
            };

            List<Bump> bumps = new List<Bump>()
            {
                new Bump(Cell.GetCell(Cell.cellWidth * 5, Cell.cellHeight * 2), 0.2f),
                new Bump(Cell.GetCell(Cell.cellWidth * 15, Cell.cellHeight * 7), 0.2f),
                new Bump(Cell.GetCell(Cell.cellWidth * 2, Cell.cellHeight * 5), 0.2f)
                
            };
            List<Obstacle> obstacles = new List<Obstacle>()
            {
                new Obstacle(Cell.GetCell(Cell.cellWidth * 9, Cell.cellHeight * 9)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 12, Cell.cellHeight * 8)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 16, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 1)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 2)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 4)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 5)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 6)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 7, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 1)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 2)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 3)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 4)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 5)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 6)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 11, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 10, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 9, Cell.cellHeight * 7)),
                new Obstacle(Cell.GetCell(Cell.cellWidth * 8, Cell.cellHeight * 7)),

            };

            List<Cell> orbs = new List<Cell>()
            {
                //new Orb(Cell.GetCell(Cell.cellWidth * 4, Cell.cellHeight * 8), StraightRail.Orientation.Horizontal),
                //new Orb(Cell.GetCell(Cell.cellWidth * 8, Cell.cellHeight * 4), StraightRail.Orientation.Vertical)
            };

            Cell startCell = Cell.GetCell(Cell.cellWidth * 1, Cell.cellHeight * 9);
            Cell finishCell = Cell.GetCell(Cell.cellWidth * 19, Cell.cellHeight * 3);

            return new Level(startCell, finishCell, 1, 0, -1, 0, 50, 10, 10, cellItems, pickupItems);//new List<Rail>(), obstacles, bumps, portals, orbs);
        }
        #endregion
    }
}