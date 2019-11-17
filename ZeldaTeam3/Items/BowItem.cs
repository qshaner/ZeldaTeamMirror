﻿using Microsoft.Xna.Framework;
using Zelda.Commands;
using Zelda.SoundEffects;

namespace Zelda.Items
{
    internal class BowItem : Item
    {
        private readonly Secondary _bowLevel;

        public BowItem(Point location, Secondary bowLevel) : base(location)
        {
            _bowLevel = bowLevel;
        }

        protected override ISprite Sprite { get; } = ItemSpriteFactory.Instance.CreateBow();

        public override ICommand PlayerEffect(IPlayer player)
        {
            Used = true;
            SoundEffectManager.Instance.PlayPickupNewItem();
            switch (_bowLevel)
            {
                case Secondary.Bow:
                    return new AddSecondaryItem(player, Secondary.Bow);
                case Secondary.FireBow:
                    return new AddSecondaryItem(player, Secondary.FireBow);
                default:
                    throw new System.ArgumentOutOfRangeException("Error: Items.Secondary _bowLevel was not a type of bow");
            }
        }
    }
}
