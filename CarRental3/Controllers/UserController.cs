using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser userRepository;
        private readonly ICar carRepository;
        private readonly IBooking bookingRepository;

        public UserController(IUser userRepository, ICar carRepository, IBooking bookingRepository)
        {
            this.userRepository = userRepository;
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult UserDashBoard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            var bookings = bookingRepository.GetByUserId(userId.Value);

            var model = new UserDashBoardViewModel
            {
                UserId = userId.Value,
                Bookings = bookings,
                Cars = bookingRepository.GetAllCars(), // För att få alla bilar
                NewBooking = new Booking() // Skapa ett nytt Booking-objekt om det behövs
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            // Returnera en vy för att skapa en bil
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = userVM.UserName,
                    Password = userVM.Password,
                    IsAdmin = userVM.IsAdmin,
                };
                userRepository.Add(user);

                return RedirectToAction("Index", "Admin");
            }

            // Om modellens tillstånd inte är giltigt, visa samma vy igen med det inskickade datat
            return View(userVM);
        }

        // Metoder för att hantera användare
        public IActionResult DetailsUser(int id)
        {
            var user = userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult EditUser(int id)
        {
            var user = userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = userRepository.GetById(user.UserId);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Password = user.Password;
                    existingUser.IsAdmin = user.IsAdmin;

                    userRepository.Update(existingUser);
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                return NotFound();
            }
            return View(user);
        }

        public IActionResult DeleteUser(int id)
        {
            var user = userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUserConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserConfirmed(int UserId)
        {
            var user = userRepository.GetById(UserId);
            userRepository.Delete(user);
            return RedirectToAction("AdminDashboard", "Admin");
        }

    }
}
