using System;
using System.Timers;
using Boxes.Entity;
using Boxes.Extensions;
using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Objective
{
    public class ObjectiveArea : IEntity
    {
        public bool Disposing { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Gravity { get; set; }

        public Vector2 Friction { get; set; }

        public event ObjectiveAreaElapsed Elapsed;

        private Texture2D _texture;

        public int Width;

        public int Height;

        public double TimeToLive = 2000;

        private Timer _timer = new Timer();
        private DateTime _startTime;
        private Color _innerColor;
        private Color _outerColor;
        private Rectangle _innerRect;

        public ObjectiveArea(int width, int height, Vector2 position, Texture2D texture)
        {
            Width = width;
            Height = height;
            Position = position;
            _texture = texture;
            TimeToLive = 1000;
            _timer.Elapsed += OnTimerElapsed;
        }

        public ObjectiveArea(Rectangle area, double ttl, Texture2D texture)
        {
            Width = area.Width;
            Height = area.Height;
            Position = area.Location.ToVector2();
            TimeToLive = ttl;
            _texture = texture;
            _timer.Elapsed += OnTimerElapsed;
        }

        public void Initialize()
        {
            _outerColor = Color.LightGray;
            _outerColor *= .4f;
            _innerColor = Color.GhostWhite;
            _innerColor *= .4f;
            _startTime = DateTime.Now;
            _timer.Interval = Math.Max(TimeToLive, 750);
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (Elapsed != null)
                Elapsed(this, new ObjectiveAreaElapsedArgs());
            _timer.Interval = TimeToLive;
            _startTime = DateTime.Now;
        }

        public void Update(GameTime time)
        {
            var mainRect = GetBoundingBox();
            _innerRect = GetBoundingBox();
            var timeLeft = DateTime.Now - _startTime;
            var timeLeftRatio = timeLeft.TotalMilliseconds / _timer.Interval;
            var widthReduction = (int)(_innerRect.Width * timeLeftRatio);
            var heightReduction = (int)(_innerRect.Height*timeLeftRatio);
            int newWidth = _innerRect.Width - widthReduction;
            int newHeight = _innerRect.Height - heightReduction;
            _innerRect.Width = newWidth;
            _innerRect.Height = newHeight;
            _innerRect.X = mainRect.X + widthReduction/2;
            _innerRect.Y = mainRect.Y + heightReduction/2;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Draw(SpriteBatch sb, GameTime time)
        {
            var mainRect = GetBoundingBox();

            sb.Draw(_texture, mainRect, null, _outerColor);
            sb.Draw(_texture, _innerRect, null, _innerColor);
        }

        public void Push(Vector2 vel)
        {
            
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
            Disposing = true;
        }
    }
}