using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewProjectInstallationTool
{
    public partial class Form1 : Form
    {
        public static class SettingAttributes
        {
            //source
            public const string FILE_SOURCE_GIT_ATTRIBUTES = "NewProjectFiles/template.gitattributes";
            public const string FILE_SOURCE_GIT_IGNORE = "NewProjectFiles/template.gitignore";

            //target
            public const string FILE_TARGET_GIT_ATTRIBUTES = ".gitattributes";
            public const string FILE_TARGET_GIT_IGNORE = ".gitignore";

            //available options
            public const string COPY_GIT_ATTRIBUTES = "Copy Unity.gitattributes";
            public const string COPY_GIT_IGNORE = "Copy Unity .gitignore";
            public const string CREATE_DEFAULT_FOLDERS = "Create Default Folders";
            public const string CREATE_MKLINK_EDITOR = "Create MKLink to Shared/Editor";
            public const string CREATE_MKLINK_SCRIPTS = "Create MKLink to Shared/Scripts";
        }

        DirectoryInfo SourceSharedDirectory;
        DirectoryInfo TargetDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(IsRunAsAdmin())
            {
                linkLabelAdministratorCheck.Text = "- Running as Administrator";
            }
            else
            {
                linkLabelAdministratorCheck.Text = "- Not Running as Administrator";
            }
         

            txtSharedProjectRoot.Text = Path.GetDirectoryName( Application.ExecutablePath );
            folderBrowserDialog.SelectedPath = txtSharedProjectRoot.Text;
            SourceSharedDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);

            checkedListBoxItems.Items.Add(SettingAttributes.COPY_GIT_ATTRIBUTES, true);
            checkedListBoxItems.Items.Add(SettingAttributes.COPY_GIT_IGNORE, true);
            checkedListBoxItems.Items.Add(SettingAttributes.CREATE_DEFAULT_FOLDERS, true);
            checkedListBoxItems.Items.Add(SettingAttributes.CREATE_MKLINK_EDITOR, true);
            checkedListBoxItems.Items.Add(SettingAttributes.CREATE_MKLINK_SCRIPTS, true);

        }

        private void buttonSharedProject_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                SourceSharedDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);

                txtSharedProjectRoot.Text = SourceSharedDirectory.FullName;
            }
        }

        private void btnTargetPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                TargetDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                txtTargetProjectRoot.Text = TargetDirectory.FullName;

                if( String.IsNullOrEmpty(txtTargetProjectRoot.Text))
                {
                    groupBoxProjectOptions.Enabled = false;
                }
                else
                {
                    groupBoxProjectOptions.Enabled = true;
                }
            }
        }

        private void AddLogMessageLine(string message)
        {
            txtResult.Text += message + "\r\n";
        }


        private void CopyFile(string source_file, string destination_file)
        {
            string full_source_file = Path.Combine(SourceSharedDirectory.FullName, source_file);
            string full_destination_file = Path.Combine(TargetDirectory.FullName, destination_file);

            AddLogMessageLine($" COPY: {full_source_file} -> {full_destination_file}");

            if( File.Exists( full_destination_file ))
            {
                AddLogMessageLine($"Exists, Delete {full_destination_file}");
                File.Delete(full_destination_file);
            }

            File.Copy(full_source_file, full_destination_file);

            AddLogMessageLine($"DONE.");
        }

        private void CreateDefaultFolder(string folder_name)
        {
            string full_path = Path.Combine(TargetDirectory.FullName, folder_name);

            if( Directory.Exists( full_path ))
            {
                AddLogMessageLine($" -> {full_path} EXISTS, SKIPPED!");
            }
            else
            {
                Directory.CreateDirectory(full_path);
                AddLogMessageLine($" -> {full_path} CREATED");
            }
        }

        private void CreateDefaultFolders()
        {
            //Animation
            CreateDefaultFolder(Path.Combine("Assets", "Animation"));
            CreateDefaultFolder(Path.Combine("Assets", "Animation", "Characters"));
            CreateDefaultFolder(Path.Combine("Assets", "Animation", "Environment"));
            CreateDefaultFolder(Path.Combine("Assets", "Animation", "Weapons"));

            //Audio
            CreateDefaultFolder(Path.Combine("Assets", "Audio"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "Ambient"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "Characters"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "HUD"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "Level"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "Menu"));
            CreateDefaultFolder(Path.Combine("Assets", "Audio", "Weapons"));

            //Editor
            CreateDefaultFolder(Path.Combine("Assets", "Editor"));

            //Effects
            CreateDefaultFolder(Path.Combine("Assets", "Effects"));

            //Fonts
            CreateDefaultFolder(Path.Combine("Assets", "Fonts"));

            //Icons
            CreateDefaultFolder(Path.Combine("Assets", "Icons"));

            //Materials
            CreateDefaultFolder(Path.Combine("Assets", "Materials"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Effects"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Environment"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Particles"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Physics"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Shared"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Sprites"));
            CreateDefaultFolder(Path.Combine("Assets", "Materials", "Weapons"));

            //Models
            CreateDefaultFolder(Path.Combine("Assets", "Models"));
            CreateDefaultFolder(Path.Combine("Assets", "Models", "Characters"));
            CreateDefaultFolder(Path.Combine("Assets", "Models", "Effects"));
            CreateDefaultFolder(Path.Combine("Assets", "Models", "Environment"));
            CreateDefaultFolder(Path.Combine("Assets", "Models", "Gameplay"));
            CreateDefaultFolder(Path.Combine("Assets", "Models", "Weapons"));

            //Physics
            CreateDefaultFolder(Path.Combine("Assets", "Physics"));
            CreateDefaultFolder(Path.Combine("Assets", "Physics", "Materials"));

            //Plugins
            CreateDefaultFolder(Path.Combine("Assets", "Plugins"));

            //Prefabs
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Cameras"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Characters"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Environment"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Gameplay"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Particles"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Projectiles"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "Scenes"));
            CreateDefaultFolder(Path.Combine("Assets", "Prefabs", "UI"));

            //Prefabs
            CreateDefaultFolder(Path.Combine("Assets", "Resources"));

            //Scenes
            CreateDefaultFolder(Path.Combine("Assets", "Scenes"));

            //Scripts
            CreateDefaultFolder(Path.Combine("Assets", "Scripts"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Audio"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Build"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Console"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Core"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "ECS"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Game"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Networking"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Render"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "Utils"));

            //Shaders
            CreateDefaultFolder(Path.Combine("Assets", "Shaders"));

            //Sprites
            CreateDefaultFolder(Path.Combine("Assets", "Sprites"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "HUD"));
            CreateDefaultFolder(Path.Combine("Assets", "Scripts", "UI"));

            //Textures
            CreateDefaultFolder(Path.Combine("Assets", "Textures"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Character"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Constants"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Effects"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Environment"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Gameplay"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Lighting"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Particles"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Shared"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Skyboxes"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Utilities"));
            CreateDefaultFolder(Path.Combine("Assets", "Textures", "Weapons"));
        }

        private void CreateSymbolicLink(string link_from, string link_to)
        {
            string link_from_path = Path.Combine(TargetDirectory.FullName, link_from);
            string link_to_path = Path.Combine(SourceSharedDirectory.FullName, link_to);

            if (Directory.Exists(link_from_path))
            {
                AddLogMessageLine($" MKLINK {link_from_path}  <<===>> {link_to_path}, EXISTS, SKIPPED! ");
            }
            else
            {
                AddLogMessageLine($" MKLINK {link_from_path}  <<===>> {link_to_path}, DONE ");

                SymbolicLinkCreator.Create(
                    link_from_path,
                    link_to_path,
                    SymbolicLinkCreator.SymbolicLink.Directory
              );

            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";

            foreach(string item in checkedListBoxItems.Items)
            {
                try
                {
                    AddLogMessageLine($"Doing: {item}");

                    switch (item)
                    {
                        case SettingAttributes.COPY_GIT_ATTRIBUTES:
                            CopyFile(SettingAttributes.FILE_SOURCE_GIT_ATTRIBUTES, SettingAttributes.FILE_TARGET_GIT_ATTRIBUTES);
                            break;

                        case SettingAttributes.COPY_GIT_IGNORE:
                            CopyFile(SettingAttributes.FILE_SOURCE_GIT_IGNORE, SettingAttributes.FILE_TARGET_GIT_IGNORE);
                            break;

                        case SettingAttributes.CREATE_DEFAULT_FOLDERS:
                            CreateDefaultFolders();
                            break;

                        case SettingAttributes.CREATE_MKLINK_EDITOR:

                            CreateSymbolicLink(
                                Path.Combine("Assets", "Editor", "Shared"),
                                Path.Combine("Assets", "Editor"));
                            break;

                        case SettingAttributes.CREATE_MKLINK_SCRIPTS:

                            CreateSymbolicLink(
                                Path.Combine("Assets", "Plugins", "Shared"),
                                Path.Combine("Assets", "Scripts"));
                            break;

                        default:
                            throw new Exception($"Unknown item: {item}");
                    }
                }
                catch(Exception ex)
                {
                    AddLogMessageLine($"Failed: {ex.ToString()}, process exited!");
                    return;
                }

            }


            AddLogMessageLine("Finished !");
        }
    }
}
