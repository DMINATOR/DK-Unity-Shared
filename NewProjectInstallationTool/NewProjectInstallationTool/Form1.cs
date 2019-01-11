using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewProjectInstallationTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSharedProjectRoot.Text = Path.GetDirectoryName( Application.ExecutablePath );

            for(int i = 0; i < checkedListBoxItems.Items.Count; i++ )
            {
                checkedListBoxItems.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void buttonSharedProject_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtSharedProjectRoot.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnTargetPath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtTargetProjectRoot.Text = folderBrowserDialog.SelectedPath;

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
    }
}
