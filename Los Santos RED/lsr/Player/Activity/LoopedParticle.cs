using Rage;
using Rage.Native;
using System.Drawing;

namespace LosSantosRED.lsr.Player.Activity
{
    public class LoopedParticle//entire class is from alexguirre
    {
        public LoopedParticle(string assetName, string particleName, Ped ped, PedBoneId bone, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedOnPedBone<uint>(particleName,ped,offset.X, offset.Y, offset.Z,rotation.Pitch, rotation.Roll, rotation.Yaw,ped.GetBoneIndex(bone),scale,false, false, false);
        }
        public LoopedParticle(string assetName, string particleName, Entity entity, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedOnEntity<uint>(particleName,entity,offset.X, offset.Y, offset.Z,rotation.Pitch, rotation.Roll, rotation.Yaw,scale,false, false, false);
        }
        public LoopedParticle(string assetName, string particleName, Entity entity, int boneIndex, Vector3 offset, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.xC6EB449E33977F0B<uint>(particleName,entity,offset.X, offset.Y, offset.Z,rotation.Pitch, rotation.Roll, rotation.Yaw,boneIndex,scale,false, false, false); // _START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE
        }
        //public LoopedParticle(string assetName, string particleName, Entity entity, string boneName, Vector3 offset, Rotator rotation, float scale): this(assetName, particleName, entity, entity.GetBoneIndex(boneName), offset, rotation, scale)
        //{

        //}
        public LoopedParticle(string assetName, string particleName, Vector3 position, Rotator rotation, float scale)
        {
            AssetName = assetName;
            ParticleName = particleName;
            LoadAsset();
            Handle = NativeFunction.Natives.StartParticleFxLoopedAtCoord<uint>(particleName,position.X, position.Y, position.Z,rotation.Pitch, rotation.Roll, rotation.Yaw,scale,false, false, false, false);
        }
        public string AssetName { get; }
        public PoolHandle Handle { get; }
        public string ParticleName { get; }
        public bool IsValid()
        {
            return NativeFunction.Natives.DoesParticleFxLoopedExist<bool>(Handle.Value);
        }
        public void SetColor(Color color)
        {
            NativeFunction.Natives.SetParticleFxLoopedColour(Handle.Value, color.R / 255f, color.G / 255f, color.B / 255f, false);
            NativeFunction.Natives.SetParticleFxLoopedAlpha(Handle.Value, color.A / 255f);
        }
        public void SetOffsets(Vector3 offset, Rotator rotation)
        {
            NativeFunction.Natives.SetParticleFxLoopedOffsets(Handle.Value,offset.X, offset.Y, offset.Z,rotation.Pitch, rotation.Roll, rotation.Yaw);
        }
        public void SetRange(float range)
        {
            NativeFunction.Natives.xDCB194B85EF7B541(Handle.Value, range); // _SET_PARTICLE_FX_LOOPED_RANGE
        }
        public void SetScale(float scale)
        {
            NativeFunction.Natives.SetParticleFxLoopedScale(Handle.Value, scale);
        }
        public void Stop()
        {
            NativeFunction.Natives.StopParticleFxLooped(Handle.Value, false);
        }
        private void LoadAsset()
        {
            NativeFunction.Natives.RequestNamedPtfxAsset(AssetName);
            int waitCounter = 10;
            while (!NativeFunction.Natives.HasNamedPtfxAssetLoaded<bool>(AssetName) && waitCounter > 0)
            {
                GameFiber.Sleep(10);
                waitCounter--;
            }
            NativeFunction.Natives.x6C38AF3693A69A91(AssetName); // _SET_PTFX_ASSET_NEXT_CALL
        }
    }
}