﻿using System;
using Microsoft.Xna.Framework;

namespace Zelda.Enemies
{
    public class Keese : EnemyAgent
    {
        private const int StunTime = 240;
        private static readonly Random Rng = new Random();
        private static readonly Point Size = new Point(16, 16);
        public override Rectangle Bounds => new Rectangle(Location, Alive ? Size : Point.Zero);
        private ISprite _sprite;
        protected override ISprite Sprite => _sprite;
        private int _stunTimer = StunTime;

        private readonly Point _origin;
        private int _movementClock;
        private int _movementPauseClock;
        private Point _playerLocation;
        private Point _nextDestination;

        public Keese(Point location)
        {
            _origin = location;
        }

        public override void Spawn()
        {
            base.Spawn();

            _sprite = EnemySpriteFactory.Instance.CreateKeese();
            _movementPauseClock = Rng.Next(10, 120);
            Location = _origin;
        }

        public override void Halt()
        {
            // NO-OP: Flies through walls
        }

        public override void Stun()
        {
            _stunTimer = 0;
        }

        protected override void Knockback()
        {
            // NO-OP: Insta-kill
        }

        private void ExecuteAction()
        {
            if (_movementPauseClock-- > 0)
            {
                _movementPauseClock--;
                return;
            }

            if (_movementClock > 0)
            {
                _movementClock--;
                AdvanceToDestination();
            }
            else
            {
                GenerateNextDestination();
                _movementClock = Rng.Next(20, 90);
                _movementPauseClock = Rng.Next(30, 60);
            }
            
        }

        private void AdvanceToDestination()
        {
            double xDiff = _playerLocation.X - _nextDestination.X;
            double yDiff = _playerLocation.Y - _nextDestination.Y;
            var magnitude = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            const float scaleDenominator = 21.3f;
            double xScale = _movementClock / scaleDenominator;
            double yScale = _movementClock / scaleDenominator;

            var normalizedY = yDiff / magnitude * yScale;
            var normalizedX = xDiff / magnitude * xScale;

            if (Location.X - (int)normalizedX < 0 || Location.Y - (int)normalizedY < 0)
            {
                GenerateNextDestination();
            }
            else
            {
                Location.X -= (int)normalizedX;
                Location.Y -= (int)normalizedY;
            }
        }

        public override void Target(Point playerLocation)
        {
            _playerLocation = playerLocation;
        }

        private void GenerateNextDestination()
        {
            const float locationScale = 1.0f;
            double xDiff = _playerLocation.X - Location.X;
            double yDiff = _playerLocation.Y - Location.Y;
            xDiff += Math.Sign(xDiff) * Rng.Next(32);
            yDiff += Math.Sign(yDiff) * Rng.Next(32);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression (too long for ternary)
            _nextDestination = Rng.Next(3) == 0
                ? new Point(_playerLocation.X + Rng.Next(-256,256), _playerLocation.Y + Rng.Next(-100,100))
                : new Point((int)(Location.X + xDiff * locationScale), (int)(Location.Y + yDiff * locationScale));
        }

        public override void Update()
        {
            if (_stunTimer < StunTime)
                _stunTimer++;
            else if (Alive && CanMove)
                ExecuteAction();

            base.Update();
        }
    }
}
