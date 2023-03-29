using System.Collections.Generic;
using System.Linq;

namespace LosSantosRED.lsr.Data
{
    public class Bones
    {
        public readonly List<Bone> PedBones = new List<Bone>();
        public Bones()
        {
            PedBones = new List<Bone>
        {
            new Bone("SKEL_ROOT", 4215, 0, BodyLocation.LowerTorso),
            new Bone("SKEL_Pelvis", 4103, 11816, BodyLocation.LowerTorso),
            new Bone("SKEL_L_Thigh", 4103, 58271, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Calf", 4103, 63931, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Foot", 4103, 14201, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Toe0", 7, 2108, BodyLocation.LeftLeg),
            new Bone("IK_L_Foot", 119, 65245, BodyLocation.LeftLeg),
            new Bone("PH_L_Foot", 119, 57717, BodyLocation.LeftLeg),
            new Bone("MH_L_Knee", 119, 46078, BodyLocation.LeftLeg),
            new Bone("SKEL_R_Thigh", 4103, 51826, BodyLocation.RightLeg),
            new Bone("SKEL_R_Calf", 4103, 36864, BodyLocation.RightLeg),
            new Bone("SKEL_R_Foot", 4103, 52301, BodyLocation.RightLeg),
            new Bone("SKEL_R_Toe0", 7, 20781, BodyLocation.RightLeg),
            new Bone("IK_R_Foot", 119, 35502, BodyLocation.RightLeg),
            new Bone("PH_R_Foot", 119, 24806, BodyLocation.RightLeg),
            new Bone("MH_R_Knee", 119, 16335, BodyLocation.RightLeg),
            new Bone("RB_L_ThighRoll", 7, 23639, BodyLocation.LeftLeg),
            new Bone("RB_R_ThighRoll", 7, 6442, BodyLocation.RightLeg),
            new Bone("SKEL_Spine_Root", 4103, 57597, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine0", 4103, 23553, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine1", 4103, 24816, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine2", 4103, 24817, BodyLocation.UpperTorso),
            new Bone("SKEL_Spine3", 4103, 24818, BodyLocation.UpperTorso),
            new Bone("SKEL_L_Clavicle", 4103, 64729, BodyLocation.LeftArm),
            new Bone("SKEL_L_UpperArm", 4103, 45509, BodyLocation.LeftArm),
            new Bone("SKEL_L_Forearm", 4215, 61163, BodyLocation.LeftArm),
            new Bone("SKEL_L_Hand", 4215, 18905, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger00", 4103, 26610, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger01", 4103, 4089, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger02", 7, 4090, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger10", 4103, 26611, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger11", 4103, 4169, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger12", 7, 4170, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger20", 4103, 26612, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger21", 4103, 4185, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger22", 7, 4186, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger30", 4103, 26613, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger31", 4103, 4137, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger32", 7, 4138, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger40", 4103, 26614, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger41", 4103, 4153, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger42", 7, 4154, BodyLocation.LeftArm),
            new Bone("PH_L_Hand", 119, 60309, BodyLocation.LeftArm),//
            new Bone("IK_L_Hand", 119, 36029, BodyLocation.LeftArm),
            new Bone("RB_L_ForeArmRoll", 7, 61007, BodyLocation.LeftArm),
            new Bone("RB_L_ArmRoll", 7, 5232, BodyLocation.LeftArm),
            new Bone("MH_L_Elbow", 119, 22711, BodyLocation.LeftArm),
            new Bone("SKEL_R_Clavicle", 4103, 10706, BodyLocation.RightArm),
            new Bone("SKEL_R_UpperArm", 4103, 40269, BodyLocation.RightArm),
            new Bone("SKEL_R_Forearm", 4215, 28252, BodyLocation.RightArm),
            new Bone("SKEL_R_Hand", 4215, 57005, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger00", 4103, 58866, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger01", 4103, 64016, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger02", 7, 64017, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger10", 4103, 58867, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger11", 4103, 64096, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger12", 7, 64097, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger20", 4103, 58868, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger21", 4103, 64112, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger22", 7, 64113, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger30", 4103, 58869, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger31", 4103, 64064, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger32", 7, 64065, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger40", 4103, 58870, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger41", 4103, 64080, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger42", 7, 64081, BodyLocation.RightArm),
            new Bone("PH_R_Hand", 119, 28422, BodyLocation.RightArm),//
            new Bone("IK_R_Hand", 119, 6286, BodyLocation.RightArm),
            new Bone("RB_R_ForeArmRoll", 7, 43810, BodyLocation.RightArm),
            new Bone("RB_R_ArmRoll", 7, 37119, BodyLocation.RightArm),
            new Bone("MH_R_Elbow", 119, 2992, BodyLocation.RightArm),
            new Bone("SKEL_Neck_1", 4103, 39317, BodyLocation.Neck),
            new Bone("SKEL_Head", 4103, 31086, BodyLocation.Head),
            new Bone("IK_Head", 119, 12844, BodyLocation.Head),
            new Bone("FACIAL_facialRoot", 4103, 65068, BodyLocation.Head),
            new Bone("FB_L_Brow_Out_000", 1799, 58331, BodyLocation.Head),
            new Bone("FB_L_Lid_Upper_000", 1911, 45750, BodyLocation.Head),
            new Bone("FB_L_Eye_000", 1799, 25260, BodyLocation.Head),
            new Bone("FB_L_CheekBone_000", 1799, 21550, BodyLocation.Head),
            new Bone("FB_L_Lip_Corner_000", 1911, 29868, BodyLocation.Head),
            new Bone("FB_R_Lid_Upper_000", 1911, 43536, BodyLocation.Head),
            new Bone("FB_R_Eye_000", 1799, 27474, BodyLocation.Head),
            new Bone("FB_R_CheekBone_000", 1799, 19336, BodyLocation.Head),
            new Bone("FB_R_Brow_Out_000", 1799, 1356, BodyLocation.Head),
            new Bone("FB_R_Lip_Corner_000", 1911, 11174, BodyLocation.Head),
            new Bone("FB_Brow_Centre_000", 1799, 37193, BodyLocation.Head),
            new Bone("FB_UpperLipRoot_000", 5895, 20178, BodyLocation.Head),
            new Bone("FB_UpperLip_000", 6007, 61839, BodyLocation.Head),
            new Bone("FB_L_Lip_Top_000", 1911, 20279, BodyLocation.Head),
            new Bone("FB_R_Lip_Top_000", 1911, 17719, BodyLocation.Head),
            new Bone("FB_Jaw_000", 5895, 46240, BodyLocation.Head),
            new Bone("FB_LowerLipRoot_000", 5895, 17188, BodyLocation.Head),
            new Bone("FB_LowerLip_000", 6007, 20623, BodyLocation.Head),
            new Bone("FB_L_Lip_Bot_000", 1911, 47419, BodyLocation.Head),
            new Bone("FB_R_Lip_Bot_000", 1911, 49979, BodyLocation.Head),
            new Bone("FB_Tongue_000", 1911, 47495, BodyLocation.Head),
            new Bone("RB_Neck_1", 7, 35731, BodyLocation.Neck),
            new Bone("IK_Root", 119, 56604, BodyLocation.LowerTorso)
        };
        }
        public Bone GetBone(int ID)
        {
            return PedBones.FirstOrDefault(x => x.Tag == ID);
        }
    }
}