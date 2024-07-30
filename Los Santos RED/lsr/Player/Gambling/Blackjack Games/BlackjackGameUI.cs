using Blackjack;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BlackjackGameUI
{
    private ICasinoGamePlayable Player;
    private ISettingsProvideable Settings;
    private BlackJackGame BlackJackGame;
    private GamblingDen GameLocation;
    public BigMessageThread BigMessage { get; private set; }
    private bool IsUICreated;
    private bool isPaused;
    private uint lastGameTime;
    private uint notificationID;
    public int DelayTime { get; set; } = 2000;
    public BlackjackGameUI(ICasinoGamePlayable player, ISettingsProvideable settings, BlackJackGame blackJackGame, GamblingDen gameLocation)
    {
        Player = player;
        Settings = settings;
        BlackJackGame = blackJackGame;
        GameLocation = gameLocation;
    }
    public void CreateOrDisplayGameUI()
    {
        if (IsUICreated)
        {
            return;
        }
        GameFiber DoorWatcher = GameFiber.StartNew(delegate
        {
            IsUICreated = true;
            while (BlackJackGame.IsActive && !BlackJackGame.IsCancelled)
            {
                isPaused = lastGameTime == Game.GameTime;
                lastGameTime = Game.GameTime;
                DisplayGameStats();
                GameFiber.Yield();
            }
            IsUICreated = false;
        }, "DoorWatcher");
    }
    private void DisplayGameStats()
    {
        if (BlackJackGame.ShowNoGameStats)
        {
            return;
        }

        float StartingPosition = Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionX + Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionXMediumOffset;// 0.2f;
        NativeHelper.DisplayTextOnScreen($"{BlackJackGame.CasinoPlayer.GetGameStatus()}", StartingPosition, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        NativeHelper.DisplayTextOnScreen($"{Player.GamblingManager.GetStats(GameLocation)}", StartingPosition + (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing / 2), Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        if (!BlackJackGame.ShowHandUI)
        {
            return;
        }
        NativeHelper.DisplayTextOnScreen(BlackJackGame.Dealer.WriteHeader(), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        NativeHelper.DisplayTextOnScreen(BlackJackGame.CasinoPlayer.PrimaryHand.WriteHeader(BlackJackGame.CasinoPlayer.Name), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        if (BlackJackGame.CasinoPlayer.SplitHands == null)
        {
            return;
        }
        foreach (Hand splitHands in BlackJackGame.CasinoPlayer.SplitHands)
        {
            NativeHelper.DisplayTextOnScreen(splitHands.WriteHeader(BlackJackGame.CasinoPlayer.Name), StartingPosition += Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplaySpacing, Settings.SettingsManager.LSRHUDSettings.TopDisplayPositionY, Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayScale, Color.White, (GTAFont)Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayFont, (GTATextJustification)2, true);
        }
    }
    private void DrawSprites(object sender, GraphicsEventArgs args)
    {
        try
        {
            if (isPaused)
            {
                return;
            }
            GetStuff(args);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"UI: Draw ERROR {ex.Message} {ex.StackTrace} ", 0);
        }
    }
    private void GetStuff(GraphicsEventArgs args)
    {
        DisplayCardIcons(args);//, InitialPosX, InitialPosY, SecondPosY, );
    }
    private void DisplayCardIcons(GraphicsEventArgs args)//, float InitialPosX, float InitialPosY, float secondPosY, float Scale)
    {
        if (isPaused || !EntryPoint.ModController.IsRunning)
        {
            return;
        }
        if (!BlackJackGame.ShowHandUI)
        {
            return;
        }
        float ConsistencyScale = (float)Game.Resolution.Width / 2160f;
        float InitialPosX = Game.Resolution.Width * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionX);//Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayPositionY;
        float InitialPosY = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY);
        float SecondPosY = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY + Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacing);
        float Scale = ConsistencyScale * Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconScale;

        int DisplayedCards = 1;
        foreach (Card card in BlackJackGame.Dealer.HiddenCards)
        {
            DisplayIconSmall(args, Player.GamblingManager.UnknownCardTexture, DisplayedCards, InitialPosX, InitialPosY, Scale);
            DisplayedCards++;
        }
        foreach (Card card in BlackJackGame.Dealer.RevealedCards)
        {
            DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, InitialPosY, Scale);
            DisplayedCards++;
        }
        DisplayedCards = 1;
        foreach (Card card in BlackJackGame.CasinoPlayer.PrimaryHand.Cards)
        {
            DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, SecondPosY, Scale);
            DisplayedCards++;
        }
        if (BlackJackGame.CasinoPlayer.SplitHands == null)
        {
            return;
        }
        int DisplayedSplits = 2;
        foreach (Hand splitHands in BlackJackGame.CasinoPlayer.SplitHands)
        {
            float SplitHandPos = Game.Resolution.Height * (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconPositionY + (Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacing * DisplayedSplits));
            DisplayedCards = 1;
            foreach (Card card in splitHands.Cards)
            {
                DisplayIconSmall(args, GetTextureFromCard(card), DisplayedCards, InitialPosX, SplitHandPos, Scale);
                DisplayedCards++;
            }
            DisplayedSplits++;
        }
    }
    private void DisplayIconSmall(GraphicsEventArgs args, Texture toShow, int Index, float XPosition, float YPosition, float Scale)
    {
        if (toShow == null || toShow.Size == null)
        {
            return;
        }
        float FinalPosX = XPosition - (Index * ((toShow.Size.Width - Settings.SettingsManager.LSRHUDSettings.ExtraTopDisplayIconSpacingPixelReduction) * Scale));//InitialPosX - (i * (toShow.Size.Width * Scale));
        float FinalPosY = YPosition;
        if (toShow == null || toShow.Size == null)
        {
            return;
        }
        RectangleF rectangleF = new RectangleF(FinalPosX, FinalPosY, toShow.Size.Width * Scale, toShow.Size.Height * Scale);
        args.Graphics.DrawTexture(toShow, rectangleF);
    }
    private Texture GetTextureFromCard(Card card)
    {
        Tuple<Card, Texture> returned = Player.GamblingManager.CardIconList.Where(x => x.Item1.Face == card.Face && x.Item1.Suit == card.Suit).FirstOrDefault();
        if (returned == null)
        {
            EntryPoint.WriteToConsole($"{card.Face} {card.Suit} NOT FOUND");
            return null;
        }
        return returned.Item2;
    }
    public void SetupTextures()
    {
        Player.GamblingManager.SetupSharedTextures();
        Game.RawFrameRender += DrawSprites;
    }
    public void DisableDraw()
    {
        Game.RawFrameRender -= DrawSprites;
    }
    public void Disable()
    {
        RemoveBigMessages();
        DisableDraw();
        RemoveNotifications();
    }
    private void RemoveBigMessages()
    {
        BigMessage.Fiber?.Abort();
        if (BigMessage != null && BigMessage.MessageInstance != null)
        {
            BigMessage.MessageInstance.Dispose();
        }
    }
    public void SetupBigMessages()
    {
        BigMessage = new BigMessageThread(true);
    }
    public void ChooseSuccessSound()
    {
        if (GameLocation != null)
        {
            GameLocation.PlaySuccessSound();
        }
        else
        {
            NativeHelper.PlaySuccessSound();
        }
    }
    public void ChooseErrorSound()
    {
        if (GameLocation != null)
        {
            GameLocation.PlayErrorSound();
        }
        else
        {
            NativeHelper.PlayErrorSound();
        }
    }
    private void DisplayCustomMessage(string header, string message)
    {
        if (GameLocation != null)
        {
            if (string.IsNullOrEmpty(header))
            {
                header = "Message";
            }
            GameLocation.DisplayMessage(header, message);
        }
        else
        {
            Game.RemoveNotification(notificationID);
            if (string.IsNullOrEmpty(header))
            {
                notificationID = Game.DisplayNotification($"{message}");
            }
            else
            {
                notificationID = Game.DisplayNotification($"{header} {message}");
            }
        }
    }
    public void OnCancelled()
    {
        DisplayCustomMessage("Result", "Cancelled.");
        ChooseErrorSound();
    }
    public void OnInvalidBet()
    {
        DisplayCustomMessage("Alert", "~y~Invalid Bet.~s~");
        ChooseErrorSound();
    }
    public void RemoveNotifications()
    {
        //BigMessage.MessageInstance?.Dispose();

        if (GameLocation == null)
        {
            Game.RemoveNotification(notificationID);
        }
        else
        {
            GameLocation.RemoveMessage();
        }
    }
    public void DisplayGameEvent(string header, string message, bool delay, bool positiveSound, bool negativeSound, bool regularSound)
    {
        if(positiveSound)
        {
            ChooseSuccessSound();
        }
        else if (negativeSound)
        {
            ChooseErrorSound();
        }
        else if (regularSound)
        {
            NativeHelper.PlayAcceptSound();
        }
        DisplayGameEvent(header, message, delay);
    }
    public void DisplayGameEvent(string header, string message, bool delay)
    {
        DisplayCustomMessage(header, message);
        if(delay)
        {
            StartDelay();
        }
    }
    public void StartDelay()
    {
        Player.ButtonPrompts.AttemptAddPrompt("casinoGame", "Skip", "casinoGameSkip", GameControl.Attack, 9999);
        uint GameTimeStarted = Game.GameTime;
        while (Game.GameTime - GameTimeStarted <= DelayTime)
        {
            GameFiber.Yield();
            if (Player.ButtonPrompts.IsPressed("casinoGameSkip"))
            {
                Player.ButtonPrompts.RemovePrompts("casinoGame");
                return;
            }
        }
        Player.ButtonPrompts.RemovePrompts("casinoGame");
    }
    public void OnCannotMeetMinimumBet()
    {
        DisplayCustomMessage("Minimum Bet ", "You do not have the minimum required bet amount.");
        ChooseErrorSound();
        StartDelay();
    }
    public void OnHandBusted(string v)
    {
        ChooseErrorSound();
        BigMessage.MessageInstance.ShowColoredShard("~r~Player Busts~s~", v, HudColor.Black, HudColor.RedDark, DelayTime);
        StartDelay();
    }
    public void OnHandSurrendered(string v1, string v2)
    {
        BigMessage.MessageInstance.ShowColoredShard(v1, v2, HudColor.Black, HudColor.Orange, DelayTime);
        ChooseErrorSound();
        StartDelay();
    }
    public void StartRound()
    {
        SetupTextures();
        SetupBigMessages();
    }
    public void OnPlayerWon(int winAmount, string handName, bool isBlackJack)
    {
        if (isBlackJack)
        {
            BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + winAmount + "~s~ with ~o~Blackjack.~s~", handName, HudColor.Black, HudColor.GreenDark, DelayTime);
        }
        else
        {
            BigMessage.MessageInstance.ShowColoredShard("~g~Player Wins $" + winAmount + "~s~", handName, HudColor.Black, HudColor.GreenDark, DelayTime);
        }
        ChooseSuccessSound();
        StartDelay();
    }
    public void OnDealerWins(string v)
    {
        BigMessage.MessageInstance.ShowColoredShard("~r~Dealer Wins.~s~", v, HudColor.Black, HudColor.RedDark, DelayTime);
        ChooseErrorSound();
        StartDelay();
    }
    public void OnPlayerDealerPush(string v)
    {
        BigMessage.MessageInstance.ShowColoredShard("Player and Dealer Push.", v, HudColor.Black, HudColor.Blue, DelayTime);
        ChooseErrorSound();
        StartDelay();
    }
}

