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
    public class Camera
    {
        public Vector2 position;
        public float zoom = 1f;
        private float targetZoom = 1f;

        public bool useZoom = true;

        public Matrix View
        {
            get
            {
                return Matrix.CreateTranslation(-(int)position.X,
                -(int)position.Y, 0) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(Globals.screenCenter, 0));
            }
        }

        public const float WASDspeed = 30f;

        private Vector2 previousPos = Vector2.Zero;
        private Vector2 Target = Vector2.Zero;

        public float minPositionX;
        public float minPositionY;
        public float maxPositionX;
        public float maxPositionY;

        public Camera(Vector2 startPosition, float xMin, float xMax, float yMin, float yMax)
        {
            position = startPosition;
            Target = position;

            minPositionX = xMin;
            maxPositionX = xMax;
            minPositionY = yMin;
            maxPositionY = yMax;
        }

        public void UpdateZoom(GameTime gameTime)
        {
            float dif = Input.mouseState.ScrollWheelValue - Input.prevMouseState.ScrollWheelValue;

            if (dif > 0f)
                targetZoom = 2f;
            else if (dif < 0f)
                targetZoom = 1f;

            zoom = MathHelper.Lerp(zoom, targetZoom, (float)gameTime.ElapsedGameTime.TotalSeconds * 10f);
        }

        public void MoveWASD(GameTime gameTime)
        {
            Vector2 move = Vector2.Zero;

            if (Input.keyState.IsKeyDown(Keys.D))
            {
                move.X = 1;
            }
            else if (Input.keyState.IsKeyDown(Keys.A))
            {
                move.X = -1;
            }
            if (Input.keyState.IsKeyDown(Keys.W))
            {
                move.Y = -1;
            }
            else if (Input.keyState.IsKeyDown(Keys.S))
            {
                move.Y = 1;
            }
            move *= WASDspeed;

            if (useZoom)
            {
                UpdateZoom(gameTime);
            }

            MoveTowards(position + move, gameTime);
        }

        public void MoveTowards(Vector2 target, GameTime gameTime)
        {
            if (position.X < minPositionX)
            {
                position.X = minPositionX;
            }
            else
            {
                position = Vector2.Lerp(position, target, gameTime.ElapsedGameTime.Ticks/1000000f);
            }
            HitMapWall();
            //SetDisplay(position);
        }
        public void HitMapWall()
        {
            bool hit = false;
            if (position.X < minPositionX)
            {
                hit = true;
                position.X = minPositionX;
            }
            else if (position.X > maxPositionX)
            {
                hit = true;
                position.X = maxPositionX;
            }
            if (position.Y < minPositionY)
            {
                hit = true;
                position.Y = minPositionY;
            }
            else if (position.Y > maxPositionY)
            {
                hit = true;
                position.Y = maxPositionY;
            }

        }
        public Vector2 WindowToWorldSpace(Vector2 windowPosition)
        {
            Vector2 cameraSpace = WindowToCameraSpace(windowPosition);

            return Vector2.Transform(cameraSpace / Globals.gameScale, Matrix.Invert(View));
        }

        public Vector2 WindowToCameraSpace(Vector2 windowPosition)
        {
            return windowPosition;
        }

        public Vector2 WorldToWindowSpace(Vector2 worldPosition)
        {
            return position + worldPosition;
        }

        public bool HasMoved(float distance)
        {
            if (Math.Abs(previousPos.X - position.X) > distance || Math.Abs(previousPos.Y - position.Y) > distance)
            {
                previousPos = position;
                return true;
            }
            return false;
        }

        public Vector2 WindowToWorldCoords(Vector2 position)
        {
            return position - new Vector2(View.Translation.X, View.Translation.Y);
        }
        public void SetDisplay(Vector2 position)
        {
            //view = Matrix.CreateTranslation(new Vector3(Globals.screenCenter - position, 0f));
        }
    }
}