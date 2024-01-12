using NAudio.CoreAudioApi.Interfaces;
using Rage;
using System;
using System.Collections.Generic;


public class DispatchableVehicles_Gangs
{
    public List<DispatchableVehicleGroup> GangVehicles { get; private set; }
    public List<DispatchableVehicle> BallasVehicles { get; private set; }
    public List<DispatchableVehicle> VagosVehicles { get; private set; }
    public List<DispatchableVehicle> FamiliesVehicles { get; private set; }
    public List<DispatchableVehicle> AncelottiVehicles { get; private set; }
    public List<DispatchableVehicle> MessinaVehicles { get; private set; }
    public List<DispatchableVehicle> LupisellaVehicles { get; private set; }
    public List<DispatchableVehicle> PavanoVehicles { get; private set; }
    public List<DispatchableVehicle> GambettiVehicles { get; private set; }
    public List<DispatchableVehicle> ArmenianVehicles { get; private set; }
    public List<DispatchableVehicle> CartelVehicles { get; private set; }
    public List<DispatchableVehicle> RedneckVehicles { get; private set; }
    public List<DispatchableVehicle> TriadVehicles { get; private set; }
    public List<DispatchableVehicle> KoreanVehicles { get; private set; }
    public List<DispatchableVehicle> MarabuntaVehicles { get; private set; }
    public List<DispatchableVehicle> DiablosVehicles { get; private set; }
    public List<DispatchableVehicle> VarriosVehicles { get; private set; }
    public List<DispatchableVehicle> LostVehicles { get; private set; }
    public List<DispatchableVehicle> YardiesVehicles { get; private set; }
    public void DefaultConfig()
    {
        SetBallasVehicles();
        SetVagosVehicles();
        SetFamiliesVehicles();
        SetAncelottiVehicles();
        SetMessinaVehicles();
        SetLupisellaVehicles();
        SetPavanoVehicles();
        SetGambettiVehicles();
        SetArmenianVehicles();
        SetCartelVehicles();
        SetRedneckVehicles();
        SetTriadVehicles();
        SetKoreanVehicles();
        SetMarabuntaVehicles();
        SetDiablosVehicles();
        SetVarriosVehicles();
        SetLostVehicles();
        SetYardiesVehicles();
    }
    private void SetBallasVehicles()
    {
        BallasVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("baller", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purple

            //Custom
            new DispatchableVehicle("primo2",20,20) {
            DebugName = "primo2_PeterBadoingy_DLCDespawn",
            MaxOccupants = 4,
            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 156,
            WheelType = 3,
            WindowTint = 3,
            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 36,
            },
            new VehicleMod() {
            ID = 25,
            Output = 6,
            },
            new VehicleMod() {
            ID = 27,
            Output = 1,
            },
            new VehicleMod() {
            ID = 28,
            Output = 6,
            },
            new VehicleMod() {
            ID = 30,
            Output = 4,
            },
            new VehicleMod() {
            ID = 33,
            Output = 1,
            },
            new VehicleMod() {
            ID = 34,
            Output = 2,
            },
            new VehicleMod() {
            ID = 35,
            Output = 13,
            },
            new VehicleMod() {
            ID = 36,
            Output = 3,
            },
            new VehicleMod() {
            ID = 37,
            Output = 6,
            },
            new VehicleMod() {
            ID = 38,
            Output = 3,
            },
            new VehicleMod() {
            ID = 39,
            Output = 5,
            },
            new VehicleMod() {
            ID = 40,
            Output = 3,
            },
            new VehicleMod() {
            ID = 43,
            Output = 2,
            },
            new VehicleMod() {
            ID = 44,
            Output = 1,
            },
            new VehicleMod() {
            ID = 45,
            Output = 2,
            },
            new VehicleMod() {
            ID = 48,
            Output = 1,
            },
            },

            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "impaler6_PeterBadoingy_DLCDespawn",
            ModelName = "impaler6",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 156,


            WheelType = 8,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 5,
            },
            new VehicleMod() {
            ID = 2,
            Output = 5,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            Output = 8,
            },
            new VehicleMod() {
            ID = 7,
            Output = 1,
            },
            new VehicleMod() {
            ID = 9,
            Output = 10,
            },
            new VehicleMod() {
            ID = 10,
            Output = 3,
            },
            new VehicleMod() {
            ID = 23,
            Output = 26,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "virgo2_PeterBadoingy_DLCDespawn",
            ModelName = "virgo2",
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 1,
            WheelColor = 156,

            Livery = 0,

            WheelType = 9,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 23,
            },
            new VehicleMod() {
            ID = 25,
            Output = 7,
            },
            new VehicleMod() {
            ID = 28,
            Output = 6,
            },
            new VehicleMod() {
            ID = 29,
            Output = -1,
            },
            new VehicleMod() {
            ID = 30,
            },
            new VehicleMod() {
            ID = 33,
            Output = 15,
            },
            new VehicleMod() {
            ID = 34,
            },
            new VehicleMod() {
            ID = 35,
            Output = 20,
            },
            new VehicleMod() {
            ID = 36,
            Output = 4,
            },
            new VehicleMod() {
            ID = 37,
            Output = 4,
            },
            new VehicleMod() {
            ID = 38,
            Output = 3,
            },
            new VehicleMod() {
            ID = 39,
            Output = 4,
            },
            new VehicleMod() {
            ID = 40,
            Output = 1,
            },
            new VehicleMod() {
            ID = 43,
            Output = 1,
            },
            new VehicleMod() {
            ID = 44,
            },
            new VehicleMod() {
            ID = 45,
            Output = 3,
            },
            new VehicleMod() {
            ID = 48,
            Output = 1,
            },
            },

            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "baller7_PeterBadoingy_DLCDespawn",
            ModelName = "baller7",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 156,


            WheelType = 3,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 1,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 9,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 30,
            },
            },

            InteriorColor = 93,
            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "baller_PeterBadoingy",
            ModelName = "baller",
            MaxOccupants = 4,
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 156,


            WheelType = 3,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 11,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 29,
            },
            },

            DashboardColor = 24,
            },
            },
            new DispatchableVehicle() {
            DebugName = "blazer4_PeterBadoingy_DLCDespawn",
            ModelName = "blazer4",
            MaxOccupants = 1,
            AmbientSpawnChance = 10,
            WantedSpawnChance = 10,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 156,


            WheelType = 9,


            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 5,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 5,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = -1,
            },
            new VehicleMod() {
            ID = 8,
            Output = 5,
            },
            new VehicleMod() {
            ID = 9,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 21,
            },
            },

            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetVagosVehicles()
    {
        VagosVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("chino", 20, 20){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },
            new DispatchableVehicle("chino2", 20, 20){ RequiredPrimaryColorID = 42,RequiredSecondaryColorID = 42 },//yellow

            //Custom
            new DispatchableVehicle() {
            DebugName = "SLAMVAN3_PeterBadoingy_DLCDespawn",
            ModelName = "SLAMVAN3",
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 118,
            PearlescentColor = 11,
            WheelColor = 120,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
            },
            new VehicleMod() {
            ID = 6,
            Output = 4,
            },
            new VehicleMod() {
            ID = 7,
            Output = 3,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 25,
            },
            new VehicleMod() {
            ID = 27,
            Output = 1,
            },
            new VehicleMod() {
            ID = 28,
            },
            new VehicleMod() {
            ID = 29,
            },
            new VehicleMod() {
            ID = 32,
            },
            new VehicleMod() {
            ID = 33,
            Output = 9,
            },
            new VehicleMod() {
            ID = 34,
            Output = 13,
            },
            new VehicleMod() {
            ID = 35,
            Output = 17,
            },
            new VehicleMod() {
            ID = 38,
            Output = 3,
            },
            new VehicleMod() {
            ID = 39,
            Output = 2,
            },
            new VehicleMod() {
            ID = 40,
            Output = 1,
            },
            new VehicleMod() {
            ID = 45,
            },
            new VehicleMod() {
            ID = 48,
            Output = 2,
            },
            },

            InteriorColor = 26,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "tulip2_PeterBadoingy_DLCDespawn",
            ModelName = "tulip2",
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 118,
            PearlescentColor = 2,
            WheelColor = 120,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 4,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 5,
            },
            new VehicleMod() {
            ID = 23,
            Output = 28,
            },
            new VehicleMod() {
            ID = 48,
            Output = 8,
            },
            },

            InteriorColor = 106,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "DEVIANT_PeterBadoingy_DLCDespawn",
            ModelName = "DEVIANT",
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 118,
            PearlescentColor = 11,
            WheelColor = 120,

            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 2,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 6,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            Output = 9,
            },
            new VehicleMod() {
            ID = 7,
            Output = 3,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 11,
            Output = 3,
            },
            new VehicleMod() {
            ID = 12,
            Output = 2,
            },
            new VehicleMod() {
            ID = 13,
            Output = 3,
            },
            new VehicleMod() {
            ID = 15,
            Output = 1,
            },
            new VehicleMod() {
            ID = 16,
            Output = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 23,
            },
            new VehicleMod() {
            ID = 48,
            Output = 4,
            },
            new VehicleMod() {
            ID = 50,
            Output = 3,
            },
            },

            InteriorColor = 2,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "IMPALER_PeterBadoingy_DLCDespawn",
            ModelName = "IMPALER",
            AmbientSpawnChance = 25,
            WantedSpawnChance = 25,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 118,
            PearlescentColor = 11,
            WheelColor = 120,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 1,
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 24,
            },
            new VehicleMod() {
            ID = 48,
            Output = 3,
            },
            },

            InteriorColor = 2,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "impaler5_PeterBadoingy_DLCDespawn",
            ModelName = "impaler5",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 11,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 11,
            PearlescentColor = 11,
            WheelColor = 120,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 4,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 14,
            },
            },

            InteriorColor = 8,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "dorado_PeterBadoingy_DLCDespawn",
            ModelName = "dorado",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 89,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 89,
            SecondaryColor = 118,
            PearlescentColor = 11,
            WheelColor = 120,

            WheelType = 3,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 9,
            },
            new VehicleMod() {
            ID = 1,
            Output = 8,
            },
            new VehicleMod() {
            ID = 2,
            Output = 8,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 6,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            Output = 8,
            },
            new VehicleMod() {
            ID = 9,
            Output = 5,
            },
            new VehicleMod() {
            ID = 15,
            Output = 3,
            },
            new VehicleMod() {
            ID = 23,
            Output = 7,
            },
            new VehicleMod() {
            ID = 48,
            Output = 8,
            },
            },

            InteriorColor = 3,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetFamiliesVehicles()
    {
        FamiliesVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("emperor",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("nemesis",15,15) {MaxOccupants = 1 },
            new DispatchableVehicle("buccaneer",15,15) { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green
            new DispatchableVehicle("tornado",15,15)  { RequiredPrimaryColorID = 53,RequiredSecondaryColorID = 53 },//green    

            //Custom
            new DispatchableVehicle("peyote3",25,25)
            {
                DebugName = "peyote3_PeterBadoingy_DLCDespawn",
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 8,
                    VehicleExtras = new List<VehicleExtra>() {
                    new VehicleExtra() {
                    },
                    new VehicleExtra() {
                    ID = 1,
                    IsTurnedOn = true,
                    },
                    new VehicleExtra() {
                    ID = 2,
                    },
                    new VehicleExtra() {
                    ID = 3,
                    },
                    new VehicleExtra() {
                    ID = 4,
                    },
                    new VehicleExtra() {
                    ID = 5,
                    },
                    },
                    VehicleToggles = new List<VehicleToggle>() {
                    new VehicleToggle() {
                    ID = 18,
                    IsTurnedOn = true,
                    },
                    },
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    ID = 4,
                    },
                    new VehicleMod() {
                    ID = 5,
                    },
                    new VehicleMod() {
                    ID = 6,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 9,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 26,
                    },
                    new VehicleMod() {
                    ID = 25,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 27,
                    Output = 7,
                    },
                    new VehicleMod() {
                    ID = 28,
                    Output = 5,
                    },
                    new VehicleMod() {
                    ID = 30,
                    },
                    new VehicleMod() {
                    ID = 33,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 35,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 39,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 43,
                    },
                    new VehicleMod() {
                    ID = 45,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 48,
                    Output = 1,
                    },
                    },
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("manana2",25,25)
            {
                DebugName = "manana2_PeterBadoingy_DLCDespawn",
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleExtras = new List<VehicleExtra>() {
                    new VehicleExtra() {
                    },
                    new VehicleExtra() {
                    ID = 1,
                    },
                    new VehicleExtra() {
                    ID = 2,
                    },
                    new VehicleExtra() {
                    ID = 3,
                    IsTurnedOn = true,
                    },
                    new VehicleExtra() {
                    ID = 4,
                    },
                    new VehicleExtra() {
                    ID = 5,
                    },
                    },
                    VehicleToggles = new List<VehicleToggle>() {
                    new VehicleToggle() {
                    ID = 18,
                    IsTurnedOn = true,
                    },
                    },
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    ID = 1,
                    },
                    new VehicleMod() {
                    ID = 4,
                    },
                    new VehicleMod() {
                    ID = 6,
                    },
                    new VehicleMod() {
                    ID = 8,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 24,
                    },
                    new VehicleMod() {
                    ID = 25,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 27,
                    },
                    new VehicleMod() {
                    ID = 28,
                    Output = 5,
                    },
                    new VehicleMod() {
                    ID = 33,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 34,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 37,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 38,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 39,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 40,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 42,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 48,
                    },
                    },
                    InteriorColor = 21,
                    DashboardColor = 4,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("glendale2",20,20)
            {
                DebugName = "glendale2_PeterBadoingy_DLCDespawn",
                MaxOccupants = 4,
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 9,
                    WindowTint = 3,
                    VehicleToggles = new List<VehicleToggle>() {
                    new VehicleToggle() {
                    ID = 17,
                    },
                    new VehicleToggle() {
                    ID = 18,
                    IsTurnedOn = true,
                    },
                    },
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    ID = 4,
                    Output = 5,
                    },
                    new VehicleMod() {
                    ID = 7,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 18,
                    },
                    new VehicleMod() {
                    ID = 25,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 27,
                    Output = 6,
                    },
                    new VehicleMod() {
                    ID = 28,
                    Output = 5,
                    },
                    new VehicleMod() {
                    ID = 33,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 34,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 36,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 37,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 38,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 39,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 45,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 48,
                    Output = 8,
                    },
                    },
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("greenwood",20,20)
            {
                DebugName = "greenwood_PeterBadoingy_DLCDespawn",
                MaxOccupants = 4,
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 3,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 4,
                    Output = 7,
                    },
                    new VehicleMod() {
                    ID = 8,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 24,
                    },
                    new VehicleMod() {
                    ID = 40,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 42,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 48,
                    Output = 3,
                    },
                    },
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("minivan2",20,20)
            {
                DebugName = "minivan2_PeterBadoingy_DLCDespawn",
                MaxOccupants = 4,
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleToggles = new List<VehicleToggle>() {
                    },
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    },
                    new VehicleMod() {
                    ID = 4,
                    },
                    new VehicleMod() {
                    ID = 5,
                    Output = -1,
                    },
                    new VehicleMod() {
                    ID = 6,
                    },
                    new VehicleMod() {
                    ID = 10,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 15,
                    },
                    new VehicleMod() {
                    ID = 24,
                    Output = -1,
                    },
                    new VehicleMod() {
                    ID = 25,
                    Output = 4,
                    },
                    new VehicleMod() {
                    ID = 27,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 28,
                    Output = 5,
                    },
                    new VehicleMod() {
                    ID = 31,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 32,
                    },
                    new VehicleMod() {
                    ID = 33,
                    Output = 6,
                    },
                    new VehicleMod() {
                    ID = 34,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 37,
                    Output = 6,
                    },
                    new VehicleMod() {
                    ID = 38,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 39,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 40,
                    Output = 1,
                    },
                    new VehicleMod() {
                    ID = 48,
                    },
                    },
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("aleutian",25,25)
            {
                DebugName = "aleutian_PeterBadoingy_DLCDespawn",
                MaxOccupants = 4,
                RequiredPrimaryColorID = 53,
                RequiredSecondaryColorID = 0,
                ForcedPlateType = 0,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 53,
                    PearlescentColor = 52,
                    WheelColor = 156,
                    WheelType = 3,
                    VehicleMods = new List<VehicleMod>() {
                    new VehicleMod() {
                    },
                    new VehicleMod() {
                    ID = 3,
                    Output = 2,
                    },
                    new VehicleMod() {
                    ID = 4,
                    Output = 6,
                    },
                    new VehicleMod() {
                    ID = 6,
                    Output = 6,
                    },
                    new VehicleMod() {
                    ID = 7,
                    Output = 3,
                    },
                    new VehicleMod() {
                    ID = 23,
                    Output = 22,
                    },
                    new VehicleMod() {
                    ID = 48,
                    },
                    },
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "manchez_PeterBadoingy_DLCDespawn",
            ModelName = "manchez",
            MaxOccupants = 1,
            AmbientSpawnChance = 10,
            WantedSpawnChance = 10,


            RequiredPrimaryColorID = 53,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 53,
            PearlescentColor = 52,
            WheelColor = 156,

            WheelType = 6,


            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = -1,
            },
            new VehicleMod() {
            ID = 7,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            },

            DashboardColor = 55,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetAncelottiVehicles()
    {
        AncelottiVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("huntley", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "huntley_PeterBadoingy",
            ModelName = "huntley",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 141,
            SecondaryColor = 141,
            PearlescentColor = 73,

            WheelType = 3,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 10,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 12,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 9,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "cogcabrio_PeterBadoingy",
            ModelName = "cogcabrio",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 141,
            SecondaryColor = 141,
            PearlescentColor = 73,


            WheelType = 12,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 17,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "cog55_PeterBadoingy_DLCDespawn",
            ModelName = "cog55",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 141,
            SecondaryColor = 141,
            PearlescentColor = 73,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 16,
            },
            },

            InteriorColor = 93,
            DashboardColor = 156,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetMessinaVehicles()
    {
        MessinaVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "superd_PeterBadoingy",
            ModelName = "superd",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 147,
            PearlescentColor = 147,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 29,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "windsor_PeterBadoingy_DLCDespawn",
            ModelName = "windsor",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 147,

            Livery = 0,

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 26,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "windsor2_PeterBadoingy_DLCDespawn",
            ModelName = "windsor2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 147,
            PearlescentColor = 147,


            WheelType = 12,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 2,
            },
            },

            InteriorColor = 21,
            DashboardColor = 27,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "jubilee_PeterBadoingy_DLCDespawn",
            ModelName = "jubilee",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 147,


            WheelType = 12,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 9,
            },
            new VehicleMod() {
            ID = 2,
            Output = 7,
            },
            new VehicleMod() {
            ID = 3,
            Output = 8,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 7,
            Output = 14,
            },
            new VehicleMod() {
            ID = 23,
            Output = 26,
            },
            },

            InteriorColor = 93,
            DashboardColor = 134,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetLupisellaVehicles()
    {
        LupisellaVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("huntley", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "komoda_PeterBadoingy_DLCDespawn",
            ModelName = "komoda",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 96,
            SecondaryColor = 96,
            PearlescentColor = 95,

            WheelType = 7,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 10,
            Output = 3,
            },
            new VehicleMod() {
            ID = 23,
            Output = 9,
            },
            },

            InteriorColor = 4,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "cypher_PeterBadoingy_DLCDespawn",
            ModelName = "cypher",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 96,
            SecondaryColor = 96,
            PearlescentColor = 95,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 5,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 7,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 10,
            Output = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 16,
            },
            new VehicleMod() {
            ID = 26,
            Output = 1,
            },
            new VehicleMod() {
            ID = 27,
            Output = 1,
            },
            new VehicleMod() {
            ID = 42,
            Output = 1,
            },
            new VehicleMod() {
            ID = 44,
            Output = 3,
            },
            },

            InteriorColor = 22,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "rhinehart_PeterBadoingy_DLCDespawn",
            ModelName = "rhinehart",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 96,
            SecondaryColor = 96,
            PearlescentColor = 95,

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 1,
            },
            new VehicleMod() {
            ID = 1,
            Output = 3,
            },
            new VehicleMod() {
            ID = 2,
            Output = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
            },
            new VehicleMod() {
            ID = 6,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 10,
            Output = 9,
            },
            new VehicleMod() {
            ID = 23,
            Output = 22,
            },
            },

            InteriorColor = 93,
            DashboardColor = 134,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetPavanoVehicles()
    {
        PavanoVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "tailgater_PeterBadoingy",
            ModelName = "tailgater",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 7,
            PearlescentColor = 5,


            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 2,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 5,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 23,
            Output = 22,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "iwagen_PeterBadoingy_DLCDespawn",
            ModelName = "iwagen",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 7,
            PearlescentColor = 5,

            WheelType = 3,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 6,
            },
            new VehicleMod() {
            ID = 23,
            Output = 11,
            },
            },

            InteriorColor = 93,
            DashboardColor = 134,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "rocoto_PeterBadoingy",
            ModelName = "rocoto",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 7,
            PearlescentColor = 5,
            WheelColor = 147,


            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 10,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 15,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "tailgater2_PeterBadoingy_DLCDespawn",
            ModelName = "tailgater2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 7,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 7,
            PearlescentColor = 5,

            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 5,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 5,
            Output = 5,
            },
            new VehicleMod() {
            ID = 7,
            Output = 3,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 7,
            },
            new VehicleMod() {
            ID = 25,
            },
            new VehicleMod() {
            ID = 27,
            Output = 2,
            },
            new VehicleMod() {
            ID = 42,
            Output = 2,
            },
            new VehicleMod() {
            ID = 44,
            Output = 4,
            },
            },

            InteriorColor = 5,
            DashboardColor = 111,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetGambettiVehicles()
    {
        GambettiVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("sentinel", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("sentinel2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "baller4_PeterBadoingy_DLCDespawn",
            ModelName = "baller4",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 2,
            PearlescentColor = 4,
            WheelColor = 147,
            WheelType = 3,
            WindowTint = 0,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 10,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 15,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 11,
            },
            },

            InteriorColor = 8,
            DashboardColor = 156,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "deity_PeterBadoingy_DLCDespawn",
            ModelName = "deity",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 2,
            PearlescentColor = 4,
            WheelColor = 147,


            WheelType = 12,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 4,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 4,
            },
            new VehicleMod() {
            ID = 6,
            Output = 2,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 10,
            Output = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 17,
            },
            },

            InteriorColor = 16,
            DashboardColor = 134,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "paragon_PeterBadoingy_DLCDespawn",
            ModelName = "paragon",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 2,
            PearlescentColor = 4,
            WheelColor = 147,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 8,
            Output = 1,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 26,
            },
            },

            InteriorColor = 13,
            DashboardColor = 111,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "baller8_PeterBadoingy_DLC",
            ModelName = "baller8",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 2,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 2,
            PearlescentColor = 4,
            WheelColor = 147,


            WheelType = 3,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 8,
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 5,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 9,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 11,
            Output = 3,
            },
            new VehicleMod() {
            ID = 50,
            Output = 3,
            },
            },

            InteriorColor = 99,
            DashboardColor = 6,
            },
            RequiresDLC = true,
            },
        };
    }
    private void SetArmenianVehicles()
    {
        ArmenianVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("schafter2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "dubsta2_PeterBadoingy_DLCDespawn",
            ModelName = "dubsta2",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,


            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 11,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 10,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 27,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "schafter3_PeterBadoingy_DLCDespawn",
            ModelName = "schafter3",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,
            PearlescentColor = 4,


            WheelType = 11,
            WindowTint = 2,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 2,
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            },
            new VehicleMod() {
            ID = 23,
            Output = 26,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "schafter4_PeterBadoingy_DLCDespawn",
            ModelName = "schafter4",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,


            WheelType = 11,
            WindowTint = 2,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 7,
            },
            new VehicleMod() {
            ID = 23,
            Output = 27,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "schwarzer_PeterBadoingy_DLCDespawn",
            ModelName = "schwarzer",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 12,
            WheelColor = 120,


            WheelType = 11,
            WindowTint = 2,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 12,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 4,
            },
            new VehicleMod() {
            ID = 6,
            Output = 2,
            },
            new VehicleMod() {
            ID = 7,
            Output = 11,
            },
            new VehicleMod() {
            ID = 9,
            Output = 3,
            },
            new VehicleMod() {
            ID = 10,
            Output = 3,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "schlagen_PeterBadoingy_DLCDespawn",
            ModelName = "schlagen",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,

            WheelType = 11,
            WindowTint = 2,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 6,
            },
            new VehicleMod() {
            ID = 9,
            Output = 1,
            },
            new VehicleMod() {
            ID = 10,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 16,
            },
            },

            },
            RequiresDLC = true,
            },
        };
    }
    private void SetCartelVehicles()
    {
        CartelVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("cavalcade2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black

            //Custom
            new DispatchableVehicle() {
            DebugName = "patriot3_PeterBadoingy_DLCDespawn",
            ModelName = "patriot3",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 5,
            },
            new VehicleMod() {
            ID = 2,
            Output = 5,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 8,
            },
            new VehicleMod() {
            ID = 7,
            Output = 1,
            },
            new VehicleMod() {
            ID = 8,
            Output = 1,
            },
            new VehicleMod() {
            ID = 10,
            Output = 7,
            },
            new VehicleMod() {
            ID = 23,
            },
            new VehicleMod() {
            ID = 28,
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "kamacho_PeterBadoingy_DLCDespawn",
            ModelName = "kamacho",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {


            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 1,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 2,
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 6,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 5,
            Output = 3,
            },
            new VehicleMod() {
            ID = 6,
            Output = 2,
            },
            new VehicleMod() {
            ID = 7,
            Output = 7,
            },
            new VehicleMod() {
            ID = 23,
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "hellion_PeterBadoingy_DLCDespawn",
            ModelName = "hellion",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 11,
            },
            new VehicleMod() {
            ID = 2,
            Output = 2,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 6,
            Output = 5,
            },
            new VehicleMod() {
            ID = 8,
            Output = 8,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 4,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "cavalcade2_PeterBadoingy_DLCDespawn",
            ModelName = "cavalcade2",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {


            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 11,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 23,
            Output = 27,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "granger2_PeterBadoingy_DLCDespawn",
            ModelName = "granger2",
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            },
            new VehicleMod() {
            ID = 8,
            Output = 1,
            },
            new VehicleMod() {
            ID = 10,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 28,
            },
            },

            },
            RequiresDLC = true,
            },
        };
    }
    private void SetRedneckVehicles()
    {
        RedneckVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("sandking2",10,10),
            new DispatchableVehicle("rebel", 20, 20),
            new DispatchableVehicle("bison", 20, 20),
            new DispatchableVehicle("sanchez2",20,20) {MaxOccupants = 1 },

            //Custom
            new DispatchableVehicle() {
            DebugName = "dukes3_PeterBadoingy_DLCDespawn",
            ModelName = "dukes3",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 21,
            SecondaryColor = 21,
            WheelColor = 2,

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 4,
            },
            new VehicleMod() {
            ID = 6,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            Output = 1,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 8,
            },
            new VehicleMod() {
            ID = 48,
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "slamvan3_PeterBadoingy_DLCDespawn",
            ModelName = "slamvan3",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 21,
            SecondaryColor = 118,
            WheelColor = 2,
            WheelType = 11,
            WindowTint = 3,
            DashboardColor = 131,
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 4,
            Output = 4,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 1,
            },
            new VehicleMod() {
            ID = 39,
            Output = 3,
            },
            new VehicleMod() {
            ID = 40,
            Output = 4,
            },
            new VehicleMod() {
            ID = 45,
            },
            new VehicleMod() {
            ID = 48,
            Output = 7,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "slamvan4_PeterBadoingy_DLCDespawn",
            ModelName = "slamvan4",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 118,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 21,
            SecondaryColor = 118,
            WheelColor = 2,

            WheelType = 1,


            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 2,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 42,
            Output = 2,
            },
            new VehicleMod() {
            ID = 48,
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "yosemite3_PeterBadoingy_DLCDespawn",
            ModelName = "yosemite3",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 21,
            SecondaryColor = 21,
            WheelColor = 120,

            WheelType = 4,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 1,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 2,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 3,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 9,
            Output = 10,
            },
            new VehicleMod() {
            ID = 23,
            },
            new VehicleMod() {
            ID = 48,
            Output = 14,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "broadway_PeterBadoingy_DLCDespawn",
            ModelName = "broadway",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 21,
            RequiredSecondaryColorID = 21,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 21,
            SecondaryColor = 21,
            WheelColor = 120,

            WheelType = 11,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            ID = 1,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 2,
            IsTurnedOn = true,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 5,
            Output = 3,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 23,
            Output = 7,
            },
            new VehicleMod() {
            ID = 48,
            Output = 9,
            },
            },

            },
            RequiresDLC = true,
            },
        };
    }
    private void SetTriadVehicles()
    {
        TriadVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("fugitive", 20, 20){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white
            new DispatchableVehicle("washington", 20, 20){ RequiredPrimaryColorID = 111,RequiredSecondaryColorID = 111 },//white

            //Custom
            new DispatchableVehicle() {
            DebugName = "ELEGY_PeterBadoingy_DLCDespawn",
            ModelName = "ELEGY",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 111,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 111,
            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 9,
            },
            new VehicleMod() {
            ID = 1,
            Output = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 5,
            Output = 1,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 7,
            Output = 4,
            },
            new VehicleMod() {
            ID = 8,
            Output = 2,
            },
            new VehicleMod() {
            ID = 10,
            },
            new VehicleMod() {
            ID = 23,
            Output = 19,
            },
            new VehicleMod() {
            ID = 26,
            },
            new VehicleMod() {
            ID = 27,
            },
            new VehicleMod() {
            ID = 31,
            },
            new VehicleMod() {
            ID = 32,
            Output = 5,
            },
            new VehicleMod() {
            ID = 33,
            Output = 2,
            },
            new VehicleMod() {
            ID = 39,
            Output = 2,
            },
            new VehicleMod() {
            ID = 40,
            Output = 7,
            },
            new VehicleMod() {
            ID = 41,
            Output = 1,
            },
            new VehicleMod() {
            ID = 43,
            Output = 6,
            },
            new VehicleMod() {
            ID = 45,
            Output = 1,
            },
            new VehicleMod() {
            ID = 46,
            Output = 1,
            },
            new VehicleMod() {
            ID = 48,
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "euros_PeterBadoingy_DLCDespawn",
            ModelName = "euros",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,
            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 10,
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 12,
            },
            new VehicleMod() {
            ID = 8,
            Output = 5,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 27,
            },
            new VehicleMod() {
            ID = 48,
            Output = 12,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "futo2_PeterBadoingy_DLCDespawn",
            ModelName = "futo2",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,

            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 1,
            },
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 2,
            Output = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 9,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 9,
            Output = 1,
            },
            new VehicleMod() {
            ID = 23,
            Output = 11,
            },
            new VehicleMod() {
            ID = 26,
            },
            new VehicleMod() {
            ID = 27,
            },
            new VehicleMod() {
            ID = 32,
            Output = 1,
            },
            new VehicleMod() {
            ID = 39,
            },
            new VehicleMod() {
            ID = 40,
            Output = 4,
            },
            new VehicleMod() {
            ID = 41,
            Output = 11,
            },
            new VehicleMod() {
            ID = 42,
            Output = 2,
            },
            new VehicleMod() {
            ID = 44,
            },
            new VehicleMod() {
            ID = 45,
            },
            new VehicleMod() {
            ID = 46,
            Output = 2,
            },
            new VehicleMod() {
            ID = 47,
            Output = 5,
            },
            new VehicleMod() {
            ID = 48,
            Output = 1,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "KURUMA_PeterBadoingy",
            ModelName = "KURUMA",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 16,
            },
            new VehicleMod() {
            ID = 48,
            Output = 3,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "HAKUCHOU_PeterBadoingy",
            ModelName = "HAKUCHOU",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,
            WheelColor = 27,

            WheelType = 6,


            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            },

            },
            },
        };
    }
    private void SetKoreanVehicles()
    {
        KoreanVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("feltzer2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("comet2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver
            new DispatchableVehicle("dubsta2", 20, 20){ RequiredPrimaryColorID = 4,RequiredSecondaryColorID = 4 },//silver

            //Custom
            new DispatchableVehicle() {
            DebugName = "JESTER3_PeterBadoingy_DLCDespawn",
            ModelName = "JESTER3",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            Output = 1,
            },
            new VehicleMod() {
            ID = 1,
            Output = 6,
            },
            new VehicleMod() {
            ID = 3,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 7,
            Output = 4,
            },
            new VehicleMod() {
            ID = 8,
            },
            new VehicleMod() {
            ID = 23,
            Output = 24,
            },
            new VehicleMod() {
            ID = 48,
            Output = 9,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "ZR350_PeterBadoingy_DLCDespawn",
            ModelName = "ZR350",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 3,
            Output = 10,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 3,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 8,
            Output = 2,
            },
            new VehicleMod() {
            ID = 9,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 14,
            },
            new VehicleMod() {
            ID = 48,
            Output = 12,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "RT3000_PeterBadoingy_DLCDespawn",
            ModelName = "RT3000",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            },
            new VehicleMod() {
            ID = 1,
            Output = 2,
            },
            new VehicleMod() {
            ID = 2,
            Output = 4,
            },
            new VehicleMod() {
            ID = 4,
            Output = 8,
            },
            new VehicleMod() {
            ID = 6,
            Output = 4,
            },
            new VehicleMod() {
            ID = 7,
            Output = 2,
            },
            new VehicleMod() {
            ID = 8,
            Output = 1,
            },
            new VehicleMod() {
            ID = 9,
            },
            new VehicleMod() {
            ID = 10,
            Output = 3,
            },
            new VehicleMod() {
            ID = 23,
            Output = 6,
            },
            new VehicleMod() {
            ID = 26,
            },
            new VehicleMod() {
            ID = 27,
            Output = 2,
            },
            new VehicleMod() {
            ID = 31,
            },
            new VehicleMod() {
            ID = 32,
            Output = 1,
            },
            new VehicleMod() {
            ID = 39,
            },
            new VehicleMod() {
            ID = 40,
            Output = 6,
            },
            new VehicleMod() {
            ID = 41,
            Output = 5,
            },
            new VehicleMod() {
            ID = 46,
            Output = 1,
            },
            new VehicleMod() {
            ID = 47,
            },
            new VehicleMod() {
            ID = 48,
            Output = 6,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "SULTAN2_PeterBadoingy_DLCDespawn",
            ModelName = "SULTAN2",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,


            WheelType = 11,
            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 3,
            },
            new VehicleMod() {
            ID = 2,
            Output = 3,
            },
            new VehicleMod() {
            ID = 3,
            Output = 2,
            },
            new VehicleMod() {
            ID = 4,
            Output = 8,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 2,
            },
            new VehicleMod() {
            ID = 7,
            Output = 7,
            },
            new VehicleMod() {
            ID = 9,
            Output = 4,
            },
            new VehicleMod() {
            ID = 23,
            Output = 16,
            },
            new VehicleMod() {
            ID = 48,
            Output = 8,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "DOUBLE_PeterBadoingy",
            ModelName = "DOUBLE",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,


            WheelType = 6,


            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 17,
            IsTurnedOn = true,
            },
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 4,
            },
            },

            },
            },
        };
    }
    private void SetMarabuntaVehicles()
    {
        MarabuntaVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("faction", 20, 20){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },
            new DispatchableVehicle("faction2", 20, 20){ RequiredPrimaryColorID = 70,RequiredSecondaryColorID = 70 },//blue

            //Custom
            new DispatchableVehicle("faction2",20,20) {
                DebugName = "faction2_PeterBadoingy_DLCDespawn",
                RequiredPrimaryColorID = 70,
                RequiredSecondaryColorID = 70,
                RequiredVariation = new VehicleVariation() 
                {
                    PrimaryColor = 70,
                    SecondaryColor = 70,
                    PearlescentColor = 38,
                    WheelColor = 90,
                    DashboardColor = 127,
                    WheelType = 9,
                    WindowTint = 3,
                    VehicleExtras = new List<VehicleExtra>() 
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,false),
                        new VehicleExtra(2,true),
                        new VehicleExtra(3,false),
                    },
                    VehicleToggles = new List<VehicleToggle>() 
                    {
                        new VehicleToggle(18,true) 
                    },
                    VehicleMods = new List<VehicleMod>() 
                    {
                        new VehicleMod(4,1),
                        new VehicleMod(7,0),
                        new VehicleMod(25,5),
                        new VehicleMod(27,1),
                        new VehicleMod(48,4),
                    },
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("chino2",20,20) {
                DebugName = "chino2_PeterBadoingy_DLCDespawn",
                RequiredPrimaryColorID = 70,
                RequiredSecondaryColorID = 70,
                RequiredVariation = new VehicleVariation() 
                {
                    PrimaryColor = 70,
                    SecondaryColor = 2,
                    WheelColor = 120,
                    DashboardColor = 127,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleExtras = new List<VehicleExtra>() 
                    {
                        new VehicleExtra(0,false),
                        new VehicleExtra(1,true),
                        new VehicleExtra(2,false),
                        new VehicleExtra(3,false),
                        new VehicleExtra(4,false),
                    },
                    VehicleToggles = new List<VehicleToggle>() 
                    {
                        new VehicleToggle(18,true),
                    },
                    VehicleMods = new List<VehicleMod>() 
                    {
                        new VehicleMod(1,0),
                        new VehicleMod(4,0),
                        new VehicleMod(9,1),
                        new VehicleMod(24,3),
                        new VehicleMod(25,5),
                        new VehicleMod(27,5),
                        new VehicleMod(37,4),
                        new VehicleMod(38,4),
                        new VehicleMod(39,3),
                        new VehicleMod(40,2),
                        new VehicleMod(45,1),
                        new VehicleMod(48,4),
                    },
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "chino2_PeterBadoingy_DLCDespawn",
            ModelName = "chino2",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 2,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 2,
            WheelColor = 120,
            DashboardColor = 127,
            WheelType = 1,
            WindowTint = 3,
            VehicleExtras = new List<VehicleExtra>() {
            new VehicleExtra() {
            },
            new VehicleExtra() {
            ID = 1,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 2,
            IsTurnedOn = true,
            },
            new VehicleExtra() {
            ID = 3,
            },
            },
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 9,
            Output = 1,
            },
            new VehicleMod() {
            ID = 24,
            Output = 3,
            },
            new VehicleMod() {
            ID = 25,
            Output = 5,
            },
            new VehicleMod() {
            ID = 27,
            Output = 5,
            },
            new VehicleMod() {
            ID = 37,
            Output = 4,
            },
            new VehicleMod() {
            ID = 38,
            Output = 4,
            },
            new VehicleMod() {
            ID = 39,
            Output = 3,
            },
            new VehicleMod() {
            ID = 40,
            Output = 2,
            },
            new VehicleMod() {
            ID = 45,
            Output = 1,
            },
            new VehicleMod() {
            ID = 48,
            Output = 7,
            },
            },
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "sabregt2_PeterBadoingy_DLCDespawn",
            ModelName = "sabregt2",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 70,
            WheelColor = 88,
            Livery = 0,
            WheelType = 1,
            WindowTint = 3,
            DashboardColor = 127,
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            },
            new VehicleMod() {
            ID = 4,
            },
            new VehicleMod() {
            ID = 24,
            Output = 3,
            },
            new VehicleMod() {
            ID = 25,
            Output = 5,
            },
            new VehicleMod() {
            ID = 27,
            Output = 7,
            },
            new VehicleMod() {
            ID = 36,
            Output = 1,
            },
            new VehicleMod() {
            ID = 37,
            Output = 4,
            },
            new VehicleMod() {
            ID = 38,
            Output = 3,
            },
            new VehicleMod() {
            ID = 39,
            Output = 6,
            },
            new VehicleMod() {
            ID = 40,
            Output = 3,
            },
            new VehicleMod() {
            ID = 48,
            Output = 7,
            },
            },
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "voodoo_PeterBadoingy_DLCDespawn",
            ModelName = "voodoo",
            AmbientSpawnChance = 20,
            WantedSpawnChance = 20,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 70,
            WheelColor = 156,
            DashboardColor = 127,
            WheelType = 9,
            WindowTint = 3,
            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 5,
            },
            new VehicleMod() {
            ID = 6,
            },
            new VehicleMod() {
            ID = 23,
            Output = 31,
            },
            new VehicleMod() {
            ID = 25,
            Output = 5,
            },
            new VehicleMod() {
            ID = 27,
            Output = 5,
            },
            new VehicleMod() {
            ID = 33,
            Output = 5,
            },
            new VehicleMod() {
            ID = 37,
            Output = 4,
            },
            new VehicleMod() {
            ID = 38,
            Output = 4,
            },
            new VehicleMod() {
            ID = 39,
            Output = 3,
            },
            new VehicleMod() {
            ID = 40,
            Output = 1,
            },
            new VehicleMod() {
            ID = 42,
            },
            new VehicleMod() {
            ID = 43,
            },
            new VehicleMod() {
            ID = 44,
            Output = 5,
            },
            new VehicleMod() {
            ID = 45,
            Output = 2,
            },
            new VehicleMod() {
            ID = 48,
            Output = 6,
            },
            },

            },
            RequiresDLC = true,
            },
        };
    }
    private void SetDiablosVehicles()
    {
        DiablosVehicles = new List<DispatchableVehicle>() {
            // Base
            new DispatchableVehicle("stalion", 25, 25) { RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28,},

            // Custom
            new DispatchableVehicle() {
              DebugName = "HERMES_PeterBadoingy_DLCDespawn",
              ModelName = "HERMES",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 120,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 22,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 4,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 13,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 15,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },

                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "stalion_PeterBadoingy",
              ModelName = "stalion",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 12,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {
                            ID = 1,
                          },
                          new VehicleExtra() {
                            ID = 2,
                            IsTurnedOn = true,
                          },
                          new VehicleExtra() {
                            ID = 3,
                          },
                        },
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 23,
                            Output = 1,
                          },
                        },

                  },
            },
            new DispatchableVehicle() {
              DebugName = "gauntlet3_PeterBadoingy_DLCDespawn",
              ModelName = "gauntlet3",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 12,

                    WheelType = 11,
                    WindowTint = 3,

                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 6,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 8,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 13,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },

                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "GAUNTLET5_PeterBadoingy_DLCDespawn",
              ModelName = "GAUNTLET5",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 120,

                    WheelType = 11,
                    WindowTint = 3,

                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 8,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 13,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 15,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 28,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 29,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 35,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 44,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },

                  },
              RequiresDLC = true,
            },
          };
    }
    private void SetVarriosVehicles()
    {
        VarriosVehicles = new List<DispatchableVehicle>() 
        {
            //Base
            new DispatchableVehicle("buccaneer", 20, 20){ RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68},

            // Custom
            new DispatchableVehicle() {
              DebugName = "BUCCANEER2_PeterBadoingy",
              ModelName = "BUCCANEER2",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 120,
                    WheelColor = 90,
                    DashboardColor = 127,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {
                            IsTurnedOn = true,
                          },
                          new VehicleExtra() {
                            ID = 1,
                          },
                          new VehicleExtra() {
                            ID = 2,
                          },
                          new VehicleExtra() {
                            ID = 3,
                          },
                          new VehicleExtra() {
                            ID = 4,
                          },
                        },
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                          },
                          new VehicleMod() {
                            ID = 6,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 8,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 25,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 28,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 34,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 35,
                            Output = 18,
                          },
                          new VehicleMod() {
                            ID = 36,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 37,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 38,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 40,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 5,
                          },
                        },

                  },
            },
            new DispatchableVehicle() {
              DebugName = "BUCCANEER2_PeterBadoingy",
              ModelName = "BUCCANEER2",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 120,
                    WheelColor = 90,

                    WheelType = 1,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {},
                          new VehicleExtra() {
                            ID = 1,
                          },
                          new VehicleExtra() {
                            ID = 2,
                            IsTurnedOn = true,
                          },
                          new VehicleExtra() {
                            ID = 3,
                          },
                          new VehicleExtra() {
                            ID = 4,
                          },
                        },
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                          },
                          new VehicleMod() {
                            ID = 6,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 8,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 25,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 26,
                            Output = -1,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 28,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 34,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 35,
                            Output = 18,
                          },
                          new VehicleMod() {
                            ID = 36,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 37,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 38,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 40,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 5,
                          },
                        },

                  },
            },
            new DispatchableVehicle() {
              DebugName = "VAMOS_PeterBadoingy_DLCDespawn",
              ModelName = "VAMOS",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 12,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {},
                          new VehicleMod() {
                            ID = 1,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 10,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "tulip_PeterBadoingy_DLCDespawn",
              ModelName = "tulip",
              MaxOccupants = 4,
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 12,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleToggles =
                        new List<VehicleToggle>() {
                          new VehicleToggle() {
                            ID = 17,
                            IsTurnedOn = true,
                          },
                          new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = true,
            },
        };
    }

    private void SetYardiesVehicles()
    {
        YardiesVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("virgo", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
            new DispatchableVehicle("voodoo2", 20, 20){ RequiredPrimaryColorID = 55,RequiredSecondaryColorID = 55 },//matte lime green
        };
    }

    private void SetLostVehicles()
    {
        LostVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("daemon", 70, 70) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 1 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 1 },
        };
    }


}

