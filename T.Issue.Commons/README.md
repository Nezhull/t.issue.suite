# t.Issue Commons
Common utility library.

## Utils
Collection of simple static utility clases and methods for asserting conditions, hashing, encrypting, etc.

### Asserts
Basic assertions to check some conditions at runtime.
Main difference from `System.Diagnostics.Contracts` is that these assertions are not optimized away when compiler optimizes code.
Asserts are attributed with `JetBrains.Annotations.ContractAnnotationAttribute` so ReSharper/Rider static code analysis is happy.

#### Currently available assertions:
* `NotNull(object)` - simple null check;
* `IsTrue(bool)` - simple boolean check;
* `IsTrue(bool?)` - simple nullable boolean check _(`null` throws assertion exception)_;
* `IsTrueSafe(bool?)` - simple nullable boolean check _(`null` does not throw assertion exception)_;
* `HasText(string)` - simple `string.IsNullOrWhiteSpace(string)` check;
* `IsNotEmpty(string)` - simple `string.IsNullOrEmpty(string)` check;
* `IsNotEmpty<T>(T[])` - simple array `null` or empty check;
* `IsNotEmpty(ICollection)` - simple collection `null` or empty check;
* `IsNotEmpty<T>(ICollection<T>)` - simple generic collection `null` or empty check;

#### Examples
```csharp
public void Method(string param1, int param2, IList<long> param3)
{
    Assert.HasText(param1, $"Parameter {nameof(param1)} must not be null, empty or consist of only spaces");
    Assert.IsTrue(param2 >= 0, $"Parameter {nameof(param2)} can not be negative.");
    Assert.IsNotEmpty(param3);
    
    // ...
}
```

### DateTime Utils
Utility methods to format and parse `DateTime` from/to string.

#### Utility methods:
* `FormatDateTime(DateTime/DateTime?, string, CultureInfo)` - formats `DateTime` or `DateTime?` using provided or default `yyyy-MM-dd HH:mm:ss` format;
* `FormatTimeSpan(TimeSpan/TimeSpan?, string, CultureInfo)` - formats `TimeSpan` or `TimeSpan?` using provided or default `hh:mm` format;
* `ParseDateTime(string, string, CultureInfo, DateTimeStyles)` - parses `DateTime?` from string using provided or default `yyyy-MM-dd HH:mm:ss` format _(returns `null` on error)_;
* `FormatDateRange(DateTime?, DateTime?, string, string, CultureInfo)` - formats date range using provided or default `yyyy-MM-dd` format and separator;

#### Examples
```csharp
public DateTime? ParseDateTime(string dateTimeStr, string format = "yyyyMMdd")
{
    Assert.HasText(format);

    return DateTimeUtils.ParseDateTime(dateTimeStr, format);
}
```

### Encryption Utils
