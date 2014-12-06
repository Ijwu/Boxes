using System;
using Boxes.Collision;
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

            Initialize();
        }

        public Box(Color color, Vector2 position, Texture2D texture)
        {
            Width = 10;
            Height = 10;
            Color = color;
            Position = position;
            Texture = texture;

            Initialize();
        }

        public Box(int width, int height, Color color, Vector2 position, Texture2D texture)
        {
            Width = width;
            Height = height;
            Color = color;
            Position = position;
            Texture = texture;

            Initialize();
        }

        public void Initialize()
        {
            Collides += OnCollide;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity;
            Position += Gravity;
            Velocity -= Friction;
        }

        public void InvokeCollides(object sender, CollisionEventArgs args)
        {
            if (Collides != null)
                Collides.Invoke(sender, args);
        }

        private void OnCollide(object sender, CollisionEventArgs args)
        {
            var otherBox = args.Other.GetBoundingBox();
            if (Position.X > (otherBox.X + otherBox.Width) / 2)
            {
                Position = new Vector2(otherBox.Right, Position.Y);
            }
            else
            {
                Position = new Vector2(otherBox.Left + otherBox.Width, Position.Y);
            }

            if (Position.Y > (otherBox.Y + otherBox.Height) / 2)
            {
                Position = new Vector2(Position.X, otherBox.Bottom);
            }
            else
            {
                Position = new Vector2(Position.X, otherBox.Top + otherBox.Height);
            }
        }

        public void Push(Vector2 vel)
        {
            Velocity += vel;
        }

        public void Draw(SpriteBatch sb, GameTime time)
        {
            sb.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Dispose()
        {
            Disposing = true;
        }
    }
}