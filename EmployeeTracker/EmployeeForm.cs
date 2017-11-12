
using EmployeeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeTracker
{
    public partial class EmployeeForm : Form
    {
        // delecrations of private class scope varialbes

        // list of employees
        List<Employee> _employees;
        
        // the current employee being added or edited
        Employee _employee;

        bool _adding = true; //this is a flag used when saving, default it to true

        // the delegate is the siginature that is used when the event is raised.  
        // the ShowResults event and will use that to define the parameters it will raise
        // to the subscriber
        delegate void SetText(string message);

        // event object: raised in code to signal to 
        // the listeners or subscribers 
        // attach a listener to handle the event
        event SetText ShowResults;

        // public event that will notifiy the callers to save
        // the data
        public event EventHandler DoSave;

        // default class constructor,
        // happens when you "new up" an object
        public EmployeeForm(List<Employee> employees)
        {
            // setting the intial state of all the 
            // class level objects

            // set the priviate project object (class level scope)
            // to the list passed in
            _employees = employees;

            _employee = new Employee();  // instanciate the employee object

            // call loading controls in the designer 
            // partial class
            InitializeComponent();

            // set the id, will get written over if it is the edit 
            // constructor calling this constructor
            txtId.Text = GetNextId().ToString();

            // the results_ShowResults is a subscriber to the ShowResults event
            ShowResults += results_ShowResults;

            // raise the event to clear the label
            ShowResults("");
        }

        // this is an overloaded constructor - : this(employees) 
        // means call the other constructor then finish by 
        // running the statements in this method
        public EmployeeForm(List<Employee> employees, int id) 
            : this(employees)
        {
            // _employee was already intanciated in the default constructor
            // but we want to set it to the one we want to edit
            _employee = _employees.FirstOrDefault(e => e.Id == id);

            _adding = false;  // editing not adding
            
            //set the controls
            txtId.Text = _employee.Id.ToString();
            txtFirst.Text = _employee.FirstName;
            txtLast.Text = _employee.LastName;
            dtHireDate.Text = _employee.HireDate.ToString();
        }

        // this is the method that subscribes to the ShowResults event
        private void results_ShowResults(string message)
        {
            lblResult.Text = message;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _employee.Id = int.Parse(txtId.Text);
            _employee.FirstName = txtFirst.Text;
            _employee.LastName = txtLast.Text;
            _employee.HireDate = DateTime.Parse(dtHireDate.Text);
            if (_adding == true)
            {
                _employees.Add(_employee);
                _adding = false;  // good idea Cooper, take it out of edit mode after the first save
            }
            ShowResults("Saved");
            DoSave(this, EventArgs.Empty);
        }

        // whenenver a form field is modified raise the ShowResults
        // event method to clear the results label
        private void txtLast_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void txtFirst_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void dtHireDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        /// <summary>
        /// This will get the next unused id in a list of projects 
        /// </summary>
        /// <returns>int max + 1</returns>
        public int GetNextId()
        {
            if (!_employees.Any())
            {
                return 0;
            }
            else
            {
                // var stores = _projects.Where(p => p.Name == "grocery");
                return _employees.Max(p => p.Id) + 1;
            }
        }
    }
}
