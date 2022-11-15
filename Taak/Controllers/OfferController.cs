using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Taak.Data;
using Taak.Models;
using Taak.Repository;
using Taak.ViewModels;

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
        public ActionResult IndexByTaskWorker(string pageno)
        {
            List<OfferViewModelIndexByWorker> offersViewModelByWorker = new List<OfferViewModelIndexByWorker>();
            var idUser = HttpContext.Session.GetString("UserId");
            var idTaskWorker = taskWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;

            //returns only the results needed to be display in one single page depending on the pagesize
            int x;
            var pageNumber = Int32.TryParse(pageno,out x)? x : 1;
            var pageSize=5;
            var offers = offerRepository.GetAll().Where(offer => offer.IdTaskWorker == idTaskWorker);
            double result = offers.Count() / pageSize;
            ViewBag.Pages = offers.Count() % pageSize != 0 ? result+1 : result;
            ViewBag.CurrentPageNo = pageNumber;
            ViewBag.PrevPage = pageNumber == 1 ? 1 : pageNumber - 1;
            ViewBag.NextPage = pageNumber == ViewBag.Pages ? pageNumber : pageNumber + 1;

            offers = offers.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            foreach(var offer in offers)
            {
                var item = new OfferViewModelIndexByWorker(offer, taakTaskRepository);
                offersViewModelByWorker.Add(item);
            }
            

            return View(offersViewModelByWorker);
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
            var offerModel = new OfferViewModelCreate(idTask, idTaskWorker,taakTaskRepository);
            
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
                    if(!offerRepository.CheckForExistingOffer(model.IdTask, model.IdTaskWorker))
                    {
                        RedirectToAction("SearchTaskIndex", "TaakTask");
                    }
                    if (model.TaskStartDate==null && model.EstimatedTime==null && model.SpecialRequirements==null && model.Buget == 0)
                    {
                        model.IsOriginalOfferAccepted = true;
                    }
                    else
                    {
                        model.IsOriginalOfferAccepted = false;
                    }                   
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
                return RedirectToAction("SearchTaskIndex", "TaakTask");
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
            return RedirectToAction("IndexByTaskWorker");
        }
        [Authorize(Roles ="Customer")]
        public ActionResult AcceptOfferByCustomer(Guid idOffer)
        {
            var offerAccepted = offerRepository.GetById(idOffer);
            var idTask = offerAccepted.IdTask;
            offerAccepted.IsAccepted = true;
            offerRepository.Update(offerAccepted, idOffer);
            return RedirectToAction("TaskWithAllOffers", "TaakTask", new {idTask=idTask});
        }
    }
}
