using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppone {
	/// <summary>
	/// Interaction logic for Child.xaml
	/// </summary>
	public partial class Child:Window {
		private AppwinOne boss;
		public Child() {
			InitializeComponent();
		}
		public Child(AppwinOne appwinOne,string title,KeyValuePair<string,NamedSolidColorBrush> Core) {
			this.boss=appwinOne;
			Title=title;
			Viewbox box = new Viewbox {
				Child=boss.plateOf(Core)
			};
			Content=box;
			Background=(box.Child as Panel).Background;
			ToolTip=boss.swatchOf(Core);
			// Do not set Owner, or Owner window is hidden.
			//this.Owner=appwinOne;
		}
		protected override void OnClosing(CancelEventArgs e) {
			e.Cancel=boss.Terminate();
			base.OnClosing(e);
		}
	}
}
