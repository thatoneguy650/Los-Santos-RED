using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

public class PedCustomizer
{
    private Camera CharCam;
    private bool IsDisposed = false;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwap PedSwap;
    private IPedSwappable Player;
    private float PreviousHeading;
    private Vector3 PreviousPos;
    private uint GameTimeLastPrinted;
    private IEntityProvideable World;
    private PedExt ModelPedExt;
    private ISettingsProvideable Settings;
    private IDispatchablePeople DispatchablePeople;
    private IHeads Heads;
    private IGangs Gangs;
    private IAgencies Agencies;
    private bool ShowDisplayName = true;

    //private CameraCycler CameraCycler;


    //public readonly Vector3 DefaultCameraPosition = new Vector3(402.8145f, -998.5043f, -98.29621f);// new Vector3(402.8985f, -998.7176f, -98.36113f);// new Vector3(402.8291f, -999.0933f, -98.39355f);//new Vector3(402.8999f, -998.8654f, -98.36703f);
    //public readonly Vector3 DefaultCameraDirection = new Vector3(-0.02121102f, 0.9286007f, -0.3704739f);// new Vector3(-0.006049804f, 0.9478015f, -0.3188036f);// new Vector3(0.01690567f, 0.959121f, -0.282491f);// new Vector3(0.003477225f, 0.9522974f, -0.3051518f);
    //public readonly Rotator DefaultCameraRotation = new Rotator(-21.74485f, -5.170386E-07f, 1.308518f);// new Rotator(-18.59059f, -2.955668E-07f, 0.3657132f);// new Rotator(-16.40893f, 5.006387E-07f, -1.009803f);// new Rotator(-17.7673f, 3.740232E-06f, -0.2092093f);
    //public readonly Vector3 DefaultCameraLookAtPosition = new Vector3(402.8473f, -996.3224f, -99.00025f);

    //public readonly Vector3 DefaultCameraPosition = new Vector3(402.7357f, -999.6426f, -98.01408f);
    //public readonly Vector3 DefaultCameraDirection = new Vector3(0.04971042f, 0.9577833f, -0.2831609f);
    //public readonly Rotator DefaultCameraRotation = new Rotator(-16.44895f, 7.789317E-06f, -2.971073f);
    //public readonly Vector3 DefaultCameraLookAtPosition = new Vector3(402.8473f, -996.3224f, -99.00025f);


    public readonly Vector3 DefaultModelPedPosition = new Vector3(402.8473f, -996.7224f, -99.00025f);
    public readonly float DefaultModelPedHeading = 182.7549f;
    public readonly Vector3 DefaultPlayerHoldingPosition = new Vector3(402.5164f, -1002.847f, -99.2587f);
    public CameraCycler CameraCycler { get; private set; }
    public string WorkingModelName { get; set; } = "S_M_M_GENTRANSPORT";


    public string WorkingVoice { get; set; } = "";

