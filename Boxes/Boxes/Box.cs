using System;
using Boxes.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes
{
    public class Box : IEntity
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Friction { get; set; }

        public Vector2 Gravity { get; set; }

        public Texture2D Texture { get; set; }

        public Box()
        {
        }

        public Box(int width, int height, Color color, Vector2 position, Texture2D texture)
        {
            Width = width;
            Height = height;
            Color = color;
            Position = position;
            Texture = texture;
        }

        public void Initialize()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
            Velocity -= Friction;
            Position += Gravity;

            //TODO: Collision detection here.
        }

        public void Draw(SpriteBatch sb, GameTime time)
        {
            sb.Draw(null, Position, Color);
        }
    }
}