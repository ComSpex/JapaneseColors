using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Drill_Color {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow:Window {
		public MainWindow() {
			InitializeComponent();
			Width=400;
			Height=200;
			Viewbox box = new Viewbox();
			TextBlock tb = new TextBlock();
			tb.Text="Click me!!";
			box.Child=tb;
			this.Content=box;
		}
		bool clicked = false;
		protected override void OnMouseDown(MouseButtonEventArgs e) {
			if(clicked) {
				Close();
				return;
			}
			clicked=true;
			CreateWindows();
			Viewbox box = this.Content as Viewbox;
			TextBlock tb = box.Child as TextBlock;
			tb.Text="Thank you!!";
			base.OnMouseDown(e);
		}
		private void CreateWindows() {
			byte c, m, y, k;
			//柿色
			c=0;
			m=68;
			y=90;
			k=10;
			Color color = (new CMYK(c,m,y,k)).RGB;
			Background=new SolidColorBrush(color);
			Title="柿色";
			CreateWindow(0,43,68,10,"近衛柿");
			CreateWindow(0,52,95,0,"蜜柑色");
			CreateWindow(0,45,60,10,"杏色");
			CreateWindow(0,38,50,5,"大和柿");
			CreateWindow(Color.FromRgb(191,191,191),"Silver");
			CreateWindow(Color.FromRgb(0,128,0),"Green");
			CreateWindow(new HSL(60,100,25),"Olive");
			CreateWindow(0,100,50,"Maroon");
			CreateWindow(0,0,50,"Gray");
			CreateWindow(Colors.Teal,"Teal");
			CreateWindow(Colors.Navy,"Navy");
		}
		private void CreateWindow(Color color,string name) {
			CreateWindow<HSL>(color,name);
		}
		private void CreateWindow(HSV hSV,string name) {
			CreateWindow<HSV>(hSV.RGB,name);
		}
		private void CreateWindow(HSL hSL,string name) {
			CreateWindow<HSL>(hSL.RGB,name);
		}
		private void CreateWindow<T>(Color color,string name,bool title=false) where T:CMYK, new(){
			Window w = new Window();
			SolidColorBrush scb = new SolidColorBrush(color);
			w.Background=scb;
			w.Width=this.Width;
			w.Height=this.Height;
			T o = new T();
			o.RGB=color;
			if(title) {
				w.Title=String.Format("{0} -- {1}",name,o);
			} else {
				w.Title=String.Format("{0} -- {1}({2})",name,o,o.RGB);
			}
			w.Owner=this;
			w.Show();
		}
		private void CreateWindow(int v1,int v2,int v3,int v4,string name) {
			CreateWindow<CMYK>((new CMYK(v1,v2,v3,v4)).RGB,name,true);
		}
		private void CreateWindow(int h,int s,int v,string name) {
			CreateWindow<HSV>((new HSV(h,s,v)).RGB,name);
		}
	}
}
