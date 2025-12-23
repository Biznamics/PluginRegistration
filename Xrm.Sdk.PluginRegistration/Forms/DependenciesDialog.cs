using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xrm.Sdk.PluginRegistration.Wrappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Diagnostics;

namespace Xrm.Sdk.PluginRegistration.Forms
{
    public partial class DependenciesDialog : Form
    {
        public DependenciesDialog(CrmPluginAssembly assembly)
        {
            InitializeComponent();
            Text = $"Dependencies for Assembly: {assembly.Name}";
            foreach (var plugin in assembly.Plugins.Values)
            {
                listBoxDependencies.Items.Add($"Plugin: {plugin.Name}");
            }
        }

        public DependenciesDialog(CrmPlugin plugin)
        {
            InitializeComponent();
            Text = $"Dependencies for Plugin: {plugin.Name}";
            if (plugin.PluginType == CrmPluginType.WorkflowActivity)
            {
                // Query CRM for workflows using this activity
                var workflows = FindWorkflowsUsingActivity(plugin);
                if (workflows.Count == 0)
                {
                    listBoxDependencies.Items.Add("No workflows found using this activity.");
                }
                else
                {
                    foreach (var wf in workflows)
                        listBoxDependencies.Items.Add($"Workflow: {wf}");
                }
            }
            else
            {
                foreach (var step in plugin.Steps.Values)
                {
                    listBoxDependencies.Items.Add($"Step: {step.Name}");
                }
            }
        }

        public DependenciesDialog(CrmPluginStep step)
        {
            InitializeComponent();
            Text = $"Dependencies for Step: {step.Name}";
            var org = step.Organization;
            bool addedLink = false;
            if (org != null && org.MessageEntities != null && org.MessageEntities.ContainsKey(step.MessageEntityId))
            {
                var msgEntity = org.MessageEntities[step.MessageEntityId];
                if (msgEntity.PrimaryEntity == "none")
                {
                    listBoxDependencies.Items.Add("All Entities");
                }
                else
                {
                    // Add clickable link for primary entity
                    AddEntityLink(msgEntity.PrimaryEntity, org.ConnectionDetail?.WebApplicationUrl);
                    addedLink = true;
                    if (!string.IsNullOrEmpty(msgEntity.SecondaryEntity) && msgEntity.SecondaryEntity != "none")
                    {
                        AddEntityLink(msgEntity.SecondaryEntity, org.ConnectionDetail?.WebApplicationUrl, "Secondary Entity: ");
                        addedLink = true;
                    }
                }
            }
            else
            {
                listBoxDependencies.Items.Add("Entity information not available.");
            }
            foreach (var image in step.Images.Values)
            {
                listBoxDependencies.Items.Add($"Image: {image.Name}");
            }
            if (addedLink)
            {
                listBoxDependencies.MouseClick += ListBoxDependencies_MouseClick;
                listBoxDependencies.MouseMove += ListBoxDependencies_MouseMove;
            }
        }

        private void AddEntityLink(string entityLogicalName, string baseUrl, string prefix = "Primary Entity: ")
        {
            if (string.IsNullOrEmpty(entityLogicalName) || string.IsNullOrEmpty(baseUrl))
            {
                listBoxDependencies.Items.Add($"{prefix}{entityLogicalName}");
                return;
            }
            string url = baseUrl.TrimEnd('/') + $"/main.aspx?etn={entityLogicalName}&pagetype=entitylist";
            // Store as a special ListBoxItem
            listBoxDependencies.Items.Add(new EntityLinkItem { Display = $"{prefix}{entityLogicalName} (Open in CRM)", Url = url });
        }

        private void ListBoxDependencies_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBoxDependencies.IndexFromPoint(e.Location);
            if (index >= 0 && listBoxDependencies.Items[index] is EntityLinkItem item)
            {
                try { Process.Start(item.Url); } catch { }
            }
        }

        private void ListBoxDependencies_MouseMove(object sender, MouseEventArgs e)
        {
            int index = listBoxDependencies.IndexFromPoint(e.Location);
            if (index >= 0 && listBoxDependencies.Items[index] is EntityLinkItem)
            {
                listBoxDependencies.Cursor = Cursors.Hand;
            }
            else
            {
                listBoxDependencies.Cursor = Cursors.Default;
            }
        }

        private class EntityLinkItem
        {
            public string Display { get; set; }
            public string Url { get; set; }
            public override string ToString() => Display;
        }

        private List<string> FindWorkflowsUsingActivity(CrmPlugin plugin)
        {
            var result = new List<string>();
            try
            {
                var org = plugin.Organization;
                if (org == null || org.OrganizationService == null)
                    return result;
                // Query workflows where XAML contains the type name
                var qe = new QueryExpression("workflow")
                {
                    ColumnSet = new ColumnSet("name", "xaml", "type", "statecode"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("type", ConditionOperator.Equal, 1), // Definition (not dialog)
                            new ConditionExpression("statecode", ConditionOperator.In, new object[] {0,1}) // Draft or Activated
                        }
                    }
                };
                var workflows = org.OrganizationService.RetrieveMultiple(qe);
                foreach (var wf in workflows.Entities)
                {
                    var xaml = wf.GetAttributeValue<string>("xaml");
                    if (!string.IsNullOrEmpty(xaml) && xaml.Contains(plugin.TypeName))
                    {
                        result.Add(wf.GetAttributeValue<string>("name"));
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxDependencies.Items.Add($"Error querying CRM: {ex.Message}");
            }
            return result;
        }
    }
}
