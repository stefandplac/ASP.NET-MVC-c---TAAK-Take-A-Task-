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
        public List<OfferModel> GetAllOffersByWorker(Guid idTaskWorker)
        {
            var offersByWorker = base.GetAll().Where(offer => offer.IdTaskWorker == idTaskWorker).ToList();
            return offersByWorker;
        }
        public List<OfferModel> GetAllOffersByTask(Guid idTask)
        {
            var offersByTask = base.GetAll().Where(offer => offer.IdTask == idTask).ToList();
            return offersByTask;
        }
        public void DeleteRange(IEnumerable<OfferModel> offers)
        {
            if (offers.Count() > 0)
            {
                foreach (var offer in offers)
                {
                    base.Delete(offer.IdOffer);
                    
                }
                
            }
        }
        public OfferModel ReturnAcceptedOffer(Guid idTask)
        {
            var acceptedOffer = base.GetAll().Where(item => item.IdTask == idTask && item.IsAccepted==true).FirstOrDefault();
            return acceptedOffer;
        }
    }
}
