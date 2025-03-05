// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace Air.Tools.Watch;

public class PowerShellTests
{
    private readonly ITestOutputHelper _testOutput;

    public PowerShellTests(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
    }
    [Fact]
    public void Test_PowerShellScriptExecution()
    {
        var filePath = GetFullTestPath("test-cli.ps1");
        (bool error, string processOutput) = RunPowershellScriptTest(filePath);
        // _testOutput.WriteLine(processOutput);
        Assert.True(!error, processOutput);
    }

    private string GetFullTestPath(string filePath)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

        for (int i = 0; i < 10; i++)
        {
            var files = dir.GetFiles(filePath);
            var all = dir.GetFiles().Select(x => x.Name).ToList();
            if (files.Length == 1)
            {
                return files[0].FullName;
            }

            dir = dir.Parent ?? throw new FileNotFoundException("Looking for script path but current folder has no parent: " + dir.FullName);
        }

        throw new FileNotFoundException("PowerShell script file not found", filePath);
    }

    private static (bool error, string processOutput) RunPowershellScriptTest(string filePath)
    {
        string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        if(!File.Exists(scriptPath)) throw new FileNotFoundException("PowerShell script file not found", scriptPath);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var output = new StringBuilder();
        var error = false;
        using (Process process = new Process { StartInfo = psi })
        {
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    output.AppendLine("Out: " + e.Data);
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null)
                {
                    output.AppendLine("Err: " + e.Data);
                    error = true;
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            var errorCode = process.ExitCode;
            output.AppendLine($"Process exited with code {errorCode}");

            if (errorCode != 0)
            {
                error = true;
            }
        }

        return (error, output.ToString());
    }
}
