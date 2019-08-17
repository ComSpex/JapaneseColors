using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using FontFamily = System.Drawing.FontFamily;
using Label = System.Windows.Forms.Label;
using FormTimer = System.Windows.Forms.Timer;
using System.Media;

namespace TileWindows {
	public partial class Child:ChildBase{
		TextBlock tb = new TextBlock();
		Random rand = new Random();
		FormTimer tick = new FormTimer();
		bool isUseNamedColor = true;
		public Child() {
			InitializeComponent();
			CollectColors();
			Viewbox vb = new Viewbox();
			vb.Child=tb;
			elementHost1.Child=vb;
			RecollectFontFamilies();
			tick.Interval=rand.Next(5000,10000);
			tick.Tick+=Tick_Tick;
		}
		public Child(Form boss,string title)
			: this() {
			this.Owner=boss;
			this.Text=title;
		}
		protected override void OnActivated(EventArgs e) {
			base.OnActivated(e);
			// Do work 100%!!
			Form1 boss = this.Owner as Form1;
			boss.Select(this);
			ToggleTimer();
			SystemSounds.Asterisk.Play();
		}
		protected override void OnDeactivate(EventArgs e) {
			base.OnDeactivate(e);
			ToggleTimer();
		}
		protected override void OnShown(EventArgs e) {
			base.OnShown(e);
			tick.Start();
		}
		bool toggle = false;
		public override string Tooltip {
			get { return (string)(elementHost1.Child as Viewbox).ToolTip; }
		}
		CancellationToken cancel = CancellationToken.None;
		bool first = true;
		private void Tick_Tick(object sender,EventArgs e) {
			//MainTick(sender);
			if(!first) { return; }
			first=true;
			DoTaskAsync();
		}
		private async Task DoTaskAsync() {
			await DoUIThreadWorkAsync(cancel);
		}
		async Task DoUIThreadWorkAsync(CancellationToken token) {
			Func<Task> idleYield = async () => await Dispatcher.Yield(DispatcherPriority.ApplicationIdle);
			var cancellationTcs = new TaskCompletionSource<bool>();
			using(token.Register(() => cancellationTcs.SetCanceled(),useSynchronizationContext: true)) {
				while(true) {
					await Task.Delay(100,token);
					await Task.WhenAny(idleYield(),cancellationTcs.Task);
					token.ThrowIfCancellationRequested();

					// do the UI-related work
					MainTick(tick);
				}

			}
		}

		public override void MainTick(object sender) {
			if(toggle) {
				return;
			}
			FormTimer time = sender as FormTimer;
			tb.Text=DateTime.Now.ToString("HH:mm:ss.fff");
			string fontName = ffs[rand.Next(0,ffs.Length-1)].Name;
			tb.FontFamily=new System.Windows.Media.FontFamily(fontName);
			Color color = Color.Empty;
			if(isUseNamedColor) {
				color=keys[rand.Next(0,keys.Count-1)];
			} else {
				color=Color.FromArgb(rand.Next(0,255),rand.Next(0,255),rand.Next(0,255));
			}
			this.BackColor=color;
			tb.Foreground=Invert(color);
			string colorName = String.Empty;
			if(isUseNamedColor) {
				colorName=dics[color];
			} else {
				colorName=ToColorName(color);
				if(String.IsNullOrEmpty(colorName)) {
					colorName=ToString(color);
				} else {
					colorName=String.Format("{0}({1})",colorName,ToString(color));
				}
			}
			//this.timerInterval.Text=String.Format("{0,4}",time.Interval);
			this.Background.Text=String.Format("{0}",colorName);
			this.fontFamily.Text=String.Format("{0}",fontName);
			if(isUseNamedColor) {
				(elementHost1.Child as Viewbox).ToolTip=String.Format("{2}({1}) {0}",fontName,colorName,time.Interval);
			} else {
				(elementHost1.Child as Viewbox).ToolTip=String.Format("{2}({1}) {0}",fontName,ToString(BackColor),time.Interval);
			}
		}
		protected override void ModifyControls(Color cc) {
			base.ModifyControls(cc);
			foreach(Control con in this.Controls) {
				if(con is Label) {
					Label la = con as Label;
					la.ForeColor=cc;
				}
			}
		}
		public override void ToggleTimer() {
			toggle=!toggle;
			if(!toggle) {
				Cursor=Cursors.Hand;
				tick.Start();
			} else {
				tick.Stop();
				Cursor=Cursors.WaitCursor;
			}
		}
	}
}
