using System.Collections.Generic;
using eraasoftTask13.Models;

namespace eraasoftTask13.ViewModels
{
    public class BookingViewModel
    {
        public ShowTime ShowTime { get; set; }
        public List<Seat> AllSeats { get; set; }
        public List<int> BookedSeatIds { get; set; }
    }

    public class BookingPostViewModel
    {
        public int ShowTimeId { get; set; }
        public List<int> SelectedSeatIds { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
