﻿namespace Zelda.Commands
{
    internal class SceneTransition : ICommand
    {
        private readonly IScene _scene;
        private readonly int _row;
        private readonly int _column;

        public SceneTransition(IScene scene, int row, int column)
        {
            _scene = scene;
            _row = row;
            _column = column;
        }

        public void Execute()
        {
            _scene.TransitionToRoom(_row, _column);
        }

        public override string ToString() => "Scene Transition to specific row/column";
    }
}
