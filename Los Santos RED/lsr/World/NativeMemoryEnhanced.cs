using Rage;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using Microsoft.VisualBasic.Logging;
using System.Globalization;
using System.Text;
using System.Net;

namespace Mod
{
    //ENTIRE CLASS IS FROM SHVDN, ADAPTED TO RPH
    public unsafe class NativeMemoryEnhanced
    {
        public static void SetMPGlobals()
        {
            byte* address = FindPatternBmh("48 03 05 ? ? ? ? 4c 85 c0 0f 84 ? ? ? ? e9");
            if (address != null)
            {
                s_yscScriptTableAddr = *(int*)(address + 3) + address + 7;
            }

            IntPtr addressInScript = IntPtr.Zero;
            addressInScript = FindPatternInScript("2D ? ? ? ? 2C ? ? ? 56 ? ? 71 2E ? ? 62", 0x39DA738B);
            if (addressInScript != IntPtr.Zero)
            {
                int enableCarsGlobalOffset = 17; // Same for Legacy and Enhanced
                int globalIndex = GetScriptGlobalFromAddress(addressInScript, enableCarsGlobalOffset);

                var MyPtr = Game.GetScriptGlobalVariableAddress(globalIndex); //the script id for respawn_controller
                Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?
                EntryPoint.WriteToConsole($"globalIndex {globalIndex} MyPtr {MyPtr}");
            }
            else
            {
                EntryPoint.WriteToConsole($"SET MP GLOBALS NOT HOOKED");
            }
        }
        public static int GetScriptGlobalFromAddress(IntPtr address, int offset)
        {
            return *(int*)((byte*)address + offset) & 0xFFFFFF;
        }
        private static byte* s_yscScriptTableAddr;
        public static unsafe byte* FindPatternBmh(string pattern, bool doSuppressLog = false) => FindPatternBmh(pattern, IntPtr.Zero, 0, doSuppressLog);

        public static IntPtr FindPatternInScript(string pattern, int scriptHash)
        {
            if (s_yscScriptTableAddr == null)
            {
                return IntPtr.Zero;
            }
            var yscScriptTable = (YscScriptTable*)s_yscScriptTableAddr;


            YscScriptTableItem* shopControllerItem = yscScriptTable->FindScript(scriptHash);

            if (shopControllerItem == null || !shopControllerItem->IsLoaded())
            {
                return IntPtr.Zero;
            }

            YscScriptHeader* shopControllerHeader = shopControllerItem->header;

            int codepageCount = shopControllerHeader->CodePageCount();
            byte* address;
            for (int i = 0; i < codepageCount; i++)
            {
                int size = shopControllerHeader->GetCodePageSize(i);
                if (size <= 0)
                {
                    continue;
                }

                bool doSuppressLog = true;
                address = FindPatternBmh(pattern, shopControllerHeader->GetCodePageAddress(i), (ulong)size, doSuppressLog);
                if (address == null)
                {
                    continue;
                }

                return new IntPtr(address);
            }

            //Log.Message(Log.Level.Warning, $"NativeMemory.FindPatternInScript could not find pattern: {pattern}. Please inform SHVDNE maintainer.");

            return IntPtr.Zero;
        }





        public static unsafe byte* FindPatternBmh(string pattern, IntPtr startAddress, ulong size, bool doSuppressLog = false)
        {
            string[] array = pattern.Split(' ');
            StringBuilder stringBuilder = new StringBuilder(array.Length);
            StringBuilder stringBuilder2 = new StringBuilder(array.Length);
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    if (text == "??" || text == "?")
                    {
                        stringBuilder.Append("\0");
                        stringBuilder2.Append("?");
                    }
                    else
                    {
                        char value = (char)short.Parse(text, NumberStyles.AllowHexSpecifier);
                        stringBuilder.Append(value);
                        stringBuilder2.Append("x");
                    }
                }
            }

            pattern = stringBuilder.ToString();
            string mask = stringBuilder2.ToString();

            byte* address;

            if (startAddress != IntPtr.Zero)
            {
                if (size != 0)
                {
                    address = FindPatternBmh(pattern, mask, startAddress, size);
                }
                else
                {
                    address = FindPatternBmh(pattern, mask, startAddress);
                }
            }
            else
            {
                address = FindPatternBmh(pattern, mask);
            }

            // For script patterns, Logging is only done at the end of NativeMemory.FindPatternInScript,
            // otherwise a "pattern not found" warning would be logged for every script codePage.
            if (address == null && !doSuppressLog)
            {
                //LogMemPatternNotFound(pattern, mask, startAddress, size);
            }

