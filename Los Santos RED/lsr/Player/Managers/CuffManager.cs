using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CuffManager
{
    private ICuffable Player;
    private ISettingsProvideable Settings;
    private uint GameTimeLastHandcuffed;
    public bool IsHandcuffed { get; private set; }
    public CuffManager(ICuffable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }
    public void Dispose()
    {

    }

    public void Setup()
    {

    }
    public void Reset()
    {
        SetPlayerHandcuffsRemoved();
    }
    public void Update()
    {
        if(IsHandcuffed)
        {
            Player.WeaponEquipment.SetUnarmed();
        }
    }
    public void SetPlayerHandcuffed()
    {
        IsHandcuffed = true;
        GameTimeLastHandcuffed = Game.GameTime;
        EntryPoint.WriteToConsole("PLAYER SET HANDCUFFED");
    }
    public void SetPlayerHandcuffsRemoved()
    {
        IsHandcuffed = false;
        EntryPoint.WriteToConsole("PLAYER SET UNHANDCUFFED");
    }
}
