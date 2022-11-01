using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Taak.Controllers
{
    public class TasksWorkerController : Controller
    {
        // GET: TasksWorkerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TasksWorkerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TasksWorkerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksWorkerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
