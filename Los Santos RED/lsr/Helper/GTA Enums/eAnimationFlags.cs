using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum eAnimationFlags
{
    AF_DEFAULT = 0,                                     //                  
    AF_LOOPING = 1,                                     // Repeat the animation 
    AF_HOLD_LAST_FRAME = 2,                             // Hold on the last frame of the animation 
    AF_REPOSITION_WHEN_FINISHED = 4,                    // When the animation finishes pop the peds physical reprsentation position to match the visual representations position Note that the animator must not unwind the animation and must have an independent mover node 
    AF_NOT_INTERRUPTABLE = 8,                           // Can the task not be interupted by extenal events 
    AF_UPPERBODY = 16,                                  // Only plays the upper body part of the animation. Dampens any motion caused by the lower body animation.Note that the animation should include the root node 
    AF_SECONDARY = 32,                                  // The task will run in the secondary task slot. This means it can be used aswell as a movement task (for instance) 
    AF_REORIENT_WHEN_FINISHED = 64,                     // When the animation finishes pop the peds physical reprsentation direction to match the visual representations direction. Note that the animator must not unwind the animation and must have an independent mover node 
    AF_ABORT_ON_PED_MOVEMENT = 128,                     // Ends the animation early if the ped attemps to move e.g. if the player tries to move using the controller. Can also be used to blend out automatically when an ai ped starts moving by combining it with the AF_SECONDARY flag.
    AF_ADDITIVE = 256,                                  // Play back the animation additively. Note, this will only produce sensible results on specifically authored additive animations!
    AF_TURN_OFF_COLLISION = 512,                        // Do not react to collision detection whilst this anim is playing
    AF_OVERRIDE_PHYSICS = 1024,                         // Do not apply any physics forces whilst the anim is playing. Automatically turns off collision, extracts any initial offset provided in the clip and uses per frame mover extraction.
    AF_IGNORE_GRAVITY = 2048,                           // Do not apply gravity while the anim is playing
    AF_EXTRACT_INITIAL_OFFSET = 4096,                   // Extract an initial offset from the playback position authored by the animator
                                                        // Use this flag when playing back anims on different peds which have been authored
                                                        // to sync with each other
    AF_EXIT_AFTER_INTERRUPTED = 8192,                   // Exit the animation task if it is interrupted by another task (ie Natural Motion).  Without this flag bing set looped animations will restart ofter the NM task

    // Tag synchronizer flags - sync the anim against ped movement (walking / running / etc)
    AF_TAG_SYNC_IN = 16384,                             // Sync the anim whilst blending in (use for seamless transitions from walk / run into a full body anim)
    AF_TAG_SYNC_OUT = 32768,                            // Sync the anim whilst blending out (use for seamless transitions from a full body anim into walking / running behaviour)
    AF_TAG_SYNC_CONTINUOUS = 65536,                     // Sync all the time (Only usefull to synchronize a partial anim e.g. an upper body)

    AF_FORCE_START = 131072,                            // Force the anim task to start even if the ped is falling / ragdolling / etc. Can fix issues with peds not playing their anims immediately after a warp / etc
    AF_USE_KINEMATIC_PHYSICS = 262144,                  // Use the kinematic physics mode on the entity for the duration of the anim (it should push other entities out of the way, and not be pushed around by players / etc
    AF_USE_MOVER_EXTRACTION = 524288,                   // Updates the peds capsule position every frame based on the animation. Use in conjunction with AF_USE_KINEMATIC_PHYSICS to create characters that cannot be pushed off course by other entities / geometry / etc whilst playing the anim.

    AF_HIDE_WEAPON = 1048576,                           // Indicates that the ped's weapon should be hidden while this animation is playing.

    AF_ENDS_IN_DEAD_POSE = 2097152,                     // When the anim ends, kill the ped and use the currently playing anim as the dead pose
    AF_ACTIVATE_RAGDOLL_ON_COLLISION = 4194304,         // If the peds ragdoll bounds make contact with something physical (that isn't flat ground) activate the ragdoll and fall over.
    AF_DONT_EXIT_ON_DEATH = 8388608,					// Currently used only on secondary anim tasks. Secondary anim tasks will end automatically when the ped dies. Setting this flag stops that from happening."
    AF_ABORT_ON_WEAPON_DAMAGE = 16777216,				// Allow aborting from damage events (including non-ragdoll damage events) even when blocking other ai events using AF_NOT_INTERRUPTABLE.
    AF_DISABLE_FORCED_PHYSICS_UPDATE = 33554432,        // Prevent adjusting the capsule on the enter state (useful if script is doing a sequence of scripted anims and they are known to more or less stand still) 
    AF_PROCESS_ATTACHMENTS_ON_START = 67108864,         // Force the attachments to be processed at the start of the clip
    AF_EXPAND_PED_CAPSULE_FROM_SKELETON = 134217728,    // Expands the capsule to the extents of the skeleton
    AF_USE_ALTERNATIVE_FP_ANIM = 268435456,             // Plays an alternative first person version of the clip on the player when in first person mode (the first person clip must be in the same dictionary, and be named the same as the anim you're playing, but with _FP appended on the end)
    AF_BLENDOUT_WRT_LAST_FRAME = 536870912,             // Start blending out the anim early, so that the blend out duration completes at the end of the animation.
    AF_USE_FULL_BLENDING = 1073741824                 	// Use full blending for this anim and override the heading/position adjustment in CTaskScriptedAnimation::CheckIfClonePlayerNeedsHeadingPositionAdjust(), so that we don't correct errors (special case such as scrip-side implemented AI tasks, i.e. diving)
}

