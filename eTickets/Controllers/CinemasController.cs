using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService _service;

        public CinemasController(ICinemasService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allCinemas = await _service.GetAllAsync();
            return View(allCinemas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null) { return View("NotFound"); }
            return View(cinema);
        }
       
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")]Cinema cinema)
        {
            if(!ModelState.IsValid) { return View(cinema); }
            await _service.AddAsync(cinema);
            return RedirectToAction("Index");   

        }
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) { return View("NotFound"); }
            
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
          
            if (!ModelState.IsValid) { return View(cinema); }
            await _service.UpdateAsync(id, cinema);
            return RedirectToAction("Index");   
        }
    }
}
