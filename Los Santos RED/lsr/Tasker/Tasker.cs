using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod
{
    public class Tasker : ITaskerable
    {
        private IEntityProvideable PedProvider;
        private ITargetable Player;
        private IWeapons Weapons;
        private ISettingsProvideable Settings;
        private uint GameTimeLastGeneratedCrime;
        private uint RandomCrimeRandomTime;
        private uint RandomTrafficCrimeRandomTime;
        //private List<PedExt> PossibleTargets;
        //private Cop ClosestCopToPlayer;
        private IPlacesOfInterest PlacesOfInterest;
        //private List<AssignedSeat> SeatAssignments = new List<AssignedSeat>();


        private CopTasker CopTasker;
        private GangTasker GangTasker;
        private CivilianTasker CivilianTasker;
        private EMTTasker EMTTasker;
        private PedExt CurrentCriminal;
        private FirefighterTasker FirefighterTasker;



        private double AverageTimeBetweenCopUpdates = 0;
        private double AverageTimeBetweenCivUpdates = 0;
        private uint MaxTimeBetweenCopUpdates = 0;
        private uint MaxTimeBetweenCivUpdates = 0;
        private uint GameTimeLastTaskedPolice;
        private uint GameTimeLastTaskedCivilians;
        private uint GameTimeLastGeneratedTrafficCrime;

        public RelationshipGroup CriminalsRG { get; set; }
        public RelationshipGroup ZombiesRG { get; set; }
        private bool IsTimeToCreateCrime => Game.GameTime - GameTimeLastGeneratedCrime >= (Settings.SettingsManager.CivilianSettings.MinimumTimeBetweenRandomCrimes + RandomCrimeRandomTime);



        private bool IsTimeToCreateTrrafficCrime => Game.GameTime - GameTimeLastGeneratedTrafficCrime >= (Settings.SettingsManager.CivilianSettings.MinimumTimeBetweenRandomTrafficCrimes + RandomTrafficCrimeRandomTime);
        public string TaskerDebug => $"Cop Max: {MaxTimeBetweenCopUpdates} Avg: {AverageTimeBetweenCopUpdates} Civ Max: {MaxTimeBetweenCivUpdates} Avg: {AverageTimeBetweenCivUpdates}";
        public Tasker(IEntityProvideable pedProvider, ITargetable player, IWeapons weapons, ISettingsProvideable settings, IPlacesOfInterest placesOfInterest)
        {
            PedProvider = pedProvider;
            Player = player;
            Weapons = weapons;
            Settings = settings;
            GameTimeLastGeneratedCrime = Game.GameTime;
            RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
            PlacesOfInterest = placesOfInterest;
            CopTasker = new CopTasker(this, PedProvider, player, weapons, settings, PlacesOfInterest);
            GangTasker = new GangTasker(this, PedProvider, player, weapons, settings, PlacesOfInterest);
            CivilianTasker = new CivilianTasker(this, PedProvider, player, weapons, settings, PlacesOfInterest);

            EMTTasker = new EMTTasker(this, PedProvider, player, weapons, settings, PlacesOfInterest);
            EMTTasker.Setup();


            FirefighterTasker = new FirefighterTasker(this, PedProvider, player, weapons, settings, PlacesOfInterest);
            FirefighterTasker.Setup();
        }
        public void Setup()
        {
            CriminalsRG = new RelationshipGroup("CRIMINALS");
            RelationshipGroup.Cop.SetRelationshipWith(CriminalsRG, Relationship.Hate);
            CriminalsRG.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);

            ZombiesRG = new RelationshipGroup("ZOMBIES");


            RelationshipGroup CIVMALERG = new RelationshipGroup("CIVMALE");
            RelationshipGroup CIVFEMALERG = new RelationshipGroup("CIVFEMALE");

            RelationshipGroup.Cop.SetRelationshipWith(ZombiesRG, Relationship.Hate);
            ZombiesRG.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);

            RelationshipGroup.Player.SetRelationshipWith(ZombiesRG, Relationship.Hate);
            ZombiesRG.SetRelationshipWith(RelationshipGroup.Player, Relationship.Hate);

            Game.SetRelationshipBetweenRelationshipGroups(CIVMALERG, ZombiesRG, Relationship.Hate);
            Game.SetRelationshipBetweenRelationshipGroups(ZombiesRG, CIVMALERG, Relationship.Hate);
            Game.SetRelationshipBetweenRelationshipGroups(CIVFEMALERG, ZombiesRG, Relationship.Hate);
            Game.SetRelationshipBetweenRelationshipGroups(ZombiesRG, CIVFEMALERG, Relationship.Hate);

            NativeFunction.Natives.REQUEST_ANIM_SET<bool>("move_m@drunk@verydrunk");
        }
        public void UpdateCivilians()
        {
            if (Settings.SettingsManager.CivilianSettings.AllowRandomCrimes)
            {
                if (IsTimeToCreateCrime)
                {
                    CreateCrime(false);
                    GameFiber.Yield();
                }
                else if(IsTimeToCreateTrrafficCrime)
                {
                    CreateCrime(true);
                    GameFiber.Yield();
                }
            }
            CivilianTasker.Update();
            GameFiber.Yield();//TR 29
            GangTasker.Update();
            GameFiber.Yield();//TR 29
            EMTTasker.Update();
            GameFiber.Yield();//TR 29
            FirefighterTasker.Update();
            GameFiber.Yield();
            if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes)
            {
                EntryPoint.WriteToConsole($"Tasker.UpdateCivilians Ran Time Since {Game.GameTime - GameTimeLastTaskedCivilians}", 5);
            }
            if (CurrentCriminal != null && CurrentCriminal.CurrentTask?.Name == "CommitCrime")
            {
                CurrentCriminal.CurrentTask?.Update();
            }
            GameTimeLastTaskedCivilians = Game.GameTime;
        }
        public void UpdatePolice()
        {
            CopTasker.Update();
            if (Settings.SettingsManager.PerformanceSettings.PrintUpdateTimes)
            {
                EntryPoint.WriteToConsole($"Tasker.UpdatePolice Ran Time Since {Game.GameTime - GameTimeLastTaskedPolice}", 5);
            }
            GameTimeLastTaskedPolice = Game.GameTime;
        }
        public void CreateCrime(bool isTrafficOnly)
        {
            PedExt Criminal = PedProvider.Pedestrians.GangMemberList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.DistanceToPlayer <= 200f && x.CanBeTasked && x.CanBeAmbientTasked && ((isTrafficOnly && x.IsDriver && x.IsInVehicle) || (!isTrafficOnly && !x.IsInVehicle))).FirstOrDefault();//85f//150f
            if (Criminal == null)
            {
                Criminal = PedProvider.Pedestrians.CivilianList.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive && x.DistanceToPlayer <= 200f && x.CanBeTasked && x.CanBeAmbientTasked && ((isTrafficOnly && x.IsDriver && x.IsInVehicle) || (!isTrafficOnly && !x.IsInVehicle))).FirstOrDefault();//85f//150f
            }
            if (Criminal != null && Criminal.Pedestrian.Exists())
            {
                if (Settings.SettingsManager.CivilianSettings.ShowRandomCriminalBlips && Criminal.Pedestrian.Exists())
                {
                    Criminal.AddBlip();
                    //Blip myBlip = Criminal.Pedestrian.AttachBlip();
                    //EntryPoint.WriteToConsole($"CRIME PED BLIP BLIP CREATED");
                    //myBlip.Color = Color.Red;
                    //myBlip.Sprite = BlipSprite.CriminalWanted;
                    //myBlip.Scale = 1.0f;
                    //NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
                    //NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME("Criminal");
                    //NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
                    //PedProvider.AddBlip(myBlip);
                }
                Criminal.CanBeAmbientTasked = false;
                Criminal.WasSetCriminal = true;
                Criminal.WillCallPolice = false;
                Criminal.WillCallPoliceIntense = false;

                if (!Criminal.IsGangMember)
                {
                    //Criminal.Pedestrian.IsPersistent = true;
                    Criminal.Pedestrian.RelationshipGroup = CriminalsRG;
                }
                Criminal.CurrentTask = new CommitCrime(Criminal, Player, Weapons, PedProvider) { IsTrafficOnly = isTrafficOnly };
                Criminal.CurrentTask.Start();

                if (isTrafficOnly)
                {
                    GameTimeLastGeneratedTrafficCrime = Game.GameTime;
                    RandomTrafficCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);
                }
                else
                {
                    GameTimeLastGeneratedCrime = Game.GameTime;
                    RandomCrimeRandomTime = RandomItems.GetRandomNumber(0, 240000);//between 0 and 4 minutes randomly added
                                                                                   //EntryPoint.WriteToConsole("TASKER: GENERATED CRIME", 5);
                }
            }
        }
       

    }
}
