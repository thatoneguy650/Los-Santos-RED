using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CabinetData
{
    public Vector3 CabinetCameraPosition { get; set; }
    public Vector3 CabinetCameraDirection { get; set; }
    public Rotator CabinetCameraRotation { get; set; }
    public float TrophyHeading { get; set; } = 180f;
    public List<TrophySlot> Slots { get; set; }
}