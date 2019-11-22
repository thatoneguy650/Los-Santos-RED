using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Agency
{
    public string ColorPrefix = "~s~";
    public string Initials;
    public string FullName;
    public List<string> Models;
    public Color AgencyColor = Color.White;
    public Agency()
    {

    }
    public Agency(string _ColorPrefix,string _Initials,string _FullName, List<string> _Models,Color _AgencyColor)
    {
        ColorPrefix = _ColorPrefix;
        Initials = _Initials;
        FullName = _FullName;
        Models = _Models;
        AgencyColor = _AgencyColor;
    }
}

