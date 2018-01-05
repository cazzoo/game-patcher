using Crc32C;
using Newtonsoft.Json;
using RFUpdater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace RFUpdater.PatchEditor
{
    public partial class ListerForm : Form
    {
        private string[] Files;

        public ListerForm()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            StartBrowsing();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveList();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RemoveFromPath(filePath.Text);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Files = GetFiles(e.Argument);

            for (int i = 0; i < Files.Length; i++)
            {
                backgroundWorker.ReportProgress(i + 1, GetFileData(Files[i]));
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdateResult(e.UserState);

            UpdateProgressBar(ComputeProgress(e.ProgressPercentage));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableButtons();
        }

        private void DisableButtons()
        {
            Progress.Value = 0;
            Result.Clear();
            saveButton.Enabled = false;
            removeButton.Enabled = false;
        }

        private void EnableButtons()
        {
            saveButton.Enabled = true;
            removeButton.Enabled = true;
        }

        public string[] GetFiles(object Path)
        {
            return Directory.GetFiles(Path.ToString(), "*.*", SearchOption.AllDirectories);
        }

        public int GetFilesCount(string[] Files)
        {
            return Files.Length;
        }

        public string GetFileData(string File)
        {
            FileInfo fileInfo = new FileInfo(File);

            return File + " " + GetHash(File) + " " + fileInfo.Length;
        }

        private string GetHash(string Name)
        {
            if (Name == string.Empty)
                return null;

            try
            {
                return Crc32CAlgorithm.Compute(File.ReadAllBytes(Name)).ToString("x2");
            }
            catch
            {
                MessageBox.Show(Name + " cannot be opened.");
                return null;
            }
        }

        private void UpdateResult(object Data)
        {
            if (!Result.IsDisposed)
            {
                Result.AppendText(Data.ToString().Replace(@"\", "/") + Environment.NewLine);
            }
        }

        private int ComputeProgress(int Percent)
        {
            return (100 * Percent) / Files.Length;
        }

        private void UpdateProgressBar(int Percent)
        {
            if (Percent < 0 || Percent > 100)
                return;

            if (!Progress.IsDisposed)
            {
                Progress.Value = Percent;
            }
        }

        private void RemoveFromPath(string Remove)
        {
            if (Remove == string.Empty)
                return;

            Result.Text = Result.Text.Replace(Remove, string.Empty);
            filePath.Text = filePath.Text.Replace(Remove, string.Empty);
        }

        private void StartBrowsing()
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DisableButtons();
                var selectedPath = folderBrowserDialog.SelectedPath;
                DirectoryInfo selectedDirectory = new DirectoryInfo(selectedPath);
                var parentDirectoryPath = selectedDirectory.Parent.FullName + @"\";
                lbl_selectedPath.Text = selectedPath.Replace(@"\", "/");
                filePath.Text = parentDirectoryPath.Replace(@"\", "/");

                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void SaveList()
        {
            saveFileDialog.FileName = "patchlist.txt";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                {
                    streamWriter.Write(Result.Text);
                }

                List<ModFile> modFiles = new List<ModFile>();
                foreach (var line in Result.Text.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] lineElements = line.Split(' ');
                    string path = Path.GetDirectoryName(lineElements[0]);
                    string filename = Path.GetFileName(lineElements[0]);
                    ModFile file = new ModFile()
                    {
                        Deletable = false,
                        FilePath = path,
                        FileName = filename,
                        FileHash = UInt32.Parse(lineElements[1], System.Globalization.NumberStyles.HexNumber),
                        FileSize = UInt32.Parse(lineElements[2]),
                    };
                    modFiles.Add(file);
                }

                string modName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                Mod mod = new Mod()
                {
                    Name = modName,
                    Files = modFiles,
                    FolderName = modName
                };

                string output = JsonConvert.SerializeObject(mod, Formatting.Indented);

                string jsonFile = Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), string.Format("{0}.json", modName));
                using (StreamWriter streamWriter = new StreamWriter(jsonFile))
                {
                    streamWriter.Write(output);
                }
            }
        }

        private void OpenList_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "patchlist.txt";
            openFileDialog.Filter = "Text file (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
                {
                    Result.Text += streamReader.ReadToEnd();
                }
                EnableButtons();
            }
            else
            {
                DisableButtons();
            }
        }
    }
}