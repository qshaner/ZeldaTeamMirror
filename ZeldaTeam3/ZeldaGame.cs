﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zelda.Blocks;
using Zelda.Dungeon;
using Zelda.Enemies;
using Zelda.Items;
using Zelda.Player;
using Zelda.Projectiles;

namespace Zelda
{
    public class ZeldaGame : Game
    {
        public bool Resetting { get; set; }
        public IPlayer Link { get; private set; }
        public Scene Scene { get; private set; }
        public JumpMap JumpMap { get; private set; }

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private IUpdatable[] _controllers;
        private string _controlsDescription = "";

        public ZeldaGame()
        {
            // Use 2x size of NES window
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 512, PreferredBackBufferHeight = 448
            };
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite.SpriteBatch = _spriteBatch;
            _font = Content.Load<SpriteFont>("Arial");

            EnemySpriteFactory.Instance.LoadAllTextures(Content);
            BlockSpriteFactory.Instance.LoadAllTextures(Content);
            ItemSpriteFactory.Instance.LoadAllTextures(Content);
            ProjectileSpriteFactory.Instance.LoadAllTextures(Content);
            LinkSpriteFactory.Instance.LoadAllTextures(Content);
            BackgroundSpriteFactory.Instance.LoadAllTextures(Content);

            JumpMap = new JumpMap(_spriteBatch,Content);

            Link = new Link(new Point(128, 122));

            _controllers = new IUpdatable[]{
                new ControllerKeyboard(this),
                new ControllerMouse(this)
            };

            foreach (var controller in _controllers)
            {
                _controlsDescription += controller + "\n";
            }

            /* SHOULD BE REMOVED! ONLY FOR PROOF */
            var result = Content.Load<int[][]>("Rooms/5-3");
            for (var row = 0; row < result.Length; row++)
            {
                Console.Write($"Row {row,2}: ");
                for (var col = 0; col < result[row].Length; col++)
                {
                    Console.Write($"{result[row][col],3},");
                }
                Console.WriteLine();
            }
            /* END REMOVE */

            var sceneController = new SceneController();
            var room = new Room(result, 5);
            Scene = new Scene(sceneController, room, Link);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var controller in _controllers)
            {
                controller.Update();
            }

            Link.Update();
            Scene.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(2.0f));

            Scene.Draw();
            Link.Draw();

            _spriteBatch.End();

            _spriteBatch.Begin();

            JumpMap.Draw();
          
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}