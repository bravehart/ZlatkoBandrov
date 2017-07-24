using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatkoBandrov.Entities.Models
{
    public class EmployeeTableItem
    {
        public EmployeeTableItem()
        {
            Arrivals = new List<EmployeeArrivalListItem>();
        }

        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Teams { get; set; }
        public int Age { get; set; }

        public List<EmployeeArrivalListItem> Arrivals { get; set; }
    }
}
