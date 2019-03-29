using System;

namespace PageWatcher.Models
{
    public class HomePageViewModel
    {
        public bool TicketsAvaliable { get; set; }
        public string LastUpdateTime { get; set; }
        public Uri Url { get; set; }
    }
}