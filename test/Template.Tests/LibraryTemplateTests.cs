using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Azure.KeyVault.Models;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Templates.Tests
{
    public class CommonApplicationTests : AcceptanceTestBase
    {
        public static IEnumerable<string> Templates = new[]
        {
            "rocketapi",
            "rocketcli",
            "rockethost",
            "rocketweb",
            "rockerworker",
        };

        class TemplateData : TheoryData<string>
        {
            public TemplateData()
            {
                foreach (var t in Templates) Add(t);
            }
        }

        public CommonApplicationTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Theory]
        [ClassData(typeof(TemplateData))]
        public void Should_Emit_DryIoc(string template)
        {
            var (directory, output) = InvokeDotnetNew(template, x => x
               .AddPair("skipSolution")
               .AddPair("dryioc")
            );

            var project = XDocument.Load(directory.GlobFiles("*.csproj").Single());
            var program = TextTasks.ReadAllText(directory.GlobFiles("Program.cs").Single());

            project.Descendants("PackageReference")
               .Should().ContainSingle(x => x.Attribute("Include").Value == "Rocket.Surgery.Hosting.DryIoc");
            program.Should().Contain(".ConfigureRocketSurgery(builder => builder.UseDryIoc())");
        }

        [Theory]
        [ClassData(typeof(TemplateData))]
        public void Should_Emit_Autofac(string template)
        {
            var (directory, output) = InvokeDotnetNew(template, x => x
               .AddPair("skipSolution")
               .AddPair("autofac")
            );

            var project = XDocument.Load(directory.GlobFiles("*.csproj").Single());
            var program = TextTasks.ReadAllText(directory.GlobFiles("Program.cs").Single());

            project.Descendants("PackageReference")
               .Should().ContainSingle(x => x.Attribute("Include").Value == "Rocket.Surgery.Hosting.Autofac");
            program.Should().Contain(".ConfigureRocketSurgery(builder => builder.UseAutofac())");
        }
    }

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
            var (directory, output) = InvokeDotnetNew(
                "rocketlib",
                x => x.AddPair("name", "test123").AddPair("skipSolution")
            );
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
        public void Should_Emit_Multiple_Conventions(Should_Emit_Multiple_Conventions_Item item)
        {
            var ( dictionary, action) = item;
            var (directory, output) = InvokeDotnetNew("rocketlib", dictionary);
            action(directory);
        }

        public class Should_Emit_Multiple_Conventions_Item
        {
            public IDictionary<string, string?> Parameters { get; }
            public Action<AbsolutePath> Validate { get; }

            public Should_Emit_Multiple_Conventions_Item(
                IDictionary<string, string?> parameters,
                Action<AbsolutePath> validate
            )
            {
                Parameters = parameters ?? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                Validate = validate;
            }

            public override string ToString() => string.Join(", ", Parameters.Select(x => $"{x.Key}:{x.Value}"));

            public void Deconstruct(
                [NotNull] out IDictionary<string, string?> parameters,
                [NotNull] [ItemNotNull] out Action<AbsolutePath> validate
            )
            {
                parameters = Parameters;
                validate = Validate;
            }
        }

        class Should_Emit_Multiple_Conventions_Data : TheoryData<Should_Emit_Multiple_Conventions_Item>
        {
            public Should_Emit_Multiple_Conventions_Data()
            {
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("autofac")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.Autofac;");
                        text.Should().Contain("public void Register(IAutofacConventionContext context)");
                        text.Should().Contain("IAutofacConvention { }");
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.Autofac");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("dryioc")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.DryIoc;");
                        text.Should().Contain("public void Register(IDryIocConventionContext context)");
                        text.Should().Contain("IDryIocConvention { }");
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.DryIoc");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("commandline")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.CommandLine;");
                        text.Should().Contain("public void Register(ICommandLineConventionContext context)");
                        text.Should().Contain("ICommandLineConvention { }");
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.CommandLine");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("serilog")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.Serilog;");
                        text.Should().Contain("public void Register(ISerilogConventionContext context)");
                        text.Should().Contain("ISerilogConvention { }");
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.Serilog");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("webjobs")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().Contain("using Rocket.Surgery.Extensions.WebJobs;");
                        text.Should().Contain("public void Register(IWebJobsConventionContext context)");
                        text.Should().Contain("IWebJobsConvention { }");
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.WebJobs");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("no-configuration")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().NotContain("using Rocket.Surgery.Extensions.Configuration;");
                        text.Should().NotContain("public void Register(IConfigurationConventionContext context)");
                        text.Should().NotContain("IConfigurationConvention { }");
                        project.Descendants("PackageReference").Should()
                           .NotContain(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.Configuration");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("no-logging")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().NotContain("using Rocket.Surgery.Extensions.Logging;");
                        text.Should().NotContain("public void Register(ILoggingConventionContext context)");
                        text.Should().NotContain("ILoggingConvention { }");
                        project.Descendants("PackageReference").Should()
                           .NotContain(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.Logging");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("no-di")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var file = path.GlobFiles("*Convention.cs").Single();
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        var text = TextTasks.ReadAllText(file);
                        text.Should().NotContain("using Rocket.Surgery.Extensions.DependencyInjection;");
                        text.Should().NotContain("public void Register(IDependencyInjectionConventionContext context)");
                        text.Should().NotContain("IDependencyInjectionConvention { }");
                        project.Descendants("PackageReference").Should()
                           .NotContain(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.DependencyInjection");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("automapper")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.AutoMapper");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("fluentvalidation")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.FluentValidation");
                    }
                ));
                Add(new Should_Emit_Multiple_Conventions_Item(
                    new Dictionary<string, string?>()
                       .AddPair("mediatr")
                       .AddPair("skipSolution"),
                    path =>
                    {
                        var project = XDocument.Load(path.GlobFiles("*.csproj").Single());
                        project.Descendants("PackageReference").Should()
                           .ContainSingle(z => z.Attribute("Include").Value == "Rocket.Surgery.Extensions.MediatR");
                    }
                ));
            }
        }
    }
}