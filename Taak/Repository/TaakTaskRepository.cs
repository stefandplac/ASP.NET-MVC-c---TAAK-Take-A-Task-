using Taak.Repository;
using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class TaakTaskRepository:GenericRepository<TaakTask,TaakTaskModel>
    {
        private readonly ApplicationDbContext _db;
        public TaakTaskRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public (decimal, decimal) GetMinMax()
        {
            var min = base.GetAll().Min(item => item.Buget);
            var max = base.GetAll().Max(item=>item.Buget);
            return (min, max);
        }
     }
}
