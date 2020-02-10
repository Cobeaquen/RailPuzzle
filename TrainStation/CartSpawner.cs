using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation
{
    public class CartSpawner : CellItem
    {
        public int spawns;

        Text text;

        public CartSpawner(Cell cell, int spawns) : base(cell)
        {
            this.spawns = spawns;
            text = new Text(ToVector2(), spawns.ToString(), cellWidth, cellHeight, 2f, GUI.defaultFont, Color.White, false, GameState.None, 1f);
            texture = Textures.CartSpawner;
            origin = SpriteOrigin;
        }
        public CartSpawner() { }

        public override void OnCartPass(Cart cart)
        {
            if (spawns < 1)
                return;
            spawns--;
            text.text = spawns.ToString();
            //text.position = ToVector2();
            Globals.player.RunSimulation();
            base.OnCartPass(cart);
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void Draw(SpriteBatch batch)
        {
            text.Draw(batch);
            base.Draw(batch);
        }
    }
}