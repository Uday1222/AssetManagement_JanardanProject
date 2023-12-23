using AssetManagement.Models;
using AssetManagement.Repository.IRepository;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using AssetManagement.Models.Dto;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;

namespace AssetManagement.Controllers
{
    public class AuthController : Controller
    {
        private IRepository<User> _userRepo;

        public AuthController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginRequestDTO)
        {
            if (loginRequestDTO != null)
            {

                var user = await _userRepo.Get(x => x.UserName == loginRequestDTO.UserName);

                if (user != null)
                {
                    if (loginRequestDTO.UserName == user.UserName && loginRequestDTO.Password == user.Password)
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);
                        HttpContext.Session.SetString("User", user.Role);
                    }

                    return RedirectToAction("Index", "Home");
                }
                //var response = await _authService.LoginAsync<APIResponse>(loginRequestDTO);

                //if (response != null && response.IsSuccess)
                //{
                //    LoginResponseDTO login = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Result.ToString());

                //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //    identity.AddClaim(new Claim(ClaimTypes.Name, login.User.Name));
                //    identity.AddClaim(new Claim(ClaimTypes.Role, login.User.Role));
                //    var principal = new ClaimsPrincipal(identity);

                //    await HttpContext.SignInAsync(principal);

                //    HttpContext.Session.SetString(SD.SessionToken, login.Token);
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                //}
            }
            return View();
        }
        public IActionResult Register()
        {
            //RegisterationRequestDTO obj = new();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (user != null && (user.Role != "Admin" && user.Role != "Employee"))
                {
                    TempData["error"] = "Allowed Roles: Admin or Employee";
                    return View();
                }

                await _userRepo.CreateAsync(user);
                TempData["success"] = "Registered Successfully";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("User", "");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
