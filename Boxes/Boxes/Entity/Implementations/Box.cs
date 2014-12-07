using System;
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
            if (Velocity.X > 0)
                Velocity += new Vector2(Friction.X, 0);

            if (Velocity.Y > 0)
                Velocity += new Vector2(0, Friction.Y);
        }

        public void InvokeCollides(object sender, CollisionEventArgs args)
        {
            if (Collides != null)
                Collides.Invoke(sender, args);
        }

        private void OnCollide(object sender, CollisionEventArgs args)
        {
            var otherBox = args.Other.GetBoundingBox();
            var myBox = GetBoundingBox();

            
        }

        /*
         if not self.colliderect(rect):
            return self
        
        difference = vec2d(0,0)
        center = vec2d(self.center)
        otherCenter = vec2d(rect.center)
        difference.x += otherCenter.x - center.x
        difference.y += otherCenter.y - center.y
        difference *= -1
        
        difference.x = center.x + difference.x
        difference.y = center.y + difference.y
        
        return Rect(difference.x, difference.y, self.width, self.height)
         */

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