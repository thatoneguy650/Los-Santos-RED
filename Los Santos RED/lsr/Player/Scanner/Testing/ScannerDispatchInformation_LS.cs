using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

public class ScannerDispatchInformation_LS : IScannerDispatchableInformation
{
    public Dispatch AimingWeaponAtPolice { get; private set; }
    public Dispatch AnnounceStolenVehicle { get; private set; }
    public Dispatch ArmedRobbery { get; private set; }
    public Dispatch BankRobbery { get; private set; }
    public Dispatch AssaultingCivilians { get; private set; }
    public Dispatch AssaultingCiviliansWithDeadlyWeapon { get; private set; }
    public Dispatch AssaultingOfficer { get; private set; }
    public Dispatch AttemptingSuicide { get; private set; }
    public Dispatch AttemptToReacquireSuspect { get; private set; }
    public Dispatch CarryingWeapon { get; private set; }
    public Dispatch ChangedVehicles { get; private set; }
    public Dispatch CivilianDown { get; private set; }
    public Dispatch CivilianInjury { get; private set; }
    public Dispatch CivilianShot { get; private set; }
    public Dispatch CriminalActivity { get; private set; }
    public Dispatch CurrentlyPlayingDispatch { get; private set; }
    public Dispatch DealingDrugs { get; private set; }
    public Dispatch DealingGuns { get; private set; }
    public Dispatch DrivingAtStolenVehicle { get; private set; }
    public Dispatch DrunkDriving { get; private set; }
    public Dispatch ExcessiveSpeed { get; private set; }
    public Dispatch FelonySpeeding { get; private set; }
    public Dispatch Speeding { get; private set; }
    public Dispatch FirefightingServicesRequired { get; private set; }
    public Dispatch GotOffFreeway { get; private set; }
    public Dispatch GotOnFreeway { get; private set; }
    public Dispatch WentInTunnel { get; private set; }
    public Dispatch GrandTheftAuto { get; private set; }
    public Dispatch Harassment { get; private set; }
    public Dispatch Kidnapping { get; private set; }
    public Dispatch LethalForceAuthorized { get; private set; }
    public Dispatch MedicalServicesRequired { get; private set; }
    public Dispatch Mugging { get; private set; }
    public Dispatch NoFurtherUnitsNeeded { get; private set; }
    public Dispatch OfficerDown { get; private set; }
    public Dispatch OfficerMIA { get; private set; }
    public Dispatch OfficerNeedsAssistance { get; private set; }
    public Dispatch OfficersNeeded { get; private set; }
    public Dispatch OnFoot { get; private set; }
    public Dispatch PedHitAndRun { get; private set; }
    public Dispatch PublicIntoxication { get; private set; }
    public Dispatch PublicNuisance { get; private set; }
    public Dispatch StandingOnVehicle { get; private set; }
    public Dispatch RecklessDriving { get; private set; }
    public Dispatch RemainInArea { get; private set; }
    public Dispatch RequestAirSupport { get; private set; }
    public Dispatch RequestBackup { get; private set; }
    public Dispatch RequestBackupSimple { get; private set; }
    public Dispatch RequestFIBUnits { get; private set; }
    public Dispatch RequestMilitaryUnits { get; private set; }
    public Dispatch RequestNOOSEUnits { get; private set; }
    public Dispatch RequestNooseUnitsAlt { get; private set; }
    public Dispatch RequestNooseUnitsAlt2 { get; private set; }
    public Dispatch RequestSwatAirSupport { get; private set; }
    public Dispatch ShotsFiredStatus { get; private set; }
    public Dispatch ResistingArrest { get; private set; }
    public Dispatch ResumePatrol { get; private set; }
    public Dispatch RunningARedLight { get; private set; }
    public Dispatch ShotsFired { get; private set; }
    public Dispatch ShotsFiredAtAnOfficer { get; private set; }
    public Dispatch StealingAirVehicle { get; private set; }
    public Dispatch SuspectArrested { get; private set; }
    public Dispatch SuspectEvaded { get; private set; }
    public Dispatch SuspectEvadedSimple { get; private set; }
    public Dispatch SuspectSpotted { get; private set; }
    public Dispatch UnlawfulBodyDisposal { get; private set; }
    public Dispatch CivilianReportUpdate { get; private set; }
    public Dispatch SuspectWasted { get; private set; }
    public Dispatch SuspiciousActivity { get; private set; }
    public Dispatch SuspiciousVehicle { get; private set; }
    public Dispatch TamperingWithVehicle { get; private set; }
    public Dispatch TerroristActivity { get; private set; }
    public Dispatch ThreateningOfficerWithFirearm { get; private set; }
    public Dispatch TrespassingOnGovernmentProperty { get; private set; }
    public Dispatch TrespassingOnMilitaryBase { get; private set; }
    public Dispatch Trespassing { get; private set; }
    public Dispatch VehicleCrashed { get; private set; }
    public Dispatch VehicleHitAndRun { get; private set; }
    public Dispatch VehicleStartedFire { get; private set; }
    public Dispatch PublicVagrancy { get; private set; }
    public Dispatch IndecentExposure { get; private set; }
    public Dispatch MaliciousVehicleDamage { get; private set; }
    public Dispatch WantedSuspectSpotted { get; private set; }
    public Dispatch WeaponsFree { get; private set; }
    public Dispatch DrugPossession { get; private set; }
    public Dispatch SuspectSpottedSimple { get; private set; }
    public Dispatch StoppingTrains { get; private set; }
    public Dispatch TheftDispatch { get; private set; }
    public Dispatch Shoplifting { get; private set; }


