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