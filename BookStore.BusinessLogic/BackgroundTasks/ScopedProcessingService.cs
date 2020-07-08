using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.Infra.Messaging;
using BookStore.Data.DataModels;
using BookStore.HealthCheck;

namespace BookStore.BusinessLogic.ServiceModels
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BackgroundServiceCheck _backgroundServiceCheck;
        public ScopedProcessingService(ILogger<ScopedProcessingService> logger,
                IMessageService messageService, 
                IUnitOfWork unitOfWork,
                BackgroundServiceCheck backgroundServiceCheck)
        {
            _logger = logger;
            _messageService = messageService;
            _unitOfWork = unitOfWork;
            _backgroundServiceCheck = backgroundServiceCheck;
        }
        
        public async Task DoWork(CancellationToken stoppingToken)
        {
            // logica de executie.
            var Orders = _unitOfWork.OrderDetails.GetAll(includeProperties: "ApplicationUser")
                            .Where(o => o.OrderStatus == OrderStatus.Shipped.ToString() &&
                                      o.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString());
            foreach(var order in Orders)
            {
                MessageOptions message = new MessageOptions
                {
                    toEamilAddress = order.ApplicationUser.Email,
                    subjcet = "Reminder",
                    message = $"Order {order.Id} has been shiped. You can make the payment."
                };

                await _messageService.SendEmailAsync(message);
            }
            _backgroundServiceCheck.StartupTaskCompleted = 1;
            await Task.Delay(System.TimeSpan.FromMinutes(1), stoppingToken);
            _backgroundServiceCheck.StartupTaskCompleted = 2;
        }
        
    }
}
