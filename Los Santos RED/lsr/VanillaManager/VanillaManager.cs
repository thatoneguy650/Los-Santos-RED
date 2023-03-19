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
    private VanillaSpawnManager VanillaCarGeneratorManager;
    private ISettingsProvideable Settings;

    public VanillaManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
    {
        Settings = settings;
        VanillaWorldManager = new VanillaWorldManager(Settings);
        VanillaCopManager = new VanillaCopManager(Settings);
        VanillaGangManager = new VanillaGangManager(Settings, placesOfInterest);
        VanillaCarGeneratorManager = new VanillaSpawnManager(Settings);
    }
    public void Setup()
    {
        VanillaWorldManager.Setup();
        VanillaCopManager.Setup();
        VanillaGangManager.Setup();
        VanillaCarGeneratorManager.Setup();
    }
    public void Dispose()
    {
        VanillaWorldManager.Dispose();
        VanillaCopManager.Dispose();
        VanillaGangManager.Dispose();
        VanillaCarGeneratorManager.Dispose();
    }
    public void Tick()
    {
        VanillaWorldManager.Tick();
        //GameFiber.Yield();
        VanillaCopManager.Tick();
        //GameFiber.Yield();
        VanillaGangManager.Tick();
        //GameFiber.Yield();
        VanillaCarGeneratorManager.Tick();
    }

}

