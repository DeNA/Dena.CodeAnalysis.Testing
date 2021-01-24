Dena.CodeAnalysis.Testing
=========================

This library provides TDD friendly DiagnosticAnalyzer test helpers:

* DiagnosticAnalyzerRunner

    A runner for [`Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer?view=roslyn-dotnet).
    The purpose of the runner is providing another runner instead of [`Microsoft.CodeAnalysis.Analyzer.Testing.AnalyzerVerifier.VerifyAnalyzerAsync`](https://github.com/dotnet/roslyn-sdk/blob/3046d1dffafd47ced55e4b76fd865179154c87ab/src/Microsoft.CodeAnalysis.Testing/Microsoft.CodeAnalysis.Analyzer.Testing/AnalyzerVerifier%603.cs#L13-L19).

    Because of the `AnalyzerVerifier` has several problems:

    1. Using AnalyzerVerifier, it is hard to instantiate analyzer with custom arguments (the custom args may be needed if your analyzer is composed by several smaller analyzer-like components)
    2. AnalyzerVerifier may throw some exceptions because it test Diagnostics. But it should be optional because analyzer-like smaller components may not need it. If it is not optional the tests for the components become to need to wrap try-catch statements for each call of `VerifyAnalyzerAsync`

* Test Doubles for DiagnosticAnalyzer
    * NullAnalyzer: it do nothing
    * StubAnalyzer: it analyze codes with a `Dena.CodeAnalysis.Testing.AnalyzerActions`
    * SpyAnalyzer: it analyze codes and do not report any Diagnostics, but instead it records all actions that registered via [`Microsoft.CodeAnalysis.Dignostics.AnalysisContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.analysiscontext?view=roslyn-dotnet)


Requirements
------------

* .NET5.0 or later



Usage
-----

### Run DiagnosticAnalyzer

```c#
var analyzer = new YourAnalyzer();

// The analyzer get intialized and get to call registered actions.
await DiagnosticAnalyzerRunner.Run(
    analyzer,
    @"public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");
```



### Get Diagnostics

```c#
var analyzer = new YourAnalyzer();

var diagnostics = await DiagnosticAnalyzerRunner.Run(
    analyzer,
    @"public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");

Assert.AreEqual(0, diagnostics.Length);
```



### Check whether the DiagnosticAnalyzer.Initialize have been called

```c#
var spyAnalyzer = new SpyAnalyzer();

var diagnostics = await DiagnosticAnalyzerRunner.Run(
    spyAnalyzer,
    @"public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");

Assert.IsTrue(spyAnalyzer.IsInitialized);
```



### Check recorded actions

```c#
var spyAnalyzer = new SpyAnalyzer();

var diagnostics = await DiagnosticAnalyzerRunner.Run(
    spyAnalyzer,
    @"public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");

// CompilationActionHistory hold the Compilation object that given
// to the action registered by AnalysisContext.RegisterCompilationAction.
Assert.AreEqual(1, spyAnalyzer.CompilationActionHistory.Count);

// Other available histories are:
//
//   - spyAnalyzer.CodeBlockActionHistory
//   - spyAnalyzer.CodeBlockStartActionHistory
//   - spyAnalyzer.CompilationActionHistory
//   - spyAnalyzer.CompilationStartActionHistory
//   - spyAnalyzer.OperationActionHistory
//   - spyAnalyzer.OperationBlockActionHistory
//   - spyAnalyzer.OperationBlockStartAction
//   - spyAnalyzer.OperationBlockStartActionHistory
//   - spyAnalyzer.SemanticModelActionHistory
//   - spyAnalyzer.SymbolActionHistory
//   - spyAnalyzer.SymbolStartActionHistory
//   - spyAnalyzer.SyntaxNodeActionHistory
//   - spyAnalyzer.SyntaxTreeActionHistory
```



### Do something in action

```c#
var stubAnalyzer = new StubAnalyzer(
    new AnalyzerActions
    {
        CodeBlockStartAction = context => DoSomething()
    }
);

await DiagnosticAnalyzerRunner.Run(
    stubAnalyzer,
    @"public static class Program
{
    public static void Main()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");

// Other available actions are:
//
//   - stubAnalyzer.CodeBlockAction
//   - stubAnalyzer.CodeBlockStartAction
//   - stubAnalyzer.CompilationAction
//   - stubAnalyzer.CompilationStartAction
//   - stubAnalyzer.OperationAction
//   - stubAnalyzer.OperationBlockAction
//   - stubAnalyzer.OperationBlockStartAction
//   - stubAnalyzer.OperationBlockStartAction
//   - stubAnalyzer.SemanticModelAction
//   - stubAnalyzer.SymbolAction
//   - stubAnalyzer.SymbolStartAction
//   - stubAnalyzer.SyntaxNodeAction
//   - stubAnalyzer.SyntaxTreeAction
```


License
-------

<details>
<summary>MIT license</summary>

Copyright 2021 DeNA Co., Ltd.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
</details>
