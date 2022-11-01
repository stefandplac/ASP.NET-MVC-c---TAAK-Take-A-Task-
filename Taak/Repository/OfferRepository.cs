using Taak.Repository;
using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class OfferRepository:GenericRepository<Offer,OfferModel>
    {
        private readonly ApplicationDbContext db;
        public OfferRepository(ApplicationDbContext db):base(db)
        {
            this.db = db;
        }
    }
}
