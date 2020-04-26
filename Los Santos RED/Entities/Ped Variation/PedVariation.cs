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

    public PedVariation(List<PropComponent> _MyPedProps)
    {
        MyPedProps = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents)
    {
        MyPedComponents = _MyPedComponents;
    }

    public PedVariation(List<PedComponent> _MyPedComponents, List<PropComponent> _MyPedProps)
    {
        MyPedComponents = _MyPedComponents;
        MyPedProps = _MyPedProps;
    }
    public List<PedComponent> MyPedComponents = new List<PedComponent>();
    public List<PropComponent> MyPedProps = new List<PropComponent>();

}

