using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

[XmlInclude(typeof(TrophyInteract))]
public class TrophyInteract : InteriorInteract
{
    [XmlIgnore]
    public Residence TrophyableLocation { get; set; }
    public string MansionLoc { get; set; } // Mansion Location Name (e.g., "Vinewood", "Richman", "Tongva")
    [XmlIgnore]
    public int LinkedInteriorID { get; internal set; }
    [XmlAttribute("LinkedInteriorID")]
    public int LinkedInteriorIDSerializable
    {
        get => LinkedInteriorID;
        set => LinkedInteriorID = value;
    }

    private ResidenceInterior ResidenceInterior;
    private Rage.Object previewObject;
    private MenuPool menuPool;
    private UIMenu slotMenu;
    private UIMenu trophySubMenu;
    private int selectedSlot;
    private int selectedTrophyID;

    public static readonly Dictionary<int, TrophyDefinition> TrophyRegistry = new Dictionary<int, TrophyDefinition>
    {
        { 1, new TrophyDefinition(1, "m25_2_prop_m52_trpy_100races", "100 Races") },
        { 2, new TrophyDefinition(2, "m25_2_prop_m52_trpy_10yo", "10 Year Anniversary") },
        { 3, new TrophyDefinition(3, "m25_2_prop_m52_trpy_3commaclub", "Three Comma Club") },
        { 4, new TrophyDefinition(4, "m25_2_prop_m52_trpy_500mil", "$500 Million Earned") },
        { 5, new TrophyDefinition(5, "m25_2_prop_m52_trpy_adversary", "Adversary Champion") },
        { 6, new TrophyDefinition(6, "m25_2_prop_m52_trpy_arcademaster", "Arcade Master") },
        { 7, new TrophyDefinition(7, "m25_2_prop_m52_trpy_arenawarlegend", "Arena War Legend") },
        { 8, new TrophyDefinition(8, "m25_2_prop_m52_trpy_carcollector", "Car Collector") },
        { 9, new TrophyDefinition(9, "m25_2_prop_m52_trpy_careercriminal", "Career Criminal") },
        { 10, new TrophyDefinition(10, "m25_2_prop_m52_trpy_cayoperico", "Cayo Perico Heist") },
        { 11, new TrophyDefinition(11, "m25_2_prop_m52_trpy_collector", "Collector") },
        { 12, new TrophyDefinition(12, "m25_2_prop_m52_trpy_completionist", "Completionist") },
        { 13, new TrophyDefinition(13, "m25_2_prop_m52_trpy_diamondcasino", "Diamond Casino Heist") },
        { 14, new TrophyDefinition(14, "m25_2_prop_m52_trpy_doomsday", "Doomsday Heist") },
        { 15, new TrophyDefinition(15, "m25_2_prop_m52_trpy_entrepreneur", "Entrepreneur") },
        { 16, new TrophyDefinition(16, "m25_2_prop_m52_trpy_flightschool", "Flight School") },
        { 17, new TrophyDefinition(17, "m25_2_prop_m52_trpy_gtav", "GTA V Veteran") },
        { 18, new TrophyDefinition(18, "m25_2_prop_m52_trpy_heistpro", "Heist Professional") },
        { 19, new TrophyDefinition(19, "m25_2_prop_m52_trpy_lscm", "LS Car Meet") },
        { 20, new TrophyDefinition(20, "m25_2_prop_m52_trpy_mastermind", "Criminal Mastermind") },
        { 21, new TrophyDefinition(21, "m25_2_prop_m52_trpy_ninefig", "Nine Figure Earner") },
        { 22, new TrophyDefinition(22, "m25_2_prop_m52_trpy_platinum", "Platinum Award") },
        { 23, new TrophyDefinition(23, "m25_2_prop_m52_trpy_rank100", "Rank 100") },
        { 24, new TrophyDefinition(24, "m25_2_prop_m52_trpy_rank500", "Rank 500") },
        { 25, new TrophyDefinition(25, "m25_2_prop_m52_trpy_rank1000", "Rank 1000") },
        { 26, new TrophyDefinition(26, "m25_2_prop_m52_trpy_spotlight", "Spotlight Award") },
        { 27, new TrophyDefinition(27, "m25_2_prop_m52_trpy_story", "Story Completion") },
        { 28, new TrophyDefinition(28, "m25_2_prop_m52_trpy_vault", "Vault Cracker") },
        { 29, new TrophyDefinition(29, "m25_2_prop_m52_trpy_wellliked", "Well Liked") },
        { 30, new TrophyDefinition(30, "m25_2_prop_m52_trpy_worldrecord", "World Record Holder") }
    };

