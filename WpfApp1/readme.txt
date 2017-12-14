* 注意点
- Offline Installの場合、「必須コンポーネント(prerequisites)」は、http://go.microsoft.com/fwlink から、DLして同梱させる必要あり
-- ex.)NDP462-KB3151800-x86-x64-AllOS-ENU.exe 

- Visual Studio 2017 の場合、Installerを保存するFolderが 2015 から大きく変更されている
|vs|folder|h
|2015以前|C:\Program Files (x86)\Microsoft Visual Studio 14.0\SDK\Bootstrapper\Packages|
|2017以降？|C:\Program Files (x86)\Microsoft SDKs\ClickOnce Bootstrapper\Packages|

- 多言語版を入れる場合は、各言語フォルダに、対象言語用のファイルも入れておく必要あり
-- ex.) packages/ja/-- ex.)NDP462-KB3151800-x86-x64-AllOS-JPN.exe 


- そもそも、Version Check のVersionが不具合？により修正された過去があり、そのせいで/ja/package.xml内の、比較Versionの修正が必要な可能性もある
|概要|URL|h
|Net Framework のDL一覧（英語版のみ？）|https://msdn.microsoft.com/en-us/library/ee942965%28v=vs.110%29.aspx、https://docs.microsoft.com/en-us/dotnet/framework/deployment/deployment-guide-for-developers|
|各言語用|/ja/package.xml内のこんな感じのとこにあるURL <String Name="DotNetFX461FullLanguagePackBootstrapper">http://go.microsoft.com/fwlink/?linkid=671731&clcid=0x411 </String> |
|Framework versionとVersion記述|https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies|
|元の情報|https://developercommunity.visualstudio.com/content/problem/10716/clickonce-can-not-find-462-prerequisite.html|



VC++runtime の追加情報　VisualStudio2017InstallerProject
https://blogs.msdn.microsoft.com/jpvsblog/2017/06/22/vs2017-vc14-installer/