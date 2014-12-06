using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Boxes.Services
{
    public class AssetService
    {
        private Game _game;
        private ContentManager _cm;

        public AssetService(Game main)
        {
            _game = main;
            _cm = main.Content;
        }

        public T LoadContent<T>(string name)
        {
            return _cm.Load<T>(name);
        }
    }
}