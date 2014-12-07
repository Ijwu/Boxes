using System;
using Microsoft.Xna.Framework;

namespace Boxes.Extensions
{
    public static class Vector2Ext
    {
        public static Vector2 FromAngle(double angle)
        {
            var rads = angle*Math.PI/180;
            return new Vector2((float)Math.Cos(rads), (float)Math.Sin(rads));
        }

        public static double GetAngle(Vector2 self, Vector2 other)
        {
            var delta = new Vector2(other.X - self.X, other.Y - self.Y);
            return Math.Atan2(delta.X, delta.Y)*180/Math.PI;
        }
    }
}