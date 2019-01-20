# DK-Unity-Shared
Shared project for unity

<h2>Instructions for creating new project using DK-Unity-Shared</h2>

Following these steps:

<ul>
  <li>1. Create a folder for new Project anywhere on your drive. (RootPath)</li>
  <li>2. Clone this repository and put it into (RootPath) ie (SharedPath)</li>
  <li>3. Create new Unity Project, put it also to the (RootPath) ie (UnityProjectPath)</li>
  <li>4. (optional) Create Git repository for the new Unity Project</li>
  <li>5. Open NewProjectInstallationTool.exe located in (SharedPath)</li>
  <li>6. Follow the steps, select correct paths to (SharedPath) and (UnityProjectPath)</li>
</ul>

</p>After app has finished, and depending on the options you've selected, the new Unity project should have all the default folders and symbolic links created automatically. 

<b>Note</b> You can run it multiple times it can add missing folders or symbolic links and doesn't delete anything except Git settings.

<h2>Branches structure</h2>

Based on: https://nvie.com/posts/a-successful-git-branching-model/

Permanent:

</p><b>master</b> - main branch. Current version is the latest production ready code.
</p><b>develop</b> - development branch, current version of the code reflects current state of development

Temporary:

</p><b>feature-*</b> - custom development done for an implementation of something
</p><b>release-*</b> - specific version that was released and production ready
</p><b>hotfix-*</b> - custom branch created from a release branch to fix specific issue

