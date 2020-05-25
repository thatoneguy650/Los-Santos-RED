using System;
using System.Runtime.InteropServices;
using Pointer = System.UInt64;

class Memory
{
    //some garbage memory class taken from god knows where
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
    [DllImport("kernel32.dll")]
    private static extern Int32 CloseHandle(IntPtr hObject);
    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);
    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesWritten);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, IntPtr flNewProtect, out IntPtr lpflOldProtect);

    private int pid;
    private IntPtr pHandle;
    private Pointer BaseAddress;
    private int ImageSize;
    private String FileName;
    private const int PROCESS_ALL_ACCESS = 2035711;

    private void initAddresses(System.Diagnostics.Process process, String moduleName)
    {
        if (moduleName == null)
        {
            BaseAddress = (Pointer)process.MainModule.BaseAddress;
            ImageSize = process.MainModule.ModuleMemorySize;
            return;
        }
        System.Diagnostics.ProcessModuleCollection Modules = process.Modules;
        int moduleCount = Modules.Count;
        if (moduleCount == 0)
        {
            BaseAddress = 0;
            ImageSize = 0;
            return;
        }
        for (int i = 0; i < moduleCount; i++)
        {
            if (Modules[i].ModuleName == moduleName)
            {
                BaseAddress = (Pointer)Modules[i].BaseAddress;
                ImageSize = Modules[i].ModuleMemorySize;
                break;
            }
        }
    }

    private void Init(System.Diagnostics.Process process, String moduleName)
    {
        initAddresses(process, moduleName);
        pid = process.Id;
        FileName = process.MainModule.FileName;
        pHandle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
    }

    public Memory(String processName, String moduleName = null)
    {
        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(processName);
        if (processes.Length > 0)
            Init(processes[0], moduleName);
    }

    public Memory(int processId, String moduleName = null)
    {
        System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(processId);
        Init(process, moduleName);
    }

    ~Memory()
    {
        if (pHandle.ToInt64() != 0)
            CloseHandle(pHandle);
    }

    public Pointer getBaseAddress()
    {
        return BaseAddress;
    }

    public int GetProcessID()
    {
        return pid;
    }

    public IntPtr GetModuleHandle()
    {
        return pHandle;
    }

    public String GetFileName()
    {
        return FileName;
    }

    public byte[] ReadMemory(Pointer address, int size)
    {
        byte[] buffer = new byte[size];
        IntPtr oldProtection;
        VirtualProtectEx(pHandle, (IntPtr)address, size, (IntPtr)0x40, out oldProtection);
        ReadProcessMemory(pHandle, (IntPtr)address, buffer, buffer.Length, IntPtr.Zero);
        VirtualProtectEx(pHandle, (IntPtr)address, size, oldProtection, out oldProtection);
        return buffer;
    }

    public Pointer GetAddress(Pointer address, int[] offsets = null)
    {
        if (offsets == null || offsets.Length == 0)
            return address;
        Pointer nAddress = Read<Pointer>(address);
        int size = offsets.Length - 1;
        for (int i = 0; i != size; i++)
            nAddress = Read<Pointer>(nAddress + (Pointer)offsets[i]);
        nAddress = nAddress + (Pointer)offsets[size];
        return nAddress;
    }

    public T Read<T>(Pointer address, int[] offsets = null) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        Pointer newaddr = GetAddress(address, offsets);
        byte[] buffer = ReadMemory(newaddr, size);
        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        T VMtype = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        handle.Free();
        return VMtype;
    }

    public string ReadString(Pointer address, int[] offsets = null, int size = 255, bool unicode = true)
    {
        System.Text.Encoding encoding = unicode ? System.Text.Encoding.UTF8 : System.Text.Encoding.Default;
        Pointer newaddr = GetAddress(address, offsets);
        byte[] buffer = ReadMemory(newaddr, size);
        String str = encoding.GetString(buffer);
        int pos = str.IndexOf('\0');
        if (pos > -1)
            str = str.Substring(0, pos);
        return str;
    }

    public void WriteMemory(Pointer address, byte[] buffer = null)
    {
        IntPtr oldProtection;
        VirtualProtectEx(pHandle, (IntPtr)address, buffer.Length, (IntPtr)0x40, out oldProtection);
        WriteProcessMemory(pHandle, (IntPtr)address, buffer, buffer.Length, IntPtr.Zero);
        VirtualProtectEx(pHandle, (IntPtr)address, buffer.Length, oldProtection, out oldProtection);
    }

    public void Write<T>(Pointer address, T content, int[] offsets = null) where T : struct
    {
        int size = Marshal.SizeOf(content);
        byte[] buffer = new byte[size];
        IntPtr pointer = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(content, pointer, true);
        Marshal.Copy(pointer, buffer, 0, size);
        Marshal.FreeHGlobal(pointer);
        Pointer newaddr = GetAddress(address, offsets);
        WriteMemory(newaddr, buffer);
    }

    public void WriteString(Pointer address, string str, int[] offsets = null)
    {
        Pointer newaddr = GetAddress(address, offsets);
        WriteMemory(newaddr, System.Text.Encoding.Default.GetBytes(str));
    }

    public Pointer PointerScan(String pattern, String mask, int find = 0, Pointer beginAddress = 0)
    {
        if (beginAddress < BaseAddress || beginAddress > (BaseAddress + (Pointer)ImageSize))
            beginAddress = BaseAddress;
        byte[] source = ReadMemory(BaseAddress, ImageSize);
        int patternLen = pattern.Length;
        int offset = (int)(BaseAddress - beginAddress);
        int matches = 0;
        int found = 0;
        while (offset < ImageSize)
        {
            if (source[offset] == pattern[matches] || mask[matches] == '?')
            {
                matches++;
                if (matches == patternLen)
                {
                    if (found == find)
                        return BaseAddress + (Pointer)(offset - matches) + 1;
                    found++;
                }
            }
            else if (matches > 0)
            {
                matches = source[offset] == pattern[0] || mask[0] == '?' ? 1 : 0;
            }
            offset++;
        }
        return 0;
    }

    public Pointer PointerScan(String pattern, int find = 0, Pointer beginAddress = 0)
    {
        String[] nibble = pattern.Split(' ');
        String patternStr = "";
        String maskStr = "";
        for (int i = 0; i < nibble.Length; i++)
        {
            if (nibble[i] == "")
                continue;
            if (nibble[i] == "?" || nibble[i] == "??" || nibble[i] == "xx")
            {
                patternStr = patternStr + '\x90';
                maskStr = maskStr + '?';
            }
            else
            {
                int j = int.Parse(nibble[i], System.Globalization.NumberStyles.HexNumber);
                patternStr = patternStr + (char)j;
                maskStr = maskStr + 'x';
            }
        }
        return PointerScan(patternStr, maskStr, find, beginAddress);
    }

    public Pointer ReadRelativeAddress(Pointer address, int offset = 3)
    {
        const int JUMP_SIZE = 4;
        return address + (Pointer)Read<int>(address + (Pointer)offset) + (Pointer)(JUMP_SIZE + offset);
    }
}