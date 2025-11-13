[Setup]
AppName=Smart Restaurant System
AppVersion=1.0.0
DefaultDirName={pf}\SmartRestaurantSystem
DefaultGroupName=SmartRestaurantSystem
OutputBaseFilename=SmartRestaurant_Setup
Compression=lzma
SolidCompression=yes
OutputDir=Output

[Files]
Source: "..\publish\wpf\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Smart Restaurant"; Filename: "{app}\RestaurantWPF.exe"
Name: "{commondesktop}\Smart Restaurant"; Filename: "{app}\RestaurantWPF.exe"
