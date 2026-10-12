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
            ResetTractionLossMultiplier(vehicleBaseHandlingData);
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
        VehicleBaseHandlingData vehicleBaseHandlingData = new VehicleBaseHandlingData(modelHash);
        UpdateDeformationDamageMultiplier(modelHash, handlingData, vehicleBaseHandlingData);
        UpdateTractionLossMultiplier(modelHash, handlingData, vehicleBaseHandlingData);
        VehicleBaseHandlingDataList.Add(vehicleBaseHandlingData);
    }
    private void UpdateDeformationDamageMultiplier(uint modelHash, HandlingData handlingData, VehicleBaseHandlingData vehicleBaseHandlingData)
    {
        float vanillaDeformation = handlingData.DeformationDamageMultiplier;
        vehicleBaseHandlingData.VanillaDeformationDamageMultiplier = vanillaDeformation;
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



    private void UpdateTractionLossMultiplier(uint modelHash, HandlingData handlingData, VehicleBaseHandlingData vehicleBaseHandlingData)
    {
        float vanillaTractionLoss = handlingData.TractionLossMultiplier;
        vehicleBaseHandlingData.VanillaTractionLossMultiplier = vanillaTractionLoss;
        float updatedTractionLoss = vanillaTractionLoss * Settings.SettingsManager.VehicleSettings.VehicleTractionLossMultiplier;
        handlingData.TractionLossMultiplier = updatedTractionLoss;
        EntryPoint.WriteToConsole($"UPDATED TRACTION LOSS FOR {modelHash} Vanilla:{vanillaTractionLoss} Updated:{updatedTractionLoss}");
    }
    private void ResetTractionLossMultiplier(VehicleBaseHandlingData vehicleBaseHandlingData)
    {
        if (vehicleBaseHandlingData == null || vehicleBaseHandlingData.HandlingData == null)
        {
            return;
        }
        vehicleBaseHandlingData.HandlingData.TractionLossMultiplier = vehicleBaseHandlingData.VanillaTractionLossMultiplier;
        EntryPoint.WriteToConsole($"RESET TRACTION LOSS FOR {vehicleBaseHandlingData.ModelHash} Vanilla:{vehicleBaseHandlingData.VanillaTractionLossMultiplier}");
    }




}

