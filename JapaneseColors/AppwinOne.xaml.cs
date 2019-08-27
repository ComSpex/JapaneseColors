using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using Drill_Color;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfAppone {
	/// <summary>
	/// Interaction logic for AppwinOne.xaml
	/// </summary>
	public partial class AppwinOne:Window {
		JapaneseColors jc = new JapaneseColors();
		public AppwinOne() {
			Application.Current.SessionEnding+=Current_SessionEnding;
			Application.Current.Exit+=Current_Exit;
			InitializeComponent();
			//this.Topmost=true;
			this.Height=1250;
			this.Width=820;
			NamedSolidColorBrush.Invert=
				CMYK.Invert=true;
			fillColors();
			fillIndice();
			updateTitle();
			SetLListBrush(exte);
			this.erase.IsEnabled=false;
		}
		private void Current_Exit(object sender,ExitEventArgs e) {
			int exitCode = e.ApplicationExitCode;
			//MessageBox.Show(String.Format("Exit Code : {0}",exitCode));
		}
		private void Current_SessionEnding(object sender,SessionEndingCancelEventArgs e) {
			switch(e.ReasonSessionEnding) {
				case ReasonSessionEnding.Logoff:
					MessageBox.Show("Logoff");
					break;
				case ReasonSessionEnding.Shutdown:
					MessageBox.Show("Shutdown");
					break;
				default:
					// Never here comes.
					MessageBox.Show("SessionEnding anyway...");
					break;
			}
		}

		private void updateTitle() {
			this.Title=String.Format("Japanese Traditional Colors ({0})",R.Items.Count);
		}
		private void fillColors() {
			Cursor keep = this.Cursor;
			this.Cursor=Cursors.Wait;
			List<NamedSolidColorBrush> checkers = new List<NamedSolidColorBrush>();
			R.Items.Clear();
			foreach(KeyValuePair<string,NamedSolidColorBrush> Core in Jc.Cores){
				if(checkers.Contains(Core.Value)) { continue; }
				checkers.Add(Core.Value);
				ListBoxItem item=new ListBoxItem();
				item.HorizontalContentAlignment=HorizontalAlignment.Stretch;
				item.Content=plateOf(Core);
				item.ToolTip=swatchOf(Core);
				R.Items.Add(item);
			}
			updateTitle();
			clea.IsEnabled=false;
			this.Cursor=keep;
		}
		/// <summary>
		/// Descriptive user interface for Content
		/// </summary>
		/// <param name="Core">KeyValuePair<string,NamedSolidColorBrush></param>
		/// <returns>UniformGrid</returns>
		public UIElement plateOf(KeyValuePair<string,NamedSolidColorBrush> Core) {
			return plateOf(Core.Key,Core.Value.Name,Core.Value.Brush);
		}
		/// <summary>
		/// Descriptive user interface core for cloning the Content
		/// </summary>
		/// <param name="key">漢字色名</param>
		/// <param name="name">読み</param>
		/// <param name="brush">色SolidColorBrush</param>
		/// <returns></returns>
		protected virtual UIElement plateOf(string key,string name,Brush brush) {
			SolidColorBrush B = brush as SolidColorBrush;
			CMYK cmyk = new CMYK(B.Color);
			HSL hsl = new HSL(B.Color);
			HSV hsv = new HSV(B.Color);
			TextBlock one = new TextBlock();
			TextBlock two = new TextBlock();
			TextBlock san = new TextBlock();
			TextBlock yon = new TextBlock();
			TextBlock goh = new TextBlock();
			one.Text=key;
			two.Text=name;
			san.Text=rgb(B.Color);
			yon.Text=rgb(B.Color,true);
			switch(NamedSolidColorBrush.howCompare) {
				case NamedSolidColorBrush.HowCompare.RGB:
					san.FontWeight=yon.FontWeight=FontWeights.Bold;
					goto default;
				case NamedSolidColorBrush.HowCompare.CMYK:
					goh.FontWeight=FontWeights.Bold;
					goto default;
				case NamedSolidColorBrush.HowCompare.HSL:
					goh.Text=hsl.ToString(false);
					goh.FontWeight=FontWeights.Bold;
					break;
				case NamedSolidColorBrush.HowCompare.HSV:
					goh.Text=hsv.ToString(false);
					goh.FontWeight=FontWeights.Bold;
					break;
				case NamedSolidColorBrush.HowCompare.Yomi:
					two.FontWeight=FontWeights.Bold;
					goto default;
				default:
					goh.Text=cmyk.ToString(false);
					break;
			}
			UniformGrid ug = new UniformGrid {
				Background=brush
			};
			ug.Children.Add(one);
			ug.Children.Add(two);
			ug.Children.Add(san);
			ug.Children.Add(yon);
			ug.Children.Add(goh);
			one.TextAlignment=TextAlignment.Right;
			two.TextAlignment=TextAlignment.Left;
			san.FontWeight=FontWeights.Bold;
			ug.Columns=ug.Children.Count;
			one.Foreground=two.Foreground=san.Foreground=yon.Foreground=goh.Foreground=invert(brush);
			one.Margin=two.Margin=san.Margin=yon.Margin=goh.Margin=new Thickness(6);
			ug.Tag=one.Text;
			return ug;
		}
		void fillIndice() {
#if false
			byte[] codes=new byte[166];
			for(int ii=0,j=0x41;ii<codes.Length;++j,ii+=2){
				codes[ii+0]=(byte)j;
				codes[ii+1]=0x30;
			}
			string cs =System.Text.Encoding.Unicode.GetString(codes);
#else
			List<string> keys = new List<string>();
			string cs = String.Empty;
			foreach(KeyValuePair<string,NamedSolidColorBrush> Core in Jc.Cores) {
				foreach(string top in Core.Value.Names) {
					string one = top.Substring(0,1);
					if(!keys.Contains(one)) {
						keys.Add(one);
					}
				}
			}
			keys.Sort();
			foreach(string key in keys) {
				cs+=key;
			}
#endif
			System.Diagnostics.Debug.WriteLine(cs);
			L.Items.Clear();
			L.Items.Add(ListItem("(全て)"));
			foreach(char c in cs){
				L.Items.Add(ListItem(c.ToString()));
			}
			L.Items.Add(ListItem("(全て)"));
		}
		private object ListItem(string p) {
			ListBoxItem item=new ListBoxItem();
			TextBlock tb=new TextBlock();
			tb.Text=p;
			tb.FontSize=14;
			tb.FontFamily=new System.Windows.Media.FontFamily("Meiryo UI");
			if(p.Contains("全て")){
				tb.Foreground=Brushes.Silver;
			}
			item.Content=tb;
			item.HorizontalContentAlignment=HorizontalAlignment.Center;
			return item;
		}
		/// <summary>
		/// Descriptive user interface for Tooltip
		/// </summary>
		/// <param name="core">KeyValuePair<string,NamedSolidColorBrush></param>
		/// <returns>StackPanel</returns>
		public Panel swatchOf(KeyValuePair<string,NamedSolidColorBrush> core) {
			return swatchOf(core.Value.Show(core.Key),core.Value.Brush);
		}
		protected virtual Panel swatchOf(string kanji,NamedSolidColorBrush nscb) {
			return swatchOf(NamedSolidColorBrush.Show(kanji,nscb.Brush,nscb.Names),nscb.Brush);
		}
		protected virtual Panel swatchOf(string any_note,Brush color) {
			StackPanel sp=new StackPanel();
			sp.Orientation=Orientation.Horizontal;

			Rectangle re=new Rectangle();
			re.Width=re.Height=16.0;
			re.Fill=color;
			re.Margin=new Thickness(4.0);
			TextBlock tb=new TextBlock();
			tb.VerticalAlignment=VerticalAlignment.Center;
			tb.Text=any_note;

			sp.Children.Add(re);
			sp.Children.Add(tb);

			return sp;
		}
		protected List<string> texs = new List<string>();
		private void L_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			if(L.SelectedIndex<0){
				return;
			}
			texs.Clear();
			foreach(var item in e.AddedItems){
				TextBlock tb=((ListBoxItem)item).Content as TextBlock;
				if(tb!=null){
					if(tb.Text.Contains("全て")) {
						fillColors();
						return;
					}
					texs.Add(tb.Text);
				}
			}
			if(texs.Count>1){
				fillColors(texs);
			}else if(texs.Count==1){
				bool clean=!(incl.IsChecked??false);
				fillColors(texs[0],clean);
			}
			e.Handled=true;
		}
		private void fillColors(List<string> texs,bool clean=false) {
			Cursor keep = this.Cursor;
			this.Cursor=Cursors.Wait;
			if(clean) {
				R.Items.Clear();
			}
			foreach(string tex in texs){
				fillColors(tex,false);
			}
			if(clean&&texs.Count==0) {
				fillColors();
			}
			updateTitle();
			this.Cursor=keep;
		}
		private void fillColors(string head,bool clean = false) {
			if(clean) {
				R.Items.Clear();
			}
			foreach(KeyValuePair<string,NamedSolidColorBrush> Core in Jc.Cores) {
				if(Core.Value.NameStartsWith(head)) {
					ListBoxItem item = new ListBoxItem();
					item.HorizontalContentAlignment=HorizontalAlignment.Stretch;
					item.Content=plateOf(Core);
					item.ToolTip=swatchOf(Core);
					if(!R.Items.Contains(item)) {
						R.Items.Add(item);
					}
				}
			}
			updateTitle();
		}
		/// <summary>
		/// Kanji supportive version of fillColors()
		/// </summary>
		/// <param name="texs"></param>
		/// <param name="clean"></param>
		private void fillColorsKanji(List<string> texs,bool clean=false) {
			Cursor keep = this.Cursor;
			this.Cursor=Cursors.Wait;
			if(clean) {
				R.Items.Clear();
			}
			int index=0;
			foreach(string tex in texs) {
				fillColorsKanji(tex,ref index);
			}
			if(clean&&texs.Count==0) {
				fillColors();
			}
			updateTitle();
			this.Cursor=keep;
		}
		private void fillColorsKanji(string tex,ref int index) {
			int ii = 0;
			List<NamedSolidColorBrush> checkers = new List<NamedSolidColorBrush>();
			foreach(KeyValuePair<string,NamedSolidColorBrush> Core in Jc.Cores) {
				if(++ii<=index) {
					continue;
				}
				++index;
				if(checkers.Contains(Core.Value)) {
					SystemSounds.Beep.Play();
					continue;
				}
				checkers.Add(Core.Value);
				if(Core.Value.Kanji.Contains(tex)) {
					ListBoxItem item = new ListBoxItem();
					item.HorizontalContentAlignment=HorizontalAlignment.Stretch;
					item.Content=plateOf(Core);
					item.ToolTip=swatchOf(Core);
					if(!R.Items.Contains(item)) {
						R.Items.Add(item);
					}
					return;
				}
			}
		}
		private void Clear_Click(object sender,RoutedEventArgs e) {
			ClearList();
			partofYomi.Text=String.Empty;
		}
		private void CheckBox_Checked(object sender,RoutedEventArgs e) {
			CheckBox cb=sender as CheckBox;
			if((string)cb.Content=="Invert") {
				CMYK.Invert=
				NamedSolidColorBrush.Invert=cb.IsChecked.Value;
				fillColors(texs,true);
				return;
			}
			clea.IsEnabled=cb.IsChecked??false;
			if(clea.IsEnabled){
				ClearListR();
			}
		}
		private void ClearListR() {
			R.Items.Clear();
			updateTitle();
			ClearListL();
		}
		private void ClearListL() {
			if(L.SelectionMode==SelectionMode.Multiple) {
				L.SelectedItems.Clear();
			} else {
				L.SelectedIndex=-1;
			}
		}
		private void RadioButton_Checked(object sender,RoutedEventArgs e) {
			RadioButton rb=sender as RadioButton;
			if(AnyOfGroupMembers(rb)) {
				string name = (string)rb.Content;
				if(name.StartsWith("~")) {
					name=name.Replace("~","n");
				}
				NamedSolidColorBrush.howCompare=(NamedSolidColorBrush.HowCompare)Enum.Parse(typeof(NamedSolidColorBrush.HowCompare),name);
				fillColors(texs,true);
				return;
			}
			L.SelectionMode=(SelectionMode)Enum.Parse(typeof(SelectionMode),(string)rb.Content);
			SetLListBrush(rb);
			ClearListL();
		}
		private bool AnyOfGroupMembers(RadioButton rb) {
			List<string> members = new List<string>();
			StackPanel sp = SortGroup.Content as StackPanel;
			foreach(UIElement elem in sp.Children) {
				if(elem is RadioButton) {
					RadioButton ra = elem as RadioButton;
					string name = (string)ra.Content;
					members.Add(name);
				}
			}
			foreach(string member in members) {
				if((string)rb.Content==member) {
					return true;
				}
			}
			return false;
		}
		private void SetLListBrush(RadioButton rb){
			SetLListBrush(rb.Background);
			if(rb.Tag!=null) {
				L.Foreground=Brushes.Snow;
			}
		}
		private void SetLListBrush(Brush brush) {
			L.Background=brush;
			//L.Foreground=Brushes.Black;//invert(L.Background);
		}
		private string rgb(Color color,bool hex=false) {
			if(hex) {
				return String.Format("#{0,2:X2}{1,2:X2}{2,2:X2}",color.R,color.G,color.B);
			}
			return String.Format("{0:000},{1:000},{2:000}",color.R,color.G,color.B);
		}
		public static Brush invert(Brush brush) {
			SolidColorBrush sc=brush as SolidColorBrush;
			return new SolidColorBrush(Color.FromRgb((byte)~sc.Color.R,(byte)~sc.Color.G,(byte)~sc.Color.B));
		}
		protected override void OnClosing(CancelEventArgs e) {
			foreach(Window win in wins.Values) {
				win.Owner=this;
			}
			foreach(KeyValuePair<string,Child> form in forms) {
				form.Value.Owner=this;
			}
			base.OnClosing(e);
		}
		bool isTerminated = false;
		public bool Terminate() {
			bool cancelled = true;
			if(!isTerminated) {
				isTerminated=true;
				if(MessageBoxResult.OK==MessageBox.Show("Are you sure to close all the windows?",Title,MessageBoxButton.OKCancel,MessageBoxImage.Question)) {
					Close();
					cancelled=false;
				}
				return cancelled;
			} else {
				return false;
			}
		}
		private void Tile_Click(object sender,RoutedEventArgs e) {
			isTerminated=false;
			TileWindows();
			BringWindowToTop(new WindowInteropHelper(this).EnsureHandle());
			this.erase.IsEnabled=true;
			this.tile.IsEnabled=false;
		}

		[DllImport("user32.dll",SetLastError = true)]
		static extern bool BringWindowToTop(IntPtr hWnd);

		private void Erase_Click(object sender,RoutedEventArgs e) {
			foreach(KeyValuePair<string,Child> form in forms) {
				form.Value.Close();
			}
			// Why the top window remains unclosed...
			foreach(KeyValuePair<string,Child> form in forms) {
				form.Value.Close();
			}
			this.tile.IsEnabled=true;
			SystemSounds.Hand.Play();
			this.erase.IsEnabled=false;
		}
		private void PartOfYomi_Click(object sender,RoutedEventArgs e) {
			if(!incl.IsChecked.Value) {
				texs.Clear();
			}
			if(String.IsNullOrEmpty(partofYomi.Text)) {
				ClearList();
				return;
			}
			bool isKanji = false;
			foreach(ListBoxItem item in R.Items) {
				if(item==null) { continue; }
				if(!(item.Content is UniformGrid ug)) { continue; }
				if(!(ug.Children[1] is TextBlock hira)) { continue; }
				if(hira.Text.Contains(partofYomi.Text)) {
					texs.Add(hira.Text);
				}
				if(!(ug.Children[0] is TextBlock kanj)) { continue; }
				if(kanj.Text.Contains(partofYomi.Text)) {
					texs.Add(kanj.Text);
					isKanji=true;
				}
			}
			if(texs.Count>0) {
				if(isKanji) {
					fillColorsKanji(texs,true);
				} else {
					fillColors(texs,true);
				}
				clea.IsEnabled=true;
			} else {
				MessageBox.Show(String.Format("cannot find any of '{0}'",partofYomi.Text),this.Title,MessageBoxButton.OK,MessageBoxImage.Information);
			}
			e.Handled=true;
		}
		private void ClearList() {
			ClearListR();
			ClearListL();
			fillColors();
		}
	}
}
