using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IMenuProvideable
{
    bool IsDisplayingMenu { get; }
    bool IsPressingActionWheelButton { get; set; }
    void ToggleMenu();
    void ToggleDebugMenu();
    void ToggleAltMenu();
}

