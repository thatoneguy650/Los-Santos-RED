using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DependencyChecker
{

    private Version AssemblyVersion;
    private Version MinVersion;
    private FileInfo AssemblyFile;
    private int ComparisonResults = -1;

    public DependencyChecker(string assemblyName, string requiredVersion)
    {
        AssemblyName = assemblyName;
        RequiredVersion = requiredVersion;
    }

    public string AssemblyName { get; set; }
    public string RequiredVersion { get; set; }
    public bool IsValid => ComparisonResults >= 0;
    public void Verify()
    {
        MinVersion = new Version(RequiredVersion);
        AssemblyFile = new FileInfo(AssemblyName);
        if (AssemblyFile != null)
        {
            AssemblyVersion = new Version(FileVersionInfo.GetVersionInfo(AssemblyFile.FullName)?.FileVersion);
            if(AssemblyVersion != null)
            {
                ComparisonResults = AssemblyVersion.CompareTo(MinVersion);
            }
        }
    }
    public string GameMessage
    {
        get
        {
            if (AssemblyFile == null)
            {
                return $"~y~{AssemblyName}~s~ ~r~Not Found~s~";
            }
            else if (ComparisonResults < 0)
            {
                return $"~y~{AssemblyName}~s~ ~r~Invalid Version~s~. Inst: ~y~{AssemblyVersion}~s~ Req: ~y~{RequiredVersion}~s~";
            }
            else if (IsValid)
            {
                return $"~y~{AssemblyName}~s~ ~g~Installed Successfully~s~. Inst: ~y~{AssemblyVersion}~s~ Req: ~y~{RequiredVersion}~s~";
            }
            return "";
        }
    }
    public string LogMessage
    {
        get
        {
            if (AssemblyFile == null)
            {
                return $"ERROR: {AssemblyName} Not Found";
            }
            else if (ComparisonResults < 0)
            {
                return $"ERROR: {AssemblyName} Invalid Version. Installed Version: {AssemblyVersion} Required Version: {RequiredVersion}";
            }
            else if (IsValid)
            {
                return $"{AssemblyName} Validated. Installed Version: {AssemblyVersion} Required Version: {RequiredVersion}";
            }
            return "";
        }
    }
}

