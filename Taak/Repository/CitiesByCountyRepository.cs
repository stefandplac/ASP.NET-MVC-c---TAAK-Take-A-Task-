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
        public ILookup<string, string> GroupCitiesByCountiesOrderedList()
        {
            ILookup<string, string> citiesByCountiesOrderedList = base.GetAll()
                                                     .OrderBy(item => item.County).ThenBy(item => item.City)
                                                     .ToLookup(
                                                                        entryKey => entryKey.County,
                                                                        entryValue => entryValue.City);
             return citiesByCountiesOrderedList;                                                          
        }
        
        public List<string> ReturnCitiesOrdered()
        {
            List<string> citiesOrdered = base.GetAll()
                                                        .OrderBy(item => item.City)
                                                        .Select(x =>  x.City).ToList<string>();
                                                        
            
            return citiesOrdered;
        }
        public List<string> ReturnCounties()
        {
            List<string> counties = base.GetAll()
                                        .OrderBy(x=>x.County)
                                        .Select(x=>x.County).Distinct().ToList<string>();


            return counties;
        }
    }
}
