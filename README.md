# client-api-windows

This project houses ezetap client API for windows. It is developed in C# and available as a Windows .NET library reference. 
It is compatible with Windows .NET version 4.0 or higher.

The following API helps you integrate your desktop application with Ezetap Services.

##Getting Started
1. .NET development environment
2. EzeTap app key/credentials
3. EzeTap device
4. SDK client available in the repository in release folder.

Initial Setup:
* Download release.zip from the release folder and unzip the contents.
* Install eze_pos_win_0_0_7.exe.
* Copy the contents in Ezecli folder into <Installed directory>/Ezetap/cli

##Sample App:

There is a sample windows application in the sample folder of this repository. This will serve as a reference to integarte with EZETAP SDK.

Steps:
* Import the sample project in Visual Studio and build the project.
* Clean and Build the project
* Run the SampleApp present in the bin directory to launch the Windows App which has been modelled to use the EzeTap SDK to use the POS services.
(LoginForm.cs serves as your first point of reference.)


##Steps to Follow :
* Download the Client SDK - eze_api_win.dll from the release.zip present in the release folder in the repository.
* Add the API dll as a reference/library to your own Windows Application project.
* Refer the <a href="http://developers.ezetap.com/api/"> Ezetap API Portal</a> for API usage.

  
