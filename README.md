# CSE Capstone Project Template


This begins your capstone development effort.  You will use GitLab as your primary repository for code and documentation.  In these early phases, you may not be delivering 'working software' but you will be delivering just enough documentation to show an understanding of the product in order to communicate a high-level understanding of the product, objectives, scope and quality requirements.  The issues and boards can also be used to collaborate on research.  For those of you on an R&D project, issues are a great way to track each research question.  The comments and team collaboration build a large body of knowledge during your efforts. 

Regarding this readme.md file, ultimately you will update this page to reflect your project and assist anyone who has access to your repository.


# Read these articles
Read the following articles to familiarize yourself with Gitlab and how you will be expected to use it during your project.

* [How to use GitLab for Agile Software Development](https://about.gitlab.com/blog/2018/03/05/gitlab-for-agile-software-development/). 
* [How to Write a Beautiful and Meaningful README.md*](https://blog.bitsrc.io/how-to-write-beautiful-and-meaningful-readme-md-for-your-next-project-897045e3f991#:~:text=It's%20a%20set%20of%20useful,github%20below%20the%20project%20directory.) - buidling your ReadMe file 
* [Always start with an issue](https://about.gitlab.com/blog/2016/03/03/start-with-an-issue/) - This article discusses issues and how to use them to collaborate.  Several issue and merge templates are provided in the .gitlab/issue_templates and .gitlab/merge_request_templates.  These should facilitate collaboration and quality. Feel free to edit them to fit the needs of this project.
* [Template Samples](https://gitlab.com/gitlab-org/gitlab/-/tree/master/.gitlab/issue_templates)


# Time Tracking

Time tracking is NOT required.  It's very simple though.  There are other useful actions like /done, /assign, /approve and /wip to name a few.

| cmd | purpose |
| ------ | ------ |
| /estimate | in the issue description, document the initial work estimate in days, hours, or minutes |
| /spend | in the comments for the issue, indicate how much time you spend working at that time | 

Here's the link to [Quick Actions](https://docs.gitlab.com/ee/user/project/quick_actions.html).  


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





 






