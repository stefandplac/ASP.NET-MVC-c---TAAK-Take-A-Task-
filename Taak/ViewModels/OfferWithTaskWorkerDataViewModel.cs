using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class OfferWithTaskWorkerDataViewModel
    {
        public string WorkerName { get; set; }
        public string Phone { get; set; }
        public Guid IdOffer { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskWorker { get; set; }
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public string? EstimatedTime { get; set; }
        public bool? IsAccepted { get; set; }
        public bool? IsOriginalOfferAccepted { get; set; }
        public OfferWithTaskWorkerDataViewModel(OfferModel offer,TasksWorkerRepository taskWorkerRepository)
        {
            IdOffer=offer.IdOffer;
            IdTask=offer.IdTask;
            IdTaskWorker=offer.IdTaskWorker;
            Buget=offer.Buget;
            SpecialRequirements=offer.SpecialRequirements;
            TaskStartDate = offer.TaskStartDate;
            EstimatedTime = offer.EstimatedTime;
            IsAccepted=offer.IsAccepted;
            IsOriginalOfferAccepted = offer.IsOriginalOfferAccepted;

            WorkerName = taskWorkerRepository.GetById(offer.IdTaskWorker).Name;
            Phone = taskWorkerRepository.GetById(offer.IdTaskWorker).Phone;
        }
    }
}
