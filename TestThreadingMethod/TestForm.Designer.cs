namespace TestThreadingMethod
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNormal = new System.Windows.Forms.Button();
            this.lstThreadGunResult = new System.Windows.Forms.ListBox();
            this.lblInfoThreadGun = new System.Windows.Forms.Label();
            this.btnThreadGun = new System.Windows.Forms.Button();
            this.btnThread = new System.Windows.Forms.Button();
            this.btnThreadPool = new System.Windows.Forms.Button();
            this.lblInfoThreadPool = new System.Windows.Forms.Label();
            this.lstThreadPoolResult = new System.Windows.Forms.ListBox();
            this.lstThread = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblInfoThread = new System.Windows.Forms.Label();
            this.lblActiveThreadCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnNormal
            // 
            this.btnNormal.Location = new System.Drawing.Point(542, 12);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(100, 25);
            this.btnNormal.TabIndex = 0;
            this.btnNormal.Text = "Normal Mode";
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // lstThreadGunResult
            // 
            this.lstThreadGunResult.FormattingEnabled = true;
            this.lstThreadGunResult.Location = new System.Drawing.Point(12, 43);
            this.lstThreadGunResult.Name = "lstThreadGunResult";
            this.lstThreadGunResult.Size = new System.Drawing.Size(206, 212);
            this.lstThreadGunResult.TabIndex = 1;
            // 
            // lblInfoThreadGun
            // 
            this.lblInfoThreadGun.AutoSize = true;
            this.lblInfoThreadGun.Location = new System.Drawing.Point(13, 280);
            this.lblInfoThreadGun.Name = "lblInfoThreadGun";
            this.lblInfoThreadGun.Size = new System.Drawing.Size(73, 13);
            this.lblInfoThreadGun.TabIndex = 2;
            this.lblInfoThreadGun.Text = "Item Count : 0";
            // 
            // btnThreadGun
            // 
            this.btnThreadGun.Location = new System.Drawing.Point(12, 12);
            this.btnThreadGun.Name = "btnThreadGun";
            this.btnThreadGun.Size = new System.Drawing.Size(206, 25);
            this.btnThreadGun.TabIndex = 3;
            this.btnThreadGun.Text = "ThreadGun Mode";
            this.btnThreadGun.UseVisualStyleBackColor = true;
            this.btnThreadGun.Click += new System.EventHandler(this.btnThreadGun_Click);
            // 
            // btnThread
            // 
            this.btnThread.Location = new System.Drawing.Point(436, 12);
            this.btnThread.Name = "btnThread";
            this.btnThread.Size = new System.Drawing.Size(100, 25);
            this.btnThread.TabIndex = 4;
            this.btnThread.Text = "Thread Mode";
            this.btnThread.UseVisualStyleBackColor = true;
            this.btnThread.Click += new System.EventHandler(this.btnThread_Click);
            // 
            // btnThreadPool
            // 
            this.btnThreadPool.Location = new System.Drawing.Point(224, 12);
            this.btnThreadPool.Name = "btnThreadPool";
            this.btnThreadPool.Size = new System.Drawing.Size(206, 25);
            this.btnThreadPool.TabIndex = 5;
            this.btnThreadPool.Text = "ThreadPool Mode";
            this.btnThreadPool.UseVisualStyleBackColor = true;
            this.btnThreadPool.Click += new System.EventHandler(this.btnThreadPool_Click);
            // 
            // lblInfoThreadPool
            // 
            this.lblInfoThreadPool.AutoSize = true;
            this.lblInfoThreadPool.Location = new System.Drawing.Point(221, 280);
            this.lblInfoThreadPool.Name = "lblInfoThreadPool";
            this.lblInfoThreadPool.Size = new System.Drawing.Size(73, 13);
            this.lblInfoThreadPool.TabIndex = 7;
            this.lblInfoThreadPool.Text = "Item Count : 0";
            // 
            // lstThreadPoolResult
            // 
            this.lstThreadPoolResult.FormattingEnabled = true;
            this.lstThreadPoolResult.Location = new System.Drawing.Point(224, 43);
            this.lstThreadPoolResult.Name = "lstThreadPoolResult";
            this.lstThreadPoolResult.Size = new System.Drawing.Size(206, 225);
            this.lstThreadPoolResult.TabIndex = 8;
            // 
            // lstThread
            // 
            this.lstThread.FormattingEnabled = true;
            this.lstThread.Location = new System.Drawing.Point(436, 43);
            this.lstThread.Name = "lstThread";
            this.lstThread.Size = new System.Drawing.Size(206, 225);
            this.lstThread.TabIndex = 11;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(12, 306);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(627, 25);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblInfoThread
            // 
            this.lblInfoThread.AutoSize = true;
            this.lblInfoThread.Location = new System.Drawing.Point(433, 280);
            this.lblInfoThread.Name = "lblInfoThread";
            this.lblInfoThread.Size = new System.Drawing.Size(73, 13);
            this.lblInfoThread.TabIndex = 13;
            this.lblInfoThread.Text = "Item Count : 0";
            // 
            // lblActiveThreadCount
            // 
            this.lblActiveThreadCount.AutoSize = true;
            this.lblActiveThreadCount.Location = new System.Drawing.Point(13, 258);
            this.lblActiveThreadCount.Name = "lblActiveThreadCount";
            this.lblActiveThreadCount.Size = new System.Drawing.Size(120, 13);
            this.lblActiveThreadCount.TabIndex = 14;
            this.lblActiveThreadCount.Text = "Active Thread Count : 0";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 343);
            this.Controls.Add(this.lblActiveThreadCount);
            this.Controls.Add(this.lblInfoThread);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lstThread);
            this.Controls.Add(this.lstThreadPoolResult);
            this.Controls.Add(this.lblInfoThreadPool);
            this.Controls.Add(this.btnThreadPool);
            this.Controls.Add(this.btnThread);
            this.Controls.Add(this.btnThreadGun);
            this.Controls.Add(this.lblInfoThreadGun);
            this.Controls.Add(this.lstThreadGunResult);
            this.Controls.Add(this.btnNormal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TestForm";
            this.Text = "Test Threading Method";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.ListBox lstThreadGunResult;
        private System.Windows.Forms.Label lblInfoThreadGun;
        private System.Windows.Forms.Button btnThreadGun;
        private System.Windows.Forms.Button btnThread;
        private System.Windows.Forms.Button btnThreadPool;
        private System.Windows.Forms.Label lblInfoThreadPool;
        private System.Windows.Forms.ListBox lstThreadPoolResult;
        private System.Windows.Forms.ListBox lstThread;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblInfoThread;
        private System.Windows.Forms.Label lblActiveThreadCount;
    }
}

