﻿using System.Collections.Generic;
using Boxes.Entity;
using Microsoft.Xna.Framework;

namespace Boxes.Collision
{
    public class Chunk
    {
        public int X;
        public int Y;
        public int Spacing;
        public List<IEntity> Entities;
        public Rectangle Area;

        public Chunk(int x, int y, int spacing)
        {
            X = x;
            Y = y;
            Spacing = spacing;
            Area = new Rectangle(x, y, spacing, spacing);
            Entities = new List<IEntity>();
        }
    }
}