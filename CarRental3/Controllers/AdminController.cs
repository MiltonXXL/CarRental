using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdministrator administratorRepository;

        public AdminController(IAdministrator administratorRepository)
        {
            this.administratorRepository = administratorRepository;
        }
        // GET: AdminController
        public IActionResult Index()
        {
            return View(administratorRepository.GetAll());
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View(administratorRepository.GetById(id));
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Administrator admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    administratorRepository.Add(admin);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {

            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Administrator admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    administratorRepository.Update(admin);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(admin);
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(administratorRepository.GetById(id));
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Administrator admin)
        {
            try
            {
                administratorRepository.Delete(admin);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
