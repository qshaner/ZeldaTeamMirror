﻿using Microsoft.Xna.Framework;

namespace Zelda.Enemies
{
    public class GelAgent
    {
        private ISprite _sprite;

        private bool _isImmobile;
        private bool _isDying;
        public bool Alive { get; private set; }
        private int _clock;
        public Point Location;

        public GelAgent(Point location)
        {
            Location = location;
            _sprite = EnemySpriteFactory.Instance.CreateGel();
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
            // NO-OP: Attack has no animation
        }

        public void MoveDown()
        {
            if (!_isImmobile)
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
            Alive = true;
        }

        public void TakeDamage()
        {
            Kill();
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
                _sprite = EnemySpriteFactory.Instance.CreateGel();
                _isImmobile = false;
            }

            if (_isDying)
            {
                _sprite = EnemySpriteFactory.Instance.CreateGel();
                _sprite.Hide();
                _isDying = false;
            }
        }
    }
}