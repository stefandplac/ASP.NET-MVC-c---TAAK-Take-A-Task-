using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class CitiesByCountyRepository:GenericRepository<CitiesByCounty,CitiesByCountyModel>
    {
        private readonly ApplicationDbContext _db;
        public CitiesByCountyRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public string GetCountyByCityName(string city)
        {
            string county = base.GetAll().FirstOrDefault(item=>item.City.Equals(city)).County;
            return county;
        }
    }
}
