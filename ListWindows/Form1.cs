using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ListWindows {
	public partial class Form1:Form {
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWindowVisible(IntPtr hWnd);
		[DllImport("user32.dll",EntryPoint = "GetWindowText",ExactSpelling = false,CharSet = CharSet.Auto,SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd,StringBuilder ipWindowText,int nMaxCount);
		[DllImport("user32.dll",EntryPoint = "EnumDesktopWindows",ExactSpelling = false,CharSet = CharSet.Auto,SetLastError = true)]
		private static extern bool EnumDesktopWindows(IntPtr hDesktop,EnumDelegate lpEnumCallbackFunction,IntPtr lParam);
		[DllImport("user32.dll",EntryPoint = "CloseWindow",ExactSpelling = false,CharSet = CharSet.Auto,SetLastError = true)]
		private static extern bool CloseWindow(IntPtr hWnd);
		[DllImport("user32.dll",EntryPoint = "DestroyWindow",ExactSpelling = false,CharSet = CharSet.Auto,SetLastError = true)]
		private static extern bool DestroyWindow(IntPtr hWnd);
		[DllImport("user32.dll",EntryPoint = "SendMessage",ExactSpelling = false,CharSet = CharSet.Auto,SetLastError = true)]
		private static extern bool SendMessage(IntPtr hWnd,Msgs Msg,int wParam,IntPtr lParam);
		private delegate bool EnumDelegate(IntPtr hWnd,int lParam);

		public static List<IntPtr> WindowHandles;
		public static List<string> WindowTitles;

		bool veryFirst = true;
		enum Msgs:int {
			WM_DESTROY=0x0002,
		}
		public static void GetDesktopWindowHandlesAndTitles(out List<IntPtr> handles,out List<string> titles) {
			WindowHandles=new List<IntPtr>();
			WindowTitles=new List<string>();
			if(!EnumDesktopWindows(IntPtr.Zero,FilterCallback,IntPtr.Zero)) {
				handles=null;
				titles=null;
			} else {
				handles=WindowHandles;
				titles=WindowTitles;
			}
		}
		private static bool FilterCallback(IntPtr hWnd,int lParam) {
			StringBuilder sb_title = new StringBuilder(1024);
			int length = GetWindowText(hWnd,sb_title,sb_title.Capacity);
			string title = sb_title.ToString();
			if(IsWindowVisible(hWnd)&&string.IsNullOrEmpty(title)==false) {
				WindowHandles.Add(hWnd);
				WindowTitles.Add(title);
			}
			return true;
		}
		public Form1() {
			InitializeComponent();
		}
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			Refresh();
		}
		public object ShowDesktopWindows() {
			List<IntPtr> handles;
			List<string> titles;
			GetDesktopWindowHandlesAndTitles(out handles,out titles);
			return titles;
		}
		private void button1_Click(object sender,EventArgs e) {
			Refresh();
		}
		public override void Refresh() {
			base.Refresh();
			veryFirst=true;
			listBox1.DataSource=ShowDesktopWindows();
		}
		private void listBox1_SelectedIndexChanged(object sender,EventArgs e) {
			if(veryFirst) {
				veryFirst=false;
				return;
			}
			ListBox lb = sender as ListBox;
			string title = (string)lb.Items[lb.SelectedIndex];
			IntPtr hWnd = WindowHandles[lb.SelectedIndex];
			Debug.WriteLine(String.Format("sender={0};0x{1:X6}",title,hWnd));
			if(DialogResult.OK==MessageBox.Show("Are you sure?","Kill Window",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)) {
				CloseWindow(hWnd);
				DestroyWindow(hWnd);// cannot destory any other window because I'm not the owner of any of other windows.
				SendMessage(hWnd,Msgs.WM_DESTROY,0,IntPtr.Zero);
				Application.DoEvents();
				Refresh();
			}
		}
		protected override void OnFormClosing(FormClosingEventArgs e) {
			switch(e.CloseReason) {
				case CloseReason.ApplicationExitCall:
					break;
				case CloseReason.FormOwnerClosing:
					break;
				case CloseReason.MdiFormClosing:
					break;
				case CloseReason.None:
					break;
				case CloseReason.TaskManagerClosing:
					break;
				case CloseReason.UserClosing:
					break;
				case CloseReason.WindowsShutDown:
					break;
			}
			base.OnFormClosing(e);
		}
	}
}
