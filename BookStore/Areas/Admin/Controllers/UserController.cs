using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;
using BookStore.Data.DataModels;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_AdminUser)]
    public class UserController : Controller
    {
        public readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        [BindProperty]
        public UserVM userVM { get; set; }

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var user = unitOfWork.ApplicationUser.GetFirstOrDefault(i=>i.Id == id);
            userVM = new UserVM
            {
                Name = user.UserName,
                Role = unitOfWork.ApplicationRole.GetById(user.AppliactionRoleId).Name,
                CompanyList = unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                RoleList = unitOfWork.ApplicationRole.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            var user = unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.UserName == userVM.Name);
            if (userVM.ApplicationRoleId != 0)
            {
                user.AppliactionRoleId = userVM.ApplicationRoleId;
                await userManager.AddToRoleAsync(user, unitOfWork.ApplicationRole.GetById(user.AppliactionRoleId).Name);
            }
            if (userVM.CompanyId != null)
            {
                user.CompanyId = userVM.CompanyId;
            }
            if (userVM.CompanyId != null || userVM.ApplicationRoleId != 0)
            {
                unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = unitOfWork.ApplicationUser
                .GetAll(i => i.Id.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier));
            foreach (var obj in allObj)
            {
                obj.ApplicationRole = unitOfWork.ApplicationRole.GetById(obj.AppliactionRoleId);
            }
            return Json(new { data = allObj });
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id.ToString() == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error with Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful." });
        }
        #endregion
    }
}