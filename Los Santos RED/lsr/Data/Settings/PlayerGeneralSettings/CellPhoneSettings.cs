using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellphoneSettings : ISettingsDefaultable
{
    [Description("Allow temporary termination of vanilla cellphone while burner is active.")]
    public bool AllowTerminateVanillaCellphoneScripts { get; set; }
    [Description("Terminate vanilla cell phone while LSR is active")]
    public bool TerminateVanillaCellphone { get; set; }
    //[Description("Overwrite the vanilla cell phone will LSR texts and contacts.")]
    //public bool OverwriteVanillaCellphone { get; set; }
    //[Description("Overwrite the vanilla emergency contact in the phone. Will allow mod dispatch to respond.")]
    //public bool OverwriteVanillaEmergencyServicesContact { get; set; }
    //[Description("Index of the vanilla emergency contact to overwrite")]
    //public int EmergencyServicesContactID { get; set; }
    //[Description("Starting index for additional custom contacts, should be one more than the total contacts you have in your phone. If set higher, you will see blank entrys until you get to the index")]
    //public int CustomContactStartingID { get; set; }
    [Description("Allow the use of a custom burner phone to interact with contacts and text messages.")]
    public bool AllowBurnerPhone { get; set; }
    [Description("Burner cell position on screen (X - Horizontal).")]
    public float BurnerCellPositionX { get; set; }
    [Description("Burner cell position on screen (Y - Vertical).")]
    public float BurnerCellPositionY { get; set; }
    [Description("Burner cell position on screen (Z - ???).")]
    public float BurnerCellPositionZ { get; set; }
    [Description("Burner cell scale.")]
    public float BurnerCellScale { get; set; }
    [Description("Type of phone to use as the burner. Choose 0-4. 0 - Default phone / Michael's phone, 1 - Trevor's phone, 2 - Franklin's phone, 3 - Unused police phone, 4 - Prologue phone.")]
    public int BurnerPhoneTypeID { get; set; }

    [Description("Choose burner phone scaleform. Choices: cellphone_ifruit, cellphone_facade, or cellphone_badget")]
    public string BurnerPhoneScaleformName { get; set; }
    [Description("Choose burner phone scaleform. Choices: cellphone_ifruit, cellphone_facade, or cellphone_badget")]


    public CellphoneSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {

        //OverwriteVanillaCellphone = false;
        //OverwriteVanillaEmergencyServicesContact = true;
        //EmergencyServicesContactID = 29;//29
        //CustomContactStartingID = 30;


        AllowBurnerPhone = true;
        BurnerCellPositionX = 99.62f;
        BurnerCellPositionY = -45.305f;
        BurnerCellPositionZ = -113f;
        BurnerCellScale = 500f;

        BurnerPhoneTypeID = 0;
        AllowTerminateVanillaCellphoneScripts = true;
        BurnerPhoneScaleformName = "cellphone_ifruit";
        TerminateVanillaCellphone = true;
    }
}