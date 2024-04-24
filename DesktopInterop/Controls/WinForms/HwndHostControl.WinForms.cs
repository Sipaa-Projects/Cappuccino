using Windows.Win32.UI.WindowsAndMessaging;
using static Windows.Win32.PInvoke;

namespace Cappuccino.DesktopInterop.Controls.WinForms;

/// <summary>
/// Hosts an HWND
/// </summary>
public class HwndHostControl : Control
{
    private IntPtr _child;
    private int _childOldStyle;

    /// <summary>
    /// The child HWND
    /// </summary>
    public IntPtr Child
    {
        get => _child;
        set
        {
            if (_child != IntPtr.Zero)
                RestoreWindow(_child);

            if (value != IntPtr.Zero)
                SetWindowAsChild(value);
            
            _child = value;
        }
    }

    public HwndHostControl()
    {
        
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (DesignMode)
            e.Graphics.DrawString("Cannot work in Design mode.", Font, new SolidBrush(ForeColor), 10, 10);
        else if (_child == IntPtr.Zero)
            e.Graphics.DrawString("The child HWND is 0.", Font, new SolidBrush(ForeColor), 10, 10);
    }

    /// <summary>
    /// Removes WS_CHILD, add WS_POPUP and set the parent to NULL for a child window.
    /// </summary>
    /// <param name="hWndChild">The child window</param>
    private void RestoreWindow(IntPtr hWndChild)
    {
        SetParent(new(hWndChild), Windows.Win32.Foundation.HWND.Null);

        unchecked {
            SetWindowLong(new(hWndChild), WINDOW_LONG_PTR_INDEX.GWL_STYLE, _childOldStyle);
        }

        ShowWindow(new(hWndChild), SHOW_WINDOW_CMD.SW_NORMAL);

        //var form = (Form)(Control.FromHandle(hWndChild));
        //form.FormBorderStyle = FormBorderStyle.Sizable;
        //form.WindowState = FormWindowState.Normal;
    }

    /// <summary>
    /// Removes WS_POPUP, add WS_CHILD and set the parent to this HWND for a window.
    /// </summary>
    /// <param name="value"></param>
    private void SetWindowAsChild(nint hWndChild)
    {
        SetParent(new(hWndChild), new(Handle));

        _childOldStyle = GetWindowLong(new(hWndChild), WINDOW_LONG_PTR_INDEX.GWL_STYLE);

        unchecked
        {
            SetWindowLong(new(hWndChild), WINDOW_LONG_PTR_INDEX.GWL_STYLE, (int)(WINDOW_STYLE.WS_CHILD));
        }

        ShowWindow(new(hWndChild), SHOW_WINDOW_CMD.SW_MAXIMIZE);

        //var form = (Form)(Control.FromHandle(hWndChild));
        //form.FormBorderStyle = FormBorderStyle.None;
        //form.WindowState = FormWindowState.Maximized;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);

        if (_child != IntPtr.Zero)
        {
            MoveWindow(new(_child), 0, 0, Width, Height, false);
        }
    }
}
