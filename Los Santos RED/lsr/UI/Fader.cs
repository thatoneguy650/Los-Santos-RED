using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public class Fader
{
    private uint GameTimeDisplayChanged;
    private uint TimeToShow;
    private uint TimeToFade;
    private bool FadeIsInverse = false;
    private string DebugName = "";

    private string CurrentValue;
    private string CurrentDisplay;
    private enum FadeState
    {
        FadingOut,
        FadedOut,
        FadingIn,
        FadedIn,
    }
    public bool TextChangedLastUpdate { get; private set; } = false;
    public int AlphaValue { get; private set; } = 255;
    public string TextToShow { get; private set; } = "";
    public Fader(uint timeToShow, uint timeToFade, string debugName)
    {
        TimeToShow = timeToShow;
        TimeToFade = timeToFade;
        DebugName = debugName;
    }

    public void Update(string incomingValue, string incomingDisplay)
    {
        if(incomingValue != CurrentValue)
        {
            TextChangedLastUpdate = true;
            OnValueChanged(incomingValue, incomingDisplay);
        }
        else
        {
            if(incomingDisplay != CurrentDisplay)
            {
                OnDisplayChanged(incomingDisplay);
            }
            TextChangedLastUpdate = false;
            if (FadeIsInverse && AlphaValue == 255)//I AM FULLY FADED IN
            {
                OnFullyFadedIn();
            }
        }
        DetermineTransaprency();
    }
    private void DetermineTransaprency()
    {
        if (FadeIsInverse)
        {
            AlphaValue = 255 - CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade / 2);
        }
        else
        {
            if (TextToShow != "" && CurrentValue == "")
            {
                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade);
            }
            else
            {
                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
            }
        }
    }
    private void OnValueChanged(string incomingValue, string incomingDisplay)
    {
        if(incomingValue != "" && CurrentValue != "")
        {
            CurrentValue = incomingValue;
            CurrentDisplay = incomingDisplay;
            TextToShow = incomingDisplay;
            FadeIsInverse = true;
        }
        else if(incomingValue != "")
        {
            CurrentValue = incomingValue;
            CurrentDisplay = incomingDisplay;
            TextToShow = incomingDisplay;
            FadeIsInverse = true;
        }
        else
        {
            if(CurrentValue == "")
            {
                TextToShow = incomingDisplay;
            }
            CurrentValue = incomingValue;
            CurrentDisplay = incomingDisplay;
            FadeIsInverse = false;
        }
        GameTimeDisplayChanged = Game.GameTime;//Environment.TickCount;
        TextChangedLastUpdate = true;
        //if (DebugName == "StreetFader")
        //{
        //    Game.DisplaySubtitle($"IncomingText {incomingValue} - {CurrentValue}, IncomingDisplay {incomingDisplay} - {CurrentDisplay}");
        //}
    }
    private void OnDisplayChanged(string incomingDisplay)
    {
        TextToShow = incomingDisplay;
    }
    public void UpdateTimeChanged()
    {
        GameTimeDisplayChanged = Game.GameTime;// Environment.TickCount;// Game.GameTime;
    }
    private void OnFullyFadedIn()
    {
        FadeIsInverse = false;
        //EntryPoint.WriteToConsole($"Fader {DebugName} OnFullyFadedIn ", 2);
    }
    private int CalculateAlpha(uint GameTimeLastChanged, uint timeToShow, uint fadeTime)
    {
        uint TimeSinceChanged = Game.GameTime - GameTimeLastChanged;// Environment.TickCount - GameTimeLastChanged;
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
}





//using ExtensionsMethods;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;


//public class Fader
//{
//    private string LastText = "";
//    private string ExistingText = "";


//    private string ExistingDisplay = "";

//    private int GameTimeDisplayChanged;
//    private uint TimeToShow;
//    private uint TimeToFade;
//    private bool FadeIsInverse = false;
//    private string DebugName = "";





//    private string CurrentValue;
//    private string LastValue;
//    private string CurrentDisplay;
//    private string LastDisplay;



//    private string strippedIncomingText;
//    private string strippedExistingText;
//    private enum FadeState
//    {
//        FadingOut,
//        FadedOut,
//        FadingIn,
//        FadedIn,
//    }
//    public bool TextChangedLastUpdate { get; private set; } = false;
//    public int AlphaValue { get; private set; } = 255;
//    public string TextToShow { get; private set; } = "";
//    public Fader(uint timeToShow, uint timeToFade, string debugName)
//    {
//        TimeToShow = timeToShow;
//        TimeToFade = timeToFade;
//        DebugName = debugName;
//    }

//    public void Update(string incomingValue, string incomingDisplay)
//    {
//        // strippedIncomingText = Regex.Replace(IncomingText, @"(~.*?~)", "");//strip all color chracters from the text to compare when it actually changes
//        if (incomingValue != ExistingText)
//        {

