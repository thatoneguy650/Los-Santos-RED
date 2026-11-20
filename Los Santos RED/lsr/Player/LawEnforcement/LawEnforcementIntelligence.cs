using System;
using System.Collections.Generic;

namespace LosSantosRED.lsr
{
    // Persistent, player-specific law-enforcement intelligence. This is deliberately
    // a data-only model: collection and decision-making are added in later phases.
    public class LawEnforcementIntelligence
    {
        public int SchemaVersion { get; set; } = 1;
        public string SubjectID { get; set; } = "PLAYER";
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public List<IntelligenceObservation> Observations { get; set; } = new List<IntelligenceObservation>();
        public List<InvestigationRecord> Investigations { get; set; } = new List<InvestigationRecord>();
        public List<WarrantRecord> Warrants { get; set; } = new List<WarrantRecord>();
        public List<BOLORecord> BOLOs { get; set; } = new List<BOLORecord>();

        public LawEnforcementIntelligence Copy()
        {
            LawEnforcementIntelligence copy = new LawEnforcementIntelligence
            {
                SchemaVersion = SchemaVersion,
                SubjectID = SubjectID,
                CreatedAt = CreatedAt,
                LastUpdatedAt = LastUpdatedAt
            };

            if (Observations != null)
            {
                foreach (IntelligenceObservation item in Observations)
                {
                    if (item != null) copy.Observations.Add(item.Copy());
                }
            }
            if (Investigations != null)
            {
                foreach (InvestigationRecord item in Investigations)
                {
                    if (item != null) copy.Investigations.Add(item.Copy());
                }
            }
            if (Warrants != null)
            {
                foreach (WarrantRecord item in Warrants)
                {
                    if (item != null) copy.Warrants.Add(item.Copy());
                }
            }
            if (BOLOs != null)
            {
                foreach (BOLORecord item in BOLOs)
                {
                    if (item != null) copy.BOLOs.Add(item.Copy());
                }
            }
            return copy;
        }

        public void Normalize()
        {
            if (SchemaVersion <= 0) SchemaVersion = 1;
            if (string.IsNullOrWhiteSpace(SubjectID)) SubjectID = "PLAYER";
            if (Observations == null) Observations = new List<IntelligenceObservation>();
            if (Investigations == null) Investigations = new List<InvestigationRecord>();
            if (Warrants == null) Warrants = new List<WarrantRecord>();
            if (BOLOs == null) BOLOs = new List<BOLORecord>();
        }
    }

    public enum IntelligenceSource
    {
        Unknown,
        PatrolOfficer,
        ANPR,
        Investigator,
        Dispatch,
        TacticalUnit,
        Witness,
        Informant,
        BusinessRecord,
        PlayerRecord
    }

    public enum IntelligenceEvidenceType
    {
        Unknown,
        VisualSighting,
        VehicleRegistration,
        LicensePlateRead,
        BusinessVisit,
        BusinessOwnership,
        WitnessStatement,
        InformantStatement,
        OfficialRecord,
        CrimeSceneEvidence,
        Surveillance
    }

    public enum IntelligenceTargetType
    {
        Unknown,
        Person,
        Vehicle,
        LicensePlate,
        Business,
        Location,
        Associate
    }

    public class IntelligenceObservation
    {
        public string ID { get; set; }
        public DateTime ObservedAt { get; set; }
        public IntelligenceSource Source { get; set; }
        public IntelligenceEvidenceType EvidenceType { get; set; }
        public IntelligenceTargetType TargetType { get; set; }
        public string TargetID { get; set; }
        public string TargetDescription { get; set; }
        public string LocationID { get; set; }
        public string LocationName { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        // Confidence is normalized to the inclusive range 0.0 - 1.0.
        public float Confidence { get; set; }
        public bool IsVerified { get; set; }
        public string Notes { get; set; }

        public IntelligenceObservation Copy()
        {
            return new IntelligenceObservation
            {
                ID = ID,
                ObservedAt = ObservedAt,
                Source = Source,
                EvidenceType = EvidenceType,
                TargetType = TargetType,
                TargetID = TargetID,
                TargetDescription = TargetDescription,
                LocationID = LocationID,
                LocationName = LocationName,
                PositionX = PositionX,
                PositionY = PositionY,
                PositionZ = PositionZ,
                Confidence = Confidence,
                IsVerified = IsVerified,
                Notes = Notes
            };
        }
    }

    public enum InvestigationStage
    {
        Preliminary,
        Active,
        Focused,
        Intensive,
        Critical,
        Suspended,
        Closed
    }

