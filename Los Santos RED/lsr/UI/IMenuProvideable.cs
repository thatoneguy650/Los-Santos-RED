using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IMenuProvideable
{
    bool IsDisplayingMenu { get; }

    void ToggleMenu();
    void ToggleDebugMenu();
    void ToggleMessagesMenu();
    //void DrawWheelMenu();
    //void DisposeWheelMenu();
    void UpdateWheelMenu(bool isPressingActionWheelMenu);
}

