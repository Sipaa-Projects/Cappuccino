using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using MUXC = Microsoft.UI.Xaml.Controls;

namespace DesktopInteropTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bool isInHHost = true;

            DispatcherQueueController.CreateOnCurrentThread();
            WindowsXamlManager.InitializeForCurrentThread();

            Window form = new Window();
            Grid grid = new Grid();
            MUXC.Button btn = new() { Content = "Click me!", HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            grid.Children.Add(btn);
            form.Content = grid;

            hwndHostControl1.Child = WinRT.Interop.WindowNative.GetWindowHandle(form);

            form.Activate();
        }
    }
}
