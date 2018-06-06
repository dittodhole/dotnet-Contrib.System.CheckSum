![](assets/noun_1117655_cc.png)

# dotnet-Contrib.System.CheckSum

> Calculate, store, and verify checksums on various occasions.

## Installing

### [myget.org][1]

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-checksum/develop.svg)][3]
[![](https://img.shields.io/myget/dittodhole/vpre/Contrib.System.CheckSum.svg)][1]

```powershell
PM> Install-Package -Id Contrib.System.CheckSum -pre --source https://www.myget.org/F/dittodhole/api/v2
```

### [myget.org][2]

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-checksum/develop.svg)][3]
[![](https://img.shields.io/myget/dittodhole/vpre/Source.Contrib.System.CheckSum.svg)][2]

```powershell
PM> Install-Package -Id Source.Contrib.System.CheckSum -pre --source https://www.myget.org/F/dittodhole/api/v2
```

### [nuget.org][4]

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-checksum/master.svg)][6]
[![](https://img.shields.io/nuget/v/Contrib.System.CheckSum.svg)][4]

```powershell
PM> Install-Package -Id Contrib.System.CheckSum
```

### [nuget.org][5]

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-checksum/master.svg)][6]
[![](https://img.shields.io/nuget/v/Source.Contrib.System.CheckSum.svg)][5]

```powershell
PM> Install-Package -Id Source.Contrib.System.CheckSum
```

## Example

```csharp
namespace Example1.CreateCheckSumForString
{
  using global::System;
  using global::Contrib.System.CheckSum;

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();

    var input = "hello";

    var checkSum = stringCheckSumCalculator.CalculateCheckSum(input);
  }
}

namespace Example2.InjectHashAlgorithm
{
  using global::System;
  using global::System.Security.Cryptography;
  using global::Contrib.System.CheckSum;

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator(MD5.Create); // default is MD5
    stringCheckSumCalculator = new StringCheckSumCalculator(SHA1.Create);
    stringCheckSumCalculator = new StringCheckSumCalculator(SHA256.Create);

    var input = "hello";

    var checkSum = stringCheckSumCalculator.CalculateCheckSum(input);
  }
}

namespace Example3.CreateCheckSumForSequence
{
  using global::System;
  using global::Contrib.System.CheckSum;

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();
    var sequenceCheckSumCalculator = new SequenceCheckSumCalculator(stringCheckSumCalculator);

    var input = new[] { "hello", "foo", "bar" };

    var checkSum = sequenceCheckSumCalculator.CalculateCheckSum(input);
  }
}

namespace Example4.CreateCheckSumForDictionary
{
  using global::System;
  using global::System.Collections.Generic;
  using global::Contrib.System.CheckSum;

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();
    var sequenceCheckSumCalculator = new SequenceCheckSumCalculator(stringCheckSumCalculator);
    var dictionaryCheckSumCalculator = new DictionaryCheckSumCalculator(sequenceCheckSumCalculator);

    var input = new Dictionary<string, string>
    {
      { "entry1", "hello" },
      { "entry2", "foo" },
      { "entry3", "bar" }
    };

    var checkSum = dictionaryCheckSumCalculator.CalculateCheckSum(input);
  }
}

namespace Example5.CreateCheckSumForDictionaryExtended
{
  using global::System;
  using global::System.Collections.Generic;
  using global::Contrib.System.CheckSum;

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();
    var sequenceCheckSumCalculator = new SequenceCheckSumCalculator(stringCheckSumCalculator);
    var dictionaryCheckSumCalculatorEx = new DictionaryCheckSumCalculatorEx(sequenceCheckSumCalculator)
    {
      CheckSumKey = "CRC"
    };

    var input = new Dictionary<string, string>
    {
      { "entry1", "hello" },
      { "entry2", "foo" },
      { "entry3", "bar" },
      { "CRC", string.Empty } // this key will be ignored by the following calculations
    };

    var checkSum = dictionaryCheckSumCalculatorEx.CalculateCheckSum(input);
    checkSum = dictionaryCheckSumCalculatorEx.CalculateAndStoreCheckSum(input);
    checkSum = dictionaryCheckSumCalculatorEx.GetStoredCheckSum(input);
    var isValid = dictionaryCheckSumCalculatorEx.IsStoredCheckSumValid(input);
  }
}

namespace Example6.CreateCheckSumForObject
{
  using global::Contrib.System.CheckSum;

  public class Input
  {
    public string Entry1 { get; set; }
    public string Entry2 { get; set; }
    public string Entry3 { get; set; }
  }

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();
    var sequenceCheckSumCalculator = new SequenceCheckSumCalculator(stringCheckSumCalculator);
    var objectCheckSumCalculator = new ObjectCheckSumCalculator(sequenceCheckSumCalculator);

    var input = new Input
    {
      Entry1 = "hello",
      Entry2 = "foo",
      Entry3 = "bar"
    };

    var checkSum = objectCheckSumCalculator.CalculateCheckSum(input);
  }
}

namespace Example7.CreateCheckSumForObjectExtended
{
  using global::System;
  using global::Contrib.System.CheckSum;

  public class Input
  {
    public string Entry1 { get; set; }
    public string Entry2 { get; set; }
    public string Entry3 { get; set; }
    public string CheckSum { get; set; }
  }

  public static void Main(params string[] args)
  {
    var stringCheckSumCalculator = new StringCheckSumCalculator();
    var sequenceCheckSumCalculator = new SequenceCheckSumCalculator(stringCheckSumCalculator);
    var objectCheckSumCalculatorEx = new ObjectCheckSumCalculatorEx(sequenceCheckSumCalculator)
    {
      CheckSumPropertyName = nameof(Input.CheckSum) // this property will be ignored by the following calculations
    };

    var input = new Input
    {
      Entry1 = "hello",
      Entry2 = "foo",
      Entry3 = "bar",
      CheckSum = string.Empty
    };

    var checkSum = objectCheckSumCalculatorEx.CalculateCheckSum(input);
    checkSum = objectCheckSumCalculatorEx.CalculateAndStoreCheckSum(input);
    checkSum = objectCheckSumCalculatorEx.GetStoredCheckSum(input);
    var isValid = objectCheckSumCalculatorEx.IsStoredCheckSumValid(input);
  }
}
```

## Developing & Building

```cmd
> git clone https://github.com/dittodhole/dotnet-Contrib.System.CheckSum.git
> cd dotnet-Contrib.System.CheckSum/
dotnet-Contrib.System.CheckSum> cd build
dotnet-Contrib.System.CheckSum/build> build.bat
```

This will create `dotnet-Contrib.System.CheckSum/artifacts/Contrib.System.CheckSum.{version}.nupkg`.

## License

dotnet-Contrib.System.CheckSum is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Linen](https://thenounproject.com/term/linen/1117655/) by [Rineesh](https://thenounproject.com/rineesh) from the Noun Project.


[1]: https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.CheckSum
[2]: https://www.myget.org/feed/dittodhole/package/nuget/Source.Contrib.System.CheckSum
[3]: https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-checksum/branch/develop
[4]: https://www.nuget.org/packages/Contrib.System.CheckSum
[5]: https://www.nuget.org/packages/Source.Contrib.System.CheckSum
[6]: https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-checksum/branch/master