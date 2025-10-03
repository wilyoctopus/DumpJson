# DumpJson - write your objects to console/debug output via JSON instantly

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


DumpJson is a lightweight .NET library allows to call `.Dump()` on any object to instantly serialize and output it to your console or debug window with formatted JSON.

## Features

- **Dead Simple** - Just add `.Dump()` to any object
- **JSON Output** - Automatic JSON serialization with indentation
- **Smart Routing** - Outputs to Console and/or Debug window automatically
- **Optional Labels** - Add a label to your output for context
- **Configurable** - Customize JSON serialization options
- **Lightweight** - ~60 lines of code (currently), System.Text.Json is the only dependency 
- **.NET Standard 2.0** - Works everywhere (.NET Framework, .NET Core, .NET 5+)

## Installation

```bash
dotnet add package DumpJson
```

Or via Package Manager Console:

```powershell
Install-Package DumpJson
```

## Quick Start

```csharp
using DumpJson;

var person = new Person 
{ 
    Name = "John Doe", 
    Age = 30,
    Email = "john@example.com"
};

// View the output in your console or debug window via .Dump()
person.Dump();

// Apply a label if needed
person.Dump("Person response");
```

### Standard Output Example:

```
{
  "Name": "John Doe",
  "Age": 30,
  "Email": "john@example.com"
}
```

### Labeled Output Example:

```
Person response
{
  "Name": "John Doe",
  "Age": 30,
  "Email": "john@example.com"
}
```

## Use Cases

### Quick Debugging
```csharp
// Inspect API responses
var response = await httpClient.GetFromJsonAsync<User>(url);
response.Dump("API Response");
```

### Chain Inspection
```csharp
// See data at each transformation step
var result = data
    .Where(x => x.IsActive)
    .Dump("After Filter")
    .Select(x => new { x.Id, x.Name })
    .Dump("After Projection")
    .ToList();
```

### Unit Test Diagnostics
```csharp
[Fact]
public void TestComplexObject()
{
    var result = CalculateComplexResult();
    result.Dump("Test Result"); // See exactly what you got
    Assert.Equal(expected, result);
}
```

## Configuration

Control where the output is sent or customize JSON serialization and output behavior to your needs:

```csharp
using System.Text.Json;
using DumpJson;

// Force console output on/off (null = auto-detect)
DumpJson.Settings.WriteToConsoleOutput = true;  // Always write to console
DumpJson.Settings.WriteToConsoleOutput = false; // Never write to console
DumpJson.Settings.WriteToConsoleOutput = null;  // Auto-detect (default)

// Force debug output on/off (null = auto-detect)
DumpJson.Settings.WriteToDebugOutput = true;  // Always write to debug output
DumpJson.Settings.WriteToDebugOutput = false; // Never write to debug output
DumpJson.Settings.WriteToDebugOutput = null;  // Auto-detect (default)

// Configure global JSON serialization settings (defaults to indented formatting)
DumpJson.Settings.JsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

// Now all .Dump() calls use your custom settings
myObject.Dump();
```

## How It Works

DumpJson automatically detects your runtime environment and outputs accordingly:

- **Console Available?** → Outputs to `Console.WriteLine()`
- **Debugger Attached?** → Outputs to `Debug.WriteLine()`
- **Both Available?** → Outputs to both!
- **Neither Available?** → Silently does nothing (no exceptions)

You can override this auto-detection behavior using `DumpJson.Settings.WriteToConsoleOutput` and `DumpJson.Settings.WriteToDebugOutput` to force output on or off regardless of the environment.

## Feedback

Found a bug or have a feature request? [Open an issue](https://github.com/wilyoctopus/DumpJson/issues)!

