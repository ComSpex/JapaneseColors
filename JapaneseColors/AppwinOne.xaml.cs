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
  public partial class AppwinOne : Window {
    JapaneseColors jc = new JapaneseColors();
    FileInfo file = new FileInfo("ComboBoxItems.txt");
    public AppwinOne() {
      Application.Current.SessionEnding += Current_SessionEnding;
      Application.Current.Exit += Current_Exit;
      InitializeComponent();
      LoadComboItems(file);
      //this.Topmost=true;
      this.Height = 1250;
      this.Width = 820;
      NamedSolidColorBrush.Invert =
        CMYK.Invert = true;
      fillColors();
      fillIndice();
      updateTitle();
      SetLListBrush(exte);
      this.erase.IsEnabled = false;
    }
    private void Current_Exit(object sender, ExitEventArgs e) {
      int exitCode = e.ApplicationExitCode;
      //MessageBox.Show(String.Format("Exit Code : {0}",exitCode));
    }
    private void Current_SessionEnding(object sender, SessionEndingCancelEventArgs e) {
      #if false
      switch (e.ReasonSessionEnding) {
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
      #endif
    }

    private void updateTitle() {
      this.Title = String.Format("Japanese Traditional Colors ({0})", R.Items.Count);
    }
    private void fillColors() {
      Cursor keep = this.Cursor;
      this.Cursor = Cursors.Wait;
      R.Items.Clear();
      foreach (KeyValuePair<string, NamedSolidColorBrush> Core in Jc.Cores) {
        ListBoxItem item = new ListBoxItem();
        item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        item.Content = plateOf(Core);
        item.ToolTip = swatchOf(Core);
        R.Items.Add(item);
      }
      updateTitle();
      clea.IsEnabled = false;
      if (!inclusive) {
        Nscbs.Clear();
      }
      this.Cursor = keep;
    }
    /// <summary>
    /// Descriptive user interface for Content
    /// </summary>
    /// <param name="Core">KeyValuePair<string,NamedSolidColorBrush></param>
    /// <returns>UniformGrid</returns>
    public UIElement plateOf(KeyValuePair<string, NamedSolidColorBrush> Core) {
      return plateOf(Core.Key, Core.Value.Name, Core.Value.Brush);
    }
    /// <summary>
    /// Descriptive user interface core for cloning the Content
    /// </summary>
    /// <param name="key">漢字色名</param>
    /// <param name="name">読み</param>
    /// <param name="brush">色SolidColorBrush</param>
    /// <returns></returns>
    protected virtual UIElement plateOf(string key, string name, Brush brush) {
      SolidColorBrush B = brush as SolidColorBrush;
      CMYK cmyk = new CMYK(B.Color);
      HSL hsl = new HSL(B.Color);
      HSV hsv = new HSV(B.Color);
      TextBlock one = new TextBlock();
      TextBlock two = new TextBlock();
      TextBlock san = new TextBlock();
      TextBlock yon = new TextBlock();
      TextBlock goh = new TextBlock();
      TextBlock txl = new TextBlock();
      TextBlock txv = new TextBlock();
      one.Text = key;
      two.Text = name;
      //san.Text=rgb(B.Color);
      san.Inlines.Add(ofR(B.Color));
      san.Inlines.Add(new Run(","));
      san.Inlines.Add(ofG(B.Color));
      san.Inlines.Add(new Run(","));
      san.Inlines.Add(ofB(B.Color));
      //yon.Text=rgb(B.Color,true);
      yon.Inlines.Add(new Run("#"));
      yon.Inlines.Add(ofR(B.Color, true));
      yon.Inlines.Add(ofG(B.Color, true));
      yon.Inlines.Add(ofB(B.Color, true));
      goh.Text = cmyk.ToString(false);
      txl.Text = hsl.ToString(false);
      txv.Text = hsv.ToString(false);
      switch (NamedSolidColorBrush.howCompare) {
        case NamedSolidColorBrush.HowCompare.Kanji:
          one.FontWeight = FontWeights.Bold;
          break;
        case NamedSolidColorBrush.HowCompare.Yomi:
          two.FontWeight = FontWeights.Bold;
          break;
        case NamedSolidColorBrush.HowCompare.R:
          int i = 0;
          foreach (Inline iline in san.Inlines) {
            if (i == 0) {
              iline.FontWeight = FontWeights.Bold;
              break;
            }
            ++i;
          }
          int yi = 0;
          foreach (Inline iline in yon.Inlines) {
            if (yi == 1) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++yi;
          }
          break;
        case NamedSolidColorBrush.HowCompare.nR:
          int iR = 0;
          foreach (Inline iline in san.Inlines) {
            if (iR == 2 || iR == 4) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++iR;
          }
          int yiR = 0;
          foreach (Inline iline in yon.Inlines) {
            if (yiR == 2 || yiR == 3) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++yiR;
          }
          break;
        case NamedSolidColorBrush.HowCompare.G:
          int ii = 0;
          foreach (Inline iline in san.Inlines) {
            if (ii == 2) {
              iline.FontWeight = FontWeights.Bold;
              break;
            }
            ++ii;
          }
          int iG = 0;
          foreach (Inline iline in yon.Inlines) {
            if (iG == 2) {
              iline.FontWeight = FontWeights.Bold;
              break;
            }
            ++iG;
          }
          break;
        case NamedSolidColorBrush.HowCompare.nG:
          int siG = 0;
          foreach (Inline iline in san.Inlines) {
            if (siG == 0 || siG == 4) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++siG;
          }
          int yiG = 0;
          foreach (Inline iline in yon.Inlines) {
            if (yiG == 1 || yiG == 3) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++yiG;
          }
          break;
        case NamedSolidColorBrush.HowCompare.B:
          int iii = 0;
          foreach (Inline iline in san.Inlines) {
            if (iii == 4) {
              iline.FontWeight = FontWeights.Bold;
              break;
            }
            ++iii;
          }
          int iB = 0;
          foreach (Inline iline in yon.Inlines) {
            if (iB == 3) {
              iline.FontWeight = FontWeights.Bold;
              break;
            }
            ++iB;
          }
          break;
        case NamedSolidColorBrush.HowCompare.nB:
          int siB = 0;
          foreach (Inline iline in san.Inlines) {
            if (siB == 0 || siB == 2) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++siB;
          }
          int yiB = 0;
          foreach (Inline iline in yon.Inlines) {
            if (yiB == 1 || yiB == 2) {
              iline.FontWeight = FontWeights.Bold;
            }
            ++yiB;
          }
          break;
        case NamedSolidColorBrush.HowCompare.RGB:
          san.FontWeight = yon.FontWeight = FontWeights.Bold;
          break;
        case NamedSolidColorBrush.HowCompare.CMYK:
          goh.FontWeight = FontWeights.Bold;
          break;
        case NamedSolidColorBrush.HowCompare.HSL:
          txl.FontWeight = FontWeights.Bold;
          break;
        case NamedSolidColorBrush.HowCompare.HSV:
          txv.FontWeight = FontWeights.Bold;
          break;
      }
      UniformGrid ug = new UniformGrid {
        Background = brush
      };
      ug.Children.Add(one);
      ug.Children.Add(two);
      ug.Children.Add(san);
      ug.Children.Add(yon);
      ug.Children.Add(goh);
      ug.Children.Add(txl);
      ug.Children.Add(txv);
      one.TextAlignment = TextAlignment.Right;
      two.TextAlignment = TextAlignment.Left;
      //san.FontWeight=FontWeights.Bold;
      ug.Columns = ug.Children.Count;
      //ug.HorizontalAlignment=HorizontalAlignment.Stretch;
      one.Foreground = two.Foreground = san.Foreground = yon.Foreground = goh.Foreground = txl.Foreground = txv.Foreground = invert(brush);
      one.Margin = two.Margin = san.Margin = yon.Margin = goh.Margin = txl.Margin = txv.Margin = new Thickness(6);
      ug.Tag = one.Text;
      return ug;
    }
    private string ofB(Color color, bool hex = false) {
      if (hex) {
        return String.Format("{0:X2}", color.B);
      }
      return color.B.ToString();
    }
    private string ofG(Color color, bool hex = false) {
      if (hex) {
        return String.Format("{0:X2}", color.G);
      }
      return color.G.ToString();
    }
    private string ofR(Color color, bool hex = false) {
      if (hex) {
        return String.Format("{0:X2}", color.R);
      }
      return color.R.ToString();
    }
    void fillIndice() {
      List<string> keys = new List<string>();
      string cs = String.Empty;
      foreach (KeyValuePair<string, NamedSolidColorBrush> Core in Jc.Cores) {
        foreach (string top in Core.Value.Names) {
          string one = top.Substring(0, 1);
          if (!keys.Contains(one)) {
            keys.Add(one);
          }
        }
      }
      keys.Sort();
      foreach (string key in keys) {
        cs += key;
      }
      //System.Diagnostics.Debug.WriteLine(cs);
      L.Items.Clear();
      L.Items.Add(ListItem("(全て)"));
      foreach (char c in cs) {
        L.Items.Add(ListItem(c.ToString()));
      }
      L.Items.Add(ListItem("(全て)"));
    }
    private object ListItem(string p) {
      ListBoxItem item = new ListBoxItem();
      TextBlock tb = new TextBlock {
        Text = p,
        FontSize = 14,
        FontFamily = new System.Windows.Media.FontFamily("Meiryo UI")
      };
      if (p.Contains("全て")) {
        tb.Foreground = Brushes.Silver;
      }
      item.Content = tb;
      item.HorizontalContentAlignment = HorizontalAlignment.Center;
      return item;
    }
    /// <summary>
    /// Descriptive user interface for Tooltip
    /// </summary>
    /// <param name="core">KeyValuePair<string,NamedSolidColorBrush></param>
    /// <returns>StackPanel</returns>
    public Panel swatchOf(KeyValuePair<string, NamedSolidColorBrush> core) {
      return swatchOf(core.Value.Show(core.Key), core.Value.Brush);
    }
    protected virtual Panel swatchOf(string kanji, NamedSolidColorBrush nscb) {
      return swatchOf(NamedSolidColorBrush.Show(kanji, nscb.Brush, nscb.Names), nscb.Brush);
    }
    protected virtual Panel swatchOf(string any_note, Brush color) {
      StackPanel sp = new StackPanel();
      sp.Orientation = Orientation.Horizontal;

      Rectangle re = new Rectangle();
      re.Width = re.Height = 16.0;
      re.Fill = color;
      re.Margin = new Thickness(4.0);
      TextBlock tb = new TextBlock();
      tb.VerticalAlignment = VerticalAlignment.Center;
      tb.Text = any_note;

      sp.Children.Add(re);
      sp.Children.Add(tb);

      return sp;
    }
    protected bool inclusive {
      get {
        bool yes = false;
        yes = L.SelectionMode == SelectionMode.Multiple;
        if (L.SelectionMode == SelectionMode.Extended) {
          yes = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }
        return yes;
      }
    }
    private void L_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      ListBox lb = sender as ListBox;
      if (lb.SelectedIndex < 0) {
        return;
      }
      fillColors();
      if (!inclusive) {
        Nscbs.Clear();
      }
      foreach (ListBoxItem item in e.AddedItems) {
        TextBlock tb = item.Content as TextBlock;
        if (tb != null) {
          if (tb.Text.Contains("全て")) {
            ClearList();
            return;
          }
          foreach (ListBoxItem ritem in R.Items) {
            if (ritem == null) { continue; }
            UniformGrid ug = ritem.Content as UniformGrid;
            TextBlock yomi = ug.Children[1] as TextBlock;
            if (yomi.Text.StartsWith(tb.Text)) {
              NamedSolidColorBrush nscb = new NamedSolidColorBrush(ritem);
              if (!Nscbs.Contains(nscb)) {
                Nscbs.Add(nscb);
              }
            }
          }
        }
      }
      R.Items.Clear();
      foreach (NamedSolidColorBrush nscb in Nscbs) {
        R.Items.Add(SetListBoxItem(nscb));
      }
      updateTitle();
      e.Handled = true;
    }
    private void fillColors(List<NamedSolidColorBrush> texs, bool clean = false) {
      Cursor keep = this.Cursor;
      this.Cursor = Cursors.Wait;
      if (clean) {
        R.Items.Clear();
      }
      foreach (NamedSolidColorBrush tex in texs) {
        R.Items.Add(SetListBoxItem(tex));
      }
      if (clean && texs.Count == 0) {
        fillColors();
      }
      updateTitle();
      this.Cursor = keep;
    }
    private void Clear_Click(object sender, RoutedEventArgs e) {
      ClearList();
      partofYomi.Text = String.Empty;
    }
    private void CheckBox_Checked(object sender, RoutedEventArgs e) {
      CheckBox cb = sender as CheckBox;
      if ((string)cb.Content == "Invert") {
        InvertClicked(cb);
        return;
      } else {
        fillColors(Nscbs, true);
      }
      clea.IsEnabled = cb.IsChecked ?? false;
      if (clea.IsEnabled) {
        ClearListR();
      }
    }
    private void InvertClicked(CheckBox cb) {
      CMYK.Invert =
      NamedSolidColorBrush.Invert = cb.IsChecked.Value;
      List<NamedSolidColorBrush> nscbs = new List<NamedSolidColorBrush>();
      foreach (ListBoxItem item in R.Items) {
        NamedSolidColorBrush nscb = new NamedSolidColorBrush(item);
        nscbs.Add(nscb);
      }
      nscbs.Sort();
      R.Items.Clear();
      foreach (NamedSolidColorBrush nscb in nscbs) {
        R.Items.Add(SetListBoxItem(nscb));
      }
    }
    private void ClearListR() {
      R.Items.Clear();
      updateTitle();
      ClearListL();
    }
    private void ClearListL() {
      if (L.SelectionMode == SelectionMode.Multiple) {
        L.SelectedItems.Clear();
      } else {
        L.SelectedIndex = -1;
      }
      searchCount = 0;
    }
    SelectionMode lastMode = SelectionMode.Extended;
    private void RadioButton_Checked(object sender, RoutedEventArgs e) {
      RadioButton rb = sender as RadioButton;
      if (AnyOfGroupMembers(rb)) {
        string name = (string)rb.Content;
        if (name.StartsWith("~")) {
          name = name.Replace("~", "n");
        }
        NamedSolidColorBrush.howCompare = (NamedSolidColorBrush.HowCompare)Enum.Parse(typeof(NamedSolidColorBrush.HowCompare), name);
        List<NamedSolidColorBrush> nscbs = new List<NamedSolidColorBrush>();
        foreach (ListBoxItem item in R.Items) {
          NamedSolidColorBrush nscb = new NamedSolidColorBrush(item);
          if (!nscbs.Contains(nscb)) {
            nscbs.Add(nscb);
          }
        }
        nscbs.Sort();
        R.Items.Clear();
        foreach (NamedSolidColorBrush nscb in nscbs) {
          R.Items.Add(SetListBoxItem(nscb));
        }
        return;
      }
      L.SelectionMode = (SelectionMode)Enum.Parse(typeof(SelectionMode), (string)rb.Content);
      if (lastMode != L.SelectionMode) {
        fillColors();
        lastMode = L.SelectionMode;
      }
      SetLListBrush(rb);
      ClearListL();
    }
    private object SetListBoxItem(NamedSolidColorBrush nscb) {
      ListBoxItem item = new ListBoxItem();
      item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
      item.Content = plateOf(nscb);
      item.ToolTip = swatchOf(nscb);
      return item;
    }
    private UIElement swatchOf(NamedSolidColorBrush nscb) {
      return swatchOf(nscb.Kanji, nscb);
    }
    private UIElement plateOf(NamedSolidColorBrush nscb) {
      return plateOf(nscb.Kanji, nscb.Yomi, nscb.Brush);
    }
    private bool AnyOfGroupMembers(RadioButton rb) {
      List<string> members = new List<string>();
      StackPanel sp = SortGroup.Content as StackPanel;
      foreach (UIElement elem in sp.Children) {
        if (elem is RadioButton) {
          RadioButton ra = elem as RadioButton;
          string name = (string)ra.Content;
          members.Add(name);
        }
      }
      foreach (string member in members) {
        if ((string)rb.Content == member) {
          return true;
        }
      }
      return false;
    }
    private void SetLListBrush(RadioButton rb) {
      SetLListBrush(rb.Background);
      if (rb.Tag != null) {
        L.Foreground = Brushes.Snow;
      }
    }
    private void SetLListBrush(Brush brush) {
      L.Background = brush;
      //L.Foreground=Brushes.Black;//invert(L.Background);
    }
    private string rgb(Color color, bool hex = false) {
      if (hex) {
        return String.Format("#{0,2:X2}{1,2:X2}{2,2:X2}", color.R, color.G, color.B);
      }
      return String.Format("{0:000},{1:000},{2:000}", color.R, color.G, color.B);
    }
    public static Brush invert(Brush brush) {
      SolidColorBrush sc = brush as SolidColorBrush;
      return new SolidColorBrush(Color.FromRgb((byte)~sc.Color.R, (byte)~sc.Color.G, (byte)~sc.Color.B));
    }
    protected override void OnClosing(CancelEventArgs e) {
      foreach (Window win in wins.Values) {
        win.Owner = this;
      }
      foreach (KeyValuePair<string, Child> form in forms) {
        form.Value.Owner = this;
      }
      base.OnClosing(e);
    }
    protected override void OnClosed(EventArgs e) {
      SaveComboItems(file);
      base.OnClosed(e);
    }
    private void LoadComboItems(FileInfo file) {
      if (file.Exists) {
        using (FileStream fs = file.OpenRead()) {
          using (StreamReader sr = new StreamReader(fs)) {
            List<string> items = new List<string>();
            while (!sr.EndOfStream) {
              string buff = sr.ReadLine();
              if (buff.StartsWith("//")) { continue; }
              if (items.Contains(buff)) { continue; }
              items.Add(buff);
              partofYomi.Items.Add(RenderComboBoxItem(buff));
            }
          }
        }
      }
    }
    private object RenderComboBoxItem(string buff) {
      if (partofYomi.IsEditable) {
        return buff;
      }
      Grid gr = new Grid();
      gr.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
      gr.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
      gr.HorizontalAlignment = HorizontalAlignment.Stretch;

      Border b = new Border {
        BorderThickness = new Thickness(1.0),
        Background =
        BorderBrush = new SolidColorBrush(Color.FromRgb(221, 88, 88)),
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
      };
      Grid.SetColumn(b, 1);

      TextBlock tb = new TextBlock {
        Text = buff,
        VerticalAlignment = VerticalAlignment.Center,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        Width = 80,
      };
      Grid.SetColumn(tb, 0);

      TextBlock x = new TextBlock {
        Text = "×",
        FontSize = tb.FontSize * 1.3,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Top,
        Foreground = Brushes.White,
        Tag = buff,
      };
      x.MouseDown += X_MouseDown;
      Grid.SetColumn(x, 1);

      gr.Children.Add(tb);
      gr.Children.Add(b);
      gr.Children.Add(x);

      ComboBoxItem cbi=new ComboBoxItem();
      cbi.Content=gr;
      return cbi;
    }
    private void X_MouseDown(object sender, MouseButtonEventArgs e) {
      TextBlock x = sender as TextBlock;
      SystemSounds.Exclamation.Play();
      MessageBox.Show(String.Format("Target is {0}", x.Tag));
    }
    private void PartofYomi_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      // https://www.wpf-tutorial.com/list-controls/combobox-control/
      #if false
      Cursor keep=this.Cursor;
      this.Cursor=Cursors.Wait;
      R.Items.Clear();
      DoSearch();
      this.Cursor=keep;
      #endif
    }
    private string ParseComboBoxItem(object gr) {
      if (partofYomi.IsEditable) {
        return (string)gr;
      }
      TextBlock tb = ((gr as ComboBoxItem).Content as Grid).Children[0] as TextBlock;
      return tb.Text;
    }
    private void SaveComboItems(FileInfo file) {
      if (partofYomi.Items.Count > 0) {
        List<string> parts = new List<string>();
        foreach (var gr in partofYomi.Items) {
          string part = ParseComboBoxItem(gr);
          if (parts.Contains(part)) { continue; }
          parts.Add(part);
        }
        parts.Sort();
        using (FileStream fs = file.OpenWrite()) {
          using (StreamWriter sw = new StreamWriter(fs)) {
            sw.AutoFlush = true;
            sw.WriteLine("// ComboBox.Items of JapaneseColors as of {0:ddMMMyyyy HH:mm:ss.fff}", DateTime.Now);
            foreach (string item in parts) {
              sw.WriteLine(item);
            }
          }
        }
      }
    }
    bool isTerminated = false;
    public bool IsShiftKeyDown {
      get {
        return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
      }
    }

    public List<NamedSolidColorBrush> Nscbs { get => nscbs; set => nscbs = value; }
    private List<NamedSolidColorBrush> nscbs = new List<NamedSolidColorBrush>();

    public bool Terminate() {
      bool cancelled = true;
      if (!isTerminated) {
        isTerminated = true;
        if (MessageBoxResult.OK == MessageBox.Show("Are you sure to close all the windows?", Title, MessageBoxButton.OKCancel, MessageBoxImage.Question)) {
          Close();
          cancelled = false;
        }
        return cancelled;
      } else {
        return false;
      }
    }
    private void Tile_Click(object sender, RoutedEventArgs e) {
      isTerminated = false;
      TileWindows();
      BringWindowToTop(new WindowInteropHelper(this).EnsureHandle());
      this.erase.IsEnabled = true;
      this.tile.IsEnabled = false;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool BringWindowToTop(IntPtr hWnd);

    private void Erase_Click(object sender, RoutedEventArgs e) {
      foreach (KeyValuePair<string, Child> form in forms) {
        form.Value.Close();
      }
      // Why the top window remains unclosed...
      foreach (KeyValuePair<string, Child> form in forms) {
        form.Value.Close();
      }
      this.tile.IsEnabled = true;
      SystemSounds.Hand.Play();
      this.erase.IsEnabled = false;
    }
    private void Search_Click(object sender, RoutedEventArgs e) {
      DoSearch();
      e.Handled = true;
    }
    int searchCount=0;
    private void DoSearch() {
      if (String.IsNullOrEmpty(partofYomi.Text)) {
        ClearList();
        return;
      }
      ++searchCount;
      ListBoxItem[] items = new ListBoxItem[0];
      if (!inclusive) {
        fillColors();
      } else {
        if (searchCount>=2) {
          items = new ListBoxItem[R.Items.Count];
          R.Items.CopyTo(items,0);
          Nscbs.Clear();
          fillColors();
        }
      }
      foreach (ListBoxItem item in R.Items) {
        if (item == null) { continue; }
        if (!(item.Content is UniformGrid ug)) { continue; }
        if (!(ug.Children[1] is TextBlock hira)) { continue; }
        if (hira.Text.Contains(partofYomi.Text)) {
          NamedSolidColorBrush nscb = new NamedSolidColorBrush(item);
          if (!Nscbs.Contains(nscb)) {
            Nscbs.Add(nscb);
          }
        }
        if (!(ug.Children[0] is TextBlock kanj)) { continue; }
        if (kanj.Text.Contains(partofYomi.Text)) {
          NamedSolidColorBrush nscb = new NamedSolidColorBrush(item);
          if (!Nscbs.Contains(nscb)) {
            Nscbs.Add(nscb);
          }
        }
      }
      R.Items.Clear();
      if (inclusive) {
        foreach (ListBoxItem item in items) {
          R.Items.Add(item);
        }
      }
      foreach (NamedSolidColorBrush nscb in Nscbs) {
        R.Items.Add(SetListBoxItem(nscb));
      }
      updateTitle();
      clea.IsEnabled = R.Items.Count > 0;
      if (clea.IsEnabled) {
        if (!partofYomi.Items.Contains(partofYomi.Text)) {
          partofYomi.Items.Add(partofYomi.Text);
        }
      }
    }
    private void ClearList() {
      ClearListR();
      ClearListL();
      fillColors();
      Nscbs.Clear();
      partofYomi.Text = String.Empty;
    }
  }
}
