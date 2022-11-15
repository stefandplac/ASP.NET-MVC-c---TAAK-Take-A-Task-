using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;

namespace Taak.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskCategoryController : Controller
    {
        private readonly TaskCategoryRepository taskCategoryRepository;
        public TaskCategoryController(ApplicationDbContext db)
        {
            taskCategoryRepository = new TaskCategoryRepository(db);
        }
        // GET: TaskCategoryController
        public ActionResult Index()
        {
            var taskCategories = taskCategoryRepository.GetAll();
            return View(taskCategories);
        }

       

        // GET: TaskCategoryController/Details/5
        public ActionResult Details(Guid id)
        {
            var taskCategory = taskCategoryRepository.GetById(id);
            if (taskCategory == null)
            {
                return RedirectToAction("Index");
            }
            return View(taskCategory);
        }

        // GET: TaskCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new TaskCategoryModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    model.IdTaskCategory = Guid.NewGuid();
                    taskCategoryRepository.Insert(model);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskCategoryController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var taskCategory = taskCategoryRepository.GetById(id);
            if (taskCategory == null)
            {
                return RedirectToAction("Index");
            }
            return View(taskCategory);
        }

        // POST: TaskCategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new TaskCategoryModel();
                var task=TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    taskCategoryRepository.Update(model,model.IdTaskCategory);
                    return RedirectToAction("Index");
                }
                return View("Edit");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: TaskCategoryController/Delete/5
        public ActionResult Delete(Guid id)
        {
            taskCategoryRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
