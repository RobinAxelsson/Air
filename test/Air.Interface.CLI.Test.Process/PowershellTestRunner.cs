// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Air.Domain;

namespace Air.Interface.CLI.ProcessTests;

public class PowerShellTests
{
    [Test]
    [Category("Build&Run")]
    public async Task Test_PowerShellScriptExecution()
    {
        var filePath = GetFullTestPath("test-cli.ps1");
        (bool error, string powershellOutput) = RunPowershellScriptTest(filePath);

        if (error)
        {
            Console.WriteLine(powershellOutput);
        }

        await Assert.That(error).IsFalse();

        var filteredOutput = FilterCliOutput(powershellOutput);
        Console.WriteLine(filteredOutput);

        var syncFlightFaresResult = JsonSerializer.Deserialize<SyncFlightFaresResult>(filteredOutput);

        await Assert.That(syncFlightFaresResult).IsNotNull();

        await Assert.That(syncFlightFaresResult!.FlightsCreated > 0 || syncFlightFaresResult.FlightsUpdated > 0).IsTrue();
    }

    private string FilterCliOutput(string powershellOutput)
    {
        Console.WriteLine("powershell output: " + powershellOutput);
        string start = "-----Start running CLI-----";
        string end   = "--------CLI exited---------";

        var startMissing = !powershellOutput.Contains(start, StringComparison.OrdinalIgnoreCase);
        var endMissing = !powershellOutput.Contains(end, StringComparison.OrdinalIgnoreCase);
        if (endMissing || startMissing)
        {
            throw new ArgumentException($"Failed trying to filter output from powershell script, script file must 'Write-Host {start}' and 'Write-Host {end}' start missing: {startMissing}, end missing: {endMissing}. The total out put was: {powershellOutput}");
        }

        var processOutPut = powershellOutput.Split(start)[^1].Split(end)[0];
        var json = processOutPut.Split("{")[^1].Split("}")[0];
        return '{' + json + '}';
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

        if (!File.Exists(scriptPath)) throw new FileNotFoundException("PowerShell script file not found", scriptPath);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "pwsh",
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
                    output.AppendLine(e.Data);
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

            Console.WriteLine("Starting process...");
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
