using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using BookStore.Infra.Messaging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageService messageService;
        private readonly UserManager<ApplicationUser> userManager;
        [BindProperty]
        public ShoppingItemVM ShoppingVM { get; set; }

        public ItemController(
            IUnitOfWork unitOfWork,
            IMessageService messageService,
            UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.messageService = messageService;
        }

        public IActionResult Index()
        {
            string UserId = GetUserId();
            ShoppingVM = new ShoppingItemVM()
            {
                orderDetails = new OrderDetails(),
                ListItem = unitOfWork.ShoppingItem.GetAll(u => u.ApplicationUserId == UserId, includeProperties: "Product")
            };
            ShoppingVM.orderDetails.OrderTotal = 0;
            ShoppingVM.orderDetails.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id.ToString() == UserId,
                includeProperties: "Company");

            foreach (var list in ShoppingVM.ListItem)
            {
                list.Price = StaticDetails.GetPrice(list.Count, list.Product.Price);
                ShoppingVM.orderDetails.OrderTotal += (list.Price * list.Count);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(ShoppingVM);
        }


        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexPost()
        {
            string UserId = GetUserId();
            var user = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id.ToString() == UserId);

            if (user == null)
            {
                TempData["ResultMessage"] = "Verificatino email is empty!";
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            MessageOptions message = new MessageOptions
            {
                toEamilAddress = user.Email,
                subjcet = "Confirm your email",
                message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            };
            await messageService.SendEmailAsync(message);
             TempData["ResultMessage"] = "Verification email was sent. Please check your email.";
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Minus(int itemId)
        {
            var shoppingItem = unitOfWork.ShoppingItem.GetFirstOrDefault(i => i.Id == itemId, includeProperties: "Product");
            if(shoppingItem.Count == 1)
            {
                var cnt = unitOfWork.ShoppingItem.GetAll(u => u.ApplicationUserId == shoppingItem.ApplicationUserId)
                   .ToList().Count();
                unitOfWork.Product.GetById(shoppingItem.ProductId).stock++;
                unitOfWork.ShoppingItem.Remove(shoppingItem);
                unitOfWork.Save();
                HttpContext.Session.SetInt32(StaticDetails.ssShoppingItem, cnt - 1);
            }
            else
            {
                shoppingItem.Count--;
                shoppingItem.Product.stock++;
                shoppingItem.Price = StaticDetails.GetPrice(shoppingItem.Count, shoppingItem.Product.Price);
                unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int itemId)
        {
            var shoppingItem = unitOfWork.ShoppingItem.GetFirstOrDefault(i => i.Id == itemId, includeProperties: "Product");
            
            var cnt = unitOfWork.ShoppingItem.GetAll(u => u.ApplicationUserId == shoppingItem.ApplicationUserId)
                   .ToList().Count();
            unitOfWork.Product.GetById(shoppingItem.ProductId).stock+= shoppingItem.Count;
            unitOfWork.ShoppingItem.Remove(shoppingItem);
            unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticDetails.ssShoppingItem, cnt - 1);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Plus(int itemId)
        {
            var shoppingItem = unitOfWork.ShoppingItem.GetFirstOrDefault(i => i.Id == itemId, includeProperties: "Product");
            shoppingItem.Count += 1;
            shoppingItem.Price = StaticDetails.GetPrice(shoppingItem.Count, shoppingItem.Product.Price);
            unitOfWork.Product.GetById(shoppingItem.ProductId).stock--;
            unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Summary()
        {
            string UserId = GetUserId();

            ShoppingVM = new ShoppingItemVM
            {
                orderDetails = new OrderDetails(),
                ListItem = unitOfWork.ShoppingItem.GetAll(c => c.ApplicationUserId == UserId, includeProperties: "Product")
            };

            ShoppingVM.orderDetails.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id.ToString() == UserId, includeProperties: "Company");

            foreach (var list in ShoppingVM.ListItem)
            {
                list.Price = StaticDetails.GetPrice(list.Count, list.Product.Price);
                ShoppingVM.orderDetails.OrderTotal += (list.Price * list.Count);
            }

            ShoppingVM.orderDetails.Name = ShoppingVM.orderDetails.ApplicationUser.UserName;
            ShoppingVM.orderDetails.PhoneNumber = ShoppingVM.orderDetails.ApplicationUser.PhoneNumber; 
            ShoppingVM.orderDetails.StreetAdress = ShoppingVM.orderDetails.ApplicationUser.StreetAdress;
            ShoppingVM.orderDetails.City = ShoppingVM.orderDetails.ApplicationUser.City;
            ShoppingVM.orderDetails.PostalCode = ShoppingVM.orderDetails.ApplicationUser.PostalCode;
            return View(ShoppingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            string UserId = GetUserId();
            ShoppingVM.orderDetails.ApplicationUser = unitOfWork.ApplicationUser
                .GetFirstOrDefault(c => c.Id.ToString() == UserId,
                        includeProperties: "Company");

            ShoppingVM.ListItem = unitOfWork.ShoppingItem.GetAll(c => c.ApplicationUserId == UserId);

            ShoppingVM.orderDetails.PaymentStatus = PaymentStatus.Pending.ToString();
            ShoppingVM.orderDetails.OrderStatus = OrderStatus.Pending.ToString();
            ShoppingVM.orderDetails.ApplicationUserId = Int32.Parse(GetUserId());

            unitOfWork.OrderDetails.Add(ShoppingVM.orderDetails);
            unitOfWork.Save();

            foreach (var item in ShoppingVM.ListItem)
            {
                var product = unitOfWork.Product.GetById(item.ProductId);
                item.Price = StaticDetails.GetPrice(item.Count, product.Price);
                OrderItem order = new OrderItem
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingVM.orderDetails.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                ShoppingVM.orderDetails.OrderTotal += order.Price * item.Count;
                unitOfWork.OrderItem.Add(order);

            }
            unitOfWork.ShoppingItem.RemoveRange(ShoppingVM.ListItem);
            unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticDetails.ssShoppingItem, 0);
            if (ShoppingVM.Card.CardNumber == null)
            {
                //user choosed to place an order with delay
                ShoppingVM.orderDetails.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingVM.orderDetails.PaymentStatus = PaymentStatus.ApprovedForDelayedPayment.ToString();
                ShoppingVM.orderDetails.OrderStatus = OrderStatus.Approved.ToString();
            }
            else
            {
                //procces the order
                ShoppingVM.orderDetails.TransactionId = "sdklfjasjdfaioerjiowfskldfk";
                ShoppingVM.orderDetails.PaymentStatus = PaymentStatus.Approved.ToString();
                ShoppingVM.orderDetails.OrderStatus = OrderStatus.Approved.ToString();
                ShoppingVM.orderDetails.PaymentDate = DateTime.Now;
            }
            unitOfWork.Save();
            return RedirectToAction("OrderConfirmation", "Item", new { id = ShoppingVM.orderDetails.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        public string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return "";
            return claim.Value;
        }
    }
}
