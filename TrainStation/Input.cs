using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TrainStation
{
    public static class Input
    {
        public static MouseState mouseState;
        public static MouseState prevMouseState;
        public static KeyboardState keyState;
        public static KeyboardState prevKeyState;

        public static Vector2 mousePosition;
        public static Vector2 mousePositionGame;
        public static Cell mouseCell;

        public static bool mouseClicked;
        public static bool mouseReleased;

        public static void LoadInput()
        {
            prevMouseState = Mouse.GetState();
            prevKeyState = Keyboard.GetState();
            mousePosition = Vector2.Zero;
        }

        public static void BeginInput()
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            mousePositionGame = Globals.player.camera.WindowToWorldSpace(mouseState.Position.ToVector2());
            mousePosition = mouseState.Position.ToVector2();
            mouseCell = Cell.GetCell(mousePositionGame);
            mouseClicked = MouseClickedDown();
            mouseReleased = MouseClickedUp();
        }

        public static void EndInput()
        {
            prevMouseState = mouseState;
            prevKeyState = keyState;
        }

        public static bool MouseOver(Vector2 position, int width, int height)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            return mousePositionGame.Y < position.Y + halfHeight && mousePositionGame.Y > position.Y - halfHeight && mousePositionGame.X < position.X + halfWidth && mousePositionGame.X > position.X - halfWidth;
        }
        public static bool MouseOverUI(Vector2 position, int width, int height)
        {
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;
            return Math.Abs(mousePosition.X - position.X) < halfWidth && Math.Abs(mousePosition.Y - position.Y) < halfHeight;
            //return mousePosition.Y < position.Y + halfHeight && mousePosition.Y > position.Y - halfHeight && mousePosition.X < position.X + halfWidth && mousePosition.X > position.X - halfWidth;
        }

        private static bool MouseClickedDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released;
        }
        private static bool MouseClickedUp()
        {
            return mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}