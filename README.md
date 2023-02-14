Dena.CodeAnalysis.Testing
=========================
[![NuGet version](https://badge.fury.io/nu/Dena.CodeAnalysis.Testing.svg)](https://www.nuget.org/packages/Dena.CodeAnalysis.Testing/)
[![CI](https://github.com/DeNA/Dena.CodeAnalysis.Testing/actions/workflows/ci.yml/badge.svg?branch=master&event=push)](https://github.com/DeNA/Dena.CodeAnalysis.Testing/actions/workflows/ci.yml)


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

* .NET Standard 2.1 or later



Usage
-----

### Run DiagnosticAnalyzer

```c#
var analyzer = new YourAnalyzer();

// The analyzer get intialized and get to call registered actions.
await DiagnosticAnalyzerRunner.Run(
    analyzer,
    @"public static class Foo
{
    public static void Bar()
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
    @"public static class Foo
{
    public static void Bar()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}");

Assert.AreEqual(0, diagnostics.Length);
```



### Assert Locations
```c#
var location = diagnostic.Location;

LocationAssert.HaveTheSpan(
    "/0/Test0.",             // Optional. Skip path assertion if the path not specified,  
    new LinePosition(1, 0),
    new LinePosition(8, 5),
    location
);
```



### Print Diagnostics
```c#
var diagnostics = await DiagnosticAnalyzerRunner.Run(
    anyAnalyzer,
    @"
internal static class Foo
{
    internal static void Bar()
    {
        System.Console.WriteLine(""Hello, World!"");
    }
}
ERROR");

Assert.AreEqual(0, diagnostics.Length, DiagnosticsFormatter.Format(diagnostics));
// This message is like:
//
//   // /0/Test0.cs(9,1): error CS0116: A namespace cannot directly contain members such as fields or methods
//   DiagnosticResult.CompilerError(""CS0116"").WithSpan(""/0/Test0.cs"", 9, 1, 9, 6),
```



### Check whether the DiagnosticAnalyzer.Initialize have been called

```c#
var spyAnalyzer = new SpyAnalyzer();

var diagnostics = await DiagnosticAnalyzerRunner.Run(
    spyAnalyzer,
    @"public static class Foo
{
    public static void Bar()
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
    @"public static class Foo
{
    public static void Bar()
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
    @"public static class Foo
{
    public static void Bar()
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

### DiagnosticAssert Class

#### AreEqual

`DiagnosticAssert.AreEqual` assert that collections of Diagnostics for equality.

Throw an assert exception if given collections satisfy the following condition:

Elements that are only contained on one side. The equivalence is based on following properties

- File path (e.g., path/to/file.cs)
- Location of the `Diagnostic` (starting line number, starting character position)-(finishing line number, finishing
  character position)
- Identifier of the `DiagnosticDescriptor` (DDID) (e.g., CS0494)
- `DiagnosticMessage` (e.g., The field 'C.hoge' is assigned but its value is never used)

Otherwise, do nothing.

```csharp
[Test]
public async Task M()
{
    var analyzer = new YourAnalyzer();
    const string testData = @"
class C
{
    string {|hoge|CS0414|The field 'C.hoge' is assigned but its value is never used|} = ""Forgot semicolon string""
}";

    var (source, expected) = TestDataParser.CreateSourceAndExpectedDiagnosticFromFile(testData);
    var actual = await DiagnosticAnalyzerRunner.Run(analyzer, source);
    DiagnosticsAssert.AreEqual(expected, actual);
}
```

Output example

```
Missing 0 diagnostics, extra 1 diagnostics of all 2 diagnostics:
    extra	/0/Test0.cs: (3,43)-(3,43), CS1002, ; expected
```

#### IsEmpty

`DiagnosticAssert.IsEmpty` assert that the `Diagnositc` is no exist.

Throw an assert exception if given collections exist any `Diagnostic`.

The output format and equivalence is the same as `DiagnosticAssert.AreEqual`.

Otherwise, do nothing.

```csharp
[Test]
public async Task M()
{
    var analyzer = new YourAnalyzer();
    var source = @"
class C
{
}"; 
    var actual = await DiagnosticAnalyzerRunner.Run(analyzer, source);
    DiagnosticsAssert.IsEmpty(actual);
}
```

### TestDataParser Class

#### CreateSourceAndExpectedDiagnostics

Create source and expected diagnostic from formatting embedded.

```csharp
[Test]
public async Task M()
{
    var analyzer = new YourAnalyzer();
    const string testData = @"
class C
{
    string {|hoge|CS0414|The field 'C.hoge' is assigned but its value is never used|} = ""Forgot semicolon string""
}";

    var (source, expected) = TestDataParser.CreateSourceAndExpectedDiagnosticFromFile(testData);
}
```

The `testData` variable has formatting embedded in the source.

You can parse this format using `CreateSourceAndExpectedDiagnosticFromFile` and get the source and expected Diagnostics.

- source

```csharp
class C
{
    string hoge = "Forgot semicolon string"
}
```

- expected Diagnostics

  - Location
    - (3,11)-(3,15)
  - DDID
    - CS0414
  - DiagnosticMessage
    - The field 'C.hoge' is assigned but its value is never used

Specify the part to be reported in the following format.

The format is enclosed in `{ }` and separated by `|`.

```
{|source|DDID|DiagnosticMessage|}
```



License
-------

[MIT license](./LICENSE)