    public static readonly Dictionary<string, CabinetData> CabinetDatas = new Dictionary<string, CabinetData>
    {
        { "Vinewood", new CabinetData
            {
                CabinetCameraPosition = new Vector3(539.2967f, 738.8112f, 199.5518f),
                CabinetCameraDirection = new Vector3(-0.7532488f, -0.6125796f, -0.2395045f),
                CabinetCameraRotation = new Rotator(-13.8573f, 8.353992E-06f, 129.1197f),
                TrophyHeading = 128.529f,
                Slots = new List<TrophySlot>
                {
                    new TrophySlot { SlotID = 1, Position = new Vector3(537.8293f, 735.0106f, 198.73f), Rotation = 180f, CameraPosition = new Vector3(538.6434f, 735.7286f, 199.2184f), CameraDirection = new Vector3(-0.7748819f, -0.6112047f, -0.1612045f), CameraRotation = new Rotator(-9.276819f, 6.055617E-06f, 128.2654f) },
                    new TrophySlot { SlotID = 2, Position = new Vector3(537.2085f, 735.796f, 198.73f), Rotation = 180f, CameraPosition = new Vector3(538.0245f, 736.5135f, 199.2184f), CameraDirection = new Vector3(-0.7748819f, -0.6112047f, -0.1612045f), CameraRotation = new Rotator(-9.276819f, 6.055617E-06f, 128.2654f) },
                    new TrophySlot { SlotID = 3, Position = new Vector3(536.5898f, 736.5798f, 198.73f), Rotation = 180f, CameraPosition = new Vector3(537.4056f, 737.2984f, 199.2184f), CameraDirection = new Vector3(-0.7748819f, -0.6112047f, -0.1612045f), CameraRotation = new Rotator(-9.276819f, 6.055617E-06f, 128.2654f) },
                    new TrophySlot { SlotID = 4, Position = new Vector3(535.9694f, 737.3654f, 198.73f), Rotation = 180f, CameraPosition = new Vector3(536.8177f, 738.0441f, 199.2184f), CameraDirection = new Vector3(-0.7748819f, -0.6112047f, -0.1612045f), CameraRotation = new Rotator(-9.276819f, 6.055617E-06f, 128.2654f) },
                    new TrophySlot { SlotID = 5, Position = new Vector3(535.3489f, 738.1505f, 198.73f), Rotation = 180f, CameraPosition = new Vector3(536.1988f, 738.829f, 199.2184f), CameraDirection = new Vector3(-0.7748819f, -0.6112047f, -0.1612045f), CameraRotation = new Rotator(-9.276819f, 6.055617E-06f, 128.2654f) }
                }
            }
        },
        { "Richman", new CabinetData
            {
                CabinetCameraPosition = new Vector3(-1656.854f, 475.0268f, 126.2049f),
                CabinetCameraDirection = new Vector3(0.3005681f, -0.9286817f, -0.2172768f),
                CabinetCameraRotation = new Rotator(-12.54914f, 2.186674E-07f, -162.0658f),
                TrophyHeading = 197.7647f,
                Slots = new List<TrophySlot>
                {
                    new TrophySlot { SlotID = 1, Position = new Vector3(-1653.841f, 472.2335f, 125.59f), Rotation = 0f, CameraPosition = new Vector3(-1654.183f, 473.2792f, 126.0424f), CameraDirection = new Vector3(0.298961f, -0.9462714f, -0.123259f), CameraRotation = new Rotator(-7.080229f, -5.592172E-06f, -162.4668f) },
                    new TrophySlot { SlotID = 2, Position = new Vector3(-1654.795f, 471.9278f, 125.59f), Rotation = 0f, CameraPosition = new Vector3(-1655.186f, 472.9626f, 126.0424f), CameraDirection = new Vector3(0.298961f, -0.9462714f, -0.123259f), CameraRotation = new Rotator(-7.080229f, -5.592172E-06f, -162.4668f) },
                    new TrophySlot { SlotID = 3, Position = new Vector3(-1655.746f, 471.6235f, 125.59f), Rotation = 0f, CameraPosition = new Vector3(-1656.093f, 472.6762f, 126.0424f), CameraDirection = new Vector3(0.298961f, -0.9462714f, -0.123259f), CameraRotation = new Rotator(-7.080229f, -5.592172E-06f, -162.4668f) },
                    new TrophySlot { SlotID = 4, Position = new Vector3(-1656.699f, 471.3182f, 125.59f), Rotation = 0f, CameraPosition = new Vector3(-1657.047f, 472.3747f, 126.0424f), CameraDirection = new Vector3(0.298961f, -0.9462714f, -0.123259f), CameraRotation = new Rotator(-7.080229f, -5.592172E-06f, -162.4668f) },
                    new TrophySlot { SlotID = 5, Position = new Vector3(-1657.652f, 471.0128f, 125.59f), Rotation = 0f, CameraPosition = new Vector3(-1658.002f, 472.0732f, 126.0424f), CameraDirection = new Vector3(0.298961f, -0.9462714f, -0.123259f), CameraRotation = new Rotator(-7.080229f, -5.592172E-06f, -162.4668f) }
                }
            }
        },
        { "Tongva", new CabinetData
            {
                CabinetCameraPosition = new Vector3(-2591.883f, 1901.446f, 164.3784f),
                CabinetCameraDirection = new Vector3(-0.9708887f, -0.09680843f, -0.2190962f),
                CabinetCameraRotation = new Rotator(-12.65596f, 4.37517E-07f, 95.69421f),
                TrophyHeading = 96.0f,
                Slots = new List<TrophySlot>
                {
                    new TrophySlot { SlotID = 1, Position = new Vector3(-2595.189f, 1899.111f, 163.742f), Rotation = 0f, CameraPosition = new Vector3(-2594.049f, 1899.246f, 164.1613f), CameraDirection = new Vector3(-0.9913322f, -0.09000064f, -0.0957096f), CameraRotation = new Rotator(-5.492163f, 3.69888E-06f, 95.18752f) },
                    new TrophySlot { SlotID = 2, Position = new Vector3(-2595.292f, 1900.106f, 163.742f), Rotation = 0f, CameraPosition = new Vector3(-2594.141f, 1900.243f, 164.1613f), CameraDirection = new Vector3(-0.9913322f, -0.09000064f, -0.0957096f), CameraRotation = new Rotator(-5.492163f, 3.69888E-06f, 95.18752f) },
                    new TrophySlot { SlotID = 3, Position = new Vector3(-2595.395f, 1901.1f, 163.742f), Rotation = 0f, CameraPosition = new Vector3(-2594.234f, 1901.239f, 164.1613f), CameraDirection = new Vector3(-0.9913208f, -0.08587798f, -0.09953921f), CameraRotation = new Rotator(-5.712637f, 2.09146E-06f, 94.95116f) },
                    new TrophySlot { SlotID = 4, Position = new Vector3(-2595.498f, 1902.095f, 163.742f), Rotation = 0f, CameraPosition = new Vector3(-2594.322f, 1902.235f, 164.1613f), CameraDirection = new Vector3(-0.9913208f, -0.08587798f, -0.09953921f), CameraRotation = new Rotator(-5.712637f, 2.09146E-06f, 94.95116f) },
                    new TrophySlot { SlotID = 5, Position = new Vector3(-2595.601f, 1903.091f, 163.742f), Rotation = 0f, CameraPosition = new Vector3(-2594.41f, 1903.231f, 164.1613f), CameraDirection = new Vector3(-0.9911417f, -0.08792067f, -0.0995392f), CameraRotation = new Rotator(-5.712636f, 2.037833E-06f, 95.06924f) }
                }
            }
        }
    };

