using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBooking bookingRepository;

        // Context tillagt för VM
        private readonly ApplicationDbContext applicationDbContext;
        public BookingController(IBooking bookingRepository, ApplicationDbContext applicationDbContext)
        {
            this.bookingRepository = bookingRepository;
            //Tillagt för VM
            this.applicationDbContext = applicationDbContext;
        }
        // GET: BookingController
        public IActionResult Index()
        {
            var viewModel = new BookingViewModel
            {
                Cars = applicationDbContext.Cars.ToList(),
                Users = applicationDbContext.Users.ToList(),
                Bookings = bookingRepository.GetAll()
            };
            return View(viewModel);
            // Ändrad till förmån för VM - return View(bookingRepository.GetAll());
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Create
        public IActionResult Create()
        {
            var cars = applicationDbContext.Cars.ToList();
            var users = applicationDbContext.Users.ToList();

            if (cars == null || users == null)
            {
                return NotFound();
            }

            var viewModel = new BookingFormViewModel
            {
                Cars = cars,
                Users = users
            };
            return View(viewModel);
        }
        

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingFormViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.Cars = applicationDbContext.Cars.ToList();
                    viewModel.Users = applicationDbContext.Users.ToList();
                    return View(viewModel);
                }
                var booking = new Booking
                {
                    CarId = viewModel.CarId,
                    UserId = viewModel.UserId,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate
                };
                bookingRepository.Add(booking);
                return RedirectToAction(nameof(Index));
            }

            catch
            {
                return View();
            }
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookingRepository.Update(booking);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(booking);
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Booking booking)
        {
            try
            {   bookingRepository.Delete(booking);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
