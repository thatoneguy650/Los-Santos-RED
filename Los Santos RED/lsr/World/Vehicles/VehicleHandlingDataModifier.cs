using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VehicleHandlingDataModifier
{
    private ISettingsProvideable Settings;
    private List<VehicleBaseHandlingData> VehicleBaseHandlingDataList = new List<VehicleBaseHandlingData>();

    public VehicleHandlingDataModifier(ISettingsProvideable settings)
    {
        Settings = settings;
    }
    public void Setup()
    {

    }
    public void Dispose()
    {
        foreach (VehicleBaseHandlingData vehicleBaseHandlingData in VehicleBaseHandlingDataList)
        {
            ResetDeformationDamageMultiplier(vehicleBaseHandlingData);
        }
        VehicleBaseHandlingDataList.Clear();
    }
    public void CheckVehicle(Vehicle vehicle)
    {
        if(!vehicle.Exists() || !EntryPoint.ModController.IsRunning)
        {
            return;
        }
        uint modelHash = vehicle.Model.Hash;
        if (VehicleBaseHandlingDataList.Any(x=> x.ModelHash == modelHash))
        {
            return;
        }
        HandlingData handlingData = vehicle.HandlingData;
        UpdateDeformationDamageMultiplier(modelHash, handlingData);
    }
    private void UpdateDeformationDamageMultiplier(uint modelHash, HandlingData handlingData)
    {
        float vanillaDeformation = handlingData.DeformationDamageMultiplier;
        VehicleBaseHandlingDataList.Add(new VehicleBaseHandlingData(modelHash, vanillaDeformation) { HandlingData = handlingData });
        float updatedDeformation = vanillaDeformation * Settings.SettingsManager.VehicleSettings.VehicleDeformationMultiplier;
        handlingData.DeformationDamageMultiplier = updatedDeformation;
        EntryPoint.WriteToConsole($"UPDATED DEFORMATION FOR {modelHash} Vanilla:{vanillaDeformation} Updated:{updatedDeformation}");
    }
    private void ResetDeformationDamageMultiplier(VehicleBaseHandlingData vehicleBaseHandlingData)
    {
        if(vehicleBaseHandlingData == null || vehicleBaseHandlingData.HandlingData == null)
        {
            return;
        }
        vehicleBaseHandlingData.HandlingData.DeformationDamageMultiplier = vehicleBaseHandlingData.VanillaDeformationDamageMultiplier;
        EntryPoint.WriteToConsole($"RESET DEFORMATION FOR {vehicleBaseHandlingData.ModelHash} Vanilla:{vehicleBaseHandlingData.VanillaDeformationDamageMultiplier}");
    }


}

