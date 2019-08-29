using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Drill_Color {
	/// <summary>
	/// HSV
	/// </summary>
	public class HSV:HSL, IEquatable<HSV>, IComparable<HSV> {
		public int V;
		public HSV() : base() { }
		public HSV(int h,int s,int v)
			: base() {
			H=h;
			S=s;
			V=v;
			RGB=ToRGB(H,S,V);
		}
		public HSV(Color color)
			: base(color) {
		}
		public HSV(SolidColorBrush scb)
			: this(scb.Color) {
		}
		public override Color ToRGB(double h,double s,double v) {
			// https://www.rapidtables.com/convert/color/hsv-to-rgb.html
			//h/=100D;
			s/=100D;
			v/=100D;
			double C = v*s;
			double X = C*(1D-Math.Abs(Math.IEEERemainder(h/60D,2D)-1D));
			double m = v-C;
			return CalculateRGB(C,X,h,m);
		}
		protected override void Calculate(double r,double g,double b,double cmax,double cmin,double delta) {
			// https://www.rapidtables.com/convert/color/rgb-to-hsv.html
			base.Calculate(r,g,b,cmax,cmin,delta);
			double h, s, v = cmax;
			h=s=double.NaN;
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
				if(cmax==0D) {
					s=0D;
				} else {
					s=delta/cmax;
				}
			}
			try {
				H=Convert.ToInt32(Math.Floor(h));
				S=Convert.ToInt32(Math.Floor(100.0*s));
				V=Convert.ToInt32(Math.Floor(100.0*v));
			} catch(Exception) { }
		}
		public bool Equals(HSV other) {
			return
				this.H==other.H&&
				this.S==other.S&&
				this.V==other.V;
		}
		public override string ToString() {
			return ToString(true);
		}
		public override string ToString(bool v) {
			return String.Format(v ? "HSV:{0},{1},{2}" : "{0},{1},{2}",H,S,V);
		}
		public int CompareTo(HSV other) {
			int U = ToInt(this);
			int D = ToInt(other);
			int reverse = Invert ? -1 : 1;
			if(U<D) { return -1*reverse; }
			if(U>D) { return 1*reverse; }
			return 0;
		}
		protected override int ToInt(CMYK o) {
			HSV p = o as HSV;
			int pH = Math.Abs(p.H);
			return (p.H<<16)|(p.S<<8)|p.V;
		}
	}
}
