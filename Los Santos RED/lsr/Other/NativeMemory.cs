using Rage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



//    public unsafe class NativeMemory
//    {

//        public static void SetMPGlobals()
//        {

//            int gameVersion = 200;
//            byte* address = FindPatternBmh("\x48\x03\x15\x00\x00\x00\x00\x4C\x23\xC2\x49\x8B\x08", "xxx????xxxxxx");
//            var yscScriptTable = (YscScriptTable*)(address + *(int*)(address + 3) + 7);

//            // find the shop_controller script
//            YscScriptTableItem* shopControllerItem = yscScriptTable->FindScript(0x39DA738B);

//            if (shopControllerItem == null || !shopControllerItem->IsLoaded())
//            {
//                return;
//            }

//            YscScriptHeader* shopControllerHeader = shopControllerItem->header;

//            string enableCarsGlobalPattern;
//            if (gameVersion >= 80)
//            {
//                // b2802 has 3 additional opcodes between CALL opcode (0x5D) and GLOBAL_U24 opcode (0x61 in b2802)
//                enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x71\x2E\x00\x01\x62\x00\x00\x00\x00\x04\x00\x71\x2E\x00\x01";
//            }
//            else if (gameVersion >= 46)
//            {
//                enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01";
//            }
//            else
//            {
//                enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01";
//            }
//            string enableCarsGlobalMask = gameVersion >= 46 ? "x??xxxx??xxxxx?xx????xxxx?x" : "xx??xxxxxx?xx????xxxx?x";
//            int enableCarsGlobalOffset = gameVersion >= 46 ? 17 : 13;

//            int codepageCount = shopControllerHeader->CodePageCount();
//            for (int i = 0; i < codepageCount; i++)
//            {
//                int size = shopControllerHeader->GetCodePageSize(i);
//                if (size <= 0)
//                {
//                    continue;
//                }

//                address = FindPatternNaive(enableCarsGlobalPattern, enableCarsGlobalMask, shopControllerHeader->GetCodePageAddress(i), (ulong)size);
//                if (address == null)
//                {
//                    continue;
//                }

//                int globalIndex = *(int*)(address + enableCarsGlobalOffset) & 0xFFFFFF;


//                var MyPtr = Game.GetScriptGlobalVariableAddress(globalIndex); //the script id for respawn_controller
//                Marshal.WriteInt32(MyPtr, 1); //setting it to 1 turns it off somehow?


//                //*(int*)GetGlobalPtr(globalIndex).ToPointer() = 1;//rage.game.getglobalwhatever
//                break;
//            }
//        }

//        public static unsafe byte* FindPatternBmh(string pattern, string mask)
//        {
//            ProcessModule module = Process.GetCurrentProcess().MainModule;
//            return FindPatternBmh(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
//        }

//        /// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
//        public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress)
//        {
//            ProcessModule module = Process.GetCurrentProcess().MainModule;

//            if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
//            {
//                return null;
//            }

//            ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

//            return FindPatternBmh(pattern, mask, startAddress, size);
//        }



//        public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress, ulong size)
//        {
//            ulong address = (ulong)startAddress.ToInt64();
//            ulong endAddress = address + size;

//            for (; address < endAddress; address++)
//            {
//                for (int i = 0; i < pattern.Length; i++)
//                {
//                    if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
//                    {
//                        break;
//                    }

//                    if (i + 1 == pattern.Length)
//                    {
//                        return (byte*)address;
//                    }
//                }
//            }

//            return null;
//        }


//        /// <summary>
//        /// Searches the address space of the current process for a memory pattern using the Boyer–Moore–Horspool algorithm.
//        /// Will perform faster than the naive algorithm when the pattern is long enough to expect the bad character skip is consistently high.
//        /// </summary>
//        /// <param name="pattern">The pattern.</param>
//        /// <param name="mask">The pattern mask.</param>
//        /// <param name="startAddress">The address to start searching at.</param>
//        /// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
//        /// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
//        public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress, ulong size)
//        {
//            // Use short array intentionally to spare heap
//            // Warning: throws an exception if length of pattern and mask strings does not match
//            short[] patternArray = new short[pattern.Length];
//            for (int i = 0; i < patternArray.Length; i++)
//            {
//                patternArray[i] = (mask[i] != '?') ? (short)pattern[i] : (short)-1;
//            }

