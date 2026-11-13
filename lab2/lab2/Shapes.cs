using System.Drawing;
using System.Text.Json.Serialization;

namespace lab2;

public abstract class Shape
{
    public Point Start { get; set; }
    public Point End { get; set; }
    public Color Color { get; set; }
    public int Width { get; set; }

    protected Shape(Point start, Color color, int width)
    {
        Start = End = start;
        Color = color;
        Width = width;
    }

    public virtual void MoveEnd(Point p) => End = p;

    protected Rectangle Bounds() => new(
        Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y),
        Math.Abs(End.X - Start.X), Math.Abs(End.Y - Start.Y));

    public abstract void Draw(Graphics g);
    public abstract bool Contains(Point p);     public virtual void Move(int dx, int dy)
    {
        Start = new Point(Start.X + dx, Start.Y + dy);
        End = new Point(End.X + dx, End.Y + dy);
    }

    public virtual void DrawSelection(Graphics g)
    {
        var b = Bounds();
        b.Inflate(4, 4);         using var pen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        g.DrawRectangle(pen, b);
    }
}

public class LineShape : Shape
{
    public LineShape(Point start, Color color, int width) : base(start, color, width) { }

    public override void Draw(Graphics g)
    {
        using var pen = new Pen(Color, Width);
        g.DrawLine(pen, Start, End);
    }

    public override bool Contains(Point p)
    {
                var d1 = Math.Sqrt(Math.Pow(p.X - Start.X, 2) + Math.Pow(p.Y - Start.Y, 2));
        var d2 = Math.Sqrt(Math.Pow(p.X - End.X, 2) + Math.Pow(p.Y - End.Y, 2));
        var len = Math.Sqrt(Math.Pow(End.X - Start.X, 2) + Math.Pow(End.Y - Start.Y, 2));
        return Math.Abs(d1 + d2 - len) < 5 + Width;
    }
}

public class RectShape : Shape
{
    public RectShape(Point start, Color color, int width) : base(start, color, width) { }

    public override void Draw(Graphics g)
    {
        using var pen = new Pen(Color, Width);
        g.DrawRectangle(pen, Bounds());
    }

    public override bool Contains(Point p) => Bounds().Contains(p);
}

public class EllipseShape : Shape
{
    public EllipseShape(Point start, Color color, int width) : base(start, color, width) { }

    public override void Draw(Graphics g)
    {
        using var pen = new Pen(Color, Width);
        g.DrawEllipse(pen, Bounds());
    }

    public override bool Contains(Point p) => Bounds().Contains(p);
}

public class PencilShape : Shape
{
    public List<Point> Points { get; set; } = new();

    public PencilShape(Point start, Color color, int width) : base(start, color, width)
    {
        Points.Add(start);
    }
    public override void Move(int dx, int dy)
    {
        base.Move(dx, dy);
        for (int i = 0; i < Points.Count; i++)
        {
            Points[i] = new Point(Points[i].X + dx, Points[i].Y + dy);
        }
    }
    public override void MoveEnd(Point p)
    {
        base.MoveEnd(p);
        Points.Add(p);
    }

    public override void Draw(Graphics g)
    {
        if (Points.Count < 2) return;
        using var pen = new Pen(Color, Width);
        g.DrawLines(pen, Points.ToArray());
    }

    public override bool Contains(Point p)
    {
        return Points.Any(pt => Math.Abs(pt.X - p.X) < 5 + Width && Math.Abs(pt.Y - p.Y) < 5 + Width);
    }
}

public static class ShapeFactory
{
    public static readonly Dictionary<string, Func<Point, Color, int, Shape>> Kinds = new()
    {
        ["LineShape"] = (p, c, w) => new LineShape(p, c, w),
        ["RectShape"] = (p, c, w) => new RectShape(p, c, w),
        ["EllipseShape"] = (p, c, w) => new EllipseShape(p, c, w),
        ["PencilShape"] = (p, c, w) => new PencilShape(p, c, w),
    };

    public static Shape Create(string kind, Point start, Color color, int width) =>
        Kinds[kind](start, color, width);
}