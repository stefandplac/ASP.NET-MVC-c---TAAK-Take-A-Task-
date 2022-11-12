using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class TasksWithSearchFiltersViewModel
    {
        public List<TaskCategoryModel> TaskCategories { get; set; }
        public List<TaakTaskModel> Tasks { get; set; }
        public ILookup<string,string> CitiesByCounty { get; set; }
        public decimal SearchBudgetMin { get; set; }
        public decimal SearchBudgetMax { get; set; }
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }

        public string SearchCategory { get; set; }
        public string SearchLocation { get; set; }
        public int PageNumber { get; set; }
        public TasksWithSearchFiltersViewModel(TaskCategoryRepository taakCategoryRepository,
                                TaakTaskRepository taakTaskRepository,
                                CitiesByCountyRepository citiesByCountyRepository)
        {
            TaskCategories = taakCategoryRepository.GetAll();
            Tasks = taakTaskRepository.GetAll();
            CitiesByCounty = citiesByCountyRepository.GroupCitiesByCountiesOrderedList();
            (decimal, decimal) result = taakTaskRepository.GetMinMax();
            BudgetMin = result.Item1;
            BudgetMax = result.Item2;
        }
       
    }
}
