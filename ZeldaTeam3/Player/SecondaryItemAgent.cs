﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zelda.Projectiles;

namespace Zelda.Player
{
    internal class SecondaryItemAgent : IDrawable
    {
        public bool UsingSecondaryItem;

        public Items.Secondary Item;

        public List<IProjectile> Projectiles { get; set; }

        public SecondaryItemAgent()
        {
            UsingSecondaryItem = false;
            Projectiles = new List<IProjectile>();
        }

        public void UseSecondaryItem(Direction facing, Point location)
        {
            switch (facing)
            {
                case Direction.Up:
                    location.Y -= 16;
                    break;
                case Direction.Down:
                    location.Y += 16;
                    break;
                case Direction.Left:
                    location.X -= 16;
                    break;
                case Direction.Right:
                    location.X += 16;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (Item)
            {
                case Items.Secondary.Bow:
                    var arrow = new Arrow(location, facing);
                    Projectiles.Add(arrow);
                    break;
                case Items.Secondary.Boomerang:
                    location.X += 4;
                    location.Y += 4;
                    var playerBoomerang = new PlayerBoomerang(location, facing);
                    Projectiles.Add(playerBoomerang);
                    break;
                case Items.Secondary.Bomb:
                    var bomb = new Bomb(location);
                    Projectiles.Add(bomb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UsingSecondaryItem = true;
        }

        public void AssignSecondaryItem(Items.Secondary item)
        {
            Item = item;
        }

        public void Update()
        {
            UsingSecondaryItem = false;
        }

        public void Draw()
        {
        }
    }
}
