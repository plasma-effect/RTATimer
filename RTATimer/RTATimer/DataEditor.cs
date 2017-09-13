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
    public partial class DataEditor : Form
    {
        private Records records;
        private List<long?> values;
        private ToolStripStatusLabel statusLable;
        
        public DataEditor()
        {
            InitializeComponent();
        }
        
        public List<long?> Values => this.values;

        public Records Records { get => this.records; set => this.records = value; }
        public ToolStripStatusLabel StatusLable { get => this.statusLable; set => this.statusLable = value; }

        private void DataEditorLoad(object sender, EventArgs e)
        {
            for (var i = 0; i < this.records.Size; ++i)
            {
                var row = new DataGridViewRow();
                row.HeaderCell.Value = this.records.Name(i);
                this.dataGridView1.Rows.Add(row);
            }
            this.values = Enumerable.Repeat<long?>(null, this.records.Size).ToList();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void ComplateClick(object sender, EventArgs e)
        {
            var s = this.nameBox.Text;
            if (s == "")
            {
                MessageBox.Show("名前を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (s == "sobs" || s == "best") 
            {
                MessageBox.Show("その名前では登録できません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (this.records.ContainsTarget(s))
            {
                if (MessageBox.Show("すでにその名前で登録されています。上書きしますか？", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.records.AddTarget(s, this.values);
                    this.statusLable.Text = "ターゲット" + s + "を上書きしました";
                    this.Close();
                }
            }
            else
            {
                this.records.AddTarget(s, this.values);
                this.statusLable.Text = "ターゲット" + s + "を作成しました";
                this.Close();
            }
        }

        private void CellFormattingAction(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.Value is string str)
            {
                var val = Utility.StringToTime(str);
                this.values[e.RowIndex] = val;
                e.Value = Utility.StrictTimeToString(val);
            }
            else
            {
                this.values[e.RowIndex] = null;
                e.Value = "";
            }
        }
    }
}
