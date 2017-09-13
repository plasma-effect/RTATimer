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
    public partial class EditNewData : Form
    {
        public EditNewData()
        {
            InitializeComponent();
        }

        public List<string> SegmentNames
        {
            get
            {
                return
                    (from i in Enumerable.Range(0, this.dataGridView1.Rows.Count - 1)
                     select this.dataGridView1.Rows[i].Cells[0].Value.ToString()).ToList();
                    
            }
        }


        private void CompleteClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
