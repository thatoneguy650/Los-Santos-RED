using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Fader
{
    private string LastText = "";
    private string ExistingText = "";
    private uint GameTimeDisplayChanged;
    private uint TimeToShow;
    private uint TimeToFade;
    private bool FadeIsInverse = false;
    private string DebugName = "";
    private FadeState CurrentFadeState = FadeState.FadedIn;

    private enum FadeState
    {
        FadingOut,
        FadedOut,
        FadingIn,
        FadedIn,
    }

    public int AlphaValue { get; private set; } = 255;
    public string TextToShow => ExistingText == "" ? LastText : ExistingText;
    public Fader(uint timeToShow, uint timeToFade, string debugName)
    {
        TimeToShow = timeToShow;
        TimeToFade = timeToFade;
        DebugName = debugName;
    }
    private FadeState GetFadeState(string IncomingText)
    {
        if (IncomingText != "" && ExistingText != "")
        {
            return FadeState.FadingIn;
        }
        else if (IncomingText != "")
        {

            return FadeState.FadingIn;
        }
        else
        {
            return FadeState.FadingOut;
        }
    }
    public void Update(string IncomingText)
    {


            //AlphaValue = 255;
        if (IncomingText != ExistingText)
        {
            if (IncomingText != "" && ExistingText != "")
            {
                CurrentFadeState = FadeState.FadingIn;
                FadeIsInverse = true;//just changed LET FADE AFTER TIME
                EntryPoint.WriteToConsole($"Fader {DebugName} 1 IncomingText:{IncomingText} ExistingText:{ExistingText} ", 5);
            }
            else if (IncomingText != "")
            {
                CurrentFadeState = FadeState.FadingIn;
                FadeIsInverse = true; //current is null, coming back on fading IN
                EntryPoint.WriteToConsole($"Fader {DebugName} 2 IncomingText:{IncomingText} ExistingText:{ExistingText} ", 5);
            }
            else
            {
                CurrentFadeState = FadeState.FadingOut;
               // FadeIsInverse = false;//old is null, going off fading OUT
               if(IncomingText == "" && AlphaValue == 0)
                {
                    ExistingText = "";
                }


                //if(AlphaValue == 0)
                //{
                //    FadeIsInverse = false;
                //}
                //else
                //{
                //    FadeIsInverse = false;
                //}
                EntryPoint.WriteToConsole($"Fader {DebugName} 3 IncomingText:{IncomingText} ExistingText:{ExistingText} AlphaValue {AlphaValue} ", 5); 
            }
            LastText = ExistingText;
            ExistingText = IncomingText;
            GameTimeDisplayChanged = Game.GameTime;
            EntryPoint.WriteToConsole($"Fader {DebugName} Changed", 5);
         }

        //if(FadeIsInverse && AlphaValue == 255)
        //{
        //    FadeIsInverse = false;
        //    EntryPoint.WriteToConsole($"Fader {DebugName} RAN FadeIsInverse && AlphaValue == 255 ", 5);
        //}
        if (ExistingText != "" && !FadeIsInverse)
        {
            AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
        }
        else
        {
            uint FadeTime = TimeToFade;
            if (FadeIsInverse)
            {
                FadeTime /= 2;
            }
            AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, FadeTime);
        }



        if (FadeIsInverse)
        {
            AlphaValue = 255 - AlphaValue;

            if(AlphaValue == 0)
            {
                FadeIsInverse = false;
            }
        }
        //if (AlphaValue == 0)
        //{
        //    ExistingText = "";
        //    EntryPoint.WriteToConsole($"Fader {DebugName} RAN RESET EXISTING TEXT ", 5);
        //}
    }
    private int CalculateAlpha(uint GameTimeLastChanged, uint timeToShow, uint fadeTime)
    {
        uint TimeSinceChanged = Game.GameTime - GameTimeLastChanged;
        if (TimeSinceChanged < timeToShow)
        {
            return 255;
        }
        else if (TimeSinceChanged < timeToShow + fadeTime)
        {
            float percentVisible = 1f - (1f * (TimeSinceChanged - timeToShow)) / (1f * (fadeTime));
            float alphafloat = percentVisible * 255f;
            return ((int)Math.Floor(alphafloat)).Clamp(0, 255);
        }
        else
        {
            return 0;
        }
    }
    //public void Update(string IncomingText)
    //{
    //    //AlphaValue = 255;
    //    if (IncomingText != ExistingText)
    //    {
    //        if (IncomingText != "" && ExistingText != "")
    //        {
    //            FadeIsInverse = false;//just changed LET FADE AFTER TIME
    //        }
    //        else if (IncomingText != "")
    //        {
    //            FadeIsInverse = true; //current is null, coming back on fading IN
    //        }
    //        else
    //        {
    //            FadeIsInverse = false;//old is null, going off fading OUT
    //        }
    //        LastText = ExistingText;
    //        ExistingText = IncomingText;
    //        GameTimeDisplayChanged = Game.GameTime;
    //    }

    //    if (FadeIsInverse && AlphaValue == 255)
    //    {
    //        FadeIsInverse = false;
    //    }
    //    if (ExistingText != "" && !FadeIsInverse)
    //    {
    //        AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
    //    }
    //    else
    //    {
    //        uint FadeTime = TimeToFade;
    //        if (FadeIsInverse)
    //        {
    //            FadeTime /= 2;
    //        }
    //        AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, FadeTime);
    //    }
    //    if (FadeIsInverse)
    //    {
    //        AlphaValue = 255 - AlphaValue;
    //    }
    //}
    //private int CalculateAlpha(uint GameTimeLastChanged, uint timeToShow, uint fadeTime)
    //{
    //    uint TimeSinceChanged = Game.GameTime - GameTimeLastChanged;
    //    if (TimeSinceChanged < timeToShow)
    //    {
    //        return 255;
    //    }
    //    else if (TimeSinceChanged < timeToShow + fadeTime)
    //    {
    //        float percentVisible = 1f - (1f * (TimeSinceChanged - timeToShow)) / (1f * (fadeTime));
    //        float alphafloat = percentVisible * 255f;
    //        return ((int)Math.Floor(alphafloat)).Clamp(0, 255);
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}
}

