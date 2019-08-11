using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BorlandOne {
	public class Range {
		public int from;
		public int upto;
		public override string ToString() {
			return String.Format("{0},{1}",from,upto);
		}
		public Range() { }
		public Range(string fu) {
			string[] vals = fu.Split(',');
			try {
				from=Convert.ToInt32(vals[0].Trim());
				upto=Convert.ToInt32(vals[1].Trim());
			} catch(Exception ex) {
				MessageBox.Show(ex.ToString(),"Exception!!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
		}
	}
	public partial class Form1 {
		WhatToDraw what = WhatToDraw.OfColorCode;
		bool UseJapaneseColor {
			get { return ConvertToBoolean(doGet("UseJapaneseColor")); }
			set { doSet("UseJapaneseColor",value.ToString()); }
		}
		bool UseAlpha {
			get { return ConvertToBoolean(doGet("UseAlpha")); }
			set { doSet("UseAlpha",value.ToString()); }
		}
		bool isScreenSaver {
			get { return ConvertToBoolean(doGet("isScreenSaver")); }
			set { doSet("isScreenSaver",value.ToString()); }
		}
		bool ConvertToBoolean(string text) {
			try {
				return Convert.ToBoolean(text);
			} catch(Exception ex) {
				MessageBox.Show(ex.ToString(),"Exception!!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
			return false;
		}
		int ConvertToInt32(string text) {
			try {
				return Convert.ToInt32(text);
			} catch(Exception ex) {
				MessageBox.Show(ex.ToString(),"Exception!!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
			return -1;
		}
		Range ofFont = new Range {
			from=10,
			upto=1000,
		};
		Range ofPen = new Range {
			from=1,
			upto=100,
		};
		int miniWide {
			get { return ConvertToInt32(doGet("miniWide")); }
			set { doSet("miniWide",value.ToString()); }
		}
		int miniHigh {
			get { return ConvertToInt32(doGet("miniHigh")); }
			set { doSet("miniHigh",value.ToString()); }
		}
		int miniSizej {
			get { return ConvertToInt32(doGet("miniSizej")); }
			set { doSet("miniSizej",value.ToString()); }
		}
		int maxSizej {
			get { return ConvertToInt32(doGet("maxSizej")); }
			set { doSet("miniSizej",value.ToString()); }
		}
		int maxSize_ {
			get { return ConvertToInt32(doGet("maxSize_")); }
			set { doSet("maxSize_",value.ToString()); }
		}
		int intervalInSeconds {
			get { return ConvertToInt32(doGet("intervalInSeconds")); }
			set { doSet("intervalInSeconds",value.ToString()); }
		}
		string doGet(string name) {
			string item = ConfigurationManager.AppSettings[name];
			return item;
		}
		string doGet(string name,string value) {
			if(String.IsNullOrEmpty(ConfigurationManager.AppSettings.Get(name))) {
				doSet(name,value);
			}
			string item=ConfigurationManager.AppSettings.Get(name);
			return item;
		}
		private void doSet(string name,string value) {
			/*
				System.Configuration.ConfigurationErrorsException
					HResult=0x80131902
					Message=The configuration is read only.
					Source=System.Configuration
					StackTrace:
					 at System.Configuration.ConfigurationElementCollection.BaseAdd(ConfigurationElement element, Boolean throwIfExists, Boolean ignoreLocks)
					 at System.Configuration.ConfigurationElementCollection.BaseAdd(ConfigurationElement element)
					 at System.Configuration.KeyValueInternalCollection.Add(String key, String value)
					 at BorlandOne.Form1.doSet(String name, String value) in J:\Yosuke\Source\Workspaces\Workspace2013\ComSpex\JapaneseColors\BorlandOne\Form1.Config.cs:line 72
					 at BorlandOne.Form1.set_UseJapaneseColor(Boolean value) in J:\Yosuke\Source\Workspaces\Workspace2013\ComSpex\JapaneseColors\BorlandOne\Form1.Config.cs:line 26
					 at BorlandOne.Form1.LoadSettings() in J:\Yosuke\Source\Workspaces\Workspace2013\ComSpex\JapaneseColors\BorlandOne\Form1.Config.cs:line 76
					 at BorlandOne.Form1..ctor() in J:\Yosuke\Source\Workspaces\Workspace2013\ComSpex\JapaneseColors\BorlandOne\Form1.cs:line 33
					 at BorlandOne.Program.Main(String[] args) in J:\Yosuke\Source\Workspaces\Workspace2013\ComSpex\JapaneseColors\BorlandOne\Program.cs:line 30
			 */
			//ConfigurationManager.AppSettings.Add(name,value);
			ConfigurationManager.AppSettings.Set(name,value);
			//SaveConfiguration();
		}
		public void LoadSettings() {
			if(Environment.CommandLine.Contains("WhatToDraw:")) {
				Match Ma = Regex.Match(Environment.CommandLine,"WhatToDraw[:](?<name>[A-Za-z]+)",RegexOptions.IgnoreCase);
				if(Ma.Success) {
					try {
						string name = Ma.Groups["name"].Value;
						WhatToDraw todraw = (WhatToDraw)Enum.Parse(typeof(WhatToDraw),name,true);
						what=todraw;
						specificDraw=true;
					} catch(Exception ex) {
						throw new Exception("Enum.Parse failed.",ex);
					}
				}
			}
#if true
			if(!UseJapaneseColor) {
				UseJapaneseColor=Environment.CommandLine.Contains("+UseJapaneseColor");
			} else {
				UseJapaneseColor=!Environment.CommandLine.Contains("-UseJapaneseColor");
			}
			if(!UseAlpha) {
				UseAlpha=Environment.CommandLine.Contains("+UseAlpha");
			} else {
				UseAlpha=!Environment.CommandLine.Contains("-UseAlpha");
			}
			if(!isScreenSaver) {
				isScreenSaver=Environment.CommandLine.Contains("/s");
			} else {
				isScreenSaver=!Environment.CommandLine.Contains("-s");
			}
			ofFont=new Range(doGet("Range.ofFont"));
			ofPen=new Range(doGet("Range.ofPen"));
			int[] values = new int[] {
				miniWide,miniHigh,miniSizej,maxSizej,maxSize_
			};
#endif
			ofFont=new Range(doGet("Range.ofFont"));
			ofPen=new Range(doGet("Range.ofPen"));
			//ShowConfig();
		}
		private void ShowConfig() {
#if true
			foreach(string key in ConfigurationManager.AppSettings) {
				string value = ConfigurationManager.AppSettings[key];
				Debug.WriteLine("{0}={1}",key,value);
			}
#else
			FileInfo xml = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			using(FileStream fs=xml.OpenWrite()) {
				using(StreamWriter sw = new StreamWriter(fs)) {
					sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
					sw.WriteLine("<configuration>");
					sw.WriteLine("<appSettings>");
					foreach(string key in ConfigurationManager.AppSettings) {
						string value = ConfigurationManager.AppSettings[key];
						sw.WriteLine(@"<add key=""{0}"" value=""{1}"" />",key,value);
						Debug.WriteLine("{0}={1}",key,value);
					}
					sw.WriteLine("</appSettings>");
					sw.WriteLine("</configuration>");
				}
			}
#endif
		}
		public void SaveSettings() {
			doSet("Range.ofFont",ofFont.ToString());
			doSet("Range.ofPen",ofPen.ToString());
			doSet("LastModified",DateTime.Now.ToString("O"));
			ShowConfig();
		}
		public void SaveConfiguration() {
			//Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			//config.Save(ConfigurationSaveMode.Modified);
		}
	}
}
