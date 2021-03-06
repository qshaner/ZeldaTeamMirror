﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zelda.Blocks;
using Zelda.Items;
using Zelda.Projectiles;

// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator (this is never helpful)
namespace Zelda.Survival
{
    public class SurvivalScene : IScene
    {
        private const int ThrottleFrameDuration = 10;
        private readonly SurvivalRoom _room;
        private readonly WaveManager _waveManager;
        private readonly IPlayer _player;
        private readonly Dictionary<IEnemy, int> _enemiesAttackThrottle = new Dictionary<IEnemy, int>();
        private readonly List<IProjectile> _projectiles = new List<IProjectile>();
        private readonly List<IItem> _items = new List<IItem>();
        private readonly Random _rnd = new Random((int) DateTime.Now.Ticks);
        private int _enemyCount = int.MinValue;

        public SurvivalScene(SurvivalRoom room, WaveManager waveManager, IPlayer player)
        {
            _room = room;
            _waveManager = waveManager;
            _player = player;
            _items.AddRange(_room.Items);
            foreach (var item in _items)
            {
                item.Reset();
            }

            foreach(var barricade in _room.Barricade)
            {
                barricade.Reset();
            }

            foreach (var roomDoor in _room.Doors.Values)
            {
                roomDoor.Reset();
            }
        }

