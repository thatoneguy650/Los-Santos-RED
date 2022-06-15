using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BarDisplaySettings : ISettingsDefaultable
{
    public bool IsEnabled { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
 
    public BarDisplaySettings()
    {
        SetDefault();
    }
    public void SetDefault()
    {    
        IsEnabled = true;
        PositionX = 0.05f;
        PositionY = 0.9925f;
        Width = 0.07f;
        Height = 0.0075f;
    }
}