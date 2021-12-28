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
    public Keys SurrenderKeyModifier { get; set; } = Keys.LShiftKey;
    public Keys DropWeaponKey { get; set; } = Keys.G;
    public Keys DropWeaponKeyModifer { get; set; } = Keys.None;
    public Keys RightIndicatorKey { get; set; } = Keys.E;
    public Keys RightIndicatorKeyModifer { get; set; } = Keys.LShiftKey;
    public Keys LeftIndicatorKey { get; set; } = Keys.Q;
    public Keys LeftIndicatorKeyModifer { get; set; } = Keys.LShiftKey;
    public Keys HazardKey { get; set; } = Keys.Space;
    public Keys HazardKeyModifer { get; set; } = Keys.LShiftKey;
    public Keys EngineToggle { get; set; } = Keys.Z;
    public Keys EngineToggleModifier { get; set; } = Keys.LShiftKey;
    public Keys ManualDriverDoorClose { get; set; } = Keys.None;
    public Keys ManualDriverDoorCloseModifier { get; set; } = Keys.LControlKey;
    public Keys SprintKey { get; set; } = Keys.Z;
    public Keys SprintKeyModifier { get; set; } = Keys.None;



    public Keys InteractStart { get; set; } = Keys.O;
    public Keys InteractPositiveOrYes { get; set; } = Keys.J;
    public Keys InteractNegativeOrNo { get; set; } = Keys.K;
    public Keys InteractCancel { get; set; } = Keys.L;
    public Keys ScenarioStart { get; set; } = Keys.P;


    public KeySettings()
    {

    }
}