using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class CarController : Controller
    {
        private readonly ICar carRepository;
        private readonly IBooking bookingRepository;

        public CarController(ICar carRepository, IBooking bookingRepository)
        {
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCar()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCar(CarViewModel carVM)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    Brand = carVM.Brand,
                    Model = carVM.Model,
                    YearModel = carVM.YearModel,
                    CostPerDay = carVM.CostPerDay,
                    ImageUrl = !string.IsNullOrEmpty(carVM.ImageUrl) ? carVM.ImageUrl : "/images/cars/default_car.jpg"
                };
                carRepository.Add(car);
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            return View(carVM);
        }

        public IActionResult DetailsCar(int id)
        {
            var car = carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        public IActionResult EditCar(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var car = carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            var carVM = new CarViewModel
            {
                CarId = car.CarId,
                Brand = car.Brand,
                Model = car.Model,
                YearModel = car.YearModel,
                CostPerDay = car.CostPerDay,
                ImageUrl = car.ImageUrl 
            };
            return View(carVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCar(CarViewModel carVM)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                var car = carRepository.GetById(carVM.CarId);
                if (car == null)
                {
                    return NotFound();
                }

                car.Brand = carVM.Brand;
                car.Model = carVM.Model;
                car.YearModel = carVM.YearModel;
                car.CostPerDay = carVM.CostPerDay;
                car.ImageUrl = carVM.ImageUrl; 

                carRepository.Update(car);
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            return View(carVM);
        }

        [HttpPost, ActionName("DeleteCarConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int CarId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (carRepository.HasActiveOrFutureBooking(CarId))
            {
                TempData["ErrorMessage"] = "Bilen kan inte tas bort eftersom den är bokad.";
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            var car = carRepository.GetById(CarId);
            if (car == null)
            {
                return NotFound();
            }
            carRepository.Delete(car);
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public IActionResult AvailableCars()
        {
            var bookedCarIds = bookingRepository.GetAll()
                .Where(b => b.StartDate <= DateTime.Now && b.EndDate >= DateTime.Now)
                .Select(b => b.CarId)
                .ToList();

            var availableCars = carRepository.GetAll()
                .Where(c => !bookedCarIds.Contains(c.CarId))
                .ToList();

            return View(availableCars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBooking(BookCarViewModel bookCarVM)
        {
            return RedirectToAction("MyBookings", "User");
        }

        
            
    }
}