    public Dispatch Nuisance { get; private set; }

    public Dispatch Vagrancy { get; private set; }
    public Dispatch Intoxication { get; private set; }
    public virtual void Setup()
    {
        OfficerDown = new Dispatch()
        {
            Name = "Officer Down",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
               // new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AcriticalsituationOfficerdown.FileName },"we have a critical situation, officer down"),
               //// new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName },"we have an officer down, possibly KIA"),
               // new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.Anofficerdown.FileName },"we have an officer down"),
               //// new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName },"we have an officer down, condition unknown"),


                new AudioSet(new List<string>() { crime_officer_down.AcriticalsituationOfficerdown.FileName },"a critical situation, officer down"),
                new AudioSet(new List<string>() { crime_officer_down.AnofferdownpossiblyKIA.FileName },"an officer down, possibly KIA"),
                new AudioSet(new List<string>() { crime_officer_down.Anofficerdown.FileName },"an officer down"),
                new AudioSet(new List<string>() { crime_officer_down.Anofficerdownconditionunknown.FileName },"an officer down, condition unknown"),

            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
            MainMultiAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_officers_down.Multipleofficersdown.FileName },"multiple officers down"),
                new AudioSet(new List<string>() { crime_officers_down.Severalofficersdown.FileName },"several officers down"),
                //new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officers_down.Multipleofficersdown.FileName },"we have multiple officers down"),
                //new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officers_down.Severalofficersdown.FileName },"we have several officers down"),
            },
        };
        OfficerMIA = new Dispatch()
        {
            Name = "Officer MIA",
            IncludeAttentionAllUnits = true,
            //ResultsInLethalForce = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                //new AudioSet(new List<string>() { crime_officer_down.AnofferdownpossiblyKIA.FileName },"an officer down, possibly KIA"),
                //new AudioSet(new List<string>() { crime_officer_down.Anofficerdownconditionunknown.FileName },"an officer down, condition unknown"),
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_in_need_of_assistance.Anofficerinneedofassistance.FileName },"we have an officer in need of assistance"),
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_in_need_of_assistance.Anofficerrequiringassistance.FileName },"we have an officer requiring assistance"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
        };
        ShotsFiredAtAnOfficer = new Dispatch()
        {
            Name = "Shots Fired at an Officer",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.Street,
            CanBeReportedMultipleTimes = false,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName },"shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Afirearmattackonanofficer.FileName },"a firearm attack on an officer"),
              //  new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficershot.FileName },"an officer shot"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficerunderfire.FileName },"a officer under fire"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Shotsfiredatanofficer.FileName },"a shots fired at an officer"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            }
        };
        AssaultingOfficer = new Dispatch()
        {
            Name = "Assault on an Officer",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName },"an assault on an officer"),
                new AudioSet(new List<string>() { crime_assault_on_an_officer.Anofficerassault.FileName },"an officer assault"),
            },
        };
        ThreateningOfficerWithFirearm = new Dispatch()
        {
            Name = "Threatening an Officer with a Firearm",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName },"a suspect threatening an officer with a firearm"),
            },
        };
        TrespassingOnGovernmentProperty = new Dispatch()
        {
            Name = "Trespassing on Government Property",
            ResultsInLethalForce = true,
            CanBeReportedMultipleTimes = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName },"trespassing on government property"),
            },
        };
        TrespassingOnMilitaryBase = new Dispatch()
        {
            Name = "Trespassing on Military Base",
            ResultsInLethalForce = true,
            CanBeReportedMultipleTimes = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName },"trespassing on military base"),
            },
        };
        Trespassing = new Dispatch()
        {
            Name = "Trespassing",
            ResultsInLethalForce = false,
            CanBeReportedMultipleTimes = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_trespassing.Trespassing.FileName },"trespassing"),
            },
        };
        StealingAirVehicle = new Dispatch()
        {
            Name = "Stolen Air Vehicle",
            ResultsInLethalForce = true,
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_stolen_aircraft.Astolenaircraft.FileName},"a stolen aircraft"),
                new AudioSet(new List<string>() { crime_hijacked_aircraft.Ahijackedaircraft.FileName },"a hijacked aircraft"),
                new AudioSet(new List<string>() { crime_theft_of_an_aircraft.Theftofanaircraft.FileName },"theft of an aircraft"),
            },
        };
        ShotsFired = new Dispatch()
        {
            Name = "Shots Fired",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName },"a firearms situation, several shots fired"),
                new AudioSet(new List<string>() { crime_shooting.Aweaponsincidentshotsfired.FileName },"a weapons incdient, shots fired"),
                new AudioSet(new List<string>() { crime_shoot_out.Ashootout.FileName },"a shoot-out"),
                new AudioSet(new List<string>() { crime_firearms_incident.AfirearmsincidentShotsfired.FileName },"a firearms incident, shots fired"),
                new AudioSet(new List<string>() { crime_firearms_incident.Anincidentinvolvingshotsfired.FileName },"an incident involving shots fired"),
                new AudioSet(new List<string>() { crime_firearms_incident.AweaponsincidentShotsfired.FileName },"a weapons incident, shots fired"),
            },
        };
        CarryingWeapon = new Dispatch()
        {
            Name = "Carrying Weapon",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            CanBeReportedMultipleTimes = false,
        };
        CivilianDown = new Dispatch()
        {
            Name = "Civilian Down",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName },"civilian fatality"),
                new AudioSet(new List<string>() { crime_civilian_down.Aciviliandown.FileName },"civilian down"),

                new AudioSet(new List<string>() { crime_1_87.A187.FileName },"a 1-87"),
                new AudioSet(new List<string>() { crime_1_87.Ahomicide.FileName },"a homicide"),
            },
        };
        CivilianShot = new Dispatch()
        {
            Name = "Civilian Shot",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_civillian_gsw.AcivilianGSW.FileName },"a civilian GSW"),
                new AudioSet(new List<string>() { crime_civillian_gsw.Acivilianshot.FileName },"a civilian shot"),
                new AudioSet(new List<string>() { crime_civillian_gsw.Agunshotwound.FileName },"a gunshot wound"),
            },
        };
        CivilianInjury = new Dispatch()
        {
            Name = "Civilian Injury",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_injured_civilian.Aninjuredcivilian.FileName },"an injured civilian"),
                new AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName },"a civilian in need of assistance"),
                new AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianrequiringassistance.FileName },"a civilian requiring assistance"),
                new AudioSet(new List<string>() { crime_assault_on_a_civilian.Anassaultonacivilian.FileName },"an assault on a civilian"),
            },
        };
        GrandTheftAuto = new Dispatch()
        {
            Name = "Grand Theft Auto",
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            IncludeCarryingWeapon = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftauto.FileName },"a grand theft auto"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftautoinprogress.FileName },"a grand theft auto in progress"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress.FileName },"a GTA in progress"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress1.FileName },"a GTA in progress"),
            },
        };
        SuspiciousActivity = new Dispatch()
        {
            Name = "Suspicious Activity",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspicious_activity.Suspiciousactivity.FileName },"suspicious activity"),
                new AudioSet(new List<string>() { crime_9_25.Asuspiciousperson.FileName },"a suspicious person"),
            },
        };
        TamperingWithVehicle = new Dispatch()
        {
            Name = "Tampering With Vehicle",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_04.Tamperingwithavehicle.FileName },"tampering with a vehicle"),
            },
        };
        CriminalActivity = new Dispatch()
        {
            Name = "Criminal Activity",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_criminal_activity.Criminalactivity.FileName },"criminal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Illegalactivity.FileName },"illegal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Prohibitedactivity.FileName },"prohibited activity"),
            },
        };
        Mugging = new Dispatch()
        {
            Name = "Mugging",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_mugging.Apossiblemugging.FileName },"a possible mugging"),
            },
        };
        TerroristActivity = new Dispatch()
        {
            Name = "Terrorist Activity",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity1.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity2.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Terroristactivity.FileName },"terrorist activity"),
            },
        };
        ArmedRobbery = new Dispatch()
        {
            Name = "Armed Robbery",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_robbery.Apossiblerobbery.FileName },"a possible robbery"),
                new AudioSet(new List<string>() { crime_2_11.Anarmedrobbery.FileName },"an armed robbery"),
                new AudioSet(new List<string>() { crime_robbery_with_a_firearm.Arobberywithafirearm.FileName },"a robbery with a firearm"),
                new AudioSet(new List<string>() { crime_hold_up.Aholdup.FileName },"a hold up"),
            },
        };
        BankRobbery = new Dispatch()
        {
            Name = "Bank Robbery",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_bank_robbery.Abankheist.FileName },"a bank robbery"),
                new AudioSet(new List<string>() { crime_bank_robbery.Abankrobbery.FileName },"a bank robbery"),
                new AudioSet(new List<string>() { crime_bank_robbery.Abankrobbery1.FileName },"a bank firearm"),
                new AudioSet(new List<string>() { crime_bank_robbery.Apossiblebankrobbery.FileName },"a possible bank robbery"),
                new AudioSet(new List<string>() { crime_bank_robbery.Apossiblebankrobbery1.FileName },"a possible bank robbery"),
            },
        };
        PublicNuisance = new Dispatch()
        {
            Name = "Public Nuisance",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_07.Apublicnuisance.FileName },"a public nuisance"),
                    //},
                },
        };
        StandingOnVehicle = new Dispatch()
        {
            Name = "Standing On Vehicle",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_94.Maliciousmischief.FileName },"malicious mischief"),
                new AudioSet(new List<string>() { crime_5_94.Maliciousmischief1.FileName },"malicious mischief"),
            },
        };
        UnlawfulBodyDisposal = new Dispatch()
        {
            Name = "Unlawful Disposal of Remains",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_4_19.Adeadbody.FileName },"a dead body"),
                new AudioSet(new List<string>() { crime_4_19.Adeceasedperson.FileName },"a deceased person"),
            },
        };
        TheftDispatch = new Dispatch()
        {
            Name = "Theft",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_theft.Apossibletheft.FileName },"a possible theft"),
            },
        };
        Shoplifting = new Dispatch()
        {
            Name = "Shoplifting",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_4_84.Apettytheft.FileName },"a petty theft"),
            },
        };
        PublicVagrancy = new Dispatch()
        {
            Name = "Public Vagrancy",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_drug_overdose.An11357PossibleOD.FileName },"a possible OD"),
                new AudioSet(new List<string>() { crime_unconscious_civilian.Anunconsciouscivilian.FileName },"an unconscious civilian"),
            },
        };
        IndecentExposure = new Dispatch()
        {
            Name = "Indecent Exposure",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_07.Apublicnuisance.FileName },"a public nuisance"),
            },
        };
        DrugPossession = new Dispatch()
        {
            Name = "Drug Possession",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_11_357.Adrugpossessionincident.FileName },"a drug possession incident"),
                new AudioSet(new List<string>() { crime_11_357.Adrugpossessionincident1.FileName },"a drug possession incident"),
                new AudioSet(new List<string>() { crime_criminal_activity.Criminalactivity.FileName },"criminal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Illegalactivity.FileName },"criminal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Prohibitedactivity.FileName },"criminal activity"),
            },
        };
        StoppingTrains = new Dispatch()
        {
            Name = "Stopping Local Trains",
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_hijacked_vehicle.Ahijackedvehicle.FileName },"a hijacked vehicle"),
            },
        };
        MaliciousVehicleDamage = new Dispatch()
        {
            Name = "Malicious Vehicle Damage",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_malicious_vehicle_damage.Maliciousvehicledamage.FileName },"malicious vehicle damage"),
            },
        };
        SuspiciousVehicle = new Dispatch()
        {
            Name = "Suspicious Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspicious_vehicle.Asuspiciousvehicle.FileName },"a suspicious vehicle"),
            },
        };
        DrivingAtStolenVehicle = new Dispatch()
        {
            Name = "Driving a Stolen Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingSpeed = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_in_a_stolen_car.Apersoninastolencar.FileName},"a person in a stolen car"),
                new AudioSet(new List<string>() { crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName },"a person in a stolen vehicle"),
            },
        };
        ResistingArrest = new Dispatch()
        {
            Name = "Resisting Arrest",
            LocationDescription = LocationSpecificity.Zone,
            IncludeCarryingWeapon = true,
            CanBeReportedMultipleTimes = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_resisting_arrest.Apersonresistingarrest.FileName },"a person resisting arrest"),
                new AudioSet(new List<string>() { crime_suspect_resisting_arrest.Asuspectresistingarrest.FileName },"a suspect resisiting arrest"),

                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest.FileName },"a criminal resisiting arrest"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest1.FileName },"a criminal resisiting arrest"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectfleeingacrimescene.FileName },"a suspect fleeing a crime scene"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectontherun.FileName },"a suspect on the run"),
            }
        };
        AttemptingSuicide = new Dispatch()
        {
            Name = "Suicide Attempt",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName },"a possible attempted suicide"),
                new AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName },"an attempted suicide")
            }
        };
        FelonySpeeding = new Dispatch()
        {
            Name = "Felony Speeding",
            IncludeDrivingVehicle = true,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName },"a speeding felony"),
                //new AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        Speeding = new Dispatch()
        {
            Name = "Speeding",
            IncludeDrivingVehicle = true,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_speeding.Speeding.FileName },"speeding"),
                 new AudioSet(new List<string>() { crime_speeding_incident.Aspeedingincident.FileName },"a speeding incident"),
                new AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        PedHitAndRun = new Dispatch()
        {
            Name = "Pedestrian Hit-and-Run",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName},"a pedestrian struck"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck1.FileName },"a pedestrian struck"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName },"a pedestrian struck by a vehicle"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName },"a pedestrian struck by a vehicle"),
            },
        };
        VehicleHitAndRun = new Dispatch()
        {
            Name = "Motor Vehicle Accident",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AnAEincident.FileName },"an A&E incident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AseriousMVA.FileName },"a serious MVA"),
            },
        };
        RunningARedLight = new Dispatch()
        {
            Name = "Running a Red Light",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_running_a_red_light.Apersonrunningaredlight.FileName},"a person running a red light"),
            },
        };
        RecklessDriving = new Dispatch()
        {
            Name = "Reckless Driving",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_reckless_driver.Arecklessdriver.FileName},"a reckless driver"),
                new AudioSet(new List<string>() { crime_5_05.A505.FileName,crime_5_05.Adriveroutofcontrol.FileName },"a 505, a driver out of control"),
            },
        };
        DrunkDriving = new Dispatch()
        {
            Name = "Drunk Driving",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_02.Adriverundertheinfluence.FileName},"a driver under the influence"),
                new AudioSet(new List<string>() { crime_5_02.Adriverundertheinfluence1.FileName},"a driver under the influence"),
                new AudioSet(new List<string>() { crime_5_02.ADUI.FileName},"a dui"),
                new AudioSet(new List<string>() { crime_5_02.A502DUI.FileName},"a 502 dui"),
            },
        };
        AssaultingCivilians = new Dispatch()
        {
            Name = "Assault on a Civilian",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault.Apossibleassault.FileName},"a possible assault"),
                new AudioSet(new List<string>() { crime_assault.Apossibleassault1.FileName},"a possible assault"),
                new AudioSet(new List<string>() { crime_assault_on_a_civilian.Anassaultonacivilian.FileName},"an assault on a civilian"),
                new AudioSet(new List<string>() { crime_assault_and_battery.AnAE.FileName},"an A&B"),
                new AudioSet(new List<string>() { crime_assault_and_battery.Anassaultandbattery.FileName},"an assault and battery"),
            },
        };
        AimingWeaponAtPolice = new Dispatch()
        {
            Name = "Threatening Officer With Firearm",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName},"a suspect threatening and officer with a firearm"),
                new AudioSet(new List<string>() { crime_officer_in_danger.Anofficerindanger.FileName},"an officer in danger"),
            },
        };
        DealingDrugs = new Dispatch()
        {
            Name = "Dealing Drugs",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_drug_deal.Adrugdeal.FileName},"a drug deal"),
                new AudioSet(new List<string>() { crime_drug_deal.Adrugdealinprogress.FileName},"a drug deal in progress"),
                new AudioSet(new List<string>() { crime_drug_deal.Apossibledrugdeal.FileName},"a possible drug deal"),
                new AudioSet(new List<string>() { crime_drug_deal.Narcoticstrafficking.FileName},"narcotics trafficing"),
            },
        };
        DealingGuns = new Dispatch()
        {
            Name = "Illegal Weapons Dealing",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_firearms_possession.Afirearmspossession.FileName},"a firearms possession"),
            },
        };
        AssaultingCiviliansWithDeadlyWeapon = new Dispatch()
        {
            Name = "Assault With a Deadly Weapon",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault_with_a_deadly_weapon.Assaultwithadeadlyweapon.FileName},"an assault with a deadly weapon"),
                new AudioSet(new List<string>() { crime_assault_with_a_deadly_weapon.AnADW.FileName},"an ADW"),
            },
        };
        Kidnapping = new Dispatch()
        {
            Name = "Kidnapping",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_2_07.Akidnapping.FileName},"a kidnapping"),
                new AudioSet(new List<string>() { crime_2_07.Akidnapping1.FileName},"a kidnapping"),
            },
        };
        PublicIntoxication = new Dispatch()
        {
            Name = "Public Intoxication",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_3_90.Publicintoxication.FileName},"public intoxication"),
            },
        };
        OfficerNeedsAssistance = new Dispatch()
        {
            Name = "Officer Needs Assistance",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_officer_in_need_of_assistance.Anofficerinneedofassistance.FileName},"an officer in need of assistance"),
                 new AudioSet(new List<string>() { crime_officer_in_need_of_assistance.Anofficerrequiringassistance.FileName},"an officer requiring assistance"),
            },
        };
        Harassment = new Dispatch()
        {
            Name = "Harassment",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_07.Apublicnuisance.FileName},"a public nuisance"),
                new AudioSet(new List<string>() { crime_disturbance.Apossibledisturbance.FileName},"a possible disturbance"),
                new AudioSet(new List<string>() { crime_disturbance.Adisturbance.FileName},"a disturbance"),
                new AudioSet(new List<string>() { crime_disturbance.Adisturbance1.FileName},"a disturbance"),
            },
        };
        OfficersNeeded = new Dispatch()
        {
            Name = "Officers Needed",
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { assistance_required.Officersneeded.FileName},"officers needed"),
                 new AudioSet(new List<string>() { assistance_required.Officersrequired.FileName},"officers required"),
            },
        };
        AnnounceStolenVehicle = new Dispatch()
        {
            Name = "Stolen Vehicle Reported",

            IncludeDrivingVehicle = true,
            CanAlwaysBeInterrupted = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {crime_stolen_vehicle.Apossiblestolenvehicle.FileName},"a possible stolen vehicle"),
            },
        };
        RequestAirSupport = new Dispatch()
        {
            Name = "Air Support Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequestinghelicoptersupport.FileName },"officers requesting helicopter support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName },"code-99 units request immediate air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport.FileName },"officers require aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport1.FileName },"officers require aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireairsupport.FileName },"officers require air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestaerialsupport.FileName },"units request aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestingairsupport.FileName },"units requesting air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName },"units requesting helicopter support"),
            },


            EpilogueAudioSet = new List<AudioSet>()
                {
                    //s_m_y_cop_white_full_01

                    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.WeAreAirbornAndMovingIn.FileName },"we are airborn and moving in"),

                }
        };
        RequestMilitaryUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Military Units Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { custom_wanted_level_line.Code13militaryunitsrequested.FileName },"code-13 military units requested"),
            },

            SecondaryAudioSet = new List<AudioSet>()
                {
                    new AudioSet(new List<string>() {dispatch_units_full.DispatchingunitsfromKazanskyAirForceBase.FileName },"dispatching units from Kazansky Air Force Base"),
                    new AudioSet(new List<string>() {dispatch_units_full.ScramblingmilitaryaircraftfromKazanskyAirForceBase.FileName },"scrambling military aircraft from Kazansky Air Force Base"),
                }
        };
        RequestNOOSEUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "NOOSE Units Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters.FileName },"dispatching swat units from police headquarters"),
                new AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters1.FileName },"dispatching swat units from police headquarters"),
            },
        };
        RequestNooseUnitsAlt = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "NOOSE Units Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, SWAT3.respondcode3.FileName},"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, SWAT3.requestingcode1alphain30minutes.FileName },"dispatching NOOSE units"),
            },
        };
        RequestNooseUnitsAlt2 = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "NOOSE Units Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            CanAddExtras = false,
            MainAudioSet = new List<AudioSet>()
            {
               // new AudioSet(new List<string>() { SWAT3.swat10minuteeta.FileName,SWAT3.suspectarmedusecaution.FileName},"dispatching swat units"),
               new AudioSet(new List<string>() { SWAT3.multipleswatunitsresponding.FileName,SWAT3.suspectarmedusecaution.FileName},"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.multipleswatunitsresponding.FileName, SWAT3.suspectsarmedwithheavyweaponsandbodyarmor.FileName },"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.swat10minuteeta.FileName },"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.calloutpending.FileName },"dispatching NOOSE units"),
            },
        };
        RequestFIBUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "FIB-HET Units Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_units_full.FIBteamdispatchingfromstation.FileName},"dispatching FIB-HET units"),
            },
        };
        RequestSwatAirSupport = new Dispatch()
        {
            Name = "Air Support Requested",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            CanAddExtras = false,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, AudioBeeps.Radio_End_1.FileName, AudioBeeps.Radio_Start_1.FileName,SWAT3.tendavidgoahead.FileName },"emergency traffic"),
               new AudioSet(new List<string>() { SWAT3.copyemergencytraffic.FileName, AudioBeeps.Radio_End_1.FileName, AudioBeeps.Radio_Start_1.FileName,SWAT3.tendavidgo.FileName },"emergency traffic"),
             },
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.airsupportimmediateinsertion.FileName,SWAT3.respondcode3.FileName },"officers requesting helicopter support"),
            },
        };
        ShotsFiredStatus = new Dispatch()
        {
            Name = "Shots Fired",
            IsPoliceStatus = true,
            IncludeReportedBy = true,
            CanBeReportedMultipleTimes = true,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName },"shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Afirearmattackonanofficer.FileName },"a firearm attack on an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficerunderfire.FileName },"a officer under fire"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Shotsfiredatanofficer.FileName },"a shots fired at an officer"),


                new AudioSet(new List<string>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName },"a shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shooting.Aweaponsincidentshotsfired.FileName },"a shots fired at an officer"),

                new AudioSet(new List<string>() { crime_shoot_out.Ashootout.FileName },"a shots fired at an officer"),



            },
            //    SecondaryAudioSet = new List<AudioSet>()
            //{
            //    new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
            //    new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
            //    new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
            //    new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            //}
        };
        CivilianReportUpdate = new Dispatch()
        {
            Name = "Report Updated",
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            IncludeCarryingWeapon = true,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
        };
        SuspectSpotted = new Dispatch()
        {
            Name = "Suspect Spotted",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,

            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual7.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual7.FileName },"suspect spotted"),
             },


            //PreambleHelicopterAudioSet = new List<AudioSet>()
            //{
            //    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingNorth.FileName },"suspect spotted"),
            //    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingEast.FileName },"suspect spotted"),
            //new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingWest.FileName },"suspect spotted"),
            //    //new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingSouth.FileName },"suspect spotted"),
            //},


        };
        SuspectSpottedSimple = new Dispatch()
        {
            Name = "Suspect Spotted",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
        };
        WantedSuspectSpotted = new Dispatch()
        {
            Name = "Wanted Suspect Spotted",
            IsPoliceStatus = true,
            IncludeReportedBy = true,
            IncludeRapSheet = true,
            Priority = 10,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual7.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual7.FileName },"suspect spotted"),
             },

            PreambleHelicopterAudioSet = new List<AudioSet>()
                {
                    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingNorth.FileName },"suspect spotted"),
                    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingEast.FileName },"suspect spotted"),
                    new AudioSet(new List<string>() { s_m_y_cop_white_full_01.AirSupportSuspectIsHeadingWest.FileName },"suspect spotted"),
                },

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_wanted_felon_on_the_loose.Awantedfelonontheloose.FileName },"a wanted felon on the loose"),
            },
        };
        OnFoot = new Dispatch()
        {
            Name = "On Foot",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_is.SuspectIs.FileName, on_foot.Onfoot.FileName },"suspect is on foot"),
                new AudioSet(new List<string>() { suspect_is.SuspectIs.FileName, on_foot.Onfoot1.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
            },
        };
        ExcessiveSpeed = new Dispatch()
        {
            Name = "Excessive Speed",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            IncludeDrivingVehicle = false,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                //new AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName },"a speeding felony"),
                //new AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        WentInTunnel = new Dispatch()
        {
            Name = "Entered Tunnel",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {


                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual1.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual2.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual3.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual4.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual5.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual6.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual7.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual8.FileName },"suspect spotted entering a tunnel"),

                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual1.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual2.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual3.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual4.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual5.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual6.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual7.FileName },"suspect spotted entering a tunnel"),

                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual1.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual2.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual3.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual4.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual5.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual6.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual7.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual8.FileName },"suspect spotted entering a tunnel"),

                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual1.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual2.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual3.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual4.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual5.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual6.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual7.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual8.FileName },"suspect spotted entering a tunnel"),

                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual1.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual2.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual3.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual4.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual5.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual6.FileName },"suspect spotted entering a tunnel"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual7.FileName },"suspect spotted entering a tunnel"),


                //new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
                //new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteredMetro.FileName },"suspect has entered a tunnel"),
            },
        };
        GotOnFreeway = new Dispatch()
        {
            Name = "Entered Freeway",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
            },
        };
        GotOffFreeway = new Dispatch()
        {
            Name = "Exited Freeway",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
            },
        };
        MedicalServicesRequired = new Dispatch()
        {
            Name = "Medical Services Required",
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            NotificationTitle = "Emergency Scanner",
            NotificationSubtitle = "~y~Injured Person Reported~s~",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_medical_aid_requested.Medicalaidrequested.FileName},"medical aid requested"),
            },
        };
        FirefightingServicesRequired = new Dispatch()
        {
            Name = "Fire Fighting Services Required",
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            NotificationTitle = "Emergency Scanner",
            NotificationSubtitle = "~y~Fire Reported~s~",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { emergency.Apossiblefire.FileName},"a possible fire"),
            },
        };
        VehicleStartedFire = new Dispatch()
        {
            Name = "Vehicle On Fire",
            IsPoliceStatus = true,
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_vehicle_on_fire.Avehicleonfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Acarfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Acaronfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Anautomobileonfire.FileName},"a vehicle on fire"),
            },
        };
        VehicleCrashed = new Dispatch()
        {
            Name = "Vehicle Crashed",
            IsPoliceStatus = true,
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
            },
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AseriousMVA.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AnAEincident.FileName},"a motor vehicle accident"),
            },
        };
        NoFurtherUnitsNeeded = new Dispatch()
        {
            Name = "Officers On-Site, Code 4-ADAM",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,

            NotificationTitle = "Emergency Scanner",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officers_on_scene.Officersareatthescene.FileName },"officers are at the scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officersarrivedonscene.FileName },"offices have arrived on scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officershavearrived.FileName },"officers have arrived"),
                new AudioSet(new List<string>() { officers_on_scene.Officersonscene.FileName },"officers on scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officersonsite.FileName },"officers on site"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded.FileName },"no additional officers needed"),
                new AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded1.FileName },"no additional officers needed"),
                new AudioSet(new List<string>() { no_further_units.Nofurtherunitsrequired.FileName },"no further units required"),
                new AudioSet(new List<string>() { no_further_units.WereCode4Adam.FileName },"we're code-4 adam"),
                new AudioSet(new List<string>() { no_further_units.Code4Adamnoadditionalsupportneeded.FileName },"code-4 adam no additional support needed"),
            },
        };
        SuspectArrested = new Dispatch()
        {
            Name = "Suspect Apprehended",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect.FileName },"officers have apprehended suspect"),
                new AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect1.FileName },"officers have apprehended suspect"),
            },
        };
        SuspectWasted = new Dispatch()
        {
            Name = "Suspect Neutralized",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crook_killed.Criminaldown.FileName },"criminal down"),
                new AudioSet(new List<string>() { crook_killed.Suspectdown.FileName },"suspect down"),
                new AudioSet(new List<string>() { crook_killed.Suspectneutralized.FileName },"suspect neutralized"),
                new AudioSet(new List<string>() { crook_killed.Suspectdownmedicalexaminerenroute.FileName },"suspect down, medical examiner in route"),
                new AudioSet(new List<string>() { crook_killed.Suspectdowncoronerenroute.FileName },"suspect down, coroner in route"),
                new AudioSet(new List<string>() { crook_killed.Officershavepacifiedsuspect.FileName },"officers have pacified suspect"),
             },
        };
        ChangedVehicles = new Dispatch()
        {
            Name = "Suspect Changed Vehicle",
            IsPoliceStatus = true,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { "" },""),
             },
        };
        RequestBackup = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Backup Required",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { assistance_required.Assistanceneeded.FileName },"assistance needed"),
                new AudioSet(new List<string>() { assistance_required.Assistancerequired.FileName },"Assistance required"),
                new AudioSet(new List<string>() { assistance_required.Backupneeded.FileName },"backup needed"),
                new AudioSet(new List<string>() { assistance_required.Backuprequired.FileName },"backup required"),
                new AudioSet(new List<string>() { assistance_required.Officersneeded.FileName },"officers needed"),
                new AudioSet(new List<string>() { assistance_required.Officersrequired.FileName },"officers required"),
             },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
             },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup1.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup2.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup3.FileName },"requesting backup"),
             },
        };
        RequestBackupSimple = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "Backup Required",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = false,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup1.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup2.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup3.FileName },"requesting backup"),
             },
        };
        WeaponsFree = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Weapons Free",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName },"suspect is armed and dangerous, weapons free"),
             },
        };
        LethalForceAuthorized = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Lethal Force Authorized",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            ResultsInLethalForce = true,
            CanAlwaysInterrupt = true,
        };
        SuspectEvaded = new Dispatch()
        {
            Name = "Suspect Evaded",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysInterrupt = false,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName },"suspect evaded pursuing officers"),
                new AudioSet(new List<string>() { suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName },"officers have lost visual on suspect"),
            },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
             },


            PreambleHelicopterAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.AirSupportLostSuspect.FileName },"air support lost suspect"),
             },
        };
        SuspectEvadedSimple = new Dispatch()
        {
            Name = "Suspect Evaded",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            CanAlwaysInterrupt = false,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName },"suspect evaded pursuing officers"),
                new AudioSet(new List<string>() { suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName },"officers have lost visual on suspect"),
            },
        };
        RemainInArea = new Dispatch()//runs when you lose wanted organicalls
        {
            Name = "Remain in Area",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all units remain on alert"),

              //  new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStandby.FileName },"all units standby"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all units remain on alert"),
            },
        };
        AttemptToReacquireSuspect = new Dispatch()//is the status one
        {
            Name = "Attempt To Reacquire",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { attempt_to_find.AllunitsATonsuspects20.FileName },"all units ATL on suspects 20"),
                new AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquire.FileName },"all units attempt to reacquire"),
                new AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquirevisual.FileName },"all units attempt to reacquire visual"),
                new AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect.FileName },"remain in the area, ATL-20 on suspect"),
                new AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect1.FileName },"remain in the area, ATL-20 on suspect"),
            },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
             },
            PreambleHelicopterAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.AirSupportLostSuspect.FileName },"air support lost suspect"),
             },
        };
        ResumePatrol = new Dispatch()
        {
            Name = "Resume Patrol",
            IsPoliceStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            NotificationTitle = "Emergency Scanner",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officer_begin_patrol.Beginpatrol.FileName },"begin patrol"),
                new AudioSet(new List<string>() { officer_begin_patrol.Beginbeat.FileName },"begin beat"),

                new AudioSet(new List<string>() { officer_begin_patrol.Assigntopatrol.FileName },"assign to patrol"),
                new AudioSet(new List<string>() { officer_begin_patrol.Proceedtopatrolarea.FileName },"proceed to patrol area"),
                new AudioSet(new List<string>() { officer_begin_patrol.Proceedwithpatrol.FileName },"proceed with patrol"),
            },
            //SecondaryAudioSet = new List<AudioSet>()
            //{
            //    new AudioSet(new List<string>() { officer_begin_patrol.Beginpatrol.FileName },"begin patrol"),
            //}
        };
    }

}

