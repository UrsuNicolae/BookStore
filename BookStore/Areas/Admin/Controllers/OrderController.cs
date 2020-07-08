using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using BookStore.Infra.Messaging;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageService messageService;

        [BindProperty]
        public OrderVM orderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            this.unitOfWork = unitOfWork;
            this.messageService = messageService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            orderVM = new OrderVM()
            {
                OrderDetails = unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == id,
                                                includeProperties: "ApplicationUser"),
                OrderItems = unitOfWork.OrderItem.GetAll(o => o.OrderId == id,
                                                includeProperties: "Product"),
                Card = new CardModel()
            };

            return View(orderVM);
        }

        [Authorize(Roles = StaticDetails.Role_AdminUser)]
        public async Task<IActionResult> StartProcessing(int id)
        {
            OrderDetails orderDetails = unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == id, includeProperties:"ApplicationUser");
            orderDetails.OrderStatus = OrderStatus.Processing.ToString();
            unitOfWork.Save();

            MessageOptions message = new MessageOptions
            {
                toEamilAddress = orderDetails.ApplicationUser.Email,
                subjcet = "Order Details",
                message = "Your order is processed. Shortly we will send an email about shipping date."
            };

            await messageService.SendEmailAsync(message);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_AdminUser)]
        public async Task<IActionResult> ShipOrder()
        {
            OrderDetails orderDetails = unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == orderVM.OrderDetails.Id, includeProperties: "ApplicationUser");
            orderDetails.TrackingNumber = orderVM.OrderDetails.TrackingNumber;
            orderDetails.Carrier = orderVM.OrderDetails.Carrier;
            orderDetails.OrderStatus = OrderStatus.Shipped.ToString();
            orderDetails.ShippingDate = DateTime.Now;
            unitOfWork.Save();

            MessageOptions message = new MessageOptions
            {
                toEamilAddress = orderDetails.ApplicationUser.Email,
                subjcet = "Order Details",
                message = $"Your order is shipped." +
                $"Shipping date:{orderDetails.ShippingDate}"
            };

            await messageService.SendEmailAsync(message);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = StaticDetails.Role_AdminUser)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            OrderDetails orderDetails = unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
            orderDetails.OrderStatus = OrderStatus.Processing.ToString();

            if(orderDetails.PaymentStatus == PaymentStatus.Approved.ToString())
            {
                //return money
            }
            else
            {
                orderDetails.OrderStatus = OrderStatus.Cancelled.ToString();
                orderDetails.PaymentStatus = PaymentStatus.Rejected.ToString();
            }
            unitOfWork.Save();

            MessageOptions message = new MessageOptions
            {
                toEamilAddress = orderDetails.ApplicationUser.Email,
                subjcet = "Order Details",
                message = "Your order was canceled"
            };

            await messageService.SendEmailAsync(message);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pay()
        {
            OrderDetails order = unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == orderVM.OrderDetails.Id);
            if (orderVM.Card.CardNumber != null 
                && orderVM.Card.CVC!= null 
                && orderVM.Card.Date != null)
            {
                
                order.TransactionId = "1211";
                order.PaymentStatus = PaymentStatus.Approved.ToString();
                order.PaymentDate = DateTime.Now;
                unitOfWork.Save();
                
            }
            return RedirectToAction("Details", "Order", new { id = order.Id });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetOrders(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            
            IEnumerable<OrderDetails> orderDetails;

            if(User.IsInRole(StaticDetails.Role_AdminUser))
            {
                orderDetails = unitOfWork.OrderDetails.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                orderDetails = unitOfWork.OrderDetails.GetAll(
                    u => u.ApplicationUserId.ToString() == claim.Value,
                    includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderDetails = orderDetails.Where(o => o.OrderStatus == OrderStatus.Pending.ToString());
                    break;
                case "inprocess":
                    orderDetails = orderDetails.Where(o => o.OrderStatus == OrderStatus.Approved.ToString() ||
                                                            o.OrderStatus == OrderStatus.Processing.ToString());
                    break;
                case "completed":
                    orderDetails = orderDetails.Where(o => o.OrderStatus ==  OrderStatus.Shipped.ToString());
                    break;
                case "rejected":
                    orderDetails = orderDetails.Where(o => o.OrderStatus == OrderStatus.Cancelled.ToString() ||
                                                           o.OrderStatus == OrderStatus.Refunded.ToString() ||
                                                           o.PaymentStatus == PaymentStatus.Rejected.ToString());
                    break;
                default:
                    break;
            }

            return Json(new{ data = orderDetails});
        }
        #endregion
    }
}