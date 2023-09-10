using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;

public class DebugSubMenu
{
    protected UIMenu Debug;
    protected MenuPool MenuPool;
    protected UIMenu SubMenu;
    protected IActionable Player;

    public DebugSubMenu(UIMenu debug, MenuPool menuPool, IActionable player)
    {
        Debug = debug;
        MenuPool = menuPool;
        Player = player;
    }
    public virtual void AddItems()
    {

    }

    public virtual void Update()
    {

    }
}

