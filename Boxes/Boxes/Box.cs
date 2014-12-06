using System;
using Boxes.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes
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

        private void OnCollide(object sender, CollisionEventArgs args)
        {
            if (Position.X > (args.Other.X + args.Other.Width)/2)
            {
                Position = new Vector2(args.Other.Right, Position.Y);
            }
            else
            {
                Position = new Vector2(args.Other.Left+args.Other.Width, Position.Y);
            }

            if (Position.Y > (args.Other.Y + args.Other.Height) / 2)
            {
                Position = new Vector2(Position.X, args.Other.Bottom);
            }
            else
            {
                Position = new Vector2(Position.X, args.Other.Top+args.Other.Height);
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