using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class KeySettings : ISettingsDefaultable
{
    public Keys DebugMenuKey { get; set; }
    public Keys MenuKey { get; set; }
    public Keys SurrenderKey { get; set; }
    public Keys SurrenderKeyModifier { get; set; }
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
    public Keys CrouchKeyModifier { get; set; }
    public Keys CrouchKey { get; set; }
    public Keys SimplePhoneKey { get; set; }
    public Keys SimplePhoneKeyModifer { get; set; }
    public Keys ActionPopUpDisplayKey { get; set; }
    public Keys ActionPopUpDisplayKeyModifier { get; set; }
    public Keys AltActionPopUpDisplayKey { get; set; }
    public Keys AltActionPopUpDisplayKeyModifier { get; set; }


    public Keys VehicleInteract { get; set; }
    public Keys VehicleInteractModifier { get; set; }


    public int ControllerAction { get; set; }
    public int ControllerActionModifier { get; set; }
    public int GameControlToDisable { get; set; }


    public int GrabPedGameControl { get; set; }

    public int HoldUpPedGameControl { get; set; }


    public Keys YellKey { get; set; }
    public Keys YellKeyModifier { get; set; }



    public Keys GroupModeToggleKey { get; set; }
    public Keys GroupModeToggleKeyModifier { get; set; }

    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public KeySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        DebugMenuKey = Keys.F11;
        MenuKey = Keys.F10;
        SimplePhoneKey = Keys.Down;
        SimplePhoneKeyModifer = Keys.LShiftKey;

        SurrenderKey = Keys.E;
        SurrenderKeyModifier = Keys.LShiftKey;

        RightIndicatorKey = Keys.Right;
        RightIndicatorKeyModifer = Keys.LShiftKey;

        LeftIndicatorKey = Keys.Left;
        LeftIndicatorKeyModifer = Keys.LShiftKey;

        HazardKey = Keys.Space;
        HazardKeyModifer = Keys.LShiftKey;

        EngineToggle = Keys.Z;
        EngineToggleModifier = Keys.LShiftKey;

        ManualDriverDoorClose = Keys.LControlKey;
        ManualDriverDoorCloseModifier = Keys.None;

        CrouchKey = Keys.LControlKey;
        CrouchKeyModifier = Keys.None;

        GroupModeToggleKey = Keys.N;
        GroupModeToggleKeyModifier = Keys.LShiftKey;

        YellKey = Keys.L;
        YellKeyModifier = Keys.None;

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

        ActionPopUpDisplayKey = Keys.XButton1;//mouse4
        ActionPopUpDisplayKeyModifier = Keys.None;

        AltActionPopUpDisplayKey = Keys.N;//N
        AltActionPopUpDisplayKeyModifier = Keys.LMenu;//left alt 

        ControllerAction = 236;//189 - frontend left//234 - ScriptPadLeft//217;// 236 - back;
        ControllerActionModifier = -1;//227 - scriptRB//206 - frontenRB

        GameControlToDisable = 0;//Next Cam


        VehicleInteract = Keys.L;
        VehicleInteractModifier = Keys.LShiftKey;
        HoldUpPedGameControl = 46;
        GrabPedGameControl = 46;
    }
}