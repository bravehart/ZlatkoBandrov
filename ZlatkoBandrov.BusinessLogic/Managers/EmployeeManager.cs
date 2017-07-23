using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ZlatkoBandrov.DataAccess;
using ZlatkoBandrov.DataAccess.Repositories;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class EmployeeManager
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public IList<JsonEmployee> GetAllEmployees()
        {
            IList<JsonEmployee> results = null;
            string filePath = HttpContext.Current.Server.MapPath("~/Data/employees.json");
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }
            using (var reader = new StreamReader(filePath))
            {
                string fileContent = reader.ReadToEnd();
                results = JsonConvert.DeserializeObject<IList<JsonEmployee>>(fileContent);
            }

            return results;
        }

        public void UpdateAllEmployees()
        {
            try
            {
                ArrivalTrackerManager arrivalTracker = new ArrivalTrackerManager();
                var employeeList = GetAllEmployees();
                foreach (var employeeItem in employeeList)
                {
                    Employee employee = UnitOfWork.EmployeeRepository.Get(e => e.ExternalID == employeeItem.Id);
                    if (employee == null)
                    {
                        employee = arrivalTracker.CreateNewEmployee(employeeItem);
                        if (employee != null)
                        {
                            UnitOfWork.EmployeeRepository.Add(employee);
                        }
                    }
                }

                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                UnitOfWork.Rollback();
            }
        }
    }
}