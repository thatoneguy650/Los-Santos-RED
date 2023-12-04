using System.ComponentModel;
using System.Runtime.Serialization;

public class PedLoadingSettings : ISettingsDefaultable
{
    [Description("")]

    public bool FadeOut { get; set; }
    public bool AllowLoadingBodies { get; set; }




    public float DefaultTrunkAttachXOffset { get; set; }
    public float DefaultTrunkAttachYOffset { get; set; }
    public float DefaultTrunkAttachZOffset { get; set; }



    public float DefaultTrunkAttachXRotation { get; set; }
    public float DefaultTrunkAttachRotation { get; set; }
    public float DefaultTrunkAttachZRotation { get; set; }






    public float DefaultBedLoadXOffset { get; set; }
    public float DefaultBedLoadYOffset { get; set; }
    public float DefaultBedLoadZOffset { get; set; }





    public int BoneIndex { get; set; }
    public bool UseBasicAttachIfPed { get; set; }
    public int Euler { get; set; }
    public bool OffsetIsRelative { get; set; }



    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        SetDefault();
    }
    public PedLoadingSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        AllowLoadingBodies = true;
        UseBasicAttachIfPed = false;
        Euler = 2;
        OffsetIsRelative = false;
        BoneIndex = 0;

        FadeOut = true;

        DefaultTrunkAttachYOffset = -0.75f;
        DefaultTrunkAttachZOffset = 0.3f;
        DefaultTrunkAttachZRotation = 180f;

        DefaultBedLoadYOffset = -0.75f;
        DefaultBedLoadZOffset = 1.25f;
    }
}