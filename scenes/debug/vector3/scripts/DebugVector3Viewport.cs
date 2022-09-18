using Godot;
using System;

public class LinePoint
{
    public Vector2 start;
    public Vector2 end;

    public LinePoint(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }
}

public class DebugVector3Viewport : Control
{
    public LinePoint[] points = { };
    public Color color;

    public override void _Draw()
    {
        foreach (var line in this.points)
        {
			this.DrawLine(line.start, line.end, this.color, 10f);
        }
    }
}
