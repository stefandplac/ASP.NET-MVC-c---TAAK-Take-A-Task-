using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;

namespace Taak.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository customerRepository;
        public CustomerController(ApplicationDbContext db)
        {
            customerRepository = new CustomerRepository(db);
        }
        // GET: CustomerController
        public ActionResult Index()
        {

            List<CustomerModel> customers = customerRepository.GetAll().ToList();
            
            
            return View(customers);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(Guid id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new CustomerModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    model.IdCustomer = Guid.NewGuid();
                    customerRepository.Insert(model);
                    TempData["success"] = "customer creation succeeded";
                    return RedirectToAction("Index");
                }

                return View("Create");
            }
            catch
            {

                return RedirectToAction("Create");
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                
                return NotFound();
            }

            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new CustomerModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    customerRepository.Insert(model);
                    TempData["success"] = "Update succeeded";
                    return RedirectToAction("Index");
                }
                TempData["error"] = "updating customer failed";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                TempData["error"] = "Customer Deletion failed";
                return RedirectToAction("Index");
            }
            customerRepository.Delete(customer, id);
            TempData["success"] = "Customer deletion succeeded";
            return RedirectToAction("Index");
        }

       

    }
}
