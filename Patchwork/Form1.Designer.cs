﻿namespace Patchwork
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.segmentView = new System.Windows.Forms.ListView();
            this.segmentImageList = new System.Windows.Forms.ImageList(this.components);
            this.canvas = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(710, 26);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(57, 22);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // openOToolStripMenuItem
            // 
            this.openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            this.openOToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.openOToolStripMenuItem.Text = "Open(&O)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.segmentView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.canvas);
            this.splitContainer1.Size = new System.Drawing.Size(710, 465);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 1;
            // 
            // segmentView
            // 
            this.segmentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentView.LargeImageList = this.segmentImageList;
            this.segmentView.Location = new System.Drawing.Point(0, 0);
            this.segmentView.Name = "segmentView";
            this.segmentView.Size = new System.Drawing.Size(236, 465);
            this.segmentView.SmallImageList = this.segmentImageList;
            this.segmentView.TabIndex = 0;
            this.segmentView.UseCompatibleStateImageBehavior = false;
            this.segmentView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.segmentView_MouseDoubleClick);
            // 
            // segmentImageList
            // 
            this.segmentImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.segmentImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.segmentImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(470, 465);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 491);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView segmentView;
        private System.Windows.Forms.ImageList segmentImageList;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.ToolStripMenuItem openOToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timer;
    }
}
