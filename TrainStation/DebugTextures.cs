using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TrainStation
{
    public class DebugTextures
    {
        public static GraphicsDevice graphicsDevice;
        public static Texture2D pixel;

        public static void LoadTextures(GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            pixel = GenerateRectangle(1, 1, Color.White);
        }

        public static Texture2D GenerateRectangle(int width, int height, Color color)
        {
            Texture2D tx = new Texture2D(graphicsDevice, width, height);

            Color[] clrs = new Color[width * height];

            for (int i = 0; i < width * height; i++)
            {
                clrs[i] = color;
            }
            tx.SetData(clrs);
            return tx;
        }
        public static Texture2D GenerateHollowRectangele(int width, int height, int edgeSize, Color color)
        {
            Texture2D tx = new Texture2D(graphicsDevice, width, height);

            Color[] clrs = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x >= edgeSize && x < width - edgeSize && y >= edgeSize && y < height - edgeSize)
                    {
                        clrs[y * width + x] = Color.Transparent;
                    }
                    else
                    {
                        clrs[y * width + x] = color;
                    }
                }
            }
            tx.SetData(clrs);
            return tx;
        }

        public static void DrawDebugLine(SpriteBatch batch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            batch.Draw(pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
