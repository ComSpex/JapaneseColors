using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FontFamily = System.Drawing.FontFamily;
using Label = System.Windows.Forms.Label;
using WpfColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;

namespace ComSpex.JapaneseColors {
	public partial class NamedSolidColorBrush:IEquatable<NamedSolidColorBrush>, IComparable<NamedSolidColorBrush> {
		public SolidColorBrush Brush;
		public string Name { get { return Names[0]; } }
		public byte R { get { return Brush.Color.R; } }
		public byte G { get { return Brush.Color.G; } }
		public byte B { get { return Brush.Color.B; } }
		public string Yomi { get { return show(Names); } }
		public string Kanji = null;
		public string[] Names;
		public bool IsAlias;
		public NamedSolidColorBrush(string kanji,SolidColorBrush scb,string yomi) {
			Brush=scb;
			Names=yomi.Split(',');
			Kanji=kanji;
		}
		public NamedSolidColorBrush(SolidColorBrush scb,string yomi) {
			Brush=scb;
			Names=yomi.Split(',');
		}
		public NamedSolidColorBrush(WpfColor c)
			: this(new SolidColorBrush(c),"Test") {
		}
		public NamedSolidColorBrush(WpfColor c,string yomi)
			: this(new SolidColorBrush(c),yomi) {
		}
		public NamedSolidColorBrush(WpfColor c,string yomi,bool alt)
			: this(new SolidColorBrush(c),yomi) {
			IsAlias=alt;
		}
		public NamedSolidColorBrush(byte r,byte g,byte b,string yomi)
			: this(WpfColor.FromRgb(r,g,b),yomi) {
		}
		public NamedSolidColorBrush(byte r,byte g,byte b,string yomi,bool alt)
			: this(WpfColor.FromRgb(r,g,b),yomi,alt) {
		}
		protected static string show(string[] names) {
			string text = String.Empty;
			foreach(string name in names) {
				if(!String.IsNullOrEmpty(text)) {
					text+=",";
				}
				text+=name;
			}
			return text;
		}
		internal bool NameStartsWith(string head) {
			bool yes = false;
			List<string> swapped = new List<string>();
			foreach(string name in Names) {
				if(name.StartsWith(head)) {
					yes=true;
					swapped.Add(name);
				}
			}
			foreach(string name in Names) {
				if(!swapped.Contains(name)) {
					swapped.Add(name);
				}
			}
			Names=swapped.ToArray();
			return yes;
		}
		public virtual string Show(string kanji) {
			return Show(kanji,Brush,Names);
		}
		public static string Show(string kanji,SolidColorBrush brush,string[] names) {
			using(StringWriter sw = new StringWriter()) {
				sw.Write("{3} : {0:000},{1:000},{2:000}",brush.Color.R,brush.Color.G,brush.Color.B,brush.Color);
				sw.Write(" {0}（{1}）",kanji,show(names));
				return sw.ToString();
			}
		}
		public override string ToString() {
			return String.Format("{0:000},{1:000},{2:000} {3}",Brush.Color.R,Brush.Color.G,Brush.Color.B,show(Names));
		}
		public virtual string ToString2() {
			return String.Format("{0}({1})",Kanji,Yomi);
		}
		public bool StartsWith(string head) {
			int i = 0;
			foreach(string name in Names) {
				if(name.StartsWith(head)) {
					++i;
				}
			}
			return i>0;
		}
		public Color GetColor() {
			return Color.FromArgb(this.R,this.G,this.B);
		}
	}
}
