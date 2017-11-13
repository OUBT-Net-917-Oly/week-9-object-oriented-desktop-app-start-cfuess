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
        
        // the current employee being added or edited
        Employee _employee;

        private bool _adding;

        // the delegate is the siginature that is used when the event is raised.  
        // the ShowResults event and will use that to define the parameters it will raise
        // to the subscriber
        delegate void SetText(string message);

        // used by the caller to save the employee data
        public delegate void SaveEmployeeHandler(Employee employee, bool add);

        // event object: raised in code to signal to 
        // the listeners or subscribers 
        // attach a listener to handle the event
        event SetText ShowResults;

        // public event that will notifiy the callers to save
        // the data
        public event SaveEmployeeHandler DoSave;

        public EmployeeForm(Employee employee, bool add)
        {
            // set the priviate project object (class level scope)
            // to the items passed in
            _employee = employee;
            _adding = add;

            // call loading controls in the designer 
            // partial class
            InitializeComponent();

            // the results_ShowResults is a subscriber to the ShowResults event
            ShowResults += Results_ShowResults;

            txtId.Text = _employee.Id.ToString();

            if (!add)
            {
                LoadExistingEmployeeControls();
            }
        }

        public void LoadExistingEmployeeControls() 
        {
            txtFirst.Text = _employee.FirstName;
            txtLast.Text = _employee.LastName;
            dtHireDate.Text = _employee.HireDate.ToString();
        }

        // this is the method that subscribes to the ShowResults event
        private void Results_ShowResults(string message)
        {
            lblResult.Text = message;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            _employee.Id = int.Parse(txtId.Text);
            _employee.FirstName = txtFirst.Text;
            _employee.LastName = txtLast.Text;
            _employee.HireDate = DateTime.Parse(dtHireDate.Text);
            if (DoSave != null) DoSave(_employee, _adding);
            _adding = false;  // take it out of edit mode after the first save
            ShowResults("Saved");
        }

        // whenenver a form field is modified raise the ShowResults
        // event method to clear the results label
        private void TxtLast_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void TxtFirst_TextChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }

        private void DtHireDate_ValueChanged(object sender, EventArgs e)
        {
            ShowResults("");
        }
    }
}
