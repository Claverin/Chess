﻿namespace Chess.Models
{
    public class Coordinates
    {
        public int x { get; }
        public int y { get; }
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}