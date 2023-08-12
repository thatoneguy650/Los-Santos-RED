using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ExportedVehicle
{
    public ExportedVehicle(MenuItem menuItem, int totalExports, DateTime timeLastExported)
    {
        MenuItem = menuItem;
        TotalExports = totalExports;
        TimeLastExported = timeLastExported;
    }

    public MenuItem MenuItem { get; set; }
    public int TotalExports { get; set; }
    public DateTime TimeLastExported { get; set; }
}

