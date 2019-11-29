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
        if (Game.LocalPlayer.Character.GetCash(Settings.MainCharacterToAlias) < Amount)
            return;

        if (Amount < Police.PreviousWantedLevel * Settings.PoliceBribeWantedLevelScale)
        {
            Game.DisplayNotification("Thats it? Thanks for the cash, but you're going downtown.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
            return;
        }
        else
        {
            InstantAction.BeingArrested = false;
            InstantAction.IsBusted = false;
            Game.DisplayNotification("Thanks for the cash, now beat it.");
            Game.LocalPlayer.Character.GiveCash(-1 * Amount, Settings.MainCharacterToAlias);
        }
        Police.PlayerIsPersonOfInterest = false;
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(true, false);
    }
    public static void RespawnAtHospital(Location Hospital)
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);
        InstantAction.IsDead = false;
        InstantAction.IsBusted = false;

        Police.CurrentPoliceState = Police.PoliceState.Normal;
        Police.PlayerIsPersonOfInterest = false;
        ResetPlayer(true, true);

        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        InstantAction.LastWeapon = 0;
        RespawnInPlace(false);
        Police.SetWantedLevel(0);
        if (Hospital == null)
            Hospital = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Hospital);

        Game.LocalPlayer.Character.Position = Hospital.LocationPosition;
        Game.LocalPlayer.Character.Heading = Hospital.Heading;

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in Hospital fees.", Settings.HospitalFee));
        Game.LocalPlayer.Character.GiveCash(-1 * Settings.HospitalFee, Settings.MainCharacterToAlias);
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

        int bailMoney = InstantAction.MaxWantedLastLife * Settings.PoliceBailWantedLevelScale;
        InstantAction.BeingArrested = false;
        InstantAction.IsBusted = false;
        Police.SetWantedLevel(0);
        Police.PlayerIsPersonOfInterest = false;
        Surrendering.RaiseHands();
        ResetPlayer(true, true);
        NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        if(PoliceStation == null)
            PoliceStation = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Police);

        Game.LocalPlayer.Character.Position = PoliceStation.LocationPosition;
        Game.LocalPlayer.Character.Heading = PoliceStation.Heading;

        Game.LocalPlayer.Character.Tasks.ClearImmediately();
        Game.LocalPlayer.Character.Inventory.Weapons.Clear();
        Game.LocalPlayer.Character.Inventory.GiveNewWeapon((WeaponHash)2725352035, -1, true);
        InstantAction.LastWeapon = 0;
        ResetPlayer(true, true);
        Police.CurrentPoliceState = Police.PoliceState.Normal;
        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);
        Game.DisplayNotification(string.Format("You have been charged ~r~${0} ~s~in bail money, try to stay out of trouble.", bailMoney));
        Game.LocalPlayer.Character.GiveCash(-1 * bailMoney, Settings.MainCharacterToAlias);
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
            Police.SetWantedLevel(0);
            Police.ResetPoliceStats();
            TrafficViolations.ResetTrafficViolations();
            Police.ResetPersonOfInterest();
        }

        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 100;
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            InstantAction.IsDead = false;
            InstantAction.IsBusted = false;
            InstantAction.BeingArrested = false;
            Game.LocalPlayer.Character.Health = 100;
            if (InstantAction.DiedInVehicle)
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
                if (Game.LocalPlayer.Character.LastVehicle.Exists() && Game.LocalPlayer.Character.LastVehicle.IsDriveable)
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            else
            {
                NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY"
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            }
            if (AsOldCharacter)
            {
                Police.SetWantedLevel(InstantAction.MaxWantedLastLife);
                ++InstantAction.TimesDied;
            }
            else
            {
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                Game.LocalPlayer.Character.Inventory.GiveNewWeapon(2725352035, 0, true);
                InstantAction.LastWeapon = 0;
                Police.PreviousWantedLevel = 0;
                Police.SetWantedLevel(0);
                InstantAction.TimesDied = 0;
                InstantAction.MaxWantedLastLife = 0;
                Police.ResetPoliceStats();
                DispatchAudio.ResetReportedItems();
                Police.ResetPersonOfInterest();

            }
            Game.TimeScale = 1f;
            InstantAction.DiedInVehicle = false;
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS
            ResetPlayer(false, false);
            Game.HandleRespawn();
            NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
        }
        catch (Exception e)
        {
            Game.LogTrivial(e.Message);
        }
    }
}

