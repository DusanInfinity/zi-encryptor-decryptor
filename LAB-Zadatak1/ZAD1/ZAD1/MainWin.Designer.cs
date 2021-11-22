
namespace ZAD1
{
    partial class MainWin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbKodiranje = new System.Windows.Forms.GroupBox();
            this.btnDekodiranje = new System.Windows.Forms.Button();
            this.btnIzmeniOdrediste = new System.Windows.Forms.Button();
            this.rbFSWNeaktivan = new System.Windows.Forms.RadioButton();
            this.tbOdrediste = new System.Windows.Forms.TextBox();
            this.lblOdrediste = new System.Windows.Forms.Label();
            this.rbFSWAktivan = new System.Windows.Forms.RadioButton();
            this.lblFSWStatus = new System.Windows.Forms.Label();
            this.btnIzmeniIzvor = new System.Windows.Forms.Button();
            this.btnKodiranje = new System.Windows.Forms.Button();
            this.tbIzvor = new System.Windows.Forms.TextBox();
            this.lblIzvor = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDiag = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDiag = new System.Windows.Forms.SaveFileDialog();
            this.fsWatcher = new System.IO.FileSystemWatcher();
            this.statStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.gbKodiranje.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatcher)).BeginInit();
            this.statStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbKodiranje
            // 
            this.gbKodiranje.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbKodiranje.Controls.Add(this.btnDekodiranje);
            this.gbKodiranje.Controls.Add(this.btnIzmeniOdrediste);
            this.gbKodiranje.Controls.Add(this.rbFSWNeaktivan);
            this.gbKodiranje.Controls.Add(this.tbOdrediste);
            this.gbKodiranje.Controls.Add(this.lblOdrediste);
            this.gbKodiranje.Controls.Add(this.rbFSWAktivan);
            this.gbKodiranje.Controls.Add(this.lblFSWStatus);
            this.gbKodiranje.Controls.Add(this.btnIzmeniIzvor);
            this.gbKodiranje.Controls.Add(this.btnKodiranje);
            this.gbKodiranje.Controls.Add(this.tbIzvor);
            this.gbKodiranje.Controls.Add(this.lblIzvor);
            this.gbKodiranje.Location = new System.Drawing.Point(12, 30);
            this.gbKodiranje.Name = "gbKodiranje";
            this.gbKodiranje.Size = new System.Drawing.Size(396, 293);
            this.gbKodiranje.TabIndex = 0;
            this.gbKodiranje.TabStop = false;
            this.gbKodiranje.Text = "File System Watcher";
            // 
            // btnDekodiranje
            // 
            this.btnDekodiranje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDekodiranje.Location = new System.Drawing.Point(263, 255);
            this.btnDekodiranje.Name = "btnDekodiranje";
            this.btnDekodiranje.Size = new System.Drawing.Size(127, 25);
            this.btnDekodiranje.TabIndex = 2;
            this.btnDekodiranje.Text = "Pokreni dekodiranje";
            this.btnDekodiranje.UseVisualStyleBackColor = true;
            this.btnDekodiranje.Click += new System.EventHandler(this.btnDekodiranje_Click);
            // 
            // btnIzmeniOdrediste
            // 
            this.btnIzmeniOdrediste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIzmeniOdrediste.Location = new System.Drawing.Point(315, 136);
            this.btnIzmeniOdrediste.Name = "btnIzmeniOdrediste";
            this.btnIzmeniOdrediste.Size = new System.Drawing.Size(75, 23);
            this.btnIzmeniOdrediste.TabIndex = 3;
            this.btnIzmeniOdrediste.Text = "Izmeni";
            this.btnIzmeniOdrediste.Click += new System.EventHandler(this.btnIzmeniOdrediste_Click);
            // 
            // rbFSWNeaktivan
            // 
            this.rbFSWNeaktivan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbFSWNeaktivan.AutoSize = true;
            this.rbFSWNeaktivan.BackColor = System.Drawing.SystemColors.Control;
            this.rbFSWNeaktivan.ForeColor = System.Drawing.Color.Maroon;
            this.rbFSWNeaktivan.Location = new System.Drawing.Point(165, 243);
            this.rbFSWNeaktivan.Name = "rbFSWNeaktivan";
            this.rbFSWNeaktivan.Size = new System.Drawing.Size(78, 19);
            this.rbFSWNeaktivan.TabIndex = 4;
            this.rbFSWNeaktivan.Text = "Neaktivan";
            this.rbFSWNeaktivan.UseVisualStyleBackColor = false;
            // 
            // tbOdrediste
            // 
            this.tbOdrediste.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOdrediste.Location = new System.Drawing.Point(21, 136);
            this.tbOdrediste.Name = "tbOdrediste";
            this.tbOdrediste.ReadOnly = true;
            this.tbOdrediste.Size = new System.Drawing.Size(269, 23);
            this.tbOdrediste.TabIndex = 5;
            // 
            // lblOdrediste
            // 
            this.lblOdrediste.AutoSize = true;
            this.lblOdrediste.Location = new System.Drawing.Point(21, 118);
            this.lblOdrediste.Name = "lblOdrediste";
            this.lblOdrediste.Size = new System.Drawing.Size(61, 15);
            this.lblOdrediste.TabIndex = 4;
            this.lblOdrediste.Text = "Odrediste:";
            // 
            // rbFSWAktivan
            // 
            this.rbFSWAktivan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbFSWAktivan.AutoSize = true;
            this.rbFSWAktivan.BackColor = System.Drawing.SystemColors.Control;
            this.rbFSWAktivan.Checked = true;
            this.rbFSWAktivan.ForeColor = System.Drawing.Color.DarkGreen;
            this.rbFSWAktivan.Location = new System.Drawing.Point(94, 243);
            this.rbFSWAktivan.Name = "rbFSWAktivan";
            this.rbFSWAktivan.Size = new System.Drawing.Size(65, 19);
            this.rbFSWAktivan.TabIndex = 3;
            this.rbFSWAktivan.TabStop = true;
            this.rbFSWAktivan.Text = "Aktivan";
            this.rbFSWAktivan.UseVisualStyleBackColor = false;
            this.rbFSWAktivan.CheckedChanged += new System.EventHandler(this.rbFSWAktivan_CheckedChanged);
            // 
            // lblFSWStatus
            // 
            this.lblFSWStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFSWStatus.AutoSize = true;
            this.lblFSWStatus.Location = new System.Drawing.Point(21, 243);
            this.lblFSWStatus.Name = "lblFSWStatus";
            this.lblFSWStatus.Size = new System.Drawing.Size(67, 15);
            this.lblFSWStatus.TabIndex = 2;
            this.lblFSWStatus.Text = "FSW status:";
            // 
            // btnIzmeniIzvor
            // 
            this.btnIzmeniIzvor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIzmeniIzvor.Location = new System.Drawing.Point(315, 69);
            this.btnIzmeniIzvor.Name = "btnIzmeniIzvor";
            this.btnIzmeniIzvor.Size = new System.Drawing.Size(75, 23);
            this.btnIzmeniIzvor.TabIndex = 0;
            this.btnIzmeniIzvor.Text = "Izmeni";
            this.btnIzmeniIzvor.Click += new System.EventHandler(this.btnIzmeniFSPutanju_Click);
            // 
            // btnKodiranje
            // 
            this.btnKodiranje.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKodiranje.Location = new System.Drawing.Point(263, 224);
            this.btnKodiranje.Name = "btnKodiranje";
            this.btnKodiranje.Size = new System.Drawing.Size(127, 25);
            this.btnKodiranje.TabIndex = 1;
            this.btnKodiranje.Text = "Pokreni kodiranje";
            this.btnKodiranje.UseVisualStyleBackColor = true;
            this.btnKodiranje.Click += new System.EventHandler(this.btnKodiranje_Click);
            // 
            // tbIzvor
            // 
            this.tbIzvor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIzvor.Location = new System.Drawing.Point(21, 69);
            this.tbIzvor.Name = "tbIzvor";
            this.tbIzvor.ReadOnly = true;
            this.tbIzvor.Size = new System.Drawing.Size(269, 23);
            this.tbIzvor.TabIndex = 1;
            // 
            // lblIzvor
            // 
            this.lblIzvor.AutoSize = true;
            this.lblIzvor.Location = new System.Drawing.Point(21, 51);
            this.lblIzvor.Name = "lblIzvor";
            this.lblIzvor.Size = new System.Drawing.Size(35, 15);
            this.lblIzvor.TabIndex = 0;
            this.lblIzvor.Text = "Izvor:";
            // 
            // openFileDiag
            // 
            this.openFileDiag.Filter = "*.txt|";
            // 
            // saveFileDiag
            // 
            this.saveFileDiag.DefaultExt = "txt";
            this.saveFileDiag.Filter = "*.txt|";
            // 
            // fsWatcher
            // 
            this.fsWatcher.EnableRaisingEvents = true;
            this.fsWatcher.NotifyFilter = System.IO.NotifyFilters.FileName;
            this.fsWatcher.SynchronizingObject = this;
            this.fsWatcher.Created += new System.IO.FileSystemEventHandler(this.fsWatcher_Created);
            // 
            // statStrip
            // 
            this.statStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statStrip.Location = new System.Drawing.Point(0, 312);
            this.statStrip.Name = "statStrip";
            this.statStrip.Size = new System.Drawing.Size(420, 22);
            this.statStrip.TabIndex = 1;
            this.statStrip.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 17);
            this.lblStatus.Text = "Aplikacija pokrenuta!";
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 334);
            this.Controls.Add(this.statStrip);
            this.Controls.Add(this.gbKodiranje);
            this.MinimumSize = new System.Drawing.Size(436, 373);
            this.Name = "MainWin";
            this.Text = "RC4 Encryptor/Decryptor";
            this.gbKodiranje.ResumeLayout(false);
            this.gbKodiranje.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatcher)).EndInit();
            this.statStrip.ResumeLayout(false);
            this.statStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbKodiranje;
        private System.Windows.Forms.Button btnIzmeniIzvor;
        private System.Windows.Forms.TextBox tbIzvor;
        private System.Windows.Forms.Label lblIzvor;
        private System.Windows.Forms.RadioButton rbFSWNeaktivan;
        private System.Windows.Forms.RadioButton rbFSWAktivan;
        private System.Windows.Forms.Label lblFSWStatus;
        private System.Windows.Forms.Button btnKodiranje;
        private System.Windows.Forms.Button btnDekodiranje;
        private System.Windows.Forms.Button btnIzmeniOdrediste;
        private System.Windows.Forms.TextBox tbOdrediste;
        private System.Windows.Forms.Label lblOdrediste;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDiag;
        private System.Windows.Forms.SaveFileDialog saveFileDiag;
        private System.IO.FileSystemWatcher fsWatcher;
        private System.Windows.Forms.StatusStrip statStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}

