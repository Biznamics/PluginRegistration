namespace Xrm.Sdk.PluginRegistration.Forms
{
    partial class DependenciesDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxDependencies;
        private System.Windows.Forms.PictureBox pictureBoxDependency;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxDependencies = new System.Windows.Forms.ListBox();
            this.pictureBoxDependency = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDependency)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxDependencies
            // 
            this.listBoxDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDependencies.FormattingEnabled = true;
            this.listBoxDependencies.Location = new System.Drawing.Point(0, 50);
            this.listBoxDependencies.Name = "listBoxDependencies";
            this.listBoxDependencies.Size = new System.Drawing.Size(384, 211);
            this.listBoxDependencies.TabIndex = 0;
            this.listBoxDependencies.DoubleClick += new System.EventHandler(this.ListBoxDependencies_DoubleClick);
            // 
            // pictureBoxDependency
            // 
            this.pictureBoxDependency.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxDependency.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDependency.Name = "pictureBoxDependency";
            this.pictureBoxDependency.Size = new System.Drawing.Size(384, 50);
            this.pictureBoxDependency.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDependency.TabIndex = 1;
            this.pictureBoxDependency.TabStop = false;
            // 
            // DependenciesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.listBoxDependencies);
            this.Controls.Add(this.pictureBoxDependency);
            this.Name = "DependenciesDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dependencies";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDependency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
