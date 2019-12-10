using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDb.Scraper
{
    public class HotelData
    {
        public double Rate { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public DateTime CheckIn { get; set; }
    }
}
