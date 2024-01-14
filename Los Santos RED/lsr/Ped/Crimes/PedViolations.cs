using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionsMethods;
using LSR.Vehicles;
using LosSantosRED.lsr.Helper;

public class PedViolations
{
    private bool IsShootingCheckerActive = false;
    private PedExt PedExt;
    private List<Crime> CrimesViolating = new List<Crime>();
    private List<Crime> CrimesObserved = new List<Crime>();
    private bool EverCommittedCrime = false;
    private uint GameTimeBecameWanted;
    private ICrimes Crimes;
    private bool IsShooting = false;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private uint GameTimeLastCommittedCrime;
    private uint GameTimeLastCommittedGTA;
    private bool prevIsWanted;
    private IEntityProvideable World;
    private uint GameTimeLastNearCops;

    private bool ShouldCheck
    {
        get
        {
            if(PedExt != null && PedExt.Pedestrian.Exists())
            {
                string RelationshipGroupName = PedExt.Pedestrian.RelationshipGroup.Name;//weirdness withthis bullshit
                if(RelationshipGroupName == string.Empty)
                {
                    //EntryPoint.WriteToConsoleTestLong($" PedExt.Pedestrian {PedExt.Pedestrian.Handle} RelationshipGroupName {RelationshipGroupName} RelationshipGroupName2 A{RelationshipGroupName}A");
                    RelationshipGroupName = RelationshipGroupName.ToUpper();
                }
                if (RelationshipGroupName == "SECURITY_GUARD" || RelationshipGroupName == "SECURITY_GUARDS" || RelationshipGroupName == "PRIVATE_SECURITY" || RelationshipGroupName == "FIREMAN" || RelationshipGroupName == "MEDIC" || RelationshipGroupName == "RANGE_IGNORE" || RelationshipGroupName == "range_IGNORE")
                {
                    return false;
                }
                else if (RelationshipGroupName == "")
                {
                    return true;
                }
                else if (RelationshipGroupName == "ZOMBIE")
                {
                    return true;
                }
                return true;
            }
            return false;
           // return PedExt != null && (PedExt.PedGroup == null || PedExt.PedGroup?.InternalName.ToUpper() == "ZOMBIE" || (PedExt.PedGroup != null && PedExt.PedGroup?.InternalName.ToUpper() != "SECURITY_GUARD" && PedExt.PedGroup?.InternalName.ToUpper() != "PRIVATE_SECURITY" && PedExt.PedGroup?.InternalName.ToUpper() != "FIREMAN" && PedExt.PedGroup?.InternalName.ToUpper() != "MEDIC"));
        }
    }
    public PedViolations(PedExt pedExt, ICrimes crimes, ISettingsProvideable settings, IWeapons weapons, IEntityProvideable world)
    {
        PedExt = pedExt;
        Crimes = crimes;
        Settings = settings;
        Weapons = weapons;
        World = world;
    }
    public int WantedLevel { get; set; } = 0;
    public bool IsWanted => WantedLevel > 0;
    public bool IsNotWanted => WantedLevel == 0;
    public bool HasCopsAround { get; private set; }
    public Vector3 PlacePoliceLastSeen { get; private set; }
    public bool CanPoliceSee { get; private set; }
    public bool CanPoliceHear { get; private set; }
    public bool IsDeadlyChase => CrimesObserved.Any(x => x.ResultsInLethalForce);

    public bool IsViolatingWanted => IsWanted || CurrentlyViolatingWantedLevel > 0;

    public int CurrentlyViolatingWantedLevel => CrimesViolating.Any() ? CrimesViolating.Max(x => x.ResultingWantedLevel) : 0;
    public string CurrentlyViolatingWantedLevelReason => CrimesViolating.OrderBy(x=> x.Priority).FirstOrDefault()?.Name;
    public List<Crime> CrimesCurrentlyViolating => CrimesViolating;
    public bool IsCurrentlyViolatingAnyCrimes => CrimesViolating.Any();
    public bool IsCurrentlyViolatingAnyCivilianReportableCrimes => CrimesViolating.Any(x=> x.CanBeReactedToByCivilians);
    public Crime WorstObservedCrime => CrimesObservedViolating.OrderBy(x => x.Priority).FirstOrDefault();
    public List<Crime> CrimesObservedViolating => CrimesObserved;

