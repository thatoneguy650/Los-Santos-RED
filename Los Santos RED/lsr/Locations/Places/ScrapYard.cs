using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ScrapYard : InteractableLocation
{
    private UIMenu ScrapMenu;
    private bool HasBannerImage = true;
    private Texture bannerTexture;
    private MenuPool MenuPool;
    private UIMenuItem ScrapVehicle;
    private bool IsDisposed = false;

    public override BlipSprite MapIcon { get; set; } = BlipSprite.CriminalCarsteal;
    public override Color MapIconColor { get; set; } = Color.White;
    public override string ButtonPromptText { get; set; }
    public ScrapYard() : base()
    {

    }
    public ScrapYard(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = $"Scrap Vehicle at {_Name}";
    }
    public override void OnInteract()
    {
        SetupMenu();
        SetupCamera();
        base.OnInteract();
        GameFiber.StartNew(delegate
        {
            ScrapMenu.Visible = true;
            Tick();
            Dispose();
        }, "Transaction");
    }
    private void SetupMenu()
    {
        MenuPool = new MenuPool();
        ScrapMenu = new UIMenu(Name, Description);
        if (BannerImage != "")
        {
            HasBannerImage = true;
            bannerTexture = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImage}");
            ScrapMenu.SetBannerType(bannerTexture);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        ScrapMenu.OnItemSelect += OnItemSelect;
        MenuPool.Add(ScrapMenu);
        ScrapVehicle = new UIMenuItem("Scrap Vehicle","Scraps the selected vehicle for the given amount");
        ScrapMenu.AddItem(ScrapVehicle);
        EntryPoint.WriteToConsole("Scrapyard: Setup Ran", 5);
    }
    private void SetupCamera()
    {

    }
    private void Tick()
    {
        while (!IsDisposed && MenuPool.IsAnyMenuOpen())
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole("Scrapyard Dispose 1", 5);
        Dispose();
        GameFiber.Sleep(1000);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == ScrapVehicle)
        {
            Game.DisplayHelp("You Selected Scrap Vehicle");
        }
    }
}

