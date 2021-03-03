using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Park.Web.Models;
using Park.Web.Models.ViewModel;
using Park.Web.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Park.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _parkRepository;
        private readonly ITrailRepository _trailRepository;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository parkRepository, ITrailRepository trailRepository)
        {
            _logger = logger;
            _parkRepository = parkRepository;
            _trailRepository = trailRepository;
        }
        public async Task<IActionResult> Index()
        {
            IndexVM listOfParksAndTrails = new IndexVM()
            {
                NationalParkList = await _parkRepository.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath),
            };
            return View(listOfParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
