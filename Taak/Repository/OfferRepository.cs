using Taak.Repository;
using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class OfferRepository:GenericRepository<Offer,OfferModel>
    {
        private readonly ApplicationDbContext _db;
        public OfferRepository(ApplicationDbContext db):base(db)
        {
            this._db = db;
        }
        public bool CheckForExistingOffer(Guid idTask, Guid idTaskWorker)
        {
            var existingOffer = _db.Offers.FirstOrDefault(offer => offer.IdTask == idTask && offer.IdTaskWorker == idTaskWorker);
            if(existingOffer == null)
            {
                return false;
            }
            return true;
        }
    }
}
