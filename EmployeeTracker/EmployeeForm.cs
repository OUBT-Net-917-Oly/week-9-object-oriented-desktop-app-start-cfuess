
using EmployeeTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeTracker
{
    public partial class EmployeeForm : Form
    {
        List<Employee> _employees;
        Employee _employee;
        bool _adding = true; //this is a flag used when saving

        // the delegate is the siginature that is used when the event is raised.  
        // the ShowResults event and will use that to define the parameters it will raise
        // to the subscriber
        delegate void SetText(string message);
        event SetText ShowResults;

        // public event that will notifiy the callers when save is done
        public event EventHandler SaveComplete;

        public EmployeeForm(List<Employee> employees)
        {
            InitializeComponent();
            _employees = employees;
            _employee = new Employee();  // instanciate the employee object
            // the results_ShowResults is a subscriber to the ShowResults event
            ShowResults += results_ShowResults;
            ShowResults("");
            txtId.Text = GetNextId().ToString();
        }

        // : this(employees) means call the other constructor then finish by running
        // the statements in this method
        public EmployeeForm(List<Employee> employees, int id) 
            : this(employees)
        {
            // _employee was already intanciated in the default constructor
            // but we want to set it to the one we want to edit
            _employee = _employees.FirstOrDefault(e => e.Id == id);
            _adding = false;  // editing not adding
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
            }
            ShowResults("Saved");
            SaveComplete(this, EventArgs.Empty);
        }

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
