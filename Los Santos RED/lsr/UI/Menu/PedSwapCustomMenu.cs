using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PedSwapCustomMenu : Menu
{
    private UIMenu PedSwapCustomUIMenu;
    private UIMenuListItem TakeoverRandomPed;
    private UIMenuItem BecomeRandomPed;
    private UIMenuItem BecomeRandomCop;
    private IPedSwap PedSwap;
    private List<DistanceSelect> Distances;
    MenuPool MenuPool;
    private UIMenu ComponentsUIMenu;
    private UIMenu PropsUIMenu;
    private UIMenuItem ClearProps;
    private string CurrentSelectedComponent = "-1";
    private string DrawableSelected;
    private int TextureSelected;

    private int CurrentComponent = -1;
    private int CurrentDrawable = -1;
    private int CurrentTexture = -1;
    private UIMenuItem ChangeModel;
    private string ModelSelected = "S_M_M_GENTRANSPORT";
    private Camera CharCam;
    private Ped PedModel;
    private Vector3 PreviousPos;
    private float PreviousHeading;
    private UIMenuItem BecomeModel;
    private UIMenuListItem SelectModel;
    private UIMenuItem Exit;
    private bool IsDisposed = false;
    private UIMenuItem RandomizeVariation;

    public PedSwapCustomMenu(MenuPool menuPool, Ped pedModel, IPedSwap pedSwap)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        PedModel = pedModel;
        PedSwapCustomUIMenu = new UIMenu("Customize Ped", "Become Custom Ped");
        PedSwapCustomUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        menuPool.Add(PedSwapCustomUIMenu);
        PedSwapCustomUIMenu.OnItemSelect += OnItemSelect;
        PedSwapCustomUIMenu.OnListChange += OnListChange;    
    }
    public void Setup()
    {
        Game.FadeScreenOut(1500, true);
        SetupPlayer();
        SetupCamera();
        SetupPedModel();
        SetupMenu();
        Game.FadeScreenIn(1500, true);
    }
    private void SetupPlayer()
    {

        PreviousPos = Game.LocalPlayer.Character.Position;
        PreviousHeading = Game.LocalPlayer.Character.Heading;
        Game.LocalPlayer.Character.Position = new Vector3(402.5164f, -1002.847f, -99.2587f);
    }
    private void SetupCamera()
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
    private void SetupPedModel()
    {
        if (PedModel.Exists())
        {
            PedModel.Delete();
        }
        PedModel = new Ped(ModelSelected, new Vector3(402.8473f, -996.3224f, -99.00025f), 182.7549f);
        GameFiber.Yield();
        if (PedModel.Exists())
        {
            PedModel.IsPersistent = true;
            PedModel.IsVisible = true;
            PedModel.BlockPermanentEvents = true;
        }
    }
    private void SetupMenu()
    {
        PedSwapCustomUIMenu.Clear();
        ChangeModel = new UIMenuItem("Change Model");
        PedSwapCustomUIMenu.AddItem(ChangeModel);
        SelectModel = new UIMenuListItem("Select Character", "", Rage.Model.PedModels.Select(x=> x.Name));
        PedSwapCustomUIMenu.AddItem(SelectModel);

        RandomizeVariation = new UIMenuItem("Randomize");
        PedSwapCustomUIMenu.AddItem(RandomizeVariation);


        ComponentsUIMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Customize Components");
        ComponentsUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        ComponentsUIMenu.OnItemSelect += OnItemSelect;
        ComponentsUIMenu.OnIndexChange += OnIndexChange;
        PropsUIMenu = MenuPool.AddSubMenu(PedSwapCustomUIMenu, "Customize Props");


        ClearProps = new UIMenuItem("Clear Props");
        PedSwapCustomUIMenu.AddItem(ClearProps);


        PropsUIMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
        PropsUIMenu.OnItemSelect += OnItemSelect;
        RefreshMenuList();

        BecomeModel = new UIMenuItem("Become Character");
        PedSwapCustomUIMenu.AddItem(BecomeModel);  
        Exit = new UIMenuItem("Exit");
        PedSwapCustomUIMenu.AddItem(Exit);
    }
    private void RefreshMenuList()
    {
        ComponentsUIMenu.Clear();
        PropsUIMenu.Clear();
        if (PedModel.Exists())
        {
            for (int ComponentNumber = 0; ComponentNumber < 12; ComponentNumber++)
            {
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(PedModel, ComponentNumber);
                UIMenu ComponentSubMenu = MenuPool.AddSubMenu(ComponentsUIMenu, $"Component: {ComponentNumber}");
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(PedModel, ComponentNumber, DrawableNumber) - 1;
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "", 0, NumberOfTextureVariations, 1);
                    ComponentSubMenu.AddItem(Test);
                }
                ComponentSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                ComponentSubMenu.OnScrollerChange += OnComponentScollerChange;
                ComponentSubMenu.OnIndexChange += OnComponentIndexChange;
            }
            for (int PropsNumber = 0; PropsNumber < 8; PropsNumber++)
            {
                int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(Game.LocalPlayer.Character, PropsNumber);
                UIMenu PropSubMenu = MenuPool.AddSubMenu(PropsUIMenu, $"Prop {PropsNumber}");
                for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
                {
                    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(PedModel, PropsNumber, DrawableNumber);
                    UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "", 0, NumberOfTextureVariations, 1);
                    PropSubMenu.AddItem(Test);
                }
                PropSubMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                PropSubMenu.OnScrollerChange += OnPropsScollerChange;
                PropSubMenu.OnIndexChange += OnPropsIndexChange;
            }
        }
    }
    public override void Hide()
    {
        PedSwapCustomUIMenu.Visible = false;
    }
    public override void Show()
    {
        PedSwapCustomUIMenu.Visible = true;
    }
    public override void Toggle()
    {

        if (!PedSwapCustomUIMenu.Visible)
        {
            PedSwapCustomUIMenu.Visible = true;
        }
        else
        {
            PedSwapCustomUIMenu.Visible = false;
        }
    }
    public void Update()
    {
        MenuPool.ProcessMenus();
    }
    public void Dispose()
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            if (!Game.IsScreenFadedOut && !Game.IsScreenFadingOut)
            {
                Game.FadeScreenOut(1500, true);
            }
            if (PedModel.Exists() && PedModel.Handle != Game.LocalPlayer.Character.Handle)
            {
                PedModel.Delete();
            }
            MenuPool.CloseAllMenus();
            CharCam.Active = false;
            Game.LocalPlayer.Character.Position = PreviousPos;
            Game.LocalPlayer.Character.Heading = PreviousHeading;
            Game.FadeScreenIn(1500, true);
        }
    }

    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if (sender == ComponentsUIMenu)
        {
            CurrentComponent = newIndex;
        }
        EntryPoint.WriteToConsole($"OnIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (sender == PedSwapCustomUIMenu)
        {
            if (selectedItem == ChangeModel)
            {
                ModelSelected = NativeHelper.GetKeyboardInput("player_zero");
                if(new Rage.Model(ModelSelected).IsValid)
                {
                    SetupPedModel();
                    RefreshMenuList();
                }
            }
            if (selectedItem == SelectModel)
            {
                if (new Rage.Model(ModelSelected).IsValid)
                {
                    SetupPedModel();
                    RefreshMenuList();
                }

            }
            if (selectedItem == BecomeModel)
            {
                if (PedModel.Exists())
                {
                    Game.FadeScreenOut(1500, true);
                    PedSwap.BecomeExistingPed(PedModel);
                    Dispose();
                }
            }
            if (selectedItem == ClearProps)
            {
                if (PedModel.Exists())
                {
                    NativeFunction.Natives.CLEAR_ALL_PED_PROPS(PedModel);
                }
            }
            if (selectedItem == RandomizeVariation)
            {
                if (PedModel.Exists())
                {
                    NativeFunction.Natives.SET_PED_RANDOM_COMPONENT_VARIATION(PedModel);
                    NativeFunction.Natives.SET_PED_RANDOM_PROPS(PedModel);
                }
            }
            if (selectedItem == Exit)
            {
                Dispose();
            }
        }
        else if (sender == ComponentsUIMenu)
        {
            CurrentSelectedComponent = selectedItem.Text;
            CurrentComponent = index;
        }
        else if (sender == PropsUIMenu)
        {
            CurrentSelectedComponent = selectedItem.Text;
            CurrentComponent = index;
        }
        //PedSwapCustomUIMenu.Visible = false;
        EntryPoint.WriteToConsole($"OnItemSelect {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if(list == SelectModel)
        {
            ModelSelected = list.SelectedValue.ToString();//  list.Items[index].Value;

        }
        EntryPoint.WriteToConsole($"OnListChange index {index} {ModelSelected} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnComponentScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        bool IsValid = false;
        if (PedModel.Exists())
        {
            IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            if (IsValid)
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, 0);
            }
        }
        EntryPoint.WriteToConsole($"OnComponentScollerChange IsValid {IsValid} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsIndexChange(UIMenu sender, int newIndex)
    {
        CurrentDrawable = newIndex;
        EntryPoint.WriteToConsole($"OnComponentIndexChange {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }
    private void OnPropsScollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        TextureSelected = newIndex;
        CurrentTexture = newIndex;
        int propID = -1;
        if (PedModel.Exists())
        {
            //bool IsValid = NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture);
            //if (IsValid || !IsValid)
            //{
            propID = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(PedModel, CurrentComponent);
            NativeFunction.Natives.CLEAR_PED_PROP(PedModel, propID);
            NativeFunction.Natives.SET_PED_PROP_INDEX<bool>(PedModel, CurrentComponent, CurrentDrawable, CurrentTexture, true);
            //}
        }
        EntryPoint.WriteToConsole($"OnPropsScollerChange propID {propID} {CurrentSelectedComponent} {DrawableSelected} {TextureSelected} : {CurrentComponent} {CurrentDrawable} {CurrentTexture}", 5);
    }

}