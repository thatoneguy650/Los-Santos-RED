using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ActionWheelSettings : ISettingsDefaultable
{
    public float ItemWidth { get; set; }
    public float ItemHeight { get; set; }
    public float ItemSpacingX { get; set; }
    public float ItemSpacingY { get; set; }
    public float ItemScale { get; set; }
    public string ItemColor { get; set; }
    public string SelectedItemColor { get; set; }
    public float ItemCenterX { get; set; }
    public float ItemCenterY { get; set; }
    public float ItemDistanceFromCenter { get; set; }
    public float TextScale { get; set; }
    public GTAFont TextFont { get; set; }
    public bool ShowCursor { get; set; }
    public string TextColor { get; set; }
    public string TransitionInEffect { get; set; }
    public string TransitionOutEffect { get; set; }
    public float MessageStartingPositionX { get; set; }
    public float MessageStartingPositionY { get; set; }
    public GTAFont MessageFont { get; set; }
    public string MessageTextColor { get; set; }
    public float MessageScale { get; set; }
    public float MessageBodySpacingY { get; set; }
    public float MessageHeaderSpacingY { get; set; }
    public int MessagesToShow { get; set; }
    public float ItemDistanceFromCenterExtraItemScalar { get; set; }
    public float ItemScaleExtraItemScalar { get; set; }
    public float TextBoxScale { get; set; }
    public float PrevPageCenterX { get; set; }
    public float PrevPageCenterY { get; set; }

    public float NextPageCenterX { get; set; }
    public float NextPageCenterY { get; set; }
    public int ItemsPerPage { get; set; }

    public ActionWheelSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ItemWidth = 0.06f;
        ItemHeight = 0.06f;
        ItemSpacingX = 0.04f;
        ItemSpacingY = 0.04f;
        ItemScale = 1.2f;
        TextScale = 0.35f;
        TextFont = GTAFont.FontChaletLondon;// GTAFont.FontMonospace;
        TextColor = "White";
        ItemColor = "Black";
        SelectedItemColor = "Red";
        ShowCursor = true;
        ItemCenterX = 0.5f;
        ItemCenterY = 0.5f;
        ItemDistanceFromCenter = 0.3f;
        TransitionInEffect = "SwitchHUDIn";
        TransitionOutEffect = "SwitchHUDOut";
        MessageStartingPositionX = 0.1f;// 0.05f;
        MessageStartingPositionY = 0.1f;// 0.05f;
        MessageFont = GTAFont.FontChaletLondon;//GTAFont.FontChaletComprimeCologne;
        MessageTextColor = "White";
        MessageScale = 0.25f;
        MessageBodySpacingY = 0.15f;//0.125f;
        MessageHeaderSpacingY = 0.055f;//0.02f;
        MessagesToShow = 4;
        ItemDistanceFromCenterExtraItemScalar = -0.005f;//-0.01f;
        ItemScaleExtraItemScalar = 0.005f;// 0.01f;

        TextBoxScale = 0.084f;//0.025f;

        NextPageCenterX = 0.875f;
        NextPageCenterY = 0.8f;


        PrevPageCenterX = 0.8f;
        PrevPageCenterY = 0.8f;


        ItemsPerPage = 10;
    }
}