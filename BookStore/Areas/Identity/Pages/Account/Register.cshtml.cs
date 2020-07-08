using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data.DataModels;
using BookStore.Infra.Messaging;
using BookStore.BusinessLogic.Repository.IRepository;
using BookStore.BusinessLogic.ServiceModels;

namespace BookStore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMessageService _messageService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IMessageService messageService,
            RoleManager<ApplicationRole> roleManager,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _messageService = messageService;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string StreetAdress { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string PhoneNumber { get; set; }

            [Required]
            public string PostalCode { get; set; }

            public IEnumerable<SelectListItem> CompanyList { get; set; }

            public IEnumerable<SelectListItem> RoleList { get; set; }
            
            public int? CompanyId { get; set; }

            public int ApplicationRoleId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            Input = new InputModel()
            {
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                RoleList = _unitOfWork.ApplicationRole.GetAll().Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    CompanyId = Input.CompanyId,
                    StreetAdress = Input.StreetAdress,
                    City = Input.City,
                    PostalCode = Input.PostalCode,
                    PhoneNumber = Input.PhoneNumber,
                    AppliactionRoleId = Input.ApplicationRoleId,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if(!await _roleManager.RoleExistsAsync(StaticDetails.Role_AdminUser))
                    {
                        await _roleManager.CreateAsync(new ApplicationRole(StaticDetails.Role_AdminUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_CompUser))
                    {
                        await _roleManager.CreateAsync(new ApplicationRole(StaticDetails.Role_CompUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_IndiUser))
                    {
                        await _roleManager.CreateAsync(new ApplicationRole(StaticDetails.Role_IndiUser));
                    }

                    if (user.AppliactionRoleId == 0)
                    {
                        //A new use has registered
                        user.AppliactionRoleId = _unitOfWork.ApplicationRole.GetFirstOrDefault(i => i.Name == StaticDetails.Role_IndiUser).Id;
                        await _userManager.AddToRoleAsync(user, StaticDetails.Role_IndiUser);
                    }
                    else
                    {
                        //Admin registered a new user
                        await _userManager.AddToRoleAsync(user, _unitOfWork.ApplicationRole.GetById(user.AppliactionRoleId).Name);
                    }

                    /*user.AppliactionRoleId = _unitOfWork.ApplicationRole.GetFirstOrDefault(i => i.Name == StaticDetails.Role_AdminUser).Id;
                    await _userManager.AddToRoleAsync(user, StaticDetails.Role_AdminUser);*/

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    MessageOptions message = new MessageOptions
                    {
                        toEamilAddress = user.Email,
                        subjcet = "Confirm your email",
                        message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                    };

                    await _messageService.SendEmailAsync(message);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (!_signInManager.IsSignedIn(User))
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            //admin is registring a new user
                            return RedirectToAction("Index", "User", new {Area = "Admin" });
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            Input = new InputModel()
            {
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                RoleList = _unitOfWork.ApplicationRole.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
