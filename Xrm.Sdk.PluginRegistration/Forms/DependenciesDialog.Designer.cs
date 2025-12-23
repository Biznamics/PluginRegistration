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
            // pictureBoxDependency
            // 
            this.pictureBoxDependency.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxDependency.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDependency.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDependency.Name = "pictureBoxDependency";
            this.pictureBoxDependency.TabIndex = 1;
            this.pictureBoxDependency.TabStop = false;
            // 
            // listBoxDependencies
            // 
            this.listBoxDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDependencies.FormattingEnabled = true;
            this.listBoxDependencies.Location = new System.Drawing.Point(0, this.pictureBoxDependency.Height);
            this.listBoxDependencies.Name = "listBoxDependencies";
            this.listBoxDependencies.Size = new System.Drawing.Size(384, 261 - this.pictureBoxDependency.Height);
            this.listBoxDependencies.TabIndex = 0;
            // 
            // DependenciesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.listBoxDependencies);
            this.Controls.Add(this.pictureBoxDependency);
            this.Name = "DependenciesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dependencies";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDependency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
