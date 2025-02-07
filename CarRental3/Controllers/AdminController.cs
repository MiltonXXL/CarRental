using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUser userRepository;
        private readonly ICar carRepository;
        private readonly IBooking bookingRepository;

        public AdminController(IUser userRepository, ICar carRepository, IBooking bookingRepository)
        {
            this.userRepository = userRepository;
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
        }

        public IActionResult AdminDashBoard()
        {
            var users = userRepository.GetAll();
            var cars = carRepository.GetAll();
            var bookings = bookingRepository.GetAll();

            var model = new AdminDashBoardViewModel
            {
                Users = users,
                Cars = cars,
                Bookings = bookings
            };

            return View(model);
        }


    }
}
