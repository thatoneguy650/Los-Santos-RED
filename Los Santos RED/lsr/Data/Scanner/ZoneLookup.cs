public class ZoneLookup
{
    public string InternalGameName { get; set; }
    public string ScannerValue { get; set; } = "";
    public string ScannerUnitValue { get; set; } = "";
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
        ScannerUnitValue = _ScannerUnitValue;
    }
}