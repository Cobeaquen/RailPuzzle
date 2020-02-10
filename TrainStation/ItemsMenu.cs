using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrainStation.ui;

namespace TrainStation
{
    public class ItemsMenu
    {
        public int categoryCount = 3;
        public float openingSpeed = 2f;
        private ItemCategory[] categories;

        private Vector2[] expandMenuPositions;

        private Vector2 itemsOpenerPosition;
        private Vector2 itemsOpenerPositionClosed;
        private Vector2 itemsOpenerPositionOpened;

        public static int scale = 2;

        private Vector2 menuPosition;
        private float openValue;

        MenuState menuState;

        bool opened;

        public ItemsMenu()
        {
            itemsOpenerPositionClosed = new Vector2(Globals.screenWidth - Textures.ItemsMenuOpen.Width * scale / 2f, Globals.screenHeight / 2f);
            itemsOpenerPositionOpened = new Vector2(Globals.screenWidth - Textures.ItemsMenuOpen.Width * scale / 2f - Textures.ItemsMenu.Width * scale, Globals.screenHeight / 2f);

            itemsOpenerPosition = itemsOpenerPositionClosed;

            menuPosition = itemsOpenerPosition - new Vector2(0, (categoryCount - 1) * (Textures.ItemsMenuOpen.Height * scale / 2f + Textures.ItemsMenu.Height * scale / 2f));

            expandMenuPositions = new Vector2[categoryCount];
            categories = new ItemCategory[categoryCount];

            PlacableItem[] rails = new PlacableItem[5];

            rails[0] = PlacableItem.StraightRail;
            rails[1] = PlacableItem.BoosterRail;
            rails[2] = PlacableItem.PushRail;
            rails[3] = PlacableItem.TurnRail;
            rails[4] = PlacableItem.DetectorRail;

            PlacableItem[] power = new PlacableItem[]
            {
                PlacableItem.Lever, PlacableItem.InverterModule
            };

            categories[0] = new ItemCategory(Vector2.Zero, "Rails", rails);
            categories[1] = new ItemCategory(Vector2.Zero, "Power", power);
            categories[2] = new ItemCategory(Vector2.Zero, "Other", power);

            for (int i = 0; i < categoryCount; i++)
            {
                float yPos = menuPosition.Y + i * Textures.ItemsMenu.Height * scale + i * Textures.ItemsMenuExpand.Height * scale;
                categories[i].UpdatePosition(Vector2.UnitY * yPos);
                expandMenuPositions[i].Y = yPos + Textures.ItemsMenu.Height / 2f * scale + Textures.ItemsMenuExpand.Height / 2f * scale;
            }

            for (int i = 0; i < categoryCount; i++)
            {
                categories[i].Update(10000);
                expandMenuPositions[i].X = 10000;
            }
            menuPosition = itemsOpenerPosition + new Vector2(Textures.ItemsMenu.Width * scale / 2f + Textures.ItemsMenuOpen.Width * scale / 2f, 0f);
        }

        public void Update(GameTime gameTime)
        {
            Globals.player.canPlaceRail = true;

            if (Input.MouseOverUI(menuPosition - new Vector2(Textures.ItemsMenuOpen.Width * scale / 2f, 0f), Textures.ItemsMenu.Width * scale + Textures.ItemsMenuOpen.Width * scale, (Textures.ItemsMenu.Height + Textures.ItemsMenuExpand.Height) * scale * categoryCount))
            {
                Globals.player.canPlaceRail = false;
            }
            if (Input.mouseClicked && Input.MouseOverUI(itemsOpenerPosition, Textures.ItemsMenuOpen.Width * scale, Textures.ItemsMenuOpen.Height * scale))
            {
                opened = true;
                if (menuState == MenuState.Closed)
                    menuState = MenuState.Opening;
                else if (menuState == MenuState.Opened)
                    menuState = MenuState.Closing;
            }

            if (!opened)
                return;

            menuPosition = itemsOpenerPosition + new Vector2(Textures.ItemsMenu.Width * scale / 2f + Textures.ItemsMenuOpen.Width * scale / 2f, 0f);

            for (int i = 0; i < categoryCount; i++)
            {
                categories[i].Update(menuPosition.X);
                expandMenuPositions[i].X = menuPosition.X;
            }
            if (menuState == MenuState.Closing)
            {
                openValue = MathHelper.Clamp(openValue - (float)gameTime.ElapsedGameTime.TotalSeconds * openingSpeed, 0f, 1f);

                if (opened)
                    itemsOpenerPosition = Vector2.Lerp(itemsOpenerPositionClosed, itemsOpenerPositionOpened, openValue);

                menuState = openValue != 0 ? MenuState.Closing : MenuState.Closed;
                opened = itemsOpenerPosition != itemsOpenerPositionClosed;
            }
            else if (menuState == MenuState.Opening)
            {
                openValue = MathHelper.Clamp(openValue + (float)gameTime.ElapsedGameTime.TotalSeconds * openingSpeed, 0f, 1f);

                menuState = openValue != 1 ? MenuState.Opening : MenuState.Opened;

                itemsOpenerPosition = Vector2.Lerp(itemsOpenerPositionClosed, itemsOpenerPositionOpened, openValue);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Textures.ItemsMenuOpen, itemsOpenerPosition, null, Color.White, 0f, Textures.ItemsMenuOpenOrigin, scale, SpriteEffects.None, 0f);

            if (opened) // then draw the menu
            {
                for (int i = 0; i < categoryCount; i++)
                {
                    categories[i].Draw(batch);
                    batch.Draw(Textures.ItemsMenuExpand, expandMenuPositions[i], null, Color.White, 0f, Textures.ItemsMenuExpandOrigin, scale, SpriteEffects.None, 0.1f);
                }
            }
        }

        public enum MenuState
        {
            Opening, Closing, Opened, Closed
        }
    }
}