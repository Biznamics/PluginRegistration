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
        private const int WorkflowTypeDefinition = 1; // 1 = Definition (Process)
        private const int WorkflowStateDraft = 0;     // 0 = Draft
        private const int WorkflowStateActivated = 1; // 1 = Activated

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
                        listBoxDependencies.Items.Add(new WorkflowLinkItem { Display = $"Workflow: {wf.Name} (Open in CRM)", Url = wf.Url });
                }
                listBoxDependencies.DoubleClick += ListBoxDependencies_DoubleClick;
                listBoxDependencies.MouseMove += ListBoxDependencies_MouseMove;
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
            if (org != null && org.MessageEntities != null && org.MessageEntities.ContainsKey(step.MessageEntityId))
            {
                var msgEntity = org.MessageEntities[step.MessageEntityId];
                if (msgEntity.PrimaryEntity == "none")
                {
                    listBoxDependencies.Items.Add("All Entities");
                }
                else
                {
                    // Add as object, not string
                    listBoxDependencies.Items.Add(new EntityLinkItem { Display = $"Primary Entity: {msgEntity.PrimaryEntity} (Open in CRM)", Url = org.ConnectionDetail?.WebApplicationUrl?.TrimEnd('/') + $"/main.aspx?etn={msgEntity.PrimaryEntity}&pagetype=entitylist" });
                    if (!string.IsNullOrEmpty(msgEntity.SecondaryEntity) && msgEntity.SecondaryEntity != "none")
                    {
                        listBoxDependencies.Items.Add(new EntityLinkItem { Display = $"Secondary Entity: {msgEntity.SecondaryEntity} (Open in CRM)", Url = org.ConnectionDetail?.WebApplicationUrl?.TrimEnd('/') + $"/main.aspx?etn={msgEntity.SecondaryEntity}&pagetype=entitylist" });
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
            listBoxDependencies.DoubleClick += ListBoxDependencies_DoubleClick;
            listBoxDependencies.MouseMove += ListBoxDependencies_MouseMove;
        }

        private void ListBoxDependencies_DoubleClick(object sender, EventArgs e)
        {
            var item = listBoxDependencies.SelectedItem;
            if (item is EntityLinkItem linkItem)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = linkItem.Url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (item is WorkflowLinkItem wfItem)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = wfItem.Url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to open link: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
        private class WorkflowLinkItem
        {
            public string Display { get; set; }
            public string Url { get; set; }
            public override string ToString() => Display;
        }

        // Update FindWorkflowsUsingActivity to return List<WorkflowInfo> with Name and Url
        private List<WorkflowInfo> FindWorkflowsUsingActivity(CrmPlugin plugin)
        {
            var result = new List<WorkflowInfo>();
            try
            {
                var org = plugin.Organization;
                if (org == null || org.OrganizationService == null)
                    return result;
                var qe = new QueryExpression("workflow")
                {
                    ColumnSet = new ColumnSet("name", "xaml", "type", "statecode", "workflowid"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("type", ConditionOperator.Equal, WorkflowTypeDefinition),
                            new ConditionExpression("statecode", ConditionOperator.In, new object[] { WorkflowStateDraft, WorkflowStateActivated })
                        }
                    }
                };
                var workflows = org.OrganizationService.RetrieveMultiple(qe);
                var baseUrl = org.ConnectionDetail?.WebApplicationUrl?.TrimEnd('/');
                foreach (var wf in workflows.Entities)
                {
                    var xaml = wf.GetAttributeValue<string>("xaml");
                    if (!string.IsNullOrEmpty(xaml) && xaml.Contains(plugin.TypeName))
                    {
                        var name = wf.GetAttributeValue<string>("name");
                        var id = wf.Id;
                        string url = null;
                        if (!string.IsNullOrEmpty(baseUrl) && id != Guid.Empty)
                        {
                            url = $"{baseUrl}/main.aspx?pagetype=entityrecord&etn=workflow&id={id}";
                        }
                        result.Add(new WorkflowInfo { Name = name, Url = url });
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxDependencies.Items.Add($"Error querying CRM: {ex.Message}");
            }
            return result;
        }
        private class WorkflowInfo
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
