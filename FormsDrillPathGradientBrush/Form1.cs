using ColorPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsDrillPathGradientBrush {
  public partial class Form1 : Form {
    public Form1() {
      InitializeComponent();
      RecalcWheelPoints();
    }
    protected override void OnPaint(PaintEventArgs e) {
      base.OnPaint(e);
      //FillEllipseWithPathGradient(e);
      DoOnPaint(e);
    }
    /// <summary>
    /// Copied from ColorPicker sample
    /// </summary>
    void RecalcWheelPoints() {
      m_path.Clear();
      m_colors.Clear();

      PointF center = Center(ColorWheelRectangle);
      float radius = Radius(ColorWheelRectangle);
      double angle = 0;
      double fullcircle = 360;
      double step = 5;
      while (angle < fullcircle) {
        double angleR = angle * (Math.PI / 180);
        double x = center.X + Math.Cos(angleR) * radius;
        double y = center.Y - Math.Sin(angleR) * radius;
        m_path.Add(new PointF((float)x, (float)y));
        m_colors.Add(new HSLColor(angle, 1, m_wheelLightness).Color);
        angle += step;
      }
    }
    RectangleF WheelRectangle {
      get {
        Rectangle r = ClientRectangle;
        r.Width -= 1;
        r.Height -= 1;
        return r;
      }
    }
    RectangleF ColorWheelRectangle {
      get {
        RectangleF r = WheelRectangle;
        r.Inflate(-5, -5);
        return r;
      }
    }
    Color m_frameColor = Color.CadetBlue;
    PathGradientBrush m_brush = null;
    List<PointF> m_path = new List<PointF>();
    List<Color> m_colors = new List<Color>();
    HSLColor m_selectedColor = new HSLColor(Color.BlanchedAlmond);
    double m_wheelLightness = 0.5;
    protected void DoOnPaint(PaintEventArgs e) {
      using (SolidBrush b = new SolidBrush(BackColor)) {
        e.Graphics.FillRectangle(b, ClientRectangle);
      }
      RectangleF wheelrect = WheelRectangle;
      DrawFrame(e.Graphics, wheelrect, 6, m_frameColor);

      wheelrect = ColorWheelRectangle;
      PointF center = Center(wheelrect);
      e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
      if (m_brush == null) {
        m_brush = new PathGradientBrush(m_path.ToArray(), WrapMode.Clamp);
        m_brush.CenterPoint = center;
        m_brush.CenterColor = Color.White;
        m_brush.SurroundColors = m_colors.ToArray();
      }
      e.Graphics.FillPie(m_brush, Rect(wheelrect), 0, 360);
      //DrawColorSelector(e.Graphics);

      if (Focused) {
        RectangleF r = WheelRectangle;
        r.Inflate(-2, -2);
        ControlPaint.DrawFocusRectangle(e.Graphics, Rect(r));
      }
    }
    float Radius(RectangleF r) {
      PointF center = Center(r);
      float radius = Math.Min((r.Width / 2), (r.Height / 2));
      return radius;
    }

    public void FillEllipseWithPathGradient(PaintEventArgs e) {
      // Create a path that consists of a single ellipse.
      GraphicsPath path = new GraphicsPath();
      path.AddEllipse(0, 0, 140, 70);

      // Use the path to construct a brush.
      PathGradientBrush pthGrBrush = new PathGradientBrush(path);

      // Set the color at the center of the path to blue.
      pthGrBrush.CenterColor = Color.FromArgb(255, 0, 0, 255);

      // Set the color along the entire boundary 
      // of the path to aqua.
      Color[] colors = { Color.FromArgb(255, 0, 255, 255) };
      pthGrBrush.SurroundColors = colors;

      e.Graphics.FillEllipse(pthGrBrush, 0, 0, 140, 70);

    }
    public static void DrawFrame(Graphics dc, RectangleF r, float cornerRadius, Color color) {
      Pen pen = new Pen(color);
      if (cornerRadius <= 0) {
        dc.DrawRectangle(pen, Rect(r));
        return;
      }
      cornerRadius = (float)Math.Min(cornerRadius, Math.Floor(r.Width) - 2);
      cornerRadius = (float)Math.Min(cornerRadius, Math.Floor(r.Height) - 2);

      GraphicsPath path = new GraphicsPath();
      path.AddArc(r.X, r.Y, cornerRadius, cornerRadius, 180, 90);
      path.AddArc(r.Right - cornerRadius, r.Y, cornerRadius, cornerRadius, 270, 90);
      path.AddArc(r.Right - cornerRadius, r.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90);
      path.AddArc(r.X, r.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90);
      path.CloseAllFigures();
      dc.DrawPath(pen, path);
    }
    public static Rectangle Rect(RectangleF rf) {
      Rectangle r = new Rectangle();
      r.X = (int)rf.X;
      r.Y = (int)rf.Y;
      r.Width = (int)rf.Width;
      r.Height = (int)rf.Height;
      return r;
    }
    public static RectangleF Rect(Rectangle r) {
      RectangleF rf = new RectangleF();
      rf.X = (float)r.X;
      rf.Y = (float)r.Y;
      rf.Width = (float)r.Width;
      rf.Height = (float)r.Height;
      return rf;
    }
    public static Point Point(PointF pf) {
      return new Point((int)pf.X, (int)pf.Y);
    }
    public static PointF Center(RectangleF r) {
      PointF center = r.Location;
      center.X += r.Width / 2;
      center.Y += r.Height / 2;
      return center;
    }
  }
}
