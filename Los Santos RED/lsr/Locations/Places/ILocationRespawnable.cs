using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ILocationRespawnable
{
    string Name { get; }
    Vector3 EntrancePosition { get; }
    float EntranceHeading { get; }
    string FullStreetAddress { get; }
    bool IsEnabled { get; }
    bool IsActivated { get; }
    string StateID { get; }

    Vector3 RespawnLocation { get; }
    float RespawnHeading { get; }
    void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world);
    bool IsSameState(GameState gameState);
}

