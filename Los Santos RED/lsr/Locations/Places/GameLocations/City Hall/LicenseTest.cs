using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LicenseTest
{
    private UIMenuItem DriversLicenseMenu;
    public LicenseTest()
    {
    }

    public LicenseTest(string name, List<LicenseQuestion> questions)
    {
        Name = name;
        Questions = questions;
    }

    public string Name { get; set; }
    public List<LicenseQuestion> Questions { get; set; } = new List<LicenseQuestion>();
    public void StartTest(UIMenuItem driversLicenseMenu)
    {
        DriversLicenseMenu = driversLicenseMenu;




    }
}

