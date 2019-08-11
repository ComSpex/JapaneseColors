using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WpfAppone;

namespace BorlandOne {
	public partial class Form1:Form {
		Timer heartbeat = new Timer();
		bool running = true;
		bool specificDraw = false;
		enum WhatToDraw:int {
			None,
			OfColorCode,
			Arc,
			Rectangular,
			Bezier,
			Circle,
			Line,
			ClosedCurve,
			Text,
			Polygon,
			Bottom
		}
		bool ShiftPressed = false;
		JapaneseColors jc = new JapaneseColors();
		Graphics g;
		Point MouseXY = Point.Empty;
		public Form1() {
			InitializeComponent();
			LoadSettings();
			MakeThisTransparent(true);
			if(isScreenSaver) {
				Cursor.Hide();
			}
			TopMost=true;
			heartbeat.Interval=intervalInSeconds*1000;
			heartbeat.Tick+=Heartbeat_Tick;
			heartbeat.Start();
		}

		private void MakeThisTransparent(bool yes) {
			if(yes) {
				this.BackColor=Color.LimeGreen;
				this.TransparencyKey=Color.LimeGreen;
			} else {
				this.BackColor=SystemColors.AppWorkspace;
			}
		}

		private void Heartbeat_Tick(object sender,EventArgs e) {
			MouseEventArgs ev = new MouseEventArgs(MouseButtons.Right,1,10,10,0);
			if(!specificDraw) {
				NextArt(ev);
			}
		}
		protected override void OnLoad(EventArgs e) {
			Bounds=Screen.PrimaryScreen.Bounds;
			base.OnLoad(e);
		}
		protected override void OnClosing(CancelEventArgs e) {
			base.OnClosing(e);
			running=!running;
			SaveSettings();
			SaveConfiguration();
		}
		protected override void OnMouseDown(MouseEventArgs e) {
			OnMouseMove(e);
			base.OnMouseDown(e);
		}
		protected override void OnMouseMove(MouseEventArgs e) {
			if(isScreenSaver) {
				if(!MouseXY.IsEmpty) {
					if(MouseXY!=new Point(e.X,e.Y)) {
						Close();
					}
					if(e.Clicks>0) {
						Close();
					}
				}
				MouseXY=new Point(e.X,e.Y);
			}
			base.OnMouseMove(e);
		}
		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if(e.KeyCode==Keys.Escape) {
				Close();
			}
		}
		protected void NextArt(MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				if(ShiftPressed) {
					ShiftPressed=false;
					if(--what<=WhatToDraw.None) {
						what=WhatToDraw.Polygon;
					}
				} else {
					if(++what>=WhatToDraw.Bottom) {
						what=WhatToDraw.OfColorCode;
					}
				}
				if(g!=null) {
					g.ResetTransform();
				}
			} else {
				if(ShiftPressed) {
					if(DialogResult.Yes==MessageBox.Show("Are you going to end this program?",this.Text,MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question)) {
						Close();
					}
					ShiftPressed=false;
				}
				this.Refresh();
			}
		}
		protected override void OnMouseClick(MouseEventArgs e) {
			NextArt(e);
			base.OnMouseClick(e);
		}
		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e) {
			base.OnPreviewKeyDown(e);
			ShiftPressed=e.Shift;
		}
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			ShiftPressed=e.Shift;
		}
		protected override void OnPaint(PaintEventArgs e) {
			//DrawCaps(e);
			DoDraw(e);
		}
		protected void DoDraw(PaintEventArgs e) {
			KeyValuePair<string,NamedSolidColorBrush>[] kvps = jc.Cores.ToArray();
			g=e.Graphics;
			Rectangle bounds = Screen.GetBounds(new Point());
			Random rand = new Random();
			SmoothingMode[] smodes = new SmoothingMode[] { SmoothingMode.AntiAlias,SmoothingMode.Default,SmoothingMode.HighQuality,SmoothingMode.HighSpeed,/*SmoothingMode.Invalid,*/SmoothingMode.None };
			LineCap[] lcaps = new LineCap[] { LineCap.Triangle,LineCap.SquareAnchor,LineCap.Square,LineCap.RoundAnchor,LineCap.Round,LineCap.NoAnchor,LineCap.Flat,LineCap.DiamondAnchor,LineCap.Custom,LineCap.ArrowAnchor,LineCap.AnchorMask };
			DashCap[] dcaps = new DashCap[] { DashCap.Triangle,DashCap.Round,DashCap.Flat };
			g.SmoothingMode=smodes[rand.Next(0,smodes.Length-1)];
			FontFamily[] ffs = FontFamily.Families;
			string ss = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@#$%^&*()_-+={[}\\]|\"':;?//>.<,";
			FontStyle[] styles = new FontStyle[] { FontStyle.Bold,FontStyle.Italic,FontStyle.Regular,FontStyle.Strikeout,FontStyle.Underline };
			FontStyle[] jstyles = new FontStyle[] { FontStyle.Bold,FontStyle.Italic,FontStyle.Regular,FontStyle.Underline };
			for(;;){
				int top = rand.Next(0,bounds.Height);
				int left = rand.Next(0,bounds.Right);
				Point loc = new Point(left,top);
				int wide = rand.Next(miniWide,bounds.Width);
				int high = rand.Next(miniHigh,bounds.Height);
				Size size = new Size(wide,high);
				byte[] rgb = { 0,0,0,0 };
				rand.NextBytes(rgb);
				if(!UseAlpha) {
					rgb[0]=(byte)0xff;
				}
				KeyValuePair<string,NamedSolidColorBrush> kvp = kvps[rand.Next(0,kvps.Length-1)];
				NamedSolidColorBrush Nscb = kvp.Value;
				Color fore;
				if(UseJapaneseColor) {
					Color jcc = Nscb.GetColor();
					fore=Color.FromArgb((int)rgb[0],jcc.R,jcc.G,jcc.B);
				} else {
					fore=Color.FromArgb((int)rgb[0],(int)rgb[1],(int)rgb[2],(int)rgb[3]);
				}
				Color back = invert(fore);
				Brush brush = new SolidBrush(fore);
				Brush hsurb = new SolidBrush(back);
				Pen pen = new Pen(hsurb,Convert.ToSingle(rand.Next(ofPen.from,ofPen.upto)));
				pen.SetLineCap(lcaps[rand.Next(0,lcaps.Length-1)],lcaps[rand.Next(0,lcaps.Length-1)],dcaps[rand.Next(0,dcaps.Length-1)]);
				Rectangle rect = new Rectangle(loc,size);
				Point[] points = GeneratePoints(rand,bounds);
				try {
					switch(what) {
						case WhatToDraw.Rectangular:
							g.DrawRectangle(pen,rect);
							g.FillRectangle(brush,rect);
							if(UseJapaneseColor&&!UseAlpha) {
								DrawSet ds = new DrawSet {
									ffs = ffs,
									rand = rand,
									styles = styles,
									jstyles = jstyles,
									rect = rect,
									kvp = kvp,
									Nscb = Nscb,
									hsurb = hsurb,
									pen = pen
								};
								DrawJapaneseColorNames(ds);
							}
							break;
						case WhatToDraw.Circle:
							g.DrawEllipse(pen,rect);
							g.FillEllipse(brush,rect);
							if(UseJapaneseColor&&!UseAlpha) {
								DrawSet ds = new DrawSet {
									ffs=ffs,
									rand=rand,
									styles=styles,
									jstyles=jstyles,
									rect=rect,
									kvp=kvp,
									Nscb=Nscb,
									hsurb=hsurb,
									pen=pen
								};
								DrawJapaneseColorNames(ds);
							}
							break;
						case WhatToDraw.Text:
							float fsize = Convert.ToSingle(rand.Next(ofFont.from,ofFont.upto));
							Font font = new Font(ffs[rand.Next(0,ffs.Length-1)],fsize,styles[rand.Next(0,styles.Length-1)]);
							PointF point = new PointF(left,top);
							g.DrawString(ss.Substring(rand.Next(0,ss.Length-1),1),font,brush,point);
							g.RotateTransform(Convert.ToSingle(rand.Next(-360,360)));
							break;
						case WhatToDraw.Line:
							g.DrawLine(pen,loc,new Point(size));
							break;
						case WhatToDraw.Arc:
							float a1 = Convert.ToSingle(rand.Next(-360,360));
							float a2 = Convert.ToSingle(rand.Next(-360,360));
							g.DrawArc(pen,rect,a1,a2);
							break;
						case WhatToDraw.Bezier:
							int x1 = rand.Next(0,bounds.Right);
							int x2 = rand.Next(0,bounds.Right);
							int y1 = rand.Next(0,bounds.Height);
							int y2 = rand.Next(0,bounds.Height);
							Point p1 = loc;
							Point p2 = new Point(size);
							Point p3 = new Point(x1,y1);
							Point p4 = new Point(x2,y2);
							g.DrawBezier(pen,p1,p2,p3,p4);
							break;
						case WhatToDraw.Polygon:
							g.DrawPolygon(pen,points);
							g.FillPolygon(brush,points);
							break;
						case WhatToDraw.ClosedCurve:
							g.DrawClosedCurve(pen,points);
							g.FillClosedCurve(brush,points);
							break;
						case WhatToDraw.OfColorCode:
							float jfsize = Convert.ToSingle(rand.Next(miniSizej,UseJapaneseColor ? maxSizej : maxSize_));
							Font jfont = new Font(ffs[rand.Next(0,ffs.Length-1)],jfsize,styles[rand.Next(0,jstyles.Length-1)]);
							PointF jpoint = new PointF(left,top);
							string text = null;
							for(int i = 0;i<10;++i) {
								if(AvoidBlankCharacter(jfont)) {
									jfont=new Font(ffs[rand.Next(0,ffs.Length-1)],jfsize,styles[rand.Next(0,jstyles.Length-1)]);
								} else {
									break;
								}
							}
							if(UseJapaneseColor) {
								text=String.Format("{0}({1})",kvp.Key,Nscb.Name);
							} else {
								string fore_Code = String.Format(UseAlpha ? "{0},{1},{2},{3}" : "{1},{2},{3}",fore.A,fore.R,fore.G,fore.B);
								text=fore.IsNamedColor ? fore.Name : fore_Code;
							}
							g.DrawString(text,jfont,brush,jpoint);
							g.RotateTransform(Convert.ToSingle(rand.Next(-360,360)));
							break;
#if false
						case WhatToDraw.Background:
							//This lets freeze for sure...I don't know why... as of July 17, 2019
							this.BackColor=back;
							this.ForeColor=fore;
							break;
#endif
					}
				} catch { }
				base.OnPaint(e);
				Application.DoEvents();
				if(isScreenSaver) {
					System.Threading.Thread.Sleep(0);
				}
				if(!running) {
					break;
				}
			}
		}
		public class DrawSet {
			public FontFamily[] ffs;
			public Random rand;
			public FontStyle[] styles,jstyles;
			public Rectangle rect;
			public KeyValuePair<string,NamedSolidColorBrush> kvp;
			public NamedSolidColorBrush Nscb;
			public Brush hsurb;
			public Pen pen;
		}
		private void DrawJapaneseColorNames(DrawSet ds) {
			FontFamily[] ffs = ds.ffs;
			Random rand = ds.rand;
			FontStyle[] styles=ds.styles,jstyles=ds.jstyles;
			Rectangle rect=ds.rect;
			KeyValuePair<string,NamedSolidColorBrush> kvp=ds.kvp;
			NamedSolidColorBrush Nscb=ds.Nscb;
			Brush hsurb = ds.hsurb;
			Pen pen = ds.pen;
			FontFamily ff = ffs[rand.Next(0,ffs.Length-1)];
			FontStyle fs = styles[rand.Next(0,jstyles.Length-1)];
			float jsize = Convert.ToSingle(rect.Height)/2.5f;
			Font jont = new Font(ff,jsize,fs);
			PointF joint = new PointF(rect.Left+pen.Width,rect.Top+pen.Width);
			string jtext = String.Format("{0}({1})",kvp.Key,Nscb.Name);
			switch(rand.Next(1,5)){
				case 1:
					jtext=String.Format("{0}",kvp.Key);
					break;
				case 2:
					jtext=String.Format("{0}",Nscb.Name);
					break;
				default:
					break;
			}
			for(int i = 0;i<10;++i) {
				if(AvoidBlankCharacter(jont)) {
					ff=ffs[rand.Next(0,ffs.Length-1)];
					fs=styles[rand.Next(0,jstyles.Length-1)];
					jont=new Font(ff,jsize,fs);
				} else {
					break;
				}
			}
			RectangleF ject = new RectangleF {
				X=Convert.ToSingle(rect.X),
				Y=Convert.ToSingle(rect.Y),
				Width=Convert.ToSingle(rect.Width),
				Height=Convert.ToSingle(rect.Height)
			};
			g.DrawString(jtext,jont,hsurb,ject);
		}

		private bool AvoidBlankCharacter(Font jfont) {
			string[] bads = new[] { "dings","LG","Marlett","Outlook","Specialty","MT Extra","Symbol" };
			for(int i = 0;i<bads.Length;++i) {
				if(jfont.Name.Contains(bads[i])) {
					return true;
				}
			}
			return false;
		}

		private Point[] GeneratePoints(Random rand,Rectangle bounds) {
			int upto = rand.Next(3,36);
			List<Point> ps = new List<Point>(upto);
			for(int i = 0;i<upto;++i) {
				int x = rand.Next(0,bounds.Right);
				int y = rand.Next(0,bounds.Bottom);
				ps.Add(new Point(x,y));
			}
			return ps.ToArray();
		}
		private Color invert(Color color) {
			return Color.FromArgb((int)~color.R&0xff,(int)~color.G&0xff,(int)~color.B&0xff);
		}
		////////////////////////
		protected void DrawCaps(PaintEventArgs e) {
			GraphicsPath hPath = new GraphicsPath();

			// Create the outline for our custom end cap.
			hPath.AddLine(new Point(0,0),new Point(0,5));
			hPath.AddLine(new Point(0,5),new Point(5,1));
			hPath.AddLine(new Point(5,1),new Point(3,1));

			// Construct the hook-shaped end cap.
			CustomLineCap HookCap = new CustomLineCap(null,hPath);

			// Set the start cap and end cap of the HookCap to be rounded.
			HookCap.SetStrokeCaps(LineCap.Round,LineCap.Round);

			// Create a pen and set end custom start and end
			// caps to the hook cap.
			Pen customCapPen = new Pen(Color.Black,5);
			customCapPen.CustomStartCap=HookCap;
			customCapPen.CustomEndCap=HookCap;

			// Create a second pen using the start and end caps from
			// the hook cap.
			Pen capPen = new Pen(Color.Red,10);
			LineCap startCap;
			LineCap endCap;
			HookCap.GetStrokeCaps(out startCap,out endCap);
			capPen.StartCap=startCap;
			capPen.EndCap=endCap;

			// Create a line to draw.
			Point[] points = { new Point(100, 100), new Point(200, 50),
				new Point(250, 300) };

			// Draw the lines.
			e.Graphics.DrawLines(capPen,points);
			e.Graphics.DrawLines(customCapPen,points);

		}
	}
}