    public bool IsVisiblyArmed { get; set; }

    public void Update(IPoliceRespondable player)
    {
        if (Settings.SettingsManager.CivilianSettings.CheckCivilianCrimes)
        {
            CrimesViolating.Clear();
            if (!PedExt.IsArrested)
            {
                CheckCrimes(player);

                if (IsWanted && !PedExt.IsDead && !PedExt.IsArrested)
                {
                    CheckWantedStatus();
                }
                if (prevIsWanted != IsWanted)
                {
                    if (IsWanted)
                    {
                        PedExt.OnBecameWanted();
                    }
                    prevIsWanted = IsWanted;
                }
                if(CurrentlyViolatingWantedLevel > 0 || WantedLevel > 0)
                {
                    GameFiber.Yield();//TR TEST 28
                    CheckPoliceSight();
                    if (player.CanBustPeds)
                    {
                        CheckPlayerSight(player);
                    }
                }
                if (WantedLevel > 0)
                {
                    OnPedSeenByPolice();
                }
            }
        }
    }
    private void CheckWantedStatus()
    {
        bool hasCloseCops = false;
        foreach (Cop cop in World.Pedestrians.PoliceList)
        {
            if (NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, cop.CellX, cop.CellY, 3))
            {
                hasCloseCops = true;
                break;
            }
        }
        if (hasCloseCops)
        {
            GameTimeLastNearCops = Game.GameTime;
        }

        HasCopsAround = hasCloseCops;
        if(PedExt.IsBusted)
        {
            if(!HasCopsAround)
            {
                PedExt.IsBusted = false;
                PedExt.CanBeAmbientTasked = true;
                PedExt.CanBeTasked = true;
            }
            return;
        }

        if(CanPoliceSee && IsWanted && PedExt != null && PedExt.Pedestrian.Exists())
        {
            PlacePoliceLastSeen = PedExt.Pedestrian.Position;
        }

