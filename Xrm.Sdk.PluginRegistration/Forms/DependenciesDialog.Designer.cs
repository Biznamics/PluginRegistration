namespace Xrm.Sdk.PluginRegistration.Forms
{
    partial class DependenciesDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox listBoxDependencies;

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
            this.SuspendLayout();
            // 
            // listBoxDependencies
            // 
            this.listBoxDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDependencies.FormattingEnabled = true;
            this.listBoxDependencies.Location = new System.Drawing.Point(0, 0);
            this.listBoxDependencies.Name = "listBoxDependencies";
            this.listBoxDependencies.Size = new System.Drawing.Size(384, 261);
            this.listBoxDependencies.TabIndex = 0;
            // 
            // DependenciesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.listBoxDependencies);
            this.Name = "DependenciesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dependencies";
            this.ResumeLayout(false);
        }
    }
}
