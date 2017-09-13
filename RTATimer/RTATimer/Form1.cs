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
    public partial class Form1 : Form
    {
        Records records;
        System.Diagnostics.Stopwatch stopwatch;
        List<long?> time;
        Config config;

        public Form1()
        {
            InitializeComponent();
            if (System.IO.File.Exists("config.bin"))
            {
                using(var fs = new System.IO.FileStream("config.bin", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var sel = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    this.config = sel.Deserialize(fs) as Config;
                }
                OpenRecord(this.config.Path);
            }
            else
            {
                this.config = new Config();
                this.records = new Records(new List<string> { "timer stop" });
            }
            this.stopwatch = new System.Diagnostics.Stopwatch();
            this.time = new List<long?>();
        }
        
        private void AddSegment(string name, long? target1,long? target2,long? segbest)
        {
            var s = new Segment(name, target1, target2, segbest, this.config.Colors);
            this.tableLayoutPanel1.Controls.Add(s);
        }

        private void SetFlags(int index, bool flag)
        {
            if(this.tableLayoutPanel1.Controls[index] is Segment seg)
            {
                seg.Flag = flag;
            }
        }

        private void SetTimes(int index, long? time, long? segt)
        {
            if(this.tableLayoutPanel1.Controls[index] is Segment seg)
            {
                seg.SetTime(time, segt);
            }
        }

        private void ResetTimerView()
        {
            if (this.time.Count != 0)
            {
                this.records.AddRecord(this.time);
                this.time = new List<long?>();
            }

            var tlist1 = this.records.GetTarget(this.config.Target1);
            var tlist2 = this.records.GetTarget(this.config.Target2);
            var sobs = this.records.GetTarget("sobs");

            this.tableLayoutPanel1.Controls.Clear();
            for(var i = 0; i < tlist1.Count; ++i)
            {
                AddSegment(this.records.Name(i), tlist1[i], tlist2[i], sobs[i]);
            }
            this.tableLayoutPanel1.ScrollControlIntoView(this.tableLayoutPanel1.Controls[0]);

            this.timerLable.Text = "00:00.00";

            this.Text = System.IO.Path.GetFileNameWithoutExtension(this.config.Path ?? "new file.rtd");
            this.targetLabel1.Text = this.config.Target1;
            this.targetLabel2.Text = this.config.Target2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetTimerView();
        }

        private void FormKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    PressEnter();
                    break;
                case (char)Keys.Escape:
                    PressEscape();
                    break;
            }
        }

        private void PressEscape()
        {
            if (this.stopwatch.IsRunning)
            {
                this.stopwatch.Stop();
                this.timer1.Enabled = false;
                this.statusLabel.Text = "キャンセルしました";
            }
            else
            {
                this.statusLabel.Text = "リセットしました";
            }
            ResetTimerView();
        }

        private void PressEnter()
        {
            if (this.stopwatch.IsRunning)
            {
                var now = this.stopwatch.ElapsedMilliseconds;
                SetTimes(this.time.Count, now, now - (this.time.Count == 0 ? 0 : this.time.Last()));
                SetFlags(this.time.Count, false);
                this.time.Add(now);

                if (this.time.Count == this.records.Size)
                {
                    this.stopwatch.Stop();
                    this.timer1.Enabled = false;
                    this.timerLable.Text = Utility.UnsingedTimeToString(now);
                    this.statusLabel.Text = "ストップしました";
                }
                else
                {
                    SetFlags(this.time.Count, true);
                }
            }
            else
            {
                this.stopwatch.Reset();
                ResetTimerView();
                SetFlags(0, true);
                this.stopwatch.Start();
                this.timer1.Enabled = true;
                this.statusLabel.Text = "スタートしました";
            }
        }

        private void Tick(object sender, EventArgs e)
        {
            var now = this.stopwatch.ElapsedMilliseconds;
            this.timerLable.Text = Utility.UnsingedTimeToString(now);
        }

        private void NewFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            var end = new EditNewData();
            end.ShowDialog(this);
            var list = end.SegmentNames;
            if (list.Count == 0)
            {
                list.Add("timer stop");
            }
            this.records = new Records(list);
            ResetTimerView();
            this.statusLabel.Text = "新規作成しました";
            end.Dispose();
        }

        private void SaveWithNewNameToolStripMenuItemClick(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                FileName = "*.rtd",
                Filter = "rtdファイル(*.rtd)|*.rtd|すべてのファイル|*.*",
                FilterIndex = 1,
                Title = "保存するファイル名を指定してください",
                RestoreDirectory = true
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveRecord(sfd.FileName);
            }
        }

        private void SaveRecord(string filename)
        {
            using (var fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(fs, this.records);
            }
            this.config.Path = filename;
            this.statusLabel.Text = filename + "に保存しました";
        }

        private void OpenToolStripMenuItemClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                FileName = "*.rtd",
                Filter = "rtdファイル(*.rtd)|*.rtd|すべてのファイル|*.*",
                FilterIndex = 1
            };
            if(ofd.ShowDialog()== DialogResult.OK)
            {
                OpenRecord(ofd.FileName);
                this.config.Path = ofd.FileName;
            }
            ResetTimerView();
        }

        private void OpenRecord(string filename)
        {
            using (var fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                this.records = formatter.Deserialize(fs) as Records;
            }
            this.statusLabel.Text = filename + "を開きました";
        }

        private void SaveThisFileToolStripMenuItemClick(object sender, EventArgs e)
        {
            if(this.config.Path is string filename)
            {
                ResetTimerView();
                SaveRecord(filename);
            }
            else
            {
                var sfd = new SaveFileDialog
                {
                    FileName = "*.rtd",
                    Filter = "rtdファイル(*.rtd)|*.rtd|すべてのファイル|*.*",
                    FilterIndex = 1,
                    Title = "保存するファイル名を指定してください",
                    RestoreDirectory = true
                };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.config.Path = sfd.FileName;
                    ResetTimerView();
                    SaveRecord(sfd.FileName);
                }
            }
        }

        private void ExportCSVClick(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                FileName = "*.csv",
                Filter = "csvファイル(*.csv)|*.csv|すべてのファイル|*.*",
                FilterIndex = 1,
                Title = "保存するファイル名を指定してください",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ResetTimerView();
                using (var stream = new System.IO.StreamWriter(sfd.FileName))
                {
                    var last = this.records.LastRecords;
                    var best = this.records.GetTarget(this.config.Target1);
                    var sobs = this.records.GetTarget(this.config.Target2);
                    stream.WriteLine(",latest record,comparison with {0},comparison with {1},,{0},{1}", this.config.Target1, this.config.Target2);
                    for (var i = 0; i < this.records.Size; ++i)
                    {
                        var diff1 = last[i] - best[i];
                        var diff2 = last[i] - sobs[i];
                        string dffs1;
                        if (diff1 is long d1)
                        {
                            if (d1 > 0)
                            {
                                dffs1 = "+" + Utility.StrictTimeToString(d1);
                            }
                            else
                            {
                                dffs1 = "-" + Utility.StrictTimeToString(-d1);
                            }
                        }
                        else
                        {
                            dffs1 = "--:--:--.---";
                        }
                        string dffs2;
                        if (diff2 is long d2)
                        {
                            if (d2 > 0)
                            {
                                dffs2 = "+" + Utility.StrictTimeToString(d2);
                            }
                            else
                            {
                                dffs2 = "-" + Utility.StrictTimeToString(-d2);
                            }
                        }
                        else
                        {
                            dffs2 = "--:--:--.---";
                        }

                        stream.WriteLine("{0},{1},{2},{3},,{4},{5}",
                            this.records.Name(i),
                            Utility.StrictTimeToString(last[i]),
                            dffs1,
                            dffs2,
                            Utility.StrictTimeToString(best[i]),
                            Utility.StrictTimeToString(sobs[i]));
                    }
                }
            }
        }

        private void TargetAddClick(object sender, EventArgs e)
        {
            var window = new DataEditor
            {
                Records = this.records,
                StatusLable = this.statusLabel
            };
            window.ShowDialog(this);
            window.Dispose();
        }

        private void SelectTargetClick(object sender, EventArgs e)
        {
            var window = new TargetSetting()
            {
                Records = this.records,
                Target1 = this.config.Target1,
                Target2 = this.config.Target2,
            };
            window.ShowDialog(this);
            if (window.Complete)
            {
                this.config.Target1 = window.Target1;
                this.config.Target2 = window.Target2;
                this.statusLabel.Text = "ターゲットを変更しました";
            }
            ResetTimerView();
            window.Dispose();
        }

        private void InitialDataClick(object sender, EventArgs e)
        {
            if(this.config.Path is string path)
            {
                using(var fs = new System.IO.FileStream("config.bin", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    var s = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    s.Serialize(fs, this.config);
                }
            }
            else
            {
                MessageBox.Show("現在のレコードのファイルが存在する必要があります", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    [Serializable()]
    public class Config
    {
        string path;
        string target1;
        string target2;
        TimerColors colors;

        public string Path { get => this.path; set => this.path = value; }
        public string Target1 { get => this.target1; set => this.target1 = value; }
        public string Target2 { get => this.target2; set => this.target2 = value; }
        public TimerColors Colors { get => this.colors; set => this.colors = value; }

        public Config()
        {
            this.path = null;
            this.target1 = "best";
            this.target2 = "sobs";
            this.colors = new TimerColors()
            {
                DefaultColor = SystemColors.ControlText,
                DefaultColorBack = SystemColors.Control,
                NormalPlus = Color.White,
                NormalPlusBack = Color.Red,
                NormalMinus = Color.White,
                NormalMinusBack = Color.Green,
                SegbestMinus = Color.Black,
                SegbestMinusBack = Color.Yellow,
                CurrentSegment = Color.White,
                CurrentSegmentBack = Color.Green,
                NotCurrentSegment = SystemColors.ControlText,
                NotCurrentSegmentBack = SystemColors.Control
            };
        }
    }

}
