namespace LosSantosRED.lsr.Interface
{
    public interface IPayoutDisbursable
    {
        void Payout(IPropertyOwnable player, ITimeReportable time);
        void HandleRaid();
    }
}