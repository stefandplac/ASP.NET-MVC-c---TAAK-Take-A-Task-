using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class TaakTaskViewModelEdit
    {
        public List<TaskCategoryModel> TaskCategories { get; set; }
        public List<string> Counties { get; set; }
        public List<string> Cities { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskCategory { get; set; }
        public Guid IdCustomer { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }

        public DateTime PostedDate { get; set; }
        public DateTime TaskDate { get; set; }
        public string? TimeOptions { get; set; }
        public string DateOption { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;
        public TaakTaskViewModelEdit(TaskCategoryRepository taskCategoryRepository,
                                        CitiesByCountyRepository citiesRepository,
                                        TaakTaskModel taakTask)
        {

            TaskCategories = taskCategoryRepository.GetAll().ToList();
            Counties = citiesRepository.ReturnCounties();
            Cities = citiesRepository.ReturnCitiesOrdered();
            IdTask = taakTask.IdTask;
            IdTaskCategory = taakTask.IdTaskCategory;
            IdCustomer = taakTask.IdCustomer;
            Title = taakTask.Title;
            Description= taakTask.Description;
            Buget= taakTask.Buget;
            SpecialRequirements= taakTask.SpecialRequirements;
            PostedDate= taakTask.PostedDate;
            TaskDate= taakTask.TaskDate;
            TimeOptions= taakTask.TimeOptions;
            DateOption= taakTask.DateOption;
            City= taakTask.City;
            Street= taakTask.Street;
            Building= taakTask.Building;
            Floor= taakTask.Floor;
            County= taakTask.County;
            Country= taakTask.Country;
        }
    }
}
