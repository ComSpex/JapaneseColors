namespace TileWindows {
	partial class Child {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			//this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			//this.timerInterval = new System.Windows.Forms.Label();
			this.Background = new System.Windows.Forms.Label();
			this.fontFamily = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// elementHost1
			// 
			this.elementHost1.BackColorTransparent = true;
			this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.elementHost1.Location = new System.Drawing.Point(0, 0);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(584, 261);
			this.elementHost1.TabIndex = 0;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = null;
#if false
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 206);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Timer Interval : ";
#endif
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 223);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "BackColor : ";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 239);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "FontFamily : ";
#if false
			// 
			// timerInterval
			// 
			this.timerInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.timerInterval.AutoSize = true;
			this.timerInterval.Location = new System.Drawing.Point(94, 206);
			this.timerInterval.Name = "timerInterval";
			this.timerInterval.Size = new System.Drawing.Size(65, 13);
			this.timerInterval.TabIndex = 4;
			this.timerInterval.Text = "computing...";
#endif
			// 
			// Background
			// 
			this.Background.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Background.AutoSize = true;
			this.Background.Location = new System.Drawing.Point(94, 223);
			this.Background.Name = "Background";
			this.Background.Size = new System.Drawing.Size(65, 13);
			this.Background.TabIndex = 5;
			this.Background.Text = "computing...";
			// 
			// fontFamily
			// 
			this.fontFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.fontFamily.AutoSize = true;
			this.fontFamily.Location = new System.Drawing.Point(94, 239);
			this.fontFamily.Name = "fontFamily";
			this.fontFamily.Size = new System.Drawing.Size(65, 13);
			this.fontFamily.TabIndex = 6;
			this.fontFamily.Text = "computing...";
			// 
			// Child
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(584, 261);
			this.ControlBox = false;
			this.Controls.Add(this.fontFamily);
			this.Controls.Add(this.Background);
			//this.Controls.Add(this.timerInterval);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			//this.Controls.Add(this.label1);
			this.Controls.Add(this.elementHost1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Child";
			this.Text = "Child";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

#endregion

		private System.Windows.Forms.Integration.ElementHost elementHost1;
		//private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		//private System.Windows.Forms.Label timerInterval;
		private System.Windows.Forms.Label Background;
		private System.Windows.Forms.Label fontFamily;
	}
}