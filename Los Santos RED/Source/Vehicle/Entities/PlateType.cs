using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PlateType
{
    public int Index { get; set; }
    public string Description { get; set; }
    public string State { get; set; }
    public int SpawnChance { get; set; }
    public bool CanOverwrite { get; set; } = true;
    public string SerialFormat { get; set; } = "";
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
    public PlateType(int index, string description, string state, int spawnChance)
    {
        Index = index;
        Description = description;
        State = state;
        SpawnChance = spawnChance;
    }
    public PlateType(int index, string description, string state, int spawnChance, string serialFormat) : this(index, description, state, spawnChance)
    {
        SerialFormat = serialFormat;
    }
    public string GenerateNewLicensePlateNumber()
    {     
        if (SerialFormat != "")
        {
            //Debugging.WriteToLog("PlateType", string.Format("State: {0}, Format: {1}",State, SerialFormat));
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
                    NewChar = General.RandomNumber();
                }
                else if (char.IsLetter(c))
                {
                    NewChar = General.RandomLetter();
                }
                NewPlateNumber += NewChar;
                
            }
            //Debugging.WriteToLog("PlateType", string.Format("State: {0}, PlateNumber: {1}", State, NewPlateNumber));
            return NewPlateNumber.ToUpper();
        }
        else
        {
            return "";
        }

    }
}

