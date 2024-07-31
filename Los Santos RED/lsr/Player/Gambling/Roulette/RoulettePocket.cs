using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette
{
    public class RoulettePocket
    {
        public string PocketDisplay => PocketID == -2 ? "000" : PocketID == -1 ? "00" : PocketID.ToString();
        public int PocketID { get; private set; }
        public bool IsOdd {get; private set; }
        public string Color { get; private set; }
        public string FullDisplay => GameColor + PocketDisplay + "~s~";
        public string GameColor => Color == "Red" ? "~r~" : Color == "Black" ? "~u~" : Color == "Green" ? "~g~" : "~s~";
        public RoulettePocket(int pocketID)
        {
            PocketID = pocketID;
        }
        public void Setup()
        {
            IsOdd = PocketID % 2 != 0;
            if (PocketID <= 0)
            {
                Color = "Green";
            }
            if ((PocketID >= 1 && PocketID <= 10) || (PocketID >= 19 && PocketID <= 28))
            {
                if (IsOdd)
                {
                    Color = "Red";
                }
                else
                {
                    Color = "Black";
                }
            }
            else
            {
                if (IsOdd)
                {
                    Color = "Black";
                }
                else
                {
                    Color = "Red";
                }
            }
        }
    }
}
