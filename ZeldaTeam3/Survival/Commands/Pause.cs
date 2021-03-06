﻿using Zelda.Survival.GameState;

namespace Zelda.Survival.Commands
{
    internal class Pause : ICommand
    {
        private readonly GameStateAgent _agent;

        public Pause(GameStateAgent agent)
        {
            _agent = agent;
        }

        public void Execute()
        {
            _agent.Pause();
        }

        public override string ToString() => "Reset the game";
    }
}