        if (!hasCloseCops && Game.GameTime - GameTimeLastNearCops >= 80000)
        {
            PedExt.OnLostWanted();
           // Reset();
            //EntryPoint.WriteToConsoleTestLong($"Removing Wanted Level, No Near Cops {PedExt?.Handle}");
        }
    }
    private void CheckCrimes(IPoliceRespondable player)
    {
        if (PedExt.Pedestrian.Exists() && !PedExt.IsBusted)
        {
            if (PedExt.IsZombie)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingCivilians"));
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"));
                return;
            }

            if (IsWanted)
            {
                if (GameTimeBecameWanted == 0)
                {
                    GameTimeBecameWanted = Game.GameTime;
                }
                if (!CrimesObserved.Any(x => x.ID == "ResistingArrest") && Game.GameTime - GameTimeBecameWanted >= 10000)
                {
                    AddObserved(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                }
            }
            if (PedExt.IsDrunk)
            {
                if (PedExt.IsInVehicle)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "DrunkDriving"));
                }
                else
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "PublicIntoxication"));
                }
            }
            if (PedExt.IsSuicidal)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AttemptingSuicide"));
            }
            if (PedExt.IsSuspicious)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));
            }
            IsVisiblyArmed = IsConsideredVisiblyArmed();
            if (IsVisiblyArmed)
            {
                if (!PedExt.IsInVehicle)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "BrandishingWeapon"));
                }
            }
            if (IsVisiblyArmed)
            {
                ShootingChecker();
                if (IsShooting)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.FiringWeaponCrimeID));
                    if (World.Pedestrians.PoliceList.Any(x => x.DistanceToPlayer <= 60f))//maybe store and do the actual one?
                    {
                        AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "FiringWeaponNearPolice"));
                        //AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "TerroristActivity"));//add TerroristActivity just for 4 stars on some peds
                    }
                }
            }
            else
            {
                IsShootingCheckerActive = false;
            }

            if(PedExt.IsSpeeding)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.FelonySpeedingCrimeID));
            }
            if(PedExt.IsDrivingRecklessly)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == StaticStrings.HitCarWithCarCrimeID));
            }

            if (PedExt.IsDealingDrugs)//if (PedExt.WasEverSetPersistent && NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(PedExt.Pedestrian, "switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(PedExt.Pedestrian, "switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper") > 0f)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "DealingDrugs"));//lslife integration?
            }
            if (PedExt.IsDealingIllegalGuns)//if (PedExt.WasEverSetPersistent && NativeFunction.Natives.IS_ENTITY_PLAYING_ANIM<bool>(PedExt.Pedestrian, "switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper", 3) || NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(PedExt.Pedestrian, "switch@franklin@002110_04_magd_3_weed_exchange", "002110_04_magd_3_weed_exchange_shopkeeper") > 0f)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "DealingGuns"));//lslife integration?
            }
            if (!IsDeadlyChase && !CrimesObserved.Any(x => x.ID == "KillingPolice"))//only loop if we have to
            {
                foreach (Cop cop in World.Pedestrians.AllPoliceList)
                {
                    if (cop.Pedestrian.Exists() && cop.Pedestrian.IsDead)
                    {
                        if (cop.CheckKilledBy(PedExt.Pedestrian))//this is already logged so only comparing uints? no game calls
                        {
                            AddObserved(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "KillingPolice"));//add killing police observed, then get outta here
                            //AddObserved(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "TerroristActivity"));//add TerroristActivity just for 4 stars on some peds
                            break;
                        }
                    }
                }
            }
            //GameFiber.Yield();
            if (PedExt.Pedestrian.Exists() && !PedExt.IsInVehicle)//do a yiled above
            {
                if (PedExt.Pedestrian.IsInCombat || PedExt.Pedestrian.IsInMeleeCombat)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingCivilians"));
                    if (IsVisiblyArmed)
                    {
                        AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingWithDeadlyWeapon"));
                    }
                    foreach (Cop cop in World.Pedestrians.AllPoliceList)
                    {
                        if (cop.Pedestrian.Exists())
                        {
                            if (PedExt.Pedestrian.CombatTarget.Exists() && PedExt.Pedestrian.CombatTarget.Handle == cop.Pedestrian.Handle)
                            {
                                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));
                                break;
                            }
                        }
                    }
                    //foreach(GangMember gangMember in World.Pedestrians.GangMemberList)
                    //{
                    //    if(gangMember.Pedestrian.Exists())
                    //    {
                    //        if (PedExt.Pedestrian.CombatTarget.Exists() && PedExt.Pedestrian.CombatTarget.Handle == gangMember.Pedestrian.Handle)
                    //        {
                    //            AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "HurtingPolice"));
                    //            break;
                    //        }
                    //    }
                    //}
                }
                if (PedExt.Pedestrian.IsJacking)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "GrandTheftAuto"));
                    GameTimeLastCommittedGTA = Game.GameTime;
                }
                if (GameTimeLastCommittedGTA != 0 && Game.GameTime - GameTimeLastCommittedGTA <= 90000 && PedExt.IsInVehicle && PedExt.IsDriver)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "DrivingStolenVehicle"));
                }
                if (CrimesViolating.Any())
                {
                    GameTimeLastCommittedCrime = Game.GameTime;
                    if (!EverCommittedCrime)
                    {
                        EverCommittedCrime = true;
                        PedExt.WillCallPolice = false;
                        PedExt.WillCallPoliceIntense = false;
                        //EntryPoint.WriteToConsole($"PEDCRIMES: FIRST CRIME {PedExt.Pedestrian.Handle} {CrimesViolating.FirstOrDefault().Name}");
                    }
                }
                if (player.Investigation.IsActive && (Game.GameTime - GameTimeLastCommittedCrime <= 7000 || (EverCommittedCrime && PedExt.Pedestrian.IsRunning)) && PedExt.Pedestrian.DistanceTo2D(player.Investigation.Position) <= player.Investigation.Distance)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));
                }
            }
        }
    }
    public void Reset()
    {
        CrimesViolating.Clear();
        CrimesObserved.Clear();
        IsShootingCheckerActive = false;
        WantedLevel = 0;
    }
    public void OnPedSeenByPolice()
    {
        int WantedLevelToAssign = 0;
        foreach (Crime crime in CrimesViolating)
        {
            if (crime.ResultingWantedLevel > WantedLevelToAssign)
            {
                WantedLevelToAssign = crime.ResultingWantedLevel;
            }
            AddObserved(crime);
        }
        if (WantedLevelToAssign > WantedLevel)
        {
            WantedLevel = WantedLevelToAssign;
        }
    }
    public void OnPedHeardByPolice()
    {
        int WantedLevelToAssign = 0;
        foreach(Crime crime in CrimesViolating.Where(x=> x.CanReportBySound))
        {
            if(crime.ResultingWantedLevel > WantedLevelToAssign)
            {
                WantedLevelToAssign = crime.ResultingWantedLevel;
            }
            AddObserved(crime);
        }
        if (WantedLevelToAssign > WantedLevel)
        {
            WantedLevel = WantedLevelToAssign;
        }  
    }
    public void ShootingChecker()
    {
        if (!IsShootingCheckerActive)
        {
            GameFiber.Yield();//TR Yield add 1
            GameFiber.StartNew(delegate
            {
                try
                {
                    IsShootingCheckerActive = true;
                    //EntryPoint.WriteToConsole($"        Ped {PedExt.Pedestrian.Handle} IsShootingCheckerActive {IsShootingCheckerActive}", 5);
                    uint GameTimeLastShot = 0;
                    while (PedExt.Pedestrian.Exists() && IsShootingCheckerActive && EntryPoint.ModController?.IsRunning == true)// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
                    {
                        if (PedExt.Pedestrian.IsShooting)
                        {
                            IsShooting = true;
                            GameTimeLastShot = Game.GameTime;
                        }
                        else if (Game.GameTime - GameTimeLastShot >= 5000)
                        {
                            IsShooting = false;
                        }
                        GameFiber.Yield();
                    }
                    IsShootingCheckerActive = false;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Ped Shooting Checker");
        }
    }
    private void CheckPoliceSight()
    {
        bool CanSeePed = false;
        bool CanHearPed = false;
        if (PedExt.Pedestrian.Exists())
        {
            foreach (Cop cop in World.Pedestrians.AllPoliceList)
            {
                if (cop.Pedestrian.Exists())
                {
                    if(NativeHelper.IsNearby(PedExt.CellX,PedExt.CellY,cop.CellX,cop.CellY,4))
                    {
                        if (PedExt.Pedestrian.Exists())
                        {
                            float DistanceTo = cop.Pedestrian.DistanceTo2D(PedExt.Pedestrian.Position);
                            if (DistanceTo <= 0.1f)
                            {
                                DistanceTo = 999f;
                            }
                            if (DistanceTo <= 10f)//right next to them = they can see ALL!
                            {
                                OnPedSeenByPolice();
                                OnPedHeardByPolice();
                                CanSeePed = true;
                                CanHearPed = true;
                                break;
                            }
                            GameFiber.Yield();//TR THIS IS ADDED WITH THE REDUCED CALLS, IF THERE ARE TONS OF CRIMINALS, ITS OK THAT THIS IS SLOW?
                            if (DistanceTo <= Settings.SettingsManager.PoliceSettings.SightDistance && IsThisPedInFrontOf(cop.Pedestrian) && !cop.IsUnconscious && !cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", cop.Pedestrian, PedExt.Pedestrian))//55f
                            {
                                OnPedSeenByPolice();
                                OnPedHeardByPolice();
                                CanSeePed = true;
                                CanHearPed = true;
                                break;
                            }
                            if (DistanceTo <= Settings.SettingsManager.PoliceSettings.GunshotHearingDistance)
                            {
                                OnPedHeardByPolice();
                                CanHearPed = true;
                            }
                        }
                    }
                }
            }
        }
        CanPoliceSee = CanSeePed;
        CanPoliceHear = CanHearPed;
    }
    private void CheckPlayerSight(IPoliceRespondable player)
    {
        bool CanSeePed = false;
        bool CanHearPed = false;
        float DistanceTo = PedExt.DistanceToPlayer;
        if (DistanceTo <= 0.1f)
        {
            DistanceTo = 999f;
        }
        if (DistanceTo <= 10f)//right next to them = they can see ALL!
        {
            OnPedSeenByPolice();
            OnPedHeardByPolice();
            CanSeePed = true;
            CanHearPed = true;
            return;
        }
        if (DistanceTo <= Settings.SettingsManager.PoliceSettings.SightDistance && IsThisPedInFrontOf(player.Character) && !player.Character.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", player.Character, PedExt.Pedestrian))//55f
        {
            OnPedSeenByPolice();
            OnPedHeardByPolice();
            CanSeePed = true;
            CanPoliceHear = true;
            return;
        }
        if (DistanceTo <= Settings.SettingsManager.PoliceSettings.GunshotHearingDistance)
        {
            OnPedHeardByPolice();
            CanHearPed = true;
        }
        CanPoliceSee = CanSeePed;
        CanPoliceHear = CanHearPed;
    }
    private bool IsThisPedInFrontOf(Ped ToCheck)
    {
        float Result = GetDotVectorResult(ToCheck, PedExt.Pedestrian);
        if (Result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private float GetDotVectorResult(Entity source, Entity target)
    {
        if (source.Exists() && target.Exists())
        {
            Vector3 dir = (target.Position - source.Position).ToNormalized();
            return Vector3.Dot(dir, source.ForwardVector);
        }
        else return -1.0f;
    }
    private bool IsConsideredVisiblyArmed()
    {
       // WeaponDescriptor CurrentWeapon = PedExt.Pedestrian.Inventory.EquippedWeapon;

        uint currentWeapon;
        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(PedExt.Pedestrian, out currentWeapon, true);


        if (currentWeapon == 0)
        {
            return false;
        }
        else if (currentWeapon == 2725352035
            || currentWeapon == 966099553
            || currentWeapon == 0x787F0BB//weapon_snowball
            || currentWeapon == 0x060EC506//weapon_fireextinguisher
            || currentWeapon == 0x34A67B97//weapon_petrolcan
            || currentWeapon == 0xBA536372//weapon_hazardcan
            || currentWeapon == 0x8BB05FD7//weapon_flashlight
            || currentWeapon == 0x23C9F95C)//weapon_ball
        {
            return false;
        }
        else
        {

            uint coolTest = NativeFunction.Natives.GET_CURRENT_PED_WEAPON_ENTITY_INDEX<uint>(PedExt.Pedestrian, 0);//new stuffo?
            if (coolTest != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    private void AddViolating(Crime crime)
    {
        if (crime != null && crime.Enabled && !CrimesViolating.Any(x=> x.ID == crime.ID))
        {
            CrimesViolating.Add(crime);
        }
    }
    private void AddObserved(Crime crime)
    {
        if (crime != null && crime.Enabled && !CrimesObserved.Any(x => x.ID == crime.ID))
        {
            CrimesObserved.Add(crime);
        }
    }
}