using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

//need some rethinking
public class ItemDropping
{
    private int CurrentWeaponAmmo;
    private List<StoredWeapon> DroppedItems = new List<StoredWeapon>();
    private bool IsDropping;
    private IActionable Player;
    private ISettingsProvideable Settings;

    public ItemDropping(IActionable currentPlayer, ISettingsProvideable settings)
    {
        Player = currentPlayer;
        Settings = settings;
    }

    public bool CanDrop => !IsDropping && !Player.IsInVehicle && Player.IsVisiblyArmed && Player.ActivityManager.CanPerformActivitiesExtended;
    public void Dispose()
    {

    }
    public void DropWeapon()
    {
        if (CanDrop)
        {
            IsDropping = true;
            GameFiber DropItem = GameFiber.StartNew(delegate
            {
                try
                {
                    DropItemAnimation();
                    IsDropping = false;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "DropItem");
        }
    }

    public void Update()
    {

    }
    private void DropItemAnimation()
    {
        GameFiber DropWeaponAnimation = GameFiber.StartNew(delegate
        {
            try
            {
                AnimationDictionary.RequestAnimationDictionay("pickup_object");
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Game.LocalPlayer.Character, "pickup_object", "pickup_low", 8.0f, -8.0f, -1, 56, 0, false, false, false);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "DropWeaponAnimation");
        if (Game.LocalPlayer.Character.IsRunning)
        {
            GameFiber.Sleep(500);
        }
        else
        {
            GameFiber.Sleep(250);
        }
    }
}