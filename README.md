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

## Troubleshooting
If you are seeing this after opening a Unity generated project:
![.Net target not found](Extras/project_target_not_installed.png?raw=true "TargetNotFound")

Do not change target and force open the project since it would lose all of its references and cannot build to run the tests. Try install corresponding .NET SDKs for Visual Studio form [here](https://www.microsoft.com/net/download/visual-studio-sdks)

* For missing .Net 3.5 target, install ".NET Framework 3.5 SP1 runtime"
* For missing .Net 4.6 target (Unity 2017 experimental scripting runtime), install ".NET Framework 4.6 Developer Pack" (Although the download page stated it is included in Visual Studio 2017, but it is actually not)
