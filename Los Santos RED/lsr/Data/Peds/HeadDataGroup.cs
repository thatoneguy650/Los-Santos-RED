using System;
using System.Collections.Generic;
[Serializable]
public class HeadDataGroup
{
    public HeadDataGroup()
    {

    }

    public HeadDataGroup(string headDataGroupID, List<RandomHeadData> headList)
    {
        HeadDataGroupID = headDataGroupID;
        HeadList = headList;
    }
    public string HeadDataGroupID { get; set; }
    public List<RandomHeadData> HeadList { get; set; } = new List<RandomHeadData>();
}