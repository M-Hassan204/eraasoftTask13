using System.Collections.Generic;
using eraasoftTask13.Models;

namespace eraasoftTask13.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalMovies { get; set; }
        public int NowShowingMovies { get; set; }
        public int TotalCinemas { get; set; }
        public int TotalActors { get; set; }
        public int TotalCategories { get; set; }
        public int TotalHalls { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }

        public List<Booking> RecentBookings { get; set; } = new List<Booking>();
        public List<ShowTime> UpcomingShows { get; set; } = new List<ShowTime>();
        public List<Movie> PopularMovies { get; set; } = new List<Movie>();
    }
}
