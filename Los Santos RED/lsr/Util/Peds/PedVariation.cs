using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
   public class PedVariation
    {

        public PedVariation()
        {

        }

    public PedVariation(List<PedPropComponent> _MyPedProps)
    {
        MyPedProps = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents)
    {
        MyPedComponents = _MyPedComponents;
    }

    public PedVariation(List<PedComponent> _MyPedComponents, List<PedPropComponent> _MyPedProps)
    {
        MyPedComponents = _MyPedComponents;
        MyPedProps = _MyPedProps;
    }
    public List<PedComponent> MyPedComponents = new List<PedComponent>();
    public List<PedPropComponent> MyPedProps = new List<PedPropComponent>();
    public void ReplacePedComponentVariation(Ped myPed)
    {
        try
        {
            foreach (PedComponent Component in MyPedComponents)
            {
                NativeFunction.CallByName<uint>("SET_PED_COMPONENT_VARIATION", myPed, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            foreach (PedPropComponent Prop in MyPedProps)
            {
                NativeFunction.CallByName<uint>("SET_PED_PROP_INDEX", myPed, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
        }
        catch (Exception e)
        {
            Debug.Instance.WriteToLog("ReplacePedComponentVariation", "ReplacePedComponentVariation Error; " + e.Message);
        }
    }
}

