using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.WebApp.Models
{
    public class FilterAndSearchModel
    {
        public int SelectedEmployeeID { get; set; }

        public List<EmployeeTableItem> EmployeeDataItems { get; set; }
    }
}