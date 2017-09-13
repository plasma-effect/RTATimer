using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTATimer
{
    public partial class TargetSetting : Form
    {
        private Records records;
        private bool complete;

        public TargetSetting()
        {
            InitializeComponent();
        }

        public string Target1 { get => this.targetBox1.Text; set => this.targetBox1.Text = value; }
        public string Target2 { get => this.targetBox2.Text; set => this.targetBox2.Text = value; }
        public bool Complete => this.complete;

        public Records Records { get => this.records; set => this.records = value; }

        private void TargetSettingLoad(object sender, EventArgs e)
        {
            foreach(var p in this.records.TargetRecords)
            {
                this.dataGridView1.Columns.Add(p.Key, p.Key);
                this.dataGridView1.Columns[p.Key].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            foreach (var (name, index) in this.records.Names.AddIndex()) 
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].HeaderCell.Value = name;
                foreach(var p in this.records.TargetRecords)
                {
                    this.dataGridView1[p.Key, index].Value = Utility.StrictTimeToString(p.Value[index]);
                }
            }
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ReadOnly = true;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (this.records.ContainsTarget(this.Target1) && this.records.ContainsTarget(this.Target2))
            {
                this.complete = true;
                this.Close();
                return;
            }
            MessageBox.Show("存在するターゲットを選択してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
