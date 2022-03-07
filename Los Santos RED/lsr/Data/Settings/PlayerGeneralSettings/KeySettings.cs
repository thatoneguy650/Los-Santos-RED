using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class KeySettings : ISettingsDefaultable
{
    public Keys DebugMenuKey { get; set; }
    public Keys MenuKey { get; set; }
    public Keys SurrenderKey { get; set; }
    public Keys SurrenderKeyModifier { get; set; }
    public Keys DropWeaponKey { get; set; }
    public Keys DropWeaponKeyModifer { get; set; }
    public Keys RightIndicatorKey { get; set; }
    public Keys RightIndicatorKeyModifer { get; set; }
    public Keys LeftIndicatorKey { get; set; }
    public Keys LeftIndicatorKeyModifer { get; set; }
    public Keys HazardKey { get; set; }
    public Keys HazardKeyModifer { get; set; }
    public Keys EngineToggle { get; set; }
    public Keys EngineToggleModifier { get; set; }
    public Keys ManualDriverDoorClose { get; set; }
    public Keys ManualDriverDoorCloseModifier { get; set; }
    public Keys SprintKey { get; set; }
    public Keys SprintKeyModifier { get; set; }
    public Keys InteractStart { get; set; }
    public Keys InteractPositiveOrYes { get; set; }
    public Keys InteractNegativeOrNo { get; set; }
    public Keys InteractCancel { get; set; }
    public Keys ScenarioStart { get; set; }
    public Keys SelectorKey { get; set; }
    public Keys SelectorKeyModifier { get; set; }
    public Keys GestureKey { get; set; }
    public Keys GestureKeyModifier { get; set; }

    public Keys ActivityKey { get; set; }
    public Keys ActivityKeyModifier { get; set; }

    public KeySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        DebugMenuKey = Keys.F11;
        MenuKey = Keys.F10;
        SurrenderKey = Keys.E;
        SurrenderKeyModifier = Keys.LShiftKey;
        DropWeaponKey = Keys.G;
        DropWeaponKeyModifer = Keys.None;
        RightIndicatorKey = Keys.E;
        RightIndicatorKeyModifer = Keys.LShiftKey;
        LeftIndicatorKey = Keys.Q;
        LeftIndicatorKeyModifer = Keys.LShiftKey;
        HazardKey = Keys.Space;
        HazardKeyModifer = Keys.LShiftKey;
        EngineToggle = Keys.Z;
        EngineToggleModifier = Keys.LShiftKey;
        ManualDriverDoorClose = Keys.None;
        ManualDriverDoorCloseModifier = Keys.LControlKey;
        SprintKey = Keys.Z;
        SprintKeyModifier = Keys.None;
        InteractStart = Keys.O;
        InteractPositiveOrYes = Keys.J;
        InteractNegativeOrNo = Keys.K;
        InteractCancel = Keys.L;
        ScenarioStart = Keys.P;
        SelectorKey = Keys.X;
        SelectorKeyModifier = Keys.LShiftKey;


        ActivityKey = Keys.O;
        ActivityKeyModifier = Keys.LShiftKey;

        GestureKey = Keys.X;
        GestureKeyModifier = Keys.Z;

    }
}