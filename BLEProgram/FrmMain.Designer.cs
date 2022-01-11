namespace BLEProgram
{
    partial class FrmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ExitToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.requestList = new System.Windows.Forms.ListBox();
            this.DataText = new System.Windows.Forms.TextBox();
            this.StartBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ExitToolStrip
            // 
            this.ExitToolStrip.Name = "ExitToolStrip";
            this.ExitToolStrip.Size = new System.Drawing.Size(43, 20);
            this.ExitToolStrip.Text = "종료";
            this.ExitToolStrip.Click += new System.EventHandler(this.ExitToolStrip_Click);
            // 
            // requestList
            // 
            this.requestList.FormattingEnabled = true;
            this.requestList.ItemHeight = 12;
            this.requestList.Location = new System.Drawing.Point(13, 28);
            this.requestList.Name = "requestList";
            this.requestList.Size = new System.Drawing.Size(775, 376);
            this.requestList.TabIndex = 1;
            // 
            // DataText
            // 
            this.DataText.Location = new System.Drawing.Point(193, 410);
            this.DataText.Name = "DataText";
            this.DataText.Size = new System.Drawing.Size(496, 21);
            this.DataText.TabIndex = 2;
            this.DataText.Text = "test";
            this.DataText.TextChanged += new System.EventHandler(this.DataText_TextChanged);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(12, 408);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(175, 23);
            this.StartBtn.TabIndex = 3;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(695, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "전송";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 438);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.DataText);
            this.Controls.Add(this.requestList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "MainFrm";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStrip;
        private System.Windows.Forms.ListBox requestList;
        private System.Windows.Forms.TextBox DataText;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button button1;
    }
}

