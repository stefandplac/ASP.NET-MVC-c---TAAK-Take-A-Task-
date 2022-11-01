using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taak.Models;
using Taak.Models.DBObjects;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class OfferViewModelIndex
    {
        public List<TaakTaskModel> TaakTasks { get; set; }
        public List<TasksWorkerModel> TasksWorkers { get; set; }
        public Guid IdOffer { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskWorker { get; set; }
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime TaskStartDate { get; set; }
        public string? EstimatedTime { get; set; }
        public bool? IsAccepted { get; set; }

        public OfferViewModelIndex(TaakTaskRepository taakTaskRepository, TasksWorkerRepository tasksWorkerRepository)
        {
            this.TaakTasks = taakTaskRepository.GetAll();
            this.TasksWorkers = tasksWorkerRepository.GetAll();
        }
    }
}
