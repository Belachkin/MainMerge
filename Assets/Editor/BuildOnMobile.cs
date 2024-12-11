using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DorferGames.Editor
{
    public static class BuildOnMobile
    {
        static readonly string Eol = Environment.NewLine;

        static readonly string keystoreName = "dorfer.keystore";
        static readonly string keystorePass = "4afW7SwZjsN&kKtXe7KwS%Ye";
        static readonly string keyaliasName = "release";
        static readonly string keyaliasPass = "4afW7SwZjsN&kKtXe7KwS%Ye";

        static readonly string pinKey = "PIN";

        [UsedImplicitly]
        public static void BuildOptions()
        {
            Dictionary<string, string> options = GetValidatedOptions();

            var architectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            var buildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), options["buildTarget"]);

            PlayerSettings.Android.bundleVersionCode = int.Parse(options["androidVersionCode"]);
            EditorUserBuildSettings.buildAppBundle = options["customBuildPath"].EndsWith(".aab");

            //PlayerSettings.Android.targetArchitectures = architectures;
            //PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
            //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
            //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = keystoreName;
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyaliasName;
            PlayerSettings.Android.keyaliasPass = keyaliasPass;

            PlayerSettings.SplashScreen.showUnityLogo = false;

            Build(buildTarget, options["customBuildPath"]);
        }

        private static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            if (!validatedOptions.TryGetValue("projectPath", out string _))
            {
                Console.WriteLine("Missing argument -projectPath");
                EditorApplication.Exit(110);
            }

            if (!validatedOptions.TryGetValue("buildTarget", out string buildTarget))
            {
                Console.WriteLine("Missing argument -buildTarget");
                EditorApplication.Exit(120);
            }

            if (!Enum.IsDefined(typeof(BuildTarget), buildTarget ?? string.Empty))
            {
                EditorApplication.Exit(121);
            }

            if (validatedOptions.TryGetValue("buildPath", out string buildPath))
            {
                validatedOptions["customBuildPath"] = buildPath;
            }

            if (!validatedOptions.TryGetValue("customBuildPath", out string _))
            {
                Console.WriteLine("Missing argument -customBuildPath");
                EditorApplication.Exit(130);
            }

            return validatedOptions;
        }

        private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#    Parsing settings     #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                // Parse flag
                bool isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                string flag = args[current].TrimStart('-');

                // Parse optional value
                bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
                string value = flagHasValue ? args[next].TrimStart('-') : "";
                string displayValue = "\"" + value + "\"";

                // Assign
                Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                providedArguments.Add(flag, value);
            }
        }

        private static void Build(BuildTarget buildTarget, string filePath)
        {
            string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();

            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                target = buildTarget,
                locationPathName = filePath,
            };

            BuildSummary buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
            ReportSummary(buildSummary);
            ExitWithResult(buildSummary.result);
        }

        private static void ReportSummary(BuildSummary summary)
        {
            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#      Build results      #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}" +
                $"Duration: {summary.totalTime.ToString()}{Eol}" +
                $"Warnings: {summary.totalWarnings.ToString()}{Eol}" +
                $"Errors: {summary.totalErrors.ToString()}{Eol}" +
                $"Size: {summary.totalSize.ToString()} bytes{Eol}" +
                $"Logo: {PlayerSettings.SplashScreen.showUnityLogo.ToString()}{Eol}" +
                $"{Eol}"
            );

            Process.Start("/bin/bash", $"-c \"echo ::set-output name=unityVersion::{Application.unityVersion}\"");
            Process.Start("/bin/bash", $"-c \"echo ::set-output name=productNameSet::{Application.productName}\"");
            Process.Start("/bin/bash", $"-c \"echo ::set-output name=productBundleSet::{Application.identifier}\"");
            Process.Start("/bin/bash", $"-c \"echo ::set-output name=productVersionSet::{Application.version}\"");
            Process.Start("/bin/bash", $"-c \"echo ::set-output name=productVersionCodeSet::{PlayerSettings.Android.bundleVersionCode}\"");
        }

        private static void ExitWithResult(BuildResult result)
        {
            switch (result)
            {
                case BuildResult.Succeeded:
                    Console.WriteLine("Build succeeded!");
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Failed:
                    Console.WriteLine("Build failed!");
                    EditorApplication.Exit(101);
                    break;
                case BuildResult.Cancelled:
                    Console.WriteLine("Build cancelled!");
                    EditorApplication.Exit(102);
                    break;
                case BuildResult.Unknown:
                default:
                    Console.WriteLine("Build result is unknown!");
                    EditorApplication.Exit(103);
                    break;
            }
        }
    }
}
