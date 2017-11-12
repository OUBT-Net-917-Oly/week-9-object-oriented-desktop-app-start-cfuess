using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmployeeTracker.Data;
using EmployeeTracker.Models;

namespace EmployeeTracker
{
    public partial class EmployeeListForm : Form
    {
        //declare field variables
        private DataStore _dataStore;
        private FileManager _fileManager;
        private int _selectedEmployeeId = 0;
        private int _selectedProjectId = 0;

        public EmployeeListForm()
        {
            InitializeComponent();
            _fileManager = new FileManager();
            _dataStore = _fileManager.Load();
            loadEmployeeList();
            loadProjectList();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm(_dataStore.Employees);
            employeeForm.DoSave += SaveAndReload;
            employeeForm.Show();
        }

        private void SaveAndReload(object sender, EventArgs e)
        {
            _fileManager.Save(_dataStore);
            loadEmployeeList();
            loadProjectList();
        }

        private void loadEmployeeList()
        {
            //// clear the list
            //lstEmployees.Items.Clear();

            //// put all the employees in the list
            //foreach (var employee in _dataStore.Employees)
            //{
            //    lstEmployees.Items.Add(employee.FullName);
            //}

            // instead of loading the listbox with a bunch of strings 
            // bind to a collection of employees and set what is displayed
            lstEmployees.DataSource = null;
            lstEmployees.DataSource = _dataStore.Employees;
            lstEmployees.DisplayMember = "FullName";
        }

        private void loadProjectList()
        {
            //// clear the list
            //lstProjects.Items.Clear();

            //// put all the projects in the list
            //foreach (var project in _dataStore.Projects)
            //{
            //    lstProjects.Items.Add(project.Name);
            //}
            lstProjects.DataSource = null;
            lstProjects.DataSource = _dataStore.Projects;
            lstProjects.DisplayMember = "Name";
        }

        private void btnAddProject_Click(object sender, EventArgs e)
        {
            ProjectForm projectForm = new ProjectForm(_dataStore.Projects);
            // attach the event listener, SaveAndReload, to the 
            // project form's DoSave event, the code in SaveAndReload 
            // will get executed when the event is raised
            projectForm.DoSave += SaveAndReload;
            projectForm.Show();
        }

        // When an item in the listbox is clicked get the id for editing and deleting
        private void lstEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListBox empListBox = sender as ListBox;
                Employee selectedEmployee = empListBox.SelectedItem as Employee;
                _selectedEmployeeId = selectedEmployee.Id;
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            // call the constructor that will initiate an edit instead of an add
            EmployeeForm employeeForm = new EmployeeForm(_dataStore.Employees, _selectedEmployeeId);
            
            // attach the event listener, SaveAndReload, to the DoSave event 
            employeeForm.DoSave += SaveAndReload;
            employeeForm.Show();
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (!isSure("Employee"))
            {
                return;  //exit the function; do not run rest of code
            }

            // find the first employee with a matching id
            var employee = _dataStore.Employees.FirstOrDefault(emp => emp.Id == _selectedEmployeeId);

            if (employee != null)
            {
                _dataStore.Employees.Remove(employee);
                //save_Clicked(null, null);
                SaveAndReload(this, EventArgs.Empty);
            }
        }

        private void btnProjectDelete(object sender, EventArgs e)
        {
            if (!isSure("Project"))
            {
                return;  //exit the function; do not run rest of code
            }

            // manual iteration and matching
            // instead of using the list's FirstOrDefault method 
            // as done in the empoyee delete method above
            Project matchedProject = null;

            foreach (var proj in _dataStore.Projects)
            {
                if (proj.Id == _selectedProjectId)
                {
                    matchedProject = proj;
                }
            }

            if (matchedProject != null)
            {
                _dataStore.Projects.Remove(matchedProject);
                SaveAndReload(this, EventArgs.Empty);
            }
        }


        private void btnEditProject_Click(object sender, EventArgs e)
        {
            // call the constructor that will initiate an edit instead of an add
            ProjectForm projectForm = new ProjectForm(_dataStore.Projects, _selectedProjectId);
            
            // attach the event listener, SaveAndReload, to the DoSave event 
            projectForm.DoSave += SaveAndReload;
            projectForm.Show();
        }

        // When an item in the listbox is clicked get the id for editing and deleting
        private void lstProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListBox projListBox = sender as ListBox;
                Project selectedProject = projListBox.SelectedItem as Project;
                _selectedProjectId = selectedProject.Id;
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private bool isSure(string item)
        {
            string message = "Are you sure you want to delete this " + item + "?";
            string caption = "Delete " + item;
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // the MessageBox can return different values depending
            // on what button is clicked
            if (result == DialogResult.No)
            {
                return false;
            }
            return true;
        }
    }
}
