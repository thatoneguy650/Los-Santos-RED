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

public class PedCrimes
{
    private bool IsShootingCheckerActive = false;
    private PedExt PedExt;
    private List<Crime> CrimesViolating = new List<Crime>();
    private List<Crime> CrimesObserved = new List<Crime>();
    private bool EverCommittedCrime = false;
    private List<WitnessedCrime> OtherPedCrimesObserved = new List<WitnessedCrime>();
    private uint GameTimeBecameWanted;
    private ICrimes Crimes;
    private bool IsShooting = false;
    private ISettingsProvideable Settings;
    private uint GameTimeLastWitnessedCivilianCrime;
    private IWeapons Weapons;
    private uint GameTimeLastCommittedCrime;
    private uint GameTimeLastCommittedGTA;
    private bool prevIsWanted;

    // private List<Ped> AlreadyCalledInPeds = new List<Ped>();
    private bool ShouldCheck
    {
        get
        {
            if(PedExt != null && PedExt.Pedestrian.Exists())
            {
                string RelationshipGroupName = PedExt.Pedestrian.RelationshipGroup.Name;//weirdness withthis bullshit
                if(RelationshipGroupName == string.Empty)
                {
                    EntryPoint.WriteToConsole($" PedExt.Pedestrian {PedExt.Pedestrian.Handle} RelationshipGroupName {RelationshipGroupName} RelationshipGroupName2 A{RelationshipGroupName}A");
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
    public PedCrimes(PedExt pedExt, ICrimes crimes, ISettingsProvideable settings, IWeapons weapons)
    {
        PedExt = pedExt;
        Crimes = crimes;
        Settings = settings;
        Weapons = weapons;
    }
    public int WantedLevel { get; set; } = 0;
    public bool IsWanted => WantedLevel > 0;
    public bool IsNotWanted => WantedLevel == 0;
    public bool IsDeadlyChase => CrimesObserved.Any(x => x.ResultsInLethalForce);
    public int CurrentlyViolatingWantedLevel => CrimesViolating.Any() ? CrimesViolating.Max(x => x.ResultingWantedLevel) : 0;
    public string CurrentlyViolatingWantedLevelReason => CrimesViolating.OrderBy(x=> x.Priority).FirstOrDefault()?.Name;
    public List<Crime> CrimesCurrentlyViolating => CrimesViolating;
    public bool IsCurrentlyViolatingAnyCrimes => CrimesViolating.Any();
    public bool IsCurrentlyViolatingAnyCivilianReportableCrimes => CrimesViolating.Any(x=> x.CanBeReportedByCivilians);
    public List<Crime> CrimesObservedViolating => CrimesObserved;
    public List<WitnessedCrime> OtherCrimesWitnessed => OtherPedCrimesObserved;
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
                IsShootingCheckerActive = true;
                //EntryPoint.WriteToConsole($"        Ped {PedExt.Pedestrian.Handle} IsShootingCheckerActive {IsShootingCheckerActive}", 5);
                uint GameTimeLastShot = 0;
                while (PedExt.Pedestrian.Exists() && IsShootingCheckerActive)// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
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
            }, "Ped Shooting Checker");
        }
    }
    public void Update(IEntityProvideable world, IPoliceRespondable player)
    {
        if (ShouldCheck && Settings.SettingsManager.CivilianSettings.CheckCivilianCrimes)
        {
            ResetViolations();
            if (!PedExt.IsArrested)
            {
                CheckCrimes(world, player);

                if(prevIsWanted != IsWanted)
                {
                    if(IsWanted)
                    {
                        OnBecameWanted();



                    }
                    prevIsWanted = IsWanted;
                }

               // GameFiber.Yield();//TR maybe if they are a criminal, check crimes more?
                if (WantedLevel > 0)
                {
                    OnPedSeenByPolice();
                }
                else
                {
                    if (CurrentlyViolatingWantedLevel > 0)//TR THIS IS NEW, STOP CHECKING EACH CIV AND EACH COP EACH TIME UNLESS THEY ARE BEING BAD GUYS!
                    {
                        CheckPoliceSight(world);
                        if (player.IsCop)
                        {
                            CheckPlayerSight(player);
                        }
                    }
                }
                if (WantedLevel == 0 && CurrentlyViolatingWantedLevel == 0 && player.IsNotWanted && Settings.SettingsManager.CivilianSettings.AllowCivilinsToCallPoliceOnOtherCivilians)
                {
                    CheckOtherPedCrimes(world, player);
                    //GameFiber.Yield();//TR//should this only yield if it did something? dont think i need this back, but it was the last to be removed
                }
            }
        }
    }
    private void OnBecameWanted()
    {
        if (PedExt.Pedestrian.Exists())
        {
            if (!PedExt.IsGangMember && !PedExt.Pedestrian.IsPersistent)
            {
                PedExt.Pedestrian.IsPersistent = true;
            }
            if(!PedExt.IsGangMember)
            {
                RelationshipGroup HatesCops = new RelationshipGroup("HATESCOPS");
                PedExt.Pedestrian.RelationshipGroup = HatesCops;
                RelationshipGroup.Cop.SetRelationshipWith(HatesCops, Relationship.Hate);
                HatesCops.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                EntryPoint.WriteToConsole($"{PedExt.Pedestrian.Handle} BECAME WANTED SET TO HATESCOPS");
            }
            Vector3 pedPos = PedExt.Pedestrian.Position;
            if(!PedExt.IsGangMember && PedExt.IsDriver)
            {
                NativeFunction.Natives.TASK_SMART_FLEE_COORD(PedExt.Pedestrian, pedPos.X, pedPos.Y, pedPos.Z, 9999f, -1, false, false);
            }
            else if(!PedExt.IsGangMember)
            {
                NativeFunction.Natives.TASK_COMBAT_HATED_TARGETS_AROUND_PED(PedExt.Pedestrian, 300f, 0);
            }
        }
    }
    private void CheckPoliceSight(IEntityProvideable world)
    {
        if (PedExt.Pedestrian.Exists())
        {
            foreach (Cop cop in world.Pedestrians.PoliceList)
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
                                return;
                            }
                            GameFiber.Yield();//TR THIS IS ADDED WITH THE REDUCED CALLS, IF THERE ARE TONS OF CRIMINALS, ITS OK THAT THIS IS SLOW?
                            if (DistanceTo <= Settings.SettingsManager.PoliceSettings.SightDistance && IsThisPedInFrontOf(cop.Pedestrian) && !cop.IsUnconscious && !cop.Pedestrian.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", cop.Pedestrian, PedExt.Pedestrian))//55f
                            {
                                OnPedSeenByPolice();
                                OnPedHeardByPolice();
                                return;
                            }
                            if (DistanceTo <= Settings.SettingsManager.PoliceSettings.GunshotHearingDistance)
                            {
                                OnPedHeardByPolice();
                            }
                        }
                    }
                }
            }
        }
    }
    private void CheckPlayerSight(IPoliceRespondable player)
    {
        float DistanceTo = PedExt.DistanceToPlayer;
        if (DistanceTo <= 0.1f)
        {
            DistanceTo = 999f;
        }
        if (DistanceTo <= 10f)//right next to them = they can see ALL!
        {
            OnPedSeenByPolice();
            OnPedHeardByPolice();
            return;
        }
        if (DistanceTo <= Settings.SettingsManager.PoliceSettings.SightDistance && IsThisPedInFrontOf(player.Character) && !player.Character.IsDead && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", player.Character, PedExt.Pedestrian))//55f
        {
            OnPedSeenByPolice();
            OnPedHeardByPolice();
            return;
        }
        if (DistanceTo <= Settings.SettingsManager.PoliceSettings.GunshotHearingDistance)
        {
            OnPedHeardByPolice();
        }
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
    private void CheckOtherPedCrimes(IEntityProvideable world, IPoliceRespondable playerToCheck)
    {
        if (IsNotWanted && !IsCurrentlyViolatingAnyCrimes)// && (PedExt.WillCallPolice || PedExt.IsGangMember))
        {
            OtherPedCrimesObserved.RemoveAll(x => x.Perpetrator != null && x.Perpetrator.Pedestrian.Exists() && x.Perpetrator.Pedestrian.IsDead);
            List<PedExt> TotalList = new List<PedExt>();
            TotalList.AddRange(world.Pedestrians.CivilianList);
            TotalList.AddRange(world.Pedestrians.ZombieList);
            //TotalList.AddRange(world.GangMemberList);

            if (PedExt.GetType() == typeof(GangMember))
            {
                GangMember gangMember = (GangMember)PedExt;
                TotalList.AddRange(world.Pedestrians.GangMemberList.Where(x => x.Gang?.ID != gangMember.Gang?.ID));
            }
            else
            {
                TotalList.AddRange(world.Pedestrians.GangMemberList);
            }


            foreach (PedExt criminal in TotalList.Where(x => x.Pedestrian.Exists() && x.IsCurrentlyViolatingAnyCivilianReportableCrimes && x.Pedestrian.IsAlive && NativeHelper.IsNearby(PedExt.CellX, PedExt.CellY, x.CellX, x.CellY, 4)).OrderByDescending(x=>x.CurrentlyViolatingWantedLevel).Take(1))
            {
                if (!PedExt.Pedestrian.Exists())
                {
                    break;
                }
                else
                {
                    GameFiber.Yield();//this is new, before it jusdt yielded forever
                    if (!criminal.Pedestrian.Exists() || !PedExt.Pedestrian.Exists())
                    {
                        return;
                    }

                    float distanceToCriminal = PedExt.Pedestrian.DistanceTo2D(criminal.Pedestrian);
                    if (!PedExt.IsGangMember && (PedExt.WillCallPolice || (PedExt.WillCallPoliceIntense && criminal.WantedLevel >= 3)))
                    {
                        uint VehicleWitnessed = 0;
                        uint WeaponWitnessed = 0;
                        Vector3 LocationWitnessed = criminal.Pedestrian.Position;
                        VehicleExt fullVehicle = null;
                        WeaponInformation fullWeapon = null;
                        if (distanceToCriminal <= 60f)
                        {
                            Vehicle tryingToEnter = criminal.Pedestrian.VehicleTryingToEnter;
                            if (criminal.Pedestrian.IsInAnyVehicle(false) && criminal.Pedestrian.CurrentVehicle.Exists())
                            {
                                VehicleWitnessed = criminal.Pedestrian.CurrentVehicle.Handle;
                            }
                            else if (tryingToEnter.Exists())
                            {
                                VehicleWitnessed = tryingToEnter.Handle;
                            }
                            uint currentWeapon;
                            NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(criminal.Pedestrian, out currentWeapon, true);
                            if (currentWeapon != 2725352035 && currentWeapon != 0)
                            {
                                WeaponWitnessed = currentWeapon;
                            }
                            fullVehicle = world.Vehicles.GetVehicleExt(VehicleWitnessed);
                            fullWeapon = Weapons.GetWeapon((ulong)WeaponWitnessed);
                            GameFiber.Yield();//this is new, before it jusdt yielded forever
                        }
                        else
                        {
                            VehicleWitnessed = 0;
                            WeaponWitnessed = 0;
                            //LocationWitnessed = Vector3.Zero;
                            fullVehicle = null;
                            fullWeapon = null;
                        }
                        if (distanceToCriminal <= 40f && criminal.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, criminal.Pedestrian))//60f
                        {
                            foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians))
                            {
                                if (distanceToCriminal <= crime.MaxReportingDistance)
                                {
                                    AddOtherPedObserved(crime, criminal, fullVehicle, fullWeapon, LocationWitnessed);
                                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                                }
                            }
                        }
                        else if (distanceToCriminal <= 100f)
                        {
                            foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians && x.CanReportBySound))
                            {
                                if (distanceToCriminal <= crime.MaxReportingDistance)
                                {
                                    AddOtherPedObserved(crime, criminal, fullVehicle, fullWeapon, LocationWitnessed);
                                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (distanceToCriminal <= 40f && criminal.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian) && NativeFunction.CallByName<bool>("HAS_ENTITY_CLEAR_LOS_TO_ENTITY_IN_FRONT", PedExt.Pedestrian, criminal.Pedestrian))//60f
                        {
                            foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians))
                            {
                                if (distanceToCriminal <= crime.MaxReportingDistance)
                                {
                                    AddOtherPedObserved(crime, criminal, null, null, PedExt.PositionLastSeenCrime);
                                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                                }
                            }
                        }
                        else if (distanceToCriminal <= 100f)
                        {
                            foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians && x.CanReportBySound))
                            {
                                if (distanceToCriminal <= crime.MaxReportingDistance)
                                {
                                    AddOtherPedObserved(crime, criminal, null, null, PedExt.PositionLastSeenCrime);
                                    GameTimeLastWitnessedCivilianCrime = Game.GameTime;
                                }
                            }
                        }
                    }


    
                }
            }
        }
    }
    //private void CheckOtherPedCrimesOLD(IEntityProvideable world, IPoliceRespondable playerToCheck)
    //{
    //    if (IsNotWanted && !IsCurrentlyViolatingAnyCrimes && PedExt.WillCallPolice)
    //    {
    //        OtherPedCrimesObserved.RemoveAll(x => x.Perpetrator != null && x.Perpetrator.Pedestrian.Exists() && x.Perpetrator.Pedestrian.IsDead);
    //        foreach (PedExt criminal in world.CivilianList.Where(x => x.Pedestrian.Exists() && x.IsCurrentlyViolatingAnyCivilianReportableCrimes && x.Pedestrian.IsAlive))
    //        {
    //            if (!PedExt.Pedestrian.Exists())
    //            {
    //                break;
    //            }
    //            else
    //            {
    //                float distanceToCriminal = PedExt.Pedestrian.DistanceTo2D(criminal.Pedestrian);
    //                uint VehicleWitnessed = 0;
    //                uint WeaponWitnessed = 0;
    //                Vector3 LocationWitnessed = criminal.Pedestrian.Position;
    //                VehicleExt fullVehicle = null;
    //                WeaponInformation fullWeapon = null;
    //                if (distanceToCriminal <= 60f)
    //                {
    //                    Vehicle tryingToEnter = criminal.Pedestrian.VehicleTryingToEnter;
    //                    if (criminal.Pedestrian.IsInAnyVehicle(false) && criminal.Pedestrian.CurrentVehicle.Exists())
    //                    {
    //                        VehicleWitnessed = criminal.Pedestrian.CurrentVehicle.Handle;
    //                    }
    //                    else if (tryingToEnter.Exists())
    //                    {
    //                        VehicleWitnessed = tryingToEnter.Handle;
    //                    }
    //                    uint currentWeapon;
    //                    NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(criminal.Pedestrian, out currentWeapon, true);
    //                    if (currentWeapon != 2725352035 && currentWeapon != 0)
    //                    {
    //                        WeaponWitnessed = currentWeapon;
    //                    }
    //                    fullVehicle = world.GetVehicleExt(VehicleWitnessed);
    //                    fullWeapon = Weapons.GetWeapon((ulong)WeaponWitnessed);
    //                }
    //                else
    //                {
    //                    VehicleWitnessed = 0;
    //                    WeaponWitnessed = 0;
    //                    //LocationWitnessed = Vector3.Zero;
    //                    fullVehicle = null;
    //                    fullWeapon = null;
    //                }
    //                if (distanceToCriminal <= 60f && criminal.Pedestrian.IsThisPedInFrontOf(PedExt.Pedestrian))
    //                {
    //                    foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians))
    //                    {
    //                        AddOtherPedObserved(crime, criminal, fullVehicle, fullWeapon, LocationWitnessed);
    //                        GameTimeLastWitnessedCivilianCrime = Game.GameTime;
    //                    }
    //                }
    //                else if (distanceToCriminal <= 100f)
    //                {
    //                    foreach (Crime crime in criminal.CrimesCurrentlyViolating.Where(x => x.CanBeReportedByCivilians && x.CanReportBySound))
    //                    {
    //                        AddOtherPedObserved(crime, criminal, fullVehicle, fullWeapon, LocationWitnessed);
    //                        GameTimeLastWitnessedCivilianCrime = Game.GameTime;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    private void CheckCrimes(IEntityProvideable world, IPoliceRespondable player)
    {
        if (PedExt.Pedestrian.Exists() && !PedExt.IsBusted)
        {
            if(PedExt.IsZombie)
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
                if(!CrimesObserved.Any(x => x.ID == "ResistingArrest") && Game.GameTime - GameTimeBecameWanted >= 10000)
                {
                    AddObserved(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "ResistingArrest"));
                }
            }
            if(PedExt.IsDrunk)
            {
                if(PedExt.IsInVehicle)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "DrunkDriving"));
                }
                else
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "PublicIntoxication"));
                }
            }
            if(PedExt.IsSuicidal)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AttemptingSuicide"));
            }
            if(PedExt.IsSuspicious)
            {
                AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));
            }
            bool isVisiblyArmed = IsVisiblyArmed();


            //if(isVisiblyArmed)
            //{
            //    int interior = NativeFunction.Natives.GET_INTERIOR_FROM_ENTITY<int>(PedExt.Pedestrian);
            //    if(interior == 80386 || interior == 48130)
            //    {
            //        isVisiblyArmed = false;
            //    }
            //}


            if (isVisiblyArmed)
            {
                if (!PedExt.IsInVehicle)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "BrandishingWeapon"));
                }
            }
            if (isVisiblyArmed)
            {
                ShootingChecker();
                if (IsShooting)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "FiringWeapon"));
                    if (world.Pedestrians.PoliceList.Any(x => x.DistanceToPlayer <= 60f))//maybe store and do the actual one?
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
                foreach (Cop cop in world.Pedestrians.PoliceList)
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
                    if (isVisiblyArmed)
                    {
                        AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "AssaultingWithDeadlyWeapon"));
                    }
                    foreach (Cop cop in world.Pedestrians.PoliceList)
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
                        EntryPoint.WriteToConsole($"PEDCRIMES: FIRST CRIME {PedExt.Pedestrian.Handle} {CrimesViolating.FirstOrDefault().Name}", 5);
                    }
                }
                if (player.Investigation.IsActive && (Game.GameTime - GameTimeLastCommittedCrime <= 7000 || (EverCommittedCrime && PedExt.Pedestrian.IsRunning)) && PedExt.Pedestrian.DistanceTo2D(player.Investigation.Position) <= player.Investigation.Distance)
                {
                    AddViolating(Crimes?.CrimeList.FirstOrDefault(x => x.ID == "SuspiciousActivity"));
                }
            }
        }
    }
    private bool IsVisiblyArmed()
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
            return true;
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
    private void AddOtherPedObserved(Crime crime, PedExt perpetrator, VehicleExt vehicle, WeaponInformation weapon, Vector3 location)
    {
        if (crime != null && crime.Enabled)
        {
            WitnessedCrime ExistingEvent = OtherPedCrimesObserved.FirstOrDefault(x => x.Crime?.ID == crime.ID && x.Perpetrator.Handle == perpetrator.Handle);
            if (ExistingEvent == null)
            {
                OtherPedCrimesObserved.Add(new WitnessedCrime(crime, perpetrator, vehicle, weapon, location));
            }
            else 
            {
                ExistingEvent.UpdateWitnessed(vehicle, weapon, location);
            }
        }
    }
    private void ResetViolations()
    {
        CrimesViolating.Clear();
    }
}