using Rage;
using Rage.Native;

public class PedHeadshot
{
    private uint handle;
    public uint Handle { get { return handle; } }

    private Ped ped;
    public Ped Ped { get { return ped; } }

    public bool IsValid
    {
        get { return NativeFunction.Natives.IsPedheadshotValid<bool>(handle); }
    }

    public bool IsReady
    {
        get { return NativeFunction.Natives.IsPedheadshotReady<bool>(handle); }
    }

    public string Txd
    {
        get { return NativeFunction.Natives.GetPedheadshotTxdString<string>(handle); }
    }

    public PedHeadshot(Ped ped)
    {
        this.ped = ped;
    }

    public void Register()
    {
        handle = NativeFunction.Natives.RegisterPedheadshot<uint>(ped);
    }

    public void Unregister()
    {
        NativeFunction.Natives.UnregisterPedheadshot<uint>(handle);
    }
}