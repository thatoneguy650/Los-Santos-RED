using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VehicleItemMakeAndCategories
{
    public VehicleItemMakeAndCategories()
    {
    }

    public VehicleItemMakeAndCategories(string makeName, List<string> typeCategories)
    {
        MakeName = makeName;
        TypeCategories = typeCategories;
    }

    public string MakeName { get; set; }
    public List<string> TypeCategories { get; set; } = new List<string>();
}