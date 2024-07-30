# Climb Higher

Climb Higher is a mobile application designed to support rock climbing training and improve accessibility for individuals with color blindness. Our team is leveraging computer vision technology to assist color blind climbers in identifying holds that they may struggle to distinguish without our software. We are developing the application using .NETMAUI and adhering to an Agile framework for efficient and iterative development.

# Demo 
(Insert pictures of new GUI here?)

# Branches

| **Name**   | **Description** |
| ------ | ------ |
| Master         | Protected branch.  You cannot push directly to master.  This branch should be what you push to your test server (ceclnx for example) or other devices for your client to review. |
| Development          |  The development branch is the primary working branch. When we work on new development issues, we create a new sprint branch from development, work on that branch, then push that branch back into Development. |
| Sprint Branches         |  Each developer that is planning to work on a development issue during the sprint will create a new sprint branch titled SprintX_Initials (Ex. Sprint1_HB). At the end of the sprint they will push their branch back into development and merge with other sprint development branches. |

# Setting up for development

Setting up a computer for development requires: Visual Studio, .NET 7.0, and Git.

Setting up Visual Studio: 
When installing [Visual Studio](https://visualstudio.microsoft.com) you need to install the .NET MAUI package and .NET 7.0.

Git:
If on Windows you need to install [Git bash](https://git-scm.com/downloads). 
Allow it to make changes to your computer and press next until the wizard is finished installing.

On Mac: Open the terminal and enter this command:
> /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

After executing this command it will request your password to ensure you have sudo access.
Enter your password and execute the following command:

> brew install git

You should now be able to use git in your terminal. 
You can check this by using the command:

> git --version

If git is installed it will simply tell you what version you are running (Ex. git version 2.44.0).

In the git bash command prompt (Windows), or the Terminal (Mac), navigate to the desired location on your computer using the cd command and execute: 
> git clone https://gitlab.csi.miamioh.edu/2024-capstone/climb-higher/climb-higher.git

This will download the files from the repository to your local computer.

To access the project:
1. Open the downloaded 'climb-higher' folder.
2. Open the 'src' directory.
3. Locate the 'climb-higher.sln' file.
4. Right-click the 'climb-higher.sln' file.
5. Hover over the menu and select "Open-With".
6. Select Visual Studio.

The project will load into Visual Studio and you should be able to run it.





 






