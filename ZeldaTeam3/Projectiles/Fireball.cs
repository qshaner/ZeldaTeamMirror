﻿using System;
using Microsoft.Xna.Framework;

namespace Zelda.Projectiles
{
    internal class Fireball : IProjectile
    {
        private const int FramesToDisappear = 140;

        private Vector2 _location;
        private readonly Vector2 _velocity;
        private readonly ISprite _sprite;

        private int _framesDelayed;

        public Rectangle Bounds => new Rectangle((int)_location.X + 4, (int)_location.Y + 4, 8, 8);
        public bool Halted { get; set; } 

        public Fireball(Point location, Vector2 velocity, bool fromAquamentus)
        {
            _location = location.ToVector2();
            _velocity = velocity;
            _framesDelayed = 0;
            _sprite = fromAquamentus ? ProjectileSpriteFactory.Instance.CreateAquamentusFireball() : ProjectileSpriteFactory.Instance.CreateOldManFireball();
            Halted = false;
        }

        public bool CollidesWith(Rectangle rectangle)
        {
            return Bounds.Intersects(rectangle);
        }

        public ICommand PlayerEffect(IPlayer player)
        {
            Halt();
            _sprite.Hide();
            return new Commands.SpawnableDamage(player);
        }

        public ICommand EnemyEffect(IEnemy enemy)
        {
            return Commands.NoOp.Instance;
        }

        public ICommand ProjectileEffect(IProjectile projectile)
        {
            return Commands.NoOp.Instance;
        }

        public void Halt() {
            Halted = true;
        }

        public void Knockback()
        {
            //no op
        }

        public void Update()
        {
            _location = Vector2.Add(_location, _velocity);
            if (_framesDelayed++ >= FramesToDisappear)
            {
                _sprite.Hide();
                Halt();
            }
            _sprite.Update();
        }

        public void Draw()
        {
            _sprite.Draw(_location);
        }
    }
}
