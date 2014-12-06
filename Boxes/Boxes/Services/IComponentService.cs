using Microsoft.Xna.Framework;

namespace Boxes.Services
{
    public interface IComponentService
    {
        void Initialize();
        void Draw(GameTime time);
        void Update(GameTime time);
    }
}