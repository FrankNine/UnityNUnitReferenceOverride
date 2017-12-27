# Unity NUnit Reference Override

This tool is for running unit tests in IDE such as Visual Studio with Resharper or Rider after Unity 5.6. 

Before Unity 5.6, Unity includes NUnit framework 2.6.4.0 which can be tested by NUnit 2 test runner without any problem. After 5.6, Unity switched to a modified 3.5 NUnit framework and cannot work with standard NUnit 3 test runner. This tool modifies Unity generated CSharp  editor project and override nunit.framework path hint to custom location. You can point the reference to standard NUnit 3.5 dll and test runner shall work again.

## How to use
* Copy editor scripts under Assets/NUnitReferenceOverride to your project
* Download NUnit 3.5 form [NUnit release page](https://github.com/nunit/nunit/releases)
* Extract NUnit package
* In Unity Edit/Preference... under "NUnit Override", check "Overriding .csproj NUnit reference" and enter path to nunit.framework.dll. Use the one in bin/net-3.5.
* Regenerate CSharp project, the nunit.framework reference should be changed.

## Tested Environment 
* Unity 5.6, Unity 2017.3.0 on Windows 10 with ReSharper 2017.3
* Unity 5.6, Unity 2017.3.0 on OSX 12.6 with Rider 2017.2
