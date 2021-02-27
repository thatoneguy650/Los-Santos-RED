using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Fight : ComplexTask
{
    WeaponInformation ToIssue;
    public Fight(IComplexTaskable ped, ITargetable player, WeaponInformation toIssue) : base(player, ped, 0)
    {
        Name = "Fight";
        SubTaskName = "";
        ToIssue = toIssue;
    }
    public override void Start()
    {
        //EntryPoint.WriteToConsole($"TASKER: Fight Start: {Ped.Pedestrian.Handle}");
        Ped.Pedestrian.Inventory.GiveNewWeapon(ToIssue.Hash, ToIssue.AmmoAmount, true);
        Ped.Pedestrian.Tasks.FightAgainst(Player.Character, -1);
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {

    }
    public override void Stop()
    {

    }
}

