using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class TaskCategoryRepository:GenericRepository<TaskCategory,TaskCategoryModel>
    {
        private readonly ApplicationDbContext _db;
        public TaskCategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

    }
}
