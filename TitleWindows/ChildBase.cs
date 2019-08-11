using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Brush = System.Windows.Media.Brush;
using WpfColor = System.Windows.Media.Color;
using FontFamily = System.Drawing.FontFamily;
using System.Drawing;
using System.Windows.Media;
using System.Reflection;
using WpfAppone;
using System.Windows.Forms;

namespace TileWindows {
	public abstract partial class ChildBase:Form,IChild<Color,Brush> {
		protected Dictionary<Color,string> dics = new Dictionary<Color,string>();
		protected List<Color> keys = new List<Color>();
		protected JapaneseColors jc = new JapaneseColors();
		protected FontFamily[] ffs = FontFamily.Families;

		public ChildBase() { }
		public abstract string Tooltip { get; }
		public IEnumerable<KeyValuePair<string,Color>> Cores {
			get {
				foreach(Color key in keys) {
					yield return new KeyValuePair<string,Color>(dics[key],key);
				}
			}
		}
		public void CollectColors() {
			PropertyInfo[] props = typeof(Color).GetProperties(BindingFlags.Static|BindingFlags.Public|BindingFlags.DeclaredOnly);
			foreach(PropertyInfo prop in props) {
				try {
					if(prop.Name.Contains("Transparent")) {
						continue;
					}
					Color c = Color.FromName(prop.Name);
					keys.Add(c);
					dics.Add(c,prop.Name);
				} catch { }
			}
			foreach(KeyValuePair<string,NamedSolidColorBrush> Core in jc.Cores) {
				Color c = Core.Value.GetColor();
				keys.Add(c);
				if(!dics.Keys.Contains(c)) {
					dics.Add(c,Core.Value.ToString2());
				}
			}
		}
		public bool EitherOf(string name) {
			string[] names = new string[] { "ding","Outlook","Marlet","Reference","Bookshelf","symbol" };
			foreach(string nn in names) {
				if(name.Contains(nn)) {
					return true;
				}
			}
			return false;
		}
		protected virtual void ModifyControls(Color cc) { }
		public Brush Invert(Color c) {
			Color cc = Color.FromArgb(~c.R&0xff,~c.G&0xff,~c.B&0xff);
			ModifyControls(cc);
			return new SolidColorBrush(WpfColor.FromRgb(cc.R,cc.G,cc.B));
		}
		public abstract void MainTick(object sender);
		public void RecollectFontFamilies() {
			List<FontFamily> fams = new List<FontFamily>();
			foreach(FontFamily ff in ffs) {
				if(EitherOf(ff.Name)) {
					continue;
				}
				fams.Add(ff);
			}
			ffs=fams.ToArray();
		}
		public string ToColorName(Color c) {
			foreach(KeyValuePair<string,Color> Core in Cores) {
				Color cc = Core.Value;
				if(cc==c) {
					return Core.Key;
				}
			}
			return String.Empty;
		}
		public abstract void ToggleTimer();
		public virtual string ToString(Color c) {
			return String.Format("R={0:000} G={1:000} B={2:000}",c.R,c.G,c.B);
		}
	}
}
