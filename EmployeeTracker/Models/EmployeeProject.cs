using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTracker.Models
{
    public class EmployeeProject
    {
        // declare public properties
        public int Id { get; set; }
        public DateTime OnProjectDate { get; set; }
        public DateTime? OffProjectDate { get; set; }
    }
}
