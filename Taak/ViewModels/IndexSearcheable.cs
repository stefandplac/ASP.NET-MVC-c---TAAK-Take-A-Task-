using Taak.Models;
using Taak.Repository;

namespace Taak.ViewModels
{
    public class IndexSearcheable
    {
        public List<TaskCategoryModel> TaskCategories { get; set; }
        public List<TaakTaskModel> Tasks { get; set; }
        public ILookup<string,string> CitiesByCounty { get; set; }

        public string SearchCategory { get; set; }
        public string SearchBudget { get; set; }
        public string SearchCity { get; set; }
        public IndexSearcheable(TaskCategoryRepository taakCategoryRepository,
                                TaakTaskRepository taakTaskRepository,
                                CitiesByCountyRepository citiesByCountyRepository)
        {
            TaskCategories = taakCategoryRepository.GetAll();
            Tasks = taakTaskRepository.GetAll();
            CitiesByCounty = citiesByCountyRepository.GetAll().ToLookup(
                                                                        entryKey=>entryKey.County,
                                                                        entryValue=>entryValue.City
                                                                        );
        }
    }
}
