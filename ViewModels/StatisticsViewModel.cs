using System.Collections.Generic;

namespace eraasoftTask13.ViewModels
{
    public class StatisticsViewModel
    {
        public int TotalMovies { get; set; }
        public int NowShowingMovies { get; set; }
        public int ComingSoonMovies { get; set; }
        public int EndedMovies { get; set; }

        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int PendingBookings { get; set; }
        public int CancelledBookings { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisMonth { get; set; }

        public Dictionary<string, int> MoviesPerCinema { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> BookingsPerCinema { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> RevenuePerCinema { get; set; } = new Dictionary<string, decimal>();

        public Dictionary<string, int> MoviesPerCategory { get; set; } = new Dictionary<string, int>();
    }
}
