using System;

namespace LosSantosRED.lsr
{
    public enum VehicleAssociationType
    {
        RegisteredOwner,
        CurrentDriver,
        PreviouslyDriven,
        SuspectedAssociated,
        ConfirmedByANPR
    }

    [Serializable]
    public class VehicleAssociationRecord
    {
        public string ID { get; set; }
        public string SubjectID { get; set; }
        public string VehicleID { get; set; }
        public VehicleAssociationType AssociationType { get; set; }
        public DateTime ObservedAt { get; set; }
        public string SourceID { get; set; }
        public bool IsKnownToPolice { get; set; }
        public float Confidence { get; set; }
        public string Notes { get; set; }

        public VehicleAssociationRecord Copy()
        {
            return (VehicleAssociationRecord)MemberwiseClone();
        }
    }
}