    public static readonly Dictionary<int, CabinetData> CabinetDatasByInterior = new Dictionary<int, CabinetData>
    {
        { 304385, CabinetDatas["Vinewood"] },
        { 302593, CabinetDatas["Richman"] },
        { 303617, CabinetDatas["Tongva"] }
    };

    public TrophyInteract() { }

    public TrophyInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {
        AutoCamera = false;
    }

    public override void OnInteract()
    {
        if (Player == null || Interior == null)
            return;

        ResidenceInterior = Interior as ResidenceInterior;
        if (ResidenceInterior == null)
        {
            Game.DisplayHelp("Not a residence interior");
            return;
        }

        LinkedInteriorID = Interior.InternalID;

        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();

        SetupCamera(false);

        if (!MoveToPosition())
        {
            Game.DisplayHelp("Access Failed");
            LocationCamera?.StopImmediately(true);
            Interior.IsMenuInteracting = false;
            return;
        }

        Player.InteriorManager.OnStartedInteriorInteract();

        try
        {
            if (menuPool == null)
                menuPool = new MenuPool();

            if (!CabinetDatasByInterior.TryGetValue(LinkedInteriorID, out CabinetData data) || data == null)
            {
                Game.DisplayHelp("Cabinet Data Missing");
                return;
            }

            MansionLoc = CabinetDatas.FirstOrDefault(x => x.Value == data).Key;
            CreateSlotMenu(data);

            while ((slotMenu != null && slotMenu.Visible) || (trophySubMenu != null && trophySubMenu.Visible))
            {
                menuPool.ProcessMenus();
                GameFiber.Yield();
            }
        }
        catch (Exception ex)
        {
            Game.DisplayHelp("Error in Trophy Interaction");
            EntryPoint.WriteToConsole($"TrophyInteract.OnInteract Exception: {ex.Message} {ex.StackTrace}");
        }
        finally
        {
            Cleanup();
        }
    }

