

namespace lab2;
using global::lab2;
using System.Drawing;
using System.Text.Json;

public class MoveShapeCommand : ICommand
{
    private readonly Shape _shape;
    private readonly Point _oldStart, _oldEnd, _newStart, _newEnd;
    private readonly List<Point>? _oldPoints, _newPoints;

    public MoveShapeCommand(Shape shape, Point oldStart, Point oldEnd, Point newStart, Point newEnd, List<Point>? oldPoints, List<Point>? newPoints)
    {
        _shape = shape;
        _oldStart = oldStart; _oldEnd = oldEnd;
        _newStart = newStart; _newEnd = newEnd;
        _oldPoints = oldPoints; _newPoints = newPoints;
    }

    public void Execute()
    {
        _shape.Start = _newStart;
        _shape.End = _newEnd;
        if (_shape is PencilShape p && _newPoints != null) p.Points = new List<Point>(_newPoints);
    }

    public void Undo()
    {
        _shape.Start = _oldStart;
        _shape.End = _oldEnd;
        if (_shape is PencilShape p && _oldPoints != null) p.Points = new List<Point>(_oldPoints);
    }
}
public class PointDto
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class ShapeDto
{
    public string Kind { get; set; } = "";
    public PointDto Start { get; set; } = new();
    public PointDto End { get; set; } = new();
    public int ColorArgb { get; set; }
    public int Width { get; set; }
    public List<PointDto>? Points { get; set; }
}
public class Drawing
{
    public List<Shape> Shapes { get; private set; } = new();

    public void Draw(Graphics g)
    {
        foreach (var shape in Shapes)
            shape.Draw(g);
    }

    public Shape? FindShapeAt(Point p) => Shapes.AsEnumerable().Reverse().FirstOrDefault(s => s.Contains(p));

    public void Add(Shape shape) => Shapes.Add(shape);
    public void Remove(Shape shape) => Shapes.Remove(shape);
    public void Clear() => Shapes.Clear();

    public void Save(string path)
    {
        try
        {
            var dtos = Shapes.Select(s => new ShapeDto
            {
                Kind = s.GetType().Name,
                Start = new PointDto { X = s.Start.X, Y = s.Start.Y },
                End = new PointDto { X = s.End.X, Y = s.End.Y },
                ColorArgb = s.Color.ToArgb(),
                Width = s.Width,
                Points = (s as PencilShape)?.Points.Select(p => new PointDto { X = p.X, Y = p.Y }).ToList()
            }).ToList();

            var json = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
        }
    }

    public void Load(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            var dtos = JsonSerializer.Deserialize<List<ShapeDto>>(json);
            if (dtos == null) return;

            Shapes.Clear();
            foreach (var dto in dtos)
            {
                                var shape = ShapeFactory.Create(
                    dto.Kind,
                    new Point(dto.Start.X, dto.Start.Y),
                    Color.FromArgb(dto.ColorArgb),
                    dto.Width);

                shape.End = new Point(dto.End.X, dto.End.Y);

                                if (shape is PencilShape pencil && dto.Points != null)
                {
                    pencil.Points = dto.Points.Select(p => new Point(p.X, p.Y)).ToList();
                }

                Shapes.Add(shape);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}\n\nВозможно, файл поврежден или имеет неверный формат.");
        }
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}

public class AddShapeCommand : ICommand
{
    private readonly Drawing _drawing;
    private readonly Shape _shape;

    public AddShapeCommand(Drawing drawing, Shape shape)
    {
        _drawing = drawing;
        _shape = shape;
    }

    public void Execute() => _drawing.Add(_shape);
    public void Undo() => _drawing.Remove(_shape);
}

public class RemoveShapeCommand : ICommand
{
    private readonly Drawing _drawing;
    private readonly Shape _shape;

    public RemoveShapeCommand(Drawing drawing, Shape shape)
    {
        _drawing = drawing;
        _shape = shape;
    }

    public void Execute() => _drawing.Remove(_shape);
    public void Undo() => _drawing.Add(_shape);
}

public class ClearCommand : ICommand
{
    private readonly Drawing _drawing;
    private List<Shape> _backup = new();

    public ClearCommand(Drawing drawing) => _drawing = drawing;

    public void Execute()
    {
        _backup = new List<Shape>(_drawing.Shapes);
        _drawing.Clear();
    }

    public void Undo()
    {
        _drawing.Clear();
        foreach (var s in _backup) _drawing.Add(s);
    }
}

public class History
{
    private readonly Stack<ICommand> _undo = new();
    private readonly Stack<ICommand> _redo = new();

    public void Do(ICommand command)
    {
        command.Execute();
        _undo.Push(command);
        _redo.Clear();
    }

    public void Undo()
    {
        if (_undo.Count == 0) return;
        var cmd = _undo.Pop();
        cmd.Undo();
        _redo.Push(cmd);
    }

    public void Redo()
    {
        if (_redo.Count == 0) return;
        var cmd = _redo.Pop();
        cmd.Execute();
        _undo.Push(cmd);
    }

    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
    }
}