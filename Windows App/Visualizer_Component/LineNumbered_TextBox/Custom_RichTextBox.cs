using System;
using System.Drawing;
using System.Windows.Forms;

public class LineNumberedRichTextBox : UserControl
{
    public RichTextBox Editor { get; } = new RichTextBox();
    private readonly GutterPanel gutter = new GutterPanel();
    private int currentLine = 0;

    // Options
    public Color GutterBackColor { get => gutter.BackColor; set { gutter.BackColor = value; gutter.Invalidate(); } }
    public Color GutterForeColor { get; set; } = Color.DimGray;
    public Color CurrentLineNumberBackColor { get; set; } = Color.FromArgb(18, 0, 0, 0); // subtle
    public Padding GutterPadding { get; set; } = new Padding(0, 2, 6, 2);

    public LineNumberedRichTextBox()
    {
        // Layout
        gutter.Dock = DockStyle.Left;
        gutter.Width = 40;
        gutter.BackColor = Color.FromArgb(245, 245, 245);
        gutter.Paint += Gutter_Paint;

        Editor.BackColor = Color.FromArgb(120,120,120);
        Editor.BorderStyle = BorderStyle.None;
        Editor.Dock = DockStyle.Fill;
        Editor.Multiline = true;
        Editor.WordWrap = false; // usually desired for code/editors
        Editor.ScrollBars = RichTextBoxScrollBars.ForcedBoth;

        // Sync events
        Editor.VScroll += (_, __) => gutter.Invalidate();
        Editor.HScroll += (_, __) => gutter.Invalidate();
        Editor.TextChanged += (_, __) => { UpdateCurrentLine(); gutter.Invalidate(); ResizeGutterIfNeeded(); };
        Editor.FontChanged += (_, __) => { gutter.Invalidate(); ResizeGutterIfNeeded(); };
        Editor.Resize += (_, __) => gutter.Invalidate();
        Editor.SelectionChanged += (_, __) => { UpdateCurrentLine(); gutter.Invalidate(); };

        // Intercept mouse wheel to refresh numbers during kinetic scroll
        Editor.MouseWheel += (_, __) => gutter.Invalidate();
        // When the control is shown, jump cursor to end and focus
        this.Load += (s, e) => BeginEditing();

        Controls.Add(Editor);
        Controls.Add(gutter);

        // A sensible default font for aligned numbers
        Editor.Font = new Font("Consolas", 10f);
    }

    public void BeginEditing()
    {
        if (Editor.CanFocus) Editor.Focus();
        Editor.SelectionStart = Editor.TextLength;  // caret at end
        Editor.ScrollToCaret();
    }

    private void UpdateCurrentLine()
    {
        currentLine = Editor.GetLineFromCharIndex(Editor.SelectionStart);
    }

    private void ResizeGutterIfNeeded()
    {
        int lineCount = Math.Max(1, Editor.Lines.Length);
        int digits = Math.Max(2, lineCount.ToString().Length);
        Size sz = TextRenderer.MeasureText(new string('9', digits), Editor.Font);
        int needed = GutterPadding.Left + sz.Width + GutterPadding.Right;
        if (needed != gutter.Width)
        {
            gutter.Width = needed;
            gutter.Invalidate();
        }
    }

    private void Gutter_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(gutter.BackColor);

        if (Editor.TextLength == 0)
        {
            // Draw "1" on empty document for consistency
            DrawLineNumber(g, 0, 0);
            return;
        }

        // First & last visible char/line
        int firstCharIndex = Editor.GetCharIndexFromPosition(new Point(0, 0));
        int firstVisibleLine = Editor.GetLineFromCharIndex(firstCharIndex);

        // Use bottom-right slightly inside client area to get last visible char
        int lastCharIndex = Editor.GetCharIndexFromPosition(
            new Point(Editor.ClientSize.Width - 1, Editor.ClientSize.Height - 1));
        int lastVisibleLine = Editor.GetLineFromCharIndex(lastCharIndex);

        // Top offset for relative positioning
        int topY = Editor.GetPositionFromCharIndex(firstCharIndex).Y;

        // Loop visible lines and draw numbers
        for (int line = firstVisibleLine; line <= lastVisibleLine + 1; line++)
        {
            int charIndex = Editor.GetFirstCharIndexFromLine(line);
            if (charIndex < 0) break;

            Point pos = Editor.GetPositionFromCharIndex(charIndex);
            int y = pos.Y - topY; // relative to top of control

            // Optional highlight for current line number
            if (line == currentLine)
            {
                using (var hl = new SolidBrush(CurrentLineNumberBackColor))
                {
                    g.FillRectangle(hl, new Rectangle(0, y, gutter.Width, Editor.Font.Height + 2));
                }
                   
            }

            DrawLineNumber(g, line, y);
        }
    }

    private void DrawLineNumber(Graphics g, int zeroBasedLine, int y)
    {
        string text = (zeroBasedLine + 1).ToString();
        var rect = new Rectangle(0, y + GutterPadding.Top, gutter.Width - GutterPadding.Right, Editor.Font.Height);
        TextRenderer.DrawText(
            g,
            text,
            Editor.Font,
            rect,
            GutterForeColor,
            TextFormatFlags.Right | TextFormatFlags.NoPadding | TextFormatFlags.PreserveGraphicsTranslateTransform);
    }

    // Double-buffered panel to avoid flicker
    private class GutterPanel : Panel
    {
        public GutterPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }
    }
}
