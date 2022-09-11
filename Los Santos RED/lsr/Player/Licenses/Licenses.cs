using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Licenses
{
    ILicenseable Player;

    public Licenses(ILicenseable player)
    {
        Player = player;
    }
    public DriversLicense DriversLicense { get; set; }
    public CCWLicense CCWLicense { get; set; }
    public bool HasDriversLicense => DriversLicense != null;
    public bool HasCCWLicense => CCWLicense != null;
    public bool HasValidCCWLicense(ITimeReportable time) => HasCCWLicense && CCWLicense.IsValid(time);
    public bool HasValidDriversLicense(ITimeReportable time) => HasDriversLicense && DriversLicense.IsValid(time);
}

