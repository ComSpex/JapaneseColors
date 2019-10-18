using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopPelmanism {
  /// <summary>
  /// Interaction logic for A_Card.xaml
  /// </summary>
  public partial class A_Card : UserControl {
    Point anchorPoint;
    Point currentPoint;
    bool isInDrag = false;
    private TranslateTransform tf = new TranslateTransform();
    private RotateTransform rt = new RotateTransform();
    private TransformGroup tg = new TransformGroup();
    public A_Card() {
      InitializeComponent();
      rt.CenterX = root.ActualWidth / 2;
      rt.CenterY = root.ActualHeight / 2;
    }
    protected override void OnMouseWheel(MouseWheelEventArgs e) {
      //base.OnMouseWheel(e);
      if (!isInDrag) {
        if (e.Delta > 0) {
          rt.Angle -= 5.0;
        }
        if (e.Delta < 0) {
          rt.Angle += 5.0;
        }
        if (!tg.Children.Contains(rt)) {
          tg.Children.Add(rt);
          this.RenderTransform= rt;
        }
      }
    }
    private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
      var element = sender as FrameworkElement;
      anchorPoint = e.GetPosition(null);
      element.CaptureMouse();
      isInDrag = true;
      tg.Children.Clear();
      e.Handled = true;
    }
    private void root_MouseMove(object sender, MouseEventArgs e) {
      if (isInDrag) {
        var element = sender as FrameworkElement;
        currentPoint = e.GetPosition(null);
        tf.X += currentPoint.X - anchorPoint.X;
        tf.Y += (currentPoint.Y - anchorPoint.Y);
        if (!tg.Children.Contains(tf)) {
          this.RenderTransform = tf;
          anchorPoint = currentPoint;
        }
      }
    }
    private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
      if (isInDrag) {
        var element = sender as FrameworkElement;
        element.ReleaseMouseCapture();
        isInDrag = false;
        e.Handled = true;
      }
    }
  }
}
