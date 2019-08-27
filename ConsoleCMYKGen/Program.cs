using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WpfAppone;
using Drill_Color;

namespace ConsoleCMYKGen {
	class Program {
		static void Main(string[] args) {
			const int version = 9;
			Console.InputEncoding=Console.OutputEncoding=Encoding.Unicode;
			FileInfo file = new FileInfo(String.Format(@"J:\Yosuke\Documents\定本 和の色事典{0}.txt",version));
			List<NamedSolidColorBrush> cmyks = new List<NamedSolidColorBrush>();
			using(StreamReader sr = file.OpenText()) {
				while(!sr.EndOfStream) {
					try {
						string buff = sr.ReadLine();
						string[] elems = buff.Split(new char[] { '　',' ' });
						//ConsoleWriteLine("{0} : {1}",elems.Length,buff);
						if(elems.Length>=6) {
							Console.ResetColor();
							CMYK item = new CMYK(is100(elems[2]),is100(elems[3]),is100(elems[4]),is100(elems[5]));
							NamedSolidColorBrush cmyk = new NamedSolidColorBrush(elems[0],item,elems[1]);
							if(!cmyks.Contains(cmyk)) {
								cmyks.Add(cmyk);
							} else {
								cmyk.IsAlias=true;
								cmyks.Add(cmyk);
							}
						} else {
							if(!buff.StartsWith("//")) {
								throw new Exception(buff);
							}
						}
					} catch(Exception ex) {
						Console.ForegroundColor=ConsoleColor.Red;
						ConsoleWriteLine(ex.Message);
						Console.ResetColor();
					}
				}
#if false
					for(int i = 0;i<elems.Length;++i) {
						if(i>=1) {
							ConsoleWrite(" ");
						}
						ConsoleWrite("'{0}'",elems[i].Trim());
					}
					ConsoleWriteLine();
#endif
				string text = null;
				FileInfo classfile = new FileInfo(String.Format("More Japanese Colors{0}.txt",version));
				using(FileStream fs = classfile.OpenWrite()) {
					using(StreamWriter sw = new StreamWriter(fs)) {
						sw.AutoFlush=true;
						foreach(NamedSolidColorBrush brush in cmyks) {
							if(brush.IsAlias) {
								text=String.Format(@"public static NamedSolidColorBrush {0}=new NamedSolidColorBrush({1},{2},{3},""{4}"",{5});",brush.Kanji,brush.R,brush.G,brush.B,brush.Yomi,brush.IsAlias.ToString().ToLower());
							} else {
								text=String.Format(@"public static NamedSolidColorBrush {0}=new NamedSolidColorBrush({1},{2},{3},""{4}"");",brush.Kanji,brush.R,brush.G,brush.B,brush.Yomi);
							}
							ConsoleWriteLine(sw,text);
						}
					}
				}
			}
		}
		private static double is100(string v) {
			int percent = Convert.ToInt32(v);
			if(0<=percent&&percent<=100) {
				return percent;
			}
			throw new IndexOutOfRangeException("not percentage(0-100)");
		}
		private static void ConsoleWriteLine(StreamWriter sw,string text) {
			sw.WriteLine(text);
			ConsoleWriteLine(text);
		}
		private static void ConsoleWriteLine(string text) {
			Debug.WriteLine(text);
			Console.WriteLine(text);
		}
		private static void ConsoleWriteLine() {
			ConsoleWriteLine("");
		}
		private static void ConsoleWrite(string format,params object[] args) {
			string text = String.Format(format,args);
			Debug.Write(text);
			Console.Write(text);
		}
		private static void ConsoleWriteLine(string format,params object[] args) {
			string text = String.Format(format,args);
			Debug.WriteLine(text);
			Console.WriteLine(text);
		}
	}
}
