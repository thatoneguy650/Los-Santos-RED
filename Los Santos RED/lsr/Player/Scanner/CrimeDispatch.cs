public class CrimeDispatch
{
    public CrimeDispatch(string crimeID, Dispatch dispatchToPlay)
    {
        CrimeID = crimeID;
        Dispatch = dispatchToPlay;
    }
    public string CrimeID { get; set; }
    public Dispatch Dispatch { get; set; }
}