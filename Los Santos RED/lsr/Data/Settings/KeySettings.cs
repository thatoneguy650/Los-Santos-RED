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
    public Keys VehicleKey { get; set; } = Keys.R;
    public KeySettings()
    {

    }
}