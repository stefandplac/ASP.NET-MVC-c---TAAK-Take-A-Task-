using Taak.Repository;
using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class TasksWorkerRepository:GenericRepository<TasksWorker,TasksWorkerModel>
    {
        private readonly ApplicationDbContext _db;
        public TasksWorkerRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
    }
}
