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
        // GET: CarController
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
            // Returnera en vy för att skapa en bil
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
                // Mappa CarViewModel till Car-modellen
                var car = new Car
                {
                    Brand = carVM.Brand,
                    Model = carVM.Model,
                    YearModel = carVM.YearModel,
                    CostPerDay = carVM.CostPerDay,
                    ImageUrl = !string.IsNullOrEmpty(carVM.ImageUrl) ? carVM.ImageUrl : "/images/cars/default_car.jpg"
                    // Fyll på fler egenskaper här om det behövs
                };

                // Spara bilen i databasen genom repositoryt
                carRepository.Add(car);

                // Omdirigera till en vy (t.ex. indexsidan för bilar)
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            // Om modellens tillstånd inte är giltigt, visa samma vy igen med det inskickade datat
            return View(carVM);
        }


        // GET: CarController/Details/5
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

            // Mappa Car-modellen till CarViewModel
            var carVM = new CarViewModel
            {
                CarId = car.CarId,
                Brand = car.Brand,
                Model = car.Model,
                YearModel = car.YearModel,
                CostPerDay = car.CostPerDay,
                ImageUrl = car.ImageUrl // Inkludera bild-URL
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
                // Hämta bilen från databasen
                var car = carRepository.GetById(carVM.CarId);
                if (car == null)
                {
                    return NotFound();
                }

                // Uppdatera bilens egenskaper
                car.Brand = carVM.Brand;
                car.Model = carVM.Model;
                car.YearModel = carVM.YearModel;
                car.CostPerDay = carVM.CostPerDay;
                car.ImageUrl = carVM.ImageUrl; // Uppdatera bild-URL

                // Spara ändringarna i databasen genom repositoryt
                carRepository.Update(car);

                // Omdirigera till AdminDashBoard efter att ha redigerat en bil
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            // Om modellens tillstånd inte är giltigt, visa samma vy igen med det inskickade datat
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

            // Kontrollera om bilen har en aktiv eller framtida bokning
            if (carRepository.HasActiveOrFutureBooking(CarId))
            {
                // Lägg till ett felmeddelande om bilen inte kan tas bort
                TempData["ErrorMessage"] = "Bilen kan inte tas bort eftersom den är bokad.";
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            // Om ingen aktiv eller framtida bokning finns, tillåt borttagning
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



        public IActionResult BookCar(int id)
        {
            var car = carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            var viewModel = new BookCarViewModel
            {
                CarId = car.CarId,
                LoginViewModel = new AvailableCarsLoginViewModel { CarId = car.CarId },
                RegisterViewModel = new RegisterViewModel { CarId = car.CarId }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBooking(BookCarViewModel bookCarVM)
        {
            // Logik för att bekräfta bokning
            return RedirectToAction("MyBookings", "User");
        }

        
            
    }
}