    public Ped ModelPed { get; set; }
    public string WorkingName { get; set; } = "John Doe";
    public int WorkingMoney { get; set; } = 5000;
    public PedVariation InitialVariation { get; set; } = new PedVariation();
    public PedVariation WorkingVariation { get; set; } = new PedVariation();
    public IClothesNames ClothesNames { get; private set; }
    public PedCustomizer(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, IDispatchablePeople dispatchablePeople, IHeads heads, IClothesNames clothesNames, IGangs gangs, IAgencies agencies)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        DispatchablePeople = dispatchablePeople;
        Heads = heads;
        ClothesNames = clothesNames;
        CharCam = new Camera(false);
        Agencies= agencies;
        Gangs= gangs;
    }
    public bool ChoseNewModel { get; private set; } = false;
    public bool ChoseToClose { get; private set; } = false;
    public PedCustomizerMenu PedCustomizerMenu { get; private set; }
    public bool PedModelIsFreeMode => ModelPed != null && ModelPed.Exists() && ModelPed.Model != null && (ModelPed.Model.Name.ToLower() == "mp_f_freemode_01" || ModelPed.Model.Name.ToLower() == "mp_m_freemode_01");
    public string PedModelGender
    {
        get
        {
            if(ModelPed == null || !ModelPed.Exists() || !PedModelIsFreeMode)
            {
                return "U";
            }
            else if(ModelPed.Model.Name.ToLower() == "mp_f_freemode_01")
            {
                return "F";
            }
            else if(ModelPed.Model.Name.ToLower() == "mp_m_freemode_01")
            {
                return "M";
            }
            else
            {
                return "U";
            }
        }
    }
    public bool PedModelIsFreeModeFemale => ModelPed != null && ModelPed.Exists() && ModelPed.Model != null && ModelPed.Model.Name.ToLower() == "mp_f_freemode_01";

    public Gang AssignedGang { get; set; }
    public Agency AssignedAgency { get; set; }

    public void Dispose(bool fadeOut)
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            if (fadeOut && !Game.IsScreenFadedOut && !Game.IsScreenFadingOut)
            {
                Game.FadeScreenOut(1500, true);
            }
            if (ModelPed.Exists() && ModelPed.Handle != Game.LocalPlayer.Character.Handle)
            {
                ModelPed.Delete();
            }
            Player.ButtonPrompts.RemovePrompts("ChangeCamera");
            MenuPool.CloseAllMenus();
            CharCam.Active = false;
            Game.LocalPlayer.Character.Position = PreviousPos;
            Game.LocalPlayer.Character.Heading = PreviousHeading;
            GameFiber.Sleep(1000);
            Game.FadeScreenIn(1500, true);
        }

    }
    public void Setup()
    {
        PedCustomizerMenu = new PedCustomizerMenu(MenuPool, PedSwap, Names, Player, World, Settings, this, DispatchablePeople, Heads, Gangs,Agencies);
        PedCustomizerMenu.Setup();

        CameraCycler = new CameraCycler(CharCam, Player, this);
        CameraCycler.Setup();
    }
    public void Update()
    {
        if (MenuPool.IsAnyMenuOpen())
        {
            ProcessButtonPrompts();
        }
        StopModelPedTasking();
        if(!MenuPool.IsAnyMenuOpen() && !ChoseToClose)
        {
            SimpleWarning popUpWarning = new SimpleWarning("Exit", "Are you sure you want to exit discarding changes", "", Player.ButtonPrompts, Settings);
            popUpWarning.Show();
            ShowDisplayName = false;
            if (popUpWarning.IsAccepted)
            {
                ChoseToClose = true;
                Game.FadeScreenOut(0, false);
                Dispose(true);
            }
            else
            {
                PedCustomizerMenu.CustomizeMainMenu.Visible = true;
                ShowDisplayName = true;
            }

        }
        
        if (ShowDisplayName)
        {
            DisplayName();
        }
    }
    //public void Tick()
    //{
    //    MenuPool.ProcessMenus();
    //}

    public void Start()
    {
        Game.FadeScreenOut(1500, true);
        MovePlayerToBookingRoom();
        WorkingModelName = Player.ModelName;
        WorkingName = Player.PlayerName;
        WorkingMoney = Player.BankAccounts.Money;
        CreateModelPed();
        SetModelAsCharacter();
        CameraCycler.SetDefault();
        Game.FadeScreenIn(1500, true);
        PedCustomizerMenu.Start();



        GameFiber.StartNew(delegate
        {
            try
            {
                while(!IsDisposed)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("PEDSWAP: BecomeCustomPed2; " + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "MENUPROCESSOR");

    }
    public void Finish()
    {

    }
    private void StopModelPedTasking()
    {
        if (ModelPed.Exists() && ModelPedExt == null)
        {
            ModelPedExt = World.Pedestrians.GetPedExt(ModelPed.Handle);
            if (ModelPedExt != null)
            {
                ModelPedExt.CanBeAmbientTasked = false;
                ModelPedExt.CanBeTasked = false;
            }
        }
    }
    private void ProcessButtonPrompts()
    {
        if (!Player.ButtonPrompts.HasPrompt($"RotateModelLeft"))
        {
            Player.ButtonPrompts.RemovePrompts("ChangeCamera");
            Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Turn Left", $"RotateModelLeft", System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.J, 1);
            Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Turn Right", $"RotateModelRight", System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.K, 2);
            Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Camera Cycle", $"CameraCycle", System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.O, 4);
            Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Reset", $"ResetCamera", System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.L, 6);
        }
    
        if (Player.ButtonPrompts.IsPressed("ResetCamera") || Player.ButtonPrompts.IsHeld("ResetCamera"))
        {
            EntryPoint.WriteToConsole("ResetCamera");
            ModelPed.Tasks.AchieveHeading(DefaultModelPedHeading, 5000);
            CameraCycler.SetDefault();
        }
        if (Player.ButtonPrompts.IsPressed("RotateModelLeft") || Player.ButtonPrompts.IsHeld("RotateModelLeft"))
        {
            EntryPoint.WriteToConsole("RotateModelLeft");
            if (ModelPed.Exists())
            {
                ModelPed.Tasks.AchieveHeading(ModelPed.Heading + 45f, 5000);
            }
        }
        if (Player.ButtonPrompts.IsPressed("RotateModelRight") || Player.ButtonPrompts.IsHeld("RotateModelRight"))
        {
            EntryPoint.WriteToConsole("RotateModelRight");
            if (ModelPed.Exists())
            {
                ModelPed.Tasks.AchieveHeading(ModelPed.Heading - 45, 5000);
            }
        }
        if (Player.ButtonPrompts.IsPressed("CameraCycle") || Player.ButtonPrompts.IsHeld("CameraCycle"))
        {
            EntryPoint.WriteToConsole("CameraCycle");
            CameraCycler.Cycle();
        }
    }
    private void MovePlayerToBookingRoom()
    {
        PreviousPos = Player.Character.Position;
        PreviousHeading = Player.Character.Heading;
        Player.Character.Position = DefaultPlayerHoldingPosition;
    }
    private void CreateModelPed()
    {
        try
        {
            if (ModelPed.Exists())
            {
                ModelPed.Delete();
            }
            ModelPed = new Ped(WorkingModelName, DefaultModelPedPosition, DefaultModelPedHeading);//new Vector3(402.8473f, -996.3224f, -99.00025f);
            if (ModelPed.Exists())
            {
                ModelPed.IsPersistent = true;
                ModelPed.IsVisible = true;
                ModelPed.BlockPermanentEvents = true;
                ModelPedExt = null;
            }
        }
        catch (Exception ex)
        {
            Game.DisplayNotification($"Error Spawning Ped {WorkingModelName}");
            EntryPoint.WriteToConsole($"Customize Ped Error {ex.Message}  {ex.StackTrace}", 0);
        }
    }
    private void ResetPedModel(bool resetVariation)
    {
        if (resetVariation)
        {
            WorkingVariation = new PedVariation();
            InitialVariation = new PedVariation();
        }
        if (ModelPed.Exists())
        {
            WorkingName = Names.GetRandomName(ModelPed.IsMale);
        }
        else
        {
            WorkingName = Names.GetRandomName(false);
        }
        WorkingMoney = 5000;
        ChoseNewModel = true;
    }
    private void SetModelAsCharacter()
    {
        if (Player.CurrentModelVariation != null)
        {
            Player.CurrentModelVariation.Copy().ApplyToPed(ModelPed);//this makes senese, but it isnt going to include headblend shit most of the time
            WorkingVariation = Player.CurrentModelVariation.Copy();
            InitialVariation = Player.CurrentModelVariation.Copy();
        }
        if(Player.RelationshipManager.GangRelationships.CurrentGang != null)
        {
            AssignedGang = Player.RelationshipManager.GangRelationships.CurrentGang;
        }
        else if(Player.IsCop)
        {
            AssignedAgency = Player.AssignedAgency;
        }

        if(Player.CharacterModelIsFreeMode)
        {
            WorkingVoice = Player.FreeModeVoice;
        }

    }
    public void OnModelChanged(bool resetVariation)
    {
        CreateModelPed();
        ResetPedModel(resetVariation);
        PedCustomizerMenu.OnModelChanged();
        OnVariationChanged();
    }
    public void OnVariationChanged()
    {
        if (ModelPed.Exists())
        {
            WorkingVariation?.ApplyToPed(ModelPed, false);
        }
    }
    public void BecomePed()
    {
        SimpleWarning popUpWarning = new SimpleWarning("Become Ped", "Are you sure you want to become the current model ped", "", Player.ButtonPrompts, Settings);
        popUpWarning.Show();
        if (popUpWarning.IsAccepted)
        {
            ChoseToClose = true;
            if (ModelPed.Exists())
            {
                Game.FadeScreenOut(0, false);
                //Game.FadeScreenOut(1500, true);
                if (!ChoseNewModel)
                {
                    PedSwap.BecomeSamePed(WorkingModelName, WorkingName, WorkingMoney, WorkingVariation);
                }
                else
                {
                    PedSwap.BecomeExistingPed(ModelPed, WorkingModelName, WorkingName, WorkingMoney, WorkingVariation, RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max), WorkingVoice);
                }


                if(AssignedAgency != null)
                {
                    Player.SetCopStatus(true, AssignedAgency);
                }
                else
                {
                    Player.SetCopStatus(false, null);
                }


                if (AssignedGang != null)
                {
                    Player.RelationshipManager.GangRelationships.SetGang(AssignedGang, true);
                }
                else
                {
                    Player.RelationshipManager.GangRelationships.ResetGang(false);
                }
                Dispose(false);
            }
            else
            {
                Dispose(true);
            }
        }
    }
    public void Exit()
    {
        SimpleWarning popUpWarning = new SimpleWarning("Exit", "Are you sure you want to exit discarding changes", "", Player.ButtonPrompts, Settings);
        popUpWarning.Show();
        if (popUpWarning.IsAccepted)
        {
            ChoseToClose = true;
            Dispose(true);
        }
    }
    public void PrintVariation()
    {
        if (WorkingVariation!= null) 
        {
            Serialization.SerializeParam(WorkingVariation, $"Plugins\\LosSantosRED\\SavedVariation{WorkingModelName}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xml");
        }
    }
    private void DisplayName()
    {
        //AffiliationCenterX = 0.92f;
        //AffiliationCenterY = 0.575f;





        DisplayTextOnScreen(WorkingName,
            Settings.SettingsManager.PedSwapSettings.NamePositionY,
            Settings.SettingsManager.PedSwapSettings.NamePositionX,
            Settings.SettingsManager.PedSwapSettings.NameScale,
            Color.FromName(Settings.SettingsManager.PedSwapSettings.NameColor),
            (GTAFont)Settings.SettingsManager.PedSwapSettings.NameFont, 
            (GTATextJustification)Settings.SettingsManager.PedSwapSettings.NameJustificationID,
            false);

        string AffiliationName = "Unaffiliated";
        Color affiliationColor = Color.FromName(Settings.SettingsManager.PedSwapSettings.AffiliationColor);
        if (AssignedAgency != null)
        {
            AffiliationName = AssignedAgency.ID;
            affiliationColor = AssignedAgency.Color;
        }
        else if (AssignedGang != null)
        {
            AffiliationName = AssignedGang.ShortName;
            affiliationColor = AssignedGang.Color;
        }
        DisplayTextOnScreen(AffiliationName,
            Settings.SettingsManager.PedSwapSettings.AffiliationPositionY,
            Settings.SettingsManager.PedSwapSettings.AffiliationPositionX,
            Settings.SettingsManager.PedSwapSettings.AffiliationScale,
            affiliationColor,
            (GTAFont)Settings.SettingsManager.PedSwapSettings.AffiliationFont,
            (GTATextJustification)Settings.SettingsManager.PedSwapSettings.AffiliationJustificationID,
            false);
    }
    private void DisplayTextOnScreen(string TextToShow, float Y, float X, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline)
    {
        DisplayTextOnScreen(TextToShow, Y, X, Scale, TextColor, Font, Justification, outline, 255);
    }
    private void DisplayTextOnScreen(string TextToShow, float Y, float X, float Scale, Color TextColor, GTAFont Font, GTATextJustification Justification, bool outline, int alpha)
    {
        try
        {
            if (TextToShow == "" || alpha == 0 || TextToShow is null)
            {
                return;
            }
            NativeFunction.Natives.SET_TEXT_FONT((int)Font);
            NativeFunction.Natives.SET_TEXT_SCALE(Scale, Scale);
            NativeFunction.Natives.SET_TEXT_COLOUR((int)TextColor.R, (int)TextColor.G, (int)TextColor.B, alpha);

            NativeFunction.Natives.SetTextJustification((int)Justification);

            NativeFunction.Natives.SET_TEXT_DROP_SHADOW();

            if (outline)
            {
                NativeFunction.Natives.SET_TEXT_OUTLINE(true);


                NativeFunction.Natives.SET_TEXT_EDGE(1, 0, 0, 0, 255);
            }
            NativeFunction.Natives.SET_TEXT_DROP_SHADOW();
            //NativeFunction.Natives.SetTextDropshadow(20, 255, 255, 255, 255);//NativeFunction.Natives.SetTextDropshadow(2, 2, 0, 0, 0);
            //NativeFunction.Natives.SetTextJustification((int)GTATextJustification.Center);
            if (Justification == GTATextJustification.Right)
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, X);
            }
            else
            {
                NativeFunction.Natives.SET_TEXT_WRAP(0f, 1f);
            }
            NativeFunction.Natives.x25fbb336df1804cb("STRING"); //NativeFunction.Natives.x25fbb336df1804cb("STRING");
            //NativeFunction.Natives.x25FBB336DF1804CB(TextToShow);
            NativeFunction.Natives.x6C188BE134E074AA(TextToShow);
            NativeFunction.Natives.xCD015E5BB0D96A57(X, Y);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI ERROR {ex.Message} {ex.StackTrace}", 0);
        }
        //return;
    }
}