// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Air.Tools.Watch
{
    class Program
    {
        private static Stopwatch _watch = new Stopwatch();
        private static string? _scriptPath;
        private static bool _processIsRunning;
        //comment
        static void Main(string[] args)
        {
            var printHelp = () =>
            {
                Console.WriteLine("Usage: air-watch <scriptPath>");
                Console.WriteLine("NOTE: Run with powershell scripts");
            };

            if(args.Length != 1)
            {
                printHelp();
                Environment.Exit(1);
            }

            var scriptPath = args[0];
            if (!File.Exists(scriptPath))
            {
                Console.WriteLine("Invalid script path provided. Path: " + scriptPath);
                printHelp();
                Environment.Exit(1);
            }

            _scriptPath = scriptPath;

            string path = Directory.GetCurrentDirectory();

            Console.WriteLine($"Monitoring directory: {path}");
            Console.WriteLine("Press 'q' to quit the application.");

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = path;
                watcher.Filter = "*.cs";
                watcher.NotifyFilter = NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Changed += OnChanged;
                watcher.Error += OnError;

                watcher.EnableRaisingEvents = true;

                while (Console.Read() != 'q') ;
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File {e.ChangeType}: {e.FullPath}");

            if (!EventBlocked())
            {
                RunPowershellScript();
            }
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"An error occurred: {e.GetException().Message}");
        }

        private static void RunPowershellScript()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{_scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine("Err: " + e.Data);
                    }
                };

                _processIsRunning = true;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                if(process.ExitCode != 0)
                {
                    Console.WriteLine($"Script FAILED: {_scriptPath} EXIT CODE: " + process.ExitCode);
                }

                _processIsRunning = false;
            }
        }

        //Some events are triggered multiple times, this method will block the event if it was triggered in the last 2 seconds or the process is still running
        private static bool EventBlocked()
        {
            if (!_processIsRunning && (!_watch.IsRunning || _watch.Elapsed > TimeSpan.FromSeconds(2)))
            {
                _watch.Restart();
                return true;
            }

            return false;
        }
    }
}
