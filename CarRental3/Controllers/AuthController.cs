using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUser userRepository;

        public AuthController(IUser userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult LoginOrRegister(int carId)
        {
            var model = new LoginOrRegisterViewModel
            {
                CarId = carId
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(int? carId)
        {
            var model = new LoginViewModel
            {
                CarId = carId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userRepository.GetByUserNameAndPassword(model.UserName, model.Password);
                if (user != null)
                {
                    // Spara användarinformationen i sessionen
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserRole", user.IsAdmin ? "Admin" : "User");

                    // Kontrollera om användaren är admin
                    if (user.IsAdmin)
                    {
                        return RedirectToAction("AdminDashBoard", "Admin");
                    }
                    else
                    {
                        // Kontrollera om bil-id finns för bokning
                        if (model.CarId.HasValue)
                        {
                            return RedirectToAction("CreateBooking", "Booking", new { carId = model.CarId, userId = user.UserId });
                        }
                        return RedirectToAction("UserDashBoard", "User");
                    }
                }
                ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(int? carId)
        {
            var model = new RegisterViewModel
            {
                CarId = carId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = userRepository.GetByUserName(model.RegisterUserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "E-postadressen är redan registrerad.");
                    return View(model);
                }

                var newUser = new User
                {
                    UserName = model.RegisterUserName,
                    Password = model.RegisterPassword,
                    IsAdmin = false  // Standardvärde för nya användare
                };

                userRepository.Add(newUser);

                // Spara användarinformationen i sessionen
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                HttpContext.Session.SetString("UserName", newUser.UserName);
                HttpContext.Session.SetString("UserRole", "User");

                // Direkt till bokning efter registrering
                if (model.CarId.HasValue)
                {
                    return RedirectToAction("CreateBooking", "Booking", new { carId = model.CarId, userId = newUser.UserId });
                }
                return RedirectToAction("UserDashBoard", "User");
            }

            var loginOrRegisterModel = new LoginOrRegisterViewModel
            {
                CarId = model.CarId
            };
            return View("LoginOrRegister", loginOrRegisterModel);
        }
    }
}