        public void DestroyProjectiles()
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Halt();
                if (projectile is PlayerBoomerang)
                    _player.Inventory.AddSecondaryItem(Secondary.Boomerang);
                if (projectile is AlchemyCoin)
                    _player.Inventory.AddCoin();
                if (projectile is ATWBoomerang)
                    _player.Inventory.AddSecondaryItem(Secondary.ATWBoomerang);
            }
            _projectiles.Clear();
        }

        public void SpawnScene()
        {
            _projectiles.Clear();

            if (_enemyCount == int.MinValue)
            {
                _enemyCount = _waveManager.Enemies.Count;
            }

            foreach (var roomDoor in _room.Doors.Values)
            {
                roomDoor.Deactivate();
            }
            _room.TransitionReset();
        }

        private void PlayerAttackCollision(ICollideable collision, IEnemy roomEnemy)
        {
            if (!_player.Alive || _enemiesAttackThrottle.ContainsKey(roomEnemy) || !collision.CollidesWith(roomEnemy.Bounds)) return;
            collision.EnemyEffect(roomEnemy).Execute();
            _enemiesAttackThrottle[roomEnemy] = ThrottleFrameDuration;

            if (roomEnemy.Alive) return;


            AddDroppedItem(roomEnemy.Bounds.Location);
        }

        private void AddDroppedItem(Point location)
        {
            var rand = _rnd.Next(100);
            if (rand < 25) return; // No drop = 25%

            IItem item;
            rand = _rnd.Next(11);

            switch (rand)
            {
                case 0:
                case 1:
                case 7:
                    item = new Rupee(location);
                    break;
                case 2:
                case 3:
                case 8:
                    item = new DroppedHeart(location);
                    break;
                case 4:
                    item = new Rupee5(location);
                    break;
                case 5:
                case 9:
                    item = new BombItem(location);
                    break;
                case 6:
                    item = new Key(location, _room);
                    break;
                default:
                    item = new Fairy(location);
                    break;
            }

            _items.Add(item);
        }

        public void Update()
        {
            var prioritizedCoinCollisions = new List<Rectangle>();
            
            for (var i = 0; i < _projectiles.Count; i++)
            {
                _projectiles[i].Update();
                if (!_projectiles[i].Halted) continue;

                if (_projectiles[i] is SwordBeam)
                {
                    _projectiles[i] = new SwordBeamParticles(_projectiles[i].Bounds.Location);
                }
                else if (_projectiles[i] is LaunchedBomb)
                {
                    _projectiles[i] = new LaunchedBombExplosion(_projectiles[i].Bounds.Location);
                }
                else
                {
                    _projectiles.RemoveAt(i--);
                }
            }

            _projectiles.AddRange(_player.Projectiles);
            _player.Projectiles.Clear();

            foreach (var roomDrawable in _room.Drawables)
            {
                roomDrawable.Update();
            }

            foreach (var droppedItem in _items)
            {
                droppedItem.Update();
                if (droppedItem.CollidesWith(_player.BodyCollision.Bounds))
                {
                    droppedItem.PlayerEffect(_player).Execute();
                }
            }

            foreach(var barricade in _room.Barricade)
            {
                barricade.Update();
            }

            foreach (var roomEnemy in _waveManager.Enemies)
            {
                roomEnemy.Target(EnemyTargetManager.GetTargetLocation(_projectiles, _player.Location, roomEnemy.Bounds.Location));
                roomEnemy.Update();
                _projectiles.AddRange(roomEnemy.Projectiles);
                roomEnemy.Projectiles.Clear();

                foreach (var roomCollidable in _room.Collidables)
                {
                    if (!roomCollidable.CollidesWith(roomEnemy.Bounds)) continue;

                    roomCollidable.EnemyEffect(roomEnemy).Execute();
                }

                foreach(var barricadeCollidable in _room.Barricade)
                {
                    if (!barricadeCollidable.CollidesWith(roomEnemy.Bounds)) continue;

                    barricadeCollidable.EnemyEffect(roomEnemy).Execute();
                }

                PlayerAttackCollision(_player.UsingPrimaryItem ? _player.SwordCollision : _player.BodyCollision, roomEnemy);

                if (roomEnemy.Alive && roomEnemy.CollidesWith(_player.BodyCollision.Bounds))
                {
                    roomEnemy.PlayerEffect(_player).Execute();
                }

                foreach (var projectile in _projectiles)
                {
                    if (roomEnemy.CollidesWith(projectile.Bounds))
                    {
                        roomEnemy.ProjectileEffect(projectile).Execute();
                        if (projectile is AlchemyCoin)
                            prioritizedCoinCollisions.Add(roomEnemy.Bounds);
                    }

                    if (projectile is ClockCollideable)
                        projectile.EnemyEffect(roomEnemy).Execute();
                    else
                        PlayerAttackCollision(projectile, roomEnemy);
                }
            }

            foreach (var roomCollidable in _room.Collidables)
            {
                if (roomCollidable.CollidesWith(_player.BodyCollision.Bounds))
                    roomCollidable.PlayerEffect(_player).Execute();

                foreach (var projectile in _projectiles)
                {
                    if (!roomCollidable.CollidesWith(projectile.Bounds)) continue;

                    roomCollidable.ProjectileEffect(projectile).Execute();
                    if (projectile is AlchemyCoin)
                        prioritizedCoinCollisions.Insert(0, roomCollidable.Bounds);

                    foreach(var barricade in _room.Barricade)
                    {
                        if (projectile.CollidesWith(barricade.Bounds))
                        {
                            barricade.ProjectileEffect(projectile).Execute();
                        }
                    }
                }
            }

            foreach(var barricade in _room.Barricade)
            {
                if (barricade.CollidesWith(_player.BodyCollision.Bounds))
                    barricade.PlayerEffect(_player).Execute();

                foreach(var otherBarricade in _room.Barricade)
                {
                    if (barricade.CollidesWith(otherBarricade.Bounds)&& barricade is KeyBarrierCenter)
                    {
                        if (otherBarricade is KeyBarrier && barricade.Unlocked)
                        {
                            otherBarricade.Unlock();
                        }
                    }

                    if (!barricade.CollidesWith(otherBarricade.Bounds) || !(barricade is RupeeBarrierCenter)) continue;

                    if (otherBarricade is RupeeBarrier && barricade.Unlocked)
                        otherBarricade.Unlock();
                }
            }

            foreach (var projectile in _projectiles)
            {
                if (projectile is AlchemyCoin coin)
                {
                    coin.Reflect(prioritizedCoinCollisions);
                }

                if (projectile.CollidesWith(_player.BodyCollision.Bounds))
                {
                    projectile.PlayerEffect(_player).Execute();
                }

                if (_player.BodyCollision.CollidesWith(projectile.Bounds))
                {
                    _player.BodyCollision.ProjectileEffect(projectile).Execute();
                }
            }
 
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator (LINQ is slow here)
            foreach (var key in _enemiesAttackThrottle.Keys.ToList())
            {
                if (_enemiesAttackThrottle[key]-- <= 0) _enemiesAttackThrottle.Remove(key);
            }
        }

        public void Draw()
        {
            foreach (var roomDrawable in _room.Drawables)
            {
                roomDrawable.Draw();
            }

            foreach (var droppedItem in _items)
            {
                droppedItem.Draw();
            }

            foreach (var projectile in _projectiles)
            {
                projectile.Draw();
            }

            foreach (var roomEnemy in _waveManager.Enemies)
            {
                roomEnemy.Draw();
            }

            foreach (var barricade in _room.Barricade)
            {
                barricade.Draw();
            }
        }
    }
}
