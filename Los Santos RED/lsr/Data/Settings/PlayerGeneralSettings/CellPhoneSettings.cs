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

    public CellphoneSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        OverwriteVanillaEmergencyServicesContact = true;
        EmergencyServicesContactID = 10;
        CustomContactStartingID = 30;
    }
}