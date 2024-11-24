using System.Drawing;

namespace AoCToolbox;

public class SimplePointWorldObject : IWorldObject
{
    public Point Position { get; }

    public char CharRepresentation { get; }

    public int Z => 0;

    public SimplePointWorldObject(int x, int y, char c)
    {
        this.Position = new Point(x, y);
        this.CharRepresentation = c;
    }
}