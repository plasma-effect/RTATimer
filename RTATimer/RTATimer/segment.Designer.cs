namespace RTATimer
{
    partial class Segment
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.targetLabel1 = new System.Windows.Forms.Label();
            this.targetLabel2 = new System.Windows.Forms.Label();
            this.nameLable = new System.Windows.Forms.Label();
            this.timeLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // targetLabel1
            // 
            this.targetLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLabel1.AutoSize = true;
            this.targetLabel1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.targetLabel1.Location = new System.Drawing.Point(293, 26);
            this.targetLabel1.Name = "targetLabel1";
            this.targetLabel1.Size = new System.Drawing.Size(104, 24);
            this.targetLabel1.TabIndex = 0;
            this.targetLabel1.Text = "+00:00.00";
            // 
            // targetLabel2
            // 
            this.targetLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.targetLabel2.AutoSize = true;
            this.targetLabel2.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.targetLabel2.Location = new System.Drawing.Point(183, 26);
            this.targetLabel2.Name = "targetLabel2";
            this.targetLabel2.Size = new System.Drawing.Size(104, 24);
            this.targetLabel2.TabIndex = 1;
            this.targetLabel2.Text = "+00:00.00";
            // 
            // nameLable
            // 
            this.nameLable.AutoSize = true;
            this.nameLable.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.nameLable.Location = new System.Drawing.Point(3, 0);
            this.nameLable.Name = "nameLable";
            this.nameLable.Size = new System.Drawing.Size(154, 24);
            this.nameLable.TabIndex = 2;
            this.nameLable.Text = "segment name";
            // 
            // timeLable
            // 
            this.timeLable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeLable.AutoSize = true;
            this.timeLable.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.timeLable.Location = new System.Drawing.Point(3, 24);
            this.timeLable.Name = "timeLable";
            this.timeLable.Size = new System.Drawing.Size(0, 24);
            this.timeLable.TabIndex = 3;
            // 
            // Segment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeLable);
            this.Controls.Add(this.nameLable);
            this.Controls.Add(this.targetLabel2);
            this.Controls.Add(this.targetLabel1);
            this.Name = "Segment";
            this.Size = new System.Drawing.Size(400, 50);
            this.Load += new System.EventHandler(this.Segment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label targetLabel1;
        private System.Windows.Forms.Label targetLabel2;
        private System.Windows.Forms.Label nameLable;
        private System.Windows.Forms.Label timeLable;
    }
}
