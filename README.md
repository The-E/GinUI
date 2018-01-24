# GinUI
A Windows frontend for gin


#For testing purposes

0. Add all the paths set by gin_shell.bat to the SYSTEM path.
1. Install Dokan https://github.com/dokan-dev/dokany/releases/download/v1.1.0/Dokan_x64.msi
2. Install the Visual Studio Installer Projects extension https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects
3. (Assuming Visual Studio 2017 is installed) Open Solution file, compile. This will trigger a nuget package restore.
4. Open the VS command line (Tools -> Visual Studio Command Prompt)
5. Go to the output directory for the Gin Client Service (should be \GinUI\GinClientService\bin\debug) on the commandline
6. Use the following command: "InstallUtil GinService.exe" This installs the Windows Service. Reboot if prompted.
7. Open services.msc. Locate the "Gin Client Service", and start it manually.
8. The VS debugger will pop up. It will ask you to start a new instance of VS with admin rights. Let it. Once it has stopped at the designated breakpoint (you'll see it in the code at GinClientWindowsService.cs line 25; this line can be commented out safely), hit F5 to let execution proceed.
9. Start the Gin Client App (should be in \GinClientApp\bin\Debug), either manually or by pressing F5 in that non-admin VS instance.
10. When first started, the Gin Client will prompt for a username and password for the default gin instance (gin-g-node.org)
11. Enter valid credentials, then press "Login"
12. If no repositories managed by the Windows client exist, a dialog will pop up that will allow you to create one. Click "Add new", and enter a gin get commandline in the now following dialog (e.g. "gin get achilleas/gin-cli-builds").
13. Close the gin repo manager window. This will create a new local copy of the repo and create a mountpoint on the Desktop under "Gin Repositories".
14. To quit, first exit the GinClient by right-clicking on its taskbar icon and choosing the only option available, then manually stop the GinClientService in services.msc
