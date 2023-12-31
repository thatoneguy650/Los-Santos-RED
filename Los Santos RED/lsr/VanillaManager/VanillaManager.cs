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
    private VanillaSpawnManager VanillaSpawnManager;
    private ISettingsProvideable Settings;
    private uint GameTimeLastRan;

    public VanillaManager(ISettingsProvideable settings, IPlacesOfInterest placesOfInterest, ISpawnBlocks spawnBlocks)
    {
        Settings = settings;
        VanillaWorldManager = new VanillaWorldManager(Settings);
        VanillaCopManager = new VanillaCopManager(Settings);
        VanillaGangManager = new VanillaGangManager(Settings, placesOfInterest);
        VanillaSpawnManager = new VanillaSpawnManager(Settings, spawnBlocks);
    }
    public void Setup()
    {
        VanillaWorldManager.Setup();
        VanillaCopManager.Setup();
        VanillaGangManager.Setup();
        VanillaSpawnManager.Setup();
    }
    public void Dispose()
    {
        VanillaWorldManager.Dispose();
        VanillaCopManager.Dispose();
        VanillaGangManager.Dispose();
        VanillaSpawnManager.Dispose();
    }
    public void Tick()
    {
        if (Game.GameTime - GameTimeLastRan >= 1000)
        {
            VanillaWorldManager.Tick();//might not need at a tick level
            GameFiber.Yield();
            VanillaCopManager.Tick();//not needed at tick
            GameFiber.Yield();
            //VanillaGangManager.Tick();//not needed at tick AT ALL
            //GameFiber.Yield();
            VanillaSpawnManager.Tick();//not needed at tick at ALL
            GameTimeLastRan = Game.GameTime;
        }
    }

}

