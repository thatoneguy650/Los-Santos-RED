using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr
{
    // Query/ingestion facade over the single persistent intelligence store.
    public class IntelligenceDatabase
    {
        public bool Record(LawEnforcementIntelligence intelligence, IntelligenceObservation observation)
        {
            return intelligence != null && observation != null && intelligence.RecordObservation(observation);
        }

        public IEnumerable<IntelligenceObservation> GetPoliceKnown(LawEnforcementIntelligence intelligence,
            DateTime throughTime, string targetID = null)
        {
            if (intelligence == null || intelligence.Observations == null)
                return Enumerable.Empty<IntelligenceObservation>();
            return intelligence.Observations.Where(x => x != null && x.IsKnownToPolice &&
                x.ObservedAt <= throughTime && (string.IsNullOrWhiteSpace(targetID) ||
                string.Equals(x.TargetID, targetID, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(x => x.ObservedAt).ToList();
        }

        public IEnumerable<IntelligenceObservation> GetEvidenceSince(LawEnforcementIntelligence intelligence,
            DateTime since, bool policeKnownOnly = true)
        {
            if (intelligence == null || intelligence.Observations == null)
                return Enumerable.Empty<IntelligenceObservation>();
            return intelligence.Observations.Where(x => x != null && x.ObservedAt >= since &&
                (!policeKnownOnly || x.IsKnownToPolice)).OrderBy(x => x.ObservedAt).ToList();
        }

        public bool MarkKnownToPolice(LawEnforcementIntelligence intelligence, string observationID,
            IntelligenceSource source, string sourceID, float confidence, DateTime recordedAt, string notes = null)
        {
            if (intelligence == null || intelligence.Observations == null ||
                string.IsNullOrWhiteSpace(observationID) || recordedAt == DateTime.MinValue) return false;
            IntelligenceObservation observation = intelligence.Observations.FirstOrDefault(x =>
                x != null && string.Equals(x.ID, observationID, StringComparison.OrdinalIgnoreCase));
            if (observation == null || recordedAt < observation.ObservedAt) return false;
            observation.IsKnownToPolice = true;
            observation.Source = source;
            observation.SourceID = string.IsNullOrWhiteSpace(sourceID) ? "UNKNOWN" : sourceID;
            observation.Confidence = float.IsNaN(confidence) || float.IsInfinity(confidence)
                ? 0f : Math.Max(0f, Math.Min(1f, confidence));
            observation.Notes = string.IsNullOrWhiteSpace(notes) ? observation.Notes : notes;
            observation.IsVerified = true;
            intelligence.LastUpdatedAt = recordedAt;
            return true;
        }
    }
}
