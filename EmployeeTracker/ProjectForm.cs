using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EmployeeTracker.Models;

namespace EmployeeTracker
{
    public partial class ProjectForm : Form
    {
        // delecrations of private class scope varialbes

        // list of projects
        private List<Project> _projects;

        // the current project being added or edited
        private Project _project;

        // update mode flag:  adding = true, editing = false 
        // used during save to either add this to the list of projects
        // or update a record
        private bool _adding;

        // a delegete method: this is the method siginature that 
        // will be used throughout the class by ShowResults and 
        // the listener Results_ShowResults to update the result text
        delegate void SetText(string message);

        // event object: raised in code to signal to 
        // the listeners or subscribers 
        // attach a listener to handle the event
        event SetText ShowResults;

        // another event: uses EventHandler a System class delegete, 
        // raise to the calling listener that we need to save,
        // done in the btnSave_Click method
        public event EventHandler DoSave;

        // default class constructor,
        // happens when you "new up" an object
        public ProjectForm(List<Project> projects)
        {
            // setting the intial state of all the 
            // class level objects

            // set the priviate project object (class level scope)
            // to the list passed in
            _projects = projects;

            _adding = true;
            _project = new Project();

            // call loading controls in the designer 
            // partial class
            InitializeComponent();

            // set the id, will get written over if it is the edit 
            // constructor calling this constructor
            txtId.Text = GetNextId().ToString();

            // attach the event listener, the Results_ShowResults method,
            // to the ShowResults event, the code in Results_ShowResults 
            // will get executed when the event is raised
            ShowResults += Results_ShowResults;

            // raising the event on start up
            // to set the results to a empty string
            ShowResults("");
        }

        // overloaded constructor: happens when you "new up" an object
        // with both of these prameters vs just the list of projects
        public ProjectForm(List<Project> projects, int projectId)
            : this(projects) // call the "base" constructor
        {
            // use Single, list extension method, to get the first one
            // if there is not exactly one, it will throw an error
            try
            {
                _project = _projects.Single(proj => proj.Id == projectId);
            }
            catch (Exception)
            {
                // throw;   // commenting out rasising the event - 
                            // this is called swallowing the error, not 
                            // good unless certian the failure isn't 
                            // caused by something else that needs to be delt with in 
                            // this case just tell the user there is a problem and continue
                string message = "Fyi... there are no or more than one projects with a matching id.";
                string caption = "There can only be one";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (_project != null)
            {
                // max val set in default constuctor
                // will be over written using the id to be edited
                txtId.Text = _project.Id.ToString();

                txtName.Text = _project.Name;
                txtDescription.Text = _project.Description;
                dtStartDate.Text = _project.StartDate.ToString();
                dtEndDate.Text = _project.EndDate.ToString();
                LoadTechnologies();
                _adding = false;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            _project.Id = int.Parse(txtId.Text);
            _project.Name = txtName.Text;
            _project.Description = txtDescription.Text;
            _project.StartDate = DateTime.Parse(dtStartDate.Text);
            _project.EndDate = DateTime.Parse(dtEndDate.Text);

            if (_adding)
            {
                _projects.Add(_project);
                _adding = false;  // take it out of edit mode after the first save
            }

            ShowResults("Saved");
            DoSave(this, EventArgs.Empty);

        }

        private void LoadTechnologies()
        {
            // clear the list
            lstTechnologies.Items.Clear();

            // put all the technologies in the list
            foreach (var tech in _project.Technologies)
            {
                lstTechnologies.Items.Add(tech);
            }
        }

        private void Results_ShowResults(string message)
        {
            lblResult.Text = message;
        }

        private void LstTechnologies_Click(object sender, EventArgs e)
        {
            // use the static Prompt class to create a custom
            // prompt
            string tech = Prompt.ShowDialog("Technology", "Add");

            // use the list Add extension method to add a new item to 
            // the list
            _project.Technologies.Add(tech);

            // reload the list
            LoadTechnologies();
        }

        /// <summary>
        /// This will get the next unused id in a list of projects 
        /// </summary>
        /// <returns>int max + 1</returns>
        public int GetNextId()
        {
            if (_projects == null || !_projects.Any())
            {
                return 0;
            }
            else
            {
                return _projects.Max(p => p.Id) + 1;
            }
        }

        // whenenver a form field is modified raise the ShowResults
        // event method to clear the results label
        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void TxtDescription_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void DtStartDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void DtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void LstTechnologies_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }
    }
}
