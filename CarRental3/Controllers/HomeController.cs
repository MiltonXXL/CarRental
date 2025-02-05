using System.Diagnostics;
using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUser userRepository;
        private readonly ICar carRepository;
        private readonly IBooking bookingRepository;

        public HomeController(ILogger<HomeController> logger, IUser userRepository, ICar carRepository, IBooking bookingRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
        }

        public IActionResult Index()
        {
             return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult AvailableCars()
        {
            var bookedCarIds = bookingRepository.GetAll().Select(b => b.CarId).ToList();
            var availableCars = carRepository.GetAll().Where(c => !bookedCarIds.Contains(c.CarId)).ToList();

            var viewModel = new AvailableCarsViewModel
            {
                AvailableCars = availableCars.Select(c => new CarViewModel
                {
                    CarId = c.CarId,
                    Brand = c.Brand,
                    Model = c.Model,
                    CostPerDay = c.CostPerDay
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AvailableCarsLogin([Bind("UserName", "Password")]AvailableCarsLoginViewModel loginVM, string returnUrl = null)
        {
         
            var user = userRepository.GetByUserNameAndPassword(loginVM.UserName, loginVM.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.IsAdmin ? "Admin" : "User");

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (user.IsAdmin)
                {
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                else
                {
                    return RedirectToAction("UserDashBoard", "User");
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            var loginViewModelInvalid = new AvailableCarsLoginViewModel
            {
                UserName = loginVM.UserName,
                Password = loginVM.Password,
                CarId = loginVM.CarId
            };
            return View("BookCar", new BookCarViewModel { LoginViewModel = loginViewModelInvalid, RegisterViewModel = new RegisterViewModel(), CarId = loginVM.CarId });
        }

        public IActionResult BookCar(int id)
        {
            var viewModel = new BookCarViewModel
            {
                CarId = id,
                LoginViewModel = new AvailableCarsLoginViewModel { CarId = id },
                RegisterViewModel = new RegisterViewModel { CarId = id }
            };

            return View(viewModel); // Använd BookCar-vyn
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookCar(BookCarViewModel viewModel)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Om användaren inte är inloggad, omdirigera till inloggningssidan
                return RedirectToAction("AvailableCarsLogin", new { returnUrl = Url.Action("BookCar", new { id = viewModel.CarId }) });
            }

            // Skapa bokning
            var booking = new Booking
            {
                CarId = viewModel.CarId,
                UserId = userId.Value,
                // Lägg till mer data om det behövs
            };

            bookingRepository.Add(booking);

            return RedirectToAction("BookingConfirmation", new { bookingId = booking.CarId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HomeLogin(HomeLoginViewModel loginVM, string returnUrl = null)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Login", loginVM);
            //}

            var user = userRepository.GetByUserNameAndPassword(loginVM.UserName, loginVM.Password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserRole", user.IsAdmin ? "Admin" : "User");

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                if (user.IsAdmin)
                {
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                else
                {
                    return RedirectToAction("UserDashBoard", "User");
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View("Login", loginVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("BookCar", new BookCarViewModel { LoginViewModel = new AvailableCarsLoginViewModel(), RegisterViewModel = registerVM, CarId = registerVM.CarId });
            }

            var user = new User
            {
                UserName = registerVM.UserName,
                Password = registerVM.Password,
                IsAdmin = false
            };
            userRepository.Add(user);
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserRole", user.IsAdmin ? "Admin" : "User");

            return RedirectToAction("CreateBooking", "User", new { carId = registerVM.CarId });
        }


    }
}
