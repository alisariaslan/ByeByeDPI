; ByeByeDPI Simple Installer Script

[Setup]
AppName=ByeByeDPI
AppVersion=1.0.0.10
DefaultDirName={commonpf}\ByeByeDPI
DefaultGroupName=ByeByeDPI
OutputBaseFilename=ByeByeDPI_Installer
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
DisableDirPage=yes

[Files]
; Main executables and config
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\ByeByeDPI.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion

; GoodbyeDPI executable
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\goodbyedpi.exe"; DestDir: "{app}"; Flags: ignoreversion restartreplace

; DLLs and support files
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion restartreplace
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\yearn\source\repos\ByeByeDPI\ByeByeDPI\bin\Release\*.sys"; DestDir: "{app}"; Flags: ignoreversion restartreplace

[Icons]
Name: "{group}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"
Name: "{commondesktop}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"

[Run]
Filename: "{app}\ByeByeDPI.exe"; Description: "Launch ByeByeDPI"; Flags: nowait postinstall skipifsilent
