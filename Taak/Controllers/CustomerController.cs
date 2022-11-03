using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;

namespace Taak.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository customerRepository;
        private UserManager<IdentityUser> _userManager;
        public CustomerController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            customerRepository = new CustomerRepository(db);
            _userManager = userManager;
        }
        // GET: CustomerController
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            
            List<CustomerModel> customers = customerRepository.GetAll().ToList();
            
            
            return View(customers);
        }

        // GET: CustomerController/Details/5
        [Authorize(Roles ="Customer,Admin")]
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
       
        public ActionResult Create(string userId)
        {
            

            var customerId = Guid.NewGuid();
            var model = new CustomerModel()
            {
                IdCustomer = customerId,
                UserId = userId,
                Name = "not set yet",
                City = "not set yet",
                Street = "not set yet",
                Building = "not set yet",
                Country = "not set yet",
                County = "not set yet",
                Phone = "0000000000",
            };
            customerRepository.Insert(model);
            ViewBag.CustomerId=customerId;
            ViewBag.UserId = userId;
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
                    customerRepository.Update(model,model.IdCustomer);
                    TempData["success"] = "customer creation succeeded";
                    
                    return RedirectToAction("Index");
                }

                return View("Create");
            }
            catch
            {

                return View("Create");
            }
        }

        // GET: CustomerController/Edit/5
        [Authorize(Roles ="Customer")]
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
        [Authorize(Roles ="Customer")]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new CustomerModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    customerRepository.Update(model,model.IdCustomer);
                    TempData["success"] = "Update succeeded";
                    return RedirectToAction("CustomerProfile");
                }
                TempData["error"] = "updating customer failed";

                return View("Edit");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: CustomerController/Delete/5
        [Authorize(Roles ="Customer,Admin")]
        public  ActionResult Delete(Guid id)
        {
            //deleting a profile means deleting the entire user data
            //var idUser = HttpContext.Session.GetString("UserId");
            if (id == null)
            {
                TempData["error"] = "Customer Deletion failed";
                return RedirectToAction("Index");
            }
            //customerRepository.Delete( id);
            //var user=await _userManager.FindByIdAsync(idUser);
            //await _userManager.DeleteAsync(user);
            TempData["success"] = "Customer profile user deletion succeeded";
            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Customer")]
        public ActionResult CustomerProfile()
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var user = customerRepository.GetCustomerByUserId(idUser);
            
            return View(user);
        }
       

    }
}
