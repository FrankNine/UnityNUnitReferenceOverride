# Unity NUnit Reference Override

本工具用於改寫 Unity 產生的 C# 專案檔，使 ReSharper 或是 Rider 的單元測試執行工具能在 IDE 內直接執行測試，不需回到 Unity 執行。相關說明在[這裡](https://blog.chunfuchao.com/?p=692)。

**Rider 2017.3 之後官方的腳本已經實作相同功能，使用 Rider 2017.3 之後版本的使用者不需要安裝這個腳本。JetBrains 官方討論在[這裡](https://github.com/JetBrains/resharper-unity/pull/256/files)。**

Unity 5.6 之前 Unity 的測試框架是使用 NUnit 2.6.4.0，ReSharper 與 Rider 能正確識別，從 5.6 開始 Unity 使用自己編譯的 NUnit 3.5，造成 ReSharper 跟 Rider 無法執行測試。這個腳本幫你在每次生成專案檔時重新指向 NUnit 官方版本的 DLL。

## 如何使用

* 將 Assets/NUnitReferenceOverride 複製到你的 Unity 專案
* 從 NUnit 官方下載 3.5 版 [NUnit release page](https://github.com/nunit/nunit/releases)
* 解壓縮 NUnit 
* 在 Preferences... 下會出現 NUnit Override 項目，勾選 "Overriding .csproj NUnit reference"，底下輸入 nunit.framework.dll 的路徑，請使用在 bin/net-3.5 .NET 3.5 版。
* 重新生成專案檔，或是點擊 Patch Now 按鈕，專案的 NUnit Reference 應該會更新。

## 已測試環境

* Unity 5.6, Unity 2017.3.0 [Windows 10 ReSharper 2017.3]
* Unity 5.6, Unity 2017.3.0 [OSX 12.6 Rider 2017.2]

## 故障排除

如果你在開啟專案的時候看到：

![.Net target not found](Images/project_target_not_installed.png?raw=true "TargetNotFound")

不要選擇 Change the target，這樣專案檔會無法使用。請到[這裡](https://www.microsoft.com/net/download/visual-studio-sdks)下載 .NET SDK for Visual Studio。

* 缺少 .Net 3.5 target 時安裝 ".NET Framework 3.5 SP1 runtime"
* 缺少 .Net 4.6 target (Unity 2017 experimental scripting runtime)時安裝 ".NET Framework 4.6 Developer Pack" (雖然說明頁面寫 Visual Studio 2017 內建這個，但是實測還是需要重裝)