using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using BookStore.Infra.Messaging;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_AdminUser)]
    public class ProductController : Controller
    {
        public readonly IUnitOfWork unitOfWork;
        public readonly IWebHostEnvironment webHostEnvironment;
        public readonly IMessageService messageService;
        [BindProperty]
        public ProductVM productVM { get; set; }
        public ProductController(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            IMessageService messageService)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
            this.messageService = messageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = unitOfWork.Category.GetAll().Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };

            if (id == null)
            {
                //create
                return View(productVM);
            }
            //edit
            productVM.Product = unitOfWork.Product.GetById(id.GetValueOrDefault());
            if(productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        //this is an edit and we neeed to remove old image
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    //update when they do not change the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = unitOfWork.Product.GetById(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    var productBeforeUpdate = unitOfWork.Product.GetById(productVM.Product.Id);
                    if(productBeforeUpdate.stock == 0 && productVM.Product.stock > 0)
                    {
                        var productsOutOfStock = unitOfWork.ProductsInStock.GetAll(i => i.ProductId == productBeforeUpdate.Id, includeProperties:"ApplicationUser");
                        MessageOptions message = new MessageOptions
                        {
                            subjcet = "Reminder!",
                            message = $"Product {productBeforeUpdate.Title} is awailable."
                        };
                        foreach (var obj in productsOutOfStock)
                        {
                            message.toEamilAddress = obj.ApplicationUser.Email;
                            await messageService.SendEmailAsync(message);
                        }
                        unitOfWork.ProductsInStock.RemoveRange(productsOutOfStock);
                    }
                    unitOfWork.Product.Update(productVM.Product);
                }
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(productVM);

        }

        

        #region API CALLS

        [HttpPost]
        public IActionResult CreateCoverType(string name)
        {
            var obj = unitOfWork.CoverType.GetFirstOrDefault(i => i.Name == name);
            if (obj != null)
            {
                return Json(new
                {
                    success = false
                ,
                    data = new SelectListItem()
                ,
                    message = "There is already a cover type with this name!"
                });
            }
            CoverType covObj = new CoverType
            {
                Name = name
            };

            unitOfWork.CoverType.Add(covObj);
            unitOfWork.Save();
            return Json(new
            {
                success = true
                ,
                data = new SelectListItem { Text = covObj.Name, Value = covObj.Id.ToString() }
                ,
                message = "Category added successfuly"
            });
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            var obj = unitOfWork.Category.GetFirstOrDefault(i => i.Name == name);
            if (obj != null)
            {
                return Json(new
                {success = false
                ,data = new SelectListItem ()
                ,message = "There is already a category with this name!"
                });
            }
            Category catObj = new Category
            {
                Name = name
            };
            
            unitOfWork.Category.Add(catObj);
            unitOfWork.Save();
            return Json(new { success = true
                ,data = new SelectListItem { Text = catObj.Name, Value = catObj.Id.ToString()}
                ,message = "Category added successfuly"});
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = unitOfWork.Product.GetById(id);
            if(obj == null)
            {
                return Json(new {success=false, message="Error while deliting"});
            }

            string webRootPath = webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            unitOfWork.Product.Remove(obj);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        
        #endregion
    }
}