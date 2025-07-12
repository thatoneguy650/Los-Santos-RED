using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;


public class DebugPropAttachSubMenu : DebugSubMenu
{
    private Vector3 Offset;
    private Rotator Rotation;
    private bool isPrecise;
    private bool isRunning;
    private string FilterString;
    private UIMenu ModelSearchResultSubMenu;
    private ModDataFileManager ModDataFileManager;
    private TestAnimation SelectedAnimation;
    private DebugMenu DebugMenu;

    private float BlendIn = 8.0f;
    private float BlendOut = -8.0f;
    private int Time = -1;
    private int Flags = 0;
    private UIMenuItem playAnimationMenu;
    private UIMenuCheckboxItem IsFacialMenu;
    private UIMenuListScrollerItem<AnimationSelector> animationsScroller;

    public DebugPropAttachSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ModDataFileManager modDataFileManager, DebugMenu debugMenu) : base(debug, menuPool, player)
    {
        ModDataFileManager = modDataFileManager;
        DebugMenu = debugMenu;
    }
    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Prop Attach Menu");
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Find and test various prop attachments.";
        SubMenu.Clear();
        SubMenu.Width = 0.5f;
        CreateMenu();

    }
    public override void Update()
    {
        if(!string.IsNullOrEmpty(DebugMenu.SelectedAnimationDictionary) && !string.IsNullOrEmpty(DebugMenu.SelectedAnimationName))
        {
            if(!animationsScroller.Items.Any(x=> x.Dictionary == DebugMenu.SelectedAnimationDictionary && x.Animation == DebugMenu.SelectedAnimationName))
            {
                animationsScroller.Items.Add(new AnimationSelector(DebugMenu.SelectedAnimationDictionary, DebugMenu.SelectedAnimationName));
            }
        }

        base.Update();
    }
    private void CreateMenu()
    {
        UIMenuListScrollerItem<PhysicalItem> physicalItemsScroller = new UIMenuListScrollerItem<PhysicalItem>("Select Item", "", ModDataFileManager.PhysicalItems.Items.ToList());

        SubMenu.AddItem(physicalItemsScroller);


        List<string> PossibleBones = new List<string>()
        {
            "BONETAG_R_PH_HAND","BONETAG_L_PH_HAND","BONETAG_HEAD","BONETAG_R_THIGH","BONETAG_L_THIGH","BONETAG_PELVIS","BONETAG_SPINE_ROOT","BONETAG_SPINE"
        };
        UIMenuListScrollerItem<string> bonesScroller = new UIMenuListScrollerItem<string>("Select Bone", "", PossibleBones.ToList());

        SubMenu.AddItem(bonesScroller);


        List<AnimationSelector> PossibleAnimations = new List<AnimationSelector>()
        {
            new AnimationSelector("mp_common","givetake1_a"),
            new AnimationSelector("mp_common","givetake1_b"),
            new AnimationSelector("oddjobs@shop_robbery@rob_till", "loop"),
            new AnimationSelector("anim@scripted@heist@ig1_table_grab@cash@male@","grab"),
            new AnimationSelector("anim@heists@fleeca_bank@drilling","drill_straight_start"),


            new AnimationSelector("missheistfbisetup1","hassle_intro_loop_f"),

        };
        animationsScroller = new UIMenuListScrollerItem<AnimationSelector>("Select Animation", "", PossibleAnimations.ToList());

        SubMenu.AddItem(animationsScroller);

        playAnimationMenu = new UIMenuItem("Start", "Start the attachment and animation");
        playAnimationMenu.Activated += (sender, selectedItem) =>
        {
            SubMenu.Visible = false;
            SetPropAttachment(physicalItemsScroller.SelectedItem.ModelName, bonesScroller.SelectedItem, animationsScroller.SelectedItem);
        };
        SubMenu.AddItem(playAnimationMenu);
    }

    private void SetPropAttachment(string propName, string boneName, AnimationSelector animationSelector)
    {
        //string PropName = NativeHelper.GetKeyboardInput(propName);
        try
        {
            Rage.Object SmokedItem = new Rage.Object(Game.GetHashKey(propName), Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            uint GameTimeLastAttached = 0;
            Offset = new Vector3();
            Rotation = new Rotator();
            isPrecise = false;
            if (SmokedItem.Exists())
            {
                string dictionary = animationSelector.Dictionary;// NativeHelper.GetKeyboardInput(animationSelector.Dictionary);
                string animation = animationSelector.Animation;// NativeHelper.GetKeyboardInput(animationSelector.Animation);




                //            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
                //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
                AnimationDictionary.RequestAnimationDictionay(dictionary);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, dictionary, animation, 4.0f, -4.0f, -1, (int)(AnimationFlags.Loop | AnimationFlags.UpperBodyOnly | AnimationFlags.SecondaryTask), 0, false, false, false);//-1
                isRunning = true;
                AttachItem(SmokedItem, boneName, new Vector3(0.0f, 0.0f, 0f), new Rotator(0f, 0f, 0f));
                GameFiber.StartNew(delegate
                {
                    try
                    {
                        while (!Game.IsKeyDownRightNow(Keys.Space) && SmokedItem.Exists())
                        {
                            if (Game.GameTime - GameTimeLastAttached >= 100 && CheckAttachmentkeys())
                            {
                                AttachItem(SmokedItem, boneName, Offset, Rotation);
                                GameTimeLastAttached = Game.GameTime;
                            }
                            if (Game.IsKeyDown(Keys.B))
                            {
                                //EntryPoint.WriteToConsoleTestLong($"Item {PropName} Attached to  {boneName} new Vector3({Offset.X}f,{Offset.Y}f,{Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)");

                                EntryPoint.WriteToConsole($"new PropAttachment(\"NAMEHERE\", \"{boneName}\", new Vector3({Offset.X}f, {Offset.Y}f, {Offset.Z}f),new Rotator({Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f)),");
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.N))
                            {
                                isPrecise = !isPrecise;
                                GameFiber.Sleep(500);
                            }
                            if (Game.IsKeyDown(Keys.D0))
                            {
                                isRunning = !isRunning;
                                NativeFunction.Natives.SET_ENTITY_ANIM_SPEED(Player.Character, dictionary, animation, isRunning ? 1.0f : 0.0f);
                                GameFiber.Sleep(500);
                            }
                            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, dictionary, animation);
                            Game.DisplayHelp($"Press SPACE to Stop~n~Press T-P to Increase~n~Press G=; to Decrease~n~Press B to print~n~Press N Toggle Precise {isPrecise} ~n~Press 0 Pause{isRunning}");
                            Game.DisplaySubtitle($"{Offset.X}f,{Offset.Y}f,{Offset.Z}f -- {Rotation.Pitch}f, {Rotation.Roll}f, {Rotation.Yaw}f");
                            //Game.DisplaySubtitle($"Current Animation Time {AnimationTime}");
                            GameFiber.Yield();
                        }

                        if (SmokedItem.Exists())
                        {
                            SmokedItem.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                        EntryPoint.ModController.CrashUnload();
                    }
                }, "Run Debug Logic");
            }
        }
        catch (Exception e)
        {
            Game.DisplayNotification("ERROR DEBUG");
        }
    }
    private void AttachItem(Rage.Object SmokedItem, string boneName, Vector3 offset, Rotator rotator)
    {
        if (SmokedItem.Exists())
        {
            SmokedItem.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, boneName), offset, rotator);

        }
    }
    private bool CheckAttachmentkeys()
    {

        float adderOffset = isPrecise ? 0.001f : 0.01f;
        float rotatorOFfset = isPrecise ? 1.0f : 10f;
        if (Game.IsKeyDownRightNow(Keys.T))//X UP?
        {
            Offset = new Vector3(Offset.X + adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.G))//X Down?
        {
            Offset = new Vector3(Offset.X - adderOffset, Offset.Y, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.Y))//Y UP?
        {
            Offset = new Vector3(Offset.X, Offset.Y + adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.H))//Y Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y - adderOffset, Offset.Z);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.U))//Z Up?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z + adderOffset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.J))//Z Down?
        {
            Offset = new Vector3(Offset.X, Offset.Y, Offset.Z - adderOffset);
            return true;
        }

        else if (Game.IsKeyDownRightNow(Keys.I))//XR Up?
        {
            Rotation = new Rotator(Rotation.Pitch + rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.K))//XR Down?
        {
            Rotation = new Rotator(Rotation.Pitch - rotatorOFfset, Rotation.Roll, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.O))//YR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll + rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.L) || Game.IsKeyDownRightNow(Keys.OemPeriod))//YR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll - rotatorOFfset, Rotation.Yaw);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.OemOpenBrackets))//ZR Up?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw + rotatorOFfset);
            return true;
        }
        else if (Game.IsKeyDownRightNow(Keys.OemSemicolon))//ZR Down?
        {
            Rotation = new Rotator(Rotation.Pitch, Rotation.Roll, Rotation.Yaw - rotatorOFfset);
            return true;
        }
        return false;
    }
    private class AnimationSelector
    {
        public string Dictionary { get; set; }
        public string Animation { get; set; }
        public AnimationSelector() { }

        public AnimationSelector(string dictionary, string animation)
        {
            Dictionary = dictionary;
            Animation = animation;
        }

        public override string ToString()
        {
            return $"{Dictionary},{Animation}";
        }
    }
}

