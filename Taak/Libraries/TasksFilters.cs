using Taak.Models;
using Taak.Repository;

namespace Taak.Libraries
{
    public class TasksFilters
    {
        public static List<TaakTaskModel> FilterTasksByCategory(List<TaakTaskModel> tasks,string cat)
        {
           
            tasks = tasks.Where(taakTask => taakTask.IdTaskCategory.ToString() == cat).ToList<TaakTaskModel>();
            
            return tasks;
        }
    }
}
