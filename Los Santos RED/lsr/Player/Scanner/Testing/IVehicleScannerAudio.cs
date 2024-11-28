using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IVehicleScannerAudio
{
    string ClassName(int classInt);
    string GetClassAudio(int classInt);
    string GetColorAudio(Color carColor);
    string GetColorAudioByID(int primaryColor);
    string GetMakeAudio(string makeName);
    string GetModelAudio(uint hash);
    List<string> GetPlateAudio(string licensePlateText);
}

