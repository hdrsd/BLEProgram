namespace BLEProgram
{
    partial class Form1
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
            this.exitToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.requestList = new System.Windows.Forms.ListBox();
            this.dataText = new System.Windows.Forms.TextBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStrip
            // 
            this.exitToolStrip.Name = "exitToolStrip";
            this.exitToolStrip.Size = new System.Drawing.Size(43, 20);
            this.exitToolStrip.Text = "종료";
            this.exitToolStrip.Click += new System.EventHandler(this.exitToolStrip_Click);
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
            // dataText
            // 
            this.dataText.Location = new System.Drawing.Point(14, 413);
            this.dataText.Name = "dataText";
            this.dataText.Size = new System.Drawing.Size(672, 21);
            this.dataText.TabIndex = 2;
            this.dataText.Text = "test";
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(692, 411);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(96, 23);
            this.startBtn.TabIndex = 3;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 438);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.dataText);
            this.Controls.Add(this.requestList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "MainFrm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStrip;
        private System.Windows.Forms.ListBox requestList;
        private System.Windows.Forms.TextBox dataText;
        private System.Windows.Forms.Button startBtn;
    }
}

