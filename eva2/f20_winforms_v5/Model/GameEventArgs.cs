using System;

namespace BoxGame.Model
{
    public class GameEventArgs : EventArgs
    {
        public IRenderable _object { get; }

        public GameEventArgs(IRenderable r = null)
        {
            _object = r;
        }
    }
}
