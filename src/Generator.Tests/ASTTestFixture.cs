using System;
using CppSharp.AST;
using CppSharp.Utils;
using CppSharp.Parser;
using CppSharp.Passes;
using CppSharp.Generators;

namespace CppSharp.Generator.Tests
{
    public class ASTTestFixture
    {
        protected Driver Driver;
        protected ASTContext AstContext;

        protected void ParseLibrary(params string[] files)
        {
            var options = new DriverOptions { GeneratorKind = GeneratorKind.CSharp };
            var parserOptions = new ParserOptions();

            var testsPath = GeneratorTest.GetTestsDirectory("Native");
            parserOptions.AddIncludeDirs(testsPath);
            parserOptions.SkipPrivateDeclarations = true;

            var module = options.AddModule("Test");
            module.Headers.AddRange(files);

            Driver = new Driver(options)
            {
                ParserOptions = parserOptions
            };

            Driver.Setup();
            if (!Driver.ParseCode())
                throw new Exception("Error parsing the code");

            Driver.SetupTypeMaps();
            AstContext = Driver.Context.ASTContext;
            new CleanUnitPass { Context = Driver.Context }.VisitASTContext(AstContext);
            new ResolveIncompleteDeclsPass { Context = Driver.Context }.VisitASTContext(AstContext);
        }
    }
}
