using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellphoneSettings : ISettingsDefaultable
{
    [Description("Overwrite the vanilla emergency contact in the phone. Will allow mod dispatch to respond.")]
    public bool OverwriteVanillaEmergencyServicesContact { get; set; }
    [Description("Index of the vanilla emergency contact to overwrite")]
    public int EmergencyServicesContactID { get; set; }
    [Description("Starting index for additional custom contacts, should be one more than the total contacts you have in your phone. If set higher, you will see blank entrys until you get to the index")]
    public int CustomContactStartingID { get; set; }
    [Description("Overwrite the text messages in your phone to be those from the mod. If disabled, you can still see the messages in the ~r~Player Information~s~ menu.")]
    public bool OverwriteVanillaTextMessages { get; set; }
    [Description("Adds contacts in your phone from the mod. If disabled, you can still see the contacts in the ~r~Player Information~s~ menu.")]
    public bool OverwriteVanillaContacts { get; set; }

    public CellphoneSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        OverwriteVanillaEmergencyServicesContact = true;
        EmergencyServicesContactID = 29;//29
        CustomContactStartingID = 30;

        OverwriteVanillaTextMessages = true;
        OverwriteVanillaContacts = true;
    }
}