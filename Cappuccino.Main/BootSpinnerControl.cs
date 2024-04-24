using System.ComponentModel;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace Cappuccino.Main;

public enum BootSpinnerTheme
{
    Windows8,
    Windows11
}

public partial class BootSpinnerControl : Label
{
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public new string Text { get; set; }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public new Font Font { get; set; }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public new bool UseCompatibleTextRendering { get; set; }

    public BootSpinnerTheme Theme
    {
        get => bst;
        set
        {
            bst = value;
            Reset();
        }
    }

    BootSpinnerTheme bst = BootSpinnerTheme.Windows8;
    PrivateFontCollection pfc = new PrivateFontCollection();
    char Win8BootSpinnerStart = '\ue052';
    char Win8BootSpinnerEnd = '\ue0cb';
    char Win11BootSpinnerStart = '\ue100';
    char Win11BootSpinnerEnd = '\ue176';
    Timer t;
    int W8WaitInterval = 0;

    public void Reset()
    {
        if (bst == BootSpinnerTheme.Windows8)
            base.Text = $"{Win8BootSpinnerStart}";
        else
            base.Text = $"{Win11BootSpinnerStart}";
    }

    public BootSpinnerControl()
    {
        int fontLength = Properties.Resources.segoe_slboot.Length;
        byte[] fontdata = Properties.Resources.segoe_slboot;
        System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
        Marshal.Copy(fontdata, 0, data, fontLength);
        pfc.AddMemoryFont(data, fontLength);

        base.UseCompatibleTextRendering = true;
        base.Font = new(pfc.Families[0], 24f);
        Reset();

        t = new();
        t.Interval = 25;
        t.Tick += (s, e) =>
        {
            try
            { 
                char c = ' ';
                if (base.Text.Length > 0)
                    c = base.Text[0];

                if (bst == BootSpinnerTheme.Windows8)
                    if (c == Win8BootSpinnerEnd && W8WaitInterval == 4) // 4 = 100 / 25
                    {
                        c = Win8BootSpinnerStart;
                        W8WaitInterval = 0;
                    }
                    else if (c == Win8BootSpinnerEnd)
                        W8WaitInterval++;
                    else
                        c++;
                else
                    if (c == Win11BootSpinnerEnd)
                        c = Win11BootSpinnerStart;
                    else
                        c++;

                base.Text = $"{c}";
            }
            catch { }
        };
        t.Start();
    }
}