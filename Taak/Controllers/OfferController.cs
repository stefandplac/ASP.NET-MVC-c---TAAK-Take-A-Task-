using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Repository;

namespace Taak.Controllers
{
    public class OfferController : Controller
    {
        private readonly OfferRepository offerRepository;
        public OfferController(ApplicationDbContext db)
        {
            offerRepository = new OfferRepository(db);
        }
        // GET: OfferController
        public ActionResult Index()
        {
            var offers = offerRepository.GetAll();
            if (offers == null)
            {
                return NotFound();
            }

            return View(offers);
        }

        // GET: OfferController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OfferController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OfferController/Create
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

        // GET: OfferController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OfferController/Edit/5
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

        // GET: OfferController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OfferController/Delete/5
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
