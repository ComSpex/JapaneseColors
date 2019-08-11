using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileWindows {
	public class WindowInfo {
		public string Title;
		public IntPtr Handle;
		public WindowInfo(string title,IntPtr handle) {
			Title=title;
			Handle=handle;
		}
		public override string ToString() {
			return Title;
		}
	}
}
