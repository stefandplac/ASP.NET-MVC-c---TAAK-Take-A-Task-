using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Data;
using Taak.Models;
using Taak.Repository;

namespace Taak.Controllers
{
    public class OfferController : Controller
    {
        private readonly OfferRepository offerRepository;
        private readonly TasksWorkerRepository taskWorkerRepository;
        private readonly TaakTaskRepository taakTaskRepository;
        public OfferController(ApplicationDbContext db)
        {
            offerRepository = new OfferRepository(db);
            taskWorkerRepository=new TasksWorkerRepository(db);
            taakTaskRepository = new TaakTaskRepository(db);
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
        [Authorize(Roles="Worker")]
        public ActionResult IndexByTaskWorker()
        {
            
            var idUser = HttpContext.Session.GetString("UserId");
            var idTaskWorker = taskWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;
            var offers = offerRepository.GetAll().Where(offer => offer.IdTaskWorker == idTaskWorker);
            return View(offers);
        }

        // GET: OfferController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OfferController/Create
        [Authorize(Roles ="Worker")]
        public ActionResult Create(Guid idTask)
        {
            var idUser = HttpContext.Session.GetString("UserId");
            var idTaskWorker = taskWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;
            var offerModel = new OfferModel();
            offerModel.IdOffer = Guid.NewGuid();
            offerModel.IdTask=idTask;
            offerModel.IdTaskWorker=idTaskWorker;

            return View(offerModel);
        }

        // POST: OfferController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Worker")]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var model = new OfferModel();
                var task=TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    offerRepository.Insert(model);
                    return RedirectToAction("IndexByTaskWorker");
                }
                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        [Authorize(Roles ="Worker")]
        public ActionResult AcceptTaskOfferCreate(Guid idTask)
        {
            var taakTask = taakTaskRepository.GetById(idTask);
            var idUser = HttpContext.Session.GetString("UserId");
            var idTaskWorker = taskWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;

            //we check first to see if there is already an offer made for the task
            if (offerRepository.CheckForExistingOffer(idTask, idTaskWorker))
            {
                TempData["error"] = "you already made an offer for that task";
                return RedirectToAction("IndexByTaskWorker","TaakTask");
            }
            else
            {
                var taskOfferAccepted = new OfferModel()
                {
                    IdTaskWorker = idTaskWorker,
                    IdOffer = Guid.NewGuid(),
                    IdTask = idTask,
                    Buget = taakTask.Buget,
                    IsOriginalOfferAccepted = true
                };
                offerRepository.Insert(taskOfferAccepted);
                return RedirectToAction("IndexByTaskWorker");
            }

           
        }

        // GET: OfferController/Edit/5
        [Authorize(Roles ="Worker")]
        public ActionResult Edit(Guid id)
        {
            var offer = offerRepository.GetById(id);
            if (offer == null)
            {
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        // POST: OfferController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Worker")]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new OfferModel();
                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    model.IsOriginalOfferAccepted = false;
                    offerRepository.Update(model,model.IdOffer);
                    return RedirectToAction("IndexByTaskWorker");
                }
                return View("Edit");
            }
            catch
            {
                return View("Edit");
            }
        }

        // GET: OfferController/Delete/5
        [Authorize(Roles ="Worker,Admin")]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            offerRepository.Delete(id);
            return RedirectToAction("Index");
        }

        
    }
}
