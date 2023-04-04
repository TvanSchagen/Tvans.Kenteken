# Tvans.Kenteken

[![Build Status](https://dev.azure.com/teunvanschagen/Tvans.Kenteken/_apis/build/status/TvanSchagen.Tvans.Kenteken?branchName=main)](https://dev.azure.com/teunvanschagen/Tvans.Kenteken/_build/latest?definitionId=3&branchName=main)

Allows parsing, formatting, and validating dutch license plates.

`Kenteken` will accept any case, with- and without hypens. Formatted means all uppercase with hyphens. 
`Kenteken`s are considered equal when their _formatted_ value is equal.

All non-`try` functions will throw a `FormatException` on an invalid format.

As of `2023-04-04`, some available sidecodes that this library considers valid, haven't been distributed yet.

## Usage
```csharp
// create instance
var kenteken = new Kenteken("55-GJ-GJ");

// instance properties
int sidecode = new Kenteken("55-GJ-GJ").Sidecode;       // 1
string formatted = new Kenteken("55gjgj").Formatted;    // 55-GJ-GJ

// static functions
bool valid = Kenteken.Validate("55-GJ-GJ");
int sidecode = Kenteken.GetSidecode("55-GJ-GJ");
bool valid = Kenteken.TryParse("55-GJ-GJ", out Kenteken? kenteken);
Kenteken kenteken = Kenteken.Parse("55-GJ-GJ");

// extension method
Kenteken kenteken = "55-GJ-GJ".ToKenteken();

// equality comparison
var unformatted = new Kenteken("55gjgj");
var formatted = new Kenteken("55-GJ-GJ");

bool isEqual = formatted == unformatted;                // true
```
