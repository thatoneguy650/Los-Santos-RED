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



    public List<DispatchableVehicle> NorthHollandVehicles { get; private set; }
    public List<DispatchableVehicle> PetrovicVehicles { get; private set; }
    public List<DispatchableVehicle> SpanishLordsVehicles { get; private set; }
    public List<DispatchableVehicle> AngelsOfDeathVehicles { get; private set; }
    public List<DispatchableVehicle> UptownRidersVehicles { get; private set; }
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


        SetNorthHollandVehicles();
        SetPetrovicVehicles();
        SetSpanishLordsVehicles();
        SetAngelsOfDeathVehicles();
        SetUptownRidersVehicles();

    }

    private void SetUptownRidersVehicles()
    {
        UptownRidersVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("burrito3",2,2) { },
            new DispatchableVehicle("double", 25, 25) { MaxOccupants = 1 },
            new DispatchableVehicle("bati", 25, 25) { MaxOccupants = 1 },
            new DispatchableVehicle("bati2", 25, 25) { MaxOccupants = 1,RequiresDLC = true },
            new DispatchableVehicle("hakuchou", 25, 25) { MaxOccupants = 1, },
            new DispatchableVehicle("hakuchou2", 25, 25) { MaxOccupants = 1,RequiresDLC = true },
            
            // Custom
            new DispatchableVehicle() {
              DebugName = "bati_PB_UptownRiders_ALT",
              ModelName = "bati",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 1,
              RequiredSecondaryColorID = 29,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 1,
                    SecondaryColor = 29,
                    PearlescentColor = 0,
                    WheelColor = 27,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "defiler_PB_UptownRiders_DLC",
              ModelName = "defiler",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 11,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 11,
                    SecondaryColor = 12,
                    PearlescentColor = 0,
                    WheelColor = 28,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 6,
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
              DebugName = "diablous2_PB_UptownRiders_DLC",
              ModelName = "diablous2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 111,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 111,
                    SecondaryColor = 12,
                    PearlescentColor = 0,
                    WheelColor = 121,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 9,
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
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 32,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 16,
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
              DebugName = "diablous2_2_PB_UptownRiders_DLC",
              ModelName = "diablous2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 118,
                    PearlescentColor = 3,
                    WheelColor = 28,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 6,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 41,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 41,
                          },
                          new VehicleMod() {
                            ID = 32,
                            Output = 13,
                          },
                          new VehicleMod() {
                            ID = 40,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 12,
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
              DebugName = "diablous2_3_PB_UptownRiders_DLC",
              ModelName = "diablous2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 27,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 27,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 27,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 2,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 14,
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
              DebugName = "diablous2_4_PB_UptownRiders_DLC",
              ModelName = "diablous2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 89,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 89,
                    SecondaryColor = 118,
                    PearlescentColor = 11,
                    WheelColor = 89,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 9,
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
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 39,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 39,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 16,
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
              DebugName = "hakuchou2_PB_UptownRiders_ALT_DLC",
              ModelName = "hakuchou2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 27,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 27,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 27,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 0,
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
              DebugName = "hakuchou2_2_PB_UptownRiders_ALT_DLC",
              ModelName = "hakuchou2",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 27,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 27,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 5,
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
                            ID = 16,
                            Output = 4,
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
              DebugName = "shinobi_PB_UptownRiders_DLC",
              ModelName = "shinobi",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 134,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 27,
                    SecondaryColor = 13,
                    PearlescentColor = 0,
                    WheelColor = 132,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
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
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 3,
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
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 13,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 4,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "shinobi_2_PB_UptownRiders_DLC",
              ModelName = "shinobi",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 112,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 112,
                    SecondaryColor = 0,
                    PearlescentColor = 12,
                    WheelColor = 1,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 3,
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
                            ID = 48,
                            Output = 11,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 4,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "shinobi_3_PB_UptownRiders_DLC",
              ModelName = "shinobi",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 112,
              RequiredSecondaryColorID = 21,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 112,
                    SecondaryColor = 21,
                    PearlescentColor = 0,
                    WheelColor = 95,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
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
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 3,
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
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 4,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "shinobi_4_PB_UptownRiders_DLC",
              ModelName = "shinobi",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 112,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 112,
                    SecondaryColor = 0,
                    PearlescentColor = 12,
                    WheelColor = 139,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 11,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 3,
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
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 4,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "vortex_PB_UptownRiders_DLC",
              ModelName = "vortex",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 39,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 39,
                    PearlescentColor = 0,
                    WheelColor = 132,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
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
                            Output = 3,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 9,
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
              DebugName = "vortex_2_PB_UptownRiders_DLC",
              ModelName = "vortex",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 89,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 89,
                    SecondaryColor = 118,
                    PearlescentColor = 11,
                    WheelColor = 120,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 24,
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
        };
    }
    private void SetAngelsOfDeathVehicles()
    {
        AngelsOfDeathVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("burrito3",2,2) { },
            new DispatchableVehicle("daemon", 25, 25) { MaxOccupants = 1,RequiredPrimaryColorID = 134,RequiredSecondaryColorID = 112 },
            new DispatchableVehicle("daemon2", 25, 25) { MaxOccupants = 1,RequiresDLC = true ,RequiredPrimaryColorID = 134,RequiredSecondaryColorID = 112},
            new DispatchableVehicle("nightblade", 25, 25) { MaxOccupants = 1,RequiresDLC = true ,RequiredPrimaryColorID = 134,RequiredSecondaryColorID = 112},
            new DispatchableVehicle("wolfsbane", 25, 25) { MaxOccupants = 1,RequiresDLC = true ,RequiredPrimaryColorID = 134,RequiredSecondaryColorID = 112},
            
            //Custom Burrito Van
            new DispatchableVehicle() {
              DebugName = "gburrito2_PB_AngelsOfDeath_DLC",
              ModelName = "gburrito2",
              AmbientSpawnChance = 30,
              WantedSpawnChance = 30,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 15,
              RequiredSecondaryColorID = 122,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 15,
                    SecondaryColor = 122,
                    PearlescentColor = 0,
                    WheelColor = 132,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output =0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 1,
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
                            ID = 10,
                            Output = 0,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 17,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            // Custom
            new DispatchableVehicle() {
              DebugName = "avarus_PB_AngelsOfDeath",
              ModelName = "avarus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 111,
              RequiredSecondaryColorID = 13,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 111,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
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
                            ID = 10,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 11,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "avarus2_PB_AngelsOfDeath_DLC",
              ModelName = "avarus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 11,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 11,
                    SecondaryColor = 12,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 44,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 44,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 10,
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
              DebugName = "avarus3_PB_AngelsOfDeath_DLC",
              ModelName = "avarus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 11,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 11,
                    SecondaryColor = 12,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
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
                            Output = 2,
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
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 45,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 45,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 9,
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
              DebugName = "daemon2_PB_AngelsOfDeath_DLC",
              ModelName = "daemon2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 6,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 47,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 47,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 10,
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
              DebugName = "hexer_PB_AngelsOfDeath_DLC",
              ModelName = "hexer",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 11,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 44,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 44,
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
              DebugName = "sanctus_PB_AngelsOfDeath_DLC",
              ModelName = "sanctus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 112,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 10,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "zombiea_PB_AngelsOfDeath_DLC",
              ModelName = "zombiea",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 112,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 12,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 9,
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
              DebugName = "zombiea2_PB_AngelsOfDeath_DLC",
              ModelName = "zombiea",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
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
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 2,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 10,
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
              DebugName = "zombieb_PB_AngelsOfDeath_DLC",
              ModelName = "zombieb",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 134,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 134,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
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
                            ID = 3,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 6,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 9,
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
    private void SetSpanishLordsVehicles()
    {
        SpanishLordsVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("primo2", 20, 20) { RequiresDLC = true,RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },
            new DispatchableVehicle("primo", 20, 20) {RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },
            new DispatchableVehicle("cavalcade", 25, 20) { RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },
            new DispatchableVehicle("cavalcade2", 25, 20) { RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },
            new DispatchableVehicle("cavalcade3", 2, 2) { RequiresDLC = true,RequiredPrimaryColorID = 68,RequiredSecondaryColorID = 68 },
              
            // Custom
            new DispatchableVehicle() {
              DebugName = "sultan_PB_SpanishLords",
              ModelName = "sultan",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 158,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 15,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "sultan2_PB_SpanishLords",
              ModelName = "sultan2",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 2,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 28,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "sultanrs_PB_SpanishLords_DLC",
              ModelName = "sultanrs",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 158,
                    WheelType = 0,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 14,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 29,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 32,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 41,
                            Output = 11,
                          },
                          new VehicleMod() {
                            ID = 42,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 0,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "banshee2_PB_SpanishLords_DLC",
              ModelName = "banshee2",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 158,
                    WheelType = 0,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 3,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 14,
                          },
                          new VehicleMod() {
                            ID = 29,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 32,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 41,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 0,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "tampa_PB_SpanishLords_DLC",
              ModelName = "tampa",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 158,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 8,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "sabregt_PB_SpanishLords",
              ModelName = "sabregt",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 158,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 12,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "tulip_PB_SpanishLords_DLC",
              ModelName = "tulip",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
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
                            Output = 0,
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
              DebugName = "Phoenix_PB_SpanishLords",
              ModelName = "Phoenix",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 50,
                            Output = 3,
                          },
                        },
                  },
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "hermes_PB_SpanishLords",
              ModelName = "hermes",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 158,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 12,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 29,
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
    private void SetPetrovicVehicles()
    {
        PetrovicVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("cognoscenti", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cogcabrio", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("ingot", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade2", 20, 20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            new DispatchableVehicle("cavalcade3", 20, 20) { RequiresDLC = true, RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },//black
            
            // Custom
            new DispatchableVehicle() {
              DebugName = "vorschlaghammer_PB_Petrovic_DLC",
              ModelName = "vorschlaghammer",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 120,
                    WheelType = 0,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 12,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 15,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 11,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "dubsta2_PB_Petrovic",
              ModelName = "dubsta2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 120,
                    WheelType = 3,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
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
              RequiresDLC = false,
            },
            new DispatchableVehicle() {
              DebugName = "novak_PB_Petrovic_DLC",
              ModelName = "novak",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 3,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
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
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 15,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 10,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "schafter3_PB_Petrovic_DLC",
              ModelName = "schafter3",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 0,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
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
              DebugName = "cheburek_PB_Petrovic_DLC",
              ModelName = "cheburek",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 0,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 0,
                    PearlescentColor = 0,
                    WheelColor = 0,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 5,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 9,
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
    private void SetNorthHollandVehicles()
    {
        NorthHollandVehicles = new List<DispatchableVehicle>()
        {
            new DispatchableVehicle("patriot3",2,2) { RequiresDLC = true, RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },
            new DispatchableVehicle("landstalker",20,20) { RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },
            new DispatchableVehicle("landstalker2",20,20) { RequiresDLC = true, RequiredPrimaryColorID = 0,RequiredSecondaryColorID = 0 },
            
            // Custom
            new DispatchableVehicle() {
              DebugName = "vstr_PB_NorthHolland_DLC",
              ModelName = "vstr",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 28,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 28,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 7,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 10,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 10,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "revolter_PB_NorthHolland_DLC",
              ModelName = "revolter",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 28,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 28,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 23,
                            Output = 28,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "buffalo4_PB_NorthHolland_DLC",
              ModelName = "buffalo4",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 1,
              RequiredSecondaryColorID = 1,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 28,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 12,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 17,
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
              DebugName = "dominator9_PB_NorthHolland_DLC",
              ModelName = "dominator9",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 11,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 12,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 9,
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
                            ID = 3,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 8,
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
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 22,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 1,
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
              DebugName = "dominator3_PB_NorthHolland_DLC",
              ModelName = "dominator3",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 28,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 12,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                       new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 15,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 15,
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
              DebugName = "gauntlet4_PB_NorthHolland_DLC",
              ModelName = "gauntlet4",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 28,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 28,
                    PearlescentColor = 35,
                    WheelColor = 0,
                    WheelType = 11,
                    WindowTint = 3,
                    VehicleMods =
                       new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
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
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 21,
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
                  },
              RequiresDLC = true,
            },
        };
    }

    private void SetBallasVehicles()
    {
        BallasVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("baller", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },
            new DispatchableVehicle("baller2", 20, 20){ RequiredPrimaryColorID = 145,RequiredSecondaryColorID = 145 },//purple

            //Custom
            new DispatchableVehicle("primo2",75,75) {
            DebugName = "primo2_PB_DLC",
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
            InteriorColor = 0,
            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "impaler6_PB_DLC",
            ModelName = "impaler6",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "virgo2_PB_DLC",
            ModelName = "virgo2",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "baller7_PB_DLC",
            ModelName = "baller7",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "baller_PB",
            ModelName = "baller",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "faction3_PB_DONK_DLC",
            ModelName = "faction3",
            MaxOccupants = 2,
            AmbientSpawnChance = 50,
            WantedSpawnChance = 50,


            RequiredPrimaryColorID = 145,
            RequiredSecondaryColorID = 105,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 145,
            SecondaryColor = 105,
            PearlescentColor = 105,
            WheelColor = 145,
            WheelType = 8,
            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 4,
            Output = 1,
            },
            new VehicleMod() {
            ID = 7,
            Output = 0,
            },
            new VehicleMod() {
            ID = 23,
            Output = 25,
            },
            new VehicleMod() {
            ID = 25,
            Output = 7,
            },
            new VehicleMod() {
            ID = 27,
            Output = 1,
            },
            new VehicleMod() {
            ID = 28,
            Output = 3,
            },
            new VehicleMod() {
            ID = 33,
            Output = 12,
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
            ID = 39,
            Output = 3,
            },
            new VehicleMod() {
            ID = 40,
            Output = 4,
            },
            },

            InteriorColor = 16,
            DashboardColor = 24,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "blazer4_PB_DLC",
            ModelName = "blazer4",
            MaxOccupants = 1,
            AmbientSpawnChance = 50,
            WantedSpawnChance = 50,


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
            DebugName = "SLAMVAN3_PB_Vagos_DLC",
            ModelName = "SLAMVAN3",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DashboardColor = 160,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "tulip2_PB_Vagos_DLC",
            ModelName = "tulip2",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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

            InteriorColor = 2,
            DashboardColor = 160,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "DEVIANT_PB_Vagos_DLC",
            ModelName = "DEVIANT",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "IMPALER_PB_Vagos_DLC",
            ModelName = "IMPALER",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DashboardColor = 160,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "impaler5_PB_Vagos_DLC",
            ModelName = "impaler5",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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

            InteriorColor = 2,
            DashboardColor = 157,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "dorado_PB_Vagos_DLC",
            ModelName = "dorado",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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

            InteriorColor = 2,
            DashboardColor = 160,
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
            new DispatchableVehicle("peyote3",75,75)
            {
                DebugName = "peyote3_PB_Families_DLC",
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
            new DispatchableVehicle("manana2",75,75)
            {
                DebugName = "manana2_PB_Families_DLC",
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
            new DispatchableVehicle("glendale2",75,75)
            {
                DebugName = "glendale2_PB_Families_DLC",
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
            new DispatchableVehicle("greenwood",75,75)
            {
                DebugName = "greenwood_PB_Families_DLC",
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
            new DispatchableVehicle("minivan2",75,75)
            {
                DebugName = "minivan2_PB_Families_DLC",
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
                    InteriorColor = 0,
                    DashboardColor = 55,
                },
                RequiresDLC = true,
            },
            new DispatchableVehicle("aleutian",75,75)
            {
                DebugName = "aleutian_PB_Families_DLC",
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
                    WindowTint = 3,
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
            DebugName = "manchez_PB_Families_DLC",
            ModelName = "manchez",
            MaxOccupants = 1,
            AmbientSpawnChance = 50,
            WantedSpawnChance = 50,


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
            DebugName = "huntley_PB_Ancelotti",
            ModelName = "huntley",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 75,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 75,
            SecondaryColor = 0,
            PearlescentColor = 157,
            WheelColor = 156,
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
            Output = 35,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "cogcabrio_PB_Ancelotti",
            ModelName = "cogcabrio",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 75,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 75,
            SecondaryColor = 0,
            PearlescentColor = 157,
            WheelColor = 156,
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
            ID = 23,
            Output = 28,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "cog55_PB_Ancelotti_DLC",
            ModelName = "cog55",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 141,
            RequiredSecondaryColorID = 141,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 75,
            SecondaryColor = 0,
            PearlescentColor = 157,
            WheelColor = 156,
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
            ID = 23,
            Output = 31,
            },
            },

            InteriorColor = 1,
            DashboardColor = 156,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "omnisegt_PB_Ancelotti_DLC",
            ModelName = "omnisegt",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 75,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 75,
            SecondaryColor = 0,
            PearlescentColor = 157,
            WheelColor = 156,
            WheelType = 7,
            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 0,
            Output = 0,
            },
            new VehicleMod() {
            ID = 1,
            Output = 2,
            },
            new VehicleMod() {
            ID = 2,
            Output = 2,
            },
            new VehicleMod() {
            ID = 6,
            Output = 0,
            },
            new VehicleMod() {
            ID = 8,
            Output = 0,
            },
            new VehicleMod() {
            ID = 10,
            Output = 3,
            },
            new VehicleMod() {
            ID = 23,
            Output = 31,
            },
            },

            InteriorColor = 4,
            DashboardColor = 156,
            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "drafter_PB_Ancelotti_DLC",
            ModelName = "drafter",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 75,
            RequiredSecondaryColorID = 0,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 75,
            SecondaryColor = 0,
            PearlescentColor = 157,
            WheelColor = 156,
            WheelType = 7,
            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 0,
            },
            new VehicleMod() {
            ID = 2,
            Output = 2,
            },
            new VehicleMod() {
            ID = 3,
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 3,
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
            ID = 8,
            Output = 0,
            },
            new VehicleMod() {
            ID = 10,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 28,
            },
            },

            InteriorColor = 111,
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
            DebugName = "superd_PB_Messina",
            ModelName = "superd",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 34,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 34,
            PearlescentColor = 47,

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
            DebugName = "windsor_PB_Messina_DLC",
            ModelName = "windsor",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 34,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 34,
            PearlescentColor = 47,

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
            DebugName = "windsor2_PB_Messina_DLC",
            ModelName = "windsor2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 34,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 34,
            PearlescentColor = 47,

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
            InteriorColor = 0,

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "jubilee_PB_Messina_DLC",
            ModelName = "jubilee",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 34,
            RequiredSecondaryColorID = 34,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 34,
            SecondaryColor = 34,
            PearlescentColor = 47,

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
            DebugName = "komoda_PB_DLC",
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
            DebugName = "cypher_PB_DLC",
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
            DebugName = "rhinehart_PB_DLC",
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
            DebugName = "tailgater_PB_Pavano",
            ModelName = "tailgater",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 52,
            RequiredSecondaryColorID = 52,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 52,
            SecondaryColor = 52,
            PearlescentColor = 59,
            DirtLevel = 5,
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
            DebugName = "iwagen_PB_Pavano_DLC",
            ModelName = "iwagen",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 52,
            RequiredSecondaryColorID = 52,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 52,
            SecondaryColor = 52,
            PearlescentColor = 59,
            DirtLevel = 5,
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
            DebugName = "rocoto_PB_Pavano",
            ModelName = "rocoto",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 52,
            RequiredSecondaryColorID = 52,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 52,
            SecondaryColor = 52,
            PearlescentColor = 59,
            DirtLevel = 5,
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
            DebugName = "tailgater2_PB_Pavano_DLC",
            ModelName = "tailgater2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 52,
            RequiredSecondaryColorID = 52,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 52,
            SecondaryColor = 52,
            PearlescentColor = 59,
            DirtLevel = 5,
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
            DebugName = "baller4_PB_Gambetti_DLC",
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
            WindowTint = 3,
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
            DebugName = "deity_PB_Gambetti_DLC",
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
            DebugName = "paragon_PB_Gambetti_DLC",
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
            DebugName = "baller8_PB_Gambetti_DLC",
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
            DebugName = "dubsta2_PB_Armenian_DLC",
            ModelName = "dubsta2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,


            WheelType = 11,
            WindowTint = 2,
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
            DebugName = "schafter3_PB_Armenian_DLC",
            ModelName = "schafter3",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,
            PearlescentColor = 4,

            InteriorColor = 1,
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
            DebugName = "schafter4_PB_Armenian_DLC",
            ModelName = "schafter4",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 147,

            InteriorColor = 1,
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
            DebugName = "schwarzer_PB_Armenian_DLC",
            ModelName = "schwarzer",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 147,
            RequiredSecondaryColorID = 147,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 147,
            SecondaryColor = 12,
            WheelColor = 0,


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
            ID = 0,
            Output = 0,
            },
            new VehicleMod() {
            ID = 1,
            Output = 0,
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
            new VehicleMod() {
            ID = 23,
            Output = 17,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "schlagen_PB_Armenian_DLC",
            ModelName = "schlagen",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "patriot3_PB_Cartel_DLC",
            ModelName = "patriot3",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {


            WheelType = 4,
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
            ID = 0,
            Output = 0,
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
            Output = 0,
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
            Output = 23,
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
            DebugName = "kamacho_PB_Cartel_DLC",
            ModelName = "kamacho",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {


            WheelType = 4,
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
            Output = 23,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "hellion_PB_Cartel_DLC",
            ModelName = "hellion",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 0,
            RequiredSecondaryColorID = 0,




            RequiredVariation = new VehicleVariation() {

            WheelType = 4,
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
            Output = 0,
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
            Output = 2,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "cavalcade2_PB_Cartel_DLC",
            ModelName = "cavalcade2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "granger2_PB_Cartel_DLC",
            ModelName = "granger2",
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "dukes3_PB_Redneck_DLC",
            ModelName = "dukes3",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 12,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 12,
            SecondaryColor = 12,

            DirtLevel = 10,

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
            DebugName = "slamvan3_PB_Redneck_DLC",
            ModelName = "slamvan3",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 12,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 12,
            SecondaryColor = 12,

            DirtLevel = 10,

            WheelColor = 2,
            WheelType = 4,

            WindowTint = 3,

            InteriorColor = 1,
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
            ID = 1,
            },
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
            Output = 23,
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
            ID = 48,
            Output = 7,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "slamvan4_PB_Redneck_DLC",
            ModelName = "slamvan4",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 12,
            RequiredSecondaryColorID = 118,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 12,
            SecondaryColor = 118,
            DirtLevel = 10,

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
            DebugName = "yosemite3_PB_Redneck_DLC",
            ModelName = "yosemite3",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 40,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 40,
            SecondaryColor = 12,

            DirtLevel = 10,

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
            Output = 20,
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
            DebugName = "broadway_PB_Redneck_DLC",
            ModelName = "broadway",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 12,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 12,
            SecondaryColor = 12,

            DirtLevel = 10,

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
            DebugName = "ELEGY_PB_Triad_DLC",
            ModelName = "ELEGY",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "euros_PB_Triad_DLC",
            ModelName = "euros",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "futo2_PB_Triad_DLC",
            ModelName = "futo2",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "KURUMA_PB_Triad",
            ModelName = "KURUMA",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


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
            DebugName = "tenf2_PB_Triad",
            ModelName = "tenf2",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,

            WheelType = 11,
            WheelColor = 0,

            InteriorColor  = 16,
            DashboardColor = 157,

            WindowTint = 3,

            VehicleToggles = new List<VehicleToggle>() {
            new VehicleToggle() {
            ID = 18,
            IsTurnedOn = true,
            },
            },
            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 0,
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
            Output = 1,
            },
            new VehicleMod() {
            ID = 4,
            Output = 18,
            },
            new VehicleMod() {
            ID = 7,
            Output = 13,
            },
            new VehicleMod() {
            ID = 8,
            Output = 4,
            },
            new VehicleMod() {
            ID = 9,
            Output = 5,
            },
            new VehicleMod() {
            ID = 10,
            Output = 8,
            },
            new VehicleMod() {
            ID = 15,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 27,
            },
            new VehicleMod() {
            ID = 27,
            Output = 0,
            },
            new VehicleMod() {
            ID = 29,
            Output = 3,
            },
            new VehicleMod() {
            ID = 44,
            Output = 3,
            },
            new VehicleMod() {
            ID = 48,
            Output = 5,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "jester4_PB_Triad",
            ModelName = "jester4",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 111,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 111,
            SecondaryColor = 12,
            PearlescentColor = 5,

            WheelType = 11,
            WheelColor = 0,

            InteriorColor  = 16,
            DashboardColor = 157,

            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 0,
            Output = 10,
            },
            new VehicleMod() {
            ID = 1,
            Output = 9,
            },
            new VehicleMod() {
            ID = 2,
            Output = 8,
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
            ID = 7,
            Output = 5,
            },
            new VehicleMod() {
            ID = 8,
            Output = 0,
            },
            new VehicleMod() {
            ID = 48,
            Output = 7,
            },
            },

            },
            },
            new DispatchableVehicle() {
            DebugName = "HAKUCHOU_PB_Triad",
            ModelName = "HAKUCHOU",
            AmbientSpawnChance = 50,
            WantedSpawnChance = 50,


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
            DebugName = "JESTER3_PB_Korean_DLC",
            ModelName = "JESTER3",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,
            WheelColor = 12,
            WheelType = 0,
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
            Output = 6,
            },
            new VehicleMod() {
            ID = 3,
            Output = 0,
            },
            new VehicleMod() {
            ID = 4,
            Output = 0,
            },
            new VehicleMod() {
            ID = 7,
            Output = 4,
            },
            new VehicleMod() {
            ID = 8,
            Output = 0,
            },
            new VehicleMod() {
            ID = 23,
            Output = 22,
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
            DebugName = "ZR350_PB_Korean_DLC",
            ModelName = "ZR350",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,
            WheelColor = 156,
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
            Output = 6,
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
            DebugName = "RT3000_PB_Korean_DLC",
            ModelName = "RT3000",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,
            WheelColor = 65,
            WheelType = 7,
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
            DebugName = "SULTAN2_PB_Korean_DLC",
            ModelName = "SULTAN2",
            MinOccupants = 2,
            MaxOccupants = 4,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,




            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,

            WheelColor = 158,
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
            DebugName = "comet6_PB_Korean_DLC",
            ModelName = "comet6",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,

            WheelColor = 12,
            WheelType = 12,

            InteriorColor = 16,
            DashboardColor = 134,

            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 0,
            Output = 6,
            },
            new VehicleMod() {
            ID = 1,
            Output = 2,
            },
            new VehicleMod() {
            ID = 2,
            Output = 6,
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
            ID = 7,
            Output = 5,
            },
            new VehicleMod() {
            ID = 8,
            Output = 2,
            },
            new VehicleMod() {
            ID = 23,
            Output = 14,
            },
            new VehicleMod() {
            ID = 25,
            Output = 0,
            },
            new VehicleMod() {
            ID = 26,
            Output = 0,
            },
            new VehicleMod() {
            ID = 27,
            Output = 0,
            },
            new VehicleMod() {
            ID = 43,
            Output = 0,
            },
            new VehicleMod() {
            ID = 48,
            Output = 5,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "growler_PB_Korean_DLC",
            ModelName = "growler",
            MaxOccupants = 2,
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,
            WheelColor = 12,
            WheelType = 0,
            InteriorColor = 16,
            DashboardColor = 67,
            WindowTint = 3,

            VehicleMods = new List<VehicleMod>() {
            new VehicleMod() {
            ID = 1,
            Output = 0,
            },
            new VehicleMod() {
            ID = 2,
            Output = 4,
            },
            new VehicleMod() {
            ID = 3,
            Output = 0,
            },
            new VehicleMod() {
            ID = 4,
            Output = 5,
            },
            new VehicleMod() {
            ID = 6,
            Output = 0,
            },
            new VehicleMod() {
            ID = 7,
            Output = 7,
            },
            new VehicleMod() {
            ID = 8,
            Output = 0,
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
            ID = 30,
            Output = 1,
            },
            new VehicleMod() {
            ID = 41,
            Output = 14,
            },
            new VehicleMod() {
            ID = 41,
            Output = 0,
            },
            new VehicleMod() {
            ID = 47,
            Output = 2,
            },
            new VehicleMod() {
            ID = 48,
            Output = 5,
            },
            },

            },
            RequiresDLC = true,
            },
            new DispatchableVehicle() {
            DebugName = "DOUBLE_PB_Korean",
            ModelName = "DOUBLE",
            AmbientSpawnChance = 50,
            WantedSpawnChance = 50,


            RequiredPrimaryColorID = 7,
            RequiredSecondaryColorID = 12,


            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 7,
            SecondaryColor = 12,
            PearlescentColor = 5,
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
            new DispatchableVehicle("faction2",75,75) {
                DebugName = "faction2_PB_Marabunta_DLC",
                RequiredPrimaryColorID = 70,
                RequiredSecondaryColorID = 70,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 70,
                    SecondaryColor = 70,
                    PearlescentColor = 38,
                    WheelColor = 90,
                    InteriorColor = 0,
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
            new DispatchableVehicle("chino2",75,75 ){
                DebugName = "chino2_PB_Marabunta_DLC",
                RequiredPrimaryColorID = 70,
                RequiredSecondaryColorID = 70,
                RequiredVariation = new VehicleVariation()
                {
                    PrimaryColor = 70,
                    SecondaryColor = 2,
                    WheelColor = 120,
                    InteriorColor = 0,
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
            DebugName = "chino2_PB_Marabunta_DLC",
            ModelName = "chino2",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 2,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 2,
            WheelColor = 120,
            InteriorColor = 0,
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
            DebugName = "sabregt2_PB_Marabunta_DLC",
            ModelName = "sabregt2",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 70,
            WheelColor = 88,
            Livery = 0,
            WheelType = 1,
            WindowTint = 3,
            InteriorColor = 0,
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
            DebugName = "voodoo_PB_Marabunta_DLC",
            ModelName = "voodoo",
            AmbientSpawnChance = 75,
            WantedSpawnChance = 75,
            RequiredPrimaryColorID = 70,
            RequiredSecondaryColorID = 70,
            RequiredVariation = new VehicleVariation() {
            PrimaryColor = 70,
            SecondaryColor = 70,
            WheelColor = 156,
            InteriorColor = 0,
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
            new DispatchableVehicle("stalion", 20, 20) { RequiredPrimaryColorID = 28,RequiredSecondaryColorID = 28,},

            // Custom
            new DispatchableVehicle() {
              DebugName = "HERMES_PB_Diablos_DLC",
              ModelName = "HERMES",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
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
              DebugName = "stalion_PB_Diablos",
              ModelName = "stalion",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
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
              DebugName = "gauntlet3_PB_Diablos_DLC",
              ModelName = "gauntlet3",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
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
              DebugName = "GAUNTLET5_PB_Diablos_DLC",
              ModelName = "GAUNTLET5",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              RequiredPrimaryColorID = 28,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 28,
                    SecondaryColor = 120,
                    DashboardColor = 134,
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
              DebugName = "BUCCANEER2_PB_Varrios",
              ModelName = "BUCCANEER2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 120,
                    WheelColor = 90,
                    InteriorColor = 156,
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
              DebugName = "BUCCANEER2_PB_Varrios",
              ModelName = "BUCCANEER2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              RequiredPrimaryColorID = 63,
              RequiredSecondaryColorID = 120,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 63,
                    SecondaryColor = 120,
                    WheelColor = 90,
                    InteriorColor = 156,
                    DashboardColor = 127,
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
              DebugName = "VAMOS_PB_Varrios_DLC",
              ModelName = "VAMOS",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
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
              DebugName = "tulip_PB_Varrios_DLC",
              ModelName = "tulip",
              MaxOccupants = 4,
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
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

            // Custom
            new DispatchableVehicle() {
              DebugName = "eudora_PB_Yardies_DLC",
              ModelName = "eudora",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 4,
              RequiredPrimaryColorID = 92,
              RequiredSecondaryColorID = 92,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 92,
                    SecondaryColor = 92,
                    PearlescentColor = 92,
                    WheelColor = 0,
                    InteriorColor = 159,
                    DashboardColor = 55,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 2,
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
                            ID = 9,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 17,
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
              DebugName = "buccaneer2_PB_Yardies_DLC",
              ModelName = "buccaneer2",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 92,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 92,
                    SecondaryColor = 118,
                    PearlescentColor = 92,
                    WheelColor = 0,
                    InteriorColor = 158,
                    DashboardColor = 55,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {},
                          new VehicleExtra() {
                            ID = 1,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 2,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 3,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 4,
                            IsTurnedOn = false,
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
                            ID = 7,
                          },
                          new VehicleMod() {
                            ID = 8,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 19,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 36,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 45,
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
              DebugName = "voodoo_PB_Yardies_DLC",
              ModelName = "voodoo",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 92,
              RequiredSecondaryColorID = 158,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 92,
                    SecondaryColor = 158,
                    PearlescentColor = 92,
                    WheelColor = 0,
                    InteriorColor = 159,
                    DashboardColor = 55,
                    WheelType = 9,
                    WindowTint = 3,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {},
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 24,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 34,
                            Output = 2,
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
                            ID = 42,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 43,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 48,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "tornado5_PB_Yardies_DLC",
              ModelName = "tornado5",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 2,
              RequiredPrimaryColorID = 92,
              RequiredSecondaryColorID = 37,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 92,
                    SecondaryColor = 37,
                    PearlescentColor = 92,
                    WheelColor = 0,
                    InteriorColor = 158,
                    DashboardColor = 55,
                    Livery = 1,
                    Livery2 = 4,
                    WheelType = 9,
                    WindowTint = 3,
                    VehicleToggles = new List<VehicleToggle>() {
                        new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 20,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 34,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 37,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 38,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 40,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 42,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 43,
                            Output = 1,
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
            new DispatchableVehicle() {
              DebugName = "manana2_PB_Yardies_DLC",
              ModelName = "manana2",
              MaxOccupants = 2,
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              RequiredPrimaryColorID = 92,
              RequiredSecondaryColorID = 37,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 92,
                    SecondaryColor = 37,
                    PearlescentColor = 92,
                    WheelColor = 0,
                    InteriorColor = 158,
                    DashboardColor = 55,
                    WheelType = 8,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {
                            ID = 0,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 1,
                            IsTurnedOn = true,
                          },
                          new VehicleExtra() {
                            ID = 2,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 3,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 4,
                            IsTurnedOn = false,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 27,
                          },
                          new VehicleMod() {
                            ID = 27,
                            Output = 13,
                          },
                          new VehicleMod() {
                            ID = 30,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 33,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 39,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 42,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 45,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 11,
                          },
                        },
                  },
              RequiresDLC = true,
            },
        };
    }
    private void SetLostVehicles()
    {
        LostVehicles = new List<DispatchableVehicle>()
        {
            //Base
            new DispatchableVehicle("daemon", 25, 25) { MaxOccupants = 1 },
            new DispatchableVehicle("slamvan2", 15, 15) { MaxOccupants = 2 },
            new DispatchableVehicle("gburrito", 15, 15) { MaxOccupants = 2 },

            //Custom
            new DispatchableVehicle() {
              DebugName = "avarus_PBDieHard_Lost_DLC",
              ModelName = "avarus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 131,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 131,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 120,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 11,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 11,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 11,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 11,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "avarus_PBRustFlag_Lost_DLC",
              ModelName = "avarus",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 12,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 12,
                    SecondaryColor = 12,
                    PearlescentColor = 0,
                    WheelColor = 2,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 21,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 21,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 15,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "zombiea_PBArmy_Lost_DLC",
              ModelName = "zombiea",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 12,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 12,
                    SecondaryColor = 118,
                    PearlescentColor = 0,
                    WheelColor = 147,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 0,
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
                            ID = 8,
                            Output = 5,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 8,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 11,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "zombieb_PBFlames_Lost_DLC",
              ModelName = "zombieb",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 12,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 12,
                    SecondaryColor = 118,
                    WheelColor = 147,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 22,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 22,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 0,
                          },
                        },
                  },
              RequiresDLC = true,
            },

            //Extra
            new DispatchableVehicle() {
              DebugName = "sanctus_PBGhostFlame_Lost_DLC",
              ModelName = "sanctus",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 13,
              RequiredSecondaryColorID = 5,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 13,
                    SecondaryColor = 5,
                    WheelColor = 73,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 0,
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
                            ID = 3,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 68,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 68,
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
              DebugName = "wolfsbane_PBPinstripe_Lost_DLC",
              ModelName = "wolfsbane",
              AmbientSpawnChance = 75,
              WantedSpawnChance = 75,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 0,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 0,
                    SecondaryColor = 118,
                    WheelColor = 112,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
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
                            ID = 7,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 6,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 21,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 21,
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
              DebugName = "wolfsbane_PBFatStripe_Lost_DLC",
              ModelName = "wolfsbane",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              MaxOccupants = 1,//2, when seat assigning added
              RequiredPrimaryColorID = 21,
              RequiredSecondaryColorID = 21,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 21,
                    SecondaryColor = 21,
                    WheelColor = 112,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                          },
                          new VehicleMod() {
                            ID = 1,
                          },
                          new VehicleMod() {
                            ID = 3,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 2,
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
                            ID = 23,
                            Output = 25,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 25,
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
              DebugName = "daemon2_PBClassicStripe_Lost_DLC",
              ModelName = "daemon2",
              AmbientSpawnChance = 25,
              WantedSpawnChance = 25,
              MaxOccupants = 1,//2, when seat assigning added
              RequiredPrimaryColorID = 21,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 21,
                    SecondaryColor = 118,
                    WheelColor = 147,
                    WheelType = 6,
                    VehicleToggles = new List<VehicleToggle>() {
                        new VehicleToggle() {
                            ID = 18,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
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
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 6,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 1,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 5,
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
                            ID = 16,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 50,
                          },
                          new VehicleMod() {
                            ID = 24,
                            Output = 50,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 5,
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
              DebugName = "chimera_PBPatinastripe_Lost_DLC",
              ModelName = "chimera",
              AmbientSpawnChance = 50,
              WantedSpawnChance = 50,
              MaxOccupants = 1,
              RequiredPrimaryColorID = 12,
              RequiredSecondaryColorID = 118,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 12,
                    SecondaryColor = 118,
                    WheelColor = 147,
                    WheelType = 6,
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 0,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 1,
                            Output = 3,
                          },
                          new VehicleMod() {
                            ID = 3,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 4,
                          },
                          new VehicleMod() {
                            ID = 5,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 8,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 10,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 21,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 17,
                          },
                        },
                  },
              RequiresDLC = true,
            },
            new DispatchableVehicle() {
              DebugName = "surfer3_PB_LOST_DLC",
              ModelName = "surfer3",
              MaxOccupants = 4,
              AmbientSpawnChance = 15,
              WantedSpawnChance = 15,
              RequiredPrimaryColorID = 12,
              RequiredSecondaryColorID = 12,
              RequiredVariation =
                  new VehicleVariation() {
                    PrimaryColor = 12,
                    SecondaryColor = 12,
                    WheelColor = 28,
                    WheelType = 1,
                    WindowTint = 3,
                    VehicleExtras =
                        new List<VehicleExtra>() {
                          new VehicleExtra() {
                            ID = 0,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 1,
                            IsTurnedOn = false,
                          },
                          new VehicleExtra() {
                            ID = 2,
                            IsTurnedOn = true,
                          },
                        },
                    VehicleMods =
                        new List<VehicleMod>() {
                          new VehicleMod() {
                            ID = 1,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 2,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 4,
                            Output = 7,
                          },
                          new VehicleMod() {
                            ID = 7,
                            Output = 0,
                          },
                          new VehicleMod() {
                            ID = 9,
                            Output = 2,
                          },
                          new VehicleMod() {
                            ID = 23,
                            Output = 9,
                          },
                          new VehicleMod() {
                            ID = 48,
                            Output = 7,
                          },
                        },
                  },
              RequiresDLC = true,
            },
        };
    }
}

