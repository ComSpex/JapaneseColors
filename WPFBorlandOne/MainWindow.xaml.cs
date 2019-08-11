using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BorlandOne {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow:Window {
		bool running = true;
		public MainWindow() {
			InitializeComponent();
		}
		protected override void OnRender(DrawingContext dc) {
			const double delta = 100.0;
			Random rand = new Random();
			for(int i = 0;i<100;i++) {
				double top = delta*rand.NextDouble();
				double left = delta*rand.NextDouble();
				Point loc = new Point(top,left);
				double wide = delta*rand.NextDouble();
				double high = delta*rand.NextDouble();
				Size size = new Size(wide,high);
				byte[] rgb = {0,0,0,0};
				rand.NextBytes(rgb);
				Brush brush = Brushes.Blue;// new SolidColorBrush(Color.FromArgb(rgb[0],rgb[1],rgb[2],rgb[3]));
				Pen pen = new Pen(Brushes.Red,1.0);
				Rect rect = new Rect(loc,size);
				dc.DrawRectangle(brush,pen,rect);
#if false
				Window win = new Window {
					Top=top,
					Left=left,
					Width=wide,
					Height=high,
					Owner=this
				};
				win.Show();
#endif
			}
			base.OnRender(dc);
		}
	}
}
