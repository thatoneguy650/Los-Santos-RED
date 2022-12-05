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
    private PedCustomizerMenu PedCustomizerMenu;
    private IHeads Heads;

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
    }
    public bool ChoseNewModel { get; private set; } = false;
    public bool PedModelIsFreeMode => ModelPed != null && ModelPed.Exists() && ModelPed.Model != null && ModelPed.Model.Name.ToLower() == "mp_f_freemode_01" || ModelPed.Model.Name.ToLower() == "mp_m_freemode_01";
    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            if (!Game.IsScreenFadedOut && !Game.IsScreenFadingOut)
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
            Game.FadeScreenIn(1500, true);
        }
    }
    public void Setup()
    {
        PedCustomizerMenu = new PedCustomizerMenu(MenuPool, PedSwap, Names, Player, World, Settings, this, DispatchablePeople, Heads);
        PedCustomizerMenu.Setup();
    }
    public void Update()
    {
        if (1 == 1)
        {
            if (!Player.ButtonPrompts.HasPrompt($"ZoomCameraIn"))
            {
                Player.ButtonPrompts.RemovePrompts("ChangeCamera");
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Turn Left", $"RotateModelLeft", System.Windows.Forms.Keys.J, 1);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Turn Right", $"RotateModelRight", System.Windows.Forms.Keys.K, 2);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Camera Up", $"CameraUp", System.Windows.Forms.Keys.O, 4);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Camera Down", $"CameraDown", System.Windows.Forms.Keys.L, 5);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Zoom In", $"ZoomCameraIn", System.Windows.Forms.Keys.U, 3);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Zoom Out", $"ZoomCameraOut", System.Windows.Forms.Keys.I, 4);
                Player.ButtonPrompts.AddPrompt("ChangeCamera", $"Reset", $"ResetCamera", System.Windows.Forms.Keys.P, 6);
            }
        }
        if (Player.ButtonPrompts.IsPressed("ResetCamera"))
        {
            ModelPed.Tasks.AchieveHeading(182.7549f, 5000);
            CharCam.Position = new Vector3(402.8473f, -998.3224f, -98.00025f);
            CharCam.Direction = NativeHelper.GetCameraDirection(CharCam);
        }
        else if (Player.ButtonPrompts.IsPressed("RotateModelLeft"))
        {
            if (ModelPed.Exists())
            {
                ModelPed.Tasks.AchieveHeading(ModelPed.Heading + 45f, 5000);
            }
        }
        else if (Player.ButtonPrompts.IsPressed("RotateModelRight"))
        {
            if (ModelPed.Exists())
            {
                ModelPed.Tasks.AchieveHeading(ModelPed.Heading - 45, 5000);
            }
        }
        else if (Player.ButtonPrompts.IsPressed("CameraUp") || Player.ButtonPrompts.IsHeld("CameraUp"))
        {
            CharCam.Position = new Vector3(CharCam.Position.X, CharCam.Position.Y, CharCam.Position.Z + 0.05f);
        }
        else if (Player.ButtonPrompts.IsPressed("CameraDown") || Player.ButtonPrompts.IsHeld("CameraDown"))//else if (Player.ButtonPromptList.Any(x => x.Identifier == "CameraDown" && (x.IsPressedNow || x.IsHeldNow)))
        {
            CharCam.Position = new Vector3(CharCam.Position.X, CharCam.Position.Y, CharCam.Position.Z - 0.05f);
        }
        else if (Player.ButtonPrompts.IsPressed("ZoomCameraIn") || Player.ButtonPrompts.IsHeld("ZoomCameraIn"))//else if (Player.ButtonPromptList.Any(x => x.Identifier == "ZoomCameraIn" && (x.IsPressedNow || x.IsHeldNow)))
        {
            EntryPoint.WriteToConsole("ZoomCameraIn", 5);
            CharCam.Position = new Vector3(CharCam.Position.X, CharCam.Position.Y + 0.05f, CharCam.Position.Z);
        }
        else if (Player.ButtonPrompts.IsPressed("ZoomCameraOut") || Player.ButtonPrompts.IsHeld("ZoomCameraOut"))//else if (Player.ButtonPromptList.Any(x => x.Identifier == "ZoomCameraOut" && (x.IsPressedNow || x.IsHeldNow)))
        {
            EntryPoint.WriteToConsole("ZoomCameraOut", 5);
            CharCam.Position = new Vector3(CharCam.Position.X, CharCam.Position.Y - 0.05f, CharCam.Position.Z);
        }


        if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.N) && Game.GameTime - GameTimeLastPrinted >= 1000)
        {
            EntryPoint.WriteToConsole(WorkingVariation.ToString());
            GameTimeLastPrinted = Game.GameTime;
        }
        if (ModelPed.Exists() && ModelPedExt == null)
        {
            ModelPedExt = World.Pedestrians.GetPedExt(ModelPed.Handle);
            if (ModelPedExt != null)
            {
                ModelPedExt.CanBeAmbientTasked = false;
                ModelPedExt.CanBeTasked = false;
            }
        }
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
        ActivateCustomizeCamera();
        Game.FadeScreenIn(1500, true);
        PedCustomizerMenu.Start();
    }
    public void Finish()
    {

    }
    private void ActivateCustomizeCamera()
    {
        CharCam = new Camera(false);
        CharCam.Position = new Vector3(402.8473f, -998.3224f, -98.00025f);
        Vector3 r = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        CharCam.Rotation = new Rotator(r.X, r.Y, r.Z);
        Vector3 ToLookAt = new Vector3(402.8473f, -996.3224f, -99.00025f);
        Vector3 _direction = (ToLookAt - CharCam.Position).ToNormalized();
        CharCam.Direction = _direction;
        CharCam.Active = true;
    }
    private void MovePlayerToBookingRoom()
    {
        PreviousPos = Player.Character.Position;
        PreviousHeading = Player.Character.Heading;
        Player.Character.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
    }
    private void CreateModelPed()
    {
        try
        {
            if (ModelPed.Exists())
            {
                ModelPed.Delete();
            }
            ModelPed = new Ped(WorkingModelName, new Vector3(402.8473f, -996.7224f, -99.00025f), 182.7549f);
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
    private void ResetPedModel()
    {
        if (ModelPed.Exists())
        {
            ModelPed.Delete();
        }
        WorkingVariation = new PedVariation();
        InitialVariation = new PedVariation();
        if (ModelPed.Exists())
        {
            WorkingName = Names.GetRandomName(ModelPed.IsMale);
        }
        else
        {
            WorkingName = Names.GetRandomName(false);
        }
        //if(ModelPed.Exists() && PedModelIsFreeMode)
        //{
        //    WorkingVariation.HeadBlendData.shapeFirst = 0;
        //    WorkingVariation.HeadBlendData.shapeMix = 1.0f;

        //    WorkingVariation.ApplyToPed(ModelPed);
        //}
        ChoseNewModel = true;
    }
    private void SetModelAsCharacter()
    {
        if (Player.CurrentModelVariation != null)
        {
            Player.CurrentModelVariation.ApplyToPed(ModelPed);//this makes senese, but it isnt going to include headblend shit most of the time
            WorkingVariation = Player.CurrentModelVariation;
            InitialVariation = Player.CurrentModelVariation.Copy();
        }
    }
    public void OnModelChanged()
    {
        ResetPedModel();
        CreateModelPed();
        PedCustomizerMenu.OnModelChanged();
        OnVariationChanged();
    }
    public void OnVariationChanged()
    {
        if (ModelPed.Exists())
        {
            WorkingVariation.ApplyToPed(ModelPed);
        }
        PedCustomizerMenu.OnVariationChanged();
    }
    public void BecomePed()
    {
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
            Dispose();
        }
    }
    public void Exit()
    {
        Dispose();
    }
}