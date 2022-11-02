using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;
using Taak.ViewModels;

namespace Taak.Controllers
{
    public class TaakTaskController : Controller
    {
        private readonly TaakTaskRepository taakTaskRepository;
        private readonly TaskCategoryRepository taskCategoryRepository;
        public TaakTaskController(ApplicationDbContext db)
        {
            this.taakTaskRepository = new TaakTaskRepository(db);
            this.taskCategoryRepository = new TaskCategoryRepository(db);
        }
        // GET: TaakTaskController
        public ActionResult Index(Guid userId)
        {
            List<TaakTaskModel> taakTasks = taakTaskRepository.GetAll().ToList<TaakTaskModel>();

            return View(taakTasks);
        }

        //get method -- returns taakTasksByUser
        public ActionResult IndexByUser()
        {
            var idUser = HttpContext.Session.GetString("UserId");
            
            return View();
        }


        // GET: TaakTaskController/Details/5
        public ActionResult Details(Guid id)
        {
            var taakTask = taakTaskRepository.GetById(id);
            if(taakTask == null)
            {
                return RedirectToAction("Index");
            }
            return View(taakTask);
        }

        // GET: TaakTaskController/Create
        public ActionResult Create()
        {
            var objToParse = new TaakTaskViewModelCreate(taskCategoryRepository);
            
            return View(objToParse);
        }

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
                    model.IdTask = Guid.NewGuid();
                    taakTaskRepository.Insert(model);
                    TempData["succes"] = "task created successfully";
                    return RedirectToAction("Index");
                }
                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        // GET: TaakTaskController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaakTaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaakTaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaakTaskController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
