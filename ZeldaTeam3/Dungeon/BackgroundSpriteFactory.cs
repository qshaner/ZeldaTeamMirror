﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zelda.Dungeon
{
    public class BackgroundSpriteFactory
    {
        private const int Height = 176;
        private const int Width = 256;

        private Texture2D _dungeonBackground;
        private Texture2D _oldManBackground;
        private Texture2D _basementBackground;
        private Texture2D _survivalDungeonBackground;
        private Texture2D _survivalShopBackground;

        public static BackgroundSpriteFactory Instance { get; } = new BackgroundSpriteFactory();

        public void LoadAllTextures(ContentManager content)
        {
            _dungeonBackground = content.Load<Texture2D>("DungeonBackground");
            _oldManBackground = content.Load<Texture2D>("OldManBackground");
            _basementBackground = content.Load<Texture2D>("BasementBackground");
            _survivalDungeonBackground = content.Load<Texture2D>("SurvivalDungeonBackground");
            _survivalShopBackground = content.Load<Texture2D>("Shop/ShopBackground");
        }

        public ISprite CreateDungeonBackground()
        {
            return new Sprite(_dungeonBackground, Width, Height, 1, Point.Zero);
        }

        public ISprite CreateOldManBackground()
        {
            return new Sprite(_oldManBackground, Width, Height, 1, Point.Zero);
        }

        public ISprite CreateBasementBackground()
        {
            return new Sprite(_basementBackground, Width, Height, 1, Point.Zero);
        }

        public ISprite CreateSurvivalDungeonBackground()
        {
            return new Sprite(_survivalDungeonBackground, 768, 528, 1, Point.Zero);
        }

        public ISprite CreateSurvivalShopBackground()
        {
            return new Sprite(_survivalShopBackground, 896, 1008, 1, Point.Zero);
        }
    }
}
