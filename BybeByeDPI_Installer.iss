; ByeByeDPI Simple Installer Script

[Setup]
AppName=ByeByeDPI
AppVersion=1.1.0.12
DefaultDirName={commonpf}\ByeByeDPI
DefaultGroupName=ByeByeDPI
OutputBaseFilename=ByeByeDPI_Installer
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
DisableDirPage=yes

[Files]
; Main executables and config
Source: ".\ByeByeDPI\bin\Release\net10.0-windows\ByeByeDPI.exe"; DestDir: "{app}"; Flags: ignoreversion

; DLLs and support files
Source: ".\ByeByeDPI\bin\Release\net10.0-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion restartreplace

; Framework JSON's (Required)
Source: ".\ByeByeDPI\bin\Release\net10.0-windows\ByeByeDPI.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ByeByeDPI\bin\Release\net10.0-windows\ByeByeDPI.deps.json"; DestDir: "{app}"; Flags: ignoreversion

; GoodbyeDPI executable
Source: ".\ByeByeDPI\bin\Release\net10.0-windows\goodbyedpi.exe"; DestDir: "{app}"; Flags: ignoreversion restartreplace

[Icons]
Name: "{group}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"
Name: "{commondesktop}\ByeByeDPI"; Filename: "{app}\ByeByeDPI.exe"

[Run]
Filename: "{app}\ByeByeDPI.exe"; Description: "Launch ByeByeDPI"; Flags: nowait postinstall skipifsilent
