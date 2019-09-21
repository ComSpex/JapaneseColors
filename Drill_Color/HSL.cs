using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Drill_Color {
	/// <summary>
	/// HSL
	/// </summary>
	public class HSL:CMYK, IEquatable<HSL>, IComparable<HSL> {
		public int H, S, L;
		public HSL()
			: base() {
		}
		public HSL(Color color) :
			base(color) {
			RGB=color;
			ToHSL(color);
		}
		public HSL(SolidColorBrush scb) :
			this(scb.Color) {
		}
		public HSL(int hue,int saturation,int lightness) {
			this.H=hue;
			this.S=saturation;
			this.L=lightness;
			RGB=ToRGB(H,S,L);
		}
		public virtual Color ToRGB(double h,double s,double l) {
			//https://www.rapidtables.com/convert/color/hsl-to-rgb.html
			s/=100D;
			l/=100D;
			double C = (1D-Math.Abs(2D*l-1.0))*s;
			double X = C*(1D-Math.Abs(Math.IEEERemainder(h/60d,2D)-1D));
			double m = l-C/2D;
			return CalculateRGB(C,X,h,m);
		}
		protected virtual Color CalculateRGB(double C,double X,double h,double m) {
			double[] rgb = new double[] { 0,0,0 };
			if(0D<=h&&h<60d) {
				rgb=new double[] { C,X,0 };
			} else if(60D<=h&&h<120D) {
				rgb=new double[] { X,C,0 };
			} else if(120D<=h&&h<180D) {
				rgb=new double[] { 0,C,X };
			} else if(180D<=h&&h<240) {
				rgb=new double[] { X,0,C };
			} else if(240<=h&&h<300) {
				rgb=new double[] { X,C,0 };
			} else if(300<=h&&h<360) {
				rgb=new double[] { C,0,X };
			}
			double r = (rgb[0]+m)*255;
			double g = (rgb[1]+m)*255;
			double b = (rgb[2]+m)*255;
			return Color.FromRgb(ToB(r),ToB(g),ToB(b));
		}
		private static byte ToB(double r) {
			try {
				return Convert.ToByte(r);
			} catch { }
			return 0;
		}
		private void ToHSL(Color color) {
			//https://www.rapidtables.com/convert/color/rgb-to-hsl.html
			double R = ToW(color.R)/255.0;
			double G = ToW(color.G)/255.0;
			double B = ToW(color.B)/255.0;
			double Cmax = Math.Max(R,Math.Max(G,B));
			double Cmin = Math.Min(R,Math.Min(G,B));
			double Delta = Cmax-Cmin;
			Calculate(R,G,B,Cmax,Cmin,Delta);
		}
		protected virtual void Calculate(double r,double g,double b,double cmax,double cmin,double delta) {
			double h, s, l;
			h=s=l=double.NaN;
			l=(cmax+cmin)/2D;
			if(delta==0D) {
				h=s=0D;
			} else {
				if(cmax==r) {
					h=60D*Math.IEEERemainder((g-b)/delta,6D);
				} else if(cmax==g) {
					h=60D*((b-r)/delta+2D);
				} else if(cmax==b) {
					h=60D*((r-g)/delta+4D);
				}
				s=delta/(1D-Math.Abs(2D*l-1D));
			}
			try {
				H=Convert.ToInt32(h);
				S=Convert.ToInt32(100*s);
				L=Convert.ToInt32(100*l);
			} catch(Exception ex) {
				throw ex;
			}
		}
		public virtual Color ToRGB(HSL o) {
			return ToRGB(o.H,o.S,o.L);
		}
		public override string ToString() {
			return ToString(true);
		}
		public override string ToString(bool v) {
			return String.Format(v ? "HSL:{0},{1},{2}" : "{0},{1},{2}",H,S,L);
		}
		public bool Equals(HSL other) {
			return
				this.H==other.H&&
				this.S==other.S&&
				this.L==other.L;
		}
		public int CompareTo(HSL other) {
			int U = ToInt(this);
			int D = ToInt(other);
			int reverse = Invert ? -1 : 1;
			if(U<D) { return -1*reverse; }
			if(U>D) { return 1*reverse; }
			return 0;
		}
		protected override int ToInt(CMYK o) {
			HSL p = o as HSL;
			int pH = Math.Abs(p.H);
			return (p.H<<16)|(p.S<<8)|p.L;
		}
	}
}
