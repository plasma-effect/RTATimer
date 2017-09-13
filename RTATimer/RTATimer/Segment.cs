using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTATimer
{
    public partial class Segment : UserControl
    {
        long? target1;
        long? target2;
        long? segment_best;
        long? time;
        bool flag;
        TimerColors colors;
        
        public Segment()
        {
            InitializeComponent();
        }
        public Segment(string name, long? target1, long? target2, long? segment_best, TimerColors colors) : this()
        {
            this.nameLable.Text = name;
            this.target1 = target1;
            this.target2 = target2;
            this.segment_best = segment_best;
            this.flag = false;
            this.colors = colors;
        }
        public void Initialize()
        {
            this.colors.SetDefaultColor(this.targetLabel1);
            this.colors.SetDefaultColor(this.targetLabel2);
            this.targetLabel1.Text = Utility.UnsingedTimeToString(this.target1);
            this.targetLabel2.Text = Utility.UnsingedTimeToString(this.target2);
        }
        public void SetTime(long? t, long? s)
        {
            this.time = t;
            this.timeLable.Text = Utility.UnsingedTimeToString(t);
            if(t is long time)
            {
                var diff1 = t - this.target1;
                var diff2 = t - this.target2;
                this.targetLabel1.Text = Utility.DiffTimeToString(diff1);
                this.targetLabel2.Text = Utility.DiffTimeToString(diff2);
                if (diff1 is long a)
                {
                    if (a > 0) 
                    {
                        this.colors.SetNormalPlus(this.targetLabel1);
                    }
                    else
                    {
                        this.colors.SetNormalMinus(this.targetLabel1);
                    }
                }
                if (diff2 is long b)
                {
                    if (b > 0)
                    {
                        this.colors.SetNormalPlus(this.targetLabel2);
                    }
                    else
                    {
                        this.colors.SetNormalMinus(this.targetLabel2);
                    }
                }
                if (s < this.segment_best) 
                {
                    this.colors.NewSegBest(this.timeLable);
                }
            }
        }
        public void Reset()
        {
            this.timeLable.Text = "--:--:--";
            this.targetLabel1.Text = Utility.UnsingedTimeToString(this.target1);
            this.targetLabel2.Text = Utility.UnsingedTimeToString(this.target2);
            this.colors.SetDefaultColor(this.targetLabel1);
            this.colors.SetDefaultColor(this.targetLabel2);
            this.time = null;
        }
        
        public bool Flag
        {
            set
            {
                this.flag = value;
                if (value)
                {
                    this.colors.SetCurrentSegment(this.nameLable);
                }
                else
                {
                    this.colors.SetNotCurrentSegment(this.nameLable);
                }
            }
            get
            {
                return this.flag;
            }
        }

        private void Segment_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
