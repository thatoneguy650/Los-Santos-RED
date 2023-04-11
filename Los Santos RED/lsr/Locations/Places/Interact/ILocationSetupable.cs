using LosSantosRED.lsr.Interface;

public interface ILocationSetupable
{
    void Setup(ICrimes crimes, INameProvideable names, ISettingsProvideable settings);
}