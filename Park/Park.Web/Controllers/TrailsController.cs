using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Park.Web.Models;
using Park.Web.Models.ViewModel;
using Park.Web.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Park.Web.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _parkRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(INationalParkRepository parkRepository, ITrailRepository trailRepository)
        {
            _parkRepository = parkRepository;
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _parkRepository.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                //this will be true for Insert/Create
                return View(objVM);
            }

            //Flow will come here for update
            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (objVM.Trail == null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _parkRepository.GetAllAsync(SD.NationalParkAPIPath);

                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}
