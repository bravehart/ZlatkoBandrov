using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Collections.Generic;
using ZlatkoBandrov.Common;
using ZlatkoBandrov.DataAccess;
using ZlatkoBandrov.DataAccess.Repositories;
using ZlatkoBandrov.Entities.Models;
using ZlatkoBandrov.BusinessLogic.Converters;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class ArrivalTrackerManager : IDisposable
    {
        private readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public static TokenData CurrentToken { get; set; }

        public void InitializeCommunication()
        {
            string callbackUrl = Utils.GetArrivalTrackerUrl();
            string trackingDate = DateTime.Now.Date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string subscribeBaseUrl = Utils.GetConfigSetting("SubscribeBaseUrl");

            string subscribeEntryPointUrl = Utils.GetConfigSetting("SubscribeEntryPointUrl");
            subscribeEntryPointUrl = string.Format(subscribeEntryPointUrl, trackingDate, callbackUrl);

            //EmployeeManager employeeManager = new EmployeeManager();
            //employeeManager.UpdateAllEmployees();

            // Execute the subscribtion
            MakeSubscribtion(subscribeBaseUrl, subscribeEntryPointUrl);
        }

        public bool ValidateToken(string token)
        {
            bool tokenIsValid = true;
            if (CurrentToken == null || string.IsNullOrEmpty(CurrentToken.Token) || string.IsNullOrEmpty(token))
            {
                tokenIsValid = false;
            }
            else if (string.Compare(CurrentToken.Token, token, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                tokenIsValid = false;
            }
            else if (CurrentToken.Expires < DateTime.Now)
            {
                tokenIsValid = false;
            }
            return tokenIsValid;
        }

        public void TrackArrivals(EmployeeArrivalData[] data)
        {
            try
            {
                if (data == null || !data.Any())
                {
                    return;
                }

                foreach (EmployeeArrivalData arrivalData in data)
                {
                    var employeeArrival = DAOConverter.ConvertFromEmployeeArrivalData(arrivalData);
                    // If employeeArrival.ArrivalTime is equal to DateTime.MinValue then the conversion has been failed
                    if (employeeArrival != null && employeeArrival.ArrivalTime > DateTime.MinValue)
                    {
                        // This will get or create new employee in the database
                        Employee employee = GetEmployeeOrCreate(arrivalData.EmployeeId);
                        if (employee != null)
                        {
                            if (employee.ID > 0)
                            {
                                employeeArrival.EmployeeID = employee.ID;
                            }
                            else
                            {
                                employeeArrival.Employee = employee;
                            }
                            UnitOfWork.EmployeeArrivalRepository.Add(employeeArrival);
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

        public Employee GetEmployeeOrCreate(int employeeId)
        {
            Employee employee = UnitOfWork.EmployeeRepository.Get(e => e.ExternalID == employeeId);
            if (employee == null)
            {
                var employeeManager = new EmployeeManager();
                var allEmployees = employeeManager.GetAllEmployees();
                if (allEmployees != null && allEmployees.Any())
                {
                    JsonEmployee jsonEmployee = allEmployees.FirstOrDefault(e => e.Id == employeeId);
                    employee = CreateNewEmployee(jsonEmployee);
                }
            }

            return employee;
        }

        public Employee CreateNewEmployee(JsonEmployee jsonEmployee)
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

            List<Team> employeeTeams = GetEmployeeTeams(jsonEmployee.Teams);
            if (employeeTeams != null)
            {
                var teamsList = employeeTeams.Select(t => new EmployeeTeam { Employee = newEmployee, TeamID = t.ID }).ToList();
                newEmployee.EmployeeTeams = teamsList;
            }

            Role role = GetEmployeeRole(jsonEmployee.Role);
            if (role != null)
            {
                newEmployee.RoleID = role.ID;
            }

            return newEmployee;
        }


        private void MakeSubscribtion(string subscribeBaseUrl, string subscribeEntryPointUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(subscribeBaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Accept-Client", "Fourth-Monitor");

                HttpResponseMessage response = httpClient.GetAsync(subscribeEntryPointUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    CurrentToken = Utils.FromTokenResult(response.Content.ReadAsAsync<TokenResult>().Result);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    CurrentToken = null;
                }
            }
        }

        private List<Team> GetEmployeeTeams(List<string> employeeTeams)
        {
            if (employeeTeams == null || !employeeTeams.Any())
            {
                return null;
            }

            List<Team> teams = UnitOfWork.TeamRepository.AsQuery(t => employeeTeams.Contains(t.DisplayName)).ToList();

            return teams;
        }

        private Role GetEmployeeRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            Role employeeRole = UnitOfWork.RoleRepository.Get(r => r.DisplayName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            return employeeRole;
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
