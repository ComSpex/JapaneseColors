using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfAppone {
	public partial class AppwinOne:Window {
		bool isFirst = false;
		Point lastPos, lastLoc;
		Size lastWin=Size.Empty;
		Dictionary<object,Window> wins = new Dictionary<object,Window>();

		public JapaneseColors Jc { get => Jc1; set => Jc1=value; }
		public JapaneseColors Jc1 { get => jc; set => jc=value; }

		private void R_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			foreach(ListBoxItem item in e.AddedItems) {
				string text = XamlWriter.Save(item);
				try {
					Clipboard.SetText(text,TextDataFormat.UnicodeText);
				} catch { }
				Panel ug = item.Content as Panel;
				Viewbox vb = new Viewbox {
					Child=Clone(ug,true)
				};
				if(wins.Keys.Contains(ug.Tag)) {
					Window one = wins[ug.Tag];
					one.Focus();
					return;
				}
				double wide = item.ActualWidth*2;
				double high = item.ActualHeight*3;
				if(!lastWin.IsEmpty) {
					wide=lastWin.Width;
					high=lastWin.Height;
				}
				Window win = new Window {
					Width=wide,
					Height=high,
					Content=vb,
					Background=ug.Background,
					WindowStyle=WindowStyle.SingleBorderWindow,
					Title=(string)ug.Tag,
					//Owner=this // Here, you cannot set Owner because the Window is not shown yet.
				};
				win.SizeChanged+=Win_SizeChanged;
				win.LocationChanged+=Win_LocationChanged;
				win.Closed+=Win_Closed;
#if true
			reposition:
				Point ul = new Point(this.Left,this.Top);
				ul.X+=this.Width;
				win.Left=ul.X;
				win.Top=ul.Y;
				if(!isFirst) {
					isFirst=true;
					lastPos=ul;
					lastPos.Y+=win.Height;
				} else {
					win.Top=lastPos.Y;
					lastPos.Y+=win.Height;
					;
				}
				lastWin=new Size(win.Width,win.Height);
#endif
				WindowWhere where;
				if(WindowIsNotVisible(win,out where)) {
					switch(where) {
						case WindowWhere.Left:
							this.Left=0;
							break;
						case WindowWhere.Top:
							this.Top=0;
							break;
						case WindowWhere.Right:
							this.Left=0;
							break;
						case WindowWhere.Bottom:
							this.Top=0;
							this.Left=win.Left+win.Width;
							isFirst=false;
							goto reposition;
							//break;
					}
					//MessageBox.Show("Here comes!!");
				}
				win.Show();
				wins.Add(ug.Tag,win);
			}
		}
		private void Win_Closed(object sender,EventArgs e) {
			Window child = sender as Window;
			wins.Remove(child.Title);
			int i=wins.Count;
		}
		enum WindowWhere {
			None,
			Left,
			Right,
			Top,
			Bottom
		}
		private bool WindowIsNotVisible(Window win,out WindowWhere where) {
			Rect area = SystemParameters.WorkArea;
			where=WindowWhere.None;
			if(win.Left<area.Left) { where=WindowWhere.Left; return true; }
			if(win.Left+win.Width>area.Left+area.Width) { where=WindowWhere.Right; return true; }
			if(win.Top<area.Top) { where=WindowWhere.Top; return true; }
			if(win.Top+win.Height>area.Top+area.Height) { where=WindowWhere.Bottom; return true; }
			return false;
		}
		private void Win_LocationChanged(object sender,EventArgs e) {
			Window win = sender as Window;
			lastLoc=new Point(win.Left,win.Top);
		}
		private void Win_SizeChanged(object sender,SizeChangedEventArgs e) {
			Window child = sender as Window;
			lastWin=e.NewSize;
			foreach(Window win in wins.Values) {
				win.Width=lastWin.Width;
				win.Height=lastWin.Height;
			}
			lastWin=new Size(child.ActualWidth,child.ActualHeight);
		}
		private UIElement Clone(Panel ug,bool tooltip = false) {
			string key = (ug.Children[0] as TextBlock).Text;
			string yom = (ug.Children[1] as TextBlock).Text;
			UIElement elem = plateOf(key,yom,ug.Background) as UIElement;
			if(tooltip) {
				Panel ugg = elem as Panel;
				NamedSolidColorBrush nscb = new NamedSolidColorBrush(key,ug.Background as SolidColorBrush,yom);
				ugg.ToolTip=swatchOf(key,nscb);
			}
			return elem;
		}
	}
}
