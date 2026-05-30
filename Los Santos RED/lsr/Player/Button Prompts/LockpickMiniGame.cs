using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LosSantosRED.lsr.Interface;


public class LockpickMiniGame
{
    private int scaleformHandle;
    private IInteractionable Player;
    // --- Game State Variables ---

    private int currentPin = 1;  // Pin 1 to 3
    private int pinStep = 1;     // Step 1 to 5 per pin

    // Angles and Progress
    private float targetL, targetR;
    private float playerL = 0f, playerR = 0f;
    private float progress = 0f;
    private float ZoneWidth = 20f; // decrease to make harder, increase to make easier
    private Random rnd = new Random();

    // Timing
    private float FillSpeed = 1.0f;

    // Scaleform
    private Scaleform yogaScaleform;

    private int TotalPinSteps = 3;
    private int TotalPins = 2;
    public LockpickMiniGame(IInteractionable player)
    {
        Player = player;
    }
    public LockpickMiniGame(IInteractionable player, int totalPins, int totalPinSteps, float zoneWidth, float fillSpeed)
    {
        Player = player;
        TotalPinSteps = totalPinSteps;
        TotalPins = totalPins;
        ZoneWidth = zoneWidth;
        FillSpeed = fillSpeed;
    }

    public bool HasPickedLock { get; private set; }
    public bool IsActive { get; private set; }
    public void Start()
    {
        IsActive = true;

        string scaleFormToLoad = "yoga_buttons";
        if(Player.IsUsingController)
        {
            scaleFormToLoad = "yoga_keys";
        }
        scaleformHandle = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>(scaleFormToLoad);
        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(scaleformHandle))
        {
            GameFiber.Yield();
        }
        yogaScaleform = new Scaleform(scaleformHandle);
        ResetPin();

        NativeFunction.CallByName<bool>("REQUEST_AMBIENT_AUDIO_BANK", "SAFE_CRACK", true);

