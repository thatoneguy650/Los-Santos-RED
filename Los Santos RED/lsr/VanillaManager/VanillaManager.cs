using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


public class VanillaManager
{
    private VanillaGangManager VanillaGangManager;
    private VanillaCopManager VanillaCopManager;
    private VanillaWorldManager VanillaWorldManager;

    private ISettingsProvideable Settings;

    public VanillaManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Settings = settings;
        VanillaWorldManager = new VanillaWorldManager(Settings);
        VanillaCopManager = new VanillaCopManager(Settings);
        VanillaGangManager = new VanillaGangManager(Settings, placesOfInterest);
    }
    public void Setup()
    {
        VanillaWorldManager.Setup();
        VanillaCopManager.Setup();
        VanillaGangManager.Setup();
    }
    public void Dispose()
    {
        VanillaWorldManager.Dispose();
        VanillaCopManager.Dispose();
        VanillaGangManager.Dispose();
    }
    public void Tick()
    {
        VanillaWorldManager.Tick();
        VanillaCopManager.Tick();
        VanillaGangManager.Tick();        
    }

}

