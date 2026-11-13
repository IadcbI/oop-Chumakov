using System.Drawing.Imaging;

namespace lab2;


public partial class MainForm : Form
{
    private readonly Drawing _drawing = new();
    private readonly History _history = new();

    private string _currentTool = "RectShape";
    private Color _currentColor = Color.Red;
    private Shape? _activeShape = null;
    private bool _isDrawing = false;
    private Shape? _selectedShape = null;
    private bool _isMoving = false;
    private Point _lastMousePos;

        private Point _moveOldStart, _moveOldEnd;
    private List<Point>? _moveOldPoints;
    public MainForm()
    {
        InitializeComponent();

                this.DoubleBuffered = true;
        this.canvasPanel.Paint += canvasPanel_Paint;
        this.canvasPanel.MouseDown += canvasPanel_MouseDown;
        this.canvasPanel.MouseMove += canvasPanel_MouseMove;
        this.canvasPanel.MouseUp += canvasPanel_MouseUp;

                this.btnLine.Click += (s, e) => SetTool("LineShape");
        this.btnRect.Click += (s, e) => SetTool("RectShape");
        this.btnEllipse.Click += (s, e) => SetTool("EllipseShape");
        this.btnPencil.Click += (s, e) => SetTool("PencilShape");
        this.btnColor.Click += btnColor_Click;
        this.btnSelect.Click += (s, e) => SetTool("Select");

        this.btnUndo.Click += (s, e) => { _history.Undo(); canvasPanel.Invalidate(); };
        this.btnClear.Click += (s, e) => { _history.Do(new ClearCommand(_drawing)); canvasPanel.Invalidate(); };
        this.btnOpen.Click += btnOpen_Click;
        this.btnSave.Click += btnSave_Click;
        this.btnExport.Click += btnExport_Click;

                this.numWidth.ValueChanged += (s, e) => UpdateStatus();
        this.KeyDown += MainForm_KeyDown;

                SetTool("RectShape");
        UpdateStatus();
    }

    private void SetTool(string toolName)
    {
        _currentTool = toolName;
        UpdateStatus();

                btnLine.BackColor = toolName == "LineShape" ? Color.LightBlue : SystemColors.Control;
        btnRect.BackColor = toolName == "RectShape" ? Color.LightBlue : SystemColors.Control;
        btnEllipse.BackColor = toolName == "EllipseShape" ? Color.LightBlue : SystemColors.Control;
        btnPencil.BackColor = toolName == "PencilShape" ? Color.LightBlue : SystemColors.Control;
        btnSelect.BackColor = toolName == "Select" ? Color.LightBlue : SystemColors.Control;
        canvasPanel.Cursor = toolName == "Select" ? Cursors.Default : Cursors.Cross;
    }

    private void UpdateStatus()
    {
        string toolRus = _currentTool switch
        {
            "LineShape" => "Линия",
            "RectShape" => "Прямоугольник",
            "EllipseShape" => "Эллипс",
            "PencilShape" => "Карандаш",
            "Select" => "Выделение",
            _ => _currentTool
        };
        string colorRus = _currentColor.Name;
        lblStatus.Text = $"{toolRus} · {colorRus} · {(int)numWidth.Value} px · фигур на холсте: {_drawing.Shapes.Count} · Ctrl+Z — отменить";
    }

    private void btnColor_Click(object? sender, EventArgs e)
    {
        using var dlg = new ColorDialog { Color = _currentColor };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _currentColor = dlg.Color;
            btnColor.BackColor = _currentColor;
            UpdateStatus();
        }
    }

    private void canvasPanel_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        if (_currentTool == "Select")
        {
            _selectedShape = _drawing.FindShapeAt(e.Location);
            if (_selectedShape != null)
            {
                _isMoving = true;
                _lastMousePos = e.Location;
                _moveOldStart = _selectedShape.Start;
                _moveOldEnd = _selectedShape.End;
                _moveOldPoints = (_selectedShape as PencilShape)?.Points.ToList();
            }
            canvasPanel.Invalidate();
            return;
        }

        _activeShape = ShapeFactory.Create(_currentTool, e.Location, _currentColor, (int)numWidth.Value);
        _isDrawing = true;
    }

    private void canvasPanel_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_currentTool == "Select" && _isMoving && _selectedShape != null)
        {
            int dx = e.X - _lastMousePos.X;
            int dy = e.Y - _lastMousePos.Y;
            _selectedShape.Move(dx, dy);
            _lastMousePos = e.Location;
            canvasPanel.Invalidate();
            return;
        }

        if (!_isDrawing || _activeShape == null) return;

        _activeShape.MoveEnd(e.Location);
        canvasPanel.Invalidate();
    }

    private void canvasPanel_MouseUp(object? sender, MouseEventArgs e)
    {
        if (_currentTool == "Select" && _isMoving && _selectedShape != null)
        {
            _isMoving = false;
            var newStart = _selectedShape.Start;
            var newEnd = _selectedShape.End;
            var newPoints = (_selectedShape as PencilShape)?.Points.ToList();

                        if (_moveOldStart != newStart || _moveOldEnd != newEnd)
            {
                _history.Do(new MoveShapeCommand(_selectedShape, _moveOldStart, _moveOldEnd, newStart, newEnd, _moveOldPoints, newPoints));
            }
            return;
        }

        if (!_isDrawing || _activeShape == null) return;

        _isDrawing = false;
        _history.Do(new AddShapeCommand(_drawing, _activeShape));
        _activeShape = null;

        canvasPanel.Invalidate();
        UpdateStatus();
    }

    private void canvasPanel_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        _drawing.Draw(e.Graphics);
        _activeShape?.Draw(e.Graphics);

                if (_currentTool == "Select" && _selectedShape != null)
        {
            _selectedShape.DrawSelection(e.Graphics);
        }
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Z)
        {
            _history.Undo();
            canvasPanel.Invalidate();
            UpdateStatus();
        }
        else if (e.Control && e.KeyCode == Keys.Y)
        {
            _history.Redo();
            canvasPanel.Invalidate();
            UpdateStatus();
        }
        else if (e.KeyCode == Keys.Delete)
        {
                        if (_selectedShape != null)
            {
                _history.Do(new RemoveShapeCommand(_drawing, _selectedShape));
                _selectedShape = null;
                canvasPanel.Invalidate();
                UpdateStatus();
            }
        }
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "рисунок.json" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _drawing.Save(dlg.FileName);
        }
    }

    private void btnOpen_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = "JSON files (*.json)|*.json" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _drawing.Load(dlg.FileName);
            _history.Clear();             _selectedShape = null;
            canvasPanel.Invalidate();
            UpdateStatus();
        }
    }

    private void btnExport_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "PNG Image (*.png)|*.png", FileName = "рисунок.png" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            using var bmp = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _drawing.Draw(g);
            bmp.Save(dlg.FileName, ImageFormat.Png);
        }
    }
        public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            this.DoubleBuffered = true;             this.ResizeRedraw = true;
        }
    }
}