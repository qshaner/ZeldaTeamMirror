﻿using Microsoft.Xna.Framework;
using Zelda.Commands;
using Zelda.Dungeon;
using Zelda.Blocks;

namespace Zelda.Blocks
{
    internal class UpDownDoor : ICollideable, IDrawable, IActivatable
    {
        private readonly ISprite _sprite;
        public Rectangle Bounds { get; }
        private DungeonManager _dungeonManager;
        private BlockType _block;
        private readonly Vector2 _drawLocation;
        private bool _lockedOrBlocked = false;

        public UpDownDoor(DungeonManager dungeon, Point location, BlockType block)
        {
            Bounds = new Rectangle(location, new Point(32, 32));
            _drawLocation = location.ToVector2();
            _sprite = BlockTypeSprite.Sprite(block);
            _dungeonManager = dungeon;
            _block = block;
            if((_block == BlockType.BombableWallTop) || (_block == BlockType.BombableWallBottom)
                || (_block == BlockType.DoorLockedUp) || (_block == BlockType.DoorLockedDown)) 
                {
                    _lockedOrBlocked = true;
                }
        }

        public bool CollidesWith(Rectangle rect)
        {
            return Bounds.Intersects(rect);
        }

        public ICommand PlayerEffect(IPlayer player)
        {
            if(_block == BlockType.DoorUp && _lockedOrBlocked == false) 
            {
                return new Transition(_dungeonManager, Direction.Up);
            }
            if(_block == BlockType.DoorDown && _lockedOrBlocked == false) 
            {
                return new Transition(_dungeonManager, Direction.Down);
            }
            if (_block == BlockType.DoorSpecialUp1_1)
            {
                return new SceneTransition(_dungeonManager, 0, 1);
            }
            return new MoveableHalt(player);
        }

        public ICommand EnemyEffect(IEnemy enemy)
        {
            return new MoveableHalt(enemy);
        }

        public ICommand ProjectileEffect(IHaltable projectile)
        {
            return new MoveableHalt(projectile);
        }

        public void Update()
        {
            _sprite?.Update();
        }

        public void Draw()
        {
            _sprite?.Draw(_drawLocation);
        }

        public void Activate()
        {
            // NO-OP
        }
    }
}
