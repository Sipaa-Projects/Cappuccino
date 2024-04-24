using DirectN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cappuccino.LibDX.Model
{
    public class D2DRenderEventArgs : EventArgs
    {
        public readonly IComObject<ID2D1Factory1> Direct2DFactory = D2D1Functions.D2D1CreateFactory<ID2D1Factory1>(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED);
        public readonly IComObject<IDWriteFactory5> DirectWriteFactory = DWriteFunctions.DWriteCreateFactory<IDWriteFactory5>(DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED);
        public IComObject<ID2D1DeviceContext6>? Device;

        internal D2DRenderEventArgs(IComObject<ID2D1DeviceContext6>? dev, IComObject<ID2D1Factory1> d2df, IComObject<IDWriteFactory5> dwf)
        {
            Direct2DFactory = d2df;
            DirectWriteFactory = dwf;
            Device = dev;
        }
    }
}
