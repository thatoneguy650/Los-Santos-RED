﻿using ExtensionsMethods;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Linq;

public class HaircutInteract : InteriorInteract
{
    private Texture BannerImage;
    private PedExt Hairstylist;
    private Rage.Object Scissors;

    public Vector3 AnimEnterPosition { get; set; }
    public Vector3 AnimEnterRotation { get; set; }
    public override bool ShouldAddPrompt => (BarberShop == null || !BarberShop.SpawnedVendors.Any()) ? false : base.ShouldAddPrompt;
    public BarberShop BarberShop { get; set; }
    public HaircutInteract()
    {

    }
    public HaircutInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteriorLoaded()
    {

    }
    public override void OnInteract()
    {
        if (BarberShop == null)
        {
            return;
        }
        if(BarberShop.SpawnedVendors == null || !BarberShop.SpawnedVendors.Any())
        {
            Game.DisplayHelp("No barbers available");
            return;
        }
        Hairstylist = BarberShop.SpawnedVendors?.PickRandom();
        if (Hairstylist == null || !Hairstylist.Pedestrian.Exists() || Hairstylist.IsLSRFleeing)
        {
            Game.DisplayHelp("No barbers available");
            return;
        }
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        PerformAnimation();
        Interior.IsMenuInteracting = false;
        if(Scissors.Exists())
        {
            Scissors.Delete();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    public bool StopPerformingAnimation()
    {
        return true;
    }
    private bool PerformAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("misshair_shop@hair_dressers");
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", 0, "misshair_shop@hair_dressers", "player_enterchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        CreateScissors();
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_enterchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);    
        }
        if(Scissors != null && Scissors.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Scissors, "misshair_shop@hair_dressers", "scissors_enterchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        }

        bool IsCancelled = false;
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "misshair_shop@hair_dressers", "player_enterchair");
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            if (AnimationTime >= 1.0f)
            {
                break;
            }
            GameFiber.Yield();
        }
        if (!IsCancelled)
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5641, 0.0f, 2, 1);
            if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5641, 0.0f, 2, 1);
            }
            if (Scissors != null && Scissors.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Scissors, "misshair_shop@hair_dressers", "scissors_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5641, 0.0f, 2, 1);
            }
            ShowHaircutMenu();
        }
        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Game.LocalPlayer.Character, "misshair_shop@hair_dressers", "player_exitchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_exitchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            if (Scissors != null && Scissors.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Scissors, "misshair_shop@hair_dressers", "scissors_exitchair", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            }
        }
        //LocationCamera?.ReturnToGameplay(true);
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "misshair_shop@hair_dressers", "player_exitchair");
            if (!Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            if (AnimationTime >= 0.7f)
            {
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Hairstylist.Pedestrian);
        }
        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void PlayHaircutAnimation(int cost)
    {
        bool hasApplied = false;
        bool hasShownStuff = false;
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole("HAIRCUT ANIM START STYLIST");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_hair_cut_a", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            if (Scissors != null && Scissors.Exists())
            {
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Scissors, "misshair_shop@hair_dressers", "scissors_hair_cut_a", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            }

            uint GameTimeStarted = Game.GameTime;
            GameFiber.Sleep(1000);
            while (Game.GameTime - GameTimeStarted <= 10000)
            {
                if (!Hairstylist.Pedestrian.Exists())
                {
                    EntryPoint.WriteToConsole("HAIRCUT ANIM NO STYLIST");
                    break;
                }
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_hair_cut_a");
                if (AnimationTime >= 1.0f)
                {
                    EntryPoint.WriteToConsole("HAIRCUT ANIM OVER TIME");
                    break;
                }
                if (AnimationTime >= 0.7f && !hasShownStuff)
                {
                    Player.BankAccounts.GiveMoney(-1 * cost, true);
                    BarberShop.PlaySuccessSound();
                    hasShownStuff = true;
                }
                GameFiber.Yield();
            }
            if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
            {
                GameFiber.Sleep(2000);
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 8.0f, -8.0f, -1, 5641, 0.0f, 2, 1);
                if (Scissors != null && Scissors.Exists())
                {
                    NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Scissors, "misshair_shop@hair_dressers", "scissors_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 8.0f, -8.0f, -1, 5641, 0.0f, 2, 1);
                }
            }
        }
    }
    private void CreateScissors()
    {
        if(Scissors.Exists())
        {
            Scissors.Delete();
        }
        //Scissors = new Rage.Object("p_cs_scissors_s", AnimEnterPosition);
    }

    private void ShowHaircutMenu()
    {
        MenuPool MenuPool = new MenuPool();
        UIMenu InteractionMenu = new UIMenu(BarberShop.Name, BarberShop.Description);
        if (BarberShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BarberShop.BannerImagePath}");
            InteractionMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            //RemoveBanner = false;
            EntryPoint.WriteToConsole($"SET BANNER IMAGE FOR HAIRCUT MENU!");
        }
        MenuPool.Add(InteractionMenu);
        ChangeHaircutProcess changeHaircutProcess = new ChangeHaircutProcess(LocationInteractable,BarberShop, this, Hairstylist,Scissors, Settings, AnimEnterPosition, AnimEnterRotation, ClothesNames);
        changeHaircutProcess.Start(MenuPool, InteractionMenu);
        while (MenuPool.IsAnyMenuOpen() && Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        changeHaircutProcess.Dispose();
    }
}