//            EntryPoint.WriteToConsole($"Text Changed!       INCOMING: {incomingValue} - {incomingDisplay} EXISTING: {ExistingText} - {ExistingDisplay}");

//            if (incomingValue != incomingDisplay)
//            {
//                Game.DisplaySubtitle($"IncomingText {incomingValue} - {ExistingText}, IncomingDisplay {incomingDisplay} - {ExistingDisplay}");
//            }
//            OnTextChanged(incomingValue, incomingDisplay);
//        }
//        else
//        {
//            TextChangedLastUpdate = false;
//            if (FadeIsInverse && AlphaValue == 255)//I AM FULLY FADED IN
//            {
//                OnFullyFadedIn();
//            }
//        }
//        if (FadeIsInverse)
//        {
//            AlphaValue = 255 - CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade / 2);
//        }
//        else
//        {
//            if (TextToShow != "" && ExistingText == "")
//            {
//                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade);
//            }
//            else
//            {
//                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
//            }
//        }
//    }





//    //public void Update(string IncomingText, string IncomingDisplay)
//    //{
//    //   // strippedIncomingText = Regex.Replace(IncomingText, @"(~.*?~)", "");//strip all color chracters from the text to compare when it actually changes
//    //    if (IncomingText != ExistingText)
//    //    {

//    //        EntryPoint.WriteToConsole($"Text Changed!       INCOMING: {IncomingText} - {IncomingDisplay} EXISTING: {ExistingText} - {ExistingDisplay}");

//    //        if (IncomingText != IncomingDisplay)
//    //        {
//    //            Game.DisplaySubtitle($"IncomingText {IncomingText} - {ExistingText}, IncomingDisplay {IncomingDisplay} - {ExistingDisplay}");
//    //        }
//    //        OnTextChanged(IncomingText, IncomingDisplay);
//    //    }
//    //    else
//    //    {
//    //        TextChangedLastUpdate = false;
//    //        if (FadeIsInverse && AlphaValue == 255)//I AM FULLY FADED IN
//    //        {
//    //            OnFullyFadedIn();
//    //        }
//    //    }
//    //    if (FadeIsInverse)
//    //    {
//    //        AlphaValue = 255 - CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade / 2);         
//    //    }
//    //    else
//    //    {
//    //        if(TextToShow != "" && ExistingText == "")
//    //        {
//    //            AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade);
//    //        }
//    //        else
//    //        {
//    //            AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
//    //        }
//    //    }
//    //}
//    public void UpdateTimeChanged()
//    {
//        GameTimeDisplayChanged = Environment.TickCount;// Game.GameTime;
//    }
//    private void OnFullyFadedIn()
//    {
//        FadeIsInverse = false;
//        //EntryPoint.WriteToConsole($"Fader {DebugName} OnFullyFadedIn ", 2);
//    }
//    private void OnTextChanged(string incomingText, string incomingDisplay)
//    {
//        //EntryPoint.WriteToConsole($"Fader {DebugName} OnTextChanged IncomingText:{incomingText} ExistingText:{ExistingText} LastText: {LastText} ", 2);
//        if (incomingText != "" && ExistingText != "")
//        {
//            ExistingText = incomingText;
//            TextToShow = incomingDisplay;//incomingText;
//            FadeIsInverse = true;// false;//just changed LET FADE AFTER TIME
//        }
//        else if (incomingText != "")
//        {
//            ExistingText = incomingText;
//            TextToShow = incomingDisplay; //incomingText;
//            FadeIsInverse = true; //current is null, coming back on fading IN
//        }
//        else
//        {
//            if (ExistingText == "")
//            {
//                ExistingText = incomingText;
//                TextToShow = incomingDisplay; //incomingText;
//            }
//            else
//            {

//                TextToShow = ExistingDisplay;//ExistingText;
//            }

//            FadeIsInverse = false;//old is null, going off fading OUT
//        }
//        LastText = ExistingText;
//        ExistingDisplay = TextToShow;

//        GameTimeDisplayChanged = Environment.TickCount;
//        TextChangedLastUpdate = true;
//        EntryPoint.WriteToConsole($"Fader {DebugName} OnTextChanged TextToShow:{TextToShow} ", 2);
//    }
//    private int CalculateAlpha(int GameTimeLastChanged, uint timeToShow, uint fadeTime)
//    {
//        int TimeSinceChanged = Environment.TickCount - GameTimeLastChanged;
//        if (TimeSinceChanged < timeToShow)
//        {
//            return 255;
//        }
//        else if (TimeSinceChanged < timeToShow + fadeTime)
//        {
//            float percentVisible = 1f - (1f * (TimeSinceChanged - timeToShow)) / (1f * (fadeTime));
//            float alphafloat = percentVisible * 255f;
//            return ((int)Math.Floor(alphafloat)).Clamp(0, 255);
//        }
//        else
//        {
//            return 0;
//        }
//    }
//}

