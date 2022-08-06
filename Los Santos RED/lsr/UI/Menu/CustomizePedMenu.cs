using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class CustomizePedMenu : Menu
{
    private UIMenuItem BecomeModel;
    private UIMenuItem ChangeModel;
    private UIMenuItem ChangeMoney;
    private UIMenuItem ChangeName;
    private Camera CharCam;
    private UIMenuItem ClearProps;
    private List<ColorLookup> ColorList;
    private List<FashionComponent> ComponentLookup;
    private UIMenu ComponentsUIMenu;
    private int CurrentComponent = -1;
    private int CurrentDrawable = -1;
    private string CurrentSelectedComponent = "-1";
    private int CurrentTexture = -1;
    private UIMenu CustomizeHeadMenu;
    private UIMenu CustomizeMainMenu;
    private UIMenuItem DefaultVariation;
    private UIMenu DemographicsSubMenu;
    private UIMenuItem Exit;
    private UIMenuListScrollerItem<ColorLookup> HairPrimaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> HairSecondaryColorMenu;
    private List<HeadLookup> HeadList;
    private List<HeadOverlayData> HeadOverlayLookups;
    private bool IsDisposed = false;
    MenuPool MenuPool;
    private Ped ModelPed;
    private INameProvideable Names;
    private string NewModelName = "S_M_M_GENTRANSPORT";
    private UIMenuListScrollerItem<HeadLookup> Parent1IDMenu;
    private UIMenuNumericScrollerItem<float> Parent1MixMenu;
    private UIMenuListScrollerItem<HeadLookup> Parent2IDMenu;
    private UIMenuNumericScrollerItem<float> Parent2MixMenu;
    private IPedSwap PedSwap;
    private IPedSwappable Player;
    private float PreviousHeading;
    private Vector3 PreviousPos;
    private List<FashionProp> PropLookup;
    private UIMenu PropsUIMenu;
    private UIMenuItem RandomizeHair;
    private UIMenuItem RandomizeHead;
    private UIMenuItem RandomizeName;
    private UIMenuItem RandomizeVariation;
    private UIMenuListScrollerItem<string> SelectModel;
    private int TextureSelected;
    private int WorkingMoney = 5000;
    private string WorkingName = "John Doe";
    private PedVariation WorkingVariation = new PedVariation();
    private uint GameTimeLastPrinted;
    private IEntityProvideable World;
    private PedExt ModelPedExt;

    public CustomizePedMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        CustomizeMainMenu = new UIMenu("Customize Ped", "Select an Option");
        CustomizeMainMenu.SetBannerType(EntryPoint.LSRedColor);
        menuPool.Add(CustomizeMainMenu);
        CustomizeMainMenu.OnItemSelect += MainMenu_OnItemSelect;
    }
    public bool ChoseNewModel { get; private set; } = false;
    private bool PedModelIsFreeMode => ModelPed.Exists() && ModelPed.Model.Name.ToLower() == "mp_f_freemode_01" || ModelPed.Model.Name.ToLower() == "mp_m_freemode_01";
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
    public override void Hide()
    {
        CustomizeMainMenu.Visible = false;
    }
    public void Setup()
    {
        Game.FadeScreenOut(1500, true);
        MovePlayerToBookingRoom();
        NewModelName = Player.ModelName;
        WorkingName = Player.PlayerName;
        WorkingMoney = Player.BankAccounts.Money;
        CreateModelPed();
        SetModelAsCharacter();
        SetupMenu();
        ActivateDefaultCamera();
        Game.FadeScreenIn(1500, true);
    }
    public override void Show()
    {
        CustomizeMainMenu.Visible = true;
    }
    public override void Toggle()
    {
        if (!CustomizeMainMenu.Visible)
        {
            CustomizeMainMenu.Visible = true;
        }
        else
        {
            CustomizeMainMenu.Visible = false;
        }
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


        if(Game.IsKeyDownRightNow(System.Windows.Forms.Keys.N) && Game.GameTime - GameTimeLastPrinted >= 1000)
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
    private void ActivateDefaultCamera()
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
    private void ComponentsMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        CurrentSelectedComponent = selectedItem.Text;
        CurrentComponent = index;
    }
    private void CreateModelPed()
    {
        try
        {
            if (ModelPed.Exists())
            {
                ModelPed.Delete();
            }
            ModelPed = new Ped(NewModelName, new Vector3(402.8473f, -996.7224f, -99.00025f), 182.7549f);
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
            Game.DisplayNotification($"Error Spawning Ped {NewModelName}");
            EntryPoint.WriteToConsole($"Customize Ped Error {ex.Message}  {ex.StackTrace}", 0);
        }
    }
    private void DemographicsMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ChangeName)
        {
            WorkingName = NativeHelper.GetKeyboardInput(WorkingName);
            ChangeName.Description = "Current: " + WorkingName;
            RandomizeName.Description = "Current: " + WorkingName;
        }
        if (selectedItem == RandomizeName)
        {
            string Name = "John Doe";
            if (ModelPed.Exists())
            {
                Name = Names.GetRandomName(ModelPed.IsMale);
            }
            else
            {
                Name = Names.GetRandomName(false);
            }
            WorkingName = Name;
            ChangeName.Description = "Current: " + WorkingName;
            RandomizeName.Description = "Current: " + WorkingName;
        }
        if (selectedItem == ChangeMoney)
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(WorkingMoney.ToString()), out int BribeAmount))
            {
                WorkingMoney = BribeAmount;
                ChangeMoney.Description = "Current: " + WorkingMoney.ToString("C0");
            }
        }
    }
    private void HeadMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == RandomizeHead)
        {
            if (ModelPed.Exists() && PedModelIsFreeMode)
            {
                RandomizeOverlay();
                RandomizeHeadblend();
                WorkingVariation.ApplyToPed(ModelPed);
                GameFiber.Yield();
            }
        }
        else if (selectedItem == RandomizeHair)
        {
            if (ModelPed.Exists() && PedModelIsFreeMode)
            {
                WorkingVariation.ApplyToPed(ModelPed);
                RandomizeHairStyle();
                WorkingVariation.ApplyToPed(ModelPed);
                GameFiber.Yield();
            }
        }
    }
    private void HeadMenu_OnScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if (item == HairPrimaryColorMenu)
        {
            WorkingVariation.PrimaryHairColor = newIndex;
            if (PedModelIsFreeMode)
            {
                NativeFunction.Natives.x4CFFC65454C93A49(ModelPed, WorkingVariation.PrimaryHairColor, WorkingVariation.SecondaryHairColor);
                EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {WorkingVariation.PrimaryHairColor} {WorkingVariation.SecondaryHairColor}", 5);
            }
        }
        else if (item == HairSecondaryColorMenu)
        {
            WorkingVariation.SecondaryHairColor = newIndex;
            if (PedModelIsFreeMode)
            {
                NativeFunction.Natives.x4CFFC65454C93A49(ModelPed, WorkingVariation.PrimaryHairColor, WorkingVariation.SecondaryHairColor);
                EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {WorkingVariation.PrimaryHairColor} {WorkingVariation.SecondaryHairColor}", 5);
            }
        }
        else if (item == Parent1IDMenu)
        {
            WorkingVariation.HeadBlendData.skinFirst = newIndex;
            WorkingVariation.HeadBlendData.shapeFirst = newIndex;
            WorkingVariation.ApplyToPed(ModelPed);
        }
        else if (item == Parent2IDMenu)
        {
            WorkingVariation.HeadBlendData.skinSecond = newIndex;
            WorkingVariation.HeadBlendData.shapeSecond = newIndex;
            WorkingVariation.ApplyToPed(ModelPed);
        }
        else if (item == Parent1MixMenu)
        {
            if (float.TryParse(item.OptionText, out float newMix))
            {
                WorkingVariation.HeadBlendData.shapeMix = newMix;
                WorkingVariation.HeadBlendData.skinMix = 1.0f - newMix;

                Parent2MixMenu.Value = 1.0f - newMix;
                WorkingVariation.ApplyToPed(ModelPed);
            }
        }
        else if (item == Parent2MixMenu)
        {
            if (float.TryParse(item.OptionText, out float newMix))
            {
                WorkingVariation.HeadBlendData.skinMix = newMix;
                WorkingVariation.HeadBlendData.shapeMix = 1.0f - newMix;
                Parent1MixMenu.Value = 1.0f - newMix;
                WorkingVariation.ApplyToPed(ModelPed);
            }
        }
        EntryPoint.WriteToConsole($"OnScrollerChange {sender.TitleText} {item.Index} {item.OptionText} {item.Text} {newIndex}", 5);
    }
    private void HeadOverlay_OnScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        string overlayName = item.Parent.TitleText;
        HeadOverlayData myOverlay = WorkingVariation.HeadOverlays.FirstOrDefault(x => x.Part == overlayName);
        if (myOverlay == null)
        {
            HeadOverlayData newmyOverlay = HeadOverlayLookups.FirstOrDefault(x => x.Part == overlayName);
            if (newmyOverlay == null)
            {
                return;
            }
            myOverlay = new HeadOverlayData(newmyOverlay.OverlayID, newmyOverlay.Part) { ColorType = newmyOverlay.ColorType };
            WorkingVariation.HeadOverlays.Add(myOverlay);
        }
        if (item.Text == "Set Primary Color")
        {
            myOverlay.PrimaryColor = newIndex;
        }
        else if (item.Text == "Set Secondary Color")
        {
            myOverlay.SecondaryColor = newIndex;
        }
        else if (item.Text == "Index")
        {
            myOverlay.Index = newIndex - 1;
        }
        else if (item.Text == "Opacity" && item.GetType() == typeof(UIMenuNumericScrollerItem<float>))// && float.TryParse(item.OptionText, out float opacity))
        {
            UIMenuNumericScrollerItem<float> test = (UIMenuNumericScrollerItem<float>)item;
            myOverlay.Opacity = test.Value;
        }
        WorkingVariation.ApplyToPed(ModelPed);
        EntryPoint.WriteToConsole($"OnHeadOverlayScrollerChange {sender.TitleText} {item.Index} {item.OptionText} {item.Text} {newIndex}", 5);
    }
    private void MainMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == ChangeModel)
        {
            NewModelName = NativeHelper.GetKeyboardInput("player_zero");
            if (new Rage.Model(NewModelName).IsValid)
            {
                ResetPedModel();
                CreateModelPed();
                RefreshMenuList();
            }
        }
        if (selectedItem == SelectModel)
        {
            NewModelName = SelectModel.OptionText;
            if (new Rage.Model(NewModelName).IsValid)
            {
                ResetPedModel();
                CreateModelPed();
                RefreshMenuList();
            }
        }
        if (selectedItem == BecomeModel)
        {
            if (ModelPed.Exists())
            {
                //ChoseNewModel = true;
                Game.FadeScreenOut(1500, true);
                if (!ChoseNewModel)
                {
                    PedSwap.BecomeSamePed(NewModelName, WorkingName, WorkingMoney, WorkingVariation);
                }
                else
                {
                    PedSwap.BecomeExistingPed(ModelPed, NewModelName, WorkingName, WorkingMoney, WorkingVariation);
                }
                Dispose();
            }
        }
        if (selectedItem == ClearProps)
        {
            if (ModelPed.Exists())
            {
                WorkingVariation.Props.Clear();
                WorkingVariation.ApplyToPed(ModelPed);
            }
        }
        if (selectedItem == RandomizeVariation)
        {
            if (ModelPed.Exists())
            {
                NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ModelPed);
                NativeFunction.Natives.SET_PED_RANDOM_COMPONENT_VARIATION(ModelPed);
                NativeFunction.Natives.SET_PED_RANDOM_PROPS(ModelPed);
                PedVariation newVariation = NativeHelper.GetPedVariation(ModelPed);
                WorkingVariation.Components = newVariation.Components;
                WorkingVariation.Props = newVariation.Props;
            }
        }
        if (selectedItem == DefaultVariation)
        {
            if (ModelPed.Exists())
            {
                NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ModelPed);
                NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ModelPed);
                PedVariation newVariation = NativeHelper.GetPedVariation(ModelPed);
                WorkingVariation.Components = newVariation.Components;
                WorkingVariation.Props = newVariation.Props;
            }
        }
        if (selectedItem == Exit)
        {
            Dispose();
        }
    }
    private void MovePlayerToBookingRoom()
    {
        PreviousPos = Player.Character.Position;
        PreviousHeading = Player.Character.Heading;
        Player.Character.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
    }
    private void OnComponentIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = index;
        CurrentTexture = index;

        TextureSelected = 0;
        CurrentTexture = 0;

        bool IsValid = false;
        if (ModelPed.Exists())
        {
            IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture);
            if (IsValid || !IsValid)
            {
                //NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture, 0);
                PedComponent pedComponent = WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == CurrentComponent);
                if (pedComponent == null)
                {
                    pedComponent = new PedComponent(CurrentComponent, CurrentDrawable, CurrentTexture, 0);
                    WorkingVariation.Components.Add(pedComponent);
                }
                pedComponent.DrawableID = CurrentDrawable;
                pedComponent.TextureID = CurrentTexture;
                WorkingVariation.ApplyToPed(ModelPed);
            }
        }
        EntryPoint.WriteToConsole($"OnComponentItemSelect IsValid {IsValid} {CurrentSelectedComponent}  {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        bool IsValid = false;
        if (ModelPed.Exists())
        {
            IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture);
            if (IsValid || !IsValid)
            {
                //NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture, 0);
                PedComponent pedComponent = WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == CurrentComponent);
                if (pedComponent == null)
                {
                    pedComponent = new PedComponent(CurrentComponent, CurrentDrawable, CurrentTexture, 0);
                    WorkingVariation.Components.Add(pedComponent);
                }
                pedComponent.DrawableID = CurrentDrawable;
                pedComponent.TextureID = CurrentTexture;
                WorkingVariation.ApplyToPed(ModelPed);
            }
        }
        EntryPoint.WriteToConsole($"OnComponentScollerChange IsValid {IsValid} {CurrentSelectedComponent} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropItemSelect(UIMenu sender, UIMenuItem selectedItem, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        int propID = -1;
        if (ModelPed.Exists())
        {
            propID = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(ModelPed, CurrentComponent);
            NativeFunction.Natives.CLEAR_PED_PROP(ModelPed, propID);
            //NativeFunction.Natives.SET_PED_PROP_INDEX<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture, true);
            PedPropComponent pedPropComponent = WorkingVariation.Props.FirstOrDefault(x => x.PropID == CurrentComponent);
            if (pedPropComponent == null)
            {
                pedPropComponent = new PedPropComponent(CurrentComponent, CurrentDrawable, CurrentTexture);
                WorkingVariation.Props.Add(pedPropComponent);
            }
            pedPropComponent.DrawableID = CurrentDrawable;
            pedPropComponent.TextureID = CurrentTexture;
            WorkingVariation.ApplyToPed(ModelPed);
        }
        EntryPoint.WriteToConsole($"OnPropsScollerChange propID {propID} {CurrentSelectedComponent}  {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        CurrentDrawable = sender.CurrentSelection;//???????
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        int propID = -1;
        if (ModelPed.Exists())
        {
            propID = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(ModelPed, CurrentComponent);
            NativeFunction.Natives.CLEAR_PED_PROP(ModelPed, propID);
            //NativeFunction.Natives.SET_PED_PROP_INDEX<bool>(ModelPed, CurrentComponent, CurrentDrawable, CurrentTexture, true);
            PedPropComponent pedPropComponent = WorkingVariation.Props.FirstOrDefault(x => x.PropID == CurrentComponent);
            if (pedPropComponent == null)
            {
                pedPropComponent = new PedPropComponent(CurrentComponent, CurrentDrawable, CurrentTexture);
                WorkingVariation.Props.Add(pedPropComponent);
            }
            pedPropComponent.DrawableID = CurrentDrawable;
            pedPropComponent.TextureID = CurrentTexture;
            WorkingVariation.ApplyToPed(ModelPed);
        }
        EntryPoint.WriteToConsole($"OnPropsScollerChange propID {propID} {CurrentSelectedComponent}{TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void PropsMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        CurrentSelectedComponent = selectedItem.Text;
        CurrentComponent = index;
    }
    private void RandomizeHairStyle()
    {
        int DrawableID = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(ModelPed, 2));
        int TextureID = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(ModelPed, 2, DrawableID) - 1);
        //NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(ModelPed, 2, DrawableID, TextureID, 0);
        WorkingVariation.PrimaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
        WorkingVariation.SecondaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
        HairPrimaryColorMenu.Index = WorkingVariation.PrimaryHairColor;
        HairSecondaryColorMenu.Index = WorkingVariation.SecondaryHairColor;
        PedComponent hairComponent = WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == 2);
        if (hairComponent == null)
        {
            WorkingVariation.Components.Add(new PedComponent(2, DrawableID, TextureID, 0));
        }
        else
        {
            hairComponent.DrawableID = DrawableID;
            hairComponent.TextureID = TextureID;
        }
    }
    private void RandomizeHeadblend()
    {
        int MotherID = 0;
        int FatherID = 0;
        float FatherSide = 0f;
        float MotherSide = 0f;
        MotherID = RandomItems.GetRandomNumberInt(0, 45);
        FatherID = RandomItems.GetRandomNumberInt(0, 45);
        if (ModelPed.IsMale)
        {
            FatherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
            MotherSide = 1.0f - FatherSide;
        }
        else
        {
            MotherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
            FatherSide = 1.0f - MotherSide;
        }

        Parent1IDMenu.Index = MotherID;
        Parent2IDMenu.Index = FatherID;
        Parent1MixMenu.Value = MotherSide;
        Parent2MixMenu.Value = FatherSide;
        WorkingVariation.HeadBlendData = new HeadBlendData(MotherID, FatherID, 0, MotherID, FatherID, 0, MotherSide, FatherSide, 0.0f);
    }
    private void RandomizeOverlay()
    {
        foreach (HeadOverlayData ho in WorkingVariation.HeadOverlays)
        {
            int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
            ho.Index = RandomItems.GetRandomNumberInt(-1, TotalItems - 1);
            ho.Opacity = RandomItems.GetRandomNumber(0.0f, 1.0f);
            ho.PrimaryColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
            ho.SecondaryColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
        }
    }
    private void RefreshMenuList()
    {
        ComponentsUIMenu.Clear();
        PropsUIMenu.Clear();
        if (ModelPed.Exists())
        {
            if (PedModelIsFreeMode)
            {
                foreach (UIMenuItem menu in CustomizeHeadMenu.MenuItems)
                {
                    menu.Enabled = true;
                }
            }
            else
            {
                foreach (UIMenuItem menu in CustomizeHeadMenu.MenuItems)
                {
                    menu.Enabled = false;
                }
            }
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                //PedComponent existingComponent = null;
                //if (Player.CurrentModelVariation != null)
                //{
                //    existingComponent = Player.CurrentModelVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentNumber);
                //}
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(ModelPed, ComponentNumber);
                string description = $"Component: {ComponentNumber}";
                FashionComponent fashionComponent = ComponentLookup.FirstOrDefault(x => x.ComponentID == ComponentNumber);
                if (fashionComponent != null)
                {
                    description = fashionComponent.ComponentName;
                }
                UIMenu ComponentSubMenu = MenuPool.AddSubMenu(ComponentsUIMenu, description);

                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(ModelPed, ComponentNumber, DrawableNumber) - 1;
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "Arrow to change texture, select to reset texture", 0, NumberOfTextureVariations, 1);
                    //if(existingComponent != null && existingComponent.DrawableID == DrawableNumber)
                    //{
                    //    Test.Value = existingComponent.TextureID;
                    //}
                    Test.Formatter = v => v == 0 ? "Default" : "Texture ID " + v.ToString() + $" of {NumberOfTextureVariations}";
                    ComponentSubMenu.AddItem(Test);
                }
                //if (existingComponent != null)
                //{
                //    ComponentSubMenu.CurrentSelection = existingComponent.DrawableID;
                //}
                ComponentSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                ComponentSubMenu.OnItemSelect += OnComponentItemSelect;
                ComponentSubMenu.OnScrollerChange += OnComponentScollerChange;
                ComponentSubMenu.OnIndexChange += OnComponentIndexChange;
            }
            for (int PropsNumber = 0; PropsNumber < 3; PropsNumber++)
            {
                //PedPropComponent existingComponent = null;
                //if (Player.CurrentModelVariation != null)
                //{
                //    existingComponent = Player.CurrentModelVariation.Props.FirstOrDefault(x => x.PropID == PropsNumber);
                //}
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(Game.LocalPlayer.Character, PropsNumber);
                string description = $"Prop: {PropsNumber}";
                FashionProp fashionProp = PropLookup.FirstOrDefault(x => x.PropID == PropsNumber);
                if (fashionProp != null)
                {
                    description = fashionProp.PropName;
                }
                UIMenu PropSubMenu = MenuPool.AddSubMenu(PropsUIMenu, description);
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(ModelPed, PropsNumber, DrawableNumber);
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "", 0, NumberOfTextureVariations, 1);
                    //if (existingComponent != null && existingComponent.DrawableID == DrawableNumber)
                    //{
                    //    Test.Value = existingComponent.TextureID;
                    //}
                    Test.Formatter = v => v == 0 ? "Default" : "Texture ID " + v.ToString() + $" of {NumberOfTextureVariations}";
                    PropSubMenu.AddItem(Test);
                }
                //if (existingComponent != null)
                //{
                //    PropSubMenu.CurrentSelection = existingComponent.DrawableID;
                //}
                PropSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                PropSubMenu.OnItemSelect += OnPropItemSelect;
                PropSubMenu.OnScrollerChange += OnPropsScollerChange;
                PropSubMenu.OnIndexChange += OnPropsIndexChange;
            }
        }
        ResetOtherItems();
    }
    private void ResetOtherItems()//this doesnt work at all, need a way to go through eahc item and set it
    {
        foreach (UIMenuItem uimen in CustomizeHeadMenu.MenuItems)
        {
            HeadOverlayData hod = HeadOverlayLookups.FirstOrDefault(x => x.Part == uimen.Text);
            if (hod != null)
            {
                HeadOverlayData ho2d = WorkingVariation.HeadOverlays.FirstOrDefault(x => x.Part == hod.Part);

                if (uimen.GetType() == typeof(UIMenu))
                {
                    UIMenu test = (UIMenu)(object)uimen;
                    foreach (UIMenuItem uimen2 in test.MenuItems)
                    {
                        if (uimen2.Text == "Set Primary Color" && uimen2.GetType() == typeof(UIMenuListScrollerItem<ColorLookup>))
                        {
                            UIMenuListScrollerItem<ColorLookup> test2 = (UIMenuListScrollerItem<ColorLookup>)(object)uimen2;
                            test2.Index = ho2d.PrimaryColor;
                        }
                        if (uimen2.Text == "Set Secondary Color" && uimen2.GetType() == typeof(UIMenuListScrollerItem<ColorLookup>))
                        {
                            UIMenuListScrollerItem<ColorLookup> test2 = (UIMenuListScrollerItem<ColorLookup>)(object)uimen2;
                            test2.Index = ho2d.SecondaryColor;
                        }
                    }
                }
            }
        }
    }
    private void ResetPedModel()
    {
        if (ModelPed.Exists())
        {
            ModelPed.Delete();
        }
        WorkingVariation = new PedVariation();
        if (ModelPed.Exists())
        {
            WorkingName = Names.GetRandomName(ModelPed.IsMale);
        }
        else
        {
            WorkingName = Names.GetRandomName(false);
        }
        ChangeName.Description = "Current: " + WorkingName;
        RandomizeName.Description = "Current: " + WorkingName;
        ChoseNewModel = true;
    }
    private void SetModelAsCharacter()
    {
        if (Player.CurrentModelVariation != null)
        {
            Player.CurrentModelVariation.ApplyToPed(ModelPed);//this makes senese, but it isnt going to include headblend shit most of the time
            WorkingVariation = Player.CurrentModelVariation;
        }
    }
    private void SetupComponentsMenu()
    {
        ComponentsUIMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Customize Components");
        ComponentsUIMenu.SetBannerType(EntryPoint.LSRedColor);
        ComponentsUIMenu.OnItemSelect += ComponentsMenu_OnItemSelect;
    }
    private void SetupDemographicsSubMenu()
    {
        DemographicsSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Set Demographics");
        DemographicsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        DemographicsSubMenu.OnItemSelect += DemographicsMenu_OnItemSelect;

        ChangeName = new UIMenuItem("Change Name", "Current: " + WorkingName);
        RandomizeName = new UIMenuItem("Randomize Name", "Current: " + WorkingName);
        ChangeMoney = new UIMenuItem("Set Money", "Amount: " + WorkingMoney.ToString("C0"));

        DemographicsSubMenu.AddItem(ChangeName);
        DemographicsSubMenu.AddItem(RandomizeName);
        DemographicsSubMenu.AddItem(ChangeMoney);
    }
    private void SetupHeadMenu()
    {
        CustomizeHeadMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Set Head");
        CustomizeHeadMenu.SetBannerType(EntryPoint.LSRedColor);
        CustomizeHeadMenu.OnItemSelect += HeadMenu_OnItemSelect;
        CustomizeHeadMenu.OnScrollerChange += HeadMenu_OnScrollerChange;

        RandomizeHead = new UIMenuItem("Randomize Head", "Set random head data");
        Parent1IDMenu = new UIMenuListScrollerItem<HeadLookup>("Set Parent 1", "Select parent ID 1", HeadList);
        Parent2IDMenu = new UIMenuListScrollerItem<HeadLookup>("Set Parent 2", "Select parent ID 2", HeadList);
        Parent1MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 1 Mix", "Select percent of parent ID 1 to use", 0.0f, 1.0f, 0.1f);
        //Parent1MixMenu.Formatter = v => v.ToString("P0");
        Parent2MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 2 Mix", "Select percent of parent ID 2 to use", 0.0f, 1.0f, 0.1f);
        //Parent2MixMenu.Formatter = v => v.ToString("P0");
        RandomizeHair = new UIMenuItem("Randomize Hair", "Set random hair (use components to select manually)");
        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Hair Color", "Select primary hair color (requires head data)", ColorList);
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Hair Color", "Select secondary hair color (requires head data)", ColorList);

        CustomizeHeadMenu.AddItem(RandomizeHead);
        CustomizeHeadMenu.AddItem(Parent1IDMenu);
        CustomizeHeadMenu.AddItem(Parent2IDMenu);
        CustomizeHeadMenu.AddItem(Parent1MixMenu);
        CustomizeHeadMenu.AddItem(Parent2MixMenu);
        CustomizeHeadMenu.AddItem(RandomizeHair);
        CustomizeHeadMenu.AddItem(HairPrimaryColorMenu);
        CustomizeHeadMenu.AddItem(HairSecondaryColorMenu);

        foreach (HeadOverlayData ho in HeadOverlayLookups)
        {
            UIMenu overlayHeaderMenu = MenuPool.AddSubMenu(CustomizeHeadMenu, ho.Part);
            overlayHeaderMenu.SetBannerType(EntryPoint.LSRedColor);
            overlayHeaderMenu.TitleText = $"{ho.Part}";
            UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Color", "Select primary color", ColorList);
            UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Color", "Select secondary color", ColorList);
            UIMenuNumericScrollerItem<float> OpacityMenu = new UIMenuNumericScrollerItem<float>($"Opacity", $"Modify opacity", 0.0f, 1.0f, 0.1f);
            OpacityMenu.Formatter = v => v.ToString("P0");
            overlayHeaderMenu.AddItem(PrimaryColorMenu);
            overlayHeaderMenu.AddItem(SecondaryColorMenu);
            overlayHeaderMenu.AddItem(OpacityMenu);
            int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
            UIMenuNumericScrollerItem<int> OverlayIndexMenu = new UIMenuNumericScrollerItem<int>($"Index", $"Modify index", -1, TotalItems - 1, 1);
            OverlayIndexMenu.Formatter = v => v == -1 ? "None" : "Overlay " + v.ToString();
            if (ho.Index != 255)
            {
                OverlayIndexMenu.Value = ho.Index;
            }
            else
            {
                OverlayIndexMenu.Value = -1;
            }
            overlayHeaderMenu.AddItem(OverlayIndexMenu);
            overlayHeaderMenu.OnScrollerChange += HeadOverlay_OnScrollerChange;
        }
    }
    private void SetupLists()
    {
        ComponentLookup = new List<FashionComponent>() {
            new FashionComponent(0,"Face"),
            new FashionComponent(1, "Mask/Beard"),
            new FashionComponent(2, "Hair"),
            new FashionComponent(3, "Torso"),
            new FashionComponent(4, "Lower"),
            new FashionComponent(5, "Bags"),
            new FashionComponent(6, "Foot"),
            new FashionComponent(7, "Accessories"),
            new FashionComponent(8, "Undershirt"),
            new FashionComponent(9, "Body Armor"),
            new FashionComponent(10, "Decals"),
            new FashionComponent(11, "Tops"), };

        PropLookup = new List<FashionProp>() {
            new FashionProp(0,"Hats"),
            new FashionProp(1, "Glasses"),
            new FashionProp(2, "Ear"),};

        ColorList = new List<ColorLookup>()
        {
        new ColorLookup(0,"Black 1"),
        new ColorLookup(1,"Black 2"),
        new ColorLookup(2,"Black 3"),
        new ColorLookup(3,"Brown 1"),
        new ColorLookup(4,"Brown 2"),
        new ColorLookup(5,"Brown 3"),
        new ColorLookup(6,"Brown 4"),
        new ColorLookup(7,"Brown 5"),
        new ColorLookup(8,"Brown 6"),
        new ColorLookup(9,"Blonde 1"),
        new ColorLookup(10,"Blonde 2"),
        new ColorLookup(11,"Blonde 3"),
        new ColorLookup(12,"Blonde 4"),
        new ColorLookup(13,"Blonde 5"),
        new ColorLookup(14,"Blonde 6"),
        new ColorLookup(15,"Blonde 7"),
        new ColorLookup(16,"Blonde 8"),
        new ColorLookup(17,"Blonde 9"),
        new ColorLookup(18,"Redhead 1"),
        new ColorLookup(19,"Redhead 2"),
        new ColorLookup(20,"Redhead 3"),
        new ColorLookup(21,"Redhead 4"),
        new ColorLookup(22,"Orange 1"),
        new ColorLookup(23,"Orange 2"),
        new ColorLookup(24,"Orange 3"),
        new ColorLookup(25,"Orange 4"),
        new ColorLookup(26,"Grey 1"),
        new ColorLookup(27,"Grey 2"),
        new ColorLookup(28,"White 1"),
        new ColorLookup(29,"White 2"),
        new ColorLookup(30,"Purple 1"),
        new ColorLookup(31,"Purple 2"),
        new ColorLookup(32,"Purple 3"),
        new ColorLookup(33,"Pink 1"),
        new ColorLookup(34,"Pink 2"),
        new ColorLookup(35,"Pink 3"),
        new ColorLookup(36,"Blue 4"),
        new ColorLookup(37,"Blue 5"),
        new ColorLookup(38,"Blue 6"),
        new ColorLookup(39,"Green 1"),
        new ColorLookup(40,"Green 2"),
        new ColorLookup(41,"Green 3"),
        new ColorLookup(42,"Green 4"),
        new ColorLookup(43,"Green 5"),
        new ColorLookup(44,"Green 6"),
        new ColorLookup(45,"Yellow 1"),
        new ColorLookup(46,"Yellow 2"),
        new ColorLookup(47,"Yellow 3"),
        new ColorLookup(48,"Orange 5"),
        new ColorLookup(49,"Orange 6"),
        new ColorLookup(50,"Orange 7"),
        new ColorLookup(51,"Orange 8"),
        new ColorLookup(52,"Red 1"),
        new ColorLookup(53,"Red 1"),
        new ColorLookup(54,"Red 1"),
        new ColorLookup(55,"Brown 7"),
        new ColorLookup(56,"Brown 8"),
        new ColorLookup(57,"Brown 9"),
        new ColorLookup(58,"Brown 10"),
        new ColorLookup(59,"Brown 11"),
        new ColorLookup(60,"Brown 12"),
        new ColorLookup(61,"Black 4"),
        new ColorLookup(62,"Unk 1"),
        new ColorLookup(63,"Unk 2"),
        };

        HeadList = new List<HeadLookup>()
        {
            new HeadLookup(0,"Benjamin"),
            new HeadLookup(1,"Daniel"),
            new HeadLookup(2,"Joshua"),
            new HeadLookup(3,"Noah"),
            new HeadLookup(4,"Andrew"),
            new HeadLookup(5,"Juan"),
            new HeadLookup(6,"Alex"),
            new HeadLookup(7,"Isaac"),
            new HeadLookup(8,"Evan"),
            new HeadLookup(9,"Ethan"),
            new HeadLookup(10,"Vincent"),
            new HeadLookup(11,"Angel"),
            new HeadLookup(12,"Diego"),
            new HeadLookup(13,"Adrian"),
            new HeadLookup(14,"Gabriel"),
            new HeadLookup(15,"Michael"),
            new HeadLookup(16,"Santiago"),
            new HeadLookup(17,"Kevin"),
            new HeadLookup(18,"Louis"),
            new HeadLookup(19,"Samuel"),
            new HeadLookup(20,"Anthony"),
            new HeadLookup(21,"Hannah"),
            new HeadLookup(22,"Audrey"),
            new HeadLookup(23,"Jasmine"),
            new HeadLookup(24,"Giselle"),
            new HeadLookup(25,"Amelia"),
            new HeadLookup(26,"Isabella"),
            new HeadLookup(27,"Zoe"),
            new HeadLookup(28,"Ava"),
            new HeadLookup(29,"Camila"),
            new HeadLookup(30,"Violet"),
            new HeadLookup(31,"Sophia"),
            new HeadLookup(32,"Evelyn"),
            new HeadLookup(33,"Nicole"),
            new HeadLookup(34,"Ashley"),
            new HeadLookup(35,"Grace"),
            new HeadLookup(36,"Brianna"),
            new HeadLookup(37,"Natalie"),
            new HeadLookup(38,"Olivia"),
            new HeadLookup(39,"Elizabeth"),
            new HeadLookup(40,"Charlotte"),
            new HeadLookup(41,"Emma"),
            new HeadLookup(42,"Claude"),
            new HeadLookup(43,"Niko"),
            new HeadLookup(44,"John"),
            new HeadLookup(45,"Misty"),
        };

        HeadOverlayLookups = new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes"),
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1 },
            new HeadOverlayData(3, "Ageing"),
            new HeadOverlayData(4, "Makeup"),
            new HeadOverlayData(5, "Blush") { ColorType = 2 },
            new HeadOverlayData(6, "Complexion"),
            new HeadOverlayData(7, "Sun Damage"),
            new HeadOverlayData(8, "Lipstick") { ColorType = 2 },
            new HeadOverlayData(9, "Moles/Freckles"),
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1 },
            new HeadOverlayData(11, "Body Blemishes"),
            new HeadOverlayData(12, "Add Body Blemishes"),};
    }
    private void SetupMenu()
    {
        SetupLists();
        CustomizeMainMenu.Clear();
        SetupDemographicsSubMenu();
        SetupModelMenu();
        SetupHeadMenu();
        SetupComponentsMenu();
        SetupPropsMenu();
        RefreshMenuList();
        SetupOtherMenu();
    }
    private void SetupModelMenu()
    {
        ChangeModel = new UIMenuItem("Input Model", "Enter model name");
        SelectModel = new UIMenuListScrollerItem<string>("Select Model", "Select from available", Rage.Model.PedModels.Select(x => x.Name));
        RandomizeVariation = new UIMenuItem("Randomize Variation", "Set random variation");
        DefaultVariation = new UIMenuItem("Default Variation", "Set default variation");
        CustomizeMainMenu.AddItem(ChangeModel);
        CustomizeMainMenu.AddItem(SelectModel);
        CustomizeMainMenu.AddItem(RandomizeVariation);
        CustomizeMainMenu.AddItem(DefaultVariation);
    }
    private void SetupOtherMenu()
    {
        ClearProps = new UIMenuItem("Clear Props", "Remove ALL props from displayed character");
        ClearProps.RightBadge = UIMenuItem.BadgeStyle.Crown;
        BecomeModel = new UIMenuItem("Become Character", "Return to gameplay as displayed character");
        BecomeModel.RightBadge = UIMenuItem.BadgeStyle.Clothes;
        Exit = new UIMenuItem("Exit", "Return to gameplay as old character");
        Exit.RightBadge = UIMenuItem.BadgeStyle.Alert;
        CustomizeMainMenu.AddItem(ClearProps);
        CustomizeMainMenu.AddItem(BecomeModel);
        CustomizeMainMenu.AddItem(Exit);
    }
    private void SetupPropsMenu()
    {
        PropsUIMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Customize Props");
        PropsUIMenu.SetBannerType(EntryPoint.LSRedColor);
        PropsUIMenu.OnItemSelect += PropsMenu_OnItemSelect;
    }
    private class ColorLookup
    {
        public ColorLookup()
        {
        }
        public ColorLookup(int colorID, string colorName)
        {
            ColorID = colorID;
            ColorName = colorName;
        }

        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public override string ToString()
        {
            return ColorName;
        }
    }
    private class FashionComponent
    {
        public FashionComponent()
        {
        }
        public FashionComponent(int componentID, string componentName)
        {
            ComponentID = componentID;
            ComponentName = componentName;
        }

        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
    }
    private class FashionProp
    {
        public FashionProp()
        {
        }
        public FashionProp(int propID, string propName)
        {
            PropID = propID;
            PropName = propName;
        }

        public int PropID { get; set; }
        public string PropName { get; set; }
    }
    private class HeadLookup
    {
        public HeadLookup()
        {
        }
        public HeadLookup(int headID, string headName)
        {
            HeadID = headID;
            HeadName = headName;
        }

        public int HeadID { get; set; }
        public string HeadName { get; set; }
        public override string ToString()
        {
            return HeadName;
        }
    }
}