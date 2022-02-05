using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GangDen : TransactableLocation
{
    private StoreCamera StoreCamera;
    private bool CanInteractWithLocation = true;
    public GangDen() : base()
    {

    }
    public override BlipSprite MapIcon { get; set; } = BlipSprite.Shrink;
    public override Color MapIconColor { get; set; } = Color.White;
    public override string ButtonPromptText { get; set; }
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }
    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, ShopMenu shopMenu, Gang _gang) : base(_EntrancePosition, _EntranceHeading, _Name, _Description, shopMenu)
    {
        AssociatedGang = _gang;
        ButtonPromptText = $"Enter {AssociatedGang.ShortName} {AssociatedGang.DenName}";
    }
    public override void OnInteract(IActivityPerformable Player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        if (CanInteractWithLocation)
        {
            Player.IsInteractingWithLocation = true;
            CanInteractWithLocation = false;
            GameFiber.StartNew(delegate
            {
                StoreCamera = new StoreCamera(this, Player);
                StoreCamera.Setup();



                StoreTransaction st = new StoreTransaction(Player, this, StoreCamera.StoreCam, modItems, world, settings, weapons, time);

                st.Start();








                StoreCamera.Dispose();
                Player.IsInteractingWithLocation = false;
            }, "GangDenInteract");       

        }
        //base.OnInteract(player);
    }
}