    private void Cleanup()
    {
        if (previewObject != null && previewObject.Exists())
        {
            previewObject.Delete();
        }
        Interior.IsMenuInteracting = false;
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Player.InteriorManager.OnEndedInteriorInteract();
    }

    private void CreateSlotMenu(CabinetData data)
    {
        if (data == null || data.Slots == null || data.Slots.Count == 0) return;

        if (slotMenu != null)
            slotMenu.Clear();
        else
        {
            slotMenu = new UIMenu("Trophy Cabinet", "Select Slot");
            menuPool.Add(slotMenu);
            slotMenu.SetBannerType(EntryPoint.LSRedColor);
        }

        List<string> slotNames = new List<string>();
        for (int i = 0; i < data.Slots.Count; i++)
        {
            TrophySlot ts = data.Slots[i];
            slotNames.Add($"Slot {ts.SlotID}");
        }

        UIMenuListScrollerItem<string> slotScroller = new UIMenuListScrollerItem<string>("Slot", "Select a slot to preview", slotNames);
        slotMenu.AddItem(slotScroller);

        slotScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            selectedSlot = data.Slots[newIndex].SlotID;
            TrophySlot ts = data.Slots[newIndex];
            LocationCamera?.MoveToPosition(ts.CameraPosition, ts.CameraDirection, ts.CameraRotation, true, false, false);

            if (ResidenceInterior?.PlacedTrophies != null && ResidenceInterior.PlacedTrophies.TryGetValue(selectedSlot, out int trophyID))
            {
                UpdatePreview(ts, trophyID);
            }
            else
            {
                UpdatePreview(ts, 0);
            }
        };

        UIMenuItem selectItem = new UIMenuItem("Select Slot", "View trophies for this slot");
        slotMenu.AddItem(selectItem);
        selectItem.Activated += (sender, item) =>
        {
            TrophySlot ts = data.Slots.FirstOrDefault(x => x.SlotID == selectedSlot);
            if (ts != null)
            {
                slotMenu.Visible = false;
                CreateTrophySubMenu(data, ts);
            }
        };

        slotMenu.Visible = true;

