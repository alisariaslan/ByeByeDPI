; ByeByeDPI Basit Kurulum Scripti

[Setup]
AppName=ByeByeDPI
AppVersion=1.0
DefaultDirName={pf}\ByeByeDPI
DefaultGroupName=ByeByeDPI
OutputBaseFilename=ByeByeDPISetup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "turkish"; MessagesFile: "compiler:Languages\Turkish.isl"

[Files]
; Ana exe ve config
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion

; DLL ve diğer destek dosyaları
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.sys"; DestDir: "{app}"; Flags: ignoreversion

; Eğer publish klasörü veya diğer alt klasörler de gerekli ise:
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\app.publish\*"; DestDir: "{app}\app.publish"; Flags: recursesubdirs ignoreversion

[Icons]
Name: "{group}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"
Name: "{commondesktop}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"

[Run]
Filename: "{app}\ByeByeDPI.exe"; Description: "{cm:LaunchProgram,ByeByeDPI}"; Flags: nowait postinstall skipifsilent
