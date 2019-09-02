using Drill_Color;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfAppone {
	public partial class NamedSolidColorBrush:IEquatable<NamedSolidColorBrush>, IComparable<NamedSolidColorBrush> {
		public NamedSolidColorBrush(ListBoxItem lbi) {
			UniformGrid ug = lbi.Content as UniformGrid;
			TextBlock one = ug.Children[0] as TextBlock;
			TextBlock two = ug.Children[1] as TextBlock;
			TextBlock san = ug.Children[2] as TextBlock;
			Kanji=one.Text;
			Names=two.Text.Split(',');
			Brush=new SolidColorBrush(ToColor(san.Inlines));
		}
		private Color ToColor(InlineCollection inlines) {
			int i = 0;
			byte R, G, B;
			R=G=B=0xff;
			foreach(Run iline in inlines) {
				if(i==0) { R=Convert.ToByte(iline.Text); }
				if(i==2) { G=Convert.ToByte(iline.Text); }
				if(i==4) { B=Convert.ToByte(iline.Text); }
				++i;
			}
			return Color.FromRgb(R,G,B);
		}
		private Color ToColor(string text) {
			string[] rgb = text.Split(',');
			byte R = Convert.ToByte(rgb[0].Trim());
			byte G = Convert.ToByte(rgb[1].Trim());
			byte B = Convert.ToByte(rgb[2].Trim());
			return Color.FromRgb(R,G,B);
		}
		public bool Equals(NamedSolidColorBrush other) {
			if(this.Kanji!=null&&other.Kanji!=null) {
				return
					this.Kanji.Trim()==other.Kanji.Trim()&&
					this.Name.Trim()==other.Name.Trim()&&
					this.Brush.Color==other.Brush.Color;
			}
			return this.Brush.Color==other.Brush.Color;
		}
		public enum HowCompare {
			R,
			nR,
			G,
			nG,
			B,
			nB,
			RGB,
			CMYK,
			HSL,
			HSV,
			Yomi,
			Kanji
		}
		public static HowCompare howCompare = HowCompare.R;
		protected static int reverse = -1;
		public static bool Invert {
			get { return reverse==-1; }
			set { if(value) { reverse=-1; } else { reverse=1; } }
		}
		public int CompareTo(NamedSolidColorBrush other) {
			if(other==null) {
				return 1;
			}
			long lV, rV;
			switch(howCompare) {
				case HowCompare.R:
					if(this.R<other.R) {
						return -1*reverse;
					}
					if(this.R>other.R) {
						return 1*reverse;
					}
					break;
				case HowCompare.nR:
					lV=(this.G<<8)|this.B;
					rV=(other.G<<8)|other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					goto case HowCompare.R;
				case HowCompare.G:
					if(this.G<other.G) {
						return -1*reverse;
					}
					if(this.G>other.G) {
						return 1*reverse;
					}
					break;
				case HowCompare.nG:
					lV=(this.R<<16)|this.B;
					rV=(other.R<<16)|other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					goto case HowCompare.G;
				case HowCompare.B:
					if(this.B<other.B) {
						return -1*reverse;
					}
					if(this.B>other.B) {
						return 1*reverse;
					}
					break;
				case HowCompare.nB:
					lV=(this.R<<16)|(this.G<<8);
					rV=(other.R<<16)|(other.G<<8);
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					goto case HowCompare.B;
				case HowCompare.RGB:
					lV=(this.R<<16)|(this.G<<8)|this.B;
					rV=(other.R<<16)|(other.G<<8)|other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					break;
				case HowCompare.CMYK:
					CMYK L = new CMYK(this.Brush.Color);
					CMYK R = new CMYK(other.Brush.Color);
					return L.CompareTo(R);
					//break;
				case HowCompare.HSL:
					HSL hL = new HSL(this.Brush.Color);
					HSL hR = new HSL(other.Brush.Color);
					return hL.CompareTo(hR);
					//break;
				case HowCompare.HSV:
					HSV vL = new HSV(this.Brush.Color);
					HSV vR = new HSV(other.Brush.Color);
					return vL.CompareTo(vR);
					//break;
				case HowCompare.Kanji:
					return this.Kanji.CompareTo(other.Kanji)*reverse;
					//break;
				case HowCompare.Yomi:
					int ii=this.Name.CompareTo(other.Name)*reverse;
					if(ii==0) {
						if(!String.IsNullOrEmpty(this.Kanji)&&!String.IsNullOrEmpty(other.Kanji)) {
							goto case HowCompare.Kanji;
						}
					}
					return ii;
			}
			return 0;
		}
	}
}
