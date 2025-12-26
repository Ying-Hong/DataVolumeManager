using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DataVolumeManager
{
    public partial class MainForm : Form
    {
        private List<FolderRule> rules = new List<FolderRule>();
        private Timer timer = new Timer();

        public MainForm()
        {
            InitializeComponent();
            LoadConfig();
            BindGrid();

            timer.Interval = 10 * 60 * 1000;
            timer.Tick += (s, e) => CleanerEngine.Run(rules);
            timer.Start();
        }



        private void LoadConfig()
        {
            if (File.Exists("config.json"))
                rules = JsonConvert.DeserializeObject<List<FolderRule>>(File.ReadAllText("config.json"));
        }

        private void SaveConfig()
        {
            File.WriteAllText("config.json", JsonConvert.SerializeObject(rules, Formatting.Indented));
        }

        private void BindGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = rules;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            rules.Add(new FolderRule { Enabled = true, KeepDays = 30, Recursive = true });
            BindGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            MessageBox.Show("Saved");
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            CleanerEngine.Run(rules);
            MessageBox.Show("Run completed");
        }
    }
}
