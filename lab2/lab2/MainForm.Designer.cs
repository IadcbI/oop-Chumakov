
namespace lab2;


partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private BufferedPanel canvasPanel;
    private Button btnLine;
    private Button btnRect;
    private Button btnEllipse;
    private Button btnPencil;
    private Button btnColor;
    private Button btnUndo;
    private Button btnClear;
    private Button btnOpen;
    private Button btnSave;
    private Button btnExport;
    private Button btnSelect; 
    private NumericUpDown numWidth;
    private Label lblThickness;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        canvasPanel = new BufferedPanel();
        btnLine = new Button();
        btnRect = new Button();
        btnEllipse = new Button();
        btnPencil = new Button();
        btnColor = new Button();
        btnUndo = new Button();
        btnClear = new Button();
        btnOpen = new Button();
        btnSave = new Button();
        btnExport = new Button();
        numWidth = new NumericUpDown();
        lblThickness = new Label();
        statusStrip = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        btnSelect = new Button();
        ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
        statusStrip.SuspendLayout();
        SuspendLayout();
                                canvasPanel.BackColor = Color.White;
        canvasPanel.BorderStyle = BorderStyle.FixedSingle;
        canvasPanel.Location = new Point(12, 90);
        canvasPanel.Name = "canvasPanel";
        canvasPanel.Size = new Size(760, 450);
        canvasPanel.TabIndex = 12;
                                btnLine.Location = new Point(120, 11);
        btnLine.Name = "btnLine";
        btnLine.Size = new Size(80, 32);
        btnLine.TabIndex = 0;
        btnLine.Text = "Линия";
                                btnRect.Location = new Point(207, 11);
        btnRect.Name = "btnRect";
        btnRect.Size = new Size(130, 32);
        btnRect.TabIndex = 1;
        btnRect.Text = "Прямоугольник";
                                btnEllipse.Location = new Point(352, 11);
        btnEllipse.Name = "btnEllipse";
        btnEllipse.Size = new Size(80, 32);
        btnEllipse.TabIndex = 2;
        btnEllipse.Text = "Эллипс";
                                btnPencil.Location = new Point(438, 11);
        btnPencil.Name = "btnPencil";
        btnPencil.Size = new Size(90, 32);
        btnPencil.TabIndex = 3;
        btnPencil.Text = "Карандаш";
                                btnColor.BackColor = Color.Red;
        btnColor.Location = new Point(545, 11);
        btnColor.Name = "btnColor";
        btnColor.Size = new Size(32, 32);
        btnColor.TabIndex = 4;
        btnColor.UseVisualStyleBackColor = false;
                                btnUndo.Location = new Point(12, 50);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(100, 30);
        btnUndo.TabIndex = 7;
        btnUndo.Text = "Отменить";
                                btnClear.Location = new Point(120, 50);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(100, 30);
        btnClear.TabIndex = 8;
        btnClear.Text = "Очистить";
                                btnOpen.Location = new Point(407, 50);
        btnOpen.Name = "btnOpen";
        btnOpen.Size = new Size(93, 30);
        btnOpen.TabIndex = 9;
        btnOpen.Text = "Открыть...";
                                btnSave.Location = new Point(510, 50);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(100, 30);
        btnSave.TabIndex = 10;
        btnSave.Text = "Сохранить...";
                                btnExport.Location = new Point(620, 50);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(100, 30);
        btnExport.TabIndex = 11;
        btnExport.Text = "Экспорт PNG";
                                numWidth.Location = new Point(670, 16);
        numWidth.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numWidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numWidth.Name = "numWidth";
        numWidth.Size = new Size(50, 27);
        numWidth.TabIndex = 6;
        numWidth.Value = new decimal(new int[] { 4, 0, 0, 0 });
                                lblThickness.AutoSize = true;
        lblThickness.Location = new Point(583, 18);
        lblThickness.Name = "lblThickness";
        lblThickness.Size = new Size(75, 20);
        lblThickness.TabIndex = 5;
        lblThickness.Text = "Толщина:";
                                statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
        statusStrip.Location = new Point(0, 546);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(784, 26);
        statusStrip.TabIndex = 13;
                                lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(112, 20);
        lblStatus.Text = "Готов к работе";
                                btnSelect.Location = new Point(12, 12);
        btnSelect.Name = "btnSelect";
        btnSelect.Size = new Size(100, 32);
        btnSelect.TabIndex = 12;
        btnSelect.Text = "Выделение";
                                ClientSize = new Size(784, 572);
        Controls.Add(btnLine);
        Controls.Add(btnRect);
        Controls.Add(btnEllipse);
        Controls.Add(btnPencil);
        Controls.Add(btnColor);
        Controls.Add(lblThickness);
        Controls.Add(numWidth);
        Controls.Add(btnUndo);
        Controls.Add(btnClear);
        Controls.Add(btnOpen);
        Controls.Add(btnSave);
        Controls.Add(btnExport);
        Controls.Add(btnSelect);
        Controls.Add(canvasPanel);
        Controls.Add(statusStrip);
        KeyPreview = true;
        Name = "MainForm";
        Text = "Paint — рисунок.json";
        ((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}