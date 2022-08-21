using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OverlayMenuGroup
{
    public OverlayMenuGroup(int overlayID, UIMenuNumericScrollerItem<int> overlayIndexMenu, UIMenuListScrollerItem<ColorLookup> primaryColorMenu, UIMenuListScrollerItem<ColorLookup> secondaryColorMenu, UIMenuNumericScrollerItem<float> opacityMenu)
    {
        OverlayID = overlayID;
        OverlayIndexMenu = overlayIndexMenu;
        PrimaryColorMenu = primaryColorMenu;
        SecondaryColorMenu = secondaryColorMenu;
        OpacityMenu = opacityMenu;
    }

    public int OverlayID { get; set; }
    public UIMenuNumericScrollerItem<int> OverlayIndexMenu { get; set; }
    public UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu { get; set; }
    public UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu { get; set; }
    public UIMenuNumericScrollerItem<float> OpacityMenu { get; set; }
}

