using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xrm.Sdk.PluginRegistration.Wrappers;

namespace Xrm.Sdk.PluginRegistration.Forms
{
    public partial class DependenciesDialog : Form
    {
        public DependenciesDialog(CrmPluginAssembly assembly)
        {
            InitializeComponent();
            Text = $"Dependencies for {assembly.Name}";
            // For demonstration, just show referenced plugin types (plugins in this assembly)
            foreach (var plugin in assembly.Plugins.Values)
            {
                listBoxDependencies.Items.Add(plugin.Name);
            }
        }
    }
}
