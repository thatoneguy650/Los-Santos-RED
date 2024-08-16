using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;


public class CustomizeVoiceMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private UIMenuItem CurrentVoice;
    private UIMenuListScrollerItem<string> SelectModel;
    private UIMenuItem SearchModel;
    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenu ModelSearchSubMenu;
    private string FilterString = "";
    private List<string> SpeechList;
    private List<VoicePreview> VoicePreviewList;
    //private List<string> VoiceList;
    private UIMenuListScrollerItem<string> PreviewVoice;
    private UIMenu VoiceSubMenu;
    private UIMenuItem VoiceSubMenuItem;
    private UIMenuListScrollerItem<VoicePreview> FilteredVoices;
    private UIMenuItem SetVoice;
    private string GenderLimit = "";
    private UIMenuListScrollerItem<string> LimitGender;

    public CustomizeVoiceMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        VoiceSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Voice");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the voice of the current ped. " +
            "Only available for the freemode peds. " +
            "Other peds will use their built in voice.";
        VoiceSubMenuItem = CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1];
        VoiceSubMenu.SetBannerType(EntryPoint.LSRedColor);
        VoiceSubMenu.InstructionalButtonsEnabled = false;
        VoiceSubMenu.Width = 0.55f;
        AddNames();


        VoiceSubMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        VoiceSubMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };


        CurrentVoice = new UIMenuItem("Current Voice", $"Current voice {PedCustomizer.WorkingVoice}") { RightLabel = PedCustomizer.WorkingVoice };
        CurrentVoice.Enabled = false;
        VoiceSubMenu.AddItem(CurrentVoice);
        VoiceSubMenu.Width = 0.55f;



        //ModelSearchSubMenu = MenuPool.AddSubMenu(VoiceSubMenu, "Search Voice");
        //VoiceSubMenu.MenuItems[VoiceSubMenu.MenuItems.Count() - 1].Description = "Search for the voice by name";
        //VoiceSubMenu.MenuItems[VoiceSubMenu.MenuItems.Count() - 1].RightLabel = "";
        //ModelSearchSubMenu.SetBannerType(EntryPoint.LSRedColor);
        //ModelSearchSubMenu.InstructionalButtonsEnabled = false;
        //ModelSearchSubMenu.Width = 0.35f;

        SearchModel = new UIMenuItem("Search", "Input the search string for the voice.");
        SearchModel.Activated += (sender, selectedItem) =>
        {
            FilterString = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(FilterString))
            {
                FilterString = "";
            }
            FilteredVoices.Items = GetFilteredList();// VoicePreviewList.Where(x => FilterString == "" || x.VoiceName.ToLower().Contains(FilterString.ToLower())).OrderBy(x => x.TypeName).ThenByDescending(x => x.Gender).ThenBy(x => x.VoiceName).ToList();
            SearchModel.RightLabel = FilterString;
        };
        VoiceSubMenu.AddItem(SearchModel);


        LimitGender = new UIMenuListScrollerItem<string>("Gender", "Select to limit the search results gender.", new List<string>() { "M","F","U","All" });
        LimitGender.Activated += (sender, selectedItem) =>
        {
            GenderLimit = LimitGender.SelectedItem == "All" ? "" : LimitGender.SelectedItem;
            FilteredVoices.Items = GetFilteredList();// VoicePreviewList.Where(x => FilterString == "" || x.VoiceName.ToLower().Contains(FilterString.ToLower())).OrderBy(x => x.TypeName).ThenByDescending(x => x.Gender).ThenBy(x => x.VoiceName).ToList();

        };
        VoiceSubMenu.AddItem(LimitGender);



        FilteredVoices = new UIMenuListScrollerItem<VoicePreview>("Search Results", "List of voices matching the search string.", VoicePreviewList.Where(x => FilterString == "" || x.VoiceName.ToLower().Contains(FilterString.ToLower())).OrderBy(x => x.TypeName).ThenByDescending(x => x.Gender).ThenBy(x => x.VoiceName));
        FilteredVoices.IndexChanged += (sender, e, selectedItem) =>
        {
            if(FilteredVoices.SelectedItem != null)
            {
                FilteredVoices.Description = $"List of voices matching the search string. ~n~{FilteredVoices.SelectedItem.GetFullDescription()}";
            }
        };

        VoiceSubMenu.AddItem(FilteredVoices);






        PreviewVoice = new UIMenuListScrollerItem<string>("Preview Voice", "Select to preview the search result voice.", SpeechList);
        PreviewVoice.Activated += (sender, selectedItem) =>
        {
            if (FilteredVoices.SelectedItem != null && !string.IsNullOrEmpty(FilteredVoices.SelectedItem.VoiceName))
            {
                PlayVoicePreview(FilteredVoices.SelectedItem.VoiceName,PreviewVoice.SelectedItem);
            }
        };
        VoiceSubMenu.AddItem(PreviewVoice);


        SetVoice = new UIMenuItem("Set Voice", "Select to set the search result voice as the ped's current voice.");
        SetVoice.Activated += (sender, selectedItem) =>
        {

            if (FilteredVoices.SelectedItem != null && !string.IsNullOrEmpty(FilteredVoices.SelectedItem.VoiceName))
            {
                Game.DisplaySubtitle($"Voice Set to {FilteredVoices.SelectedItem}");
                SetVoiceFromString(FilteredVoices.SelectedItem.VoiceName);
            }
        };
        VoiceSubMenu.AddItem(SetVoice);


    }
    private List<VoicePreview> GetFilteredList()
    {
        return VoicePreviewList.Where(x => 
        
        
       ( FilterString == "" || x.VoiceName.ToLower().Contains(FilterString.ToLower()) )


       &&

       ( GenderLimit == "" || x.Gender.ToLower().Contains(GenderLimit.ToLower()))


        
        
        
        ).OrderBy(x => x.TypeName).ThenByDescending(x => x.Gender).ThenBy(x => x.VoiceName).ToList();
    }


    private void PlayVoicePreview(string voiceName, string toPlay)
    {
        if (PedCustomizer.ModelPed.Exists() && voiceName != "")
        {
            PedCustomizer.ModelPed.PlayAmbientSpeech(voiceName, toPlay, 0, SpeechModifier.Force | SpeechModifier.AllowRepeat);     
        }
    }
    public void OnModelChanged()
    {
        if (PedCustomizer.PedModelIsFreeMode)
        {
            VoiceSubMenuItem.Enabled = true;
        }
        else
        {
            VoiceSubMenuItem.Enabled = false;
        }
    }
    private void SetVoiceFromString(string voiceName)
    {
        CurrentVoice.Description = $"Current: {voiceName}";
        CurrentVoice.RightLabel = voiceName;
        PedCustomizer.WorkingVoice = voiceName;
    }
    private void AddNames()
    {
        SpeechList = new List<string>()
        {
            "GENERIC_HOWS_IT_GOING", "GENERIC_HI","PROVOKE_GENERIC", "GENERIC_WHATEVER","CHAT_RESP", "PED_RANT_RESP", "CHAT_STATE", "PED_RANT","CHAT_STATE", "PED_RANT", "CHAT_RESP", "PED_RANT_RESP", "CULT_TALK",
        };
        //A_M_Y_BUSINESS_01_WHITE_FULL_01


        VoicePreviewList = new List<VoicePreview>()
        {
            new VoicePreview("A_M_M_AFRIAMER_01_BLACK_FULL_01","Ambient","M","Middle","AFRIAMER")
            ,new VoicePreview("A_M_M_BEACH_01_BLACK_MINI_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_01_LATINO_FULL_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_01_LATINO_MINI_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_01_WHITE_FULL_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_01_WHITE_MINI_02","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_02_BLACK_FULL_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_02_WHITE_FULL_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_02_WHITE_MINI_01","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEACH_02_WHITE_MINI_02","Ambient","M","Middle","BEACH")
            ,new VoicePreview("A_M_M_BEVHILLS_01_BLACK_FULL_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_01_BLACK_MINI_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_01_WHITE_FULL_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_01_WHITE_MINI_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_02_BLACK_FULL_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_02_BLACK_MINI_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_02_WHITE_FULL_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BEVHILLS_02_WHITE_MINI_01","Ambient","M","Middle","BEVHILLS")
            ,new VoicePreview("A_M_M_BUSINESS_01_BLACK_FULL_01","Ambient","M","Middle","BUSINESS")
            ,new VoicePreview("A_M_M_BUSINESS_01_BLACK_MINI_01","Ambient","M","Middle","BUSINESS")
            ,new VoicePreview("A_M_M_BUSINESS_01_WHITE_FULL_01","Ambient","M","Middle","BUSINESS")
            ,new VoicePreview("A_M_M_BUSINESS_01_WHITE_MINI_01","Ambient","M","Middle","BUSINESS")
            ,new VoicePreview("A_M_M_EASTSA_01_LATINO_FULL_01","Ambient","M","Middle","EASTSA")
            ,new VoicePreview("A_M_M_EASTSA_01_LATINO_MINI_01","Ambient","M","Middle","EASTSA")
            ,new VoicePreview("A_M_M_EASTSA_02_LATINO_FULL_01","Ambient","M","Middle","EASTSA")
            ,new VoicePreview("A_M_M_EASTSA_02_LATINO_MINI_01","Ambient","M","Middle","EASTSA")
            ,new VoicePreview("A_M_M_FARMER_01_WHITE_MINI_01","Ambient","M","Middle","FARMER")
            ,new VoicePreview("A_M_M_FARMER_01_WHITE_MINI_02","Ambient","M","Middle","FARMER")
            ,new VoicePreview("A_M_M_FARMER_01_WHITE_MINI_03","Ambient","M","Middle","FARMER")
            ,new VoicePreview("A_M_M_FATLATIN_01_LATINO_FULL_01","Ambient","M","Middle","FATLATIN")
            ,new VoicePreview("A_M_M_FATLATIN_01_LATINO_MINI_01","Ambient","M","Middle","FATLATIN")
            ,new VoicePreview("A_M_M_GENERICMALE_01_WHITE_MINI_01","Ambient","M","Middle","GENERICMALE")
            ,new VoicePreview("A_M_M_GENERICMALE_01_WHITE_MINI_02","Ambient","M","Middle","GENERICMALE")
            ,new VoicePreview("A_M_M_GENERICMALE_01_WHITE_MINI_03","Ambient","M","Middle","GENERICMALE")
            ,new VoicePreview("A_M_M_GENERICMALE_01_WHITE_MINI_04","Ambient","M","Middle","GENERICMALE")
            ,new VoicePreview("A_M_M_GENFAT_01_LATINO_FULL_01","Ambient","M","Middle","GENFAT")
            ,new VoicePreview("A_M_M_GENFAT_01_LATINO_MINI_01","Ambient","M","Middle","GENFAT")
            ,new VoicePreview("A_M_M_GOLFER_01_BLACK_FULL_01","Ambient","M","Middle","GOLFER")
            ,new VoicePreview("A_M_M_GOLFER_01_WHITE_FULL_01","Ambient","M","Middle","GOLFER")
            ,new VoicePreview("A_M_M_GOLFER_01_WHITE_MINI_01","Ambient","M","Middle","GOLFER")
            ,new VoicePreview("A_M_M_HASJEW_01_WHITE_MINI_01","Ambient","M","Middle","HASJEW")
            ,new VoicePreview("A_M_M_HILLBILLY_01_WHITE_MINI_01","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_01_WHITE_MINI_02","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_01_WHITE_MINI_03","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_02_WHITE_MINI_01","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_02_WHITE_MINI_02","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_02_WHITE_MINI_03","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_HILLBILLY_02_WHITE_MINI_04","Ambient","M","Middle","HILLBILLY")
            ,new VoicePreview("A_M_M_INDIAN_01_INDIAN_MINI_01","Ambient","M","Middle","INDIAN")
            ,new VoicePreview("A_M_M_KTOWN_01_KOREAN_FULL_01","Ambient","M","Middle","KTOWN")
            ,new VoicePreview("A_M_M_KTOWN_01_KOREAN_MINI_01","Ambient","M","Middle","KTOWN")
            ,new VoicePreview("A_M_M_MALIBU_01_BLACK_FULL_01","Ambient","M","Middle","MALIBU")
            ,new VoicePreview("A_M_M_MALIBU_01_LATINO_FULL_01","Ambient","M","Middle","MALIBU")
            ,new VoicePreview("A_M_M_MALIBU_01_LATINO_MINI_01","Ambient","M","Middle","MALIBU")
            ,new VoicePreview("A_M_M_MALIBU_01_WHITE_FULL_01","Ambient","M","Middle","MALIBU")
            ,new VoicePreview("A_M_M_MALIBU_01_WHITE_MINI_01","Ambient","M","Middle","MALIBU")
            ,new VoicePreview("A_M_M_POLYNESIAN_01_POLYNESIAN_FULL_01","Ambient","M","Middle","POLYNESIAN")
            ,new VoicePreview("A_M_M_POLYNESIAN_01_POLYNESIAN_MINI_01","Ambient","M","Middle","POLYNESIAN")
            ,new VoicePreview("A_M_M_SALTON_01_WHITE_FULL_01","Ambient","M","Middle","SALTON")
            ,new VoicePreview("A_M_M_SALTON_02_WHITE_FULL_01","Ambient","M","Middle","SALTON")
            ,new VoicePreview("A_M_M_SALTON_02_WHITE_MINI_01","Ambient","M","Middle","SALTON")
            ,new VoicePreview("A_M_M_SALTON_02_WHITE_MINI_02","Ambient","M","Middle","SALTON")
            ,new VoicePreview("A_M_M_SKATER_01_BLACK_FULL_01","Ambient","M","Middle","SKATER")
            ,new VoicePreview("A_M_M_SKATER_01_WHITE_FULL_01","Ambient","M","Middle","SKATER")
            ,new VoicePreview("A_M_M_SKATER_01_WHITE_MINI_01","Ambient","M","Middle","SKATER")
            ,new VoicePreview("A_M_M_SKIDROW_01_BLACK_FULL_01","Ambient","M","Middle","SKIDROW")
            ,new VoicePreview("A_M_M_SOCENLAT_01_LATINO_FULL_01","Ambient","M","Middle","SOCENLAT")
            ,new VoicePreview("A_M_M_SOCENLAT_01_LATINO_MINI_01","Ambient","M","Middle","SOCENLAT")
            ,new VoicePreview("A_M_M_SOUCENT_01_BLACK_FULL_01","Ambient","M","Middle","SOUCENT")
            ,new VoicePreview("A_M_M_SOUCENT_02_BLACK_FULL_01","Ambient","M","Middle","SOUCENT")
            ,new VoicePreview("A_M_M_SOUCENT_03_BLACK_FULL_01","Ambient","M","Middle","SOUCENT")
            ,new VoicePreview("A_M_M_SOUCENT_04_BLACK_FULL_01","Ambient","M","Middle","SOUCENT")
            ,new VoicePreview("A_M_M_SOUCENT_04_BLACK_MINI_01","Ambient","M","Middle","SOUCENT")
            ,new VoicePreview("A_M_M_STLAT_02_LATINO_FULL_01","Ambient","M","Middle","STLAT")
            ,new VoicePreview("A_M_M_TENNIS_01_BLACK_MINI_01","Ambient","M","Middle","TENNIS")
            ,new VoicePreview("A_M_M_TENNIS_01_WHITE_MINI_01","Ambient","M","Middle","TENNIS")
            ,new VoicePreview("A_M_M_TOURIST_01_WHITE_MINI_01","Ambient","M","Middle","TOURIST")
            ,new VoicePreview("A_M_M_TRAMP_01_BLACK_FULL_01","Ambient","M","Middle","TRAMP")
            ,new VoicePreview("A_M_M_TRAMP_01_BLACK_MINI_01","Ambient","M","Middle","TRAMP")
            ,new VoicePreview("A_M_M_TRAMPBEAC_01_BLACK_FULL_01","Ambient","M","Middle","TRAMPBEAC")
            ,new VoicePreview("A_M_M_TRANVEST_01_WHITE_MINI_01","Ambient","M","Middle","TRANVEST")
            ,new VoicePreview("A_M_M_TRANVEST_02_LATINO_FULL_01","Ambient","M","Middle","TRANVEST")
            ,new VoicePreview("A_M_M_TRANVEST_02_LATINO_MINI_01","Ambient","M","Middle","TRANVEST")
            ,new VoicePreview("A_M_O_BEACH_01_WHITE_FULL_01","Ambient","M","Old","BEACH")
            ,new VoicePreview("A_M_O_BEACH_01_WHITE_MINI_01","Ambient","M","Old","BEACH")
            ,new VoicePreview("A_M_O_GENSTREET_01_WHITE_FULL_01","Ambient","M","Old","GENSTREET")
            ,new VoicePreview("A_M_O_GENSTREET_01_WHITE_MINI_01","Ambient","M","Old","GENSTREET")
            ,new VoicePreview("A_M_O_SALTON_01_WHITE_FULL_01","Ambient","M","Old","SALTON")
            ,new VoicePreview("A_M_O_SALTON_01_WHITE_MINI_01","Ambient","M","Old","SALTON")
            ,new VoicePreview("A_M_O_SOUCENT_01_BLACK_FULL_01","Ambient","M","Old","SOUCENT")
            ,new VoicePreview("A_M_O_SOUCENT_02_BLACK_FULL_01","Ambient","M","Old","SOUCENT")
            ,new VoicePreview("A_M_O_SOUCENT_03_BLACK_FULL_01","Ambient","M","Old","SOUCENT")
            ,new VoicePreview("A_M_O_TRAMP_01_BLACK_FULL_01","Ambient","M","Old","TRAMP")
            ,new VoicePreview("A_M_Y_BEACH_01_CHINESE_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_01_CHINESE_MINI_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_01_WHITE_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_01_WHITE_MINI_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_02_LATINO_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_02_WHITE_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_03_BLACK_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_03_BLACK_MINI_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACH_03_WHITE_FULL_01","Ambient","M","Young","BEACH")
            ,new VoicePreview("A_M_Y_BEACHVESP_01_CHINESE_FULL_01","Ambient","M","Young","BEACHVESP")
            ,new VoicePreview("A_M_Y_BEACHVESP_01_CHINESE_MINI_01","Ambient","M","Young","BEACHVESP")
            ,new VoicePreview("A_M_Y_BEACHVESP_01_WHITE_FULL_01","Ambient","M","Young","BEACHVESP")
            ,new VoicePreview("A_M_Y_BEACHVESP_02_WHITE_FULL_01","Ambient","M","Young","BEACHVESP")
            ,new VoicePreview("A_M_Y_BEACHVESP_02_WHITE_MINI_01","Ambient","M","Young","BEACHVESP")
            ,new VoicePreview("A_M_Y_BEVHILLS_01_BLACK_FULL_01","Ambient","M","Young","BEVHILLS")
            ,new VoicePreview("A_M_Y_BEVHILLS_01_WHITE_FULL_01","Ambient","M","Young","BEVHILLS")
            ,new VoicePreview("A_M_Y_BEVHILLS_02_BLACK_FULL_01","Ambient","M","Young","BEVHILLS")
            ,new VoicePreview("A_M_Y_BEVHILLS_02_WHITE_FULL_01","Ambient","M","Young","BEVHILLS")
            ,new VoicePreview("A_M_Y_BEVHILLS_02_WHITE_MINI_01","Ambient","M","Young","BEVHILLS")
            ,new VoicePreview("A_M_Y_BUSICAS_01_WHITE_MINI_01","Ambient","M","Young","BUSICAS")
            ,new VoicePreview("A_M_Y_BUSINESS_01_BLACK_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_01_BLACK_MINI_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_01_CHINESE_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_01_WHITE_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_01_WHITE_MINI_02","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_02_BLACK_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_02_BLACK_MINI_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_02_WHITE_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_02_WHITE_MINI_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_02_WHITE_MINI_02","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_03_BLACK_FULL_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_BUSINESS_03_WHITE_MINI_01","Ambient","M","Young","BUSINESS")
            ,new VoicePreview("A_M_Y_DOWNTOWN_01_BLACK_FULL_01","Ambient","M","Young","DOWNTOWN")
            ,new VoicePreview("A_M_Y_EASTSA_01_LATINO_FULL_01","Ambient","M","Young","EASTSA")
            ,new VoicePreview("A_M_Y_EASTSA_01_LATINO_MINI_01","Ambient","M","Young","EASTSA")
            ,new VoicePreview("A_M_Y_EASTSA_02_LATINO_FULL_01","Ambient","M","Young","EASTSA")
            ,new VoicePreview("A_M_Y_EPSILON_01_BLACK_FULL_01","Ambient","M","Young","EPSILON")
            ,new VoicePreview("A_M_Y_EPSILON_01_KOREAN_FULL_01","Ambient","M","Young","EPSILON")
            ,new VoicePreview("A_M_Y_EPSILON_01_WHITE_FULL_01","Ambient","M","Young","EPSILON")
            ,new VoicePreview("A_M_Y_EPSILON_02_WHITE_MINI_01","Ambient","M","Young","EPSILON")
            ,new VoicePreview("A_M_Y_GAY_01_BLACK_FULL_01","Ambient","M","Young","GAY")
            ,new VoicePreview("A_M_Y_GAY_01_LATINO_FULL_01","Ambient","M","Young","GAY")
            ,new VoicePreview("A_M_Y_GAY_02_WHITE_MINI_01","Ambient","M","Young","GAY")
            ,new VoicePreview("A_M_Y_GENSTREET_01_CHINESE_FULL_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_01_CHINESE_MINI_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_01_WHITE_FULL_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_01_WHITE_MINI_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_02_BLACK_FULL_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_02_LATINO_FULL_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GENSTREET_02_LATINO_MINI_01","Ambient","M","Young","GENSTREET")
            ,new VoicePreview("A_M_Y_GOLFER_01_WHITE_FULL_01","Ambient","M","Young","GOLFER")
            ,new VoicePreview("A_M_Y_GOLFER_01_WHITE_MINI_01","Ambient","M","Young","GOLFER")
            ,new VoicePreview("A_M_Y_HASJEW_01_WHITE_MINI_01","Ambient","M","Young","HASJEW")
            ,new VoicePreview("A_M_Y_HIPPY_01_WHITE_FULL_01","Ambient","M","Young","HIPPY")
            ,new VoicePreview("A_M_Y_HIPPY_01_WHITE_MINI_01","Ambient","M","Young","HIPPY")
            ,new VoicePreview("A_M_Y_HIPSTER_01_BLACK_FULL_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_01_WHITE_FULL_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_01_WHITE_MINI_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_02_WHITE_FULL_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_02_WHITE_MINI_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_03_WHITE_FULL_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_HIPSTER_03_WHITE_MINI_01","Ambient","M","Young","HIPSTER")
            ,new VoicePreview("A_M_Y_KTOWN_01_KOREAN_FULL_01","Ambient","M","Young","KTOWN")
            ,new VoicePreview("A_M_Y_KTOWN_01_KOREAN_MINI_01","Ambient","M","Young","KTOWN")
            ,new VoicePreview("A_M_Y_KTOWN_02_KOREAN_FULL_01","Ambient","M","Young","KTOWN")
            ,new VoicePreview("A_M_Y_KTOWN_02_KOREAN_MINI_01","Ambient","M","Young","KTOWN")
            ,new VoicePreview("A_M_Y_LATINO_01_LATINO_MINI_01","Ambient","M","Young","LATINO")
            ,new VoicePreview("A_M_Y_LATINO_01_LATINO_MINI_02","Ambient","M","Young","LATINO")
            ,new VoicePreview("A_M_Y_MEXTHUG_01_LATINO_FULL_01","Ambient","M","Young","MEXTHUG")
            ,new VoicePreview("A_M_Y_MUSCLBEAC_01_BLACK_FULL_01","Ambient","M","Young","MUSCLBEAC")
            ,new VoicePreview("A_M_Y_MUSCLBEAC_01_WHITE_FULL_01","Ambient","M","Young","MUSCLBEAC")
            ,new VoicePreview("A_M_Y_MUSCLBEAC_01_WHITE_MINI_01","Ambient","M","Young","MUSCLBEAC")
            ,new VoicePreview("A_M_Y_MUSCLBEAC_02_CHINESE_FULL_01","Ambient","M","Young","MUSCLBEAC")
            ,new VoicePreview("A_M_Y_MUSCLBEAC_02_LATINO_FULL_01","Ambient","M","Young","MUSCLBEAC")
            ,new VoicePreview("A_M_Y_POLYNESIAN_01_POLYNESIAN_FULL_01","Ambient","M","Young","POLYNESIAN")
            ,new VoicePreview("A_M_Y_RACER_01_WHITE_MINI_01","Ambient","M","Young","RACER")
            ,new VoicePreview("A_M_Y_ROLLERCOASTER_01_MINI_01","Ambient","M","Young","ROLLERCOASTER")
            ,new VoicePreview("A_M_Y_ROLLERCOASTER_01_MINI_02","Ambient","M","Young","ROLLERCOASTER")
            ,new VoicePreview("A_M_Y_ROLLERCOASTER_01_MINI_03","Ambient","M","Young","ROLLERCOASTER")
            ,new VoicePreview("A_M_Y_ROLLERCOASTER_01_MINI_04","Ambient","M","Young","ROLLERCOASTER")
            ,new VoicePreview("A_M_Y_RUNNER_01_WHITE_FULL_01","Ambient","M","Young","RUNNER")
            ,new VoicePreview("A_M_Y_RUNNER_01_WHITE_MINI_01","Ambient","M","Young","RUNNER")
            ,new VoicePreview("A_M_Y_SALTON_01_WHITE_MINI_01","Ambient","M","Young","SALTON")
            ,new VoicePreview("A_M_Y_SALTON_01_WHITE_MINI_02","Ambient","M","Young","SALTON")
            ,new VoicePreview("A_M_Y_SKATER_01_WHITE_FULL_01","Ambient","M","Young","SKATER")
            ,new VoicePreview("A_M_Y_SKATER_01_WHITE_MINI_01","Ambient","M","Young","SKATER")
            ,new VoicePreview("A_M_Y_SKATER_02_BLACK_FULL_01","Ambient","M","Young","SKATER")
            ,new VoicePreview("A_M_Y_SOUCENT_01_BLACK_FULL_01","Ambient","M","Young","SOUCENT")
            ,new VoicePreview("A_M_Y_SOUCENT_02_BLACK_FULL_01","Ambient","M","Young","SOUCENT")
            ,new VoicePreview("A_M_Y_SOUCENT_03_BLACK_FULL_01","Ambient","M","Young","SOUCENT")
            ,new VoicePreview("A_M_Y_SOUCENT_04_BLACK_FULL_01","Ambient","M","Young","SOUCENT")
            ,new VoicePreview("A_M_Y_SOUCENT_04_BLACK_MINI_01","Ambient","M","Young","SOUCENT")
            ,new VoicePreview("A_M_Y_STBLA_01_BLACK_FULL_01","Ambient","M","Young","STBLA")
            ,new VoicePreview("A_M_Y_STBLA_02_BLACK_FULL_01","Ambient","M","Young","STBLA")
            ,new VoicePreview("A_M_Y_STLAT_01_LATINO_FULL_01","Ambient","M","Young","STLAT")
            ,new VoicePreview("A_M_Y_STLAT_01_LATINO_MINI_01","Ambient","M","Young","STLAT")
            ,new VoicePreview("A_M_Y_STWHI_01_WHITE_FULL_01","Ambient","M","Young","STWHI")
            ,new VoicePreview("A_M_Y_STWHI_01_WHITE_MINI_01","Ambient","M","Young","STWHI")
            ,new VoicePreview("A_M_Y_STWHI_02_WHITE_FULL_01","Ambient","M","Young","STWHI")
            ,new VoicePreview("A_M_Y_STWHI_02_WHITE_MINI_01","Ambient","M","Young","STWHI")
            ,new VoicePreview("A_M_Y_SUNBATHE_01_BLACK_FULL_01","Ambient","M","Young","SUNBATHE")
            ,new VoicePreview("A_M_Y_SUNBATHE_01_WHITE_FULL_01","Ambient","M","Young","SUNBATHE")
            ,new VoicePreview("A_M_Y_SUNBATHE_01_WHITE_MINI_01","Ambient","M","Young","SUNBATHE")
            ,new VoicePreview("A_M_Y_TRIATHLON_01_MINI_01","Ambient","M","Young","TRIATHLON")
            ,new VoicePreview("A_M_Y_TRIATHLON_01_MINI_02","Ambient","M","Young","TRIATHLON")
            ,new VoicePreview("A_M_Y_TRIATHLON_01_MINI_03","Ambient","M","Young","TRIATHLON")
            ,new VoicePreview("A_M_Y_TRIATHLON_01_MINI_04","Ambient","M","Young","TRIATHLON")
            ,new VoicePreview("A_M_Y_VINEWOOD_01_BLACK_FULL_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_01_BLACK_MINI_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_02_WHITE_FULL_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_02_WHITE_MINI_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_03_LATINO_FULL_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_03_LATINO_MINI_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_03_WHITE_FULL_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_03_WHITE_MINI_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_04_WHITE_FULL_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_M_Y_VINEWOOD_04_WHITE_MINI_01","Ambient","M","Young","VINEWOOD")
            ,new VoicePreview("A_F_M_BEACH_01_WHITE_FULL_01","Ambient","F","Middle","BEACH")
            ,new VoicePreview("A_F_M_BEACH_01_WHITE_MINI_01","Ambient","F","Middle","BEACH")
            ,new VoicePreview("A_F_M_BEVHILLS_01_WHITE_FULL_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_01_WHITE_MINI_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_01_WHITE_MINI_02","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_02_BLACK_FULL_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_02_BLACK_MINI_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_02_WHITE_FULL_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_02_WHITE_FULL_02","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BEVHILLS_02_WHITE_MINI_01","Ambient","F","Middle","BEVHILLS")
            ,new VoicePreview("A_F_M_BODYBUILD_01_BLACK_FULL_01","Ambient","F","Middle","BODYBUILD")
            ,new VoicePreview("A_F_M_BODYBUILD_01_BLACK_MINI_01","Ambient","F","Middle","BODYBUILD")
            ,new VoicePreview("A_F_M_BODYBUILD_01_WHITE_FULL_01","Ambient","F","Middle","BODYBUILD")
            ,new VoicePreview("A_F_M_BODYBUILD_01_WHITE_MINI_01","Ambient","F","Middle","BODYBUILD")
            ,new VoicePreview("A_F_M_BUSINESS_02_WHITE_MINI_01","Ambient","F","Middle","BUSINESS")
            ,new VoicePreview("A_F_M_DOWNTOWN_01_BLACK_FULL_01","Ambient","F","Middle","DOWNTOWN")
            ,new VoicePreview("A_F_M_EASTSA_01_LATINO_FULL_01","Ambient","F","Middle","EASTSA")
            ,new VoicePreview("A_F_M_EASTSA_01_LATINO_MINI_01","Ambient","F","Middle","EASTSA")
            ,new VoicePreview("A_F_M_EASTSA_02_LATINO_FULL_01","Ambient","F","Middle","EASTSA")
            ,new VoicePreview("A_F_M_EASTSA_02_LATINO_MINI_01","Ambient","F","Middle","EASTSA")
            ,new VoicePreview("A_F_M_FATWHITE_01_WHITE_FULL_01","Ambient","F","Middle","FATWHITE")
            ,new VoicePreview("A_F_M_FATWHITE_01_WHITE_MINI_01","Ambient","F","Middle","FATWHITE")
            ,new VoicePreview("A_F_M_KTOWN_01_KOREAN_FULL_01","Ambient","F","Middle","KTOWN")
            ,new VoicePreview("A_F_M_KTOWN_01_KOREAN_MINI_01","Ambient","F","Middle","KTOWN")
            ,new VoicePreview("A_F_M_KTOWN_02_CHINESE_MINI_01","Ambient","F","Middle","KTOWN")
            ,new VoicePreview("A_F_M_KTOWN_02_KOREAN_FULL_01","Ambient","F","Middle","KTOWN")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_FULL_01","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_FULL_02","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_FULL_03","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_MINI_01","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_MINI_02","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SALTON_01_WHITE_MINI_03","Ambient","F","Middle","SALTON")
            ,new VoicePreview("A_F_M_SKIDROW_01_BLACK_FULL_01","Ambient","F","Middle","SKIDROW")
            ,new VoicePreview("A_F_M_SKIDROW_01_BLACK_MINI_01","Ambient","F","Middle","SKIDROW")
            ,new VoicePreview("A_F_M_SKIDROW_01_WHITE_FULL_01","Ambient","F","Middle","SKIDROW")
            ,new VoicePreview("A_F_M_SKIDROW_01_WHITE_MINI_01","Ambient","F","Middle","SKIDROW")
            ,new VoicePreview("A_F_M_SOUCENT_01_BLACK_FULL_01","Ambient","F","Middle","SOUCENT")
            ,new VoicePreview("A_F_M_SOUCENT_02_BLACK_FULL_01","Ambient","F","Middle","SOUCENT")
            ,new VoicePreview("A_F_M_TOURIST_01_WHITE_MINI_01","Ambient","F","Middle","TOURIST")
            ,new VoicePreview("A_F_M_TRAMP_01_WHITE_FULL_01","Ambient","F","Middle","TRAMP")
            ,new VoicePreview("A_F_M_TRAMP_01_WHITE_MINI_01","Ambient","F","Middle","TRAMP")
            ,new VoicePreview("A_F_M_TRAMPBEAC_01_BLACK_FULL_01","Ambient","F","Middle","TRAMPBEAC")
            ,new VoicePreview("A_F_M_TRAMPBEAC_01_BLACK_MINI_01","Ambient","F","Middle","TRAMPBEAC")
            ,new VoicePreview("A_F_M_TRAMPBEAC_01_WHITE_FULL_01","Ambient","F","Middle","TRAMPBEAC")
            ,new VoicePreview("A_F_O_GENSTREET_01_WHITE_MINI_01","Ambient","F","Old","GENSTREET")
            ,new VoicePreview("A_F_O_INDIAN_01_INDIAN_MINI_01","Ambient","F","Old","INDIAN")
            ,new VoicePreview("A_F_O_KTOWN_01_KOREAN_FULL_01","Ambient","F","Old","KTOWN")
            ,new VoicePreview("A_F_O_KTOWN_01_KOREAN_MINI_01","Ambient","F","Old","KTOWN")
            ,new VoicePreview("A_F_O_SALTON_01_WHITE_FULL_01","Ambient","F","Old","SALTON")
            ,new VoicePreview("A_F_O_SALTON_01_WHITE_MINI_01","Ambient","F","Old","SALTON")
            ,new VoicePreview("A_F_O_SOUCENT_01_BLACK_FULL_01","Ambient","F","Old","SOUCENT")
            ,new VoicePreview("A_F_O_SOUCENT_02_BLACK_FULL_01","Ambient","F","Old","SOUCENT")
            ,new VoicePreview("A_F_Y_BEACH_BLACK_FULL_01","Ambient","F","Young","BEACH")
            ,new VoicePreview("A_F_Y_BEACH_01_BLACK_MINI_01","Ambient","F","Young","BEACH")
            ,new VoicePreview("A_F_Y_BEACH_01_WHITE_FULL_01","Ambient","F","Young","BEACH")
            ,new VoicePreview("A_F_Y_BEACH_01_WHITE_MINI_01","Ambient","F","Young","BEACH")
            ,new VoicePreview("A_F_Y_BEVHILLS_01_WHITE_FULL_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_01_WHITE_MINI_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_02_WHITE_FULL_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_02_WHITE_MINI_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_02_WHITE_MINI_02","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_03_WHITE_FULL_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_03_WHITE_MINI_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_04_WHITE_FULL_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BEVHILLS_04_WHITE_MINI_01","Ambient","F","Young","BEVHILLS")
            ,new VoicePreview("A_F_Y_BUSINESS_01_WHITE_FULL_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_01_WHITE_MINI_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_01_WHITE_MINI_02","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_02_WHITE_FULL_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_02_WHITE_MINI_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_03_CHINESE_FULL_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_03_CHINESE_MINI_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_03_LATINO_FULL_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_04_BLACK_FULL_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_04_BLACK_MINI_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_BUSINESS_04_WHITE_MINI_01","Ambient","F","Young","BUSINESS")
            ,new VoicePreview("A_F_Y_EASTSA_01_LATINO_FULL_01","Ambient","F","Young","EASTSA")
            ,new VoicePreview("A_F_Y_EASTSA_01_LATINO_MINI_01","Ambient","F","Young","EASTSA")
            ,new VoicePreview("A_F_Y_EASTSA_02_WHITE_FULL_01","Ambient","F","Young","EASTSA")
            ,new VoicePreview("A_F_Y_EASTSA_03_LATINO_FULL_01","Ambient","F","Young","EASTSA")
            ,new VoicePreview("A_F_Y_EASTSA_03_LATINO_MINI_01","Ambient","F","Young","EASTSA")
            ,new VoicePreview("A_F_Y_EPSILON_01_WHITE_MINI_01","Ambient","F","Young","EPSILON")
            ,new VoicePreview("A_F_Y_FITNESS_01_WHITE_FULL_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_FITNESS_01_WHITE_MINI_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_FITNESS_02_BLACK_FULL_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_FITNESS_02_BLACK_MINI_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_FITNESS_02_WHITE_FULL_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_FITNESS_02_WHITE_MINI_01","Ambient","F","Young","FITNESS")
            ,new VoicePreview("A_F_Y_GOLFER_01_WHITE_FULL_01","Ambient","F","Young","GOLFER")
            ,new VoicePreview("A_F_Y_GOLFER_01_WHITE_MINI_01","Ambient","F","Young","GOLFER")
            ,new VoicePreview("A_F_Y_HIKER_01_WHITE_FULL_01","Ambient","F","Young","HIKER")
            ,new VoicePreview("A_F_Y_HIKER_01_WHITE_MINI_01","Ambient","F","Young","HIKER")
            ,new VoicePreview("A_F_Y_HIPSTER_01_WHITE_FULL_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_01_WHITE_MINI_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_02_WHITE_FULL_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_02_WHITE_MINI_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_02_WHITE_MINI_02","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_03_WHITE_FULL_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_03_WHITE_MINI_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_03_WHITE_MINI_02","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_04_WHITE_FULL_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_04_WHITE_MINI_01","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_HIPSTER_04_WHITE_MINI_02","Ambient","F","Young","HIPSTER")
            ,new VoicePreview("A_F_Y_INDIAN_01_INDIAN_MINI_01","Ambient","F","Young","INDIAN")
            ,new VoicePreview("A_F_Y_INDIAN_01_INDIAN_MINI_02","Ambient","F","Young","INDIAN")
            ,new VoicePreview("A_F_Y_ROLLERCOASTER_01_MINI_01","Ambient","F","Young","ROLLERCOASTER")
            ,new VoicePreview("A_F_Y_ROLLERCOASTER_01_MINI_02","Ambient","F","Young","ROLLERCOASTER")
            ,new VoicePreview("A_F_Y_ROLLERCOASTER_01_MINI_03","Ambient","F","Young","ROLLERCOASTER")
            ,new VoicePreview("A_F_Y_ROLLERCOASTER_01_MINI_04","Ambient","F","Young","ROLLERCOASTER")
            ,new VoicePreview("A_F_Y_SKATER_01_WHITE_FULL_01","Ambient","F","Young","SKATER")
            ,new VoicePreview("A_F_Y_SKATER_01_WHITE_MINI_01","Ambient","F","Young","SKATER")
            ,new VoicePreview("A_F_Y_SOUCENT_01_BLACK_FULL_01","Ambient","F","Young","SOUCENT")
            ,new VoicePreview("A_F_Y_SOUCENT_02_BLACK_FULL_01","Ambient","F","Young","SOUCENT")
            ,new VoicePreview("A_F_Y_SOUCENT_03_LATINO_FULL_01","Ambient","F","Young","SOUCENT")
            ,new VoicePreview("A_F_Y_SOUCENT_03_LATINO_MINI_01","Ambient","F","Young","SOUCENT")
            ,new VoicePreview("A_F_Y_TENNIS_01_BLACK_MINI_01","Ambient","F","Young","TENNIS")
            ,new VoicePreview("A_F_Y_TENNIS_01_WHITE_MINI_01","Ambient","F","Young","TENNIS")
            ,new VoicePreview("A_F_Y_TOURIST_01_BLACK_FULL_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_01_BLACK_MINI_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_01_LATINO_FULL_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_01_LATINO_MINI_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_01_WHITE_FULL_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_01_WHITE_MINI_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_TOURIST_02_WHITE_MINI_01","Ambient","F","Young","TOURIST")
            ,new VoicePreview("A_F_Y_VINEWOOD_01_WHITE_FULL_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_01_WHITE_MINI_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_02_WHITE_FULL_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_02_WHITE_MINI_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_03_CHINESE_FULL_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_03_CHINESE_MINI_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_04_WHITE_FULL_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_04_WHITE_MINI_01","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("A_F_Y_VINEWOOD_04_WHITE_MINI_02","Ambient","F","Young","VINEWOOD")
            ,new VoicePreview("G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_01","Gang","M","Middle","ARMBOSS")
            ,new VoicePreview("G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_02","Gang","M","Middle","ARMBOSS")
            ,new VoicePreview("G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_01","Gang","M","Middle","ARMLIEUT")
            ,new VoicePreview("G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_02","Gang","M","Middle","ARMLIEUT")
            ,new VoicePreview("G_M_M_CHIBOSS_01_CHINESE_MINI_01","Gang","M","Middle","CHIBOSS")
            ,new VoicePreview("G_M_M_CHIBOSS_01_CHINESE_MINI_02","Gang","M","Middle","CHIBOSS")
            ,new VoicePreview("G_M_M_CHIGOON_01_CHINESE_MINI_01","Gang","M","Middle","CHIGOON")
            ,new VoicePreview("G_M_M_CHIGOON_01_CHINESE_MINI_02","Gang","M","Middle","CHIGOON")
            ,new VoicePreview("G_M_M_CHIGOON_02_CHINESE_MINI_01","Gang","M","Middle","CHIGOON")
            ,new VoicePreview("G_M_M_CHIGOON_02_CHINESE_MINI_02","Gang","M","Middle","CHIGOON")
            ,new VoicePreview("G_M_M_KORBOSS_01_KOREAN_MINI_01","Gang","M","Middle","KORBOSS")
            ,new VoicePreview("G_M_M_KORBOSS_01_KOREAN_MINI_02","Gang","M","Middle","KORBOSS")
            ,new VoicePreview("G_M_M_MEXBOSS_01_LATINO_MINI_01","Gang","M","Middle","MEXBOSS")
            ,new VoicePreview("G_M_M_MEXBOSS_01_LATINO_MINI_02","Gang","M","Middle","MEXBOSS")
            ,new VoicePreview("G_M_M_MEXBOSS_02_LATINO_MINI_01","Gang","M","Middle","MEXBOSS")
            ,new VoicePreview("G_M_M_MEXBOSS_02_LATINO_MINI_02","Gang","M","Middle","MEXBOSS")
            ,new VoicePreview("G_M_Y_ARMGOON_02_WHITE_ARMENIAN_MINI_01","Gang","M","Young","ARMGOON")
            ,new VoicePreview("G_M_Y_ARMGOON_02_WHITE_ARMENIAN_MINI_02","Gang","M","Young","ARMGOON")
            ,new VoicePreview("G_M_Y_BALLAEAST_01_BLACK_FULL_01","Gang","M","Young","BALLAEAST")
            ,new VoicePreview("G_M_Y_BALLAEAST_01_BLACK_FULL_02","Gang","M","Young","BALLAEAST")
            ,new VoicePreview("G_M_Y_BALLAEAST_01_BLACK_MINI_01","Gang","M","Young","BALLAEAST")
            ,new VoicePreview("G_M_Y_BALLAEAST_01_BLACK_MINI_02","Gang","M","Young","BALLAEAST")
            ,new VoicePreview("G_M_Y_BALLAEAST_01_BLACK_MINI_03","Gang","M","Young","BALLAEAST")
            ,new VoicePreview("G_M_Y_BALLAORIG_01_BLACK_FULL_01","Gang","M","Young","BALLAORIG")
            ,new VoicePreview("G_M_Y_BALLAORIG_01_BLACK_FULL_02","Gang","M","Young","BALLAORIG")
            ,new VoicePreview("G_M_Y_BALLAORIG_01_BLACK_MINI_01","Gang","M","Young","BALLAORIG")
            ,new VoicePreview("G_M_Y_BALLAORIG_01_BLACK_MINI_02","Gang","M","Young","BALLAORIG")
            ,new VoicePreview("G_M_Y_BALLAORIG_01_BLACK_MINI_03","Gang","M","Young","BALLAORIG")
            ,new VoicePreview("G_M_Y_BALLASOUT_01_BLACK_FULL_01","Gang","M","Young","BALLASOUT")
            ,new VoicePreview("G_M_Y_BALLASOUT_01_BLACK_FULL_02","Gang","M","Young","BALLASOUT")
            ,new VoicePreview("G_M_Y_BALLASOUT_01_BLACK_MINI_01","Gang","M","Young","BALLASOUT")
            ,new VoicePreview("G_M_Y_BALLASOUT_01_BLACK_MINI_02","Gang","M","Young","BALLASOUT")
            ,new VoicePreview("G_M_Y_BALLASOUT_01_BLACK_MINI_03","Gang","M","Young","BALLASOUT")
            ,new VoicePreview("G_M_Y_FAMCA_01_BLACK_FULL_01","Gang","M","Young","FAMCA")
            ,new VoicePreview("G_M_Y_FAMCA_01_BLACK_FULL_02","Gang","M","Young","FAMCA")
            ,new VoicePreview("G_M_Y_FAMCA_01_BLACK_MINI_01","Gang","M","Young","FAMCA")
            ,new VoicePreview("G_M_Y_FAMCA_01_BLACK_MINI_02","Gang","M","Young","FAMCA")
            ,new VoicePreview("G_M_Y_FAMCA_01_BLACK_MINI_03","Gang","M","Young","FAMCA")
            ,new VoicePreview("G_M_Y_FAMDNF_01_BLACK_FULL_01","Gang","M","Young","FAMDNF")
            ,new VoicePreview("G_M_Y_FAMDNF_01_BLACK_FULL_02","Gang","M","Young","FAMDNF")
            ,new VoicePreview("G_M_Y_FAMDNF_01_BLACK_MINI_01","Gang","M","Young","FAMDNF")
            ,new VoicePreview("G_M_Y_FAMDNF_01_BLACK_MINI_02","Gang","M","Young","FAMDNF")
            ,new VoicePreview("G_M_Y_FAMDNF_01_BLACK_MINI_03","Gang","M","Young","FAMDNF")
            ,new VoicePreview("G_M_Y_FAMFOR_01_BLACK_FULL_01","Gang","M","Young","FAMFOR")
            ,new VoicePreview("G_M_Y_FAMFOR_01_BLACK_FULL_02","Gang","M","Young","FAMFOR")
            ,new VoicePreview("G_M_Y_FAMFOR_01_BLACK_MINI_01","Gang","M","Young","FAMFOR")
            ,new VoicePreview("G_M_Y_FAMFOR_01_BLACK_MINI_02","Gang","M","Young","FAMFOR")
            ,new VoicePreview("G_M_Y_FAMFOR_01_BLACK_MINI_03","Gang","M","Young","FAMFOR")
            ,new VoicePreview("G_M_Y_KOREAN_01_KOREAN_MINI_01","Gang","M","Young","KOREAN")
            ,new VoicePreview("G_M_Y_KOREAN_01_KOREAN_MINI_02","Gang","M","Young","KOREAN")
            ,new VoicePreview("G_M_Y_KOREAN_02_KOREAN_MINI_01","Gang","M","Young","KOREAN")
            ,new VoicePreview("G_M_Y_KOREAN_02_KOREAN_MINI_02","Gang","M","Young","KOREAN")
            ,new VoicePreview("G_M_Y_KORLIEUT_01_KOREAN_MINI_01","Gang","M","Young","KORLIEUT")
            ,new VoicePreview("G_M_Y_KORLIEUT_01_KOREAN_MINI_02","Gang","M","Young","KORLIEUT")
            ,new VoicePreview("G_M_Y_LOST_01_BLACK_FULL_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_BLACK_FULL_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_BLACK_MINI_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_BLACK_MINI_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_BLACK_MINI_03","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_WHITE_FULL_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_WHITE_MINI_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_01_WHITE_MINI_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_LATINO_FULL_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_LATINO_FULL_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_LATINO_MINI_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_LATINO_MINI_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_LATINO_MINI_03","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_WHITE_FULL_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_WHITE_MINI_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_02_WHITE_MINI_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_03_WHITE_FULL_01","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_03_WHITE_MINI_02","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_LOST_03_WHITE_MINI_03","Gang","M","Young","LOST")
            ,new VoicePreview("G_M_Y_MEXGOON_01_LATINO_MINI_01","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_MEXGOON_01_LATINO_MINI_02","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_MEXGOON_02_LATINO_MINI_01","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_MEXGOON_02_LATINO_MINI_02","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_MEXGOON_03_LATINO_MINI_01","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_MEXGOON_03_LATINO_MINI_02","Gang","M","Young","MEXGOON")
            ,new VoicePreview("G_M_Y_POLOGOON_01_LATINO_MINI_01","Gang","M","Young","POLOGOON")
            ,new VoicePreview("G_M_Y_POLOGOON_01_LATINO_MINI_02","Gang","M","Young","POLOGOON")
            ,new VoicePreview("G_M_Y_SALVABOSS_01_SALVADORIAN_MINI_01","Gang","M","Young","SALVABOSS")
            ,new VoicePreview("G_M_Y_SALVABOSS_01_SALVADORIAN_MINI_02","Gang","M","Young","SALVABOSS")
            ,new VoicePreview("G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_01","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_02","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_03","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_01","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_02","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_03","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_01","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_02","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_03","Gang","M","Young","SALVAGOON")
            ,new VoicePreview("G_M_Y_STREETPUNK_01_BLACK_MINI_01","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK_01_BLACK_MINI_02","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK_01_BLACK_MINI_03","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK_02_BLACK_MINI_01","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK_02_BLACK_MINI_02","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK_02_BLACK_MINI_03","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_M_Y_STREETPUNK02_BLACK_MINI_04","Gang","M","Young","STREETPUNK")
            ,new VoicePreview("G_F_Y_BALLAS_01_BLACK_MINI_01","Gang","F","Young","BALLAS")
            ,new VoicePreview("G_F_Y_BALLAS_01_BLACK_MINI_02","Gang","F","Young","BALLAS")
            ,new VoicePreview("G_F_Y_BALLAS_01_BLACK_MINI_03","Gang","F","Young","BALLAS")
            ,new VoicePreview("G_F_Y_BALLAS_01_BLACK_MINI_04","Gang","F","Young","BALLAS")
            ,new VoicePreview("G_F_Y_FAMILIES_01_BLACK_MINI_01","Gang","F","Young","FAMILIES")
            ,new VoicePreview("G_F_Y_FAMILIES_01_BLACK_MINI_02","Gang","F","Young","FAMILIES")
            ,new VoicePreview("G_F_Y_FAMILIES_01_BLACK_MINI_03","Gang","F","Young","FAMILIES")
            ,new VoicePreview("G_F_Y_FAMILIES_01_BLACK_MINI_04","Gang","F","Young","FAMILIES")
            ,new VoicePreview("G_F_Y_FAMILIES_01_BLACK_MINI_05","Gang","F","Young","FAMILIES")
            ,new VoicePreview("G_F_Y_VAGOS_01_LATINO_MINI_01","Gang","F","Young","VAGOS")
            ,new VoicePreview("G_F_Y_VAGOS_01_LATINO_MINI_02","Gang","F","Young","VAGOS")
            ,new VoicePreview("G_F_Y_VAGOS_01_LATINO_MINI_03","Gang","F","Young","VAGOS")
            ,new VoicePreview("G_F_Y_VAGOS_01_LATINO_MINI_04","Gang","F","Young","VAGOS")
            ,new VoicePreview("S_M_M_AMMUCOUNTRY_01_WHITE_01","Service","M","Middle","AMMUCOUNTRY")
            ,new VoicePreview("S_M_M_AMMUCOUNTRY_WHITE_MINI_01","Service","M","Middle","AMMUCOUNTRY")
            ,new VoicePreview("S_M_M_AUTOSHOP_01_WHITE_01","Service","M","Middle","AUTOSHOP")
            ,new VoicePreview("S_M_M_BOUNCER_01_BLACK_FULL_01","Service","M","Middle","BOUNCER")
            ,new VoicePreview("S_M_M_BOUNCER_01_LATINO_FULL_01","Service","M","Middle","BOUNCER")
            ,new VoicePreview("S_M_M_CIASEC_01_BLACK_MINI_01","Service","M","Middle","CIASEC")
            ,new VoicePreview("S_M_M_CIASEC_01_BLACK_MINI_02","Service","M","Middle","CIASEC")
            ,new VoicePreview("S_M_M_CIASEC_01_WHITE_MINI_01","Service","M","Middle","CIASEC")
            ,new VoicePreview("S_M_M_CIASEC_01_WHITE_MINI_02","Service","M","Middle","CIASEC")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_BLACK_MINI_01","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_BLACK_MINI_02","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_LATINO_MINI_01","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_LATINO_MINI_02","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_WHITE_MINI_01","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_FIBOFFICE_01_WHITE_MINI_02","Service","M","Middle","FIBOFFICE")
            ,new VoicePreview("S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_01","Service","M","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_02","Service","M","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_03","Service","M","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_04","Service","M","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_M_GENERICMARINE_01_LATINO_MINI_01","Service","M","Middle","GENERICMARINE")
            ,new VoicePreview("S_M_M_GENERICMARINE_01_LATINO_MINI_02","Service","M","Middle","GENERICMARINE")
            ,new VoicePreview("S_M_M_GENERICMECHANIC_01_BLACK_MINI_01","Service","M","Middle","GENERICMECHANIC")
            ,new VoicePreview("S_M_M_GENERICMECHANIC_01_BLACK_MINI_02","Service","M","Middle","GENERICMECHANIC")
            ,new VoicePreview("S_M_M_GENERICPOSTWORKER_01_BLACK_MINI_01","Service","M","Middle","GENERICPOSTWORKER")
            ,new VoicePreview("S_M_M_GENERICPOSTWORKER_01_BLACK_MINI_02","Service","M","Middle","GENERICPOSTWORKER")
            ,new VoicePreview("S_M_M_GENERICPOSTWORKER_01_WHITE_MINI_01","Service","M","Middle","GENERICPOSTWORKER")
            ,new VoicePreview("S_M_M_GENERICPOSTWORKER_01_WHITE_MINI_02","Service","M","Middle","GENERICPOSTWORKER")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_BLACK_MINI_01","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_BLACK_MINI_02","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_BLACK_MINI_03","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_LATINO_MINI_01","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_LATINO_MINI_02","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_WHITE_MINI_01","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_WHITE_MINI_02","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_GENERICSECURITY_01_WHITE_MINI_03","Service","M","Middle","GENERICSECURITY")
            ,new VoicePreview("S_M_M_HAIRDRESS_01_BLACK_01","Service","M","Middle","HAIRDRESS")
            ,new VoicePreview("S_M_M_HAIRDRESSER_01_BLACK_MINI_01","Service","M","Middle","HAIRDRESSER")
            ,new VoicePreview("S_M_M_PARAMEDIC_01_BLACK_MINI_01","Service","M","Middle","PARAMEDIC")
            ,new VoicePreview("S_M_M_PARAMEDIC_01_LATINO_MINI_01","Service","M","Middle","PARAMEDIC")
            ,new VoicePreview("S_M_M_PARAMEDIC_01_WHITE_MINI_01","Service","M","Middle","PARAMEDIC")
            ,new VoicePreview("S_M_M_PILOT_01_BLACK_FULL_01","Service","M","Middle","PILOT")
            ,new VoicePreview("S_M_M_PILOT_01_BLACK_FULL_02","Service","M","Middle","PILOT")
            ,new VoicePreview("S_M_M_PILOT_01_WHITE_FULL_01","Service","M","Middle","PILOT")
            ,new VoicePreview("S_M_M_PILOT_01_WHITE_FULL_02","Service","M","Middle","PILOT")
            ,new VoicePreview("S_M_M_TRUCKER_01_BLACK_FULL_01","Service","M","Middle","TRUCKER")
            ,new VoicePreview("S_M_M_TRUCKER_01_WHITE_FULL_01","Service","M","Middle","TRUCKER")
            ,new VoicePreview("S_M_M_TRUCKER_01_WHITE_FULL_02","Service","M","Middle","TRUCKER")
            ,new VoicePreview("S_M_Y_AIRWORKER_BLACK_FULL_01","Service","M","Young","AIRWORKER")
            ,new VoicePreview("S_M_Y_AIRWORKER_BLACK_FULL_02","Service","M","Young","AIRWORKER")
            ,new VoicePreview("S_M_Y_AIRWORKER_LATINO_FULL_01","Service","M","Young","AIRWORKER")
            ,new VoicePreview("S_M_Y_AIRWORKER_LATINO_FULL_02","Service","M","Young","AIRWORKER")
            ,new VoicePreview("S_M_Y_AMMUCITY_01_WHITE_01","Service","M","Young","AMMUCITY")
            ,new VoicePreview("S_M_Y_AMMUCITY_01_WHITE_MINI_01","Service","M","Young","AMMUCITY")
            ,new VoicePreview("S_M_Y_BAYWATCH_01_BLACK_FULL_01","Service","M","Young","BAYWATCH")
            ,new VoicePreview("S_M_Y_BAYWATCH_01_BLACK_FULL_02","Service","M","Young","BAYWATCH")
            ,new VoicePreview("S_M_Y_BAYWATCH_01_WHITE_FULL_01","Service","M","Young","BAYWATCH")
            ,new VoicePreview("S_M_Y_BAYWATCH_01_WHITE_FULL_02","Service","M","Young","BAYWATCH")
            ,new VoicePreview("S_M_Y_BLACKOPS_01_BLACK_MINI_01","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_01_BLACK_MINI_02","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_01_WHITE_MINI_01","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_01_WHITE_MINI_02","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_02_LATINO_MINI_01","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_02_LATINO_MINI_02","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_BLACKOPS_02_WHITE_MINI_01","Service","M","Young","BLACKOPS")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_FULL_01","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_FULL_02","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_MINI_01","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_MINI_02","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_MINI_03","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_BLACK_MINI_04","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_FULL_01","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_FULL_02","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_MINI_01","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_MINI_02","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_MINI_03","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_COP_01_WHITE_MINI_04","Service","M","Young","COP")
            ,new VoicePreview("S_M_Y_FIREMAN_01_LATINO_FULL_01","Service","M","Young","FIREMAN")
            ,new VoicePreview("S_M_Y_FIREMAN_01_LATINO_FULL_02","Service","M","Young","FIREMAN")
            ,new VoicePreview("S_M_Y_FIREMAN_01_WHITE_FULL_01","Service","M","Young","FIREMAN")
            ,new VoicePreview("S_M_Y_FIREMAN_01_WHITE_FULL_02","Service","M","Young","FIREMAN")
            ,new VoicePreview("S_M_Y_GENERICCHEAPWORKER_01_BLACK_MINI_01","Service","M","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_Y_GENERICCHEAPWORKER_01_BLACK_MINI_02","Service","M","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_Y_GENERICCHEAPWORKER_01_WHITE_MINI_01","Service","M","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_M_Y_GENERICMARINE_01_BLACK_MINI_01","Service","M","Young","GENERICMARINE")
            ,new VoicePreview("S_M_Y_GENERICMARINE_01_BLACK_MINI_02","Service","M","Young","GENERICMARINE")
            ,new VoicePreview("S_M_Y_GENERICMARINE_01_WHITE_MINI_01","Service","M","Young","GENERICMARINE")
            ,new VoicePreview("S_M_Y_GENERICMARINE_01_WHITE_MINI_02","Service","M","Young","GENERICMARINE")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_BLACK_MINI_01","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_BLACK_MINI_02","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_LATINO_MINI_01","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_LATINO_MINI_02","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_WHITE_MINI_01","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_GENERICWORKER_01_WHITE_MINI_02","Service","M","Young","GENERICWORKER")
            ,new VoicePreview("S_M_Y_HWAYCOP_01_BLACK_FULL_01","Service","M","Young","HWAYCOP")
            ,new VoicePreview("S_M_Y_HWAYCOP_01_BLACK_FULL_02","Service","M","Young","HWAYCOP")
            ,new VoicePreview("S_M_Y_HWAYCOP_01_WHITE_FULL_01","Service","M","Young","HWAYCOP")
            ,new VoicePreview("S_M_Y_HWAYCOP_01_WHITE_FULL_02","Service","M","Young","HWAYCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_01","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_02","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_03","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_04","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_05","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_MCOP_01_WHITE_MINI_06","Service","M","Young","MCOP")
            ,new VoicePreview("S_M_Y_RANGER_01_LATINO_FULL_01","Service","M","Young","RANGER")
            ,new VoicePreview("S_M_Y_RANGER_01_WHITE_FULL_01","Service","M","Young","RANGER")
            ,new VoicePreview("S_M_Y_SHERIFF_01_WHITE_FULL_01","Service","M","Young","SHERIFF")
            ,new VoicePreview("S_M_Y_SHERIFF_01_WHITE_FULL_02","Service","M","Young","SHERIFF")
            ,new VoicePreview("S_M_Y_SHOP_MASK_WHITE_MINI_01","Service","M","Young","SHOPMASK")
            ,new VoicePreview("S_M_Y_SWAT_01_WHITE_FULL_01","Service","M","Young","SWAT")
            ,new VoicePreview("S_M_Y_SWAT_01_WHITE_FULL_02","Service","M","Young","SWAT")
            ,new VoicePreview("S_M_Y_SWAT_01_WHITE_FULL_03","Service","M","Young","SWAT")
            ,new VoicePreview("S_M_Y_SWAT_01_WHITE_FULL_04","Service","M","Young","SWAT")
            ,new VoicePreview("S_F_M_FEMBARBER_BLACK_MINI_01","Service","F","Middle","FEMBARBER")
            ,new VoicePreview("S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_01","Service","F","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_02","Service","F","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_03","Service","F","Middle","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_M_PONSEN_01_BLACK_01","Service","F","Middle","PONSEN")
            ,new VoicePreview("S_F_M_SHOP_HIGH_WHITE_MINI_01","Service","F","Middle","SHOP")
            ,new VoicePreview("S_F_Y_AIRHOSTESS_01_BLACK_FULL_01","Service","F","Young","AIRHOSTESS")
            ,new VoicePreview("S_F_Y_AIRHOSTESS_01_BLACK_FULL_02","Service","F","Young","AIRHOSTESS")
            ,new VoicePreview("S_F_Y_AIRHOSTESS_01_WHITE_FULL_01","Service","F","Young","AIRHOSTESS")
            ,new VoicePreview("S_F_Y_AIRHOSTESS_01_WHITE_FULL_02","Service","F","Young","AIRHOSTESS")
            ,new VoicePreview("S_F_Y_BAYWATCH_01_BLACK_FULL_01","Service","F","Young","BAYWATCH")
            ,new VoicePreview("S_F_Y_BAYWATCH_01_BLACK_FULL_02","Service","F","Young","BAYWATCH")
            ,new VoicePreview("S_F_Y_BAYWATCH_01_WHITE_FULL_01","Service","F","Young","BAYWATCH")
            ,new VoicePreview("S_F_Y_BAYWATCH_01_WHITE_FULL_02","Service","F","Young","BAYWATCH")
            ,new VoicePreview("S_F_Y_COP_01_BLACK_FULL_01","Service","F","Young","COP")
            ,new VoicePreview("S_F_Y_COP_01_BLACK_FULL_02","Service","F","Young","COP")
            ,new VoicePreview("S_F_Y_COP_01_WHITE_FULL_01","Service","F","Young","COP")
            ,new VoicePreview("S_F_Y_COP_01_WHITE_FULL_02","Service","F","Young","COP")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_BLACK_MINI_01","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_BLACK_MINI_02","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_01","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_02","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_03","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_04","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_WHITE_MINI_01","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_GENERICCHEAPWORKER_01_WHITE_MINI_02","Service","F","Young","GENERICCHEAPWORKER")
            ,new VoicePreview("S_F_Y_HOOKER_01_WHITE_FULL_01","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_01_WHITE_FULL_02","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_01_WHITE_FULL_03","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_02_WHITE_FULL_01","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_02_WHITE_FULL_02","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_02_WHITE_FULL_03","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_03_BLACK_FULL_01","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_HOOKER_03_BLACK_FULL_03","Service","F","Young","HOOKER")
            ,new VoicePreview("S_F_Y_PECKER_01_WHITE_01","Service","F","Young","PECKER")
            ,new VoicePreview("S_F_Y_RANGER_01_WHITE_MINI_01","Service","F","Young","RANGER")
            ,new VoicePreview("S_F_Y_SHOP_LOW_WHITE_MINI_01","Service","F","Young","SHOP")
            ,new VoicePreview("S_F_Y_SHOP_MID_WHITE_MINI_01","Service","F","Young","SHOP")
            ,new VoicePreview("ABIGAIL","Unknown","","U","")
            ,new VoicePreview("AIRCRAFT_WARNING_FEMALE_01","Unknown","WARNING","U","01,")
            ,new VoicePreview("AIRCRAFT_WARNING_MALE_01","Unknown","WARNING","U","01,")
            ,new VoicePreview("AIRDUMMER","Unknown","","U","")
            ,new VoicePreview("AIRGUITARIST","Unknown","","U","")
            ,new VoicePreview("AIRPIANIST","Unknown","","U","")
            ,new VoicePreview("ALIENS","Unknown","","U","")
            ,new VoicePreview("AMANDA_NORMAL","Unknown","NORMAL,","U","")
            ,new VoicePreview("AMANDA_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("AMMUCITY","Unknown","","U","")
            ,new VoicePreview("ANDY_MOON","Unknown","MOON,","U","")
            ,new VoicePreview("ANTON","Unknown","","U","")
            ,new VoicePreview("AVI","Unknown","","U","")
            ,new VoicePreview("BAILBOND1JUMPER","Unknown","","U","")
            ,new VoicePreview("BAILBOND2JUMPER","Unknown","","U","")
            ,new VoicePreview("BAILBOND3JUMPER","Unknown","","U","")
            ,new VoicePreview("BAILBOND4JUMPER","Unknown","","U","")
            ,new VoicePreview("BALLASOG","Unknown","","U","")
            ,new VoicePreview("BANKWM1","Unknown","","U","")
            ,new VoicePreview("BANKWM2","Unknown","","U","")
            ,new VoicePreview("BARRY","Unknown","","U","")
            ,new VoicePreview("BAYGOR","Unknown","","U","")
            ,new VoicePreview("BENNY","Unknown","","U","")
            ,new VoicePreview("BEVERLY","Unknown","","U","")
            ,new VoicePreview("BILLBINDER","Unknown","","U","")
            ,new VoicePreview("BJPILOT_TRAIN","Unknown","TRAIN,","U","")
            ,new VoicePreview("BJPILOT_CANAL","Unknown","CANAL,","U","")
            ,new VoicePreview("BRAD","Unknown","","U","")
            ,new VoicePreview("BTL_YOHAN","Unknown","YOHAN,","U","")
            ,new VoicePreview("BTL_TONY","Unknown","TONY,","U","")
            ,new VoicePreview("BTL_TFRIEND","Unknown","TFRIEND,","U","")
            ,new VoicePreview("BTL_SOLOMUN","Unknown","SOLOMUN,","U","")
            ,new VoicePreview("BTL_POLICE1","Unknown","POLICE1,","U","")
            ,new VoicePreview("BTL_MFRIEND","Unknown","MFRIEND,","U","")
            ,new VoicePreview("BTL_MATEO","Unknown","MATEO,","U","")
            ,new VoicePreview("BTL_MARCEL","Unknown","MARCEL,","U","")
            ,new VoicePreview("BTL_LAZLOW","Unknown","LAZLOW,","U","")
            ,new VoicePreview("BTL_DIXON","Unknown","DIXON,","U","")
            ,new VoicePreview("BTL_DAVE","Unknown","DAVE,","U","")
            ,new VoicePreview("BTL_CONNIE","Unknown","CONNIE,","U","")
            ,new VoicePreview("BTL_CARMINE","Unknown","CARMINE,","U","")
            ,new VoicePreview("BTL_BLMADONNA","Unknown","BLMADONNA,","U","")
            ,new VoicePreview("BUSINESSMAN","Unknown","","U","")
            ,new VoicePreview("CASEY","Unknown","","U","")
            ,new VoicePreview("CHASTITY","Unknown","","U","")
            ,new VoicePreview("CHEETAH","Unknown","","U","")
            ,new VoicePreview("CHEF","Unknown","","U","")
            ,new VoicePreview("CHENG","Unknown","","U","")
            ,new VoicePreview("CLETUS","Unknown","","U","")
            ,new VoicePreview("CLINTON","Unknown","","U","")
            ,new VoicePreview("CLOWNS","Unknown","","U","")
            ,new VoicePreview("COOK","Unknown","","U","")
            ,new VoicePreview("CST4ACTRESS","Unknown","","U","")
            ,new VoicePreview("DARYL","Unknown","","U","")
            ,new VoicePreview("DAVE","Unknown","","U","")
            ,new VoicePreview("DENISE","Unknown","","U","")
            ,new VoicePreview("DOM","Unknown","","U","")
            ,new VoicePreview("EDDIE","Unknown","","U","")
            ,new VoicePreview("EXECPA_MALE","Unknown","MALE,","U","")
            ,new VoicePreview("EXECPA_FEMALE","Unknown","FEMALE,","U","")
            ,new VoicePreview("EXT1HELIPILOT","Unknown","","U","")
            ,new VoicePreview("FLOYD","Unknown","","U","")
            ,new VoicePreview("FRANKLIN_NORMAL","Unknown","NORMAL,","U","")
            ,new VoicePreview("FRANKLIN_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("FRANKLIN_ANGRY","Unknown","ANGRY,","U","")
            ,new VoicePreview("FUFU","Unknown","","U","")
            ,new VoicePreview("GARDENER","Unknown","","U","")
            ,new VoicePreview("GAYMILITARY","Unknown","","U","")
            ,new VoicePreview("GERALD","Unknown","","U","")
            ,new VoicePreview("GIRL1","Unknown","","U","")
            ,new VoicePreview("GIRL2","Unknown","","U","")
            ,new VoicePreview("GRIFF","Unknown","","U","")
            ,new VoicePreview("GROOM","Unknown","","U","")
            ,new VoicePreview("GUSTAVO","Unknown","","U","")
            ,new VoicePreview("HAO","Unknown","","U","")
            ,new VoicePreview("HEISTMANAGER","Unknown","","U","")
            ,new VoicePreview("HOSTAGEBF1","Unknown","","U","")
            ,new VoicePreview("HOSTAGEBM1","Unknown","","U","")
            ,new VoicePreview("HOSTAGEWF1","Unknown","","U","")
            ,new VoicePreview("HOSTAGEWF2","Unknown","","U","")
            ,new VoicePreview("HOSTAGEWM1","Unknown","","U","")
            ,new VoicePreview("HOSTAGEWM2","Unknown","","U","")
            ,new VoicePreview("HUGH","Unknown","","U","")
            ,new VoicePreview("IMPOTENT_RAGE","Unknown","RAGE,","U","")
            ,new VoicePreview("INFERNUS","Unknown","","U","")
            ,new VoicePreview("JANE","Unknown","","U","")
            ,new VoicePreview("JANET","Unknown","","U","")
            ,new VoicePreview("JEROME","Unknown","","U","")
            ,new VoicePreview("JESSE","Unknown","","U","")
            ,new VoicePreview("JIMMY_NORMAL","Unknown","NORMAL,","U","")
            ,new VoicePreview("JIMMY_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("JIMMYBOSTON","Unknown","","U","")
            ,new VoicePreview("JOE","Unknown","","U","")
            ,new VoicePreview("JOSEF","Unknown","","U","")
            ,new VoicePreview("JOSH","Unknown","","U","")
            ,new VoicePreview("JULIET","Unknown","","U","")
            ,new VoicePreview("KAREN","Unknown","","U","")
            ,new VoicePreview("KARIM","Unknown","","U","")
            ,new VoicePreview("KARL","Unknown","","U","")
            ,new VoicePreview("KERRY","Unknown","","U","")
            ,new VoicePreview("KIDNAPPEDFEMALE","Unknown","","U","")
            ,new VoicePreview("LACEY","Unknown","","U","")
            ,new VoicePreview("LAMAR_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("LAMAR_2_NORMAL","Unknown","2","U","")
            ,new VoicePreview("LAMAR_1_NORMAL","Unknown","1","U","")
            ,new VoicePreview("LESTER","Unknown","","U","")
            ,new VoicePreview("LIENGINEER","Unknown","","U","")
            ,new VoicePreview("LIENGINEER2","Unknown","","U","")
            ,new VoicePreview("LOSTKIDNAPGIRL","Unknown","","U","")
            ,new VoicePreview("MAID","Unknown","","U","")
            ,new VoicePreview("MALE_STRIP_DJ_WHITE","Unknown","STRIP","U","WHITE,")
            ,new VoicePreview("MANI","Unknown","","U","")
            ,new VoicePreview("MARNIE","Unknown","","U","")
            ,new VoicePreview("MARYANN","Unknown","","U","")
            ,new VoicePreview("MAUDE","Unknown","","U","")
            ,new VoicePreview("MELVIN","Unknown","","U","")
            ,new VoicePreview("MELVINSCIENTIST","Unknown","","U","")
            ,new VoicePreview("MICHAEL_NORMAL","Unknown","NORMAL,","U","")
            ,new VoicePreview("MICHAEL_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("MICHAEL_ANGRY","Unknown","ANGRY,","U","")
            ,new VoicePreview("MIME","Unknown","","U","")
            ,new VoicePreview("MIRANDA","Unknown","","U","")
            ,new VoicePreview("MISTERK","Unknown","","U","")
            ,new VoicePreview("MP_M_SHOPKEEP_01_CHINESE_MINI_01","Unknown","M","U","SHOPKEEP")
            ,new VoicePreview("MP_M_SHOPKEEP_01_LATINO_MINI_01","Unknown","M","U","SHOPKEEP")
            ,new VoicePreview("MP_M_SHOPKEEP_01_PAKISTANI_MINI_01","Unknown","M","U","SHOPKEEP")
            ,new VoicePreview("MRSTHORNHILL","Unknown","","U","")
            ,new VoicePreview("NERVOUSRON","Unknown","","U","")
            ,new VoicePreview("NIGEL","Unknown","","U","")
            ,new VoicePreview("NIGEL1BCELEBMALE01","Unknown","","U","")
            ,new VoicePreview("NIKKI","Unknown","","U","")
            ,new VoicePreview("NORM","Unknown","","U","")
            ,new VoicePreview("PACKIE","Unknown","","U","")
            ,new VoicePreview("PAIGE","Unknown","","U","")
            ,new VoicePreview("PAMELA_DRAKE","Unknown","DRAKE,","U","")
            ,new VoicePreview("PANIC_WALLA","Unknown","WALLA,","U","")
            ,new VoicePreview("PATRICIA","Unknown","","U","")
            ,new VoicePreview("PEACH","Unknown","","U","")
            ,new VoicePreview("POPPY","Unknown","","U","")
            ,new VoicePreview("PRISON_ANNOUNCER","Unknown","ANNOUNCER,","U","")
            ,new VoicePreview("PRISONER","Unknown","","U","")
            ,new VoicePreview("REDOCASTRO","Unknown","","U","")
            ,new VoicePreview("REDR1DRUNK1","Unknown","","U","")
            ,new VoicePreview("REDR2DRUNKM","Unknown","","U","")
            ,new VoicePreview("REHH2HIKER","Unknown","","U","")
            ,new VoicePreview("REHH3HIPSTER","Unknown","","U","")
            ,new VoicePreview("REHH5BRIDE","Unknown","","U","")
            ,new VoicePreview("REHOMGIRL","Unknown","","U","")
            ,new VoicePreview("REPRI1LOST","Unknown","","U","")
            ,new VoicePreview("SAPPHIRE","Unknown","","U","")
            ,new VoicePreview("SHOPASSISTANT","Unknown","","U","")
            ,new VoicePreview("SIMEON","Unknown","","U","")
            ,new VoicePreview("SOL1ACTOR","Unknown","","U","")
            ,new VoicePreview("SPACE_RANGER","Unknown","RANGER,","U","")
            ,new VoicePreview("STEVE","Unknown","","U","")
            ,new VoicePreview("STRETCH","Unknown","","U","")
            ,new VoicePreview("TALINA","Unknown","","U","")
            ,new VoicePreview("TAXIALONZO","Unknown","","U","")
            ,new VoicePreview("TAXICLYDE","Unknown","","U","")
            ,new VoicePreview("TAXIDARREN","Unknown","","U","")
            ,new VoicePreview("TAXIDERRICK","Unknown","","U","")
            ,new VoicePreview("TAXIFELIPE","Unknown","","U","")
            ,new VoicePreview("TAXIKEYLA","Unknown","","U","")
            ,new VoicePreview("TAXIKWAK","Unknown","","U","")
            ,new VoicePreview("TAXILIZ","Unknown","","U","")
            ,new VoicePreview("TAXIMIRANDA","Unknown","","U","")
            ,new VoicePreview("TAXIOTIS","Unknown","","U","")
            ,new VoicePreview("TAXIPAULIE","Unknown","","U","")
            ,new VoicePreview("TAXIWALTER","Unknown","","U","")
            ,new VoicePreview("TOM","Unknown","","U","")
            ,new VoicePreview("TONYA","Unknown","","U","")
            ,new VoicePreview("TRACEY","Unknown","","U","")
            ,new VoicePreview("TRANSLATOR","Unknown","","U","")
            ,new VoicePreview("TREVOR_NORMAL","Unknown","NORMAL,","U","")
            ,new VoicePreview("TREVOR_DRUNK","Unknown","DRUNK,","U","")
            ,new VoicePreview("TREVOR_ANGRY","Unknown","ANGRY,","U","")
            ,new VoicePreview("U_M_Y_TATTOO_01_WHITE_MINI_01","Unknown","M","Young","TATTOO")
            ,new VoicePreview("WADE","Unknown","","U","")
            ,new VoicePreview("WAVELOAD_PAIN_FEMALE","Unknown","PAIN","U","")
            ,new VoicePreview("WAVELOAD_PAIN_FRANKLIN","Unknown","PAIN","U","")
            ,new VoicePreview("WAVELOAD_PAIN_MALE","Unknown","PAIN","U","")
            ,new VoicePreview("WAVELOAD_PAIN_MICHAEL","Unknown","PAIN","U","")
            ,new VoicePreview("WAVELOAD_PAIN_TREVOR","Unknown","PAIN","U","")
            ,new VoicePreview("WFSTEWARDESS","Unknown","","U","")
            ,new VoicePreview("WHISTLINGJANITOR","Unknown","","U","")
            ,new VoicePreview("YACHTCAPTAIN","Unknown","","U","")
            ,new VoicePreview("ZOMBIE","Unknown","","U","")
        };


//        VoiceList = new List<string>()
//        {
//"A_F_M_BEACH_01_WHITE_FULL_01",
//"A_F_M_BEACH_01_WHITE_MINI_01",
//"A_F_M_BEVHILLS_01_WHITE_FULL_01",
//"A_F_M_BEVHILLS_01_WHITE_MINI_01",
//"A_F_M_BEVHILLS_01_WHITE_MINI_02",
//"A_F_M_BEVHILLS_02_BLACK_FULL_01",
//"A_F_M_BEVHILLS_02_BLACK_MINI_01",
//"A_F_M_BEVHILLS_02_WHITE_FULL_01",
//"A_F_M_BEVHILLS_02_WHITE_FULL_02",
//"A_F_M_BEVHILLS_02_WHITE_MINI_01",
//"A_F_M_BODYBUILD_01_BLACK_FULL_01",
//"A_F_M_BODYBUILD_01_BLACK_MINI_01",
//"A_F_M_BODYBUILD_01_WHITE_FULL_01",
//"A_F_M_BODYBUILD_01_WHITE_MINI_01",
//"A_F_M_BUSINESS_02_WHITE_MINI_01",
//"A_F_M_DOWNTOWN_01_BLACK_FULL_01",
//"A_F_M_EASTSA_01_LATINO_FULL_01",
//"A_F_M_EASTSA_01_LATINO_MINI_01",
//"A_F_M_EASTSA_02_LATINO_FULL_01",
//"A_F_M_EASTSA_02_LATINO_MINI_01",
//"A_F_M_FATWHITE_01_WHITE_FULL_01",
//"A_F_M_FATWHITE_01_WHITE_MINI_01",
//"A_F_M_KTOWN_01_KOREAN_FULL_01",
//"A_F_M_KTOWN_01_KOREAN_MINI_01",
//"A_F_M_KTOWN_02_CHINESE_MINI_01",
//"A_F_M_KTOWN_02_KOREAN_FULL_01",
//"A_F_M_SALTON_01_WHITE_FULL_01",
//"A_F_M_SALTON_01_WHITE_FULL_02",
//"A_F_M_SALTON_01_WHITE_FULL_03",
//"A_F_M_SALTON_01_WHITE_MINI_01",
//"A_F_M_SALTON_01_WHITE_MINI_02",
//"A_F_M_SALTON_01_WHITE_MINI_03",
//"A_F_M_SKIDROW_01_BLACK_FULL_01",
//"A_F_M_SKIDROW_01_BLACK_MINI_01",
//"A_F_M_SKIDROW_01_WHITE_FULL_01",
//"A_F_M_SKIDROW_01_WHITE_MINI_01",
//"A_F_M_SOUCENT_01_BLACK_FULL_01",
//"A_F_M_SOUCENT_02_BLACK_FULL_01",
//"A_F_M_TOURIST_01_WHITE_MINI_01",
//"A_F_M_TRAMP_01_WHITE_FULL_01",
//"A_F_M_TRAMP_01_WHITE_MINI_01",
//"A_F_M_TRAMPBEAC_01_BLACK_FULL_01",
//"A_F_M_TRAMPBEAC_01_BLACK_MINI_01",
//"A_F_M_TRAMPBEAC_01_WHITE_FULL_01",
//"A_F_O_GENSTREET_01_WHITE_MINI_01",
//"A_F_O_INDIAN_01_INDIAN_MINI_01",
//"A_F_O_KTOWN_01_KOREAN_FULL_01",
//"A_F_O_KTOWN_01_KOREAN_MINI_01",
//"A_F_O_SALTON_01_WHITE_FULL_01",
//"A_F_O_SALTON_01_WHITE_MINI_01",
//"A_F_O_SOUCENT_01_BLACK_FULL_01",
//"A_F_O_SOUCENT_02_BLACK_FULL_01",
//"A_F_Y_BEACH_01_BLACK_MINI_01",
//"A_F_Y_BEACH_01_WHITE_FULL_01",
//"A_F_Y_BEACH_01_WHITE_MINI_01",
//"A_F_Y_BEACH_BLACK_FULL_01",
//"A_F_Y_BEVHILLS_01_WHITE_FULL_01",
//"A_F_Y_BEVHILLS_01_WHITE_MINI_01",
//"A_F_Y_BEVHILLS_02_WHITE_FULL_01",
//"A_F_Y_BEVHILLS_02_WHITE_MINI_01",
//"A_F_Y_BEVHILLS_02_WHITE_MINI_02",
//"A_F_Y_BEVHILLS_03_WHITE_FULL_01",
//"A_F_Y_BEVHILLS_03_WHITE_MINI_01",
//"A_F_Y_BEVHILLS_04_WHITE_FULL_01",
//"A_F_Y_BEVHILLS_04_WHITE_MINI_01",
//"A_F_Y_BUSINESS_01_WHITE_FULL_01",
//"A_F_Y_BUSINESS_01_WHITE_MINI_01",
//"A_F_Y_BUSINESS_01_WHITE_MINI_02",
//"A_F_Y_BUSINESS_02_WHITE_FULL_01",
//"A_F_Y_BUSINESS_02_WHITE_MINI_01",
//"A_F_Y_BUSINESS_03_CHINESE_FULL_01",
//"A_F_Y_BUSINESS_03_CHINESE_MINI_01",
//"A_F_Y_BUSINESS_03_LATINO_FULL_01",
//"A_F_Y_BUSINESS_04_BLACK_FULL_01",
//"A_F_Y_BUSINESS_04_BLACK_MINI_01",
//"A_F_Y_BUSINESS_04_WHITE_MINI_01",
//"A_F_Y_EASTSA_01_LATINO_FULL_01",
//"A_F_Y_EASTSA_01_LATINO_MINI_01",
//"A_F_Y_EASTSA_02_WHITE_FULL_01",
//"A_F_Y_EASTSA_03_LATINO_FULL_01",
//"A_F_Y_EASTSA_03_LATINO_MINI_01",
//"A_F_Y_EPSILON_01_WHITE_MINI_01",
//"A_F_Y_FITNESS_01_WHITE_FULL_01",
//"A_F_Y_FITNESS_01_WHITE_MINI_01",
//"A_F_Y_FITNESS_02_BLACK_FULL_01",
//"A_F_Y_FITNESS_02_BLACK_MINI_01",
//"A_F_Y_FITNESS_02_WHITE_FULL_01",
//"A_F_Y_FITNESS_02_WHITE_MINI_01",
//"A_F_Y_GOLFER_01_WHITE_FULL_01",
//"A_F_Y_GOLFER_01_WHITE_MINI_01",
//"A_F_Y_HIKER_01_WHITE_FULL_01",
//"A_F_Y_HIKER_01_WHITE_MINI_01",
//"A_F_Y_HIPSTER_01_WHITE_FULL_01",
//"A_F_Y_HIPSTER_01_WHITE_MINI_01",
//"A_F_Y_HIPSTER_02_WHITE_FULL_01",
//"A_F_Y_HIPSTER_02_WHITE_MINI_01",
//"A_F_Y_HIPSTER_02_WHITE_MINI_02",
//"A_F_Y_HIPSTER_03_WHITE_FULL_01",
//"A_F_Y_HIPSTER_03_WHITE_MINI_01",
//"A_F_Y_HIPSTER_03_WHITE_MINI_02",
//"A_F_Y_HIPSTER_04_WHITE_FULL_01",
//"A_F_Y_HIPSTER_04_WHITE_MINI_01",
//"A_F_Y_HIPSTER_04_WHITE_MINI_02",
//"A_F_Y_INDIAN_01_INDIAN_MINI_01",
//"A_F_Y_INDIAN_01_INDIAN_MINI_02",
//"A_F_Y_ROLLERCOASTER_01_MINI_01",
//"A_F_Y_ROLLERCOASTER_01_MINI_02",
//"A_F_Y_ROLLERCOASTER_01_MINI_03",
//"A_F_Y_ROLLERCOASTER_01_MINI_04",
//"A_F_Y_SKATER_01_WHITE_FULL_01",
//"A_F_Y_SKATER_01_WHITE_MINI_01",
//"A_F_Y_SOUCENT_01_BLACK_FULL_01",
//"A_F_Y_SOUCENT_02_BLACK_FULL_01",
//"A_F_Y_SOUCENT_03_LATINO_FULL_01",
//"A_F_Y_SOUCENT_03_LATINO_MINI_01",
//"A_F_Y_TENNIS_01_BLACK_MINI_01",
//"A_F_Y_TENNIS_01_WHITE_MINI_01",
//"A_F_Y_TOURIST_01_BLACK_FULL_01",
//"A_F_Y_TOURIST_01_BLACK_MINI_01",
//"A_F_Y_TOURIST_01_LATINO_FULL_01",
//"A_F_Y_TOURIST_01_LATINO_MINI_01",
//"A_F_Y_TOURIST_01_WHITE_FULL_01",
//"A_F_Y_TOURIST_01_WHITE_MINI_01",
//"A_F_Y_TOURIST_02_WHITE_MINI_01",
//"A_F_Y_VINEWOOD_01_WHITE_FULL_01",
//"A_F_Y_VINEWOOD_01_WHITE_MINI_01",
//"A_F_Y_VINEWOOD_02_WHITE_FULL_01",
//"A_F_Y_VINEWOOD_02_WHITE_MINI_01",
//"A_F_Y_VINEWOOD_03_CHINESE_FULL_01",
//"A_F_Y_VINEWOOD_03_CHINESE_MINI_01",
//"A_F_Y_VINEWOOD_04_WHITE_FULL_01",
//"A_F_Y_VINEWOOD_04_WHITE_MINI_01",
//"A_F_Y_VINEWOOD_04_WHITE_MINI_02",
//"A_M_M_AFRIAMER_01_BLACK_FULL_01",
//"A_M_M_BEACH_01_BLACK_MINI_01",
//"A_M_M_BEACH_01_LATINO_FULL_01",
//"A_M_M_BEACH_01_LATINO_MINI_01",
//"A_M_M_BEACH_01_WHITE_FULL_01",
//"A_M_M_BEACH_01_WHITE_MINI_02",
//"A_M_M_BEACH_02_BLACK_FULL_01",
//"A_M_M_BEACH_02_WHITE_FULL_01",
//"A_M_M_BEACH_02_WHITE_MINI_01",
//"A_M_M_BEACH_02_WHITE_MINI_02",
//"A_M_M_BEVHILLS_01_BLACK_FULL_01",
//"A_M_M_BEVHILLS_01_BLACK_MINI_01",
//"A_M_M_BEVHILLS_01_WHITE_FULL_01",
//"A_M_M_BEVHILLS_01_WHITE_MINI_01",
//"A_M_M_BEVHILLS_02_BLACK_FULL_01",
//"A_M_M_BEVHILLS_02_BLACK_MINI_01",
//"A_M_M_BEVHILLS_02_WHITE_FULL_01",
//"A_M_M_BEVHILLS_02_WHITE_MINI_01",
//"A_M_M_BUSINESS_01_BLACK_FULL_01",
//"A_M_M_BUSINESS_01_BLACK_MINI_01",
//"A_M_M_BUSINESS_01_WHITE_FULL_01",
//"A_M_M_BUSINESS_01_WHITE_MINI_01",
//"A_M_M_EASTSA_01_LATINO_FULL_01",
//"A_M_M_EASTSA_01_LATINO_MINI_01",
//"A_M_M_EASTSA_02_LATINO_FULL_01",
//"A_M_M_EASTSA_02_LATINO_MINI_01",
//"A_M_M_FARMER_01_WHITE_MINI_01",
//"A_M_M_FARMER_01_WHITE_MINI_02",
//"A_M_M_FARMER_01_WHITE_MINI_03",
//"A_M_M_FATLATIN_01_LATINO_FULL_01",
//"A_M_M_FATLATIN_01_LATINO_MINI_01",
//"A_M_M_GENERICMALE_01_WHITE_MINI_01",
//"A_M_M_GENERICMALE_01_WHITE_MINI_02",
//"A_M_M_GENERICMALE_01_WHITE_MINI_03",
//"A_M_M_GENERICMALE_01_WHITE_MINI_04",
//"A_M_M_GENFAT_01_LATINO_FULL_01",
//"A_M_M_GENFAT_01_LATINO_MINI_01",
//"A_M_M_GOLFER_01_BLACK_FULL_01",
//"A_M_M_GOLFER_01_WHITE_FULL_01",
//"A_M_M_GOLFER_01_WHITE_MINI_01",
//"A_M_M_HASJEW_01_WHITE_MINI_01",
//"A_M_M_HILLBILLY_01_WHITE_MINI_01",
//"A_M_M_HILLBILLY_01_WHITE_MINI_02",
//"A_M_M_HILLBILLY_01_WHITE_MINI_03",
//"A_M_M_HILLBILLY_02_WHITE_MINI_01",
//"A_M_M_HILLBILLY_02_WHITE_MINI_02",
//"A_M_M_HILLBILLY_02_WHITE_MINI_03",
//"A_M_M_HILLBILLY_02_WHITE_MINI_04",
//"A_M_M_INDIAN_01_INDIAN_MINI_01",
//"A_M_M_KTOWN_01_KOREAN_FULL_01",
//"A_M_M_KTOWN_01_KOREAN_MINI_01",
//"A_M_M_MALIBU_01_BLACK_FULL_01",
//"A_M_M_MALIBU_01_LATINO_FULL_01",
//"A_M_M_MALIBU_01_LATINO_MINI_01",
//"A_M_M_MALIBU_01_WHITE_FULL_01",
//"A_M_M_MALIBU_01_WHITE_MINI_01",
//"A_M_M_POLYNESIAN_01_POLYNESIAN_FULL_01",
//"A_M_M_POLYNESIAN_01_POLYNESIAN_MINI_01",
//"A_M_M_SALTON_01_WHITE_FULL_01",
//"A_M_M_SALTON_02_WHITE_FULL_01",
//"A_M_M_SALTON_02_WHITE_MINI_01",
//"A_M_M_SALTON_02_WHITE_MINI_02",
//"A_M_M_SKATER_01_BLACK_FULL_01",
//"A_M_M_SKATER_01_WHITE_FULL_01",
//"A_M_M_SKATER_01_WHITE_MINI_01",
//"A_M_M_SKIDROW_01_BLACK_FULL_01",
//"A_M_M_SOCENLAT_01_LATINO_FULL_01",
//"A_M_M_SOCENLAT_01_LATINO_MINI_01",
//"A_M_M_SOUCENT_01_BLACK_FULL_01",
//"A_M_M_SOUCENT_02_BLACK_FULL_01",
//"A_M_M_SOUCENT_03_BLACK_FULL_01",
//"A_M_M_SOUCENT_04_BLACK_FULL_01",
//"A_M_M_SOUCENT_04_BLACK_MINI_01",
//"A_M_M_STLAT_02_LATINO_FULL_01",
//"A_M_M_TENNIS_01_BLACK_MINI_01",
//"A_M_M_TENNIS_01_WHITE_MINI_01",
//"A_M_M_TOURIST_01_WHITE_MINI_01",
//"A_M_M_TRAMP_01_BLACK_FULL_01",
//"A_M_M_TRAMP_01_BLACK_MINI_01",
//"A_M_M_TRAMPBEAC_01_BLACK_FULL_01",
//"A_M_M_TRANVEST_01_WHITE_MINI_01",
//"A_M_M_TRANVEST_02_LATINO_FULL_01",
//"A_M_M_TRANVEST_02_LATINO_MINI_01",
//"A_M_O_BEACH_01_WHITE_FULL_01",
//"A_M_O_BEACH_01_WHITE_MINI_01",
//"A_M_O_GENSTREET_01_WHITE_FULL_01",
//"A_M_O_GENSTREET_01_WHITE_MINI_01",
//"A_M_O_SALTON_01_WHITE_FULL_01",
//"A_M_O_SALTON_01_WHITE_MINI_01",
//"A_M_O_SOUCENT_01_BLACK_FULL_01",
//"A_M_O_SOUCENT_02_BLACK_FULL_01",
//"A_M_O_SOUCENT_03_BLACK_FULL_01",
//"A_M_O_TRAMP_01_BLACK_FULL_01",
//"A_M_Y_BEACH_01_CHINESE_FULL_01",
//"A_M_Y_BEACH_01_CHINESE_MINI_01",
//"A_M_Y_BEACH_01_WHITE_FULL_01",
//"A_M_Y_BEACH_01_WHITE_MINI_01",
//"A_M_Y_BEACH_02_LATINO_FULL_01",
//"A_M_Y_BEACH_02_WHITE_FULL_01",
//"A_M_Y_BEACH_03_BLACK_FULL_01",
//"A_M_Y_BEACH_03_BLACK_MINI_01",
//"A_M_Y_BEACH_03_WHITE_FULL_01",
//"A_M_Y_BEACHVESP_01_CHINESE_FULL_01",
//"A_M_Y_BEACHVESP_01_CHINESE_MINI_01",
//"A_M_Y_BEACHVESP_01_WHITE_FULL_01",
//"A_M_Y_BEACHVESP_02_WHITE_FULL_01",
//"A_M_Y_BEACHVESP_02_WHITE_MINI_01",
//"A_M_Y_BEVHILLS_01_BLACK_FULL_01",
//"A_M_Y_BEVHILLS_01_WHITE_FULL_01",
//"A_M_Y_BEVHILLS_02_BLACK_FULL_01",
//"A_M_Y_BEVHILLS_02_WHITE_FULL_01",
//"A_M_Y_BEVHILLS_02_WHITE_MINI_01",
//"A_M_Y_BUSICAS_01_WHITE_MINI_01",
//"A_M_Y_BUSINESS_01_BLACK_FULL_01",
//"A_M_Y_BUSINESS_01_BLACK_MINI_01",
//"A_M_Y_BUSINESS_01_CHINESE_FULL_01",
//"A_M_Y_BUSINESS_01_WHITE_FULL_01",
//"A_M_Y_BUSINESS_01_WHITE_MINI_02",
//"A_M_Y_BUSINESS_02_BLACK_FULL_01",
//"A_M_Y_BUSINESS_02_BLACK_MINI_01",
//"A_M_Y_BUSINESS_02_WHITE_FULL_01",
//"A_M_Y_BUSINESS_02_WHITE_MINI_01",
//"A_M_Y_BUSINESS_02_WHITE_MINI_02",
//"A_M_Y_BUSINESS_03_BLACK_FULL_01",
//"A_M_Y_BUSINESS_03_WHITE_MINI_01",
//"A_M_Y_DOWNTOWN_01_BLACK_FULL_01",
//"A_M_Y_EASTSA_01_LATINO_FULL_01",
//"A_M_Y_EASTSA_01_LATINO_MINI_01",
//"A_M_Y_EASTSA_02_LATINO_FULL_01",
//"A_M_Y_EPSILON_01_BLACK_FULL_01",
//"A_M_Y_EPSILON_01_KOREAN_FULL_01",
//"A_M_Y_EPSILON_01_WHITE_FULL_01",
//"A_M_Y_EPSILON_02_WHITE_MINI_01",
//"A_M_Y_GAY_01_BLACK_FULL_01",
//"A_M_Y_GAY_01_LATINO_FULL_01",
//"A_M_Y_GAY_02_WHITE_MINI_01",
//"A_M_Y_GENSTREET_01_CHINESE_FULL_01",
//"A_M_Y_GENSTREET_01_CHINESE_MINI_01",
//"A_M_Y_GENSTREET_01_WHITE_FULL_01",
//"A_M_Y_GENSTREET_01_WHITE_MINI_01",
//"A_M_Y_GENSTREET_02_BLACK_FULL_01",
//"A_M_Y_GENSTREET_02_LATINO_FULL_01",
//"A_M_Y_GENSTREET_02_LATINO_MINI_01",
//"A_M_Y_GOLFER_01_WHITE_FULL_01",
//"A_M_Y_GOLFER_01_WHITE_MINI_01",
//"A_M_Y_HASJEW_01_WHITE_MINI_01",
//"A_M_Y_HIPPY_01_WHITE_FULL_01",
//"A_M_Y_HIPPY_01_WHITE_MINI_01",
//"A_M_Y_HIPSTER_01_BLACK_FULL_01",
//"A_M_Y_HIPSTER_01_WHITE_FULL_01",
//"A_M_Y_HIPSTER_01_WHITE_MINI_01",
//"A_M_Y_HIPSTER_02_WHITE_FULL_01",
//"A_M_Y_HIPSTER_02_WHITE_MINI_01",
//"A_M_Y_HIPSTER_03_WHITE_FULL_01",
//"A_M_Y_HIPSTER_03_WHITE_MINI_01",
//"A_M_Y_KTOWN_01_KOREAN_FULL_01",
//"A_M_Y_KTOWN_01_KOREAN_MINI_01",
//"A_M_Y_KTOWN_02_KOREAN_FULL_01",
//"A_M_Y_KTOWN_02_KOREAN_MINI_01",
//"A_M_Y_LATINO_01_LATINO_MINI_01",
//"A_M_Y_LATINO_01_LATINO_MINI_02",
//"A_M_Y_MEXTHUG_01_LATINO_FULL_01",
//"A_M_Y_MUSCLBEAC_01_BLACK_FULL_01",
//"A_M_Y_MUSCLBEAC_01_WHITE_FULL_01",
//"A_M_Y_MUSCLBEAC_01_WHITE_MINI_01",
//"A_M_Y_MUSCLBEAC_02_CHINESE_FULL_01",
//"A_M_Y_MUSCLBEAC_02_LATINO_FULL_01",
//"A_M_Y_POLYNESIAN_01_POLYNESIAN_FULL_01",
//"A_M_Y_RACER_01_WHITE_MINI_01",
//"A_M_Y_ROLLERCOASTER_01_MINI_01",
//"A_M_Y_ROLLERCOASTER_01_MINI_02",
//"A_M_Y_ROLLERCOASTER_01_MINI_03",
//"A_M_Y_ROLLERCOASTER_01_MINI_04",
//"A_M_Y_RUNNER_01_WHITE_FULL_01",
//"A_M_Y_RUNNER_01_WHITE_MINI_01",
//"A_M_Y_SALTON_01_WHITE_MINI_01",
//"A_M_Y_SALTON_01_WHITE_MINI_02",
//"A_M_Y_SKATER_01_WHITE_FULL_01",
//"A_M_Y_SKATER_01_WHITE_MINI_01",
//"A_M_Y_SKATER_02_BLACK_FULL_01",
//"A_M_Y_SOUCENT_01_BLACK_FULL_01",
//"A_M_Y_SOUCENT_02_BLACK_FULL_01",
//"A_M_Y_SOUCENT_03_BLACK_FULL_01",
//"A_M_Y_SOUCENT_04_BLACK_FULL_01",
//"A_M_Y_SOUCENT_04_BLACK_MINI_01",
//"A_M_Y_STBLA_01_BLACK_FULL_01",
//"A_M_Y_STBLA_02_BLACK_FULL_01",
//"A_M_Y_STLAT_01_LATINO_FULL_01",
//"A_M_Y_STLAT_01_LATINO_MINI_01",
//"A_M_Y_STWHI_01_WHITE_FULL_01",
//"A_M_Y_STWHI_01_WHITE_MINI_01",
//"A_M_Y_STWHI_02_WHITE_FULL_01",
//"A_M_Y_STWHI_02_WHITE_MINI_01",
//"A_M_Y_SUNBATHE_01_BLACK_FULL_01",
//"A_M_Y_SUNBATHE_01_WHITE_FULL_01",
//"A_M_Y_SUNBATHE_01_WHITE_MINI_01",
//"A_M_Y_TRIATHLON_01_MINI_01",
//"A_M_Y_TRIATHLON_01_MINI_02",
//"A_M_Y_TRIATHLON_01_MINI_03",
//"A_M_Y_TRIATHLON_01_MINI_04",
//"A_M_Y_VINEWOOD_01_BLACK_FULL_01",
//"A_M_Y_VINEWOOD_01_BLACK_MINI_01",
//"A_M_Y_VINEWOOD_02_WHITE_FULL_01",
//"A_M_Y_VINEWOOD_02_WHITE_MINI_01",
//"A_M_Y_VINEWOOD_03_LATINO_FULL_01",
//"A_M_Y_VINEWOOD_03_LATINO_MINI_01",
//"A_M_Y_VINEWOOD_03_WHITE_FULL_01",
//"A_M_Y_VINEWOOD_03_WHITE_MINI_01",
//"A_M_Y_VINEWOOD_04_WHITE_FULL_01",
//"A_M_Y_VINEWOOD_04_WHITE_MINI_01",
//"ABIGAIL",
//"AIRCRAFT_WARNING_FEMALE_01",
//"AIRCRAFT_WARNING_MALE_01",
//"AIRDUMMER",
//"AIRGUITARIST",
//"AIRPIANIST",
//"ALIENS",
//"AMANDA_DRUNK",
//"AMANDA_NORMAL",
//"AMMUCITY",
//"ANDY_MOON",
//"ANTON",
//"AVI",
//"BAILBOND1JUMPER",
//"BAILBOND2JUMPER",
//"BAILBOND3JUMPER",
//"BAILBOND4JUMPER",
//"BALLASOG",
//"BANKWM1",
//"BANKWM2",
//"BARRY",
//"BAYGOR",
//"BENNY",
//"BEVERLY",
//"BILLBINDER",
//"BJPILOT_CANAL",
//"BJPILOT_TRAIN",
//"BRAD",
//"BTL_BLMADONNA",
//"BTL_CARMINE",
//"BTL_CONNIE",
//"BTL_DAVE",
//"BTL_DIXON",
//"BTL_LAZLOW",
//"BTL_MARCEL",
//"BTL_MATEO",
//"BTL_MFRIEND",
//"BTL_POLICE1",
//"BTL_SOLOMUN",
//"BTL_TFRIEND",
//"BTL_TONY",
//"BTL_YOHAN",
//"BUSINESSMAN",
//"CASEY",
//"CHASTITY",
//"CHEETAH",
//"CHEF",
//"CHENG",
//"CLETUS",
//"CLINTON",
//"CLOWNS",
//"COOK",
//"CST4ACTRESS",
//"DARYL",
//"DAVE",
//"DENISE",
//"DOM",
//"EDDIE",
//"EXECPA_FEMALE",
//"EXECPA_MALE",
//"EXT1HELIPILOT",
//"FLOYD",
//"FRANKLIN_ANGRY",
//"FRANKLIN_DRUNK",
//"FRANKLIN_NORMAL",
//"FUFU",
//"G_F_Y_BALLAS_01_BLACK_MINI_01",
//"G_F_Y_BALLAS_01_BLACK_MINI_02",
//"G_F_Y_BALLAS_01_BLACK_MINI_03",
//"G_F_Y_BALLAS_01_BLACK_MINI_04",
//"G_F_Y_FAMILIES_01_BLACK_MINI_01",
//"G_F_Y_FAMILIES_01_BLACK_MINI_02",
//"G_F_Y_FAMILIES_01_BLACK_MINI_03",
//"G_F_Y_FAMILIES_01_BLACK_MINI_04",
//"G_F_Y_FAMILIES_01_BLACK_MINI_05",
//"G_F_Y_VAGOS_01_LATINO_MINI_01",
//"G_F_Y_VAGOS_01_LATINO_MINI_02",
//"G_F_Y_VAGOS_01_LATINO_MINI_03",
//"G_F_Y_VAGOS_01_LATINO_MINI_04",
//"G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_01",
//"G_M_M_ARMBOSS_01_WHITE_ARMENIAN_MINI_02",
//"G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_01",
//"G_M_M_ARMLIEUT_01_WHITE_ARMENIAN_MINI_02",
//"G_M_M_CHIBOSS_01_CHINESE_MINI_01",
//"G_M_M_CHIBOSS_01_CHINESE_MINI_02",
//"G_M_M_CHIGOON_01_CHINESE_MINI_01",
//"G_M_M_CHIGOON_01_CHINESE_MINI_02",
//"G_M_M_CHIGOON_02_CHINESE_MINI_01",
//"G_M_M_CHIGOON_02_CHINESE_MINI_02",
//"G_M_M_KORBOSS_01_KOREAN_MINI_01",
//"G_M_M_KORBOSS_01_KOREAN_MINI_02",
//"G_M_M_MEXBOSS_01_LATINO_MINI_01",
//"G_M_M_MEXBOSS_01_LATINO_MINI_02",
//"G_M_M_MEXBOSS_02_LATINO_MINI_01",
//"G_M_M_MEXBOSS_02_LATINO_MINI_02",
//"G_M_Y_ARMGOON_02_WHITE_ARMENIAN_MINI_01",
//"G_M_Y_ARMGOON_02_WHITE_ARMENIAN_MINI_02",
//"G_M_Y_BALLAEAST_01_BLACK_FULL_01",
//"G_M_Y_BALLAEAST_01_BLACK_FULL_02",
//"G_M_Y_BALLAEAST_01_BLACK_MINI_01",
//"G_M_Y_BALLAEAST_01_BLACK_MINI_02",
//"G_M_Y_BALLAEAST_01_BLACK_MINI_03",
//"G_M_Y_BALLAORIG_01_BLACK_FULL_01",
//"G_M_Y_BALLAORIG_01_BLACK_FULL_02",
//"G_M_Y_BALLAORIG_01_BLACK_MINI_01",
//"G_M_Y_BALLAORIG_01_BLACK_MINI_02",
//"G_M_Y_BALLAORIG_01_BLACK_MINI_03",
//"G_M_Y_BALLASOUT_01_BLACK_FULL_01",
//"G_M_Y_BALLASOUT_01_BLACK_FULL_02",
//"G_M_Y_BALLASOUT_01_BLACK_MINI_01",
//"G_M_Y_BALLASOUT_01_BLACK_MINI_02",
//"G_M_Y_BALLASOUT_01_BLACK_MINI_03",
//"G_M_Y_FAMCA_01_BLACK_FULL_01",
//"G_M_Y_FAMCA_01_BLACK_FULL_02",
//"G_M_Y_FAMCA_01_BLACK_MINI_01",
//"G_M_Y_FAMCA_01_BLACK_MINI_02",
//"G_M_Y_FAMCA_01_BLACK_MINI_03",
//"G_M_Y_FAMDNF_01_BLACK_FULL_01",
//"G_M_Y_FAMDNF_01_BLACK_FULL_02",
//"G_M_Y_FAMDNF_01_BLACK_MINI_01",
//"G_M_Y_FAMDNF_01_BLACK_MINI_02",
//"G_M_Y_FAMDNF_01_BLACK_MINI_03",
//"G_M_Y_FAMFOR_01_BLACK_FULL_01",
//"G_M_Y_FAMFOR_01_BLACK_FULL_02",
//"G_M_Y_FAMFOR_01_BLACK_MINI_01",
//"G_M_Y_FAMFOR_01_BLACK_MINI_02",
//"G_M_Y_FAMFOR_01_BLACK_MINI_03",
//"G_M_Y_KOREAN_01_KOREAN_MINI_01",
//"G_M_Y_KOREAN_01_KOREAN_MINI_02",
//"G_M_Y_KOREAN_02_KOREAN_MINI_01",
//"G_M_Y_KOREAN_02_KOREAN_MINI_02",
//"G_M_Y_KORLIEUT_01_KOREAN_MINI_01",
//"G_M_Y_KORLIEUT_01_KOREAN_MINI_02",
//"G_M_Y_LOST_01_BLACK_FULL_01",
//"G_M_Y_LOST_01_BLACK_FULL_02",
//"G_M_Y_LOST_01_BLACK_MINI_01",
//"G_M_Y_LOST_01_BLACK_MINI_02",
//"G_M_Y_LOST_01_BLACK_MINI_03",
//"G_M_Y_LOST_01_WHITE_FULL_01",
//"G_M_Y_LOST_01_WHITE_MINI_01",
//"G_M_Y_LOST_01_WHITE_MINI_02",
//"G_M_Y_LOST_02_LATINO_FULL_01",
//"G_M_Y_LOST_02_LATINO_FULL_02",
//"G_M_Y_LOST_02_LATINO_MINI_01",
//"G_M_Y_LOST_02_LATINO_MINI_02",
//"G_M_Y_LOST_02_LATINO_MINI_03",
//"G_M_Y_LOST_02_WHITE_FULL_01",
//"G_M_Y_LOST_02_WHITE_MINI_01",
//"G_M_Y_LOST_02_WHITE_MINI_02",
//"G_M_Y_LOST_03_WHITE_FULL_01",
//"G_M_Y_LOST_03_WHITE_MINI_02",
//"G_M_Y_LOST_03_WHITE_MINI_03",
//"G_M_Y_MEXGOON_01_LATINO_MINI_01",
//"G_M_Y_MEXGOON_01_LATINO_MINI_02",
//"G_M_Y_MEXGOON_02_LATINO_MINI_01",
//"G_M_Y_MEXGOON_02_LATINO_MINI_02",
//"G_M_Y_MEXGOON_03_LATINO_MINI_01",
//"G_M_Y_MEXGOON_03_LATINO_MINI_02",
//"G_M_Y_POLOGOON_01_LATINO_MINI_01",
//"G_M_Y_POLOGOON_01_LATINO_MINI_02",
//"G_M_Y_SALVABOSS_01_SALVADORIAN_MINI_01",
//"G_M_Y_SALVABOSS_01_SALVADORIAN_MINI_02",
//"G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_01",
//"G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_02",
//"G_M_Y_SALVAGOON_01_SALVADORIAN_MINI_03",
//"G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_01",
//"G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_02",
//"G_M_Y_SALVAGOON_02_SALVADORIAN_MINI_03",
//"G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_01",
//"G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_02",
//"G_M_Y_SALVAGOON_03_SALVADORIAN_MINI_03",
//"G_M_Y_STREETPUNK_01_BLACK_MINI_01",
//"G_M_Y_STREETPUNK_01_BLACK_MINI_02",
//"G_M_Y_STREETPUNK_01_BLACK_MINI_03",
//"G_M_Y_STREETPUNK_02_BLACK_MINI_01",
//"G_M_Y_STREETPUNK_02_BLACK_MINI_02",
//"G_M_Y_STREETPUNK_02_BLACK_MINI_03",
//"G_M_Y_STREETPUNK02_BLACK_MINI_04",
//"GARDENER",
//"GAYMILITARY",
//"GERALD",
//"GIRL1",
//"GIRL2",
//"GRIFF",
//"GROOM",
//"GUSTAVO",
//"HAO",
//"HEISTMANAGER",
//"HOSTAGEBF1",
//"HOSTAGEBM1",
//"HOSTAGEWF1",
//"HOSTAGEWF2",
//"HOSTAGEWM1",
//"HOSTAGEWM2",
//"HUGH",
//"IMPOTENT_RAGE",
//"INFERNUS",
//"JANE",
//"JANET",
//"JEROME",
//"JESSE",
//"JIMMY_DRUNK",
//"JIMMY_NORMAL",
//"JIMMYBOSTON",
//"JOE",
//"JOSEF",
//"JOSH",
//"JULIET",
//"KAREN",
//"KARIM",
//"KARL",
//"KERRY",
//"KIDNAPPEDFEMALE",
//"LACEY",
//"LAMAR_1_NORMAL",
//"LAMAR_2_NORMAL",
//"LAMAR_DRUNK",
//"LESTER",
//"LIENGINEER",
//"LIENGINEER2",
//"LOSTKIDNAPGIRL",
//"MAID",
//"MALE_STRIP_DJ_WHITE",
//"MANI",
//"MARNIE",
//"MARYANN",
//"MAUDE",
//"MELVIN",
//"MELVINSCIENTIST",
//"MICHAEL_ANGRY",
//"MICHAEL_DRUNK",
//"MICHAEL_NORMAL",
//"MIME",
//"MIRANDA",
//"MISTERK",
//"MP_M_SHOPKEEP_01_CHINESE_MINI_01",
//"MP_M_SHOPKEEP_01_LATINO_MINI_01",
//"MP_M_SHOPKEEP_01_PAKISTANI_MINI_01",
//"MRSTHORNHILL",
//"NERVOUSRON",
//"NIGEL",
//"NIGEL1BCELEBMALE01",
//"NIKKI",
//"NORM",
//"PACKIE",
//"PAIGE",
//"PAMELA_DRAKE",
//"PANIC_WALLA",
//"PATRICIA",
//"PEACH",
//"POPPY",
//"PRISON_ANNOUNCER",
//"PRISONER",
//"REDOCASTRO",
//"REDR1DRUNK1",
//"REDR2DRUNKM",
//"REHH2HIKER",
//"REHH3HIPSTER",
//"REHH5BRIDE",
//"REHOMGIRL",
//"REPRI1LOST",
//"S_F_M_FEMBARBER_BLACK_MINI_01",
//"S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_01",
//"S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_02",
//"S_F_M_GENERICCHEAPWORKER_01_LATINO_MINI_03",
//"S_F_M_PONSEN_01_BLACK_01",
//"S_F_M_SHOP_HIGH_WHITE_MINI_01",
//"S_F_Y_AIRHOSTESS_01_BLACK_FULL_01",
//"S_F_Y_AIRHOSTESS_01_BLACK_FULL_02",
//"S_F_Y_AIRHOSTESS_01_WHITE_FULL_01",
//"S_F_Y_AIRHOSTESS_01_WHITE_FULL_02",
//"S_F_Y_BAYWATCH_01_BLACK_FULL_01",
//"S_F_Y_BAYWATCH_01_BLACK_FULL_02",
//"S_F_Y_BAYWATCH_01_WHITE_FULL_01",
//"S_F_Y_BAYWATCH_01_WHITE_FULL_02",
//"S_F_Y_COP_01_BLACK_FULL_01",
//"S_F_Y_COP_01_BLACK_FULL_02",
//"S_F_Y_COP_01_WHITE_FULL_01",
//"S_F_Y_COP_01_WHITE_FULL_02",
//"S_F_Y_GENERICCHEAPWORKER_01_BLACK_MINI_01",
//"S_F_Y_GENERICCHEAPWORKER_01_BLACK_MINI_02",
//"S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_01",
//"S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_02",
//"S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_03",
//"S_F_Y_GENERICCHEAPWORKER_01_LATINO_MINI_04",
//"S_F_Y_GENERICCHEAPWORKER_01_WHITE_MINI_01",
//"S_F_Y_GENERICCHEAPWORKER_01_WHITE_MINI_02",
//"S_F_Y_HOOKER_01_WHITE_FULL_01",
//"S_F_Y_HOOKER_01_WHITE_FULL_02",
//"S_F_Y_HOOKER_01_WHITE_FULL_03",
//"S_F_Y_HOOKER_02_WHITE_FULL_01",
//"S_F_Y_HOOKER_02_WHITE_FULL_02",
//"S_F_Y_HOOKER_02_WHITE_FULL_03",
//"S_F_Y_HOOKER_03_BLACK_FULL_01",
//"S_F_Y_HOOKER_03_BLACK_FULL_03",
//"S_F_Y_PECKER_01_WHITE_01",
//"S_F_Y_RANGER_01_WHITE_MINI_01",
//"S_F_Y_SHOP_LOW_WHITE_MINI_01",
//"S_F_Y_SHOP_MID_WHITE_MINI_01",
//"S_M_M_AMMUCOUNTRY_01_WHITE_01",
//"S_M_M_AMMUCOUNTRY_WHITE_MINI_01",
//"S_M_M_AUTOSHOP_01_WHITE_01",
//"S_M_M_BOUNCER_01_BLACK_FULL_01",
//"S_M_M_BOUNCER_01_LATINO_FULL_01",
//"S_M_M_CIASEC_01_BLACK_MINI_01",
//"S_M_M_CIASEC_01_BLACK_MINI_02",
//"S_M_M_CIASEC_01_WHITE_MINI_01",
//"S_M_M_CIASEC_01_WHITE_MINI_02",
//"S_M_M_FIBOFFICE_01_BLACK_MINI_01",
//"S_M_M_FIBOFFICE_01_BLACK_MINI_02",
//"S_M_M_FIBOFFICE_01_LATINO_MINI_01",
//"S_M_M_FIBOFFICE_01_LATINO_MINI_02",
//"S_M_M_FIBOFFICE_01_WHITE_MINI_01",
//"S_M_M_FIBOFFICE_01_WHITE_MINI_02",
//"S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_01",
//"S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_02",
//"S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_03",
//"S_M_M_GENERICCHEAPWORKER_01_LATINO_MINI_04",
//"S_M_M_GENERICMARINE_01_LATINO_MINI_01",
//"S_M_M_GENERICMARINE_01_LATINO_MINI_02",
//"S_M_M_GENERICMECHANIC_01_BLACK_MINI_01",
//"S_M_M_GENERICMECHANIC_01_BLACK_MINI_02",
//"S_M_M_GENERICPOSTWORKER_01_BLACK_MINI_01",
//"S_M_M_GENERICPOSTWORKER_01_BLACK_MINI_02",
//"S_M_M_GENERICPOSTWORKER_01_WHITE_MINI_01",
//"S_M_M_GENERICPOSTWORKER_01_WHITE_MINI_02",
//"S_M_M_GENERICSECURITY_01_BLACK_MINI_01",
//"S_M_M_GENERICSECURITY_01_BLACK_MINI_02",
//"S_M_M_GENERICSECURITY_01_BLACK_MINI_03",
//"S_M_M_GENERICSECURITY_01_LATINO_MINI_01",
//"S_M_M_GENERICSECURITY_01_LATINO_MINI_02",
//"S_M_M_GENERICSECURITY_01_WHITE_MINI_01",
//"S_M_M_GENERICSECURITY_01_WHITE_MINI_02",
//"S_M_M_GENERICSECURITY_01_WHITE_MINI_03",
//"S_M_M_HAIRDRESS_01_BLACK_01",
//"S_M_M_HAIRDRESSER_01_BLACK_MINI_01",
//"S_M_M_PARAMEDIC_01_BLACK_MINI_01",
//"S_M_M_PARAMEDIC_01_LATINO_MINI_01",
//"S_M_M_PARAMEDIC_01_WHITE_MINI_01",
//"S_M_M_PILOT_01_BLACK_FULL_01",
//"S_M_M_PILOT_01_BLACK_FULL_02",
//"S_M_M_PILOT_01_WHITE_FULL_01",
//"S_M_M_PILOT_01_WHITE_FULL_02",
//"S_M_M_TRUCKER_01_BLACK_FULL_01",
//"S_M_M_TRUCKER_01_WHITE_FULL_01",
//"S_M_M_TRUCKER_01_WHITE_FULL_02",
//"S_M_Y_AIRWORKER_BLACK_FULL_01",
//"S_M_Y_AIRWORKER_BLACK_FULL_02",
//"S_M_Y_AIRWORKER_LATINO_FULL_01",
//"S_M_Y_AIRWORKER_LATINO_FULL_02",
//"S_M_Y_AMMUCITY_01_WHITE_01",
//"S_M_Y_AMMUCITY_01_WHITE_MINI_01",
//"S_M_Y_BAYWATCH_01_BLACK_FULL_01",
//"S_M_Y_BAYWATCH_01_BLACK_FULL_02",
//"S_M_Y_BAYWATCH_01_WHITE_FULL_01",
//"S_M_Y_BAYWATCH_01_WHITE_FULL_02",
//"S_M_Y_BLACKOPS_01_BLACK_MINI_01",
//"S_M_Y_BLACKOPS_01_BLACK_MINI_02",
//"S_M_Y_BLACKOPS_01_WHITE_MINI_01",
//"S_M_Y_BLACKOPS_01_WHITE_MINI_02",
//"S_M_Y_BLACKOPS_02_LATINO_MINI_01",
//"S_M_Y_BLACKOPS_02_LATINO_MINI_02",
//"S_M_Y_BLACKOPS_02_WHITE_MINI_01",
//"S_M_Y_COP_01_BLACK_FULL_01",
//"S_M_Y_COP_01_BLACK_FULL_02",
//"S_M_Y_COP_01_BLACK_MINI_01",
//"S_M_Y_COP_01_BLACK_MINI_02",
//"S_M_Y_COP_01_BLACK_MINI_03",
//"S_M_Y_COP_01_BLACK_MINI_04",
//"S_M_Y_COP_01_WHITE_FULL_01",
//"S_M_Y_COP_01_WHITE_FULL_02",
//"S_M_Y_COP_01_WHITE_MINI_01",
//"S_M_Y_COP_01_WHITE_MINI_02",
//"S_M_Y_COP_01_WHITE_MINI_03",
//"S_M_Y_COP_01_WHITE_MINI_04",
//"S_M_Y_FIREMAN_01_LATINO_FULL_01",
//"S_M_Y_FIREMAN_01_LATINO_FULL_02",
//"S_M_Y_FIREMAN_01_WHITE_FULL_01",
//"S_M_Y_FIREMAN_01_WHITE_FULL_02",
//"S_M_Y_GENERICCHEAPWORKER_01_BLACK_MINI_01",
//"S_M_Y_GENERICCHEAPWORKER_01_BLACK_MINI_02",
//"S_M_Y_GENERICCHEAPWORKER_01_WHITE_MINI_01",
//"S_M_Y_GENERICMARINE_01_BLACK_MINI_01",
//"S_M_Y_GENERICMARINE_01_BLACK_MINI_02",
//"S_M_Y_GENERICMARINE_01_WHITE_MINI_01",
//"S_M_Y_GENERICMARINE_01_WHITE_MINI_02",
//"S_M_Y_GENERICWORKER_01_BLACK_MINI_01",
//"S_M_Y_GENERICWORKER_01_BLACK_MINI_02",
//"S_M_Y_GENERICWORKER_01_LATINO_MINI_01",
//"S_M_Y_GENERICWORKER_01_LATINO_MINI_02",
//"S_M_Y_GENERICWORKER_01_WHITE_MINI_01",
//"S_M_Y_GENERICWORKER_01_WHITE_MINI_02",
//"S_M_Y_HWAYCOP_01_BLACK_FULL_01",
//"S_M_Y_HWAYCOP_01_BLACK_FULL_02",
//"S_M_Y_HWAYCOP_01_WHITE_FULL_01",
//"S_M_Y_HWAYCOP_01_WHITE_FULL_02",
//"S_M_Y_MCOP_01_WHITE_MINI_01",
//"S_M_Y_MCOP_01_WHITE_MINI_02",
//"S_M_Y_MCOP_01_WHITE_MINI_03",
//"S_M_Y_MCOP_01_WHITE_MINI_04",
//"S_M_Y_MCOP_01_WHITE_MINI_05",
//"S_M_Y_MCOP_01_WHITE_MINI_06",
//"S_M_Y_RANGER_01_LATINO_FULL_01",
//"S_M_Y_RANGER_01_WHITE_FULL_01",
//"S_M_Y_SHERIFF_01_WHITE_FULL_01",
//"S_M_Y_SHERIFF_01_WHITE_FULL_02",
//"S_M_Y_SHOP_MASK_WHITE_MINI_01",
//"S_M_Y_SWAT_01_WHITE_FULL_01",
//"S_M_Y_SWAT_01_WHITE_FULL_02",
//"S_M_Y_SWAT_01_WHITE_FULL_03",
//"S_M_Y_SWAT_01_WHITE_FULL_04",
//"SAPPHIRE",
//"SHOPASSISTANT",
//"SIMEON",
//"SOL1ACTOR",
//"SPACE_RANGER",
//"STEVE",
//"STRETCH",
//"TALINA",
//"TAXIALONZO",
//"TAXICLYDE",
//"TAXIDARREN",
//"TAXIDERRICK",
//"TAXIFELIPE",
//"TAXIKEYLA",
//"TAXIKWAK",
//"TAXILIZ",
//"TAXIMIRANDA",
//"TAXIOTIS",
//"TAXIPAULIE",
//"TAXIWALTER",
//"TOM",
//"TONYA",
//"TRACEY",
//"TRANSLATOR",
//"TREVOR_ANGRY",
//"TREVOR_DRUNK",
//"TREVOR_NORMAL",
//"U_M_Y_TATTOO_01_WHITE_MINI_01",
//"WADE",
//"WAVELOAD_PAIN_FEMALE",
//"WAVELOAD_PAIN_FRANKLIN",
//"WAVELOAD_PAIN_MALE",
//"WAVELOAD_PAIN_MICHAEL",
//"WAVELOAD_PAIN_TREVOR",
//"WFSTEWARDESS",
//"WHISTLINGJANITOR",
//"YACHTCAPTAIN",
//"ZOMBIE",
//        };



    }
}

