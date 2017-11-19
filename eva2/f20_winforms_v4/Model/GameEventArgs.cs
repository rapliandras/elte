using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxGame.Model
{
    public class GameEventArgs : EventArgs
    {
        public IRenderable _object { get; }

        public GameEventArgs(IRenderable r)
        {
            _object = r;
        }
    }
}
