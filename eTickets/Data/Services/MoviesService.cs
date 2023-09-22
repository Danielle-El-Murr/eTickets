using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace eTickets.Data.Services
{
    public class MoviesService : EntityBaseRepository<Movie>,IMoviesService
    {
        private readonly AppDbContext _context;
        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(NewMovieVM movie)
        {
            var newMovie = new Movie()
            {
                Name = movie.Name,
                Description = movie.Description,
                ImageURL = movie.ImageURL,
                Price = movie.Price,
                CinemaId = movie.CinemaId,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                MovieCategory = movie.MovieCategory,
                ProducerId = movie.ProducerId,
            };
            await _context.Movies.AddAsync(newMovie);
            await _context.SaveChangesAsync();
            foreach (var actorId in movie.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId,
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
           var movieDetails = await _context.Movies
                .Include(c=>c.Cinema)
                .Include(p=>p.Producer)
                .Include(am=>am.Actors_Movies).ThenInclude(a=>a.Actor)
                .FirstOrDefaultAsync(n=>n.Id==id);
            return movieDetails;

        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            var response = new NewMovieDropdownsVM()
            {
                Actors = await _context.Actors.OrderBy(a => a.FullName).ToListAsync(),
                Cinemas = await _context.Cinemas.OrderBy(a => a.Name).ToListAsync(),
                Producers = await _context.Producers.OrderBy(a => a.FullName).ToListAsync(),
            };
           
            return response;
        }

        public async Task UpdateMovieAsync(NewMovieVM movie)
        {
            var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.Id==movie.Id);

            if (dbMovie != null)
            {
               
                    dbMovie.Name = movie.Name;
                    dbMovie.Description = movie.Description;
                    dbMovie.ImageURL = movie.ImageURL;
                    dbMovie.CinemaId = movie.CinemaId;
                    dbMovie.Price = movie.Price;
                    dbMovie.StartDate = movie.StartDate;
                    dbMovie.EndDate = movie.EndDate;
                    dbMovie.MovieCategory = movie.MovieCategory;
                    dbMovie.ProducerId = movie.ProducerId;
                
            }
            var existingActorsDb =  _context.Actors_Movies.Where(n => n.MovieId == movie.Id).ToList();

             _context.Actors_Movies.RemoveRange(existingActorsDb);
            await _context.SaveChangesAsync();
            foreach (var actorId in movie.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = movie.Id,
                    ActorId = actorId,
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
            }
            await _context.SaveChangesAsync();
        }
    }
}
