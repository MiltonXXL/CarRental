using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRental3.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBooking bookingRepository;
        private readonly ICar carRepository;
        private readonly IUser userRepository;

        public BookingController(IBooking bookingRepository, ICar carRepository, IUser userRepository)
        {
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
            this.userRepository = userRepository;
        }
        public IActionResult DetailsBooking(int id)
        {
            var booking = bookingRepository.GetById(id); 
            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new BookingDetailsViewModel
            {
                Booking = booking,
                User = booking.User,
                Car = booking.Car
            };

            return View(viewModel); 
        }

        public IActionResult CreateBooking(int carId, int userId)
        {
            var car = carRepository.GetById(carId);
            if (car == null)
            {
                return NotFound();
            }

            var user = userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.Session.GetString("UserRole");

            var bookingVM = new BookingViewModel
            {
                CarId = car.CarId,
                UserId = user.UserId,
                Car = car,
                User = user,
                ImageUrl = car.ImageUrl,
                UserRole = userRole,
                Cars = carRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.CarId.ToString(),
                    Text = $"{c.Brand} {c.Model} ({c.YearModel})"
                }).ToList(),
                Users = userRepository.GetAll().Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.Email
                }).ToList()
            };

            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBooking(BookingViewModel bookingVM)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            var selectedCar = carRepository.GetById(bookingVM.CarId);
                var selectedUser = userRepository.GetById(bookingVM.UserId);

                if (selectedCar == null || selectedUser == null)
                {
                    ModelState.AddModelError("", "Invalid car or user selection.");
                    return View(bookingVM); 
                }

                var newBooking = new Booking
                {
                    CarId = bookingVM.CarId,
                    UserId = bookingVM.UserId,
                    StartDate = bookingVM.StartDate,
                    EndDate = bookingVM.EndDate
                };

                bookingRepository.Add(newBooking);

            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == "Admin")
            {
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            else
            {
                return RedirectToAction("UserDashBoard", "User");
            }
        }

        [HttpGet]
        public IActionResult BackToDashboard(string userRole)
        {
            if (userRole == "Admin")
            {
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            return RedirectToAction("UserDashBoard", "User");
        }

        public IActionResult EditBooking(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || booking.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (booking.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot edit past bookings.");
                return View("Error");
            }

            var car = carRepository.GetById(booking.CarId) ?? new Car { Brand = "Unknown Car" };
            var user = userRepository.GetById(booking.UserId) ?? new User { Email = "Unknown User" };

            var bookingVM = new BookingViewModel
            {
                BookingId = booking.BookingId,
                CarId = booking.CarId,
                UserId = booking.UserId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                Car = car,
                User = user,
                Cars = carRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.CarId.ToString(),
                    Text = $"{c.Brand} {c.Model} {c.YearModel}"
                }).ToList(),
                Users = userRepository.GetAll().Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = u.Email
                }).ToList()
            };

            return View(bookingVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBooking(BookingViewModel bookingVM)
        {
            var booking = bookingRepository.GetById(bookingVM.BookingId);
            if (booking == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || booking.UserId != userId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (booking.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot edit past bookings.");
                return View("Error");
            }

            booking.CarId = bookingVM.CarId;
            booking.UserId = bookingVM.UserId;
            booking.StartDate = bookingVM.StartDate;
            booking.EndDate = bookingVM.EndDate;

            bookingRepository.Update(booking);

            var user = userRepository.GetById(userId.Value);
            if (user == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
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

        [HttpGet]
        public IActionResult DeleteBooking(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || (!userRepository.GetById(userId.Value).IsAdmin && booking.UserId != userId))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (booking.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot delete past bookings.");
                return View("Error");
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBookingConfirmed(int bookingId)
        {
            var booking = bookingRepository.GetById(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            if (booking.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot delete past bookings.");
                return View("Error");
            }

            bookingRepository.Delete(booking);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            var user = userRepository.GetById(userId.Value);
            if (user == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
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


    }
}
