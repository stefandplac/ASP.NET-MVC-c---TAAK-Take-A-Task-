using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;

namespace Taak.Controllers
{
    public class TasksWorkerController : Controller
    {
        private readonly TasksWorkerRepository tasksWorkerRepository;
        public TasksWorkerController(ApplicationDbContext db)
        {
            tasksWorkerRepository = new TasksWorkerRepository(db);
        }
        // GET: TasksWorkerController
        public ActionResult Index()
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var tasksWorkers = tasksWorkerRepository.GetAll();
            return View(tasksWorkers);
        }

        // GET: TasksWorkerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TasksWorkerController/Create
        public ActionResult Create(string userId)
        {
            var idTaskWorker = Guid.NewGuid();
            var taskWorker = new TasksWorkerModel()
            {
                IdTaskWorker=idTaskWorker,
                UserId=userId,
                Name = "not set yet",
                City = "not set yet",
                Street = "not set yet",
                Building = "not set yet",
                Country = "not set yet",
                County = "not set yet",
                Phone = "0000000000",
            };
            tasksWorkerRepository.Insert(taskWorker);
            ViewBag.IdTaskWorker = idTaskWorker;
            ViewBag.UserId = userId;
            return View();
        }

        // POST: TasksWorkerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new TasksWorkerModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    tasksWorkerRepository.Update(model,model.IdTaskWorker);
                    TempData["success"] = "profile was successfully created";
                    return RedirectToAction("Index");
                }
                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        // GET: TasksWorkerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TasksWorkerController/Edit/5
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

        // GET: TasksWorkerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TasksWorkerController/Delete/5
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
