namespace eTickets.Models
{
    public class Actor_Movie
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }    
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        //Tab iza hol FK leh ma katabna foreign key mitl in movies
    }
}
