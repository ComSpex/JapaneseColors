using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using Control = System.Windows.Forms.Control;

namespace TileWindows {
	public partial class Form1:Form {
		Dictionary<string,Form> forms = new Dictionary<string,Form>();
		public Form1() {
			InitializeComponent();
			var result = MyMath.ratio(3900,2160);
			this.Text=String.Format("ratio 3900:2160 = {0}",ToRatio(result));
		}
		private string ToRatio(Tuple<int,int> result) {
			string text = String.Format("{0}:{1}",result.Item1,result.Item2);
			return text;
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			Refresh();
		}
		private void button1_Click(object sender,EventArgs e) {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			int x = r.Width/2-this.Width-48;
			int y = r.Height-this.Height;
			MoveWindow(this,x,y,this.Width,this.Height);
			if(forms.Count==0) {
				GenerateWindows();
			}
			DoTileWindows();
			listBox1.SelectedIndex=-1;
			//this.Activate();
			this.Focus();
		}
		public void DoTileWindows() {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			//r=new Rectangle(r.X,r.Y,r.Width/2-58,r.Height);
			int wide_limit = (int)(r.Width/this.numericUpDown2.Value);
			int high_limit = (int)(r.Height/this.numericUpDown1.Value);
			int wnum = 0;
			int y = r.Top;
			for(int row = 0;row<numericUpDown1.Value;row++) {
				int x = r.Left;
				for(int col = 0;col<numericUpDown2.Value;col++) {
					Form f = GetForm(wnum);
					if(f==null) { continue; }
					MoveWindow(f,x,y,wide_limit,high_limit);
					if(++wnum>=forms.Count) {
						return;
					}
					x+=wide_limit;
				}
				y+=high_limit;
			}
		}
		public void Select(Child child) {
			listBox1.SelectedIndex=listBox1.FindStringExact(child.Text);
			this.Text=child.Tooltip;
		}
		private void MoveWindow(Form f,int x,int y,int wide_limit,int high_limit) {
			/*
				System.ComponentModel.Win32Exception
					HResult=0x80004005
					Message=Not enough quota is available to process this command
					Source=WindowsBase
					StackTrace:
					 at MS.Win32.UnsafeNativeMethods.PostMessage(HandleRef hwnd, WindowMessage msg, IntPtr wparam, IntPtr lparam)
					 at System.Windows.Interop.HwndTarget.UpdateWindowSettings(Boolean enableRenderTarget, Nullable`1 channelSet)
					 at System.Windows.Interop.HwndTarget.UpdateWindowPos(IntPtr lParam)
					 at System.Windows.Interop.HwndTarget.HandleMessage(WindowMessage msg, IntPtr wparam, IntPtr lparam)
					 at System.Windows.Interop.HwndSource.HwndTargetFilterMessage(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
					 at MS.Win32.HwndWrapper.WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
					 at MS.Win32.HwndSubclass.DispatcherCallbackOperation(Object o)
					 at System.Windows.Threading.ExceptionWrapper.InternalRealCall(Delegate callback, Object args, Int32 numArgs)
					 at System.Windows.Threading.ExceptionWrapper.TryCatchWhen(Object source, Delegate callback, Object args, Int32 numArgs, Delegate catchHandler)
					 at System.Windows.Threading.Dispatcher.LegacyInvokeImpl(DispatcherPriority priority, TimeSpan timeout, Delegate method, Object args, Int32 numArgs)
					 at MS.Win32.HwndSubclass.SubclassWndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam)
			 */
			try {
				f.Location=new Point(x,y);
				f.Width=wide_limit;
				f.Height=high_limit;
			} catch(Exception) {
			}
		}
		public override void Refresh() {
			base.Refresh();
			foreach(Form ff in forms.Values) {
				ff.Close();
			}
			forms.Clear();
			ListWindows.Form1 f = new ListWindows.Form1();
			listBox1.DataSource=f.ShowDesktopWindows();
		}
		private void button2_Click(object sender,EventArgs e) {
			Refresh();
			GenerateWindows();
		}
		private void GenerateWindows() {
			for(int i = 0;i<(int)numericUpDown3.Value;++i) {
				string title = String.Format("Child-{0:000}",1+i);
				Child f = new Child(this,title);
				f.Show();
				forms.Add(title,f);
			}
			listBox1.DataSource=new List<string>(forms.Keys);
		}
		/// <summary>
		/// Let a child window focused when clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBox1_SelectedIndexChanged(object sender,EventArgs e) {
			if(forms.Count==0) { return; }
			System.Windows.Forms.ListBox lb = sender as System.Windows.Forms.ListBox;
			if(lb.SelectedIndex<0){ return; }
			for(int i=0;i<lb.SelectedIndices.Count;++i){
				int ii = lb.SelectedIndices[i];
				Child form = GetForm(ii) as Child;
				form.Focus();
			}
		}
		private Form GetForm(int wnum) {
			if(forms.Count==0) { return null; }
			int i = 0;
			foreach(string key in forms.Keys) {
				if(wnum==i) {
					return forms[key];
				}
				++i;
			}
			throw new IndexOutOfRangeException();
		}
	}
}
