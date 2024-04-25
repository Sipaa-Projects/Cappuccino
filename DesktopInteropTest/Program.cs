using Microsoft.UI.Dispatching;
using WinRT;

namespace DesktopInteropTest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                HwndHostTest();
            }
            else
            {
                switch (args[0])
                {
                    case "hwndhost": 
                        HwndHostTest();
                        break;
                    case "wuiforms":
                        WinUIFormsTest();
                        break;
                    default:
                        HwndHostTest();
                        break;
                };
            }
        }

        static void WinUIFormsTest()
        {
            ComWrappersSupport.InitializeComWrappers();
            Microsoft.UI.Xaml.Application.Start(delegate
            {
                DispatcherQueueSynchronizationContext synchronizationContext = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(synchronizationContext);
                new WinUIFormsApp();
            });
        }

        static void HwndHostTest()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}