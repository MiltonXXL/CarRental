using CarRental3.Data;
using CarRental3.Models;
using CarRental3.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental3.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser userRepository;

        public AccountController(IUser userRepository)
        {
            this.userRepository = userRepository;
        }
        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

    }
}


       