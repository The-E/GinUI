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
