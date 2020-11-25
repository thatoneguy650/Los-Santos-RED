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

}