    public class InvestigationRecord
    {
        public string ID { get; set; }
        public string SubjectID { get; set; }
        public InvestigationStage Stage { get; set; }
        public int HeatLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public List<string> ObservationIDs { get; set; } = new List<string>();
        public List<string> WarrantIDs { get; set; } = new List<string>();
        public List<string> BOLOIDs { get; set; } = new List<string>();
        public List<string> KnownVehicleIDs { get; set; } = new List<string>();
        public List<string> KnownLocationIDs { get; set; } = new List<string>();
        public List<string> AssociateIDs { get; set; } = new List<string>();
        public string AssignedAgencyID { get; set; }
        public string Notes { get; set; }

        public InvestigationRecord Copy()
        {
            return new InvestigationRecord
            {
                ID = ID,
                SubjectID = SubjectID,
                Stage = Stage,
                HeatLevel = HeatLevel,
                CreatedAt = CreatedAt,
                LastActivityAt = LastActivityAt,
                ObservationIDs = CopyStrings(ObservationIDs),
                WarrantIDs = CopyStrings(WarrantIDs),
                BOLOIDs = CopyStrings(BOLOIDs),
                KnownVehicleIDs = CopyStrings(KnownVehicleIDs),
                KnownLocationIDs = CopyStrings(KnownLocationIDs),
                AssociateIDs = CopyStrings(AssociateIDs),
                AssignedAgencyID = AssignedAgencyID,
                Notes = Notes
            };
        }

        private static List<string> CopyStrings(List<string> source)
        {
            return source == null ? new List<string>() : new List<string>(source);
        }
    }

    public enum WarrantType
    {
        Arrest,
        Search,
        Bench,
        ProbationParole,
        Extradition
    }

    public enum WarrantStatus
    {
        Active,
        Served,
        Cleared,
        Expired,
        Suspended
    }

    public enum WarrantPriority
    {
        Low,
        Moderate,
        High,
        Critical
    }

    public class WarrantRecord
    {
        public string ID { get; set; }
        public string SubjectID { get; set; }
        public WarrantType Type { get; set; }
        public WarrantStatus Status { get; set; }
        public WarrantPriority Priority { get; set; }
        public string IssuingAgencyID { get; set; }
        public string JurisdictionID { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public List<string> LinkedCrimeIDs { get; set; } = new List<string>();
        public float ResidenceConfidence { get; set; }
        public string SuspectedResidenceID { get; set; }
        public string ResolutionNotes { get; set; }

        public WarrantRecord Copy()
        {
            return new WarrantRecord
            {
                ID = ID,
                SubjectID = SubjectID,
                Type = Type,
                Status = Status,
                Priority = Priority,
                IssuingAgencyID = IssuingAgencyID,
                JurisdictionID = JurisdictionID,
                IssuedAt = IssuedAt,
                ResolvedAt = ResolvedAt,
                LinkedCrimeIDs = LinkedCrimeIDs == null ? new List<string>() : new List<string>(LinkedCrimeIDs),
                ResidenceConfidence = ResidenceConfidence,
                SuspectedResidenceID = SuspectedResidenceID,
                ResolutionNotes = ResolutionNotes
            };
        }
    }

    public enum BOLOTargetType
    {
        Person,
        Vehicle,
        LicensePlate
    }

    public enum BOLOStatus
    {
        Active,
        Cancelled,
        Expired,
        Resolved
    }

    public class BOLORecord
    {
        public string ID { get; set; }
        public string SubjectID { get; set; }
        public BOLOTargetType TargetType { get; set; }
        public BOLOStatus Status { get; set; }
        public string Description { get; set; }
        public string VehicleModel { get; set; }
        public string PlateNumber { get; set; }
        public string ZoneID { get; set; }
        public string LastKnownLocationID { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public float StopChanceInZone { get; set; }
        public float StopChanceOutsideZone { get; set; }
        public string Notes { get; set; }

        public BOLORecord Copy()
        {
            return new BOLORecord
            {
                ID = ID,
                SubjectID = SubjectID,
                TargetType = TargetType,
                Status = Status,
                Description = Description,
                VehicleModel = VehicleModel,
                PlateNumber = PlateNumber,
                ZoneID = ZoneID,
                LastKnownLocationID = LastKnownLocationID,
                IssuedAt = IssuedAt,
                ExpiresAt = ExpiresAt,
                StopChanceInZone = StopChanceInZone,
                StopChanceOutsideZone = StopChanceOutsideZone,
                Notes = Notes
            };
        }
    }
}
