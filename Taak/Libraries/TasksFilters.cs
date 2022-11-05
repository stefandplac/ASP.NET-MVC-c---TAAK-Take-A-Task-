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
        public static List<TaakTaskModel> FilterByCityOrCounty(List<TaakTaskModel> tasks, string location)
        {
            if (location.Contains("#"))
            {
                //the county name contains  # in the city post value to be identified as county
                tasks = tasks.Where(taakTask => taakTask.County == location).ToList<TaakTaskModel>();
            }
            tasks = tasks.Where(taakTask=>taakTask.City==location).ToList<TaakTaskModel>();
            return tasks;
        }
    }
}