        Game.RawFrameRender += OnRawFrameRender;
        GameFiber.StartNew(MainLoop);
    }
    public void MainLoop()
    {
        while (true)
        {
            GameFiber.Yield();
            if (Game.IsControlJustPressed(0, GameControl.Aim) || NativeFunction.Natives.x91AEF906BCA88877<bool>(0, 25))
            {
                IsActive = false;
                break;
            }
            if (IsActive)
            {
                //Game.DisplaySubtitle($"I AM RUNNING THE SCALEFORM{Game.GameTime}");
                DisablePlayerControls();

                // 2. Input Capture
                float lx = NativeFunction.CallByName<float>("GET_CONTROL_NORMAL", 0, 218);
                float ly = NativeFunction.CallByName<float>("GET_CONTROL_NORMAL", 0, 219);
                float rx = NativeFunction.CallByName<float>("GET_CONTROL_NORMAL", 0, 220);
                float ry = NativeFunction.CallByName<float>("GET_CONTROL_NORMAL", 0, 221);

                // Smooth stick movement
                if (Math.Abs(lx) > 0.1f || Math.Abs(ly) > 0.1f)
                    playerL = LerpAngle(playerL, (float)(Math.Atan2(ly, lx) * 180 / Math.PI) + 90f, 0.2f);
                if (Math.Abs(rx) > 0.1f || Math.Abs(ry) > 0.1f)
                    playerR = LerpAngle(playerR, (float)(Math.Atan2(ry, rx) * 180 / Math.PI) + 90f, 0.2f);

                UpdateScaleforms();

                // 3. Logic: Check Alignment
                float diffL = Math.Abs(NormalizeAngle(playerL - targetL));
                float diffR = Math.Abs(NormalizeAngle(playerR - targetR));

                // Increment progress if aligned, else decay progress
                if (diffL <= ZoneWidth && diffR <= ZoneWidth)
                    progress = Math.Min(100f, progress + FillSpeed);
                else
                    progress = Math.Max(0f, progress - (FillSpeed * 1.5f));

                // 4. Pin/Step Progression System
                if (progress >= 100f)
                {
                    pinStep++;
                    progress = 0f;
                    if (pinStep <= TotalPinSteps)
                    {
                        PlayLockpickSound("TUMBLER_TURN", "SAFE_CRACK_SOUNDSET");
                        targetR = (float)rnd.NextDouble() * 360f;
                        //Game.DisplayNotification($"Pin {currentPin} - Step {pinStep}/{TotalPinSteps}");
                    }
                    else
                    {
                        currentPin++;
                        pinStep = 1;
                        if (currentPin <= TotalPins)
                        {
                            PlayLockpickSound("TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET");
                            ResetPin();
                            Game.DisplayNotification($"Moving to Pin {currentPin}/{TotalPins}");
                        }
                        else
                        {
                            PlayLockpickSound("TUMBLER_RESET", "SAFE_CRACK_SOUNDSET");
                            IsActive = false;
                            HasPickedLock = true;
                            //Game.DisplayNotification("Lock Picked!");
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
        Dispose();
    }
    private void UpdateScaleforms()
    {
        yogaScaleform.CallFunction("SET_STICK_POINTER_ANGLE", 0, targetL);
        yogaScaleform.CallFunction("SET_STICK_POINTER_ANGLE", 1, targetR);
        yogaScaleform.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 0, playerL);
        yogaScaleform.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 1, playerR);

        bool alignedL = Math.Abs(NormalizeAngle(playerL - targetL)) <= ZoneWidth;
        bool alignedR = Math.Abs(NormalizeAngle(playerR - targetR)) <= ZoneWidth;

        // Green if aligned, Red if not
        yogaScaleform.CallFunction("SET_STICK_POINTER_RGB", 0, alignedL ? 0 : 255, alignedL ? 255 : 0, 0);
        yogaScaleform.CallFunction("SET_STICK_POINTER_RGB", 1, alignedR ? 0 : 255, alignedR ? 255 : 0, 0);

        NativeFunction.CallByName<int>("DRAW_SCALEFORM_MOVIE", yogaScaleform.Handle, 0.5f, 0.88f, 0.75125f, 0.32f, 255, 255, 255, 255, 0);
    }
    private void OnRawFrameRender(object sender, Rage.GraphicsEventArgs e)
    {
        if (!IsActive)
        {
            return;
        }
        float cx = Game.Resolution.Width / 2f;
        e.Graphics.DrawRectangle(new RectangleF(cx - 100, Game.Resolution.Height * 0.86f, 200, 10), Color.Gray);
        e.Graphics.DrawRectangle(new RectangleF(cx - 100, Game.Resolution.Height * 0.86f, progress * 2, 10), Color.Green);
    }
    // --- Helper Methods ---
    private void PlayLockpickSound(string soundName, string soundSet)
    {
        NativeFunction.CallByName<int>("PLAY_SOUND_FRONTEND", -1, soundName, soundSet, 1);
    }
    private void ResetGame()
    {
        currentPin = 1; pinStep = 1; progress = 0f;
        ResetPin();
    }
    private void ResetPin()
    {
        targetL = (float)rnd.NextDouble() * 360f;
        targetR = (float)rnd.NextDouble() * 360f;
        FillSpeed = 1.0f;
    }
    private void DisablePlayerControls()
    {
        Game.DisableControlAction(0, GameControl.MoveUpDown, true);
        Game.DisableControlAction(0, GameControl.MoveLeftRight, true);
        Game.DisableControlAction(0, GameControl.LookUpDown, true);
        Game.DisableControlAction(0, GameControl.LookLeftRight, true);
    }
    private float NormalizeAngle(float angle) { angle %= 360; return angle > 180 ? angle - 360 : angle < -180 ? angle + 360 : angle; }
    private float LerpAngle(float start, float end, float amount) { return start + NormalizeAngle(end - start) * amount; }

    internal void Dispose()
    {
        IsActive = false;
        Game.RawFrameRender -= OnRawFrameRender;
    }
}

