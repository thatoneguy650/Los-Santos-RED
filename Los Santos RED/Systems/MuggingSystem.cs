using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MuggingSystem
{
    private static bool IsMugging = false;
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        MainLoop();
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            try
            {
                while (IsRunning)
                {
                    MuggingTick();
                    GameFiber.Yield();
                }
            }
            catch (Exception e)
            {
                Dispose();
                Debugging.WriteToLog("Error", e.Message + " : " + e.StackTrace);
            }
        });
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    private static void MuggingTick()
    {
        if(!IsMugging && Game.LocalPlayer.Character.IsAiming && Game.LocalPlayer.IsFreeAimingAtAnyEntity)
        {
            Entity Target = Game.LocalPlayer.GetFreeAimingTarget();

            if (!(Target is Ped))
                return;

            if (PoliceScanning.CopPeds.Any(x => x.Pedestrian.Handle == Target.Handle))
                return;//aiming at cop

            GTAPed GTAPedTarget = PoliceScanning.Civilians.FirstOrDefault(x => x.Pedestrian.Handle == Target.Handle);

            if (GTAPedTarget == null)
                GTAPedTarget = new GTAPed((Ped)Target, false, 100);

            bool CanSee = GTAPedTarget.Pedestrian.CanSeePlayer();

            if (!GTAPedTarget.HasBeenMugged && CanSee && GTAPedTarget.DistanceToPlayer <= 15f && !GTAPedTarget.Pedestrian.IsInAnyVehicle(false))
            {
                //Game.DisplayHelp("Press E to Mug Ped",5000);
                //uint GameTimeStartedMugging = Game.GameTime;
                //bool StartMugging = false;
                //while (Game.GameTime - GameTimeStartedMugging <= 5000)
                //{
                //    if (Game.IsKeyDownRightNow(Settings.SurrenderKey))
                //    {
                //        StartMugging = true;
                //        break;
                //    }
                //    GameFiber.Yield();
                //}
                    
                //if(StartMugging)
                    MugTarget(GTAPedTarget);
            }
        }
    }
    private static void MugTarget(GTAPed MuggingTarget)
    {
        GameFiber.StartNew(delegate
        {
            IsMugging = true;
            MuggingTarget.Pedestrian.BlockPermanentEvents = true;

            LosSantosRED.RequestAnimationDictionay("ped");

            Game.LocalPlayer.Character.PlayAmbientSpeech("CHALLENGE_THREATEN");

            GameFiber.Sleep(500);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", MuggingTarget.Pedestrian, "ped", "handsup_enter", 2.0f, -2.0f, -1, 2, 0, false, false, false);
            MuggingTarget.Pedestrian.PlayAmbientSpeech("GUN_BEG");

            uint GameTimeStartedMugging = Game.GameTime;
            bool Intimidated = false;
            while (Game.GameTime - GameTimeStartedMugging <= 4000)
            {                
                if (!Game.LocalPlayer.IsFreeAiming)
                {
                    Intimidated = false;
                    break;
                }
                Intimidated = true;
                GameFiber.Yield();
            }

            if (Intimidated)
            {
                Game.LocalPlayer.Character.PlayAmbientSpeech("GENERIC_INSULT");

                GameFiber.Sleep(250);
                Vector3 MoneyPos = MuggingTarget.Pedestrian.Position.Around2D(0.5f, 1.5f);
                NativeFunction.CallByName<bool>("CREATE_AMBIENT_PICKUP", Game.GetHashKey("PICKUP_MONEY_VARIABLE"), MoneyPos.X, MoneyPos.Y, MoneyPos.Z, 0, LosSantosRED.MyRand.Next(15, 450), 1, false, true);
                MuggingTarget.HasBeenMugged = true;
            }
            MuggingTarget.Pedestrian.BlockPermanentEvents = false;
            //MuggingTarget.Pedestrian.Tasks.Clear();


            MuggingTarget.Pedestrian.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
            IsMugging = false;

           // if(LosSantosRED.MyRand.Next(1,11)<= 4)
                WatchForDeath(MuggingTarget);


        });
    }
    public static void WatchForDeath(GTAPed MyPed)
    {
        GameFiber WatchForDeath = GameFiber.StartNew(delegate
        {
            uint GameTimeStolen = Game.GameTime;
            while (MyPed.Pedestrian.Exists())
            {
                MyPed.Pedestrian.IsPersistent = true;

                if (MyPed.Pedestrian.IsDead)
                {
                    MyPed.Pedestrian.IsPersistent = false;
                    break;
                }
                else if (Game.GameTime - GameTimeStolen > 15000 && !MyPed.Pedestrian.IsRagdoll)
                {
                    NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", MyPed.Pedestrian, 10000);
                    MyPed.Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");
                    GameFiber.Sleep(5000);

                    Police.SetWantedLevel(1, "Mugging Peds");
                    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportLowLevelMugging, 10));
                    if (MyPed.Pedestrian.Exists() && !MyPed.Pedestrian.IsDead && !MyPed.Pedestrian.IsRagdoll)
                    {
                        MyPed.Pedestrian.IsPersistent = false;
                    }
                    break;
                }

                GameFiber.Yield();
            }
        }, "WatchForDeath");
        Debugging.GameFibers.Add(WatchForDeath);
    }

    //DeadlyChaseSpeech = new List<string> { "CHALLENGE_THREATEN","GUN_BEG" };

}
