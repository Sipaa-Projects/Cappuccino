using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Cappuccino.CafePaint;

public sealed partial class MainWindow : Window
{
    AppWindow m_AppWindow;

    public MainWindow()
    {
        this.InitializeComponent();

        m_AppWindow = this.AppWindow;
        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        ExtendsContentIntoTitleBar = true;
        try
        {
            TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;
        }
        catch (COMException) // happens when the app isn't packaged, in this case we set to the window title
        {
            TitleBarTextBlock.Text = Title;
        }
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar == true)
        {
            // Set the initial interactive regions.
            SetRegionsForCustomTitleBar();
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar == true)
        {
            // Update interactive regions if the size of the window changes.
            SetRegionsForCustomTitleBar();
        }
    }

    // Allow input on menu bar
    private void SetRegionsForCustomTitleBar()
    {
        // Specify the interactive regions of the title bar.

        double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        RightPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(m_AppWindow.TitleBar.LeftInset / scaleAdjustment);

        GeneralTransform transform = TitleBarMenu.TransformToVisual(null);
        Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                         TitleBarMenu.ActualWidth,
                                                         TitleBarMenu.ActualHeight));
        Windows.Graphics.RectInt32 MenuBarRect = GetRect(bounds, scaleAdjustment);

        var rectArray = new Windows.Graphics.RectInt32[] { MenuBarRect };

        InputNonClientPointerSource nonClientInputSrc =
            InputNonClientPointerSource.GetForWindowId(this.AppWindow.Id);
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }

    private Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
    {
        return new Windows.Graphics.RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );
    }

    // Painting
    bool penDown = false;
    Point oldPoint;

    InkStrokeBuilder inkStrokeBuilder = new InkStrokeBuilder();
    List<InkStroke> strokes = new List<InkStroke>();
    List<Point> currentStrokePoints;
    Windows.UI.Color currentColor;

    private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;

        if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse ||
            e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Pen)
        {
            penDown = true;
            var pnt = e.GetCurrentPoint(DrawCanvas);
            oldPoint = pnt.Position;
        }

        currentStrokePoints = new List<Point>();
        currentStrokePoints.Add(oldPoint);
    }

    private void DrawCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!penDown) return;

        var pnt = e.GetCurrentPoint(DrawCanvas);

        Line line = new Line();
        line.X1 = oldPoint.X;
        line.Y1 = oldPoint.Y;
        line.X2 = pnt.Position.X;
        line.Y2 = pnt.Position.Y;
        line.Stroke = new SolidColorBrush(currentColor);
        line.StrokeThickness = 2 * pnt.Properties.Pressure;

        DrawCanvas.Children.Add(line);

        oldPoint = pnt.Position;

        currentStrokePoints.Add(pnt.Position);
    }

    private void DrawCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        penDown = false;
        currentStrokePoints.Add(e.GetCurrentPoint(DrawCanvas).Position);
        strokes.Add(inkStrokeBuilder.CreateStroke(currentStrokePoints));
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        canvasSv.ZoomTo((float)e.NewValue, null, new(ScrollingAnimationMode.Enabled, ScrollingSnapPointsMode.Ignore));
    }

    private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
    {
        currentColor = args.NewColor;
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        new AboutWindow().Activate();
    }
}
