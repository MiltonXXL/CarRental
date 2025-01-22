using CarRental3.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace CarRental3.ViewModels
{
    public class AdminViewModel
    {
        public Administrator Admin { get; set; }
        public CurrentUser CurrentUser { get; set; }
    }
}
