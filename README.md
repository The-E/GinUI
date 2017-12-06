# GinUI
A Windows frontend for gin


#For testing purposes

0. Add all the paths set by gin_shell.bat to the SYSTEM path.
1. Install Dokan https://github.com/dokan-dev/dokany/releases/download/v1.0.5/DokanSetup-1.0.5.1000.exe
2. (Assuming Visual Studio 2017 is installed) Open Solution file, compile. This will trigger a nuget package restore.
3. Open the VS command line (Tools -> Visual Studio Command Prompt)
4. Go to the output directory for the Gin Client Service (should be \GinUI\GinClientService\bin) on the commandline
5. Use the following command: "InstallUtil GinClientService.exe" This installs the Windows Service. Reboot if prompted.
6. Open services.msc. Locate the "Gin Client Service", and start it manually.
7. The VS debugger will pop up. It will ask you to start a new instance of VS with admin rights. Let it. Once it has stopped at the designated breakpoint (you'll see it in the code at GinClientWindowsService.cs line 25; this line can be commented out safely), hit F5 to let execution proceed.
8. Start the Gin Client App (should be in \GinClientApp\bin\Debug), either manually or by pressing F5 in that non-admin VS instance.
9. When first started, the Gin Client will prompt for a username and password for the default gin instance (gin-g-node.org)
10. Enter valid credentials, then press "Login"
11. If no repositories managed by the Windows client exist, a dialog will pop up that will allow you to create one. Click "Add new", and enter a gin get commandline in the now following dialog (e.g. "gin get achilleas/gin-cli-builds").
12. Close the gin repo manager window. This will create a new local copy of the repo and create a mountpoint on the Desktop under "Gin Repositories".
13. To quit, first exit the GinClient by right-clicking on its taskbar icon and choosing the only option available, then manually stop the GinClientService in services.msc
