using Taak.Data;
using Taak.Models;
using Taak.Models.DBObjects;

namespace Taak.Repository
{
    public class CustomerRepository:GenericRepository<Customer,CustomerModel>
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public Guid GetCustomerId(string userId)
        {
            
             var id= _dbContext.Customers.FirstOrDefault(x => x.UserId == userId).IdCustomer;
            return id;
        }
        public CustomerModel GetCustomerByUserId(string userId)
        {
            var customer = MapDBObjectToModel(_dbContext.Customers.FirstOrDefault(x => x.UserId == userId),new CustomerModel());
            return customer;
        }
    }
}
