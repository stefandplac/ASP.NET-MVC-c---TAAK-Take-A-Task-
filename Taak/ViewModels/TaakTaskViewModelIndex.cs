using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class TaakTaskViewModelIndex
    {
        public string CustomerName { get; set; }
        public string CategoryName { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskCategory { get; set; }
        public Guid IdCustomer { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public string? TimeOptions { get; set; }
        public string DateOption { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;
        public TaakTaskViewModelIndex( TaakTaskModel taakTaskModel,
                                        TaskCategoryRepository taskCategoryRepository,
                                        CustomerRepository customerRepository)
        {
            this.IdTask= taakTaskModel.IdTask;
            this.IdTaskCategory = taakTaskModel.IdTaskCategory;
            this.IdCustomer=taakTaskModel.IdCustomer;
            this.Title = taakTaskModel.Title;
            this.Description = taakTaskModel.Description;
            this.Buget = taakTaskModel.Buget;
            this.SpecialRequirements = taakTaskModel.SpecialRequirements;
            this.TaskStartDate = taakTaskModel.TaskStartDate;
            this.TaskEndDate = taakTaskModel.TaskEndDate;
            this.City = taakTaskModel.City;
            this.Street = taakTaskModel.Street;
            this.Building = taakTaskModel.Building;
            this.Floor = taakTaskModel.Floor;
            this.County=taakTaskModel.County;
            this.Country = taakTaskModel.Country;

            this.CustomerName = customerRepository.GetById(taakTaskModel.IdCustomer).Name;
            this.CategoryName = taskCategoryRepository.GetById(taakTaskModel.IdTaskCategory).Name;
        }
    }
}
