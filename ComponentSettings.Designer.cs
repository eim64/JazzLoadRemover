namespace LiveSplit.UI.Components
{
    partial class ComponentSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoSplitGroupBox = new System.Windows.Forms.GroupBox();
            this.AutoSplitterCB = new System.Windows.Forms.CheckBox();
            this.LevelSplitsBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // AutoSplitGroupBox
            // 
            this.AutoSplitGroupBox.Location = new System.Drawing.Point(10, 197);
            this.AutoSplitGroupBox.Name = "AutoSplitGroupBox";
            this.AutoSplitGroupBox.Size = new System.Drawing.Size(200, 70);
            this.AutoSplitGroupBox.TabIndex = 0;
            this.AutoSplitGroupBox.TabStop = false;
            this.AutoSplitGroupBox.Text = "Other Splits";
            // 
            // AutoSplitterCB
            // 
            this.AutoSplitterCB.AutoSize = true;
            this.AutoSplitterCB.Location = new System.Drawing.Point(10, 11);
            this.AutoSplitterCB.Name = "AutoSplitterCB";
            this.AutoSplitterCB.Size = new System.Drawing.Size(114, 17);
            this.AutoSplitterCB.TabIndex = 1;
            this.AutoSplitterCB.Text = "Enable Autosplitter";
            this.AutoSplitterCB.UseVisualStyleBackColor = true;
            // 
            // LevelSplitsBox
            // 
            this.LevelSplitsBox.Location = new System.Drawing.Point(11, 35);
            this.LevelSplitsBox.Name = "LevelSplitsBox";
            this.LevelSplitsBox.Size = new System.Drawing.Size(200, 156);
            this.LevelSplitsBox.TabIndex = 2;
            this.LevelSplitsBox.TabStop = false;
            this.LevelSplitsBox.Text = "Level Transition Splits";
            // 
            // ComponentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LevelSplitsBox);
            this.Controls.Add(this.AutoSplitterCB);
            this.Controls.Add(this.AutoSplitGroupBox);
            this.Name = "ComponentSettings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(218, 539);
            this.Load += new System.EventHandler(this.ComponentSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox AutoSplitGroupBox;
        private System.Windows.Forms.CheckBox AutoSplitterCB;
        private System.Windows.Forms.GroupBox LevelSplitsBox;
    }
}
