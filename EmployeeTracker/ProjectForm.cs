using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeTracker.Models;

namespace EmployeeTracker
{
    public partial class ProjectForm : Form
    {
        private readonly List<Project> _projects;
        private Project _project = new Project();


        delegate void SetText(string message);
        event SetText ShowResults;
        public event EventHandler SaveComplete;

        public ProjectForm(List<Project> projects)
        {
            InitializeComponent();
            _projects = projects;
            txtId.Text = GetNextId().ToString();
            ShowResults += results_ShowResults;
            ShowResults("");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _project.Id = int.Parse(txtId.Text);
            _project.Name = txtName.Text;
            _project.Description = txtDescription.Text;
            _project.StartDate = DateTime.Parse(dtStartDate.Text);
            _project.EndDate = DateTime.Parse(dtEndDate.Text);
            _projects.Add(_project);
            ShowResults("Saved");
            SaveComplete(this, EventArgs.Empty);
        }

        private void loadTechnologies()
        {
            // clear the list
            lstTechnologies.Items.Clear();

            // put all the technologies in the list
            foreach (var tech in _project.Technologies)
            {
                lstTechnologies.Items.Add(tech);
            }
        }

        private void results_ShowResults(string message)
        {
            lblResult.Text = message;
        }

        private void lstTechnologies_Click(object sender, EventArgs e)
        {
            string tech = Prompt.ShowDialog("Technology", "Add");
            _project.Technologies.Add(tech);
            loadTechnologies();
        }

        public int GetNextId()
        {
            if (!_projects.Any())
            {
                return 0;
            }
            else
            {
                //var stores = _projects.Where(p => p.Name == "grocery");
                return _projects.Max(p => p.Id) + 1;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void lstTechnologies_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }
    }
}
