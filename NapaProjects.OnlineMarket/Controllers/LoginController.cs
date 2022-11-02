
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace NapaProjects.OnlineMarket.Controllers
{
    public class LoginController : Controller
    {
        private UserManager<AppUser> userManager;

        private RoleManager<IdentityRole<int>> roleManager;

        private SignInManager<AppUser> signInManager;

        private UserValidator<AppUser> userValidator;

        private PasswordValidator<AppUser> passwordValidator;

        public LoginController(UserManager<AppUser> userManager,
                    RoleManager<IdentityRole<int>> roleManager, SignInManager<AppUser> signInManager,
                    UserValidator<AppUser> userValidator, PasswordValidator<AppUser> passwordValidator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.userValidator = userValidator;
            this.passwordValidator = passwordValidator;
        }

        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new UserModel());
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(UserModel user, string password, string returnUrl)
        {
            AppUser newUser = new AppUser
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            var result = await passwordValidator.ValidateAsync(userManager, newUser, password);
            if (!result.Succeeded)
            {
                foreach (var er in result.Errors)
                {
                    ModelState.AddModelError(er.Code, er.Description);
                }
            }
            result = await userValidator.ValidateAsync(userManager, newUser);
            if (!result.Succeeded)
            {
                foreach (var er in result.Errors)
                {
                    ModelState.AddModelError(er.Code, er.Description);
                }
            }
            
            result = await userManager.CreateAsync(newUser,password);
            if (!result.Succeeded)
            {
                foreach (var er in result.Errors)
                {
                    ModelState.AddModelError(er.Code, er.Description);
                }
            }
            if (ModelState.IsValid)
            {
                await userManager.AddToRoleAsync(newUser, AppRoles.UserRole.Name);
                await signInManager.PasswordSignInAsync(newUser, password, false, false);
                return Redirect(returnUrl);
            }
                ViewBag.ReturnUrl = returnUrl;
                return View(user);
            
        }

        [Authorize]
        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInModel());
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInModel loginModel, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var user = await userManager.FindByNameAsync(loginModel.UserName);
            if(user == null)
            {
                ModelState.AddModelError("UserName", $"'{loginModel.UserName}' user name is not found");
                return View(new {userName = loginModel.UserName, password = loginModel.password});
            }
            await signInManager.SignOutAsync();
            var signInResult = await signInManager.PasswordSignInAsync(user, loginModel.password, false, false);
            if (signInResult.Succeeded)
            {

                return Redirect(returnUrl??"/");
            }
            ModelState.AddModelError("password", "This password is wrong");
            return View(loginModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SignOut(string returnUrl)
        {
            await signInManager.SignOutAsync();
            Console.WriteLine("Directed to " + returnUrl);
            return Redirect(returnUrl);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new UserModel());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            List<UserModel> Users = userManager.Users.ToList()
        .Select(x =>
        {
            var y = (UserModel)x;
            roleManager.Roles.ToList().ForEach(async role =>
            {
                if (await userManager.IsInRoleAsync(x, role.Name))
                {
                    y.Roles.Add(role.Name);
                }
            });
            return y;
        }).ToList();
            return View(Users);
        }
                                     
                                     //=> View(userManager.Users.Select(u => (UserModel)u).ToList());


        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel newUser, string password, string returnUrl)
        {
            AppUser user = new AppUser()
            {
                UserName = newUser.UserName,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber
            };

            

            var userValidate = await userValidator.ValidateAsync(userManager, user);

            if (!userValidate.Succeeded)
            {
                userValidate.Errors.ToList().ForEach(er => ModelState.AddModelError(er.Code, er.Description));
            }

            var passwordValidate = await passwordValidator.ValidateAsync(userManager, user, password);

            if (!passwordValidate.Succeeded)
            {
                passwordValidate.Errors.ToList().ForEach(er => ModelState.AddModelError(er.Code, er.Description));
            }
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return View(newUser);
                }
                await userManager.AddToRoleAsync(user, AppRoles.UserRole.Name);
                return Redirect(returnUrl);
            }
            return View(user);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userName, string returnUrl)
        {
            var result = await userManager.DeleteAsync(await userManager.FindByNameAsync(userName));
            if (!result.Succeeded)
            {
                foreach (var er in result.Errors)
                {
                    ModelState.AddModelError(er.Code, er.Description);
                }
            }
            return Redirect(returnUrl);

        }
    }
}
