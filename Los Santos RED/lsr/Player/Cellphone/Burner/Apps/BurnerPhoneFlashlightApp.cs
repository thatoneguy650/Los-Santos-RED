using LosSantosRED.lsr.Interface;
using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

public class BurnerPhoneFlashlightApp : BurnerPhoneApp
{
    private IModItems ModItems;
    private ModItem ModItem;

    public BurnerPhoneFlashlightApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index, IModItems modItems) : base(burnerPhone, player, time, settings, index, "Flashlight", 1)
    {
        ModItems = modItems;
    }
    public override void Open(bool Reset)
    {
        BurnerPhone.ClosePhone();
        GameFiber.Sleep(500);
        if(ModItem == null)
        {
            return;
        }
        Player.ActivityManager.UseInventoryItem(ModItem, true);
    }

    public override void Setup()
    {
        string modItemName = "iFruit Cellphone";
        if (Player.CellPhone.PhoneType == 1)
        {
            modItemName = "Facade Cellphone";
        }
        else if (Player.CellPhone.PhoneType == 2)
        {
            modItemName = "Badger Cellphone";
        }
        ModItem = ModItems.Get(modItemName);
    }
}

