namespace Patchwork
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
            this.saveSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patchPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.combineCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.reverseLeftrightRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseJointNameJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringForwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringBackwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.patchView = new System.Windows.Forms.ListView();
            this.patchImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.CBoxDrawSkeleton = new System.Windows.Forms.CheckBox();
            this.CBoxDrawRefSkeleton = new System.Windows.Forms.CheckBox();
            this.CBoxDrawPolygon = new System.Windows.Forms.CheckBox();
            this.CBoxDrawMesh = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scaleSlider = new System.Windows.Forms.TrackBar();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.patchPToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(967, 26);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOToolStripMenuItem,
            this.saveSToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(57, 22);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // openOToolStripMenuItem
            // 
            this.openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            this.openOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openOToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.openOToolStripMenuItem.Text = "Open(&O)";
            this.openOToolStripMenuItem.Click += new System.EventHandler(this.openOToolStripMenuItem_Click);
            // 
            // saveSToolStripMenuItem
            // 
            this.saveSToolStripMenuItem.Name = "saveSToolStripMenuItem";
            this.saveSToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.saveSToolStripMenuItem.Text = "Save (&S)";
            this.saveSToolStripMenuItem.Click += new System.EventHandler(this.saveSToolStripMenuItem_Click);
            // 
            // patchPToolStripMenuItem
            // 
            this.patchPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.combineCToolStripMenuItem,
            this.deleteDToolStripMenuItem,
            this.toolStripSeparator1,
            this.reverseLeftrightRToolStripMenuItem,
            this.reverseJointNameJToolStripMenuItem,
            this.toolStripSeparator2,
            this.bringToFrontToolStripMenuItem,
            this.bringForwardToolStripMenuItem,
            this.bringToBackToolStripMenuItem,
            this.bringBackwardToolStripMenuItem});
            this.patchPToolStripMenuItem.Name = "patchPToolStripMenuItem";
            this.patchPToolStripMenuItem.Size = new System.Drawing.Size(73, 22);
            this.patchPToolStripMenuItem.Text = "Patch (&P)";
            // 
            // combineCToolStripMenuItem
            // 
            this.combineCToolStripMenuItem.Name = "combineCToolStripMenuItem";
            this.combineCToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.combineCToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.combineCToolStripMenuItem.Text = "Combine(&C)";
            this.combineCToolStripMenuItem.Click += new System.EventHandler(this.combineCToolStripMenuItem_Click);
            // 
            // deleteDToolStripMenuItem
            // 
            this.deleteDToolStripMenuItem.Name = "deleteDToolStripMenuItem";
            this.deleteDToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteDToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.deleteDToolStripMenuItem.Text = "Delete(&D)";
            this.deleteDToolStripMenuItem.Click += new System.EventHandler(this.deleteDToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(250, 6);
            // 
            // reverseLeftrightRToolStripMenuItem
            // 
            this.reverseLeftrightRToolStripMenuItem.Name = "reverseLeftrightRToolStripMenuItem";
            this.reverseLeftrightRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.reverseLeftrightRToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.reverseLeftrightRToolStripMenuItem.Text = "Reverse left-right (R)";
            this.reverseLeftrightRToolStripMenuItem.Click += new System.EventHandler(this.reverseLeftrightRToolStripMenuItem_Click);
            // 
            // reverseJointNameJToolStripMenuItem
            // 
            this.reverseJointNameJToolStripMenuItem.Name = "reverseJointNameJToolStripMenuItem";
            this.reverseJointNameJToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.reverseJointNameJToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.reverseJointNameJToolStripMenuItem.Text = "Reverse joint name (J)";
            this.reverseJointNameJToolStripMenuItem.Click += new System.EventHandler(this.reverseJointNameJToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(250, 6);
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.bringToFrontToolStripMenuItem.Text = "Bring to front";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringToFrontToolStripMenuItem_Click);
            // 
            // bringForwardToolStripMenuItem
            // 
            this.bringForwardToolStripMenuItem.Name = "bringForwardToolStripMenuItem";
            this.bringForwardToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.bringForwardToolStripMenuItem.Text = "Bring forward";
            this.bringForwardToolStripMenuItem.Click += new System.EventHandler(this.bringForwardToolStripMenuItem_Click);
            // 
            // bringToBackToolStripMenuItem
            // 
            this.bringToBackToolStripMenuItem.Name = "bringToBackToolStripMenuItem";
            this.bringToBackToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.bringToBackToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.bringToBackToolStripMenuItem.Text = "Bring to back";
            this.bringToBackToolStripMenuItem.Click += new System.EventHandler(this.bringToBackToolStripMenuItem_Click);
            // 
            // bringBackwardToolStripMenuItem
            // 
            this.bringBackwardToolStripMenuItem.Name = "bringBackwardToolStripMenuItem";
            this.bringBackwardToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.bringBackwardToolStripMenuItem.Text = "Bring backward";
            this.bringBackwardToolStripMenuItem.Click += new System.EventHandler(this.bringBackwardToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.patchView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(967, 472);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 1;
            // 
            // patchView
            // 
            this.patchView.AllowDrop = true;
            this.patchView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patchView.LargeImageList = this.patchImageList;
            this.patchView.Location = new System.Drawing.Point(0, 0);
            this.patchView.Name = "patchView";
            this.patchView.Size = new System.Drawing.Size(320, 472);
            this.patchView.SmallImageList = this.patchImageList;
            this.patchView.TabIndex = 0;
            this.patchView.UseCompatibleStateImageBehavior = false;
            this.patchView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.patchView_ItemDrag);
            this.patchView.DragEnter += new System.Windows.Forms.DragEventHandler(this.patchVieww_DragOver);
            this.patchView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.patchView_MouseDoubleClick);
            // 
            // patchImageList
            // 
            this.patchImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.patchImageList.ImageSize = new System.Drawing.Size(100, 100);
            this.patchImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.CBoxDrawSkeleton);
            this.splitContainer2.Panel1.Controls.Add(this.CBoxDrawRefSkeleton);
            this.splitContainer2.Panel1.Controls.Add(this.CBoxDrawPolygon);
            this.splitContainer2.Panel1.Controls.Add(this.CBoxDrawMesh);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.scaleSlider);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.canvas);
            this.splitContainer2.Size = new System.Drawing.Size(643, 472);
            this.splitContainer2.SplitterDistance = 109;
            this.splitContainer2.TabIndex = 1;
            // 
            // CBoxDrawSkeleton
            // 
            this.CBoxDrawSkeleton.AutoSize = true;
            this.CBoxDrawSkeleton.Location = new System.Drawing.Point(6, 57);
            this.CBoxDrawSkeleton.Name = "CBoxDrawSkeleton";
            this.CBoxDrawSkeleton.Size = new System.Drawing.Size(95, 16);
            this.CBoxDrawSkeleton.TabIndex = 5;
            this.CBoxDrawSkeleton.Text = "draw skeleton";
            this.CBoxDrawSkeleton.UseVisualStyleBackColor = true;
            // 
            // CBoxDrawRefSkeleton
            // 
            this.CBoxDrawRefSkeleton.AutoSize = true;
            this.CBoxDrawRefSkeleton.Checked = true;
            this.CBoxDrawRefSkeleton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBoxDrawRefSkeleton.Location = new System.Drawing.Point(6, 79);
            this.CBoxDrawRefSkeleton.Name = "CBoxDrawRefSkeleton";
            this.CBoxDrawRefSkeleton.Size = new System.Drawing.Size(147, 16);
            this.CBoxDrawRefSkeleton.TabIndex = 4;
            this.CBoxDrawRefSkeleton.Text = "draw reference skeleton";
            this.CBoxDrawRefSkeleton.UseVisualStyleBackColor = true;
            // 
            // CBoxDrawPolygon
            // 
            this.CBoxDrawPolygon.AutoSize = true;
            this.CBoxDrawPolygon.Location = new System.Drawing.Point(6, 35);
            this.CBoxDrawPolygon.Name = "CBoxDrawPolygon";
            this.CBoxDrawPolygon.Size = new System.Drawing.Size(91, 16);
            this.CBoxDrawPolygon.TabIndex = 3;
            this.CBoxDrawPolygon.Text = "draw polygon";
            this.CBoxDrawPolygon.UseVisualStyleBackColor = true;
            // 
            // CBoxDrawMesh
            // 
            this.CBoxDrawMesh.AutoSize = true;
            this.CBoxDrawMesh.Checked = true;
            this.CBoxDrawMesh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBoxDrawMesh.Location = new System.Drawing.Point(6, 13);
            this.CBoxDrawMesh.Name = "CBoxDrawMesh";
            this.CBoxDrawMesh.Size = new System.Drawing.Size(79, 16);
            this.CBoxDrawMesh.TabIndex = 2;
            this.CBoxDrawMesh.Text = "draw mesh";
            this.CBoxDrawMesh.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(208, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scale";
            // 
            // scaleSlider
            // 
            this.scaleSlider.Location = new System.Drawing.Point(259, 13);
            this.scaleSlider.Maximum = 100;
            this.scaleSlider.Name = "scaleSlider";
            this.scaleSlider.Size = new System.Drawing.Size(304, 45);
            this.scaleSlider.TabIndex = 0;
            this.scaleSlider.Value = 10;
            this.scaleSlider.Scroll += new System.EventHandler(this.scaleSlider_Scroll);
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(643, 359);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            this.canvas.DragDrop += new System.Windows.Forms.DragEventHandler(this.canvas_DragDrop);
            this.canvas.DragEnter += new System.Windows.Forms.DragEventHandler(this.canvas_DragEnter);
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
            this.ClientSize = new System.Drawing.Size(967, 498);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scaleSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView patchView;
        private System.Windows.Forms.ImageList patchImageList;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.ToolStripMenuItem openOToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem patchPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem combineCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar scaleSlider;
        private System.Windows.Forms.ToolStripMenuItem saveSToolStripMenuItem;
        private System.Windows.Forms.CheckBox CBoxDrawRefSkeleton;
        private System.Windows.Forms.CheckBox CBoxDrawPolygon;
        private System.Windows.Forms.CheckBox CBoxDrawMesh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem reverseLeftrightRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverseJointNameJToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringForwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringToBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringBackwardToolStripMenuItem;
        private System.Windows.Forms.CheckBox CBoxDrawSkeleton;
    }
}