            return address;
        }
        public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress)
        {
            ProcessModule module = Process.GetCurrentProcess().MainModule;

            if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
            {
                return null;
            }

            ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);
            return FindPatternBmh(pattern, mask, startAddress, size);
        }
        public static unsafe byte* FindPatternBmh(string pattern, string mask)
        {
            ProcessModule module = Process.GetCurrentProcess().MainModule;
            return FindPatternBmh(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
        }
        private static short[] CreateShiftTableForBmh(short[] pattern)
        {
            short[] skipTable = new short[256];
            int lastIndex = pattern.Length - 1;

            int diff = lastIndex - Math.Max(Array.LastIndexOf<short>(pattern, -1), 0);
            if (diff == 0)
            {
                diff = 1;
            }

            for (int i = 0; i < skipTable.Length; i++)
            {
                skipTable[i] = (short)diff;
            }

            for (int i = lastIndex - diff; i < lastIndex; i++)
            {
                short patternVal = pattern[i];
                if (patternVal >= 0)
                {
                    skipTable[patternVal] = (short)(lastIndex - i);
                }
            }

            return skipTable;
        }
        public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress, ulong size)
        {
            // Use short array intentionally to spare heap
            // Warning: throws an exception if length of pattern and mask strings does not match
            short[] patternArray = new short[pattern.Length];
            for (int i = 0; i < patternArray.Length; i++)
            {
                patternArray[i] = (mask[i] != '?') ? (short)pattern[i] : (short)-1;
            }

            int lastPatternIndex = patternArray.Length - 1;
            short[] skipTable = CreateShiftTableForBmh(patternArray);

            byte* endAddressToScan = (byte*)startAddress + size - patternArray.Length;

            // Pin arrays to avoid boundary check and search will be long enough to amortize the pin cost in time wise
            fixed (short* skipTablePtr = skipTable)
            fixed (short* patternArrayPtr = patternArray)
            {
                for (byte* curHeadAddress = (byte*)startAddress; curHeadAddress <= endAddressToScan; curHeadAddress += Math.Max((int)skipTablePtr[(curHeadAddress)[lastPatternIndex] & 0xFF], 1))
                {
                    for (int i = lastPatternIndex; patternArrayPtr[i] < 0 || ((byte*)curHeadAddress)[i] == patternArrayPtr[i]; --i)
                    {
                        if (i == 0)
                        {
                            // LogMemPatternNotFound(pattern, mask, startAddress, size); // Uncomment if the game crashes without "pattern not found" messages logged. This way, you know that operations happening after the last logged pattern are the problem.
                            return curHeadAddress;
                        }
                    }
                }
            }

            return null;
        }




        [StructLayout(LayoutKind.Explicit)]
        private struct YscScriptHeader
        {
            [FieldOffset(0x10)]
            internal byte** codeBlocksOffset;
            [FieldOffset(0x1C)]
            internal int codeLength;
            [FieldOffset(0x24)]
            internal int localCount;
            [FieldOffset(0x2C)]
            internal int nativeCount;
            [FieldOffset(0x30)]
            internal long* localOffset;
            [FieldOffset(0x40)]
            internal long* nativeOffset;
            [FieldOffset(0x58)]
            internal int nameHash;

            internal int CodePageCount()
            {
                return (codeLength + 0x3FFF) >> 14;
            }
            internal int GetCodePageSize(int page)
            {
                return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? codeLength & 0x3FFF : 0x4000);
            }
            internal IntPtr GetCodePageAddress(int page)
            {
                return new IntPtr(codeBlocksOffset[page]);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct YscScriptTableItem
        {
            [FieldOffset(0x0)]
            internal YscScriptHeader* header;
            [FieldOffset(0xC)]
            internal int hash;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal bool IsLoaded()
            {
                return header != null;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct YscScriptTable
        {
            [FieldOffset(0x0)]
            internal YscScriptTableItem* TablePtr;
            [FieldOffset(0x18)]
            internal uint count;

            internal YscScriptTableItem* FindScript(int hash)
            {
                if (TablePtr == null)
                {
                    return null; // table initialisation hasn't happened yet
                }
                for (int i = 0; i < count; i++)
                {
                    if (TablePtr[i].hash == hash)
                    {
                        return &TablePtr[i];
                    }
                }
                return null;
            }
        }
    }

}