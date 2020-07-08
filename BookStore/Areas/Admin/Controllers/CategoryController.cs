using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_AdminUser)]
    public class CategoryController : Controller
    {
        public readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id == null)
            {
                //create
                return View(category);
            }
            //edit
            category = unitOfWork.Category.GetById(id.GetValueOrDefault());
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if(ModelState.IsValid)
            {
                if(category.Id == 0)
                {
                    unitOfWork.Category.Add(category);
                }
                else unitOfWork.Category.Update(category);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);

        }

        #region API CALLS

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = unitOfWork.Category.GetById(id);
            if(obj == null)
            {
                return Json(new {success=false, message="Error while deliting"});
            }
            unitOfWork.Category.Remove(obj);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}