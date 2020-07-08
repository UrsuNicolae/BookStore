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
    public class CoverTypeController : Controller
    {
        public readonly IUnitOfWork unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if(id == null)
            {
                //create
                return View(coverType);
            }
            //edit
            coverType = unitOfWork.CoverType.GetById(id.GetValueOrDefault());
            if(coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if(ModelState.IsValid)
            {
                if(coverType.Id == 0)
                {
                    unitOfWork.CoverType.Add(coverType);
                }
                else unitOfWork.CoverType.Update(coverType);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);

        }

        #region API CALLS

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = unitOfWork.CoverType.GetById(id);
            if(obj == null)
            {
                return Json(new {success=false, message="Error while deliting"});
            }
            unitOfWork.CoverType.Remove(obj);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}