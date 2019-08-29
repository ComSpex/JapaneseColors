using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Drill_Color {
	/// <summary>
	/// CMYK
	/// </summary>
	public class CMYK:IEquatable<CMYK>,IComparable<CMYK> {
		static public bool Invert = false;
		public Color RGB;
		public int C, M, Y, K;
		public CMYK() { }
		public CMYK(int c,int m,int y,int k) {
			C=c;
			M=m;
			Y=y;
			K=k;
			RGB=ToColor(c,m,y,k);
		}
		public CMYK(double c,double m,double y,double k) {
			RGB=ToColor(c,m,y,k);
			ToCMYK(RGB);
		}
		public CMYK(string c,string m,string y,string k)
			: this(ToW(c),ToW(m),ToW(y),ToW(k)) {
		}
		public CMYK(SolidColorBrush scb):
			this(scb.Color){
		}
		public CMYK(Color color) {
			RGB=color;
			ToCMYK(RGB);
		}
		protected static double ToW(string c) {
			return Convert.ToDouble(c);
		}
		protected double ToW(byte c) {
			return Convert.ToDouble(c);
		}
		public Color Color => RGB;
		protected virtual Color ToColor(double c,double m,double y,double k) {
			//https://www.rapidtables.com/convert/color/cmyk-to-rgb.html
			c/=100.0;
			m/=100.0;
			y/=100.0;
			k/=100.0;
			byte r = (byte)(ToInt(255.0*(1.0-c)*(1.0-k)));
			byte g = (byte)(ToInt(255.0*(1.0-m)*(1.0-k)));
			byte b = (byte)(ToInt(255.0*(1.0-y)*(1.0-k)));
			Debug.WriteLine(String.Format("C={3},M={4},Y={5},K={6} | R={0},G={1},B={2}",r,g,b,c,m,y,k));
			return Color.FromRgb(r,g,b);
		}
		public virtual Color ToColor(double[] cmyk) {
			return ToColor(cmyk[0],cmyk[1],cmyk[2],cmyk[3]);
		}
		public double[] ToCMYK(Color color) {
			//https://www.rapidtables.com/convert/color/rgb-to-cmyk.html
			double[] cmyk = new double[] { 0D,0D,0D,0D };
			double R = ToW(color.R)/255.0;
			double G = ToW(color.G)/255.0;
			double B = ToW(color.B)/255.0;
			double K = 1.0-Math.Max(B,Math.Max(R,G));
			double C = (1.0-R-K)/(1.0-K);
			double M = (1.0-G-K)/(1.0-K);
			double Y = (1.0-B-K)/(1.0-K);
			if(!(Double.IsNaN(C)&&Double.IsNaN(M)&&Double.IsNaN(Y))){
				try {
					this.C=Convert.ToInt32(100*C);
					this.M=Convert.ToInt32(100*M);
					this.Y=Convert.ToInt32(100*Y);
					this.K=Convert.ToInt32(100*K);
					return cmyk=new double[] { C,M,Y,K };
				} catch { }
			}
			return cmyk;
		}
		static protected int ToInt(double v) {
			return Convert.ToInt32(v);
		}
		public bool Equals(CMYK other) {
			return
				this.C==other.C&&
				this.M==other.M&&
				this.Y==other.Y&&
				this.K==other.K;
		}
		public int CompareTo(CMYK other) {
			int L = ToInt(this);
			int R = ToInt(other);
			int reverse = Invert ? -1 : 1;
			if(L<R) { return -1*reverse; }
			if(L>R) { return 1*reverse; }
			return 0;
		}
		protected virtual int ToInt(CMYK o) {
#if false
			string text = String.Format("{0,3}{1:000}{2:000}",o.C,o.M,o.Y);
			Int64 bigi = Convert.ToInt64(text.Trim());
			return Convert.ToInt32(bigi&0x0000000000000000);
#else
			return (o.C<<24)|(o.M<<16)|(o.Y<<8)|o.K;
#endif
		}
		public override string ToString() {
			return ToString(true);
		}
		public virtual string ToString(bool v) {
			return String.Format(v? "CMYK:{0},{1},{2},{3}" : "{0},{1},{2},{3}",C,M,Y,K);
		}
	}
}
