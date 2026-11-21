using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<SurveillanceAssignment> SurveillanceAssignments { get; set; } = new List<SurveillanceAssignment>();
        public List<PoliceOperationRecord> PoliceOperations { get; set; } = new List<PoliceOperationRecord>();
        public List<VehicleAssociationRecord> VehicleAssociations { get; set; } = new List<VehicleAssociationRecord>();

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
            if (SurveillanceAssignments != null)
            {
                foreach (SurveillanceAssignment item in SurveillanceAssignments)
                {
                    if (item != null) copy.SurveillanceAssignments.Add(item.Copy());
                }
            }
            if (PoliceOperations != null)
            {
                foreach (PoliceOperationRecord item in PoliceOperations)
                {
                    if (item != null) copy.PoliceOperations.Add(item.Copy());
                }
            }
            if (VehicleAssociations != null)
            {
                foreach (VehicleAssociationRecord item in VehicleAssociations)
                {
                    if (item != null) copy.VehicleAssociations.Add(item.Copy());
                }
            }
            return copy;
        }

        public bool RecordPoliceSighting(DateTime observedAt, string officerID, bool inVehicle, string vehicleID, string vehicleDescription, float x, float y, float z, string locationID = null, string locationName = null)
        {
            string sourceID = "COP:" + (officerID ?? "UNKNOWN");
            bool recorded = RecordObservation(new IntelligenceObservation
            {
                ObservedAt = observedAt, Source = IntelligenceSource.PatrolOfficer,
                EvidenceType = IntelligenceEvidenceType.VisualSighting, TargetType = IntelligenceTargetType.Person,
                TargetID = SubjectID, TargetDescription = "Player", SourceID = sourceID, EntityID = officerID,
                IsKnownToPolice = true, Confidence = 0.85f, PositionX = x, PositionY = y, PositionZ = z,
                LocationID = locationID, LocationName = locationName
            });
            if (inVehicle && !string.IsNullOrWhiteSpace(vehicleID))
            {
                RecordObservation(new IntelligenceObservation
                {
                    ObservedAt = observedAt, Source = IntelligenceSource.PatrolOfficer,
                    EvidenceType = IntelligenceEvidenceType.VisualSighting, TargetType = IntelligenceTargetType.Vehicle,
                    TargetID = vehicleID, TargetDescription = vehicleDescription, SourceID = sourceID, EntityID = officerID,
                    IsKnownToPolice = true, Confidence = 0.8f, PositionX = x, PositionY = y, PositionZ = z,
                    LocationID = locationID, LocationName = locationName
                });
            }
            if (inVehicle && !string.IsNullOrWhiteSpace(vehicleID))
                RecordVehicleAssociation(observedAt, vehicleID, VehicleAssociationType.CurrentDriver,
                    sourceID, 0.8f, true, "Player was visibly driving this vehicle when observed by patrol.");
            return recorded;
        }

        public bool RecordANPRRead(DateTime observedAt, string officerID, string plateNumber, string registrationID, string vehicleDescription, float x, float y, float z, string locationID = null, string locationName = null)
        {
            if (string.IsNullOrWhiteSpace(plateNumber)) return false;
            bool recorded = RecordObservation(new IntelligenceObservation
            {
                ObservedAt = observedAt, Source = IntelligenceSource.ANPR,
                EvidenceType = IntelligenceEvidenceType.LicensePlateRead, TargetType = IntelligenceTargetType.LicensePlate,
                TargetID = plateNumber.Trim().ToUpperInvariant(), TargetDescription = vehicleDescription,
                SourceID = "ANPR:" + (officerID ?? "UNKNOWN"), EntityID = registrationID,
                IsKnownToPolice = true, Confidence = 0.98f, PositionX = x, PositionY = y, PositionZ = z,
                LocationID = locationID, LocationName = locationName,
                Notes = "Plate read by an active police ANPR scan."
            });
            if (recorded)
            {
                string vehicleID = !string.IsNullOrWhiteSpace(registrationID)
                    ? "REG:" + registrationID : "PLATE:" + plateNumber.Trim().ToUpperInvariant();
                RecordVehicleAssociation(observedAt, vehicleID, VehicleAssociationType.ConfirmedByANPR,
                    "ANPR:" + (officerID ?? "UNKNOWN"), 0.98f, true,
                    "Vehicle and displayed plate were confirmed by an active police ANPR scan while the player was visibly driving.");
            }
            return recorded;
        }

        public bool RecordVehicleAssociation(DateTime observedAt, string vehicleID,
            VehicleAssociationType associationType, string sourceID, float confidence,
            bool isKnownToPolice, string notes)
        {
            if (observedAt == DateTime.MinValue || string.IsNullOrWhiteSpace(vehicleID)) return false;
            Normalize();
            VehicleAssociationRecord previous = VehicleAssociations.FirstOrDefault(x => x != null &&
                x.VehicleID == vehicleID && x.AssociationType == associationType &&
                x.SourceID == sourceID && x.ObservedAt <= observedAt &&
                observedAt - x.ObservedAt < TimeSpan.FromMinutes(5));
            if (previous != null) return false;
            VehicleAssociations.Add(new VehicleAssociationRecord
            {
                ID = Guid.NewGuid().ToString("N"),
                SubjectID = SubjectID,
                VehicleID = vehicleID,
                AssociationType = associationType,
                ObservedAt = observedAt,
                SourceID = string.IsNullOrWhiteSpace(sourceID) ? "UNKNOWN" : sourceID,
                IsKnownToPolice = isKnownToPolice,
                Confidence = NormalizeProbability(confidence),
                Notes = notes
            });
            LastUpdatedAt = observedAt;
            return true;
        }

        // Activity records are not automatically police-known; a later evidence source must establish access.
        public bool RecordBusinessVisit(DateTime observedAt, string businessName, float x, float y, float z)
        {
            return RecordBusinessActivity(observedAt, businessName, x, y, z, IntelligenceEvidenceType.BusinessVisit, "Player visited this business.");
        }

        public bool RecordBusinessOwnership(DateTime observedAt, string businessName, float x, float y, float z, bool acquired)
        {
            return RecordBusinessActivity(observedAt, businessName, x, y, z, IntelligenceEvidenceType.BusinessOwnership,
                acquired ? "Player acquired this business." : "Player ceased owning this business.");
        }

        private bool RecordBusinessActivity(DateTime observedAt, string businessName, float x, float y, float z, IntelligenceEvidenceType evidenceType, string notes)
        {
            if (string.IsNullOrWhiteSpace(businessName)) return false;
            string businessID = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "BUSINESS:{0}:{1:0.00}:{2:0.00}:{3:0.00}", businessName.Trim(), x, y, z);
            return RecordObservation(new IntelligenceObservation
            {
                ObservedAt = observedAt, Source = IntelligenceSource.PlayerRecord, EvidenceType = evidenceType,
                TargetType = IntelligenceTargetType.Business, TargetID = businessID, TargetDescription = businessName,
                SourceID = "PLAYER_ACTIVITY", IsKnownToPolice = false, Confidence = 1.0f,
                PositionX = x, PositionY = y, PositionZ = z, LocationID = businessID,
                LocationName = businessName, Notes = notes
            });
        }

        public bool RecordObservation(IntelligenceObservation observation)
        {
            if (observation == null || observation.ObservedAt == DateTime.MinValue) return false;
            Normalize();
            if (string.IsNullOrWhiteSpace(observation.ID)) observation.ID = Guid.NewGuid().ToString("N");
            if (string.IsNullOrWhiteSpace(observation.SourceID)) observation.SourceID = "UNKNOWN";
            observation.Confidence = NormalizeProbability(observation.Confidence);
            IntelligenceObservation previous = Observations
                .Where(x => x != null && x.SourceID == observation.SourceID && x.EvidenceType == observation.EvidenceType &&
                    x.TargetType == observation.TargetType && x.TargetID == observation.TargetID && x.Notes == observation.Notes)
                .OrderByDescending(x => x.ObservedAt).FirstOrDefault();
            if (previous != null && observation.ObservedAt >= previous.ObservedAt &&
                observation.ObservedAt - previous.ObservedAt < TimeSpan.FromMinutes(5)) return false;

            Observations.Add(observation);
            if (observation.IsKnownToPolice)
            {
                foreach (WarrantRecord w in Warrants.Where(x => x != null && x.Status == WarrantStatus.Active && observation.ObservedAt >= x.IssuedAt))
                {
                    if (w.EvidenceObservationIDs == null) w.EvidenceObservationIDs = new List<string>();
                    if (!w.EvidenceObservationIDs.Contains(observation.ID)) w.EvidenceObservationIDs.Add(observation.ID);
                }
            }
            LastUpdatedAt = observation.ObservedAt;
            if (CreatedAt == DateTime.MinValue) CreatedAt = observation.ObservedAt;
            if (Observations.Count > 5000)
                Observations = Observations.Where(x => x != null).OrderByDescending(x => x.ObservedAt)
                    .Take(5000).OrderBy(x => x.ObservedAt).ToList();
            return true;
        }


        public WarrantRecord IssueWarrant(DateTime issuedAt, WarrantType type, WarrantPriority priority, string issuingAgencyID, string jurisdictionID, IEnumerable<string> linkedCrimeIDs, IEnumerable<string> ownedBusinessIDsAtIssuance, string suspectedResidenceID = null, float residenceConfidence = 0.0f)
        {
            if (issuedAt == DateTime.MinValue) return null;
            Normalize();
            WarrantRecord w = new WarrantRecord { ID = Guid.NewGuid().ToString("N"), SubjectID = SubjectID, Type = type, Status = WarrantStatus.Active, Priority = priority, IssuingAgencyID = issuingAgencyID, JurisdictionID = jurisdictionID, IssuedAt = issuedAt, LinkedCrimeIDs = linkedCrimeIDs == null ? new List<string>() : linkedCrimeIDs.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList(), KnownBusinessIDsAtIssuance = ownedBusinessIDsAtIssuance == null ? new List<string>() : ownedBusinessIDsAtIssuance.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList(), SuspectedResidenceID = suspectedResidenceID, ResidenceConfidence = NormalizeProbability(residenceConfidence) };
            Warrants.Add(w); LastUpdatedAt = issuedAt; return w;
        }
        public WarrantRecord GetActiveArrestWarrant()
        {
            if (Warrants == null) return null;
            return Warrants.Where(x => x != null && x.Status == WarrantStatus.Active &&
                (x.Type == WarrantType.Arrest || x.Type == WarrantType.Bench ||
                 x.Type == WarrantType.ProbationParole || x.Type == WarrantType.Extradition))
                .OrderByDescending(x => x.Priority).ThenBy(x => x.IssuedAt).FirstOrDefault();
        }

        public WarrantRecord GetActiveArrestWarrantForAgency(string agencyID, string jurisdictionID)
        {
            // Retained for source compatibility. Warrants are valid and discoverable across jurisdictions.
            return GetActiveArrestWarrant();
        }

        public bool ResolveWarrant(string id, WarrantStatus status, DateTime at, string notes)
        {
            if (status != WarrantStatus.Served && status != WarrantStatus.Cleared && status != WarrantStatus.Expired) return false;
            WarrantRecord w = Warrants.FirstOrDefault(x => x != null && x.ID == id && x.Status == WarrantStatus.Active);
            if (w == null || at == DateTime.MinValue || at < w.IssuedAt) return false;
            w.Status = status; w.ResolvedAt = at; w.ResolutionNotes = notes; LastUpdatedAt = at; return true;
        }
        public bool SetWarrantSuspended(string id, bool suspended, DateTime at)
        {
            WarrantRecord w = Warrants.FirstOrDefault(x => x != null && x.ID == id && (x.Status == WarrantStatus.Active || x.Status == WarrantStatus.Suspended));
            if (w == null || at == DateTime.MinValue) return false;
            w.Status = suspended ? WarrantStatus.Suspended : WarrantStatus.Active; LastUpdatedAt = at; return true;
        }
        public BOLORecord IssueBOLO(DateTime issuedAt, BOLOTargetType targetType, string description, string vehicleModel, string plateNumber, string zoneID, string lastKnownLocationID, DateTime? expiresAt, float stopChanceInZone, float stopChanceOutsideZone, string notes)
        {
            if (issuedAt == DateTime.MinValue || (expiresAt.HasValue && expiresAt.Value <= issuedAt)) return null;
            if (targetType == BOLOTargetType.LicensePlate && string.IsNullOrWhiteSpace(plateNumber)) return null;
            Normalize();
            BOLORecord b = new BOLORecord { ID = Guid.NewGuid().ToString("N"), SubjectID = SubjectID, TargetType = targetType, Status = BOLOStatus.Active, Description = description, VehicleModel = vehicleModel, PlateNumber = string.IsNullOrWhiteSpace(plateNumber) ? null : plateNumber.Trim().ToUpperInvariant(), ZoneID = zoneID, LastKnownLocationID = lastKnownLocationID, IssuedAt = issuedAt, ExpiresAt = expiresAt, StopChanceInZone = NormalizeProbability(stopChanceInZone), StopChanceOutsideZone = NormalizeProbability(stopChanceOutsideZone), Notes = notes };
            BOLOs.Add(b); LastUpdatedAt = issuedAt; return b;
        }

        public bool SetBOLOStatus(string id, BOLOStatus status, DateTime at)
        {
            if (status == BOLOStatus.Active || at == DateTime.MinValue) return false;
            BOLORecord b = BOLOs.FirstOrDefault(x => x != null && x.ID == id && x.Status == BOLOStatus.Active);
            if (b == null || at < b.IssuedAt) return false;
            b.Status = status; LastUpdatedAt = at; return true;
        }
        public void UpdateExpirations(DateTime at)
        {
            if (at == DateTime.MinValue) return;
            foreach (BOLORecord b in BOLOs.Where(x => x != null && x.Status == BOLOStatus.Active && x.ExpiresAt.HasValue && at >= x.ExpiresAt.Value))
                b.Status = BOLOStatus.Expired;
            LastUpdatedAt = at;
        }
        public float GetBOLOStopChance(DateTime at, string zoneID, string plateNumber, string vehicleModel)
        {
            UpdateExpirations(at);
            float chance = 0.0f;
            foreach (BOLORecord b in BOLOs.Where(x => x != null && x.Status == BOLOStatus.Active && x.SubjectID == SubjectID && x.TargetType != BOLOTargetType.Person))
            {
                if (b.TargetType == BOLOTargetType.LicensePlate &&
                    (string.IsNullOrWhiteSpace(plateNumber) || !string.Equals(b.PlateNumber, plateNumber.Trim(), StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (b.TargetType == BOLOTargetType.Vehicle && !string.IsNullOrWhiteSpace(b.VehicleModel) &&
                    !string.Equals(b.VehicleModel, vehicleModel, StringComparison.OrdinalIgnoreCase))
                    continue;
                float candidate = !string.IsNullOrWhiteSpace(b.ZoneID) && string.Equals(b.ZoneID, zoneID, StringComparison.OrdinalIgnoreCase) ? b.StopChanceInZone : b.StopChanceOutsideZone;
                if (candidate > chance) chance = candidate;
            }
            return Math.Max(0.0f, Math.Min(1.0f, chance));
        }

        public InvestigationRecord RefreshInvestigation(DateTime currentTime, string assignedAgencyID = null)
        {
            if (currentTime == DateTime.MinValue) return null;
            Normalize();
            InvestigationRecord investigation = Investigations.FirstOrDefault(x => x != null && x.SubjectID == SubjectID &&
                x.Stage != InvestigationStage.Closed && x.Stage != InvestigationStage.Suspended);
            if (investigation == null)
            {
                investigation = new InvestigationRecord
                {
                    ID = Guid.NewGuid().ToString("N"), SubjectID = SubjectID, Stage = InvestigationStage.Preliminary,
                    CreatedAt = currentTime, LastActivityAt = currentTime, AssignedAgencyID = assignedAgencyID
                };
                Investigations.Add(investigation);
            }
            if (investigation.ObservationIDs == null) investigation.ObservationIDs = new List<string>();
            if (investigation.KnownVehicleIDs == null) investigation.KnownVehicleIDs = new List<string>();
            if (investigation.KnownLocationIDs == null) investigation.KnownLocationIDs = new List<string>();
            List<IntelligenceObservation> evidence = Observations.Where(x => x != null && x.IsKnownToPolice && x.ObservedAt <= currentTime).ToList();
            foreach (IntelligenceObservation item in evidence)
            {
                if (!investigation.ObservationIDs.Contains(item.ID)) investigation.ObservationIDs.Add(item.ID);
                if (item.TargetType == IntelligenceTargetType.Vehicle && !string.IsNullOrWhiteSpace(item.TargetID) &&
                    !investigation.KnownVehicleIDs.Contains(item.TargetID)) investigation.KnownVehicleIDs.Add(item.TargetID);
                if (!string.IsNullOrWhiteSpace(item.LocationID) && !investigation.KnownLocationIDs.Contains(item.LocationID))
                    investigation.KnownLocationIDs.Add(item.LocationID);
            }
            int recentCount = evidence.Count(x => x.ObservedAt >= currentTime.AddDays(-7));
            investigation.HeatLevel = Math.Min(5, recentCount / 3);
            investigation.Stage = recentCount >= 15 ? InvestigationStage.Critical :
                recentCount >= 9 ? InvestigationStage.Intensive :
                recentCount >= 5 ? InvestigationStage.Focused :
                recentCount >= 2 ? InvestigationStage.Active : InvestigationStage.Preliminary;
            if (evidence.Count > 0) investigation.LastActivityAt = evidence.Max(x => x.ObservedAt);
            foreach (WarrantRecord warrant in Warrants.Where(x => x != null && x.Status == WarrantStatus.Active))
                if (!investigation.WarrantIDs.Contains(warrant.ID)) investigation.WarrantIDs.Add(warrant.ID);
            foreach (BOLORecord bolo in BOLOs.Where(x => x != null && x.Status == BOLOStatus.Active))
                if (!investigation.BOLOIDs.Contains(bolo.ID)) investigation.BOLOIDs.Add(bolo.ID);
            LastUpdatedAt = currentTime;
            return investigation;
        }

        private static float NormalizeProbability(float value, float fallback = 0.0f)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return fallback;
            return Math.Max(0.0f, Math.Min(1.0f, value));
        }

        public void Normalize()
        {
            if (SchemaVersion <= 0) SchemaVersion = 1;
            if (string.IsNullOrWhiteSpace(SubjectID)) SubjectID = "PLAYER";
            if (Observations == null) Observations = new List<IntelligenceObservation>();
            if (Investigations == null) Investigations = new List<InvestigationRecord>();
            if (Warrants == null) Warrants = new List<WarrantRecord>();
            if (BOLOs == null) BOLOs = new List<BOLORecord>();
            if (SurveillanceAssignments == null) SurveillanceAssignments = new List<SurveillanceAssignment>();
            if (PoliceOperations == null) PoliceOperations = new List<PoliceOperationRecord>();
            if (VehicleAssociations == null) VehicleAssociations = new List<VehicleAssociationRecord>();
            foreach (VehicleAssociationRecord association in VehicleAssociations.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(association.ID)) association.ID = Guid.NewGuid().ToString("N");
                if (string.IsNullOrWhiteSpace(association.SubjectID)) association.SubjectID = SubjectID;
                association.Confidence = NormalizeProbability(association.Confidence);
            }
            foreach (SurveillanceAssignment assignment in SurveillanceAssignments.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(assignment.ID)) assignment.ID = Guid.NewGuid().ToString("N");
                if (string.IsNullOrWhiteSpace(assignment.TargetSubjectID)) assignment.TargetSubjectID = SubjectID;
            }
            foreach (PoliceOperationRecord operation in PoliceOperations.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(operation.ID)) operation.ID = Guid.NewGuid().ToString("N");
            }

            // Older or partially written saves may contain records whose nested lists are null.
            foreach (InvestigationRecord investigation in Investigations.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(investigation.ID)) investigation.ID = Guid.NewGuid().ToString("N");
                if (investigation.ObservationIDs == null) investigation.ObservationIDs = new List<string>();
                if (investigation.WarrantIDs == null) investigation.WarrantIDs = new List<string>();
                if (investigation.BOLOIDs == null) investigation.BOLOIDs = new List<string>();
                if (investigation.KnownVehicleIDs == null) investigation.KnownVehicleIDs = new List<string>();
                if (investigation.KnownLocationIDs == null) investigation.KnownLocationIDs = new List<string>();
                if (investigation.AssociateIDs == null) investigation.AssociateIDs = new List<string>();
                if (string.IsNullOrWhiteSpace(investigation.SubjectID)) investigation.SubjectID = SubjectID;
                investigation.HeatLevel = Math.Max(0, Math.Min(5, investigation.HeatLevel));
            }
            foreach (WarrantRecord warrant in Warrants.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(warrant.ID)) warrant.ID = Guid.NewGuid().ToString("N");
                if (string.IsNullOrWhiteSpace(warrant.SubjectID)) warrant.SubjectID = SubjectID;
                if (warrant.LinkedCrimeIDs == null) warrant.LinkedCrimeIDs = new List<string>();
                if (warrant.EvidenceObservationIDs == null) warrant.EvidenceObservationIDs = new List<string>();
                if (warrant.KnownBusinessIDsAtIssuance == null) warrant.KnownBusinessIDsAtIssuance = new List<string>();
                warrant.ResidenceConfidence = NormalizeProbability(warrant.ResidenceConfidence);
            }
            foreach (BOLORecord bolo in BOLOs.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(bolo.ID)) bolo.ID = Guid.NewGuid().ToString("N");
                if (string.IsNullOrWhiteSpace(bolo.SubjectID)) bolo.SubjectID = SubjectID;
                bolo.StopChanceInZone = NormalizeProbability(bolo.StopChanceInZone);
                bolo.StopChanceOutsideZone = NormalizeProbability(bolo.StopChanceOutsideZone);
            }
            foreach (IntelligenceObservation observation in Observations.Where(x => x != null))
            {
                if (string.IsNullOrWhiteSpace(observation.ID)) observation.ID = Guid.NewGuid().ToString("N");
                if (string.IsNullOrWhiteSpace(observation.SourceID)) observation.SourceID = "UNKNOWN";
                observation.Confidence = NormalizeProbability(observation.Confidence);
            }
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
        public string SourceID { get; set; }
        public string EntityID { get; set; }
        public bool IsKnownToPolice { get; set; }
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
                SourceID = SourceID,
                EntityID = EntityID,
                IsKnownToPolice = IsKnownToPolice,
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
        public DateTime? ExpiresAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public List<string> LinkedCrimeIDs { get; set; } = new List<string>();
        public List<string> EvidenceObservationIDs { get; set; } = new List<string>();
        public List<string> KnownBusinessIDsAtIssuance { get; set; } = new List<string>();
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
                ExpiresAt = ExpiresAt,
                ResolvedAt = ResolvedAt,
                LinkedCrimeIDs = LinkedCrimeIDs == null ? new List<string>() : new List<string>(LinkedCrimeIDs),
                EvidenceObservationIDs = EvidenceObservationIDs == null ? new List<string>() : new List<string>(EvidenceObservationIDs),
                KnownBusinessIDsAtIssuance = KnownBusinessIDsAtIssuance == null ? new List<string>() : new List<string>(KnownBusinessIDsAtIssuance),
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
