using Cappuccino.LibDX.Model;
using DirectN;
using System.ComponentModel;
using System.Drawing;

namespace Cappuccino.LibDX;

/// <summary>
/// Defines the base class for hybrid control with Direct2D and GDI+ graphics support.
/// </summary>
public class D2DCanvas : Control
{
    protected readonly IComObject<ID2D1Factory1> _d2DFactory = D2D1Functions.D2D1CreateFactory<ID2D1Factory1>(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED);
    protected readonly IComObject<IDWriteFactory5> _dWriteFactory = DWriteFunctions.DWriteCreateFactory<IDWriteFactory5>(DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED);
    protected IComObject<ID2D1HwndRenderTarget>? _renderTarget;
    protected IComObject<ID2D1DeviceContext6>? _device;
    protected float _dpi = 96.0f;
    protected bool _firstPaintBackground = true;

    public event EventHandler<D2DRenderEventArgs> D2DRender;

    /// <summary>
    /// Gets, sets the DPI for drawing when using <see cref="D2DGraphics"/>.
    /// </summary>
    [Browsable(false)]
    public float BaseDpi
    {
        get => _dpi;
        set
        {
            if (_device == null) return;

            _dpi = value;
            _device.Object.SetDpi(_dpi, _dpi);
        }
    }

    public D2DCanvas()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();

        DoubleBuffered = false;

        if (_renderTarget == null || _device == null)
        {
            DisposeDevice();
            CreateDevice();
        }
    }

    protected override void DestroyHandle()
    {
        base.DestroyHandle();

        _d2DFactory.Dispose();
        _dWriteFactory.Dispose();

        DisposeDevice();
    }


    protected override void WndProc(ref Message m)
    {
        const int WM_SIZE = 0x0005;
        const int WM_ERASEBKGND = 0x0014;
        const int WM_DESTROY = 0x0002;

        switch (m.Msg)
        {
            case WM_ERASEBKGND:

                // to fix background is delayed to paint on launch
                if (_firstPaintBackground)
                {
                    _firstPaintBackground = false;
                    _device?.BeginDraw();
                    _device?.Clear(DXHelper.FromColor(BackColor));
                    _device?.EndDraw();
                }
                break;

            case WM_SIZE:
                base.WndProc(ref m);

                _renderTarget?.Resize(new((uint)ClientSize.Width, (uint)ClientSize.Height));
                break;

            case WM_DESTROY:
                _d2DFactory.Dispose();
                _dWriteFactory.Dispose();
                DisposeDevice();

                base.WndProc(ref m);
                break;

            default:
                base.WndProc(ref m);
                break;
        }
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (DesignMode) return;

        _renderTarget?.Resize(new((uint)ClientSize.Width, (uint)ClientSize.Height));

        // update the control once size/windows state changed
        ResizeRedraw = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        DoubleBuffered = false;

        _device.BeginDraw();
        _device.Clear(DXHelper.FromColor(BackColor));
        OnD2DRender(_device, _d2DFactory, _dWriteFactory);
        _device.EndDraw();
    }

    #region Events
    
    protected virtual void OnD2DRender(IComObject<ID2D1DeviceContext6> dev, IComObject<ID2D1Factory1> d2d, IComObject<IDWriteFactory5> dw)
    {
        if (D2DRender != null)
            D2DRender(this, new(dev, d2d, dw));
    }

    #endregion

    #region Device-specific code

    /// <summary>
    /// Initializes value for <see cref="Device"/>, <see cref="RenderTarget"/> and <see cref="D2Graphics"/>.
    /// </summary>
    public void CreateDevice()
    {
        var renderTargetProps = new D2D1_RENDER_TARGET_PROPERTIES()
        {
            dpiX = _dpi,
            dpiY = _dpi,
            type = D2D1_RENDER_TARGET_TYPE.D2D1_RENDER_TARGET_TYPE_DEFAULT,
            usage = D2D1_RENDER_TARGET_USAGE.D2D1_RENDER_TARGET_USAGE_NONE,
            minLevel = D2D1_FEATURE_LEVEL.D2D1_FEATURE_LEVEL_DEFAULT,
            pixelFormat = new D2D1_PIXEL_FORMAT()
            {
                alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED,
                format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
            },
        };

        var hwndRenderTargetProps = new D2D1_HWND_RENDER_TARGET_PROPERTIES()
        {
            hwnd = Handle,
            pixelSize = new D2D_SIZE_U((uint)Width, (uint)Height),
            presentOptions = D2D1_PRESENT_OPTIONS.D2D1_PRESENT_OPTIONS_IMMEDIATELY,
        };

        _renderTarget = _d2DFactory.CreateHwndRenderTarget(hwndRenderTargetProps, renderTargetProps);

        _renderTarget.Object.SetAntialiasMode(D2D1_ANTIALIAS_MODE.D2D1_ANTIALIAS_MODE_PER_PRIMITIVE);
        _renderTarget.Object.SetDpi(_dpi, _dpi);
        _renderTarget.Resize(new((uint)ClientSize.Width, (uint)ClientSize.Height));

        _device = _renderTarget.AsComObject<ID2D1DeviceContext6>();
    }

    /// <summary>
    /// Dispose <see cref="Device"/> and <see cref="RenderTarget"/> objects.
    /// </summary>
    public void DisposeDevice()
    {
        _device?.Dispose();
        _device = null;

        _renderTarget?.Dispose();
        _renderTarget = null;

        GC.Collect();
    }

    #endregion
}