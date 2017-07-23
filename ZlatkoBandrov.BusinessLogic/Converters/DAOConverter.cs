using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatkoBandrov.Common;
using ZlatkoBandrov.DataAccess;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Converters
{
    public class DAOConverter
    {
        public static Employee ConvertFromJsonEmployee(JsonEmployee jsonEmployee)
        {
            if (jsonEmployee == null)
            {
                return null;
            }

            Employee newEmployee = new Employee
            {
                ExternalID = jsonEmployee.Id,
                Age = jsonEmployee.Age,
                Email = jsonEmployee.Email,
                ManagerID = jsonEmployee.ManagerId,
                Name = jsonEmployee.Name,
                Surename = jsonEmployee.SurName
            };

            return newEmployee;
        }

        public static EmployeeArrival ConvertFromEmployeeArrivalData(EmployeeArrivalData arrivalData)
        {
            if (arrivalData == null)
            {
                return null;
            }

            DateTime arrivalTime = Utils.ParseDateFromUniversal(arrivalData.When);
            EmployeeArrival employeeArrival = new EmployeeArrival
            {
                ArrivalTime = arrivalTime
            };

            return employeeArrival;
        }
    }
}
