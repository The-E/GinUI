# GinUI
A Windows frontend for gin

## Introduction
GinUI is a graphical frontend for the gin infrastructure developed by the [German Neuroinformatics Node](http://www.g-node.org/).  It features an easy-to-use UI that allows users to perform basic operations on gin repositories; It also integrates into Explorer so that it can be used similar to commercial products like Dropbox(TM).

### How it works
At their heart, gin repositories are git repositories using the git-annex extension to store large files. This means that the bulk of data in a given repository is not actually present on user harddrives when they make a checkout; as a result, files stored within gin are not directly accessible right away. To make them available for use, it is necessary to use the gin client to perform the actual file retrieval. 
GinUI simplifies this process by automating it. When a file in a repository is accessed, GinUI's background service will automatically intercept that file access call and, if necessary, retrieve the file from the remote repository.
To make this process invisible to the user, GinUI stores the actual data within its AppData directory while putting a mirror of that directory on the user's Desktop. 

## Installation

(Add DL link to the release)

## Building the project

This project is built for Visual Studio 2017. The following extensions should be installed in order to fully build the project:
1. [Visual Studio Installer Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects)
2. (Optionally) The Windows Communication Foundation component, available in the Visual Studio Installer under Development Activities

The project uses the following nuget packages:
1. DokanNet 
2. MetroModernUI
4. Newtonsoft.Json
5. SharpShell

## Development Guidelines
### Project setup
The GinClientApp solution is comprised of the following subprojects:
1. GinClientApp
This contains the source code for the desktop client
2. GinClientLibrary
This project contains common code referenced by the other projects. It also contains the implementation of the Dokan driver interface, GinRepository and RepositoryManager classes the service relies on
3. GinService
This project contains the source code for the Windows service component
4. GinShellExtension
This project contains the source code for the Shell Extension component
5. InstallerLibrary
This project contains the source code for custom actions used within the Setup project
6. Setup
A Visual Studio Installer project that builds the installer

### Coding Style
This project uses ReSharper style throughout.
Private variables and fields should always be lowercase and preceded by an underscore, i.e. `private int _someint;`
Wherever possible, use `var` instead of an explicit type.
Brace style is Allman.

### Subproject-specific notes
#### GinClientApp
The GinClientApp is a Windows Forms-based client. By default, the only UI displayed is the notification area icon created through the GinApplicationContext class. To achieve a more modern look, the MetroModernUI is used.
When adding new dialogues or new messages, take care to place these within the app's Resources (resources.resx) to allow for easier localization.
#### GinClientLibrary
The output of this project has to be signed, preferably using the same key used for the GinShellExtension library.
#### GinService
The IGinService interface defines the communications protocol for all interactions between the Desktop client and the service. 
The service exposes a WCF-based interface configured to use a multithreaded backend; as such, great care should be taken to enclose anything that will affect the state of the repositories or the RepositoryManager within locks.
#### GinShellExtension
The shell extension uses the SharpShell framework by Dave Kerr. Documentation for its use can be found here: https://github.com/dwmkerr/sharpshell
#### InstallerLibrary and setup
The InstallerLibrary performs custom actions that cannot be replicated within a VS Installer project. At the moment, these are:
1. Downloading, unpacking and installing the gin-cli client for Windows
2. Giving the user the right to reserve the url "http://+:8738/GinService" on the local system
3. Starts the service and the client after installation is complete
The setup itself is a mostly straightforward VS Installer project, except for one thing: When the installer is built, the script file "ModifyMsiToEnableLaunchApplication.js" is executed to enable the installer to launch the DokanSetup.exe after installation is complete; this is something that cannot be defined within a VS Installer project for unknown reasons.

### Future work
The following tasks are planned to be executed sometime post-initial release:
1. Rolling the service into the GinClientApp application
Nothing within the service _requires_ running it within a service context; this design was chosen at the start of implementation based on then-available information. It should be possible to implement its functionality within the desktop client; this would result in an overall cleaner project using the least amount of priviledge required
2. Switching the WCF communication backend to use named pipes
At the moment, the project is using HTTP-based methods to communicate. While certainly straightforward and, within the context of a locally hosted and executed application, reasonably fast, it is also more open than it needs to be; using a named pipe would resolve this.
