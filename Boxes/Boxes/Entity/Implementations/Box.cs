using System;
using System.Diagnostics;
using Boxes.Collision;
using Boxes.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity.Implementations
{
    public class Box : IEntity, IDisposable
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Color Color { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Friction { get; set; }

        public Vector2 Gravity { get; set; }

        public Texture2D Texture { get; set; }

        public bool Disposing { get; set; }

        public event CollisionEvent Collides;

        public Box(Texture2D texture)
        {
            Width = 10;
            Height = 10;
            Color = Color.White;
            Texture = texture;
        }

        public Box(Color color, Vector2 position, Texture2D texture)
        {
            Width = 10;
            Height = 10;
            Color = color;
            Position = position;
            Texture = texture;
        }

        public Box(int width, int height, Color color, Vector2 position, Texture2D texture)
        {
            Width = width;
            Height = height;
            Color = color;
            Position = position;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            Position += (Velocity);

            if (Velocity.X > 0)
                Velocity -= new Vector2(Friction.X, 0);

            if (Velocity.Y > 0)
                Velocity -= new Vector2(0, Friction.Y);

            Velocity += (Gravity * (float)(gameTime.ElapsedGameTime.TotalSeconds + .5));

            if (Position.X > 1260)
            {
                Position = new Vector2(1260-Width, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
            }

            if (Position.X < 20)
            {
                Position = new Vector2(20, Position.Y);
                Velocity = new Vector2(0, Velocity.Y);
            }

            if (Position.Y > 748)
            {
                Position = new Vector2(Position.X, 748-Height);
                Velocity = new Vector2(Velocity.X, 0);
            }

            if (Position.Y < 20)
            {
                Position = new Vector2(Position.X, 20);
                Velocity = new Vector2(Velocity.X, 0);
            }
        }

        public void Draw(SpriteBatch sb, GameTime time)
        {
            sb.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Push(Vector2 vel)
        {
            Velocity += vel;
        }

        public void Dispose()
        {
            Disposing = true;
        }

        public void Initialize()
        {
            
        }
    }
}