using System.Linq;
using System.Web.Http;
using ZlatkoBandrov.BusinessLogic.Managers;
using ZlatkoBandrov.Common;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.WebApp.Controllers
{
    public class ArrivalTrackerController : ApiController
    {
        [HttpPost]
        [Route("api/arrivaltracker/trackemployeearrival")]
        public IHttpActionResult TrackEmployeeArrival([FromBody]EmployeeArrivalData[] employeeArrivals)
        {
            if (employeeArrivals == null || !employeeArrivals.Any())
            {
                return BadRequest("Employees arrival list is empty!");
            }

            // Get the request token and validate it with the current token
            string requestToken = Utils.GetRequestToken(Request);
            var manager = new ArrivalTrackerManager();
            if (!manager.ValidateToken(requestToken))
            {
                return Unauthorized();
            }

            // Save the employee arrivals in the database
            manager.TrackArrivals(employeeArrivals);
                
            return Ok();
        }
    }
}
