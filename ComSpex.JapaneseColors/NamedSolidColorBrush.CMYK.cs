using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComSpex.JapaneseColors {
	public partial class NamedSolidColorBrush:IEquatable<NamedSolidColorBrush>, IComparable<NamedSolidColorBrush> {
		public NamedSolidColorBrush(CMYK cmyk)
			: this(cmyk.Color) {
		}
		public NamedSolidColorBrush(CMYK cmyk,string yomi)
			: this(cmyk.Color,yomi) {
		}
		public NamedSolidColorBrush(string kanji,CMYK cmyk,string yomi)
			: this(kanji,new SolidColorBrush(cmyk.Color),yomi) {
		}
		public NamedSolidColorBrush(CMYK cmyk,string yomi,bool alt)
			: this(cmyk.Color,yomi,alt) {
		}
		public NamedSolidColorBrush(double c,double m,double y,double k,string yomi)
			: this(new CMYK(c,m,y,k),yomi) {
		}
		public NamedSolidColorBrush(double c,double m,double y,double k,string yomi,bool alt)
			: this(new CMYK(c,m,y,k),yomi,alt) {
		}
	}
}
