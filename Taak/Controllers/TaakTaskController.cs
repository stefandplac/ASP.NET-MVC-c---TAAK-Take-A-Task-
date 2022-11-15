using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Libraries.Constants;
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
            //first time and only time get method is called will return default pagination
            var pageSize = 5;
            double result = tasks.Tasks.Count() / pageSize;
            ViewBag.Pages = tasks.Tasks.Count() % pageSize != 0 ? result + 1 : result;
            ViewBag.CurrentPageNo = 1;
            ViewBag.PrevPage = 1;
            ViewBag.NextPage = 1 == ViewBag.Pages ? 1 : 2;
            tasks.Tasks = tasks.Tasks.Take(pageSize).ToList();

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
            var pageNo = collection["PageNumber"];

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
            //pagination filter by pagesize
            
            var pageNumber = Int32.TryParse(pageNo, out int z) ? z : 1;
            var pageSize = 5;
            double result = tasksFiltered.Tasks.Count() / pageSize;
            ViewBag.Pages = tasksFiltered.Tasks.Count() % pageSize != 0 ? result + 1 : result;
            ViewBag.CurrentPageNo = pageNumber;
            ViewBag.PrevPage = pageNumber == 1 ? 1 : pageNumber - 1;
            ViewBag.NextPage = pageNumber == ViewBag.Pages ? pageNumber : pageNumber + 1;

            tasksFiltered.Tasks = tasksFiltered.Tasks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
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
        public ActionResult IndexByUser(string pageno)
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var idCustomer = customerRepository.GetCustomerId(idUser);
            var taakTasks = taakTaskRepository.GetAll().Where(task => task.IdCustomer == idCustomer);
            taakTasks = TasksFilters.SortByMostRecent(taakTasks);
            
            ViewBag.TimeFrames = Constants.TimeFrames;
            ViewBag.TimeFramesIcons = Constants.TimeFramesIcons;

            //pagination
            var pageNumber = Int32.TryParse(pageno, out int x) ? x : 1;
            var pageSize = 5;
            double result = taakTasks.Count() / pageSize;
            ViewBag.Pages = taakTasks.Count() % pageSize != 0 ? result + 1 : result;
            ViewBag.CurrentPageNo = pageNumber;
            ViewBag.PrevPage = pageNumber == 1 ? 1 : pageNumber - 1;
            ViewBag.NextPage = pageNumber == ViewBag.Pages ? pageNumber : pageNumber + 1;

            taakTasks = taakTasks.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            
            return View(taakTasks);
        }
        //only GET method for this action
        [Authorize(Roles = "Customer")]
        public ActionResult TaskWithAllOffers(Guid idTask)
        {
            var taakTask = taakTaskRepository.GetById(idTask);
            var offers = offerRepository.GetAllOffersByTask(idTask);
            List<OfferWithTaskWorkerDataViewModel> offersWithWorkerData = new List<OfferWithTaskWorkerDataViewModel>();
            foreach(var offer in offers)
            {
                var offerWithWorkerDetails = new OfferWithTaskWorkerDataViewModel(offer,taskWorkerRepository);
                offersWithWorkerData.Add(offerWithWorkerDetails);
            }
            
            var taakTaskWithOffer = new TaakTaskWithOffersViewModel(taakTask, offersWithWorkerData);
            
            //we want also to add the accepted offer to the model if exists - to present it individually without looping the list
            var acceptedOffer = offerRepository.ReturnAcceptedOffer(idTask);
            OfferWithTaskWorkerDataViewModel offerWithWorkerData;
            if (acceptedOffer != null)
            {
                offerWithWorkerData = new OfferWithTaskWorkerDataViewModel(acceptedOffer, taskWorkerRepository);
                taakTaskWithOffer.AcceptedOffer = offerWithWorkerData;
            }
            ViewBag.TimeFrames = Constants.TimeFrames;
            ViewBag.TimeFramesIcons = Constants.TimeFramesIcons;

            return View(taakTaskWithOffer);
        }


        // GET: TaakTaskController/Details/5
        [Authorize(Roles ="Customer")]
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
            var taakTaskViewModelCreate = new TaakTaskViewModelCreate(taskCategoryRepository, citiesByCountyRepository);
            var idUser = HttpContext.Session.GetString("UserId");
            var idCustomer = customerRepository.GetCustomerId(idUser);
            taakTaskViewModelCreate.IdCustomer = idCustomer;
            taakTaskViewModelCreate.Country = "Romania";

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
                             
                    model.IdTask = Guid.NewGuid();
                    taakTaskRepository.Insert(model);
                    TempData["succes"] = "task created successfully";
                    return RedirectToAction("IndexByUser");
                }
                var taakTaskViewModelCreate = new TaakTaskViewModelCreate(taskCategoryRepository, citiesByCountyRepository);
                
                return View("Create", taakTaskViewModelCreate);
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
            var taskToEdit= new TaakTaskViewModelEdit(taskCategoryRepository,citiesByCountyRepository,taakTask);
            
            return View(taskToEdit);
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
                    return RedirectToAction("IndexByUser");
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
            //remove all entries from offer table that have as foreign key the deleted IdTask
            var offers = offerRepository.GetAll().Where(offer => offer.IdTask == id);
            offerRepository.DeleteRange(offers);
            taakTaskRepository.Delete(id);
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("IndexByUser");
            }
            return RedirectToAction("Index");
        }

      
    }
}
