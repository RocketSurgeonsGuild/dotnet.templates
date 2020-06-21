using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.FileSystemTasks;
using Xunit.Abstractions;

namespace Rocket.Surgery.Templates.Tests
{
    // TODO: THis is all wrong... need to figure out something better
    /// <summary>
    /// Base class for Acceptance tests.
    /// </summary>
    public class AcceptanceTestBase
    {
        private readonly ITestOutputHelper _outputHelper;

        private const string TestSummaryStatusMessageFormat =
            "Total tests: {0}. Passed: {1}. Failed: {2}. Skipped: {3}";

        protected string StandardTestOutput { get; private set; } = string.Empty;
        protected string StandardTestError { get; private set; } = string.Empty;

        private string _arguments = string.Empty;
        private readonly string _currentTest;
        private readonly string _testDirectory;

        public AcceptanceTestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            var type = outputHelper.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(outputHelper);
            _currentTest = test.DisplayName;
            using var sha1 = SHA1.Create();
            var directory = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(_currentTest)));
            if (directory.Length > 12)
            {
                directory = directory.Substring(0, 12);
            }

            _testDirectory = directory;
        }

        /// <summary>
        /// Invokes <c>dotnet</c> with specified arguments.
        /// </summary>
        /// <param name="arguments">Arguments provided to <c>dotnet</c>.exe</param>
        public (AbsolutePath directory, IEnumerable<Output> output) InvokeDotnetNew(string templateName, Action<IDictionary<string, string?>> parameters)
        {
            var dictonary = new Dictionary<string, string?>();
            parameters(dictonary);
            return InvokeDotnetNew(templateName, dictonary);
        }

        /// <summary>
        /// Invokes <c>dotnet</c> with specified arguments.
        /// </summary>
        /// <param name="arguments">Arguments provided to <c>dotnet</c>.exe</param>
        public (AbsolutePath directory, IEnumerable<Output> output) InvokeDotnetNew(string templateName, IDictionary<string, string?> parameters)
        {
            var result = Execute(templateName, parameters);
            StandardTestError = Regex.Replace(StandardTestError, @"\s+", " ");
            StandardTestOutput = Regex.Replace(StandardTestOutput, @"\s+", " ");
            _outputHelper.WriteLine("AcceptanceTestBase.Execute: stdError = {0}", StandardTestError);
            _outputHelper.WriteLine("AcceptanceTestBase.Execute: stdOut = {0}", StandardTestOutput);
            return result;
        }

        private (AbsolutePath directory, IEnumerable<Output> output) Execute(
            string templateName,
            IDictionary<string, string?> parameters
        )
        {
            var args = new List<string>()
            {
                "new",
            };

            var dir = NukeBuild.TemporaryDirectory / _testDirectory;

            args.Add(templateName);
            args.Add("-o");
            args.Add($"\"{dir}\"");
            foreach (var param in parameters)
            {
                args.Add($"--{param.Key}");
                if (!string.IsNullOrWhiteSpace(param.Value))
                {
                    args.Add($"\"{param.Value}\"");
                }
            }

            EnsureCleanDirectory(dir);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                DotNet($"new --install {NukeBuild.RootDirectory / "src"}");
                var result = DotNet(
                    string.Join(" ", args),
                    timeout: 80 * 1000,
                    outputFilter: (v) =>
                    {
                        _outputHelper.WriteLine(v);
                        return v;
                    }
                );

                StandardTestError = string.Join(Environment.NewLine, result.Where(z => z.Type == OutputType.Err).Select(x => x.Text));
                StandardTestOutput = string.Join(Environment.NewLine, result.Where(z => z.Type == OutputType.Std).Select(x => x.Text));
                return (dir, result);
            }
            catch (ProcessException exception)
            {
                StandardTestError = exception.Message;
                StandardTestOutput = string.Empty;

                stopwatch.Stop();
                _outputHelper.WriteLine(
                    $"AcceptanceTestBase.Execute: Total execution time: {stopwatch.Elapsed.Duration()}"
                );
            }
            catch (Exception exception)
            {
                StandardTestError = $"timeout {exception.Message}";
                StandardTestOutput = string.Empty;

                stopwatch.Stop();
                _outputHelper.WriteLine(
                    $"AcceptanceTestBase.Execute: Total execution time: {stopwatch.Elapsed.Duration()}"
                );
            }

            return (dir, Enumerable.Empty<Output>());
        }
    }
}