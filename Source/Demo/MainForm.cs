using SevenZip;
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
using WinDebExtraction;

namespace Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    WinDebExtractor extractor = new WinDebExtractor(Path.GetDirectoryName(openFileDialog.FileName), Path.GetFileName(openFileDialog.FileName), @"C:\Program Files\7-Zip\7z.dll", "tar.gz", "tar.xz");
                    extractor.Extract(folderBrowserDialog.SelectedPath);
                }
            }
        }

    }
}
