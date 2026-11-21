using System;
using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr
{
    // Warrant operations are centralized here; the intelligence object remains the sole record owner.
    public class WarrantManager
    {
        public WarrantRecord Issue(LawEnforcementIntelligence intelligence, DateTime issuedAt, WarrantType type,
            WarrantPriority priority, string agencyID, string jurisdictionID, IEnumerable<string> crimeIDs,
            IEnumerable<string> ownedBusinessIDs, string residenceID = null, float residenceConfidence = 0f)
        {
            if (intelligence == null || issuedAt == DateTime.MinValue) return null;
            intelligence.Normalize();
            return intelligence.IssueWarrant(issuedAt, type, priority, agencyID, jurisdictionID,
                crimeIDs, ownedBusinessIDs, residenceID, residenceConfidence);
        }

        public WarrantRecord FindActive(LawEnforcementIntelligence intelligence, string agencyID, string jurisdictionID)
        {
            if (intelligence == null || intelligence.Warrants == null) return null;
            // Warrants are discoverable across jurisdiction boundaries. Agency/jurisdiction are provenance,
            // not a legal barrier to recognizing an active warrant.
            return intelligence.Warrants.Where(x => x != null && x.Status == WarrantStatus.Active &&
                    IsArrestAuthority(x.Type))
                .OrderByDescending(x => x.Priority).ThenBy(x => x.IssuedAt).FirstOrDefault();
        }

        public IEnumerable<WarrantRecord> GetActive(LawEnforcementIntelligence intelligence)
        {
            if (intelligence == null || intelligence.Warrants == null) return Enumerable.Empty<WarrantRecord>();
            return intelligence.Warrants.Where(x => x != null && x.Status == WarrantStatus.Active)
                .OrderByDescending(x => x.Priority).ThenBy(x => x.IssuedAt).ToList();
        }

        public bool CanAuthorizeArrest(WarrantRecord warrant)
        {
            return warrant != null && warrant.Status == WarrantStatus.Active && IsArrestAuthority(warrant.Type);
        }

        public bool CanAuthorizeSearch(WarrantRecord warrant)
        {
            // A search warrant is the only warrant type that authorizes a planned premises search.
            return warrant != null && warrant.Status == WarrantStatus.Active &&
                warrant.Type == WarrantType.Search;
        }

        public bool Resolve(LawEnforcementIntelligence intelligence, string warrantID, WarrantStatus status,
            DateTime resolvedAt, string notes)
        {
            return intelligence != null && intelligence.ResolveWarrant(warrantID, status, resolvedAt, notes);
        }

        private static bool IsArrestAuthority(WarrantType type)
        {
            return type == WarrantType.Arrest || type == WarrantType.Bench ||
                type == WarrantType.ProbationParole || type == WarrantType.Extradition;
        }
    }
}