        if (data.Slots.Count > 0)
        {
            slotScroller.Index = 0;
            TrophySlot firstSlot = data.Slots[0];
            selectedSlot = firstSlot.SlotID;
            LocationCamera?.MoveToPosition(firstSlot.CameraPosition, firstSlot.CameraDirection, firstSlot.CameraRotation, true, false, false);

            if (ResidenceInterior?.PlacedTrophies != null && ResidenceInterior.PlacedTrophies.TryGetValue(firstSlot.SlotID, out int trophyID))
            {
                UpdatePreview(firstSlot, trophyID);
            }
            else
            {
                UpdatePreview(firstSlot, 0);
            }
        }
    }

    private void CreateTrophySubMenu(CabinetData data, TrophySlot ts)
    {
        if (ts == null) return;
        if (trophySubMenu != null)
            trophySubMenu.Clear();
        else
        {
            trophySubMenu = new UIMenu($"Slot {ts.SlotID}", "Select Trophy");
            menuPool.Add(trophySubMenu);
            trophySubMenu.SetBannerType(EntryPoint.LSRedColor);
        }

        // Temporarily hide the placed trophy while previewing
        Rage.Object placedTrophy = null;
        int originalTrophyID = 0;
        if (ResidenceInterior.SpawnedTrophies.TryGetValue(ts.SlotID, out Rage.Object existing) && existing.Exists())
        {
            placedTrophy = existing;
            ResidenceInterior.PlacedTrophies.TryGetValue(ts.SlotID, out originalTrophyID);
            existing.Delete();
            ResidenceInterior.SpawnedTrophies.Remove(ts.SlotID);
            Interior.SpawnedProps.Remove(existing);
        }

        List<string> trophyNames = new List<string> { "None" };
        trophyNames.AddRange(TrophyRegistry.Values.Select(x => x.Description));

        UIMenuListScrollerItem<string> trophyScroller = new UIMenuListScrollerItem<string>("Trophy", "Select a trophy to preview", trophyNames);
        trophySubMenu.AddItem(trophyScroller);

        trophyScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            string selectedDesc = trophyScroller.SelectedItem;
            selectedTrophyID = selectedDesc == "None" ? 0 : TrophyRegistry.Values.FirstOrDefault(x => x.Description == selectedDesc)?.ID ?? 0;
            UpdatePreview(ts, selectedTrophyID);
        };

        UIMenuItem confirmItem = new UIMenuItem("Confirm", "Apply the selected trophy");
        trophySubMenu.AddItem(confirmItem);
        confirmItem.Activated += (sender, item) =>
        {
            ApplyTrophy(ts);
            trophySubMenu.Visible = false;
            slotMenu.Visible = true;
            LocationCamera?.MoveToPosition(data.CabinetCameraPosition, data.CabinetCameraDirection, data.CabinetCameraRotation, true, false, false);
        };

        UIMenuItem backItem = new UIMenuItem("Back", "Return to slot selection");
        trophySubMenu.AddItem(backItem);
        backItem.Activated += (sender, item) =>
        {
            if (placedTrophy != null && !placedTrophy.Exists() && originalTrophyID != 0)
            {
                if (TrophyRegistry.TryGetValue(originalTrophyID, out TrophyDefinition originalDef))
                {
                    uint modelHash = Game.GetHashKey(originalDef.ModelName);
                    NativeFunction.Natives.REQUEST_MODEL(modelHash);
                    while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
                    {
                        GameFiber.Yield();
                    }
                    if (NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
                    {
                        float heading = CabinetDatas[MansionLoc].TrophyHeading;
                        Rage.Object restored = new Rage.Object(modelHash, ts.Position, heading);
                        if (restored.Exists())
                        {
                            int entityHandle = (int)restored.Handle.Value;
                            NativeFunction.Natives.SET_ENTITY_COLLISION(entityHandle, false, false);
                            NativeFunction.Natives.FREEZE_ENTITY_POSITION(entityHandle, true);
                            restored.IsPersistent = true;
                            ResidenceInterior.SpawnedTrophies[ts.SlotID] = restored;
                            Interior.SpawnedProps.Add(restored);
                        }
                    }
                    NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(modelHash);
                }
            }

            trophySubMenu.Visible = false;
            slotMenu.Visible = true;
            LocationCamera?.MoveToPosition(data.CabinetCameraPosition, data.CabinetCameraDirection, data.CabinetCameraRotation, true, false, false);
        };

        int initialID = 0;
        if (ResidenceInterior?.PlacedTrophies != null)
            ResidenceInterior.PlacedTrophies.TryGetValue(ts.SlotID, out initialID);

        TrophyRegistry.TryGetValue(initialID, out TrophyDefinition initialDef);
        trophyScroller.Index = initialDef != null ? trophyNames.IndexOf(initialDef.Description) : 0;

        selectedTrophyID = initialID;
        UpdatePreview(ts, initialID);

        trophySubMenu.Visible = true;
    }

    private void UpdatePreview(TrophySlot ts, int trophyID)
    {
        if (previewObject != null && previewObject.Exists())
        {
            previewObject.Delete();
            previewObject = null;
        }

        if (trophyID == 0) return;

        if (TrophyRegistry.TryGetValue(trophyID, out TrophyDefinition def))
        {
            uint modelHash = Game.GetHashKey(def.ModelName);
            NativeFunction.Natives.REQUEST_MODEL(modelHash);
            while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
            {
                GameFiber.Yield();
            }

            float heading = CabinetDatas[MansionLoc].TrophyHeading;
            previewObject = new Rage.Object(modelHash, ts.Position, heading)
            {
                IsPersistent = true
            };

            if (previewObject.Exists())
            {
                int entityHandle = (int)previewObject.Handle.Value;
                NativeFunction.Natives.SET_ENTITY_COLLISION(entityHandle, false, false);
                NativeFunction.Natives.FREEZE_ENTITY_POSITION(entityHandle, true);
            }

            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(modelHash);
        }
    }

    private void ApplyTrophy(TrophySlot ts)
    {
        if (previewObject != null && previewObject.Exists())
        {
            previewObject.Delete();
            previewObject = null;
        }

        if (ResidenceInterior.SpawnedTrophies.TryGetValue(selectedSlot, out Rage.Object existing) && existing.Exists())
        {
            existing.Delete();
            ResidenceInterior.SpawnedTrophies.Remove(selectedSlot);
            Interior.SpawnedProps.Remove(existing);
        }

        ResidenceInterior.PlacedTrophies[selectedSlot] = selectedTrophyID;

        if (selectedTrophyID != 0 && TrophyRegistry.TryGetValue(selectedTrophyID, out TrophyDefinition def))
        {
            uint modelHash = Game.GetHashKey(def.ModelName);
            NativeFunction.Natives.REQUEST_MODEL(modelHash);
            while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
            {
                GameFiber.Yield();
            }

            if (NativeFunction.Natives.HAS_MODEL_LOADED<bool>(modelHash))
            {
                float heading = CabinetDatas[MansionLoc].TrophyHeading;
                Rage.Object newTrophy = new Rage.Object(modelHash, ts.Position, heading);
                if (newTrophy.Exists())
                {
                    int entityHandle = (int)newTrophy.Handle.Value;
                    NativeFunction.Natives.SET_ENTITY_COLLISION(entityHandle, false, false);
                    NativeFunction.Natives.FREEZE_ENTITY_POSITION(entityHandle, true);
                    newTrophy.IsPersistent = true;
                    ResidenceInterior.SpawnedTrophies[selectedSlot] = newTrophy;
                    Interior.SpawnedProps.Add(newTrophy);
                }
            }

            NativeFunction.Natives.SET_MODEL_AS_NO_LONGER_NEEDED(modelHash);
        }

        ResidenceInterior.SavedPlacedTrophies.Clear();
        foreach (KeyValuePair<int, int> kvp in ResidenceInterior.PlacedTrophies)
        {
            ResidenceInterior.SavedPlacedTrophies.Add(new TrophyPlacement { SlotID = kvp.Key, TrophyID = kvp.Value });
        }
    }

    public override void AddPrompt()
    {
        if (Player == null) return;
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
}

public class TrophyDefinition
{

    public TrophyDefinition() { }
    public TrophyDefinition(int id, string modelName, string description)
    {
        ID = id;
        ModelName = modelName;
        Description = description;
    }
    public int ID { get; set; }
    public string ModelName { get; set; }
    public string Description { get; set; }
}

public class CabinetData
{
    public Vector3 CabinetCameraPosition { get; set; }
    public Vector3 CabinetCameraDirection { get; set; }
    public Rotator CabinetCameraRotation { get; set; }
    public float TrophyHeading { get; set; } = 180f;
    public List<TrophySlot> Slots { get; set; }
}
[Serializable]
public class TrophyPlacement
{
    public int SlotID { get; set; }
    public int TrophyID { get; set; }

    public TrophyPlacement() { }
}
public class TrophySlot
{
    public int SlotID { get; set; }
    public Vector3 Position { get; set; }
    public float Rotation { get; set; }
    public Vector3 CameraPosition { get; set; }
    public Vector3 CameraDirection { get; set; }
    public Rotator CameraRotation { get; set; }
    public string Description { get; set; }
}