using System.Web.Mvc;
using ZlatkoBandrov.BusinessLogic.Managers;
using ZlatkoBandrov.Entities.Models;
using ZlatkoBandrov.WebApp.Models;
using ZlatkoBandrov.BusinessLogic.Extensions.EmployeeTableListExtensions;

namespace ZlatkoBandrov.WebApp.Controllers
{
    public class FilterAndSearchController : Controller
    {
        public ActionResult Index()
        {
            var model = GetModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Search(SearchWithOrderModel searchModel)
        {
            var viewModel = GetModel(searchModel);
            return PartialView("FilterAndSearch/_RenderEmployeeArrivalsPartial", viewModel);
        }

        private FilterAndSearchModel GetModel(SearchWithOrderModel searchModel = null)
        {
            var manager = new FilterDemoManager();
            SearchWithOrderModel filter = manager.ValidateFilter(searchModel);
            SearchWithOrderModel orderData = manager.ValidateSorting(searchModel);

            var model = new FilterAndSearchModel();
            model.EmployeeDataItems = manager.GetEmployeeTableItems().WithFilter(filter).WithOrder(orderData);

            return model;
        }
    }
}