//            int lastPatternIndex = patternArray.Length - 1;
//            short[] skipTable = CreateShiftTableForBmh(patternArray);

//            byte* endAddressToScan = (byte*)startAddress + size - patternArray.Length;

//            // Pin arrays to avoid boundary check and search will be long enough to amortize the pin cost in time wise
//            fixed (short* skipTablePtr = skipTable)
//            fixed (short* patternArrayPtr = patternArray)
//            {
//                for (byte* curHeadAddress = (byte*)startAddress; curHeadAddress <= endAddressToScan; curHeadAddress += Math.Max((int)skipTablePtr[(curHeadAddress)[lastPatternIndex] & 0xFF], 1))
//                {
//                    for (int i = lastPatternIndex; patternArrayPtr[i] < 0 || ((byte*)curHeadAddress)[i] == patternArrayPtr[i]; --i)
//                    {
//                        if (i == 0)
//                        {
//                            return curHeadAddress;
//                        }
//                    }
//                }
//            }

//            return null;
//        }

//        private static short[] CreateShiftTableForBmh(short[] pattern)
//        {
//            short[] skipTable = new short[256];
//            int lastIndex = pattern.Length - 1;

//            int diff = lastIndex - Math.Max(Array.LastIndexOf<short>(pattern, -1), 0);
//            if (diff == 0)
//            {
//                diff = 1;
//            }

//            for (int i = 0; i < skipTable.Length; i++)
//            {
//                skipTable[i] = (short)diff;
//            }

//            for (int i = lastIndex - diff; i < lastIndex; i++)
//            {
//                short patternVal = pattern[i];
//                if (patternVal >= 0)
//                {
//                    skipTable[patternVal] = (short)(lastIndex - i);
//                }
//            }

//            return skipTable;
//        }


//        [StructLayout(LayoutKind.Explicit)]
//        private struct YscScriptHeader
//        {
//            [FieldOffset(0x10)]
//            internal byte** codeBlocksOffset;
//            [FieldOffset(0x1C)]
//            internal int codeLength;
//            [FieldOffset(0x24)]
//            internal int localCount;
//            [FieldOffset(0x2C)]
//            internal int nativeCount;
//            [FieldOffset(0x30)]
//            internal long* localOffset;
//            [FieldOffset(0x40)]
//            internal long* nativeOffset;
//            [FieldOffset(0x58)]
//            internal int nameHash;

//            internal int CodePageCount()
//            {
//                return (codeLength + 0x3FFF) >> 14;
//            }
//            internal int GetCodePageSize(int page)
//            {
//                return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? codeLength & 0x3FFF : 0x4000);
//            }
//            internal IntPtr GetCodePageAddress(int page)
//            {
//                return new IntPtr(codeBlocksOffset[page]);
//            }
//        }


//        [StructLayout(LayoutKind.Explicit)]
//        private struct YscScriptTableItem
//        {
//            [FieldOffset(0x0)]
//            internal YscScriptHeader* header;
//            [FieldOffset(0xC)]
//            internal int hash;

//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            internal bool IsLoaded()
//            {
//                return header != null;
//            }
//        }
//        [StructLayout(LayoutKind.Explicit)]
//        private struct YscScriptTable
//        {
//            [FieldOffset(0x0)]
//            internal YscScriptTableItem* TablePtr;
//            [FieldOffset(0x18)]
//            internal uint count;

//            internal YscScriptTableItem* FindScript(int hash)
//            {
//                if (TablePtr == null)
//                {
//                    return null; //table initialisation hasn't happened yet
//                }
//                for (int i = 0; i < count; i++)
//                {
//                    if (TablePtr[i].hash == hash)
//                    {
//                        return &TablePtr[i];
//                    }
//                }
//                return null;
//            }
//        }
    
    
//}
