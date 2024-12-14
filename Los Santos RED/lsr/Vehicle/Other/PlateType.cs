using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PlateType
{
    public int Index { get; set; }
    public string Description { get; set; }
    public string StateID { get; set; }
    public int SpawnChance { get; set; }
    public bool CanOverwrite { get; set; } = true;
    public string SerialFormat { get; set; } = "12ABC345";
    public int Order { get; set; }
    public bool DisablePrefix { get; set; }
    public int InStateSpawnChance { get; set; }
    public bool CanSpawn
    {
        get
        {
            if(SpawnChance > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public PlateType()
    {
    }
    public PlateType(int index, string description, string stateID, int spawnChance)
    {
        Index = index;
        Description = description;
        StateID = stateID;
        SpawnChance = spawnChance;
    }
    public PlateType(int index, string description, string stateID, int spawnChance, string serialFormat) : this(index, description, stateID, spawnChance)
    {
        SerialFormat = serialFormat;
    }
    public string GenerateNewLicensePlateNumber()
    {     
        if (SerialFormat != "")
        {
            string NewPlateNumber = "";
            foreach (char c in SerialFormat)
            {
                char NewChar = c;
                if (c == Convert.ToChar(" "))
                {
                    NewChar = Convert.ToChar(" ");
                }
                else if (char.IsDigit(c))
                {
                    NewChar = RandomItems.RandomNumber();
                }
                else if (char.IsLetter(c))
                {
                    NewChar = RandomItems.RandomLetter();
                }
                NewPlateNumber += NewChar;
                
            }
            return NewPlateNumber.ToUpper();
        }
        else
        {
            return "";
        }

    }
    public override string ToString()
    {
        return Description;
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        Order = 0;
        InStateSpawnChance = 100;
    }
}

