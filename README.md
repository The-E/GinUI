# GinUI
A Windows frontend for gin


#For testing purposes

1. Install Dokan https://github.com/dokan-dev/dokany/releases/download/v1.0.5/DokanSetup-1.0.5.1000.exe
2. (Assuming Visual Studio 2017 is installed) Open Solution file, compile. This will trigger a nuget package restore.
3. In the GinClientApp project, locate Program.cs
4. In line 47 and following, change the paths mentioned there to something that would be valid on your end.
    The first path points to a directory with an existing gin repository, the second one defines a mountpoint.
    Recompile.
5. Open the VS command line (Tools -> Visual Studio Command Prompt)
6. Go to the output directory for the Gin Client Service (should be \GinUI\GinClientService\bin) on the commandline
7. Use the following command: "InstallUtil GinClientService.exe" This installs the Windows Service. Reboot if prompted.
8. Open services.msc. Locate the "Gin Client Service", and start it manually.
9. The VS debugger will pop up. It will ask you to start a new instance of VS with admin rights. Let it. Once it has stopped at the designated breakpoint (you'll see it in the code at GinClientWindowsService.cs line 25; this line can be commented out safely), hit F5 to let execution proceed.
10. Start the Gin Client App (should be in \GinClientApp\bin\Debug), either manually or by pressing F5 in that non-admin VS instance.
11. To quit, first exit the GinClient by right-clicking on its taskbar icon and choosing the only option available, then manually stop the GinClientService in services.msc
