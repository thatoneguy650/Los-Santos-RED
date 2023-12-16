using LosSantosRED.lsr.Helper;
using Rage;

public class VehicleNameSelect
{
    public VehicleNameSelect()
    {
    }

    public VehicleNameSelect(string modelName)
    {
        ModelName = modelName;
    }
    public void UpdateItems()
    {
        VehicleMakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(ModelName.ToLower()));
        VehicleModelName = NativeHelper.VehicleModelName(Game.GetHashKey(ModelName.ToLower()));
    }
    public string ModelName { get; set; }
    public string VehicleModelName { get; set; }
    public string VehicleMakeName { get; set; }
    public DispatchableVehicle DispatchableVehicle { get; set; }
    public override string ToString()
    {
        string toReturn = "";
        if (string.IsNullOrEmpty(VehicleMakeName) && string.IsNullOrEmpty(VehicleModelName))
        {
            toReturn = ModelName;
        }
        else
        {
            if (!string.IsNullOrEmpty(VehicleMakeName) && !string.IsNullOrEmpty(VehicleModelName))
            {
                toReturn = VehicleMakeName + " " + VehicleModelName;
            }
            else if (!string.IsNullOrEmpty(VehicleModelName))
            {
                toReturn = VehicleModelName;
            }
            else
            {
                toReturn = ModelName;
            }
        }
        return toReturn;
    }
}