using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Managers
{
    public class EmployeeManager
    {
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
    }
}