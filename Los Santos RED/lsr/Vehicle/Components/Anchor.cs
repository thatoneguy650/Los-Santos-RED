using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;

namespace LSR.Vehicles
{
    public class Anchor
    {
        private readonly VehicleExt VehicleToMonitor;
        private uint GameTimeLastToggledAnchor;
        private uint GameTimeLastUpdated;
        private const uint AnchorToggleCooldown = 1500;
        private const uint UpdateCooldown = 3000;
        private const float MinPlayerDistance = 50.0f;
        private const float RopeLeewayMultiplier = 1.4f; // 40% slack
        private const float MinRopeLengthWithLeeway = 8.0f;
        private const float SeabedPinDepth = 3.0f; // Deeper = more sag
        private const int RopeVertexCount = 12; // Smoother curve
        private int ropeID = 0;
        private bool spawnedRope = false;
        private bool lastAnchorState = false;
        private readonly List<int> CreatedRopes = new List<int>();
        private static readonly List<Anchor> ActiveAnchors = new List<Anchor>();

        private static readonly Dictionary<string, BoatAnchorConfig> AnchorConfigs = new Dictionary<string, BoatAnchorConfig>(StringComparer.OrdinalIgnoreCase)
        {
            // Jet Skis (Seashark variants)
            { "seashark", new BoatAnchorConfig { Offset = new Vector3(0f, -1.4f, 0f), BoneName = "chassis" } },
            { "seashark2", new BoatAnchorConfig { Offset = new Vector3(0f, -1.4f, 0f), BoneName = "chassis" } },
            { "seashark3", new BoatAnchorConfig { Offset = new Vector3(0f, -1.4f, 0f), BoneName = "chassis" } },
            { "seashark4", new BoatAnchorConfig { Offset = new Vector3(0f, -1.4f, 0f), BoneName = "chassis" } },
            // Dinghy variants
            { "dinghy", new BoatAnchorConfig { Offset = new Vector3(0f, -2.5f, 0f), BoneName = "chassis" } },
            { "dinghy2", new BoatAnchorConfig { Offset = new Vector3(0f, -2.5f, 0f), BoneName = "chassis" } },
            { "dinghy3", new BoatAnchorConfig { Offset = new Vector3(0f, -2.5f, 0f), BoneName = "chassis" } },
            { "dinghy4", new BoatAnchorConfig { Offset = new Vector3(0f, -2.5f, 0f), BoneName = "chassis" } },
            { "dinghy5", new BoatAnchorConfig { Offset = new Vector3(0f, -2.5f, 0f), BoneName = "chassis" } },
            // Other boats
            { "squalo", new BoatAnchorConfig { Offset = new Vector3(0f, 3.9f, 0.3f), BoneName = "bow" } },
            { "suntrap", new BoatAnchorConfig { Offset = new Vector3(0f, 3.0f, 0.3f), BoneName = "bow" } },
            { "tropic", new BoatAnchorConfig { Offset = new Vector3(0f, 3.6f, 0.3f), BoneName = "bow" } },
            { "tropic2", new BoatAnchorConfig { Offset = new Vector3(0f, 3.6f, 0.3f), BoneName = "bow" } },
            { "tug", new BoatAnchorConfig { Offset = new Vector3(0f, 13.0f, 0f), BoneName = "bow" } },
            // Police/Patrol Boats
            { "0xef813606", new BoatAnchorConfig { Offset = new Vector3(0f, 5.16f, 0.2f), BoneName = "bow" } },
            { "predator", new BoatAnchorConfig { Offset = new Vector3(0f, 5.16f, 0.2f), BoneName = "bow" } },
            //Speedboats
            { "jetmax", new BoatAnchorConfig { Offset = new Vector3(0f, 4.9f, 0.5f), BoneName = "bow" } },
            { "longfin", new BoatAnchorConfig { Offset = new Vector3(0f, 6.2f, 0.5f), BoneName = "bow" } },
            { "speeder", new BoatAnchorConfig { Offset = new Vector3(0f, 4.8f, 0.6f), BoneName = "bow" } },
            { "speeder2", new BoatAnchorConfig { Offset = new Vector3(0f, 4.8f, 0.6f), BoneName = "bow" } },
            { "toro", new BoatAnchorConfig { Offset = new Vector3(0f, 4.2f, 1.0f), BoneName = "bow" } },
            { "toro2", new BoatAnchorConfig { Offset = new Vector3(0f, 4.2f, 1.0f), BoneName = "bow" } },
            
            // Submersibles - commented out as they typically don't use anchors + don't work with IsBoat...
            // So what are they? _IS_THIS_MODEL_A_JETSKI ??? | Old names: _IS_THIS_MODEL_A_SUBMERSIBLE | _IS_THIS_MODEL_AN_EMERGENCY_BOAT
            //{ "avisa", new BoatAnchorConfig { Offset = new Vector3(0f, 0f, 0f), BoneName = "chassis" } },
            //{ "kraken", new BoatAnchorConfig { Offset = new Vector3(0f, 0f, 0f), BoneName = "chassis" } },
            //{ "submersible", new BoatAnchorConfig { Offset = new Vector3(0f, 0f, 0f), BoneName = "chassis" } },
            //{ "submersible2", new BoatAnchorConfig { Offset = new Vector3(0f, 0f, 0f), BoneName = "chassis" } },
            //Yachts
            { "marquis", new BoatAnchorConfig { Offset = new Vector3(0f, 6.9f, 0.9f), BoneName = "bow" } },
        };
        public Anchor(VehicleExt vehicleToMonitor)
        {
            VehicleToMonitor = vehicleToMonitor;
            IsDeployed = false;
        }
        public bool IsDeployed { get; private set; }
        public bool CanToggle =>
            VehicleToMonitor?.Vehicle.Exists() == true &&
            VehicleToMonitor.IsBoat &&
            VehicleToMonitor.Vehicle.Speed < 3.0f &&
            Game.GameTime - GameTimeLastToggledAnchor >= AnchorToggleCooldown;
        public void Toggle()
        {
            if (!CanToggle)
            {
                if (!VehicleToMonitor.IsBoat)
                    Game.DisplaySubtitle("~r~Anchor can only be used on boats.");
                else if (VehicleToMonitor.Vehicle.Speed >= 3.0f)
                    Game.DisplaySubtitle("~r~Slow down to toggle anchor.");
                return;
            }
            SetState(!IsDeployed);
            GameTimeLastToggledAnchor = Game.GameTime;
        }
        public void SetState(bool deploy)
        {
            if (VehicleToMonitor?.Vehicle.Exists() != true || !VehicleToMonitor.IsBoat)
            {
                Game.DisplaySubtitle("~r~Anchor can only be used on boats.");
                return;
            }
            bool canAnchor = NativeFunction.Natives.CAN_ANCHOR_BOAT_HERE<bool>(VehicleToMonitor.Vehicle);
            if (deploy && !canAnchor)
            {
                Game.DisplaySubtitle("~r~Cannot anchor here.");
                return;
            }
            IsDeployed = deploy;
            const float lodDistance = 100.0f;
            if (IsDeployed)
            {
                NativeFunction.Natives.SET_BOAT_ANCHOR(VehicleToMonitor.Vehicle, true);
                NativeFunction.Natives.SET_BOAT_REMAINS_ANCHORED_WHILE_PLAYER_IS_DRIVER(VehicleToMonitor.Vehicle, true);
                NativeFunction.Natives.SET_BOAT_LOW_LOD_ANCHOR_DISTANCE(VehicleToMonitor.Vehicle, lodDistance);
                VehicleToMonitor.Engine.SetState(false);
                SpawnRopeToSeabed();
                Game.DisplaySubtitle("~g~Anchor Down", 3000);
            }
            else
            {
                NativeFunction.Natives.SET_BOAT_ANCHOR(VehicleToMonitor.Vehicle, false);
                NativeFunction.Natives.SET_BOAT_REMAINS_ANCHORED_WHILE_PLAYER_IS_DRIVER(VehicleToMonitor.Vehicle, false);
                NativeFunction.Natives.SET_BOAT_LOW_LOD_ANCHOR_DISTANCE(VehicleToMonitor.Vehicle, -1.0f);
                DeleteRope();
                Game.DisplaySubtitle("~g~Anchor Up", 3000);
            }
            lastAnchorState = IsDeployed;
        }
        private void SpawnRopeToSeabed()
        {
            if (spawnedRope || ropeID > 0 || VehicleToMonitor?.Vehicle.Exists() != true) return;
            string modelName = VehicleToMonitor.Vehicle.Model.Name.ToLower();
            BoatAnchorConfig config = GetConfigForModel(modelName);
            Vector3 boatAttach = GetBoatAttachPoint(config, modelName);
            float groundZ = GetGroundZ(boatAttach);
            Vector3 seabedPos = new Vector3(boatAttach.X, boatAttach.Y, groundZ - SeabedPinDepth);
            float baseLength = boatAttach.DistanceTo(seabedPos);
            float ropeLength = Math.Max(MinRopeLengthWithLeeway, baseLength * RopeLeewayMultiplier);
            NativeFunction.Natives.ROPE_LOAD_TEXTURES();
            GameFiber.Sleep(100);
            ropeID = NativeFunction.Natives.ADD_ROPE<int>(
                boatAttach.X, boatAttach.Y, boatAttach.Z,
                -90f, 0f, 0f,
                ropeLength, 1, ropeLength, 0.0f, 0.3f,
                false, true, true, 1.0f, false
            );
            CreatedRopes.Add(ropeID);
            if (ropeID <= 0)
            {
                Game.LogTrivial($"[Anchor] ADD_ROPE failed for {modelName}");
                return;
            }
            NativeFunction.Natives.SET_DISABLE_BREAKING(ropeID, true);
            GameFiber.Sleep(50);
            // More vertices = smoother sag
            try { NativeFunction.Natives.SET_ROPE_VERTEX_COUNT(ropeID, RopeVertexCount); }
            catch { }
            NativeFunction.Natives.ATTACH_ROPE_TO_ENTITY(ropeID, VehicleToMonitor.Vehicle,
                boatAttach.X, boatAttach.Y, boatAttach.Z, false);
            NativeFunction.Natives.PIN_ROPE_VERTEX(ropeID,
                NativeFunction.Natives.GET_ROPE_VERTEX_COUNT<int>(ropeID) - 1,
                seabedPos.X, seabedPos.Y, seabedPos.Z);
            NativeFunction.Natives.ROPE_SET_UPDATE_PINVERTS(ropeID);
            NativeFunction.Natives.ROPE_FORCE_LENGTH(ropeID, ropeLength);
            NativeFunction.Natives.ACTIVATE_PHYSICS(ropeID);
            spawnedRope = true;

            if (!ActiveAnchors.Contains(this))
                ActiveAnchors.Add(this);
            //Game.LogTrivial($"[Anchor] Deployed with {RopeLeewayMultiplier:F1}x leeway | Length: {ropeLength:F1}m | Vertices: {RopeVertexCount}");
        }
        private Vector3 GetBoatAttachPoint(BoatAnchorConfig config, string modelName)
        {
            if (!string.IsNullOrEmpty(config.BoneName) && VehicleToMonitor.Vehicle.HasBone(config.BoneName))
            {
                int boneIndex = NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", VehicleToMonitor.Vehicle, config.BoneName);
                Vector3 attachBonePos = VehicleToMonitor.Vehicle.GetBonePosition(boneIndex);
                Game.LogTrivial($"[Anchor] Using bone '{config.BoneName}' for {modelName}");
                return attachBonePos;
            }
            Vector3 attachOffsetPos = VehicleToMonitor.Vehicle.GetOffsetPosition(config.Offset);
            Game.LogTrivial($"[Anchor] Using offset {config.Offset} for {modelName}");
            return attachOffsetPos;
        }
        private BoatAnchorConfig GetConfigForModel(string modelName)
        {
            if (AnchorConfigs.TryGetValue(modelName, out BoatAnchorConfig config))
                return config;
            return new BoatAnchorConfig { Offset = new Vector3(0f, 0f, 0f), BoneName = null };
        }
        private float GetGroundZ(Vector3 pos)
        {
            // Cast downward through water to find the seabed
            HitResult hit = World.TraceLine(
                pos + new Vector3(0f, 0f, 5f),
                pos + new Vector3(0f, 0f, -200f),
                TraceFlags.IntersectWorld,
                VehicleToMonitor.Vehicle
            );
            if (hit.Hit)
                return hit.HitPosition.Z;
            // Fallback if something is wrong
            return pos.Z - 15f;
        }
        private void DeleteRope()
        {
            if (ropeID > 0 && spawnedRope)
            {
                unsafe
                {
                    int id = ropeID;
                    NativeFunction.CallByName<bool>("DELETE_ROPE", &id);
                }
                ropeID = 0;
                spawnedRope = false;
            }
        }
        public void Update()
        {
            if (VehicleToMonitor?.Vehicle.Exists() != true || !VehicleToMonitor.IsBoat)
                return;
            if (Game.GameTime - GameTimeLastUpdated < UpdateCooldown)
                return;
            float playerDist = Game.LocalPlayer.Character.DistanceTo(VehicleToMonitor.Vehicle.Position);
            if (playerDist < MinPlayerDistance)
            {
                GameTimeLastUpdated = Game.GameTime;
                return;
            }
            if (IsDeployed)
            {
                bool isAnchored = NativeFunction.Natives.IS_BOAT_ANCHORED<bool>(VehicleToMonitor.Vehicle);
                if (!isAnchored && lastAnchorState)
                {
                    NativeFunction.Natives.SET_BOAT_ANCHOR(VehicleToMonitor.Vehicle, true);
                    NativeFunction.Natives.SET_BOAT_REMAINS_ANCHORED_WHILE_PLAYER_IS_DRIVER(VehicleToMonitor.Vehicle, true);
                    NativeFunction.Natives.SET_BOAT_LOW_LOD_ANCHOR_DISTANCE(VehicleToMonitor.Vehicle, 100.0f);
                    if (!spawnedRope || ropeID <= 0)
                        SpawnRopeToSeabed();
                }
                lastAnchorState = isAnchored;
            }
            GameTimeLastUpdated = Game.GameTime;
        }
        public void Cleanup()
        {
            foreach (int id in CreatedRopes)
            {
                int rope = id;
                if (rope != 0)
                {
                    NativeFunction.Natives.DELETE_ROPE(ref rope);
                }
            }
            CreatedRopes.Clear();
        }
        public void Dispose()
        {
            Cleanup();
            DeleteRope();
        }
        public static void CleanupAllRopes()
        {
            foreach (var anchor in ActiveAnchors.ToArray())
            {
                try
                {
                    anchor.Dispose();
                }
                catch { }
            }
            ActiveAnchors.Clear();
        }
    }
    internal class BoatAnchorConfig
    {
        public Vector3 Offset { get; set; }
        public string BoneName { get; set; }
    }
}