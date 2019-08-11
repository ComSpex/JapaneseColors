using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BorlandOne {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try {
				if(args.Length > 0) {
					// load the config stuff
					if(args[0].ToLower().Trim().Substring(0,2) == "/c") {
						//...
						throw new NotImplementedException();
					} else if(args[0].ToLower() == "/s") // load the screensaver
						{
						//...
					} else if(args[0].ToLower() == "/p") // load the preview
						{
						//...
					}
				} else // there are no arguments...nevertheless, do something!
					{
					//...
				}
				Application.Run(new Form1());
			}catch(Exception ex) {
				MessageBox.Show(ex.ToString(),"Exception!!",MessageBoxButtons.OK,MessageBoxIcon.Stop);
			}
		}
	}
}
