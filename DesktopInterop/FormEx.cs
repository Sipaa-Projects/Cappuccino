using Microsoft.UI.Xaml;
using WinRT.Interop;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using System.Runtime.InteropServices;

namespace System.Windows.Forms;

/// <summary>
/// An extension to the SWF.Form class, which adds the ToWinUI extension method
/// </summary>
public static class FormEx
{
    public static unsafe Window ToWinUI(this Form form)
    {
        var w = new Window();

        w.Title = form.Text;
        #region Load icon into the new window
        var icp = System.IO.Path.GetTempFileName();

        var fs = File.Open(icp, FileMode.OpenOrCreate);
        form.Icon.Save(fs);
        fs.Close();

        fixed (char* ptr = icp)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(w);
            nint hIcon = PInvoke.LoadImage(HINSTANCE.Null, new PCWSTR(ptr),
                      GDI_IMAGE_TYPE.IMAGE_ICON, 16, 16, IMAGE_FLAGS.LR_LOADFROMFILE);

            PInvoke.SendMessage(new(hwnd), PInvoke.WM_SETICON, new(UIntPtr.Zero), hIcon);
        }
        #endregion

        return w;
    }
}
