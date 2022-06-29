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
    private string LastText = "";
    private string ExistingText = "";
    private int GameTimeDisplayChanged;
    private uint TimeToShow;
    private uint TimeToFade;
    private bool FadeIsInverse = false;
    private string DebugName = "";
    private string strippedIncomingText;
    private string strippedExistingText;
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
    public void Update(string IncomingText)
    {
       // strippedIncomingText = Regex.Replace(IncomingText, @"(~.*?~)", "");//strip all color chracters from the text to compare when it actually changes
        if (IncomingText != ExistingText)
        {
            OnTextChanged(IncomingText);
        }
        else
        {
            TextChangedLastUpdate = false;
            if (FadeIsInverse && AlphaValue == 255)//I AM FULLY FADED IN
            {
                OnFullyFadedIn();
            }
        }
        if (FadeIsInverse)
        {
            AlphaValue = 255 - CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade / 2);         
        }
        else
        {
            if(TextToShow != "" && ExistingText == "")
            {
                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, 0, TimeToFade);
            }
            else
            {
                AlphaValue = CalculateAlpha(GameTimeDisplayChanged, TimeToShow, TimeToFade);
            }
        }
    }
    public void UpdateTimeChanged()
    {
        GameTimeDisplayChanged = Environment.TickCount;// Game.GameTime;
    }
    private void OnFullyFadedIn()
    {
        FadeIsInverse = false;
        //EntryPoint.WriteToConsole($"Fader {DebugName} OnFullyFadedIn ", 2);
    }
    private void OnTextChanged(string incomingText)
    {
        //EntryPoint.WriteToConsole($"Fader {DebugName} OnTextChanged IncomingText:{incomingText} ExistingText:{ExistingText} LastText: {LastText} ", 2);
        if (incomingText != "" && ExistingText != "")
        {
            TextToShow = incomingText;
            FadeIsInverse = true;// false;//just changed LET FADE AFTER TIME
        }
        else if (incomingText != "")
        {
            TextToShow = incomingText;
            FadeIsInverse = true; //current is null, coming back on fading IN
        }
        else
        {
            if(ExistingText == "")
            {
                TextToShow = incomingText;
            }
            else
            {
                TextToShow = ExistingText;
            }
            
            FadeIsInverse = false;//old is null, going off fading OUT
        }
        LastText = ExistingText;
        ExistingText = incomingText;
        GameTimeDisplayChanged = Environment.TickCount;
        TextChangedLastUpdate = true;
        //EntryPoint.WriteToConsole($"Fader {DebugName} OnTextChanged TextToShow:{TextToShow} ", 2);
    }
    private int CalculateAlpha(int GameTimeLastChanged, uint timeToShow, uint fadeTime)
    {
        int TimeSinceChanged = Environment.TickCount - GameTimeLastChanged;
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

