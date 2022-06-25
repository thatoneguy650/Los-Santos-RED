﻿using System;
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

    public ActionWheelSettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        ItemWidth = 0.08f;
        ItemHeight = 0.08f;
        ItemSpacingX = 0.04f;
        ItemSpacingY = 0.04f;
        ItemScale = 1.2f;
        TextScale = 0.3f;
        TextFont = GTAFont.FontMonospace;
        TextColor = "White";
        ItemColor = "Black";
        SelectedItemColor = "Red";
        ShowCursor = false;
        ItemCenterX = 0.5f;
        ItemCenterY = 0.5f;
        ItemDistanceFromCenter = 0.3f;
        TransitionInEffect = "SwitchHUDIn";
        TransitionOutEffect = "SwitchHUDOut";
        MessageStartingPositionX = 0.05f;
        MessageStartingPositionY = 0.05f;
        MessageFont = GTAFont.FontChaletLondon;
        MessageTextColor = "White";
        MessageScale = 0.2f;
        MessageBodySpacingY = 0.125f;
        MessageHeaderSpacingY = 0.02f;
        MessagesToShow = 4;
    }
}