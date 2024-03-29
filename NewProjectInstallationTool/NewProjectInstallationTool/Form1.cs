﻿using System;
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
            public const string FILE_SOURCE_DEFAULT_GENERATED_SETTINGS = "NewProjectFiles/template.SettingsConstants.cs";
            public const string FILE_SOURCE_DEFAULT_BASE_SETTINGS = "NewProjectFiles/template.SettingValueData.json";
            public const string FILE_SOURCE_DEFAULT_INPUT = "NewProjectFiles/template.InputConstants.cs";

            //target
            public const string FILE_TARGET_GIT_ATTRIBUTES = ".gitattributes";
            public const string FILE_TARGET_GIT_IGNORE = ".gitignore";
            public const string FILE_TARGET_DEFAULT_GENERATED_SETTINGS = "Assets/Scripts/Generated/SettingsConstants.cs";
            public const string FILE_TARGET_DEFAULT_BASE_SETTINGS = "Assets/StreamingAssets/SettingValueData.json";
            public const string FILE_TARGET_DEFAULT_INPUT = "Assets/Scripts/Generated/InputConstants.cs";

            //available options
            public const string COPY_GIT_ATTRIBUTES = "Copy Unity.gitattributes";
            public const string COPY_GIT_IGNORE = "Copy Unity .gitignore";
            public const string COPY_DEFAULT_SETTINGS = "Copy Default settings";
            public const string COPY_DEFAULT_INPUTS = "Copy Default input settings";
            public const string CREATE_DEFAULT_FOLDERS = "Create Default Folders";
            public const string CREATE_MKLINK_EDITOR = "Create MKLink to Shared/Editor";
            public const string CREATE_MKLINK_SCRIPTS_PLUGINS = "Create MKLink to Shared/Scripts/Plugins";
            public const string CREATE_MKLINK_SCRIPTS_SHARED = "Create MKLink to Shared/Scripts/Scripts";
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
        }


        private void AddCheckedItems()
        {
            if (checkedListBoxItems.Items.Count == 0)
            {
                checkedListBoxItems.Items.Add(SettingAttributes.COPY_GIT_ATTRIBUTES, true);
                checkedListBoxItems.Items.Add(SettingAttributes.COPY_GIT_IGNORE, true);

                //if files exist already - (when project has matured) have these set to false
                bool exist = File.Exists(Path.Combine(TargetDirectory.FullName, SettingAttributes.FILE_TARGET_DEFAULT_GENERATED_SETTINGS));
                checkedListBoxItems.Items.Add(SettingAttributes.COPY_DEFAULT_SETTINGS, !exist);

                exist = File.Exists(Path.Combine(TargetDirectory.FullName, SettingAttributes.FILE_TARGET_DEFAULT_INPUT));
                checkedListBoxItems.Items.Add(SettingAttributes.COPY_DEFAULT_INPUTS, !exist);


                checkedListBoxItems.Items.Add(SettingAttributes.CREATE_DEFAULT_FOLDERS, true);
                checkedListBoxItems.Items.Add(SettingAttributes.CREATE_MKLINK_EDITOR, true);
                checkedListBoxItems.Items.Add(SettingAttributes.CREATE_MKLINK_SCRIPTS_PLUGINS, true);
                checkedListBoxItems.Items.Add(SettingAttributes.CREATE_MKLINK_SCRIPTS_SHARED, true);
            }
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
                    AddCheckedItems();
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

            var dir = new FileInfo(full_destination_file).Directory;

            if( !dir.Exists )
            {
                AddLogMessageLine($"Creating new dir {dir.FullName}");
                dir.Create();
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

            foreach(string item in checkedListBoxItems.CheckedItems)
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

                        case SettingAttributes.COPY_DEFAULT_INPUTS:
                            CopyFile(SettingAttributes.FILE_SOURCE_DEFAULT_INPUT, SettingAttributes.FILE_TARGET_DEFAULT_INPUT);
                            break;

                        case SettingAttributes.COPY_DEFAULT_SETTINGS:
                            CopyFile(SettingAttributes.FILE_SOURCE_DEFAULT_GENERATED_SETTINGS, SettingAttributes.FILE_TARGET_DEFAULT_GENERATED_SETTINGS);
                            CopyFile(SettingAttributes.FILE_SOURCE_DEFAULT_BASE_SETTINGS, SettingAttributes.FILE_TARGET_DEFAULT_BASE_SETTINGS);
                            break;

                        case SettingAttributes.CREATE_DEFAULT_FOLDERS:
                            CreateDefaultFolders();
                            break;

                        case SettingAttributes.CREATE_MKLINK_EDITOR:

                            CreateSymbolicLink(
                                Path.Combine("Assets", "Editor", "Shared"),
                                Path.Combine("Assets", "Editor"));
                            break;

                        case SettingAttributes.CREATE_MKLINK_SCRIPTS_PLUGINS:

                            CreateSymbolicLink(
                                Path.Combine("Assets", "Plugins", "Shared"),
                                Path.Combine("Assets", "Scripts", "Plugins"));
                            break;

                        case SettingAttributes.CREATE_MKLINK_SCRIPTS_SHARED:

                            CreateSymbolicLink(
                                Path.Combine("Assets", "Scripts", "Shared"),
                                Path.Combine("Assets", "Scripts", "Scripts"));
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
