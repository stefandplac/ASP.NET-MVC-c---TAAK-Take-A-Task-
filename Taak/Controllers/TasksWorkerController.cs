using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            
            var tasksWorkers = tasksWorkerRepository.GetAll();
            return View(tasksWorkers);
        }
        

        // GET: TasksWorkerController/Details/5
        [Authorize(Roles="Worker,Admin")]
        public ActionResult Details(Guid id)
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
        [Authorize(Roles="Worker")]
        public ActionResult Edit(Guid id)
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var taskWorker = tasksWorkerRepository.GetTaskWorkerByUserId(idUser);
            return View(taskWorker);
        }

        // POST: TasksWorkerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Worker")]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new TasksWorkerModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    tasksWorkerRepository.Update(model,model.IdTaskWorker);
                    return RedirectToAction("WorkerProfile");
                }

                return View("Edit");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: TasksWorkerController/Delete/5
        [Authorize(Roles="Worker,Admin")]
        public ActionResult Delete(Guid id)
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var taskWorker = tasksWorkerRepository.GetTaskWorkerByUserId(idUser);
            return View(idUser);
        }

        // POST: TasksWorkerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Worker,Admin")]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
           
            tasksWorkerRepository.Delete(id);
            if (User.IsInRole("Worker"))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult WorkerProfile()
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var taskWorker = tasksWorkerRepository.GetTaskWorkerByUserId(idUser);
            return View("WorkerProfile",taskWorker);
        }
    }
}
