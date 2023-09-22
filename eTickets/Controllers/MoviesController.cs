using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n=> n.Cinema);
            return View(allMovies);
        }
        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n=> n.Cinema);
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allMovies.Where(n => n.Name.Contains(searchString) || n.Description.Contains(searchString)).ToList();
                return View("Index",filteredResult);
            }
            return View("Index",allMovies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            return View(movieDetails);
        }
        public async Task<IActionResult> Create()
        {
            var movieDropdown = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas,"Id","Name");
            ViewBag.Producers = new SelectList(movieDropdown.Producers,"Id","FullName");
            ViewBag.Actors = new SelectList(movieDropdown.Actors,"Id","FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if (!ModelState.IsValid)
            {

                var movieDropdown = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdown.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdown.Actors, "Id", "FullName");
                return View(movie);
            }
            await _service.AddNewMovieAsync(movie);
            return RedirectToAction("Index");
        } 
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                ImageURL = movieDetails.ImageURL,
                Price = movieDetails.Price,
                CinemaId = movieDetails.CinemaId,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                MovieCategory = movieDetails.MovieCategory,
                ProducerId = movieDetails.ProducerId,
                                
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList(),
            };

            var movieDropdown = await _service.GetNewMovieDropdownsValues();
            ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas,"Id","Name");
            ViewBag.Producers = new SelectList(movieDropdown.Producers,"Id","FullName");
            ViewBag.Actors = new SelectList(movieDropdown.Actors,"Id","FullName");
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {

                var movieDropdown = await _service.GetNewMovieDropdownsValues();
                ViewBag.Cinemas = new SelectList(movieDropdown.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdown.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdown.Actors, "Id", "FullName");
                return View(movie);
            }
            await _service.UpdateMovieAsync(movie);
            return RedirectToAction("Index");
        }
    }
}
