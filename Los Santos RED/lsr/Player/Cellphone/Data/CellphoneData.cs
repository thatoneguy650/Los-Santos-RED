using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellphoneData
{
    public CellphoneData()
    {
    }

    public CellphoneData(string modItemName, int phoneType, string phoneOS)
    {
        ModItemName = modItemName;
        PhoneType = phoneType;
        PhoneOS = phoneOS;
    }
    public string ID { get; set; }  
    public string ModItemName { get; set; }
    public List<CellphoneIDLookup> Themes { get; set; } = 
        new List<CellphoneIDLookup>() 
        { 
            new CellphoneIDLookup(1,"Blue"), 
            new CellphoneIDLookup(2,"Green"), 
            new CellphoneIDLookup(3,"Red"),
            new CellphoneIDLookup(4,"Orange"),
            new CellphoneIDLookup(5,"Gray"),
            new CellphoneIDLookup(6,"Purple"),
            new CellphoneIDLookup(7,"Pink"),
        };
    public List<CellphoneIDLookup> Backgrounds { get; set; } =
        new List<CellphoneIDLookup>()
        {
            new CellphoneIDLookup(0,"Default"),
            new CellphoneIDLookup(4,"Purple Glow"),
            new CellphoneIDLookup(5,"Green Squares"),
            new CellphoneIDLookup(6,"Orange Herringbone"),
            new CellphoneIDLookup(7,"Orange Halftone"),
            new CellphoneIDLookup(8,"Green Triangles"),
            new CellphoneIDLookup(9,"Green Shards"),
            new CellphoneIDLookup(10,"Blue Angles"),
            new CellphoneIDLookup(11,"Blue Shards"),
            new CellphoneIDLookup(12,"Blue Circles"),
            new CellphoneIDLookup(13,"Diamonds"),
            new CellphoneIDLookup(14,"Green Glow"),
            new CellphoneIDLookup(15,"Orange 8-Bit"),
            new CellphoneIDLookup(16,"Orange Triangles"),
            new CellphoneIDLookup(17,"Purple Tartan"),
        };
    public int PhoneType { get; set; }
    public string PhoneOS { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsRegular { get; set; } = true;
    public bool IsHistoric { get; set; } = false;
    public bool HasFlashlight { get; set; } = true;
    public int GetRandomBackground()
    {
       if(Backgrounds == null)
        {
            return 0;        
        }
        CellphoneIDLookup background = Backgrounds.PickRandom();
        if(background == null)
        {
            return 0;
        }
        return background.ID;
    }
    public int GetRandomTheme()
    {
        if (Themes == null)
        {
            return 0;
        }
        CellphoneIDLookup theme = Themes.PickRandom();
        if (theme == null)
        {
            return 0;
        }
        return theme.ID;
    }
}

