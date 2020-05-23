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
    private static uint GameTimeLastBribedPolice;
    private static uint GameTimeLastUndied;
    private static int HospitalBillPastDue;
    private static int BailFeePastDue;
    public static bool RecentlyUndied
    {
        get
        {
            if (GameTimeLastUndied == 0)
                return false;
            else if (Game.GameTime - GameTimeLastUndied <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyBribedPolice
    {
        get
        {
            if (GameTimeLastBribedPolice == 0)
                return false;
            else if (Game.GameTime - GameTimeLastBribedPolice <= 10000)
                return true;
            else
                return false;
        }
    }
    public static void BribePolice(int Amount)
    {
        if (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.IsSwimming)
            return;

        if (Game.LocalPlayer.Character.GetCash() < Amount)
            return;

        if (Amount < Police.PreviousWantedLevel * General.MySettings.Police.PoliceBribeWantedLevelScale)
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", string.Format("Thats it? ${0}?", Amount));
            Game.LocalPlayer.Character.GiveCash(-1 * Amount);
            return;
        }
        else
        {
            GameTimeLastBribedPolice = Game.GameTime;
            GTACop ClosestCop = PedList.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
            if (ClosestCop == null)
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "Officer Friendly", "Expedited Service Fee", "Thanks for the cash, now beat it.");
                Game.LocalPlayer.Character.GiveCash(-1 * Amount);
                Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);
                ResetPlayer(true, false);
            }
            else
            {
                BribePoliceAnimation(ClosestCop, Amount);
            }
        }
    }
    public static void Talk()
    {
        //GTACop ClosestCop = PoliceScanning.CopPeds.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();
        //MovePlayerToCop(ClosestCop,false,0);
        Game.DisplayHelp("~INPUT_SELECT_WEAPON_UNARMED~ \"Hello Officer, what seems to be the problem?\",~INPUT_SELECT_WEAPON_MELEE~ \"Am I being Detained?\"", 8000);
    }
    public static void BribePoliceAnimation(GTACop CopToBribe, int Amount)//temp public
    {
        GameFiber.StartNew(delegate
        {
            NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
            Game.TimeScale = 1.0f;
            Tasking.SetUnarmed(CopToBribe);

            Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);

            while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", Game.LocalPlayer.Character, "random@arrests", "kneeling_arrest_escape", 1))
                GameFiber.Wait(250);


            GameFiber.Wait(2000);

            if (!CopToBribe.Pedestrian.Exists())
                return;

            CopToBribe.Pedestrian.BlockPermanentEvents = true;
            CopToBribe.Pedestrian.IsPositionFrozen = true;


            Ped PedToMove = Game.LocalPlayer.Character;
            Ped PedToMoveTo = CopToBribe.Pedestrian;
            Vector3 OriginalPosition = PedToMoveTo.Position;

            bool Continue = true;
            Vector3 PositionToMoveTo = PedToMoveTo.GetOffsetPositionFront(1f);
            float DesiredHeading = PedToMoveTo.Heading - 180;
            NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading);
            uint GameTimeStarted = Game.GameTime;
            while (Game.GameTime - GameTimeStarted <= 15000 && !(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.FacingOppositeDirection(PedToMoveTo)))// PedToMove.Heading.IsWithin(DesiredHeading - 15f, DesiredHeading + 15f)))
            {
                GameFiber.Yield();
                if (Extensions.IsMoveControlPressed() || PedToMoveTo.DistanceTo2D(OriginalPosition) >= 0.1f)
                {
                    Continue = false;
                    break;
                }
            }
            if (!Continue)
            {
                CopToBribe.Pedestrian.BlockPermanentEvents = false;
                CopToBribe.Pedestrian.IsPositionFrozen = false;
                PedToMove.Tasks.Clear();
                return;
            }

            General.RequestAnimationDictionay("mp_common");

            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_common", "givetake1_a", 8.0f, -8.0f, -1, 2, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToMoveTo, "mp_common", "givetake1_b", 8.0f, -8.0f, -1, 2, 0, false, false, false);

            Rage.Object MoneyPile = General.AttachMoneyToPed(PedToMove);

            GameFiber.Wait(1500);
            if (MoneyPile.Exists())
                MoneyPile.Delete();

            MoneyPile = General.AttachMoneyToPed(PedToMoveTo);
            GameFiber.Wait(1500);
            if (MoneyPile.Exists())
                MoneyPile.Delete();

            Game.LocalPlayer.Character.Tasks.Clear();
            PedToMoveTo.Tasks.Clear();
            CopToBribe.Pedestrian.BlockPermanentEvents = false;
            CopToBribe.Pedestrian.IsPositionFrozen = false;

            Game.LocalPlayer.Character.GiveCash(-1 * Amount);
            CopToBribe.Pedestrian.PlayAmbientSpeech("GENERIC_THANKS");
            DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.AvailableDispatch.ResumePatrol, 3));

            ResetPlayer(true, false);
        });
    }
    public static void RespawnAtHospital(Location Hospital)
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        PlayerState.IsDead = false;
        PlayerState.IsBusted = false;

        RespawnInPlace(false);

        if (Hospital == null)
            Hospital = Locations.GetClosestLocationByType(Game.LocalPlayer.Character.Position, Location.LocationType.Hospital);

        Game.LocalPlayer.Character.Position = Hospital.LocationPosition;
        Game.LocalPlayer.Character.Heading = Hospital.Heading;
        PedList.ClearPoliceCompletely();
        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);


        int HospitalFee = General.MySettings.Police.HospitalFee * (1 + PlayerState.MaxWantedLastLife);
        int CurrentCash = Game.LocalPlayer.Character.GetCash();
        int TodaysPayment = 0;

        int TotalNeededPayment = HospitalFee + HospitalBillPastDue;
        
        if(TotalNeededPayment > CurrentCash)
        {
            HospitalBillPastDue = TotalNeededPayment - CurrentCash;
            TodaysPayment = CurrentCash;
        }
        else
        {
            HospitalBillPastDue = 0;
            TodaysPayment = TotalNeededPayment;
        }

        Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA",Hospital.Name,"Hospital Fees",string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}", HospitalFee, TodaysPayment,HospitalBillPastDue));

        Game.LocalPlayer.Character.GiveCash(-1 * TodaysPayment);
    }
    public static void ResistArrest()
    {
        PlayerState.IsBusted = false;
        PlayerState.BeingArrested = false;
        PlayerState.HandsAreUp = false;
        Police.CurrentPoliceState = Police.LastPoliceState;
        Police.SetWantedLevel(PlayerState.PlayerWantedLevel, "Resisting Arrest",true);
        Surrendering.UnSetArrestedAnimation(Game.LocalPlayer.Character);
        NativeFunction.CallByName<uint>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        ResetPlayer(false, false);
        Tasking.UntaskAll(true);

        Police.CurrentCrimes.ResistingArrest.CrimeObserved();

        //DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportResistingArrest, 2));
    }
    public static void Surrender(Location PoliceStation)
    {
        Game.FadeScreenOut(1500);
        GameFiber.Wait(1500);

        bool prePlayerKilledPolice = Police.CurrentCrimes.KillingPolice.HasBeenWitnessedByPolice;
        int BailFee = PlayerState.MaxWantedLastLife * General.MySettings.Police.PoliceBailWantedLevelScale;

        PlayerState.BeingArrested = false;
        PlayerState.IsBusted = false;

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

        PedList.ClearPoliceCompletely();

        GameFiber.Wait(1500);
        Game.FadeScreenIn(1500);

        int CurrentCash = Game.LocalPlayer.Character.GetCash();
        int TodaysPayment = 0;

        int TotalNeededPayment = BailFee + BailFeePastDue;

        if (TotalNeededPayment > CurrentCash)
        {
            BailFeePastDue = TotalNeededPayment - CurrentCash;
            TodaysPayment = CurrentCash;
        }
        else
        {
            BailFeePastDue = 0;
            TodaysPayment = TotalNeededPayment;
        }

        bool LesterHelp = General.MyRand.Next(1, 11) <= 4;
        if (!LesterHelp)
        {
            Game.DisplayNotification("CHAR_BANK_FLEECA", "CHAR_BANK_FLEECA", PoliceStation.Name, "Bail Fees", string.Format("Todays Bill: ~r~${0}~s~~n~Payment Today: ~g~${1}~s~~n~Outstanding: ~r~${2}", BailFee, TodaysPayment, BailFeePastDue));
            Game.LocalPlayer.Character.GiveCash(-1 * TodaysPayment);
        }
        else
        {
            Game.DisplayNotification("CHAR_LESTER", "CHAR_LESTER", PoliceStation.Name, "Bail Fees", string.Format("~g~${0} ~s~", 0));
        }
    }
    public static void UnDie()
    {
        GameTimeLastUndied = Game.GameTime;
        RespawnInPlace(true);
        DispatchAudio.AbortAllAudio();
    }
    public static void ResetPlayer(bool ClearWanted, bool ResetHealth)
    {
        PlayerState.IsDead = false;
        PlayerState.IsBusted = false;
        PlayerState.BeingArrested = false;

        NativeFunction.CallByName<bool>("NETWORK_REQUEST_CONTROL_OF_ENTITY", Game.LocalPlayer.Character);
        NativeFunction.Natives.xC0AA53F866B3134D();
        Game.TimeScale = 1f;
        if (ClearWanted)
        {
            PersonOfInterest.ResetPersonOfInterest(false);
            Police.ResetPoliceStats();
            Police.SetWantedLevel(0,"Reset player with Clear Wanted",false);
            PlayerState.MaxWantedLastLife = 0;  
            NativeFunction.CallByName<bool>("RESET_PLAYER_ARREST_STATE", Game.LocalPlayer);
        }

        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
        NativeFunction.Natives.x80C8B1846639BB19(0);
        if (ResetHealth)
            Game.LocalPlayer.Character.Health = 200;

        NativeFunction.CallByName<bool>("RESET_HUD_COMPONENT_VALUES", 0);

        NativeFunction.Natives.xB9EFD5C25018725A("DISPLAY_HUD", true);
        NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE

        PlayerHealth.ResetDamageStats();
    }
    public static void RespawnInPlace(bool AsOldCharacter)
    {
        try
        {
            PlayerState.IsDead = false;
            PlayerState.IsBusted = false;
            PlayerState.BeingArrested = false;
            Game.LocalPlayer.Character.Health = Game.LocalPlayer.Character.MaxHealth;
            NativeFunction.Natives.xB69317BF5E782347(Game.LocalPlayer.Character);//"NETWORK_REQUEST_CONTROL_OF_ENTITY" 
            if (PlayerState.DiedInVehicle)
            {
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X + 10f, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
                if (Game.LocalPlayer.Character.LastVehicle.Exists() && Game.LocalPlayer.Character.LastVehicle.IsDriveable)
                {
                    Game.LocalPlayer.Character.WarpIntoVehicle(Game.LocalPlayer.Character.LastVehicle, -1);
                }
                PlayerState.DiedInVehicle = false;
            }
            else
            {
                NativeFunction.Natives.xEA23C49EAA83ACFB(Game.LocalPlayer.Character.Position.X, Game.LocalPlayer.Character.Position.Y, Game.LocalPlayer.Character.Position.Z, 0, false, false);//"NETWORK_RESURRECT_LOCAL_PLAYER"
            }
            NativeFunction.Natives.xC0AA53F866B3134D();//_RESET_LOCALPLAYER_STATE
            if (AsOldCharacter)
            {
                ResetPlayer(false, false);
                Police.SetWantedLevel(PlayerState.MaxWantedLastLife,"Resetting to max wanted last life after respawn in place",true);
                ++PlayerState.TimesDied;
            }
            else
            {
                ResetPlayer(true, true);
                Game.LocalPlayer.Character.Inventory.Weapons.Clear();
                PlayerState.LastWeapon = 0;
                Police.PreviousWantedLevel = 0;
                PlayerState.TimesDied = 0;
                PlayerState.MaxWantedLastLife = 0;
            }
            Game.HandleRespawn();
            DispatchAudio.AbortAllAudio();
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("RespawnInPlace",e.Message);
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
            GTAWeapon.WeaponVariation DroppedGunVariation = General.GetWeaponVariation(Game.LocalPlayer.Character, (uint)Weapon.Hash);
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
                General.ApplyWeaponVariation(Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Variation);
                NativeFunction.CallByName<bool>("ADD_AMMO_TO_PED", Game.LocalPlayer.Character, (uint)MyNewGun.Weapon.Hash, MyNewGun.Ammo + 1);
            }
        }
    }

    //public static void MovePlayerToCop(GTACop CopToBribe, bool BribeAnimation, int BribeAmount)
    //{
    //    GameFiber.StartNew(delegate
    //    {
    //        NativeFunction.Natives.xB4EDDC19532BFB85(); //_STOP_ALL_SCREEN_EFFECTS;
    //        Game.TimeScale = 1.0f;

    //        Ped PedToMove = Game.LocalPlayer.Character;
    //        Ped PedToMoveTo = CopToBribe.Pedestrian;

    //        Surrendering.UnSetArrestedAnimation(PedToMove);

    //        while (NativeFunction.CallByName<bool>("IS_ENTITY_PLAYING_ANIM", PedToMove, "random@arrests", "kneeling_arrest_escape", 1))
    //            GameFiber.Wait(250);


    //        GameFiber.Wait(2000);

    //        PedToMoveTo.BlockPermanentEvents = true;
    //        PedToMoveTo.IsPositionFrozen = true;


    //        Vector3 OriginalPosition = PedToMoveTo.Position;

    //        bool Continue = true;
    //        Vector3 PositionToMoveTo = PedToMoveTo.GetOffsetPositionFront(1f);
    //        float DesiredHeading = PedToMoveTo.Heading - 180;
    //        NativeFunction.CallByName<uint>("TASK_PED_SLIDE_TO_COORD", PedToMove, PositionToMoveTo.X, PositionToMoveTo.Y, PositionToMoveTo.Z, DesiredHeading);
    //        uint GameTimeStarted = Game.GameTime;
    //        while (Game.GameTime - GameTimeStarted <= 15000 && !(PedToMove.DistanceTo2D(PositionToMoveTo) <= 0.15f && PedToMove.FacingOppositeDirection(PedToMoveTo)))// PedToMove.Heading.IsWithin(DesiredHeading - 15f, DesiredHeading + 15f)))
    //        {
    //            GameFiber.Yield();
    //            if (Extensions.IsMoveControlPressed() || PedToMoveTo.DistanceTo2D(OriginalPosition) >= 0.1f)
    //            {
    //                Continue = false;
    //                break;
    //            }
    //        }
    //        if (!Continue)
    //        {
    //            PedToMoveTo.BlockPermanentEvents = false;
    //            PedToMoveTo.IsPositionFrozen = false;
    //            PedToMove.Tasks.Clear();
    //            return;
    //        }

    //        if (BribeAnimation)
    //            BribePoliceAnimation(CopToBribe, BribeAmount);
    //    });
    //}
    //public static void BribePoliceAnimation(GTACop CopToBribe, int Amount)
    //{
    //    LosSantosRED.RequestAnimationDictionay("mp_common");

    //    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "mp_common", "givetake1_a", 8.0f, -8.0f, -1, 2, 0, false, false, false);
    //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", CopToBribe.Pedestrian, "mp_common", "givetake1_b", 8.0f, -8.0f, -1, 2, 0, false, false, false);

    //    Rage.Object MoneyPile = LosSantosRED.AttachMoneyToPed(Game.LocalPlayer.Character);

    //    GameFiber.Wait(1500);
    //    if (MoneyPile.Exists())
    //        MoneyPile.Delete();

    //    MoneyPile = LosSantosRED.AttachMoneyToPed(CopToBribe.Pedestrian);
    //    GameFiber.Wait(1500);
    //    if (MoneyPile.Exists())
    //        MoneyPile.Delete();

    //    Game.LocalPlayer.Character.Tasks.Clear();
    //    CopToBribe.Pedestrian.Tasks.Clear();
    //    CopToBribe.Pedestrian.BlockPermanentEvents = false;
    //    CopToBribe.Pedestrian.IsPositionFrozen = false;

    //    Game.LocalPlayer.Character.GiveCash(-1 * Amount);
    //    CopToBribe.Pedestrian.PlayAmbientSpeech("GENERIC_THANKS");
    //    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportResumePatrol, 3));

    //    ResetPlayer(true, false);
    //}
}


