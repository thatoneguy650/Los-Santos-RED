using System.Collections.Generic;

public class ZoneLookup
{
    public string InternalGameName { get; set; }
    public string ScannerValue { get; set; } = "";
    public List<string> ScannerUnitValues { get; set; } = new List<string>();
    public ZoneLookup()
    {

    }
    public ZoneLookup(string _GameName, string _ScannerValue)
    {
        InternalGameName = _GameName;
        ScannerValue = _ScannerValue;
    }
    public ZoneLookup(string _GameName, string _ScannerValue, string _ScannerUnitValue)
    {
        InternalGameName = _GameName;
        ScannerValue = _ScannerValue;
        ScannerUnitValues = new List<string>() { _ScannerUnitValue };
    }
    public ZoneLookup(string _GameName, string _ScannerValue, List<string> scannerUnitValues)
    {
        InternalGameName = _GameName;
        ScannerValue = _ScannerValue;
        ScannerUnitValues = scannerUnitValues;
    }
}