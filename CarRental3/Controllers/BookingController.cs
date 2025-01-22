using CarRental3.Data;
using CarRental3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBooking bookingRepository;

        public BookingController(IBooking bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }
        // GET: BookingController
        public ActionResult Index()
        {
            return View(bookingRepository.GetAll());
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bookingRepository.Add(booking);
                }
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
            return View();
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
