using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZlatkoBandrov.BusinessLogic.Managers;
using ZlatkoBandrov.WebApp.Models;

namespace ZlatkoBandrov.WebApp.Controllers
{
    public class FilterAndSearchController : Controller
    {
        public ActionResult Index()
        {
            var model = GetModel();

            return View(model);
        }

        private FilterAndSearchModel GetModel()
        {
            var manager = new FilterDemoManager();
            var model = new FilterAndSearchModel();
            model.EmployeeDataItems = manager.GetEmployeeTableItems();

            return model;
        }
    }
}