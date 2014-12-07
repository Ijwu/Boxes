using Microsoft.Xna.Framework;

namespace Boxes.Extensions
{
    public static class PointEx
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}