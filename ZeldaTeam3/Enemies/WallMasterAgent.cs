﻿using Microsoft.Xna.Framework;

namespace Zelda.Enemies
{
    public class WallMasterAgent
    {
        private ISprite _sprite;

        private int _health;
        private int _clock;
        public bool Alive { get; private set; }
        private bool _isImmobile;
        private bool _isDying;

        public Point Location;

        public WallMasterAgent(Point location)
        {
            Location = location;
            Alive = true;
            _health = 0;
            _sprite = EnemySpriteFactory.Instance.CreateWallMaster();
            _sprite.Hide();
            _isImmobile = true;
            _isDying = false;
        }

        public void Kill()
        {
            if (!Alive)
            {
                return;
            }
            _sprite.Hide();
            _clock = 32;
            _sprite = EnemySpriteFactory.Instance.CreateDeathSparkle();
            _isDying = true;
            Alive = false;
        }

        public void UseAttack()
        {
            // NO OP
        }

        public void MoveDown()
        {
            if (_isImmobile)
            {
                Location.Y += 1;
            }
        }

        public void MoveLeft()
        {
            if (!_isImmobile)
            {
                Location.X -= 1;
            }
        }

        public void MoveRight()
        {
            if (!_isImmobile)
            {
                Location.X += 1;
            }
        }

        public void MoveUp()
        {
            if (!_isImmobile)
            {
                Location.Y -= 1;
            }
        }

        public void Spawn()
        {
            _sprite = EnemySpriteFactory.Instance.CreateSpawnExplosion();
            _isImmobile = true;
            _clock = 30;
            _health = 10;
            Alive = true;
        }

        public void TakeDamage()
        {
            if (Alive)
            {
                _health--;
                _sprite.PaletteShift();
            }

            if (_health < 1)
            {
                Kill();
            }
        }

        public void Draw()
        {
            _sprite.Draw(Location.ToVector2());
        }

        public void Update()
        {
            if (_clock > 0)
            {
                _clock--;
                if (_clock == 0)
                {
                    CheckFlags();
                }
            }
            _sprite.Update();
        }

        private void CheckFlags()
        {
            if (_isImmobile)
            {
                _sprite = EnemySpriteFactory.Instance.CreateWallMaster();
                _isImmobile = false;
            }

            if (_isDying)
            {
                _sprite = EnemySpriteFactory.Instance.CreateWallMaster();
                _sprite.Hide();
                _isDying = false;
            }
        }
    }
}