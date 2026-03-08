using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAGENativeUI.Elements;
using LosSantosRED.lsr.Interface;


public class GenericMinigame
{
    private int globalScaleformIDKeys;
    private int globalScaleformIDButtons;
    private IInputable Player;



    private static readonly string YogaMatress = "prop_yoga_mat_03";
    //private static Prop Matress;
    private static Scaleform YogaButtons;
    private static Scaleform YogaKeys;
    private static Camera YogaCam;
    private static Vector3 Coords = new Vector3(-791.0036f, 186.3552f, 71.8295f);
    private static int iLocal_613 = -1;
    private static Camera uLocal_612;
    private static bool someBool = true;
    private static bool switchButtons = false;
    private static int iLocal_450 = 0;
    private static int iLocal_451 = 0;

    private static int Paramf_29 = 0;
    private static int Paramf_31 = 0;
    private static int Paramf_30 = 0;
    private static int Paramf_32 = 0;
    public void Start(IInputable player)
    {
        Player = player;
        globalScaleformIDKeys = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("yoga_keys");
        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformIDKeys))
        {
            GameFiber.Yield();
        }
        globalScaleformIDButtons = NativeFunction.Natives.REQUEST_SCALEFORM_MOVIE<int>("yoga_buttons");
        while (!NativeFunction.Natives.HAS_SCALEFORM_MOVIE_LOADED<bool>(globalScaleformIDButtons))
        {
            GameFiber.Yield();
        }

        YogaKeys = new Scaleform(globalScaleformIDKeys);
        YogaButtons = new Scaleform(globalScaleformIDButtons);
    }


    public void Update()
    {
        //SetPlayerControl(PlayerId(), false, 0);
        //while (!YogaButtons.IsLoaded) await BaseScript.Delay(1);
        //while (!YogaKeys.IsLoaded) await BaseScript.Delay(1);
        //if (IsControlJustPressed(0, 223))
        //{
        //    switchButtons = !switchButtons;
        //}
        //if (IsHelpMessageDisplayed("STICKS_KM") || IsHelpMessageDisplayed("STICKS"))
        //{

        NativeFunction.Natives.SET_PLAYER_CONTROL(Game.LocalPlayer, false, 0);


            if (IsInputDisabled(2))
            {
                func_346(ref Paramf_29, ref Paramf_31, 0, 180, 180);
                func_332(YogaKeys);
            }
            else
            {
                func_346(ref Paramf_30, ref Paramf_32, 1, 180, 180);
                func_332(YogaButtons);
            }
        //}
        if (!Player.IsUsingController)
            NativeFunction.Natives.DRAW_SCALEFORM_MOVIE(YogaKeys.Handle, 0.5f, 0.88f, 0.609375f * 1.2f, 0.266666f * 1.2f, 100, 100, 100, 255, 0);
        //uLocal_440
        else
            NativeFunction.Natives.DRAW_SCALEFORM_MOVIE(YogaButtons.Handle, 0.5f, 0.88f, 0.609375f * 1.2f, 0.266666f * 1.2f, 100, 100, 100, 255, 0);


        //uLocal_439
    }


    private void func_332(Scaleform scale)
    {
        int r = 255;
        int g = 255;
        int b = 255;
        int a = 255;
        //GetHudColour(129, ref r, ref g, ref b, ref a);
        if (IsInputDisabled(2))
        {
            if (switchButtons)
            {
                scale.CallFunction("REPLACE_KEYS_WITH_STICK", 0);
                scale.CallFunction("REPLACE_STICK_WITH_KEYS", 1);
            }
            else
            {
                scale.CallFunction("REPLACE_STICK_WITH_KEYS", 0);
                scale.CallFunction("REPLACE_KEYS_WITH_STICK", 1);
            }
        }
        scale.CallFunction("SET_STICK_POINTER_ANGLE", 0, 180);
        scale.CallFunction("SET_STICK_POINTER_ANGLE", 1, 180);
        scale.CallFunction("SET_STICK_POINTER_RGB", 0, r, g, b);
        scale.CallFunction("SET_STICK_POINTER_RGB", 1, r, g, b);

        //Paramf_30 = 270;


        if (Paramf_31 == 1)
            scale.CallFunction("HIDE_STICK_POINTER", 0);
        else
            scale.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 0, Paramf_29);
        if (Paramf_31 == 1)
            scale.CallFunction("HIDE_STICK_POINTER", 1);
        else
            scale.CallFunction("SET_STICK_POINTER_HIGHLIGHT_ANGLE", 1, Paramf_30);


        Game.DisplaySubtitle($"Paramf_31-{Paramf_31} Paramf_29-{Paramf_29} Paramf_30-{Paramf_30}");

    }

    private void func_348(ref int uParam0, ref int uParam1, ref int uParam2, ref int uParam3, bool bParam4)
    {
        uParam0 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 218) * 127f));
        uParam1 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 219) * 127f));
        uParam2 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 220) * 127f));
        uParam3 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_NORMAL<float>(2, 221) * 127f));





        if (bParam4)
        {
            if ((uParam0) == 0f && (uParam1) == 0f)
            {
                uParam0 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(2, 218) * 127f));
                uParam1 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(2, 219) * 127f));
            }
            if ((uParam2) == 0f && (uParam3) == 0f)
            {
                uParam2 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(2, 220) * 127f));
                uParam3 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_NORMAL<float>(2, 221) * 127f));
            }
        }
    }


    private int func_346(ref int uParam0, ref int uParam1, int iParam2, int iParam3, int iParam4)
    {
        uParam0 = func_349(iParam2);
        uParam1 = func_347(iParam2);
        if (uParam1 == 0)
        {
            switch (iParam4)
            {
                case 0:
                    switch (iParam3)
                    {
                        case 0:
                            if (uParam0 >= 345 || uParam0 <= 15)
                            {
                                return 1;
                            }
                            break;

                        case 45:
                            if (uParam0 >= 30 && uParam0 <= 60)
                            {
                                return 1;
                            }
                            break;

                        case 90:
                            if (uParam0 >= 75 && uParam0 <= 105)
                            {
                                return 1;
                            }
                            break;

                        case 135:
                            if (uParam0 >= 120 && uParam0 <= 150)
                            {
                                return 1;
                            }
                            break;

                        case 180:
                            if (uParam0 >= 165 && uParam0 <= 195)
                            {
                                return 1;
                            }
                            break;

                        case 225:
                            if (uParam0 >= 210 && uParam0 <= 240)
                            {
                                return 1;
                            }
                            break;

                        case 270:
                            if (uParam0 >= 255 && uParam0 <= 285)
                            {
                                return 1;
                            }
                            break;

                        case 315:
                            if (uParam0 >= 300 && uParam0 <= 330)
                            {
                                return 1;
                            }
                            break;
                    }
                    break;

                case 1:
                    switch (iParam3)
                    {
                        case 0:
                            if (uParam0 >= 305 || uParam0 <= 55)
                            {
                                return 1;
                            }
                            break;

                        case 45:
                            if (uParam0 >= 350 || uParam0 <= 100)
                            {
                                return 1;
                            }
                            break;

                        case 90:
                            if (uParam0 >= 35 && uParam0 <= 145)
                            {
                                return 1;
                            }
                            break;

                        case 135:
                            if (uParam0 >= 80 && uParam0 <= 190)
                            {
                                return 1;
                            }
                            break;

                        case 180:
                            if (uParam0 >= 125 && uParam0 <= 235)
                            {
                                return 1;
                            }
                            break;

                        case 225:
                            if (uParam0 >= 170 && uParam0 <= 280)
                            {
                                return 1;
                            }
                            break;

                        case 270:
                            if (uParam0 >= 215 && uParam0 <= 325)
                            {
                                return 1;
                            }
                            break;

                        case 315:
                            if (uParam0 >= 260 || uParam0 <= 10)
                            {
                                return 1;
                            }
                            break;
                    }
                    break;
            }
        }
        return 0;
    }
    private int func_347(int iParam0)
    {
        int iVar0 = 0;
        int iVar1 = 0;
        int iVar2 = 0;
        int iVar3 = 0;
        float fVar4;
        int uVar5 = 0;
        int uVar6 = 0;

        func_348(ref iVar0, ref iVar1, ref iVar2, ref iVar3, false);
        if (IsInputDisabled(0))
        {
            if (switchButtons)
            {
                uVar5 = iVar2;
                uVar6 = iVar3;
                iVar2 = iVar0;
                iVar3 = iVar1;
                iVar0 = uVar5;
                iVar1 = uVar6;
            }
        }
        switch (iParam0)
        {
            case 0:
                if (((IsInputDisabled(2) && iLocal_450 != 0) && iLocal_451 != 0) && switchButtons)
                {
                    return 0;
                }
                fVar4 = Vmag((iVar0), (iVar1), 0f);
                if (fVar4 < 100f)
                {
                    return 1;
                }
                break;

            case 1:
                if (((IsInputDisabled(2) && iLocal_450 != 0) && iLocal_451 != 0) && !switchButtons)
                {
                    return 0;
                }
                fVar4 = Vmag((iVar2), (iVar3), 0f);
                if (fVar4 < 100f)
                {
                    return 1;
                }
                break;
        }
        return 0;
    }

    private float Vmag(float x,float y, float z)
    {
        return (float)Math.Sqrt((x * x + y * y + z * z));
    }


    private bool IsInputDisabled(int value)
    {
        if(value == 2)
        {
            return !Player.IsUsingController;
        }
        else
        {
            return Player.IsUsingController;
        }
    }

    private int func_349(int iParam0)
    {
        int iVar0 = 0;
        int iVar1 = 0;
        int iVar2 = 0;
        int iVar3 = 0;
        int iVar4 = 0;
        int iVar5 = 0;
        int uVar6 = 0;
        int uVar7 = 0;

        func_350(ref iVar2, ref iVar3, ref iVar4, ref iVar5, false, false);
        if (IsInputDisabled(2))
        {
            if (switchButtons)
            {
                uVar6 = iVar4;
                uVar7 = iVar5;
                iVar4 = iVar2;
                iVar5 = iVar3;
                iVar2 = uVar6;
                iVar3 = uVar7;
                iVar2 = (iVar2 / 4);
                iVar3 = (iVar3 / 4);
                if (iVar2 == 0 || iVar3 == 0)
                {
                    iVar2 = iLocal_450;
                    iVar3 = iLocal_451;
                }
                iLocal_450 = iVar2;
                iLocal_451 = iVar3;
            }
            else
            {
                iVar4 = (iVar4 / 4);
                iVar5 = (iVar5 / 4);
                if (iVar4 == 0 || iVar5 == 0)
                {
                    iVar4 = iLocal_450;
                    iVar5 = iLocal_451;
                }
                iLocal_450 = iVar4;
                iLocal_451 = iVar5;
            }
        }
        iVar0 = (int)Math.Round(NativeFunction.Natives.GET_ANGLE_BETWEEN_2D_VECTORS<float>(0f, -127f, (float)(iVar2), (float)(iVar3)));
        iVar1 = (int)Math.Round(NativeFunction.Natives.GET_ANGLE_BETWEEN_2D_VECTORS<float>(0f, -127f, (float)(iVar4), (float)(iVar5)));
        if (iVar2 < 0)
        {
            iVar0 = (180 + (180 - iVar0));
        }
        if (iVar4 < 0)
        {
            iVar1 = (180 + (180 - iVar1));
        }
        switch (iParam0)
        {
            case 0:
                return iVar0;
            case 1:
                return iVar1;
        }
        return 0;
    }

    private void func_350(ref int uParam0, ref int uParam1, ref int uParam2, ref int uParam3, bool bParam4, bool bParam5)
    {
        uParam0 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_UNBOUND_NORMAL<float>(2, 218) * 127f));
        uParam1 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_UNBOUND_NORMAL<float>(2, 219) * 127f));
        uParam2 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_UNBOUND_NORMAL<float>(2, 220) * 127f));
        uParam3 = (int)Math.Floor((NativeFunction.Natives.GET_CONTROL_UNBOUND_NORMAL<float>(2, 221) * 127f));
        if (bParam4)
        {
            if (!NativeFunction.Natives.IS_CONTROL_ENABLED<bool>(2, 218))
            {
                uParam0 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_UNBOUND_NORMAL<float>(2, 218) * 127f));
            }
            if (!NativeFunction.Natives.IS_CONTROL_ENABLED<bool>(2, 219))
            {
                uParam1 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_UNBOUND_NORMAL<float>(2, 219) * 127f));
            }
            if (!NativeFunction.Natives.IS_CONTROL_ENABLED<bool>(2, 220))
            {
                uParam2 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_UNBOUND_NORMAL<float>(2, 220) * 127f));
            }
            if (!NativeFunction.Natives.IS_CONTROL_ENABLED<bool>(2, 221))
            {
                uParam3 = (int)Math.Floor((NativeFunction.Natives.GET_DISABLED_CONTROL_UNBOUND_NORMAL<float>(2, 221) * 127f));
            }
        }
        if (IsInputDisabled(2))
        {
            if (bParam5)
            {
                if (NativeFunction.Natives.IS_LOOK_INVERTED<bool>())//IsLookInverted())
                {
                    uParam3 *= -1;
                }
                if (NativeFunction.Natives.IS_MOUSE_LOOK_INVERTED<bool>())//N_0xe1615ec03b3bb4fd())
                {
                    uParam3 *= -1;
                }
            }
        }
    }


}

