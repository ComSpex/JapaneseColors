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
#if true
					lV=(this.R<<16)|(this.G<<8)|this.B;
					rV=(other.R<<16)|(other.G<<8)|other.B;
					if(lV<rV) { return -1*reverse; }
					if(lV>rV) { return 1*reverse; }
#else
					Color c = this.Brush.Color;
					Color d = other.Brush.Color;
					string Lrgb = c.ToString().ToUpper().Replace("#FF","0x");
					string Rrgb = d.ToString().ToUpper().Replace("#FF","0x");
					int lhs = Convert.ToInt32(Lrgb,16);
					int rhs = Convert.ToInt32(Rrgb,16);
					if(lhs<rhs) { return -1*reverse; }
					if(lhs>rhs) { return 1*reverse; }
#endif
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
