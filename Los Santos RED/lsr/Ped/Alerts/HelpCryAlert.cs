using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class HelpCryAlert : PedAlert
{
    public HelpCryAlert(PedExt pedExt, ISettingsProvideable settings) : base(pedExt, settings, ePedAlertType.HelpCry)
    {
        Priority = 5;
        IsPositionAlert = false;
        IsPedAlert = true;
        TimeOutTime = Settings.SettingsManager.WorldSettings.HelpCryAlertTimeout;
    }
    public override void Update(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if (!PedExt.PedAlertTypes.HasFlag(ePedAlertType.HelpCry))
        {
            return;
        }
        if (policeRespondable.ActivityManager.IsWavingHands && (PedExt.DistanceToPlayer <= 75f || PedExt.CanSeePlayer))
        {
            EntryPoint.WriteToConsole($"I AM PED {PedExt.Handle} AND I JUST HEARD A HELP CRY");
            AddAlert(policeRespondable.Position);
        }
        base.Update(policeRespondable, world);
    }
}

