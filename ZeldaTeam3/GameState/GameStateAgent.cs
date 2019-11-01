﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zelda.Dungeon;
using Zelda.HUD;
using Zelda.Music;
using Zelda.Player;

namespace Zelda.GameState
{
    /*
     * Handles game state transitions and screen panning
     */
    public class GameStateAgent : IUpdatable
    {
        public const float Scale = 2.0f;

        public DungeonManager DungeonManager { get; } = new DungeonManager();
        public HUDScreen HUD { get; } = new HUDScreen(new Point(0, -HUDSpriteFactory.ScreenHeight));
        public IPlayer Player { get; private set; } = new Link(Point.Zero);
        public bool Quitting { get; private set; }

        private readonly SpriteBatch _spriteBatch;
        private PauseTransitionStateMachine _pauseMachine = new PauseTransitionStateMachine();

        private WorldState _worldState = WorldState.Playing;
        private GameWorld _world;

        public GameStateAgent(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void Play()
        {
            _world = new PlayingWorld(this);
            _worldState = WorldState.Playing;
        }

        public void Resume()
        {
            if (_pauseMachine.State != PauseState.Paused) return;
            _pauseMachine.Resume();
        }

        public void Pause()
        {
            if (_worldState != WorldState.Playing || _pauseMachine.State != PauseState.Playing) return;
            _pauseMachine.Pause();
            _world = new PausedWorld(this);
            _worldState = WorldState.Playing;
        }

        public void JumpMap()
        {
            if (_worldState == WorldState.JumpMap) return;
            _world = new JumpMapWorld(this);
            _worldState = WorldState.JumpMap;
        }

        public void GameOver()
        {
            if (_worldState == WorldState.GameOver) return;
            _world = new GameOverWorld(this);
            _worldState = WorldState.GameOver;
        }

        public void GameWin()
        {
            if (_worldState == WorldState.GameWin) return;
            _world = new GameWinWorld(this);
            _worldState = WorldState.GameWin;
        }

        public void JumpToRoom(int row, int column)
        {
            DungeonManager.JumpToRoom(row, column);
            Play();
        }

        public void DungeonPan()
        {
            if (_worldState == WorldState.DungeonPanning) return;
            _world = new PanningWorld(this);
            _worldState = WorldState.DungeonPanning;
        }

        public void Continue()
        {
            DungeonManager.ResetScenes();
            Player.Spawn();
            DungeonManager.JumpToRoom(5, 2);
            Play();
        }

        public void Reset()
        {
            if (_worldState == WorldState.Reset) return;
            _worldState = WorldState.Reset;

            Player = new Link(Point.Zero);
            _pauseMachine = new PauseTransitionStateMachine();
            MusicManager.Instance.StopMusic();
            DungeonManager.LoadScenes(Player);
            Continue();
        }

        public void Quit()
        {
            Quitting = true;
        }

        public void Update()
        {
            _pauseMachine.Update();

            if (_world == null) return;

            foreach (var updatable in _world.Updatables)
            {
                updatable.Update();
            }

            if (!Player.Alive) GameOver();

            if (_pauseMachine.State == PauseState.Unpaused)
            {
                _pauseMachine.Play();
                Play();
            }
        }

        public void Draw()
        {
            if (_world == null) return;

            var yOffset = HUDSpriteFactory.ScreenHeight + _pauseMachine.YOffset;
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(Scale) * Matrix.CreateTranslation(0.0f, yOffset * Scale, 0.0f));
            foreach (var drawable in _world.ScaledDrawables)
            {
                drawable.Draw();
            }
            _spriteBatch.End();

            _spriteBatch.Begin();
            foreach (var drawable in _world.UnscaledDrawables)
            {
                drawable.Draw();
            }
            _spriteBatch.End();
        }
    }
}