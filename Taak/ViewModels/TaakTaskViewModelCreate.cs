using Taak.Models;
using Taak.Models.DBObjects;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class TaakTaskViewModelCreate
    {
        
        public List<TaskCategoryModel> TaskCategories { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskCategory { get; set; }
        public Guid IdCustomer { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;
        public TaakTaskViewModelCreate( TaskCategoryRepository taskCategoryRepository)
        {
            
            TaskCategories = taskCategoryRepository.GetAll().ToList();
        }
    }
}
