using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDb.Model
{
    public class HotelHeader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }

        public ICollection<HotelPrice> Prices { get; set; }

        public HotelHeader()
        {
            Prices = new List<HotelPrice>();
        }
    }
}
