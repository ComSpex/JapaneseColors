using Drill_Color;
using System;
using System.Windows.Media;

namespace WpfAppone {
	public partial class NamedSolidColorBrush:IEquatable<NamedSolidColorBrush>, IComparable<NamedSolidColorBrush> {
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
			int lV, rV;
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
					lV=this.G+this.B;
					rV=other.G+other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					break;
				case HowCompare.G:
					if(this.G<other.G) {
						return -1*reverse;
					}
					if(this.G>other.G) {
						return 1*reverse;
					}
					break;
				case HowCompare.nG:
					lV=this.R+this.B;
					rV=other.R+other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					break;
				case HowCompare.B:
					if(this.B<other.B) {
						return -1*reverse;
					}
					if(this.B>other.B) {
						return 1*reverse;
					}
					break;
				case HowCompare.nB:
					lV=this.R+this.G;
					rV=other.R+other.G;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
					break;
				case HowCompare.RGB:
					Color c = this.Brush.Color;
					Color d = other.Brush.Color;
					string Lrgb = c.ToString().ToUpper().Replace("#FF","0x");
					string Rrgb = d.ToString().ToUpper().Replace("#FF","0x");
					int lhs = Convert.ToInt32(Lrgb,16);
					int rhs = Convert.ToInt32(Rrgb,16);
					if(lhs<rhs) { return -1*reverse; }
					if(lhs>rhs) { return 1*reverse; }
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
#if true
					int ii=this.Name.CompareTo(other.Name)*reverse;
					if(ii==0) {
						if(!String.IsNullOrEmpty(this.Kanji)&&!String.IsNullOrEmpty(other.Kanji)) {
							return this.Kanji.CompareTo(other.Kanji)*reverse;
						}
					}
					return ii;
#else
					char ll = Convert.ToChar(this.Name.Substring(0,1));
					char rr = Convert.ToChar(other.Name.Substring(0,1));
					for(int i = 0, j = 0;ll==rr;++i, ++j) {
						if(i>=this.Name.Length) { break; }
						if(j>=other.Name.Length) { break; }
						ll=Convert.ToChar(this.Name.Substring(i,1));
						rr=Convert.ToChar(other.Name.Substring(j,1));
					}
					if(ll<rr) { return -1*reverse; }
					if(ll>rr) { return 1*reverse; }
					if(this.Name.Length<other.Name.Length) { return -1*reverse; }
					if(this.Name.Length>other.Name.Length) { return 1*reverse; }
					break;
#endif
			}
			return 0;
		}
	}
}
