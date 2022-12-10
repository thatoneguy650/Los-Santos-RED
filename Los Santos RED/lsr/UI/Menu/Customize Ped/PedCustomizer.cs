using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
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
    private CameraCycler CameraCycler;

    public readonly Vector3 DefaultCameraPosition = new Vector3(402.7357f, -999.6426f, -98.01408f);// new Vector3(402.8473f, -998.3224f, -98.00025f);
    public readonly Vector3 DefaultCameraDirection = new Vector3(0.04971042f, 0.9577833f, -0.2831609f);
    public readonly Rotator DefaultCameraRotation = new Rotator(-16.44895f, 7.789317E-06f, -2.971073f);


    public readonly Vector3 DefaultCameraLookAtPosition = new Vector3(402.8473f, -996.3224f, -99.00025f);
    public readonly Vector3 DefaultModelPedPosition = new Vector3(402.8473f, -996.7224f, -99.00025f);
    public readonly float DefaultModelPedHeading = 182.7549f;
    public readonly Vector3 DefaultPlayerHoldingPosition = new Vector3(402.5164f, -1002.847f, -99.2587f);

    public string WorkingModelName { get; set; } = "S_M_M_GENTRANSPORT";
    public Ped ModelPed { get; set; }
    public string WorkingName { get; set; } = "John Doe";
    public int WorkingMoney { get; set; } = 5000;
    public PedVariation InitialVariation { get; set; } = new PedVariation();
    public PedVariation WorkingVariation { get; set; } = new PedVariation();
    public PedCustomizer(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, IDispatchablePeople dispatchablePeople, IHeads heads)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        DispatchablePeople = dispatchablePeople;
        Heads = heads;
        CharCam = new Camera(false);
    }
    public bool ChoseNewModel { get; private set; } = false;
    public bool ChoseToClose { get; private set; } = false;
    public PedCustomizerMenu PedCustomizerMenu { get; private set; }
    public bool PedModelIsFreeMode => ModelPed != null && ModelPed.Exists() && ModelPed.Model != null && ModelPed.Model.Name.ToLower() == "mp_f_freemode_01" || ModelPed.Model.Name.ToLower() == "mp_m_freemode_01";
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
        PedCustomizerMenu = new PedCustomizerMenu(MenuPool, PedSwap, Names, Player, World, Settings, this, DispatchablePeople, Heads);
        PedCustomizerMenu.Setup();

        CameraCycler = new CameraCycler(CharCam, Player, this);
        CameraCycler.Setup();
    }
    public void Update()
    {
        ProcessButtonPrompts();
        StopModelPedTasking();
        MenuPool.ProcessMenus();
    }
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
        ChoseToClose = true;
        if (ModelPed.Exists())
        {
            Game.FadeScreenOut(1500, true);
            if (!ChoseNewModel)
            {
                PedSwap.BecomeSamePed(WorkingModelName, WorkingName, WorkingMoney, WorkingVariation);
            }
            else
            {
                PedSwap.BecomeExistingPed(ModelPed, WorkingModelName, WorkingName, WorkingMoney, WorkingVariation, RandomItems.GetRandomNumberInt(Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Min, Settings.SettingsManager.PlayerOtherSettings.PlayerSpeechSkill_Max));
            }
            Dispose(false);
        }
        else
        {
            Dispose(true);
        }
    }
    public void Exit()
    {
        ChoseToClose = true;
        Dispose(true);
    }
    public void PrintVariation()
    {
        if (WorkingVariation!= null) 
        {
            Serialization.SerializeParam(WorkingVariation, $"Plugins\\LosSantosRED\\SavedVariation{WorkingModelName}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xml");
        }
    }
}