using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Azure.KeyVault.Models;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Templates.Tests
{
    public class LibraryTemplateTests : AcceptanceTestBase
    {
        public LibraryTemplateTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public void Should_Emit_Conventions()
        {
            var (directory, output) = InvokeDotnetNew("rocketlib", x => x.AddPair("skipSolution"));
            var name = Path.GetFileName(directory);
            directory
               .GlobFiles("*.cs")
               .Select(z => z.ToString())
               .Select(Path.GetFileName)
               .Should().HaveCount(1)
               .And.BeEquivalentTo($"{name}Convention.cs");
            directory
               .GlobFiles("*.csproj")
               .Select(z => z.ToString())
               .Select(Path.GetFileName)
               .Should().HaveCount(1)
               .And.BeEquivalentTo($"{name}.csproj");
        }

        [Fact]
        public void Should_Emit_Conventions_With_Name()
        {
            var (directory, output) = InvokeDotnetNew("rocketlib", x => x.AddPair("name", "test123").AddPair("skipSolution"));
            var name = Path.GetFileName(directory);
            directory
               .GlobFiles("*.cs")
               .Select(z => z.ToString())
               .Select(Path.GetFileName)
               .Should().HaveCount(1)
               .And.BeEquivalentTo("test123Convention.cs");
            directory
               .GlobFiles("*.csproj")
               .Select(z => z.ToString())
               .Select(Path.GetFileName)
               .Should().HaveCount(1)
               .And.BeEquivalentTo("test123.csproj");
        }

        [Theory]
        [ClassData(typeof(Should_Emit_Multiple_Conventions_Data))]
        public void Should_Emit_Multiple_Conventions(IDictionary<string, string?> dictionary, Action<AbsolutePath> action)
        {
            var (directory, output) = InvokeDotnetNew("rocketlib", dictionary);
            action(directory);
        }

        class Should_Emit_Multiple_Conventions_Data : TheoryData<IDictionary<string, string?>, Action<AbsolutePath>>
        {
            public Should_Emit_Multiple_Conventions_Data()
            {
                Add(new Dictionary<string, string?>()
                       .AddPair("autofac")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.Autofac;");
                        text.Should().Contain("public void Register(IAutofacConventionContext context)");
                        text.Should().Contain("IAutofacConvention { }");
                    });
                /*
                 --autofac                 Include Rocket.Surgery.Extensions.Autofac as a package reference
                            bool - Optional
                            Default: false / (*) true

  --dryioc                  Enable dryioc packages
                            bool - Optional
                            Default: false / (*) true

  -am|--automapper          Include AutoMapper in the dependencies
                            bool - Optional
                            Default: false / (*) true

  -cl|--commandline         Include Rocket.Surgery.Extensions.CommandLine as a package reference
                            bool - Optional
                            Default: false / (*) true

  --no-configuration        Do not include Rocket.Surgery.Extensions.Configuration as a package reference
                            bool - Optional
                            Default: false / (*) true

  -fv|--fluentvalidation    Include FluentValidation in the dependencies
                            bool - Optional
                            Default: false / (*) true

  --mediatr                 Include MediatR in the dependencies
                            bool - Optional
                            Default: false / (*) true

  --no-dependencyinjection  Include Rocket.Surgery.Extensions.DependencyInjection as a package reference
                            bool - Optional
                            Default: false / (*) true

  --logging                 Include Rocket.Surgery.Extensions.Logging as a package reference
                            bool - Optional
                            Default: false / (*) true

  --serilog                 Include Rocket.Surgery.Extensions.Serilog as a package reference
                            bool - Optional
                            Default: false / (*) true

  --webjobs                 Include Rocket.Surgery.Extensions.WebJobs as a package reference
                            bool - Optional
                            Default: false / (*) true
                 */
            }
        }
    }
}