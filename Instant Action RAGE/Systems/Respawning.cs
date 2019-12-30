using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Respawning
{
    public static void BribePolice(int Amount)
    {
        if (Game.LocalPlayer.Character.GetCash() < Amount)
            return;

        if (Amount < Police.PreviousWantedLevel * Settings.PoliceBribeWantedLevelScale)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount);
            return;
        }
        else
        {
            InstantAction.BeingArrested = false;
            InstantAction.IsBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount);
        }

        Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        ResetPlayer(true, false);
    }
    public static void RespawnAtHospital(Location Hospital)
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        bool prePlayerKilledPolice = Police.PlayerKilledPolice;

        InstantAction.IsDead = false;
        InstantAction.IsBusted = false;

        int HospitalFee = Settings.HospitalFee * (1+InstantAction.MaxWantedLastLife);
        int OfficerFee = 0;
        if (prePlayerKilledPolice)
            OfficerFee = HospitalFee * Police.CopsKilledByPlayer;

        HospitalFee += OfficerFee;

        RespawnInPlace(false);

        if (Hospital == null)
            Hospital = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Hospital);

        Game.LocalPlayer.Character.Position = Hospital.LocationPosition;
        Game.LocalPlayer.Character.Heading = Hospital.Heading;

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);

        if(!prePlayerKilledPolice)
            Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in Hospital fees.", HospitalFee));
        else
            Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in Hospital fees and ~r~${1} ~s~in ~o~Officers funeral expenses~s~", HospitalFee, OfficerFee));

        Game.LocalPlayer.Character.GiveCash(-1 * HospitalFee);
    }
    public static void ResistArrest()
    {
        InstantAction.IsBusted = false;
        InstantAction.BeingArrested = false;
        InstantAction.HandsAreUp = false;
        Police.CurrentPoliceState = Police.PoliceState.DeadlyChase;
        Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(false, false);
        Tasking.UntaskAll(true);
    }
    public static void Surrender(Location PoliceStation)
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        bool prePlayerKilledPolice = Police.PlayerKilledPolice;
        int bailMoney = InstantAction.MaxWantedLastLife * Settings.PoliceBailWantedLevelScale;

        InstantAction.BeingArrested = false;
        InstantAction.IsBusted = false;

        Surrendering.RaiseHands();
        ResetPlayer(true, true);

        if (PoliceStation == null)
            PoliceStation = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Police);

        Game.LocalPlayer.Character.Position = PoliceStation.LocationPosition;
        Game.LocalPlayer.Character.Heading = PoliceStation.Heading;

        Game.LocalPlayer.Character.Tasks.ClearImmediately();

        if (!prePlayerKilledPolice)//the actual gets changed and i want to run this after you have transitioned 
        {
            RemoveIllegalWeapons();
        }
        else
        {
            Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        }

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);

        bool DABlewTheCase = InstantAction.MyRand.Next(1, 11) == 1;
        if (!DABlewTheCase)
        {
            Game.DisplayNotification(string.Format("You got out with only ~r~${0} ~s~in bail money, try to stay out of trouble.", bailMoney));
            Game.LocalPlayer.Character.GiveCash(-1 * bailMoney);
        }
        else
        {
            Game.DisplayNotification(string.Format("The DA Blew the case, you got off! Saved ~g~${0} ~s~in bail money", bailMoney));
        }
    }
    public static void UnDie()
    {
        RespawnInPlace(true);
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        InstantAction.IsDead = false;
        InstantAction.IsBusted = false;
        InstantAction.BeingArrested = false;

        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
        {
            PersonOfInterest.ResetPersonOfInterest(false);
            Police.ResetPoliceStats();
            Police.SetWantedLevel(0,"Reset player with Clear Wanted");
            InstantAction.MaxWantedLastLife = 0;  
            NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        }

        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 200;

        NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);

        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            InstantAction.IsDead = false;
            InstantAction.IsBusted = false;
            InstantAction.BeingArrested = false;
            Game.LocalPlayer.Character.Health = 100;
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
            if (InstantAction.DiedInVehicle)
            {
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                if (Game.LocalPlayer.Character.LastVehicle.Exists() && Game.LocalPlayer.Character.LastVehicle.IsDriveable)
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                InstantAction.DiedInVehicle = false;
            }
            else
            {
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
            }
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            if (AsOldCharacter)
            {
                ResetPlayer(false, false);
                Police.SetWantedLevel(InstantAction.MaxWantedLastLife,"Resetting to max wanted last life after respawn in place");
                ++InstantAction.TimesDied;
            }
            else
            {
                ResetPlayer(true, true);
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                InstantAction.LastWeapon = 0;
                Police.PreviousWantedLevel = 0;
                InstantAction.TimesDied = 0;
                InstantAction.MaxWantedLastLife = 0;
            }
            Game.HandleRespawn();
            DispatchAudio.AbortAllAudio();
        }
        catch (Exception e)
        {
            Game.LogTrivial(e.Message);
        }
    }
    public static void RemoveIllegalWeapons()
    {
        //Needed cuz for some reason the other weapon list just forgets your last gun in in there and it isnt applied, so until I can find it i can only remove all
        //Make a list of my old guns
        List<DroppedWeapon> MyOldGuns = new List<DroppedWeapon>();
        WeaponDescriptorCollection CurrentWeapons = Game.LocalPlayer.Character.Inventory.Weapons;
        foreach (WeaponDescriptor Weapon in CurrentWeapons)
        {
            WeaponVariation DroppedGunVariation = InstantAction.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Weapon.Hash);
            DroppedWeapon MyGun = new DroppedWeapon(Weapon, Vector3.Zero, DroppedGunVariation,Weapon.Ammo);
            MyOldGuns.Add(MyGun);
        }
        //Totally clear our guns
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        //Add out guns back with variations
        foreach (DroppedWeapon MyNewGun in MyOldGuns)
        {
            GTAWeapon MyGTANewGun = GTAWeapons.GetWeaponFromHash((ulong)MyNewGun.Weapon.Hash);
            if (MyGTANewGun == null || MyGTANewGun.IsLegal)//or its an addon gun
            {
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(MyNewGun.Weapon.Hash, (short)MyNewGun.Ammo, false);
                InstantAction.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Variation);
                NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Ammo + 1);
            }
        }
    }
}

