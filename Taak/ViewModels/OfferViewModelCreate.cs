using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class OfferViewModelCreate
    {
        public TaakTaskModel TaakTask { get; set; }
        public Guid IdOffer { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskWorker { get; set; }
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public string? EstimatedTime { get; set; }
        public bool? IsAccepted { get; set; }
        public bool IsOriginalOfferAccepted { get; set; }
        public OfferViewModelCreate(Guid idTask,
                                    Guid idTaskWorker,
                                    TaakTaskRepository taakTaskRepository)
        {
            IdOffer = Guid.NewGuid();
            IdTask = idTask;
            IdTaskWorker = idTaskWorker;
            TaakTask = taakTaskRepository.GetById(idTask);
            
        }
    }
}
