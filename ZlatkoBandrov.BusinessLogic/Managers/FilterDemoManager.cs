using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatkoBandrov.DataAccess;
using ZlatkoBandrov.DataAccess.Repositories;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class FilterDemoManager : IDisposable
    {
        private readonly FilterDemoUnitOfWork UnitOfWork = new FilterDemoUnitOfWork();

        public FilterDemoManager()
        {
        }

        public List<EmployeeTableItem> GetEmployeeTableItems()
        {
            var list = new List<EmployeeTableItem>();

            List<int> employeeIDs = UnitOfWork.EmployeeArrivalRepository.AsQuery().Select(p => p.EmployeeID).Distinct().ToList();
            List<Employee> employeesList = UnitOfWork.EmployeeRepository.AsQuery().Where(e => employeeIDs.Contains(e.ID)).ToList();
            EmployeeTableItem newEmployeeTableItem = new EmployeeTableItem();

            foreach (var employee in employeesList)
            {
                newEmployeeTableItem.EmployeeID = employee.ID;
                newEmployeeTableItem.Age = employee.Age;
                newEmployeeTableItem.Email = employee.Email;
                newEmployeeTableItem.FullName = string.Format("{0} {1}", employee.Name, employee.Surename);

                if (employee.Role != null)
                {
                    newEmployeeTableItem.Role = employee.Role.DisplayName;
                }

                if (employee.EmployeeTeams != null && employee.EmployeeTeams.Any())
                {
                    newEmployeeTableItem.Teams = string.Join(", ", employee.EmployeeTeams.Select(t => t.Team.DisplayName).ToArray());
                }

                list.Add(newEmployeeTableItem);
            }

            return list;
        }

        #region Dispose Pattern

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UnitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
