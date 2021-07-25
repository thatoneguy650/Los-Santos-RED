using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class KeySettings
{
    public Keys DebugMenuKey { get; set; } = Keys.F11;
    public Keys MenuKey { get; set; } = Keys.F10;
    public Keys SurrenderKey { get; set; } = Keys.E;
    public Keys DropWeaponKey { get; set; } = Keys.G;
    public Keys RightIndicatorKey { get; set; } = Keys.E;
    public Keys LeftIndicatorKey { get; set; } = Keys.Q;
    public Keys HazardKey { get; set; } = Keys.Space;
    public Keys EngineToggle { get; set; } = Keys.X;
    public KeySettings()
    {

    }
}