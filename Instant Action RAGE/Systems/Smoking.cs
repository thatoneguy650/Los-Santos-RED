using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Smoking
{
    public static CigarettePosition CurrentAttachedPosition = CigarettePosition.None;
    public static CigaretteAnimation CurrentAnimation = CigaretteAnimation.None;
    private static uint GameTimeLastEmitSmoke;
    public static bool IsRunning { get; set; } = true;
    public static Rage.Object PlayersCurrentCigarette { get; set; } = null;
    public static bool PlayerIsSmoking { get; set; } = false;
    public enum CigarettePosition
    {
        None = -1,
        Hand = 0,
        Mouth = 1,
    }
    public enum CigaretteAnimation
    {
        None = -1,
        Start = 0,
        Puffing = 1,
        Exit = 2,
        ExitStayInMouth = 3,
    }
    public static void Initialize()
    {
        MainLoop();
    }
    public static void MainLoop()
    {
        GameFiber.StartNew(delegate
        {
            while(IsRunning)
            {
                if (Game.GameTime - GameTimeLastEmitSmoke >= 3500 && CurrentAttachedPosition != CigarettePosition.None)
                {
                    CreateSmoke();
                    GameTimeLastEmitSmoke = Game.GameTime;
                }
                GameFiber.Yield();
            }

        });
    }
    public static bool CancelSmoking
    {
        get
        {
            if (Game.IsControlPressed(0, GameControl.Aim) || Game.IsControlPressed(0, GameControl.Sprint) || Game.IsControlPressed(0, GameControl.Enter))
                return true;
            else
                return false;
        }
    }
    public static void Start()
    {
        if (PlayerIsSmoking)
            return;
        GameFiber.StartNew(delegate
        {
            InstantAction.WriteToLog("Smoking", string.Format("StartedSmoking {0}", true));
            PlayerIsSmoking = true;
            SetPedUnarmed(Game.LocalPlayer.Character, false);
            bool Cancel = false;
            if (!PlayersCurrentCigarette.Exists())
            {
                InstantAction.WriteToLog("Smoking", string.Format("PlayersCurrentCigarette.Exists {0}", false));
                PlayersCurrentCigarette = new Rage.Object("ng_proc_cigarette01a", Game.LocalPlayer.Character.GetOffsetPositionUp(50f));
                if (CurrentAttachedPosition != CigarettePosition.Mouth)
                {
                    InstantAction.WriteToLog("Smoking", string.Format("CurrentAttachedPosition {0}", CurrentAttachedPosition));
                    Cancel = !PutCigaretteInMouth(false);
                }
            }
            if (Cancel)
            {
                InstantAction.WriteToLog("Smoking", string.Format("Cancel after PutInMouth {0}", Cancel));
                PlayerIsSmoking = false;
                return;        
            }
            Cancel = !StartPuffingCigarette();
            if (Cancel)
            {
                InstantAction.WriteToLog("Smoking", string.Format("Cancel after StartPuffing {0}", Cancel));
                Stop();
                PlayerIsSmoking = false;
                return;
            }
                

            //startPTFX("core", "ent_anim_cig_smoke");
            while (!Game.IsControlPressed(0, GameControl.Aim) && !Game.IsControlPressed(0, GameControl.Enter) && !Game.IsControlPressed(0, GameControl.Sprint))
            {
                GameFiber.Sleep(50);
            }
            InstantAction.WriteToLog("Smoking", string.Format("Cancel after waiting after puffing {0}", Cancel));
            Stop();
            PlayerIsSmoking = false;

        });
    }
    public static void CreateSmoke()
    {
        string Dictionary = "core";
        string FX = "ent_anim_cig_exhale_mth_car";

        LoadParticleDictionary(Dictionary);
        NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, Dictionary);
        int Particle = 0;
        if(CurrentAttachedPosition == CigarettePosition.Mouth)
            Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0.0f, 0.2f, 0f, 0f, 0f, 0f, 31086, 1.0f, false, false, false);
        else if(CurrentAttachedPosition == CigarettePosition.Hand)
            Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0.1f, 0.18f, 0f, 0f, 0f, 0f, 57005, 1.0f, false, false, false);

        InstantAction.WriteToLog("CreateSmoke", "CreateSmoke");
        GameFiber.Sleep(3500);
        NativeFunction.CallByName<int>("STOP_PARTICLE_FX_LOOPED", Particle, true);
        InstantAction.WriteToLog("CreateSmoke", "StopSMoke");

        //if (PlayersCurrentCigarette.Exists())
        //{
        //    string PTFX = "core";
        //    string FX = "ent_anim_cig_smoke";// "ent_anim_cig_exhale_mth_car";

        //    if (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
        //    {
        //        NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
        //        while (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
        //            GameFiber.Sleep(50);
        //    }


        //    //NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
        //    //GameFiber.Sleep(200);
        //    NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, PTFX);
        //    //NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", FX, offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 1.0f, false, false, false);
        //    //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 1f, false, false, false);
        //    //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_ENTITY", FX, Smoking.PlayersCurrentCigarette, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.0f, false, false, false);
        //    InstantAction.WriteToLog("DebugNumpad6", "StartLoop");
        //    int Particle = 0;
        //    //while (PlayersCurrentCigarette.Exists())
        //    //{
        //    // Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 2.0f, false, false, false);

        //    Particle = NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_ENTITY", FX, PlayersCurrentCigarette, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 2.0f, false, false, false);



        //    GameFiber.Sleep(2000);
        //    //}
        //    NativeFunction.CallByName<int>("STOP_PARTICLE_FX_LOOPED", Particle, true);
        //    InstantAction.WriteToLog("DebugNumpad6", "StopLoop");
        //}
    }
    public static int PlayParticleEffectOnEntity(string Dictionary,string Effect,Entity EntityToEffect,float Scale)
    {
        if (!EntityToEffect.Exists())
            return 0;

        LoadParticleDictionary(Dictionary);
        NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, Dictionary);
        int ParticleID =  NativeFunction.CallByName<int>("START_PARTICLE_FX_LOOPED_ON_ENTITY", Effect, EntityToEffect, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1065353216, false, false, false);
       // NativeFunction.CallByName<int>("SET_PARTICLE_FX_LOOPED_EVOLUTION", ParticleID, "LOD", 1.0f, 0);
        return ParticleID;
        
    }
    public static void LoadParticleDictionary(string Dictionary)
    {
        if (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", Dictionary))
        {
            NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", Dictionary);
            while (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", Dictionary))
                GameFiber.Sleep(50);
        }
    }
    public static void Stop()
    {
        if (!PlayersCurrentCigarette.Exists())
            return;

        if (Game.IsControlPressed(0, GameControl.Aim))
            StopPuffingCigarette();
        else
            StartPuffingCigaretteLeaveInMouth();
    }
    public static void SetPedUnarmed(Ped Pedestrian, bool SetCantChange)
    {
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, (uint)2725352035, true); //Unequip weapon so you don't get shot
        }
        if (SetCantChange)
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
    }
    //Start Puffing
    public static bool PutCigaretteInMouth(bool ClearTasks)
    {
        SetPedUnarmed(Game.LocalPlayer.Character, false);
        InstantAction.RequestAnimationDictionay("amb@world_human_smoking@male@male_a@enter");
        CurrentAnimation = CigaretteAnimation.Start;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "amb@world_human_smoking@male@male_a@enter", "enter", 4.0f, -4.0f, -1, 48, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 2500 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        if(CancelSmoking)
        {
            InstantAction.WriteToLog("PutCigaretteInMouth", string.Format("FirstCancel {0}", CancelSmoking));
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            return false;
        }
        AttachCigaretteToPedLowerLip(Game.LocalPlayer.Character);
        GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 7500 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        if (CancelSmoking)
        {
            InstantAction.WriteToLog("PutCigaretteInMouth", string.Format("SecondCanel {0}", CancelSmoking));
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            return false;
        }
        if (ClearTasks)
        {
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
        }
        InstantAction.WriteToLog("PutCigaretteInMouth", string.Format("Return {0}", true));
        return true;
    }
    //Puffing Animations
    public static bool StartPuffingCigarette()
    {
        InstantAction.RequestAnimationDictionay("amb@world_human_smoking@male@male_a@idle_a");
        CurrentAnimation = CigaretteAnimation.Puffing;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "amb@world_human_smoking@male@male_a@idle_a", "idle_c", 4.0f, -4.0f, -1, 49, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 2000 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        if (CancelSmoking)
        {
            CurrentAnimation = CigaretteAnimation.None;
            Game.LocalPlayer.Character.Tasks.Clear();
            return false;
        }
        AttachCigaretteToPedRightHand(Game.LocalPlayer.Character);
        return true;
    }
    //Stop Puffing
    public static bool StartPuffingCigaretteLeaveInMouth()
    {
        InstantAction.RequestAnimationDictionay("amb@world_human_smoking@male@male_a@idle_a");
        CurrentAnimation = CigaretteAnimation.ExitStayInMouth;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "amb@world_human_smoking@male@male_a@idle_a", "idle_c", 8.0f, -8.0f, -1, 48, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 2000)
        {
            GameFiber.Yield();
        }
        AttachCigaretteToPedLowerLip(Game.LocalPlayer.Character);
        GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 2000 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        Game.LocalPlayer.Character.Tasks.Clear();
        return true;
    }
    public static void StopPuffingCigarette()
    {
        InstantAction.RequestAnimationDictionay("amb@world_human_smoking@male@male_a@exit");
        CurrentAnimation = CigaretteAnimation.Exit;
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "amb@world_human_smoking@male@male_a@exit", "exit", 4.0f, -4.0f, -1, 48, 0, false, false, false);
        uint GameTimeStartedAnimation = Game.GameTime;
        while (Game.GameTime - GameTimeStartedAnimation <= 1500 && !CancelSmoking)
        {
            GameFiber.Yield();
        }
        CurrentAnimation = CigaretteAnimation.None;
        CurrentAttachedPosition = CigarettePosition.None;
        PlayersCurrentCigarette.Detach();    
        Game.LocalPlayer.Character.Tasks.Clear();
        GameFiber.Sleep(5000);
        if (PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette.Delete();
            PlayersCurrentCigarette = null;
        }
    }
    public static void AttachCigaretteToPedRightHand(Ped Pedestrian)
    {
        if (!PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette = new Rage.Object("ng_proc_cigarette01a", Pedestrian.GetOffsetPositionUp(50f));
            PlayParticleEffectOnEntity("core", "ent_anim_cig_smoke", PlayersCurrentCigarette, 2.0f);
            InstantAction.CreatedObjects.Add(PlayersCurrentCigarette);
        }
        //Right Hand
        int BoneIndexRightHand = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 57005);//58868 SKEL_R_Finger20 //57005 right hand
        PlayersCurrentCigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.14f, 0.03f, 0.0f), new Rotator(0.49f, 79f, 79f));
        //Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.03f, -0.02f, 0.01f), new Rotator(0f, 0f, 90f));//Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.14f, 0.03f, 0.0f), new Rotator(0.49f, 79f, 79f));//Cigarette.AttachTo(Pedestrian, BoneIndexRightHand, new Vector3(0.13f, 0.02f, 0.0f), new Rotator(0.49f, 79f, 79f));
        CurrentAttachedPosition = CigarettePosition.Hand;
    }
    public static void AttachCigaretteToPedLowerLip(Ped Pedestrian)
    {
        if (!PlayersCurrentCigarette.Exists())
        {
            PlayersCurrentCigarette = new Rage.Object("ng_proc_cigarette01a", Pedestrian.GetOffsetPositionUp(50f));
            PlayParticleEffectOnEntity("core", "ent_anim_cig_smoke", PlayersCurrentCigarette, 2.0f);
            InstantAction.CreatedObjects.Add(PlayersCurrentCigarette);
        }
        int BoneIndexLowerLip = NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Game.LocalPlayer.Character, 17188);//17188 ll root--//20623 LL reg
        PlayersCurrentCigarette.AttachTo(Pedestrian, BoneIndexLowerLip, new Vector3(0.046f, 0.015f, 0.014f), new Rotator(0.0f, -180f, 0f)); //Cigarette.AttachTo(Pedestrian, BoneIndexLowerLip, new Vector3(0.03f, 0.01f, 0.0f), new Rotator(0.0f, -127f, 0f)); //Cigarette.AttachTo(Pedestrian, BoneIndexLowerLip, new Vector3(0.02f, 0.0f, 0.0f), new Rotator(0.0f, -58f, -164f));//.03,.01 -127f
        CurrentAttachedPosition = CigarettePosition.Mouth;
    }
    
    public static void startPTFX(string PTFX,string FX)
    {
        if (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
        {
            NativeFunction.CallByName<bool>("REQUEST_NAMED_PTFX_ASSET", PTFX);
            while (!NativeFunction.CallByName<bool>("HAS_NAMED_PTFX_ASSET_LOADED", PTFX))
                GameFiber.Sleep(50);
        }



        Vector3 offset = Game.LocalPlayer.Character.GetOffsetPositionFront(2f);
        NativeFunction.CallByHash<bool>(0x6C38AF3693A69A91, FX);
        //NativeFunction.CallByName<bool>("START_PARTICLE_FX_LOOPED_ON_PED_BONE", FX, Game.LocalPlayer.Character, 0f, 0f, 0f, 0f, 0f, 0f, 57005, 1f, false, false, false);
        uint TimeStarted = Game.GameTime;
        GameFiber.StartNew(delegate
        {
            while (Game.GameTime - TimeStarted <= 15000)
            {
                NativeFunction.CallByName<bool>("START_PARTICLE_FX_NON_LOOPED_AT_COORD", "scr_carsteal5_car_muzzle_flash", offset.X, offset.Y, offset.Z, 0f, 0f, 0f, 1.0f, false, false, false);
                GameFiber.Yield();
            }
        });
    }

    /*
     * 
     * if(get_key_pressed(Keys.B)) then  
     * ptfx = "scr_rcbarry1"       
     * fx = "scr_alien_teleport"       
     * fx_loop = 25     
     * i=0      
     * coords = ENTITY.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS(playerPed, 0, 0, 0)     
     * while (i<fx_loop) do        
     * STREAMING.REQUEST_NAMED_PTFX_ASSET(ptfx)            
     * GRAPHICS._SET_PTFX_ASSET_NEXT_CALL(ptfx)      
     * GRAPHICS.START_PARTICLE_FX_NON_LOOPED_AT_COORD(fx, coords.x, coords.y, coords.z, 0.0, 0.0, 0.0, 0x3F800000, false, false, false)   
     * i=i+1          
     * wait(25)         
     * print("Loop: " .. i)    
     * end                     
     * ENTITY.SET_ENTITY_ALPHA(playerPed, 0, false)       
     * wait(4000)                
     * ENTITY.SET_ENTITY_COORDS_NO_OFFSET(playerPed, 1747.0, 3273.7, 41.1 , false, false, true) --teleports you to the desert airfield    
     * ENTITY.SET_ENTITY_ALPHA(playerPed, 0, false)       
     * coords = ENTITY.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS(playerPed, 0, 0, 0)      
     * wait(1000)                
     * i=0                   
     * while (i<fx_loop) do      
     * STREAMING.REQUEST_NAMED_PTFX_ASSET(ptfx)    
     * GRAPHICS._SET_PTFX_ASSET_NEXT_CALL(ptfx)      
     * GRAPHICS.START_PARTICLE_FX_NON_LOOPED_AT_COORD(fx, coords.x, coords.y, coords.z, 0.0, 0.0, 0.0, 0x3F800000, false, false, false)   
     * i=i+1    
     * wait(25)        
     * print("Loop: " .. i)        
     * end                  
     * ENTITY.SET_ENTITY_ALPHA(playerPed, 255, false)   
     * end
     * */
}

