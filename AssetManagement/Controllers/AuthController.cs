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

                if(user != null)
                {
                    if(loginRequestDTO.UserName == user.UserName && loginRequestDTO.Password == user.Password)
                    {
                        HttpContext.Session.SetString("User", user.UserName);
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
            HttpContext.Session.SetString("User", "");
            return RedirectToAction("Login");
        }


    }
}
