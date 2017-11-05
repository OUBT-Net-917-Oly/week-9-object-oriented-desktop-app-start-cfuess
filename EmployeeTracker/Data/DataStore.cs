using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeTracker.Models;

namespace EmployeeTracker.Data
{

    // The datastore object contains all the lists and
    // relationships used by the app
    public class DataStore
    {
        // declare public properties
        public List<Employee> Employees { get; set; }
        public List<Project> Projects { get; set; }

        public DataStore()
        {
            // instantiate the list of employees and projcets in the constructor  
            // so whenever we create the DataStore object, i.e., new it up, we 
            // will already have Employeee and Project objects not just variables.  
            Employees = new List<Employee>();
            Projects = new List<Project>();
        }
    }
}
