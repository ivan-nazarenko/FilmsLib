namespace FilmsLib.Models
{
    public class WatchListFilm
    {
        public int WatchListId { get; set; }
        public virtual WatchList WatchList { get; set; }

        public int FilmId { get; set; }
        public virtual Film Film { get; set; }
    }
}