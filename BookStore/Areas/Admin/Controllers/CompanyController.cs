
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using BookStore.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_AdminUser)]
    public class CompanyController : Controller
    {
        public readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if(id == null)
            {
                //create
                return View(company);
            }
            //edit
            company = unitOfWork.Company.GetById(id.GetValueOrDefault());
            if(company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if(ModelState.IsValid)
            {
                if(company.Id == 0)
                {
                    unitOfWork.Company.Add(company);
                }
                else unitOfWork.Company.Update(company);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);

        }

        #region API CALLS

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = unitOfWork.Company.GetById(id);
            if(obj == null)
            {
                return Json(new {success=false, message="Error while deliting"});
            }
            unitOfWork.Company.Remove(obj);
            unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}