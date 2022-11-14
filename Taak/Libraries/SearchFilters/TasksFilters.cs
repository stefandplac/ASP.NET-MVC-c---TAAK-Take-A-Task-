using Taak.Models;
using Taak.Repository;

namespace Taak.Libraries.SearchFilters
{
    public class TasksFilters
    {
        private static List<TaakTaskModel> FilterTasksByCategory(List<TaakTaskModel> tasks, string cat)
        {

            tasks = tasks.Where(taakTask => taakTask.IdTaskCategory.ToString() == cat).ToList();

            return tasks;
        }
        private static List<TaakTaskModel> FilterByLocation(List<TaakTaskModel> tasks, string location)
        {
            if (location.Contains("#"))
            {
                //the county name contains  # in the city post value to be identified as county
                location = location.Trim(new char[] {'#'});
                tasks = tasks.Where(taakTask => taakTask.County == location).ToList();
            }
            tasks = tasks.Where(taakTask => taakTask.City == location).ToList();
            return tasks;
        }
        private static List<TaakTaskModel> FilterByBudget(List<TaakTaskModel> tasks,
                                                          decimal budgetMin, 
                                                          decimal budgetMax)
        {
            tasks = tasks.Where(task=> task.Buget>=budgetMin && task.Buget<=budgetMax).ToList();
            return tasks;
        }
        public static IEnumerable<TaakTaskModel> SortByMostRecent(IEnumerable<TaakTaskModel> tasks)
        {
            tasks = tasks.OrderByDescending(task=>task.PostedDate);
            return tasks;
        }

        public static List<TaakTaskModel> FilterByCategoryByCityByBudget(List<TaakTaskModel> tasks,
                                                                         string cat, 
                                                                         string location, 
                                                                         decimal searchBudgetMin,
                                                                         decimal searchBudgetMax,
                                                                         decimal budgetMin,
                                                                         decimal budgetMax)
        {
            if(searchBudgetMin!=budgetMin || searchBudgetMax != budgetMax)
            {
                tasks = FilterByBudget(tasks, searchBudgetMin, searchBudgetMax);
            }

            if (!String.IsNullOrEmpty(cat) && cat != "All")
            {
                tasks = FilterTasksByCategory(tasks, cat);
            }
           
            if (!String.IsNullOrEmpty(location) && location != "All")
            {
                tasks = FilterByLocation(tasks, location);
            }
            return tasks;
        }
        public static List<TaakTaskModel> FilterByWorkerWithNoOffer(
                                                                List<TaakTaskModel> tasks, 
                                                                OfferRepository offerRepository,
                                                                TasksWorkerRepository tasksWorkerRepository,
                                                                string idUser
                                                                )
        {
            if (idUser != null)
            {
                var idTaskWorker = tasksWorkerRepository.GetTaskWorkerByUserId(idUser).IdTaskWorker;
                var offersByWorker = offerRepository.GetAllOffersByWorker(idTaskWorker);
                var tasksWithNoOffer = tasks.Except(
                                                     tasks.Join(offersByWorker,
                                                                    t => t.IdTask,
                                                                    o => o.IdTask,
                                                                    (t, o) => t)
                                                    ).ToList();
                return tasksWithNoOffer;
            }
            return tasks;
        }
    }
}
