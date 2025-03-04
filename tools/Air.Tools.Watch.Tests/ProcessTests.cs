// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Air.Tools.Watch;

public class PowerShellTests
{
    [Fact]
    public void Test_PowerShellScriptExecution()
    {
        string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "test.ps1");

        //static void logAction(string mess) => Console.WriteLine(mess);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            //process.OutputDataReceived += (sender, e) => { if (e.Data != null) logAction(e.Data); };
            //process.ErrorDataReceived += (sender, e) => { if (e.Data != null) logAction(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }
    }
}
