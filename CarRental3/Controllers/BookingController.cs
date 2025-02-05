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
    //[Route("Admin/[controller]/[action]")]
    //[Authorize(Roles = "Admin")]
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
            var booking = bookingRepository.GetById(id); // Hämtar en Booking-modell
            if (booking == null)
            {
                return NotFound();
            }

            // Mappa från Booking till BookingDetailsViewModel
            var viewModel = new BookingDetailsViewModel
            {
                Booking = booking,
                User = booking.User,
                Car = booking.Car
                // Lägg till alla andra nödvändiga egenskaper från Booking till BookingDetailsViewModel
            };

            return View(viewModel); // Skicka rätt ViewModel till vyn
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
                    Text = u.UserName
                }).ToList()
            };

            return View(bookingVM);
        }



        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBooking(BookingViewModel bookingVM)
        {
            // Endast inloggade användare kan boka bilar
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            // Hämta vald bil och användare från repository baserat på ID
            var selectedCar = carRepository.GetById(bookingVM.CarId);
                var selectedUser = userRepository.GetById(bookingVM.UserId);

                if (selectedCar == null || selectedUser == null)
                {
                    ModelState.AddModelError("", "Invalid car or user selection.");
                    return View(bookingVM); // Visa formuläret igen om något är fel
                }

                // Skapa en ny bokning med de inskickade värdena
                var newBooking = new Booking
                {
                    CarId = bookingVM.CarId,
                    UserId = bookingVM.UserId,
                    StartDate = bookingVM.StartDate,
                    EndDate = bookingVM.EndDate
                };

                // Lägg till den nya bokningen till databasen
                bookingRepository.Add(newBooking);

                // Omdirigera till admin-dashboard eller en annan vy när bokningen har skapats
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
            var user = userRepository.GetById(booking.UserId) ?? new User { UserName = "Unknown User" };

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
                    Text = u.UserName
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

            // Hämta användaren från användar-ID
            var user = userRepository.GetById(userId.Value);
            if (user == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            // Kontrollera om användaren är admin och omdirigera till rätt dashboard
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
            if (userId == null || booking.UserId != userId)
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

        [HttpPost, ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBookingConfirmed(int bookingId)
        {
            var booking = bookingRepository.GetById(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            // Kontrollera om bokningen har passerat datumet
            if (booking.StartDate < DateTime.Now)
            {
                ModelState.AddModelError("", "You cannot delete past bookings.");
                return View("Error");
            }

            bookingRepository.Delete(booking);

            // Hämta användar-ID från sessionen
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            // Hämta användaren från användar-ID
            var user = userRepository.GetById(userId.Value);
            if (user == null)
            {
                return RedirectToAction("LoginOrRegister", "Auth");
            }

            // Kontrollera om användaren är admin och omdirigera till rätt dashboard
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
