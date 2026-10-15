using System.Linq;

/// <summary>
/// Tests about a ped that more than one gang system needs to agree on.
///
/// Extracted rather than duplicated. The dealer test in particular has already been wrong
/// once — it required GangMember when most street dealers are plain PedExt out of
/// Zone.GetIllicitMenu, and so missed nearly every dealer in the game. A second copy of that
/// mistake, drifting independently, is exactly the failure this file exists to prevent.
///
/// Static and dependency-free on purpose: the crew roster must not have to reach through the
/// goodwill economy to ask a question about a ped, or the two features cannot ship apart.
/// </summary>
public static class GangPedTests
{
    /// <summary>
    /// Somebody worth robbing: a ped who actually SELLS something, is not a legitimate
    /// shopkeeper or a cop, and does not belong to our own outfit.
    ///
    /// The seller check is the load-bearing part. A ped with a ShopMenu might be a buyer —
    /// a fence, a pawn — and shaking down the man who buys your goods is not the same act.
    /// </summary>
    public static bool IsShakeableDealer(PedExt ped, Gang myGang)
    {
        if (ped == null || ped.IsCop || ped is Merchant || ped.ShopMenu == null || ped.ShopMenu.Items == null)
        {
            return false;
        }
        if (!ped.ShopMenu.Items.Any(x => x != null && x.NumberOfItemsToSellToPlayer > 0))
        {
            return false; // a buyer, not a seller
        }
        GangMember gangMember = ped as GangMember;
        if (gangMember != null && gangMember.Gang != null)
        {
            return myGang == null || myGang.ID != gangMember.Gang.ID; // never our own people
        }
        return true; // unaffiliated corner dealer
    }

    /// <summary>A member of an outfit that is not ours. Null gang means we are nobody, so everyone counts.</summary>
    public static bool IsRival(PedExt ped, Gang myGang)
    {
        GangMember gangMember = ped as GangMember;
        if (gangMember == null || gangMember.Gang == null)
        {
            return false;
        }
        return myGang == null || myGang.ID != gangMember.Gang.ID;
    }
}
