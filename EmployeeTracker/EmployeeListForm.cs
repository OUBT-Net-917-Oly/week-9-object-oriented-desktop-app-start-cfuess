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
            LoadEmployeeList();
            LoadProjectList();
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            var employee = new Employee();
            employee.Id = _dataStore.GetNextEmployeeId();

            // indicate that we are saving via the true add parameter
            EmployeeForm employeeForm = new EmployeeForm(employee, true);
            
            // attach the event listener, SaveEmployeeAndReload, to the DoSave event 
            employeeForm.DoSave += SaveEmployeeAndReload;
            employeeForm.Show();
        }

        private void SaveAndReload(object sender, EventArgs e)
        {
            _fileManager.Save(_dataStore);
            LoadEmployeeList();
            LoadProjectList();
        }

        private void SaveEmployeeAndReload(Employee sender, bool add)
        {
            if (add)
            {
                _dataStore.Employees.Add(sender);
            }
            _fileManager.Save(_dataStore);
            LoadEmployeeList();
        }

        private void LoadEmployeeList()
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

        private void LoadProjectList()
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

        private void BtnAddProject_Click(object sender, EventArgs e)
        {
            ProjectForm projectForm = new ProjectForm(_dataStore.Projects);
            // attach the event listener, SaveAndReload, to the 
            // project form's DoSave event, the code in SaveAndReload 
            // will get executed when the event is raised
            projectForm.DoSave += SaveAndReload;
            projectForm.Show();
        }

        // When an item in the listbox is clicked get the id for editing and deleting
        private void LstEmployees_SelectedIndexChanged(object sender, EventArgs e)
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

        private void BtnEditEmployee_Click(object sender, EventArgs e)
        {
            var employee = _dataStore.Employees.Single(proj => proj.Id == _selectedEmployeeId);
            
            // call the constructor, indicate edit with the false parameter
            EmployeeForm employeeForm = new EmployeeForm(employee, false);

            // attach the event listener, SaveEmployeeAndReload, to the DoSave event 
            employeeForm.DoSave += SaveEmployeeAndReload;
            employeeForm.Show();
        }

        private void BtnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (!IsSure("Employee"))
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

        private void BtnProjectDelete(object sender, EventArgs e)
        {
            if (!IsSure("Project"))
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

        private void BtnEditProject_Click(object sender, EventArgs e)
        {
            // call the constructor that will initiate an edit instead of an add
            ProjectForm projectForm = new ProjectForm(_dataStore.Projects, _selectedProjectId);
            
            // attach the event listener, SaveAndReload, to the DoSave event 
            projectForm.DoSave += SaveAndReload;
            projectForm.Show();
        }

        // When an item in the listbox is clicked get the id for editing and deleting
        private void LstProjects_SelectedIndexChanged(object sender, EventArgs e)
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

        private bool IsSure(string item)
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
