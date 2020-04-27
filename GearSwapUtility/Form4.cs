using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GearSwapUtility
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            textBox1.Text = Properties.Settings.Default.WindowerDirectory;
            textBox2.Text = Properties.Settings.Default.IconsDirectory;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = Properties.Settings.Default.WindowerDirectory;
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath != "" && System.IO.Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    Properties.Settings.Default.WindowerDirectory = folderBrowserDialog1.SelectedPath;
                    Properties.Settings.Default.Save();
                    textBox1.Text = Properties.Settings.Default.WindowerDirectory;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = Properties.Settings.Default.IconsDirectory;
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath != "" && System.IO.Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    Properties.Settings.Default.IconsDirectory = folderBrowserDialog1.SelectedPath;
                    Properties.Settings.Default.Save();
                    textBox2.Text = Properties.Settings.Default.IconsDirectory;
                }
            }
        }
    }
}
