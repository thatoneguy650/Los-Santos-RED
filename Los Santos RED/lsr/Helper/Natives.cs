using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Helper
{
    public static class Natives
    {
        public static uint CashHash(string PlayerName)
        {
            switch (PlayerName)
            {
                case "Michael":
                    return Game.GetHashKey("SP0_TOTAL_CASH");
                case "Franklin":
                    return Game.GetHashKey("SP1_TOTAL_CASH");
                case "Trevor":
                    return Game.GetHashKey("SP2_TOTAL_CASH");
                default:
                    return Game.GetHashKey("SP0_TOTAL_CASH");
            }
        }
        public static uint GetTargettingHandle()
        {
            uint TargetEntity;
            bool Found;
            unsafe
            {
                Found = NativeFunction.CallByName<bool>("GET_PLAYER_TARGET_ENTITY", Game.LocalPlayer, &TargetEntity);
            }
            if (!Found)
                return 0;

            uint Handle = TargetEntity;
            return Handle;
        }

    }
}
