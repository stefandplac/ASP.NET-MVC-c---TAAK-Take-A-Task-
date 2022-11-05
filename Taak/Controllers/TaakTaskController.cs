using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Libraries;
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
                                                                                
            var indexSearcheable = new IndexSearcheable(taskCategoryRepository, taakTaskRepository,citiesByCountyRepository);
            return View(indexSearcheable);
        }
        //POST method searcheable index by anyone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchTaskIndex(IFormCollection collection)
        {
            var cat = collection["SearchCategory"];
            var budget = collection["SearchBudget"];
            var city = collection["SearchCity"];
            var indexSearcheable = new IndexSearcheable(taskCategoryRepository, taakTaskRepository, citiesByCountyRepository);
            indexSearcheable.SearchCategory = cat;

            if (!String.IsNullOrEmpty(cat)&&cat!="All")
            {
                indexSearcheable.Tasks = TasksFilters.FilterTasksByCategory(indexSearcheable.Tasks,cat);
            }
            //if (!String.IsNullOrEmpty(budget))
            //{

            //}
            //if (!String.IsNullOrEmpty(city))
            //{

            //}



            return View("SearchTaskIndex", indexSearcheable);
        }

        // GET: TaakTaskController
        //access to all of the tasks is granted only to admin
        [Authorize(Roles="Admin")]
        public ActionResult Index(Guid userId)
        {
            List<TaakTaskModel> taakTasks = taakTaskRepository.GetAll().ToList<TaakTaskModel>();

            return View(taakTasks);
        }
        public ActionResult IndexByTaskWorker()
        {
            //display only those tasks for which a taskWorker did not made an offer yet
            var idUser = HttpContext.Session.GetString("UserId");
            var idTaskWorker = taskWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;
            var offers = offerRepository.GetAll().Where(offer => offer.IdTaskWorker == idTaskWorker).Select(offer => new { offer.IdTask });
            var taakTasks = taakTaskRepository.GetAll();
            var tasksWithNoOffer = taakTasks.Except(
                                             taakTasks.Join(offers,
                                                            t=>t.IdTask,
                                                            o=>o.IdTask,
                                                            (t,o)=>t )   
                                            );

            return View(tasksWithNoOffer);
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
