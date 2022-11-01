﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Taak.Controllers
{
    public class TaskCategoryController : Controller
    {
        // GET: TaskCategoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TaskCategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskCategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskCategoryController/Edit/5
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

        // GET: TaskCategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskCategoryController/Delete/5
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
