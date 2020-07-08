using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using System.Linq;
using AutoMapper;

namespace BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;       

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        public HomeVM productList { get; set; }

        [HttpGet]
        public IActionResult Index(int productPage = 1)
        {
            if(string.IsNullOrWhiteSpace(productList.Search))
            {
                productList.Products = unitOfWork.Product.GetAll();
            }
            else
            {
                productList.Products = unitOfWork.Product.GetAll()
                    .Where(e => e.Title.Contains(productList.Search) ||
                                e.Author.Contains(productList.Search));
            }

            var countItem = productList.Products.Count();
            productList.Products = productList.Products.OrderBy(p=>p.Title).Skip((productPage - 1) * 4).Take(4).ToList();

            productList.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = 4,
                TotalItem = countItem,
                urlParam = "/Customer/Home/Index?productPage=:"
            };
            string UserId = GetUserId();
            if(!string.IsNullOrEmpty(UserId))
            {
                var count = unitOfWork.ShoppingItem.
                    GetAll(i => i.ApplicationUserId == UserId).
                    ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.ssShoppingItem, count);
            }
            return View(productList);
        }


        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = unitOfWork.Product.GetFirstOrDefault(i=>i.Id == id, includeProperties:"Category,CoverType");
            ShoppingItem cartObj = new ShoppingItem()
            {
                Product = product,
                ProductId = product.Id
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingItem cartObj)
        {
            cartObj.Id = 0;
            if(ModelState.IsValid)
            {
                if(cartObj.Count > unitOfWork.Product.GetById(cartObj.ProductId).stock)
                {
                    ViewBag.Error = "The number of items you want to buy is bigger than the numbers of available products";
                    var product = unitOfWork.Product
                        .GetFirstOrDefault(i => i.Id == cartObj.ProductId,
                                   includeProperties: "Category,CoverType");
                    ShoppingItem cart = new ShoppingItem()
                    {
                        Product = product,
                        ProductId = product.Id
                    };
                    return View(cart);
                }

                string UserId = GetUserId();

                cartObj.ApplicationUserId = UserId;
                
                ShoppingItem cartFromDb = unitOfWork.ShoppingItem.GetFirstOrDefault(
                    i => i.ApplicationUserId == cartObj.ApplicationUserId &&
                    i.ProductId == cartObj.ProductId, includeProperties: "Product");
                if(cartFromDb == null)
                {
                    //no record exist in db for that product for that user
                    unitOfWork.ShoppingItem.Add(cartObj);
                    unitOfWork.Product.GetById(cartObj.ProductId).stock -= cartObj.Count;
                }
                else
                {
                    cartFromDb.Count += cartObj.Count;
                    cartFromDb.Product.stock -= cartObj.Count;
                    unitOfWork.ShoppingItem.Update(cartFromDb);
                }
                
                unitOfWork.ShoppingItem.Update(cartFromDb);
                unitOfWork.Save();

                var count = unitOfWork.ShoppingItem.
                    GetAll(i => i.ApplicationUserId == cartObj.ApplicationUserId).
                    ToList().Count();
                HttpContext.Session.SetInt32(StaticDetails.ssShoppingItem, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = unitOfWork.Product
                    .GetFirstOrDefault(i => i.Id == cartObj.ProductId,
                    includeProperties: "Category,CoverType");
                ShoppingItem cart = new ShoppingItem()
                {
                    Product = product,
                    ProductId = product.Id
                };
                return View(cart);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public string GetUserId()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) return "";
            return claim.Value;
        }

        #region API CALLS

        [HttpPost]
        [Authorize]
        public IActionResult Notify([FromBody] string id)
        {
            var ProductOutOfStock = new ProductsInStock();

            string UserId = GetUserId();

            ProductOutOfStock.ApplicationUserId = Int32.Parse(UserId);
            ProductOutOfStock.ProductId = Int32.Parse(id);
            var objInDb = unitOfWork.ProductsInStock
                .GetFirstOrDefault(i => i.ProductId == Int32.Parse(id) &&
                                        i.ApplicationUserId == ProductOutOfStock.ApplicationUserId);
            if (objInDb == null)
            {
                unitOfWork.ProductsInStock.Add(ProductOutOfStock);
                unitOfWork.Save();
            }
            return Json(new { success = true, message = "Operation Successful." });
        }
        #endregion
    }
}
