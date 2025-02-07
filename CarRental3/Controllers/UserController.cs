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
                Cars = bookingRepository.GetAllCars(),
                NewBooking = new Booking() 
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
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
                    Email = userVM.Email,
                    Password = userVM.Password,
                    IsAdmin = userVM.IsAdmin,
                };
                userRepository.Add(user);
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            return View(userVM);
        }

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
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    existingUser.IsAdmin = user.IsAdmin;

                    userRepository.Update(existingUser);
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                return NotFound();
            }
            return View(user);
        }

        public IActionResult DeleteUser(int Userid)
        {
            var user = userRepository.GetById(Userid);
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
