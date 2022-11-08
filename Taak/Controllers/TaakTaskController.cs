using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Libraries.SearchFilters;
using Taak.Models;
using Taak.Repository;
using Taak.ViewModels;

namespace Taak.Controllers
{
    public class TaakTaskController : Controller
    {
        private readonly TaakTaskRepository taakTaskRepository;
        private readonly TaskCategoryRepository taskCategoryRepository;
        private readonly CustomerRepository customerRepository;
        private readonly OfferRepository offerRepository;
        private readonly TasksWorkerRepository taskWorkerRepository;
        private readonly CitiesByCountyRepository citiesByCountyRepository;
        public TaakTaskController(ApplicationDbContext db)
        {
            this.taakTaskRepository = new TaakTaskRepository(db);
            this.taskCategoryRepository = new TaskCategoryRepository(db);
            this.customerRepository = new CustomerRepository(db);
            this.offerRepository = new OfferRepository(db);
            this.taskWorkerRepository = new TasksWorkerRepository(db);
            this.citiesByCountyRepository = new CitiesByCountyRepository(db);
        }
        //GET METHOD searcheable index by anyone visitors inclusive
        public ActionResult SearchTaskIndex()
        {
                                                                                
            var tasks = new TasksWithSearchFiltersViewModel(taskCategoryRepository, taakTaskRepository,citiesByCountyRepository);
            tasks.SearchBudgetMin = tasks.BudgetMin;
            tasks.SearchBudgetMax = tasks.BudgetMax;
            if (User.IsInRole("Worker"))
            {
                var idUser = HttpContext.Session.GetString("UserId");
                tasks.Tasks = TasksFilters.FilterByWorkerWithNoOffer(tasks.Tasks, offerRepository, taskWorkerRepository, idUser);
            }
            return View(tasks);
        }
        //POST method searcheable index by anyone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTaskIndex(IFormCollection collection)
        {
            decimal x;
            var tasksFiltered = new TasksWithSearchFiltersViewModel(taskCategoryRepository, taakTaskRepository, citiesByCountyRepository);
            tasksFiltered.SearchCategory = collection["SearchCategory"];
            tasksFiltered.SearchLocation = collection["SearchLocation"];
            tasksFiltered.SearchBudgetMin = decimal.TryParse(collection["SearchBudgetMin"], out x) ? x : 0;
            tasksFiltered.SearchBudgetMax = decimal.TryParse(collection["SearchBudgetMax"], out x)? x : 0;

            if (User.IsInRole("Worker"))
            {
                var idUser = HttpContext.Session.GetString("UserId");
                tasksFiltered.Tasks = TasksFilters.FilterByWorkerWithNoOffer(tasksFiltered.Tasks,offerRepository,taskWorkerRepository,idUser);
            }
           
            tasksFiltered.Tasks = TasksFilters.FilterByCategoryByCityByBudget(tasksFiltered.Tasks,
                                                                              tasksFiltered.SearchCategory,
                                                                              tasksFiltered.SearchLocation,
                                                                              tasksFiltered.SearchBudgetMin,
                                                                              tasksFiltered.SearchBudgetMax,
                                                                              tasksFiltered.BudgetMin,
                                                                              tasksFiltered.BudgetMax);
            
            return View("SearchTaskIndex", tasksFiltered);
        }

        // GET: TaakTaskController
        //access to all of the tasks is granted only to admin
        [Authorize(Roles="Admin")]
        public ActionResult Index(Guid userId)
        {
            List<TaakTaskModel> taakTasks = taakTaskRepository.GetAll().ToList<TaakTaskModel>();

            return View(taakTasks);
        }
      

        //get method -- returns taakTasksByUser
        [Authorize(Roles="Customer")]
        public ActionResult IndexByUser()
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var idCustomer = customerRepository.GetCustomerId(idUser);
            var taakTasks = taakTaskRepository.GetAll().Where(task => task.IdCustomer == idCustomer);
            
            return View(taakTasks);
        }


        // GET: TaakTaskController/Details/5
        [Authorize(Roles ="Customer,Admin")]
        public ActionResult Details(Guid id)
        {
            var taakTask = taakTaskRepository.GetById(id);
            var taakTaskViewModelIndex = new TaakTaskViewModelIndex(taakTask, taskCategoryRepository, customerRepository);
            if(taakTask == null)
            {
                if (User.IsInRole("Customer"))
                {
                    return RedirectToAction("IndexByUser");
                }
                return RedirectToAction("Index");
            }
            return View(taakTaskViewModelIndex);
        }
        [Authorize(Roles ="Customer")]
        // GET: TaakTaskController/Create
        public ActionResult Create()
        {
            var taakTaskViewModelCreate = new TaakTaskViewModelCreate(taskCategoryRepository);
            
            return View(taakTaskViewModelCreate);
        }
        [Authorize(Roles ="Customer")]
        // POST: TaakTaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new TaakTaskModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    //here we set the model.IdCustomer and also the model.IdTaaktask
                    var idUser = HttpContext.Session.GetString("UserId");
                    var idCustomer = customerRepository.GetCustomerId(idUser);
                    model.IdTask = Guid.NewGuid();
                    model.IdCustomer = idCustomer;
                    taakTaskRepository.Insert(model);
                    TempData["succes"] = "task created successfully";
                    return RedirectToAction("IndexByUser");
                }
                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        // GET: TaakTaskController/Edit/5
        [Authorize(Roles ="Customer")]
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var taakTask = taakTaskRepository.GetById(id);
            
            return View(taakTask);
        }

        // POST: TaakTaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Customer")]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new TaakTaskModel();
                var task=TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    taakTaskRepository.Update(model,model.IdTask);
                    return RedirectToAction("Index");
                }
                return View("Edit");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: TaakTaskController/Delete/5
        [Authorize(Roles ="Customer,Admin")]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                if (User.IsInRole("Customer"))
                {
                    return RedirectToAction("IndexByUser");
                }
                return RedirectToAction("Index");
               
                
            }
            taakTaskRepository.Delete(id);
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("IndexByUser");
            }
            return RedirectToAction("Index");
        }

      
    }
}
