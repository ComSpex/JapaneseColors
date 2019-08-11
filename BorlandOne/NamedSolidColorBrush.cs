using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace BorlandOne {
	public class NamedSolidColorBrush {
		public SolidBrush Brush;
		public string Name { get { return Names[0]; } }
		public string[] Names;
		public readonly bool Alias;
		public NamedSolidColorBrush(SolidBrush scb,string yomi) {
			Brush=scb;
			Names=yomi.Split(',');
		}
		public NamedSolidColorBrush(Color c,string yomi)
			: this(new SolidBrush(c),yomi) {
		}
		public NamedSolidColorBrush(Color c,string yomi,bool alt)
			: this(new SolidBrush(c),yomi) {
			Alias=alt;
		}
		public NamedSolidColorBrush(int r,int g,int b,string yomi)
			: this(Color.FromArgb(r,g,b),yomi) {
		}
		public NamedSolidColorBrush(int r,int g,int b,string yomi,bool alt)
			: this(Color.FromArgb(r,g,b),yomi,alt) {
		}
		protected virtual string show(string[] names) {
			string text = String.Empty;
			foreach(string name in names) {
				if(!String.IsNullOrEmpty(text)) {
					text+="／";
				}
				text+=name;
			}
			return text;
		}
		public virtual string Show(string kanji) {
			using(StringWriter sw = new StringWriter()) {
				sw.Write("{3} : {0:000},{1:000},{2:000}",Brush.Color.R,Brush.Color.G,Brush.Color.B,Brush.Color);
				sw.Write(" {0}（{1}）",kanji,show(Names));
				return sw.ToString();
			}
		}
		public override string ToString() {
			return String.Format("{0:000},{1:000},{2:000} {3}",Brush.Color.R,Brush.Color.G,Brush.Color.B,show(Names));
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
	}
}
