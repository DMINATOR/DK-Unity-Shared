namespace NewProjectInstallationTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxProjectSettings = new System.Windows.Forms.GroupBox();
            this.groupBoxProjectOptions = new System.Windows.Forms.GroupBox();
            this.txtSharedProjectRoot = new System.Windows.Forms.TextBox();
            this.buttonSharedProject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTargetProjectRoot = new System.Windows.Forms.TextBox();
            this.btnTargetPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBoxItems = new System.Windows.Forms.CheckedListBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxProjectSettings.SuspendLayout();
            this.groupBoxProjectOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProjectSettings
            // 
            this.groupBoxProjectSettings.Controls.Add(this.label2);
            this.groupBoxProjectSettings.Controls.Add(this.label1);
            this.groupBoxProjectSettings.Controls.Add(this.btnTargetPath);
            this.groupBoxProjectSettings.Controls.Add(this.buttonSharedProject);
            this.groupBoxProjectSettings.Controls.Add(this.txtTargetProjectRoot);
            this.groupBoxProjectSettings.Controls.Add(this.txtSharedProjectRoot);
            this.groupBoxProjectSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProjectSettings.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProjectSettings.Name = "groupBoxProjectSettings";
            this.groupBoxProjectSettings.Size = new System.Drawing.Size(637, 112);
            this.groupBoxProjectSettings.TabIndex = 0;
            this.groupBoxProjectSettings.TabStop = false;
            this.groupBoxProjectSettings.Text = "Unity Project Options";
            // 
            // groupBoxProjectOptions
            // 
            this.groupBoxProjectOptions.Controls.Add(this.richTextBox1);
            this.groupBoxProjectOptions.Controls.Add(this.btnGo);
            this.groupBoxProjectOptions.Controls.Add(this.checkedListBoxItems);
            this.groupBoxProjectOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProjectOptions.Enabled = false;
            this.groupBoxProjectOptions.Location = new System.Drawing.Point(0, 112);
            this.groupBoxProjectOptions.Name = "groupBoxProjectOptions";
            this.groupBoxProjectOptions.Size = new System.Drawing.Size(637, 341);
            this.groupBoxProjectOptions.TabIndex = 1;
            this.groupBoxProjectOptions.TabStop = false;
            this.groupBoxProjectOptions.Text = "Unity Project Copy Options";
            // 
            // txtSharedProjectRoot
            // 
            this.txtSharedProjectRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSharedProjectRoot.Location = new System.Drawing.Point(6, 38);
            this.txtSharedProjectRoot.Name = "txtSharedProjectRoot";
            this.txtSharedProjectRoot.ReadOnly = true;
            this.txtSharedProjectRoot.Size = new System.Drawing.Size(595, 20);
            this.txtSharedProjectRoot.TabIndex = 0;
            // 
            // buttonSharedProject
            // 
            this.buttonSharedProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSharedProject.Location = new System.Drawing.Point(607, 39);
            this.buttonSharedProject.Name = "buttonSharedProject";
            this.buttonSharedProject.Size = new System.Drawing.Size(24, 19);
            this.buttonSharedProject.TabIndex = 1;
            this.buttonSharedProject.Text = "...";
            this.buttonSharedProject.UseVisualStyleBackColor = true;
            this.buttonSharedProject.Click += new System.EventHandler(this.buttonSharedProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Shared Project Root Path";
            // 
            // txtTargetProjectRoot
            // 
            this.txtTargetProjectRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetProjectRoot.Location = new System.Drawing.Point(6, 83);
            this.txtTargetProjectRoot.Name = "txtTargetProjectRoot";
            this.txtTargetProjectRoot.ReadOnly = true;
            this.txtTargetProjectRoot.Size = new System.Drawing.Size(595, 20);
            this.txtTargetProjectRoot.TabIndex = 0;
            // 
            // btnTargetPath
            // 
            this.btnTargetPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTargetPath.Location = new System.Drawing.Point(607, 84);
            this.btnTargetPath.Name = "btnTargetPath";
            this.btnTargetPath.Size = new System.Drawing.Size(24, 19);
            this.btnTargetPath.TabIndex = 1;
            this.btnTargetPath.Text = "...";
            this.btnTargetPath.UseVisualStyleBackColor = true;
            this.btnTargetPath.Click += new System.EventHandler(this.btnTargetPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Unity Project Path";
            // 
            // checkedListBoxItems
            // 
            this.checkedListBoxItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBoxItems.FormattingEnabled = true;
            this.checkedListBoxItems.Items.AddRange(new object[] {
            "Copy Unity .gitattributes",
            "Copy Unity .gitignore",
            "Create Default Folders",
            "Create MKLink to Shared/Editor",
            "Create MKLink to Shared/Scripts"});
            this.checkedListBoxItems.Location = new System.Drawing.Point(9, 19);
            this.checkedListBoxItems.Name = "checkedListBoxItems";
            this.checkedListBoxItems.Size = new System.Drawing.Size(197, 274);
            this.checkedListBoxItems.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGo.Location = new System.Drawing.Point(9, 306);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(212, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(419, 274);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 453);
            this.Controls.Add(this.groupBoxProjectOptions);
            this.Controls.Add(this.groupBoxProjectSettings);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxProjectSettings.ResumeLayout(false);
            this.groupBoxProjectSettings.PerformLayout();
            this.groupBoxProjectOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProjectSettings;
        private System.Windows.Forms.GroupBox groupBoxProjectOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSharedProject;
        private System.Windows.Forms.TextBox txtSharedProjectRoot;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTargetPath;
        private System.Windows.Forms.TextBox txtTargetProjectRoot;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckedListBox checkedListBoxItems;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

