using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MeleeWeaponAlias
{
    private ISettingsProvideable Settings;
    public bool IsCancelled;
    private IActionable Player;
    private ModItem ItemToAlias;
    private bool hadAliasedWeapon;
    private Rage.Object ItemToAliasObject;
    private string HandBoneName = "BONETAG_R_PH_HAND";
    private Vector3 HandOffset = new Vector3();
    private Rotator HandRotator = new Rotator();
    private string PropModelName = "prop_tool_shovel";
    private string WeaponHandBoneName;
    private Vector3 WeaponHandOffset;
    private Rotator WeaponHandRotator;

    private uint MeleeWeaponToAliasHash;

    public MeleeWeaponAlias(IActionable player, ISettingsProvideable settings, ModItem itemToAlias) : base()
    {
        Player = player;
        Settings = settings;
        ItemToAlias = itemToAlias;
    }

    public void Start()
    {
        //EntryPoint.WriteToConsoleTestLong("MELEE WEAPON ALIAS START");
        Setup();
        SpawnAndAttach();
    }
    public void Update()
    {
        if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
        {
            Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = false;
        }
        uint currentWeapon;
        NativeFunction.Natives.GET_CURRENT_PED_WEAPON<bool>(Player.Character, out currentWeapon, true);
        if (currentWeapon != MeleeWeaponToAliasHash)
        {
            //EntryPoint.WriteToConsoleTestLong("Player changed weapon, ending");
            IsCancelled = true;
        }
        if(!Player.IsAliveAndFree)
        {
            IsCancelled = true;
        }
    }
    public void Dispose()
    {
        if (ItemToAliasObject.Exists())
        {
            ItemToAliasObject.Delete();
        }
        if (!hadAliasedWeapon && NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, MeleeWeaponToAliasHash, false))
        {
            NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(Player.Character, MeleeWeaponToAliasHash);
            Player.WeaponEquipment.SetUnarmed();
        }
        Player.ActivityManager.IsUsingToolAsWeapon = false;
        //EntryPoint.WriteToConsoleTestLong("MELEE WEAPON ALIAS ENDED");
    }
    private void SpawnAndAttach()
    {
        hadAliasedWeapon = NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, MeleeWeaponToAliasHash, false);
        if (!hadAliasedWeapon)
        {
            NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, MeleeWeaponToAliasHash, 0, false, false);
        }
        NativeFunction.Natives.SET_CURRENT_PED_WEAPON(Player.Character, MeleeWeaponToAliasHash, true);
        uint GameTimeStarted = Game.GameTime;
        while (!Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists() && Game.GameTime - GameTimeStarted <= 500)
        {
            GameFiber.Yield();
        }
        if (Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.Exists())
        {
            Game.LocalPlayer.Character.Inventory.EquippedWeaponObject.IsVisible = false;
        }
        AttachAliasItemToHand();
    }
    private void AttachAliasItemToHand()
    {
        CreateShovel();
        if (ItemToAliasObject.Exists())
        {
            ItemToAliasObject.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, WeaponHandBoneName), WeaponHandOffset, WeaponHandRotator);
            Player.AttachedProp.Add(ItemToAliasObject);
        }
    }
    private void CreateShovel()
    {
        if (!ItemToAliasObject.Exists() && PropModelName != "")
        {
            try
            {
                ItemToAliasObject = new Rage.Object(PropModelName, Player.Character.GetOffsetPositionUp(50f));
            }
            catch (Exception ex)
            {
                //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
            }
            if (!ItemToAliasObject.Exists())
            {
                //EntryPoint.WriteToConsoleTestLong("Error Creating Item for Melee Alias");
                IsCancelled = true;
            }
        }
    }
    private void Setup()
    {
        PropModelName = "prop_tool_shovel";
        WeaponHandBoneName = "BONETAG_R_PH_HAND";
        WeaponHandOffset = new Vector3();
        WeaponHandRotator = new Rotator();
        MeleeWeaponToAliasHash = 1317494643;
        if (ItemToAlias != null)
        {
            PropModelName = ItemToAlias.ModelItem.ModelName;
            PropAttachment handWeaponAttachment = ItemToAlias.ModelItem.Attachments.Where(x => x.Name == "RightHandWeapon").FirstOrDefault();
            if (handWeaponAttachment != null)
            {
                WeaponHandBoneName = handWeaponAttachment.BoneName;
                WeaponHandOffset = handWeaponAttachment.Attachment;
                WeaponHandRotator = handWeaponAttachment.Rotation;
            }
            MeleeWeaponToAliasHash = ItemToAlias.ModelItem.AliasWeaponHash;

            Player.ActivityManager.IsUsingToolAsWeapon = true;

        }
        else
        {
            IsCancelled = true;
        }
    }
}

