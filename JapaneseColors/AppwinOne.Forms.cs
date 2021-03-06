﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Application;
using ComSpex.JapaneseColors;

namespace WpfAppone {
	public partial class AppwinOne {
		Dictionary<string,Child> forms = new Dictionary<string,Child>();
		int _limit;
		int Limit => _limit;
		private void TileWindows() {
			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			int wide=10;
			int high=10;
			Match Ma = Regex.Match(this.wxh.Text,"(?<w>[0-9]+).[x].(?<h>[0-9]+)");
			if(Ma.Success) {
				wide=Convert.ToInt32(Ma.Groups["w"].Value.Trim());
				high=Convert.ToInt32(Ma.Groups["h"].Value.Trim());
				_limit=wide*high;
				if(R.Items.Count>0) {
					_limit=Math.Min(_limit,R.Items.Count);
				}
			} else {
				throw new InvalidOperationException("cannot read out 'W x H'!!");
			}
			int wnum = ColorIndex;
			GenerateWindows(Limit);
			int wide_limit = (int)(r.Width/wide);
			int high_limit = (int)(r.Height/high);
			int y = r.Top;
			for(int row = 0;row<high;row++) {
				int x = r.Left;
				for(int col = 0;col<wide;col++) {
					Child f = GetWindow(wnum);
					if(f==null) { continue; }
					MoveWindow(f,x,y,wide_limit,high_limit);
					if(++wnum>=forms.Count) {
						goto breakout;
					}
					x+=wide_limit;
				}
				y+=high_limit;
			}
			breakout:
			this.WindowState=WindowState.Minimized;
		}
		private void MoveWindow(Child f,int x,int y,int wide_limit,int high_limit) {
			try {
				f.Left=x;
				f.Top=y;
				f.Width=wide_limit;
				f.Height=high_limit;
			} catch(Exception) {
			}
		}
		private Child GetWindow(int wnum) {
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
		int ColorIndex = 0;
		List<NamedSolidColorBrush> listValues = new List<NamedSolidColorBrush>();
		List<string> listKeys = new List<string>();
		private void GenerateWindows(int limit) {
			if(listValues.Count==0) {
#if true
				foreach(System.Windows.Controls.ListBoxItem item in R.Items) {
					NamedSolidColorBrush nscb = new NamedSolidColorBrush(item);
					listKeys.Add(nscb.Kanji);
					listValues.Add(nscb);
          DoEvents();
				}
#else
				foreach(KeyValuePair<string,NamedSolidColorBrush> Core in Jc.Cores) {
					listKeys.Add(Core.Key);
					listValues.Add(Core.Value);
				}
#endif
			}
			for(int i = 0;i<limit;++i) {
				if(ColorIndex>=listValues.Count) {
					ColorIndex=0;
				}
				string jname = listKeys[ColorIndex];
				string title = String.Format("Child-Color#{0:000}-{1}",ColorIndex,jname);
				Child f = new Child(this,title,new KeyValuePair<string, NamedSolidColorBrush>(jname,listValues[ColorIndex]));
				f.Show();
				forms.Add(title,f);
				++ColorIndex;
        DoEvents();
			}
		}
    public static void DoEvents() {
      Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
    }
  }
}
