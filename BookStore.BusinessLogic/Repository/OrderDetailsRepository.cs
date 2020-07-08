using System;
using System.Linq;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Data.Data;
using BookStore.Data.DataModels;
using Microsoft.Extensions.Logging;

namespace BookStore.BusinessLogic.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger logger;

        public OrderDetailsRepository(ApplicationDbContext db, ILogger<UnitOfWork> logger) : base(db, logger)
        {
            _db = db;
            this.logger = logger;
        }

        public bool Update(OrderDetails obj)
        {
            try
            {
                var objFromDb = _db.OrderDetails.FirstOrDefault(s => s.Id == obj.Id);
                if (objFromDb != null)
                {
                    objFromDb.ApplicationUser = obj.ApplicationUser;
                    objFromDb.ShippingDate = obj.ShippingDate;
                    objFromDb.OrderTotal = obj.OrderTotal;
                    objFromDb.TrackingNumber = obj.TrackingNumber;
                    objFromDb.Carrier = obj.Carrier;
                    objFromDb.OrderStatus = obj.OrderStatus;
                    objFromDb.PaymentStatus = obj.PaymentStatus;
                    objFromDb.PaymentDate = obj.PaymentDate;
                    objFromDb.PaymentDueDate = obj.PaymentDueDate;
                    objFromDb.TransactionId = obj.TransactionId;
                    objFromDb.PhoneNumber = obj.PhoneNumber;
                    objFromDb.StreetAdress = obj.StreetAdress;
                    objFromDb.City = obj.City;
                    objFromDb.PostalCode = obj.PostalCode;
                    objFromDb.Name = obj.Name;
                }
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return false;
            }
        }
    }
}
